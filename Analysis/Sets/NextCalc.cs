using System.Linq;

namespace Grammar.Analysis.Sets;

public class NextCalc
{
    public static Dictionary<string, List<string>> getNext(ProductionRule rules, Dictionary<string, List<string>> first)
    {
        var next = new Dictionary<string, List<string>>();
        foreach (var rule in rules.Productions)
        {
            if (!next.ContainsKey(rule.Key))
            {
                next.Add(rule.Key, new List<string>());
                if (rule.Key == rules.Productions.First().Key)
                {
                    next[rule.Key].Add("$");
                }
                getNext(rule.Key, rules, first, next);
            }
        }
        getNext(rules, first, next);
        foreach (var set in next)
        {
            next[set.Key] = set.Value.Distinct().OrderBy(x => x).ToList();
            next[set.Key].Remove("ε");
        }
        return next;
    }

    private static void getNext(string nonTerminal, ProductionRule rules, Dictionary<string, List<string>> first, Dictionary<string, List<string>> next)
    {
        foreach (var list in rules.Productions)
        {
            var key = list.Key;
            var elements = list.Value;
            foreach (string[] values in elements)
            {
                for (int i = 0; i < values.Length - 1; i++)
                {
                    if (values[i] == nonTerminal)
                    {
                        if (i == values.Length - 1)
                        {
                            if (!next.ContainsKey(key))
                            {
                                next.Add(key, new List<string>());
                                getNext(key, rules, first, next);
                            }
                            next[nonTerminal].AddRange(next[key]);
                        }
                        else
                        {
                            var nextElement = values[i + 1];
                            if (rules.Productions.ContainsKey(nextElement))
                            {
                                if (nonTerminal != nextElement)
                                {
                                    next[nonTerminal].AddRange(first[nextElement]);
                                    if (next[nonTerminal].Contains("ε"))
                                    {
                                        next[nonTerminal].Remove("ε");
                                        if (!next.ContainsKey(nextElement))
                                        {
                                            next.Add(nextElement, new List<string>());
                                            getNext(nextElement, rules, first, next);
                                        }
                                        next[nonTerminal].AddRange(next.First().Value);
                                    }
                                } 
                            }
                            else if (nextElement == "ε")
                            {
                                if (!next.ContainsKey(key))
                                {
                                    next.Add(key, new List<string>());
                                    getNext(key, rules, first, next);
                                }
                                next[nonTerminal].AddRange(next[key]);
                            }
                            else next[nonTerminal].Add(nextElement);
                        }
                    }
                }
            }
        }
    }

    private static void getNext(ProductionRule rules, Dictionary<string, List<string>> first, Dictionary<string, List<string>> next)
    {
        if (!next.ContainsKey(rules.Productions.First().Key))
        {
            next.Add(rules.Productions.First().Key, new List<string>());
        }
        next[rules.Productions.First().Key].Add("$");
        foreach (var rule in rules.Productions)
        {
            foreach (var list in rule.Value)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var element = list[i];

                    if (rules.Productions.ContainsKey(element))
                    {
                        if (!next.ContainsKey(element))
                        {
                            next.Add(element, new List<string>());
                        }

                        if (i == list.Length - 1)
                        {
                            next[element].AddRange(next[rule.Key]);
                        }
                        else
                        {
                            var nextElement = list[i + 1];
                            if (rules.Productions.ContainsKey(nextElement))
                            {
                                next[element].AddRange(first[nextElement]);
                            }
                            else
                            {
                                next[element].Add(nextElement);
                            }
                        }
                    }
                }
            }
        }
    }
}