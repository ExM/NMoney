using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mono.TextTemplating;

namespace NMoney.SourceCodeRenderer
{
	public class Template
	{
		private static readonly string _preprocessTemplatesNamespace = "NMoney.SourceCodeRenderer.PreprocessTemplates";

		private readonly string _name;
		private readonly string _content;
		private readonly string _extension;
		private readonly string _preprocessTemplateClass;

		public Template(string name, string content, string extension)
		{
			_name = name;
			_content = content;
			_extension = extension;
			_preprocessTemplateClass = _name.Replace(".", "_") + "_Preprocess";
		}

		public SyntaxTree Preprocess(TemplateGenerator generator, HashSet<string> allReferences)
		{
			var preprocessResult = generator.PreprocessTemplate(
				_name + ".tt",
				_preprocessTemplateClass,
				_preprocessTemplatesNamespace,
				_content,
				out _,
				out var references,
				out var templateSourceCode);

			//UNDONE show warnings
			if (!preprocessResult)
				throw new AggregateException(generator.Errors.Cast<CompilerError>().Select(x => new Exception(x.ToString())));

			//UNDONE trace template code
			//Console.WriteLine(templateSourceCode);

			foreach (var reference in references)
				allReferences.Add(reference);

			return CSharpSyntaxTree.ParseText(
				templateSourceCode,
				CSharpParseOptions.Default,
				_name + ".cs",
				Encoding.UTF8);
		}

		public string FileName => _name + "." + _extension;

		public string Render(Assembly assembly, Action<RenderContext> configureContext)
		{
			var typeName = _preprocessTemplatesNamespace + "." + _preprocessTemplateClass;
			var t = assembly.GetType(typeName);
			if (t == null)
				throw new Exception($"type {typeName} not found");

			var mi = t.GetMethod("TransformText")!;

			var inst = Activator.CreateInstance(t)!;

			if (inst is RenderContext context)
				configureContext(context);

			var result = (string) mi.Invoke(inst, new object?[0])!;

			return result;
		}

		public static Assembly Emit(IEnumerable<Template> templates)
		{
			return Emit(new TemplateGenerator(), templates);
		}

		public static Assembly Emit(TemplateGenerator generator, IEnumerable<Template> templates)
		{
			var references = new HashSet<string>(StringComparer.Ordinal);
			var syntaxTrees = templates
				.Select(template => template.Preprocess(generator, references))
				.ToList();

			references.Add(typeof(RenderContext).Assembly.Location);
			references.Add(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "System.Runtime").Location);

			var compilation = CSharpCompilation.Create(
				"NMoney.SourceCodeRenderer.PreprocessTemplating",
				syntaxTrees,
				references.Select(x => MetadataReference.CreateFromFile(x)),
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			using var dllStream = new MemoryStream();
			using var pdbStream = new MemoryStream();

			var emitResult = compilation.Emit(dllStream, pdbStream);
			if (!emitResult.Success)
				throw new AggregateException(emitResult.Diagnostics.Select(x => new Exception(x.ToString())));

			return Assembly.Load(dllStream.ToArray(), pdbStream.ToArray());
		}
	}
}
