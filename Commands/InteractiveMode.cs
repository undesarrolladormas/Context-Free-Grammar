using Grammar.Analysis;
using Spectre.Console;

namespace Grammar.Commands;

public class InteractiveMode
{

    public static void Interactive()
    {
        var rule = new Rule("[violet]Interactive mode[/]").LeftJustified();
        AnsiConsole.Write(rule);
        AnsiConsole.Markup("Type your grammar here (press [green]Enter[/] in a new line to finish):\n");
        AnsiConsole.Markup("[orange3]Note:[/] [green]Special Characters[/]: Episilon = [green]\u03F5[/]\n");
        List<string> lines = new();
        do {
            var line = AnsiConsole.Prompt(
                new TextPrompt<string>("[orange3]> [/]")
                    .AllowEmpty()
            );
            if (line is null  || line == "")
            {
                break;
            }
            lines.Add(line);
        } while (true);
        ProductionRule rules = new();
        try {
            rules = SeparateElements.SeparateNonTerminals(lines);
        }
        catch (Exception e)
        {
            AnsiConsole.Markup($"[red]{e.Message}[/]\n");
            return;
        }
        finally {
            Console.WriteLine(rules);
        }
        
    }
}