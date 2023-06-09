namespace Grammar.Analysis;

public class FixGrammar
{

    public static ProductionRule RemoveLeftRecursion(ProductionRule rules)
    {
        ProductionRule newRules = new();
        foreach (KeyValuePair<string, List<string[]>> rule in rules.Productions)
        {
            bool rec = false;
            foreach (string[] production in rule.Value)
            {
                if (production[0] == rule.Key)
                {
                    rec = true;
                    break;
                }
            }
            if (rec)
            {
                string newNonTerminal = rule.Key + "'";
                newRules.Productions.Add(rule.Key, new List<string[]>());
                newRules.Productions.Add(newNonTerminal, new List<string[]>());
                foreach (string[] production in rule.Value)
                {
                    if (production[0] == rule.Key)
                    {
                        newRules.Productions[newNonTerminal].Add(production.Skip(1).Append(newNonTerminal).ToArray());
                    }
                    else
                    {
                        newRules.Productions[rule.Key].Add(production.Append(newNonTerminal).ToArray());
                    }
                }
                newRules.Productions[newNonTerminal].Add(new string[] { "ε" });
            }
            else
            {
                newRules.Productions.Add(rule.Key, rule.Value);
            }
        }
        return newRules;
    }

    public static ProductionRule LeftFactoring(ProductionRule rules)
    {
        ProductionRule newRules = new();
        foreach (var rule in rules.Productions)
        {
            var prefix = CommonPrefix(rule.Value);
            if (prefix.Length > 0)
            {
                var newNonTerminal = rule.Key + "'";
                if (!newRules.Productions.ContainsKey(rule.Key))
                    newRules.Productions.Add(rule.Key, new List<string[]>());
                if(!newRules.Productions.ContainsKey(newNonTerminal))
                    newRules.Productions.Add(newNonTerminal, new List<string[]>());
                foreach (var production in rule.Value)
                {
                    if (production[0] == prefix)
                    {
                        if(!(production.Length == 1))
                        newRules.Productions[newNonTerminal].Add(production.Skip(1).ToArray());
                    }
                    else
                    {
                        newRules.Productions[rule.Key].Add(production.ToArray());
                    }
                }
                newRules.Productions[newNonTerminal].Add(new string[] { "ε" });
                newRules.Productions[rule.Key].Add(new string[] { prefix, newNonTerminal });
            }
            else 
            {
                newRules.Productions.Add(rule.Key, rule.Value);
            }
            newRules.Productions[rule.Key] = newRules.Productions[rule.Key].Where(x => x.Length > 0).ToList();
        }

        return newRules;
    }

    private static string CommonPrefix(List<string[]> a)
    {
        string first = a.FirstOrDefault()?.FirstOrDefault();
        if (first != null)
        {
            var sublistasCompartenPrimerElemento = a
                .Select((lista, indice) => new { Indice = indice, Lista = lista })
                .Where(item => item.Lista.FirstOrDefault() == first)
                .ToList();
            if (sublistasCompartenPrimerElemento.Count > 1)
            return first;
        }
        return "";
        
    }


}