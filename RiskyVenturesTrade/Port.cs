namespace RiskyVenturesTrade
{
    internal class Port
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public double Danger { get; set; }
        public int ProductionType { get; set; }
        public int ProductionSpeed { get; set; }
        public Dictionary<int, double> Market { get; set; } = new();
        public Dictionary<int, double> Appetites { get; set; } = new();
        public Dictionary<int, int> StockPile { get; set; } = new();
    }
}
