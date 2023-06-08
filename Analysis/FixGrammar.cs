namespace Grammar.Analysis;

public class FixGrammar {

    public static ProductionRule RemoveLeftRecursion( ProductionRule rules ) {
        ProductionRule newRules = new();
        foreach (KeyValuePair<string, List<string[]>> rule in rules.Productions) {
            List<string[]> newProductions = new();
            List<string[]> oldProductions = new();
            foreach (string[] production in rule.Value) {
                // If the production starts with the non-terminal, we have left recursion
                if (production[0] == rule.Key) {
                    oldProductions.Add(production);
                }
                // Otherwise, we don't need to do anything
                else {
                    newProductions.Add(production);
                }
            }
            if (oldProductions.Count == 0) {
                newRules.Productions.Add(rule.Key, rule.Value);
            }
            else {
                string newNonTerminal = rule.Key + "'";
                List<string[]> newProductions2 = new();
                foreach (string[] production in newProductions) {
                    newProductions2.Add(production.Append(newNonTerminal).ToArray());
                }
                newRules.Productions.Add(rule.Key, newProductions2);
                newRules.Productions.Add(newNonTerminal, new List<string[]>());
                foreach (string[] production in oldProductions) {
                    newRules.Productions[newNonTerminal].Add(production.Skip(1).Append(newNonTerminal).ToArray());
                }
                newRules.Productions[newNonTerminal].Add(new string[] { "\u03F5" });
            }
        }
        return newRules;
    }

    public static void LeftFactorization( ProductionRule rules ) {

    }


}