using Grammar.Analysis;
using Grammar.Analysis.Sets;
using Spectre.Console;

namespace Grammar.Commands;

public class InteractiveMode
{

    public static void Interactive()
    {
        var rule = new Rule("[violet]Interactive mode[/]").LeftJustified();
        AnsiConsole.Write(rule);
        AnsiConsole.Markup("Type your grammar here (press [green]Enter[/] in a new line to finish):\n");
        AnsiConsole.Markup("[orange3]Note:[/] [green]Special Characters[/]: Episilon = [green]ε[/]\n");
        List<string> lines = new();
        do {
            var line = AnsiConsole.Prompt(
                new TextPrompt<string>("[cyan]> [/]")
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
            var norecur = FixGrammar.RemoveLeftRecursion(rules);
            AnsiConsole.Markup("\n[green]Left Recursion Removed Grammar[/]\n");
            AnsiConsole.WriteLine(norecur.ToString());

            //var nofact = FixGrammar.LeftFactoring(rules);
            //AnsiConsole.Markup("\n[green] Left Factored Grammar[/]\n");
            //AnsiConsole.WriteLine(nofact.ToString());

            var first = FirstCalc.getFirst(norecur);
            AnsiConsole.Markup("\n[green]Firsts Sets[/]\n");
            foreach (var item in first)
            {
                AnsiConsole.WriteLine($"{item.Key} = {string.Join(", ", item.Value)}");
            }

            var next = NextCalc.getNext(norecur, first);
            AnsiConsole.Markup("\n[green]Follow Sets[/]\n");
            foreach (var item in next)
            {
                AnsiConsole.WriteLine($"{item.Key} = {string.Join(", ", item.Value)}");
            }

        }
        
    }
}