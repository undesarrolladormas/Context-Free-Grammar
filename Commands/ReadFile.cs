using Grammar.Analysis;
using Grammar.Analysis.Sets;
using Spectre.Console;

namespace Grammar.Commands;

public class ReadFile
{
    public static void Read(string path)
    {
        var rule = new Rule($"[violet]Reading file[/] [gray]{path}[/]").LeftJustified();
        AnsiConsole.Write(rule);
        var lines = System.IO.File.ReadAllLines(path).ToList();

        ProductionRule rules = new();
        try
        {
            rules = SeparateElements.SeparateNonTerminals(lines);
        }
        catch (Exception e)
        {
            AnsiConsole.Markup($"[red]{e.Message}[/]\n");
            return;
        }

        AnsiConsole.Markup("\n[green]Grammar[/]\n");
        AnsiConsole.WriteLine(rules.ToString());

        var norecur = FixGrammar.RemoveLeftRecursion(rules);
        AnsiConsole.Markup("\n[green]Left Recursion Removed Grammar[/]\n");
        AnsiConsole.WriteLine(norecur.ToString());

        var nofact = FixGrammar.LeftFactoring(norecur);
        AnsiConsole.Markup("\n[green]Left Factored Grammar[/]\n");
        AnsiConsole.WriteLine(nofact.ToString());

        var first = FirstCalc.getFirst(nofact);
        AnsiConsole.Markup("\n[green]Firsts Sets[/]\n");
        foreach (var item in first)
        {
            AnsiConsole.WriteLine($"{item.Key} = {string.Join(", ", item.Value)}");
        }

        var next = NextCalc.getNext(nofact, first);
        AnsiConsole.Markup("\n[green]Follow Sets[/]\n");
        foreach (var item in next)
        {
            AnsiConsole.WriteLine($"{item.Key} = {string.Join(", ", item.Value)}");
        }
    }
}