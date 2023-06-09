namespace Grammar.Analysis;

public class FirstCalc
{
    public static Dictionary<string, List<string>> getFirst(ProductionRule rules)
    {
        var first = new Dictionary<string, List<string>>();
        foreach (var rule in rules.Productions)
        {
            if (!first.ContainsKey(rule.Key))
            {
                first.Add(rule.Key, new List<string>());
                getFirst(rule.Key, rules, first);
            }
        }

        foreach (var set in first)
        {
            first[set.Key] = set.Value.Distinct().OrderBy(x => x).ToList();
        }
        return first;
    }

    private static void getFirst(string nonTerminal, ProductionRule rules, Dictionary<string, List<string>> dict)
    {
        foreach (var production in rules.Productions[nonTerminal])
        {
            var element = production[0];
            if (rules.Productions.ContainsKey(element))
            {
                if (!dict.ContainsKey(element))
                {
                    dict.Add(element, new List<string>());
                    getFirst(element, rules, dict);
                }
                dict[nonTerminal].AddRange(dict[element]);
            }
            else
            {
                dict[nonTerminal].Add(element);
            }
        }
    }



}

