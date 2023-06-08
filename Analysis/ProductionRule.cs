namespace Grammar.Analysis
{
    public class ProductionRule
    {
        public Dictionary<string, List<string[]>> Productions { get; set; }

        // The key is the non-terminal, the value is the list of productions
        public ProductionRule()
        {
            Productions = new Dictionary<string, List<string[]>>();
        }

        public override string ToString()
        {
            return string.Join("\n", Productions.Select(x => $"{x.Key} -> {string.Join(" | ", x.Value.Select(y => string.Join(" ", y)))}"));
        }

    }
}