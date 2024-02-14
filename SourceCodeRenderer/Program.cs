using McMaster.Extensions.CommandLineUtils;

namespace NMoney.SourceCodeRenderer;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandLineApplication();

        app.HelpOption();

        var inputPath = app
            .Option<string>("-i|--input <path>", "Set path to data files", CommandOptionType.SingleValue)
            .IsRequired();

        var outputPath = app
            .Option<string>("-o|--output <path>", "Set path to generated source files", CommandOptionType.SingleValue)
            .IsRequired();

        var update = app.Option<bool>("-u|--update", "Upload last specification of ISO 4217", CommandOptionType.NoValue);

        app.OnExecute(() => Execute(inputPath.ParsedValue, outputPath.ParsedValue, update.ParsedValue));

        return app.Execute(args);
    }

    public static void Execute(string inputPath, string outputPath, bool updateSpecification)
    {
        if (!Path.IsPathRooted(inputPath))
            inputPath = Path.Combine(Environment.CurrentDirectory, inputPath);

        if (!Path.IsPathRooted(outputPath))
            outputPath = Path.Combine(Environment.CurrentDirectory, outputPath);

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        var specs = new Specifications(inputPath);
        if(updateSpecification)
            specs.Update();

        var actualCurrencies = specs.LoadActualList();
        var obsoleteCurrencies = specs.LoadObsoleteList();

        var currencySymbols = new CurrencySymbols(Path.Combine(inputPath, "unicodeSymbols.csv"));
        var nameOverride = new NameOverride(Path.Combine(inputPath, "nameOverride.csv"));


        var templates = new[]
        {
            new Template("CurrencySet.List", CodeTemplateManager.Read("CurrencySet.List"), "cs"),
            new Template("CurrencySet.ObsoleteList", CodeTemplateManager.Read("CurrencySet.ObsoleteList"), "cs"),
            new Template("Names", CodeTemplateManager.Read("Names"), "resx"),
            new Template("Names.Obsolete", CodeTemplateManager.Read("Names.Obsolete"), "resx"),
        };
        
        var assembly = Template.Emit(templates);

        foreach (var template in templates)
        {
            var fileName = Path.Combine(outputPath, template.FileName);
            var content = template.Render(assembly, ctx =>
            {
                ctx.ActualCurrencies = actualCurrencies;
                ctx.ObsoleteCurrencies = obsoleteCurrencies;
                ctx.CurrencySymbols = currencySymbols;
                ctx.NameOverride = nameOverride;
            });
            File.WriteAllText(fileName, content);
        }
    }

}