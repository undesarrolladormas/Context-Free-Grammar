namespace Grammar.Analysis
{
    public class SeparateElements
    {
        public static ProductionRule SeparateNonTerminals(List<string> lines)
        {
            ProductionRule rules = new();
            foreach (var line in lines)
            {
                string[] parts = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    throw new Exception("Invalid production rule");
                }
                string nonterminal = parts[0].Trim();
                if (nonterminal == "" || nonterminal.Contains(" ") || nonterminal.Contains("|"))
                {
                    throw new Exception("Invalid production rule");
                }
                string[] productions = parts[1].Trim().Split("|", StringSplitOptions.RemoveEmptyEntries);
                // Remove the spaces from the productions
                for (int i = 0; i < productions.Length; i++)
                {
                    productions[i] = productions[i].Trim();
                }
                ProductionRule rule = new ProductionRule();
                if (rules.Productions.ContainsKey(nonterminal))
                {
                    foreach (var production in productions)
                    {
                        rules.Productions[nonterminal].Add(production.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                    }
                }
                else
                {
                    rules.Productions.Add(nonterminal, new List<string[]>());
                    foreach (var production in productions)
                    {
                        rules.Productions[nonterminal].Add(production.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                    }
                }
                
            }
            return rules;
        }
    
    }

}