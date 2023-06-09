namespace Grammar.Analysis;

public class FixGrammar {

    public static ProductionRule RemoveLeftRecursion( ProductionRule rules ) {
        ProductionRule newRules = new();
        foreach (KeyValuePair<string, List<string[]>> rule in rules.Productions) {
            bool rec = false;
            foreach (string[] production in rule.Value) {
                if (production[0] == rule.Key) {
                    rec = true;
                    break;
                }
            }
            if (rec) {
                string newNonTerminal = rule.Key + "'";
                newRules.Productions.Add(rule.Key, new List<string[]>());
                newRules.Productions.Add(newNonTerminal, new List<string[]>());
                foreach (string[] production in rule.Value) {
                    if (production[0] == rule.Key) {
                        newRules.Productions[newNonTerminal].Add(production.Skip(1).Append(newNonTerminal).ToArray());
                    }
                    else {
                        newRules.Productions[rule.Key].Add(production.Append(newNonTerminal).ToArray());
                    }
                }
                newRules.Productions[newNonTerminal].Add(new string[] { "ε" });
            }
            else {
                newRules.Productions.Add(rule.Key, rule.Value);
            }
        }
        return newRules;
    }

    public static ProductionRule LeftFactoring( ProductionRule rules ) {
        ProductionRule newRules = new();
        foreach (KeyValuePair<string, List<string[]>> rule in rules.Productions) {
            // If there is only one production, we don't need to do anything
            if (rule.Value.Count == 1) {
                newRules.Productions.Add(rule.Key, rule.Value);
            }
            else {
                // Find the longest common prefix
                string[] prefix = rule.Value[0];
                for (int i = 1; i < rule.Value.Count; i++) {
                    prefix = LongestCommonPrefix(prefix, rule.Value[i]);
                }
                // If there is no common prefix, we don't need to do anything
                if (prefix.Length == 0) {
                    newRules.Productions.Add(rule.Key, rule.Value);
                }
                else {
                    // Create a new non-terminal
                    string newNonTerminal = rule.Key + "'";
                    newRules.Productions.Add(rule.Key, new List<string[]>());
                    newRules.Productions.Add(newNonTerminal, new List<string[]>());
                    // Add the new production to the original non-terminal
                    newRules.Productions[rule.Key].Add(prefix.Append(newNonTerminal).ToArray());
                    // Add the new productions to the new non-terminal
                    foreach (string[] production in rule.Value) {
                        newRules.Productions[newNonTerminal].Add(production.Skip(prefix.Length).ToArray());
                    }
                }
            }
        }
        return newRules;
    }

    private static string[] LongestCommonPrefix( string[] a, string[] b ) {
        int i = 0;
        while (i < a.Length && i < b.Length && a[i] == b[i]) {
            i++;
        }
        return a.Take(i).ToArray();
    }


}