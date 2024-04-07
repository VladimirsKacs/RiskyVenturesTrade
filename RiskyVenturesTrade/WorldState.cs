namespace RiskyVenturesTrade
{
    internal class WorldState
    {
        public List<Port> Ports { get; set; } = new();
        public int PortCount { get; set; }
        public HashSet<Ship> Ships { get; set; } = new();
        public List<ShipType> ShipTypes { get; set; } = new();
        public int ShipCount { get; set; }
        public HashSet<Captain> Captains { get; set; } = new();
        public int CapCount { get; set; }
        public List<Good> Goods { get; set; } = new();
        public int GoodCount { get; set; }
        public HashSet<Enterprise> Enterprises { get; set; } = new();
        public double PriceAdjustment => 0.1;
        public int Turn { get; set; }
    }
}
