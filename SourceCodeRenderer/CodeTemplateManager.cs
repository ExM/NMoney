namespace NMoney.SourceCodeRenderer
{
	public static class CodeTemplateManager
	{
		public static string Read(string name)
		{
			var resourceName = $"NMoney.SourceCodeRenderer.CodeTemplates.{name}.tt";
			using var stream = typeof(CodeTemplateManager).Assembly.GetManifestResourceStream(resourceName);
			if (stream == null)
				throw new Exception($"resource {resourceName} not found");
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
	}
}
