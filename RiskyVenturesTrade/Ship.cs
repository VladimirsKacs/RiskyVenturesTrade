namespace RiskyVenturesTrade
{
    internal class Ship
    {
        public string Name { get; set; }
        public Dictionary<int, int> Cargo { get; set; } = new();
        public int CargoTotal
        {
            get
            {
                var total = 0;
                foreach (var item in Cargo)
                {
                    total += item.Value;
                }
                return total;
            }
        }
        public int Captain { get; set; }
        public int Type { get; set; }
        public int Hp { get; set; }
        public int HpMax { get; set; }
        public int Speed { get; set; }
        public int? Destination { get; set; }
        public int Progress { get; set; }
        public double Florins { get; set; }
        public int MarketTurn { get; set; }
        public Dictionary<int, double> Market { get; set; } = new();
        public Dictionary<int, int> StockPile { get; set; } = new();
        public List<Item> Items { get; set; } = new();
    }
}
