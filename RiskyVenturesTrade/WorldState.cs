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
    }


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

    internal class Ship
    {
        public string Name { get; set; }
        public Dictionary<int, int> Cargo { get; set; } = new();
        public int CargoTotal {
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
        public int Hp { get; set; };
        public int? Destination { get; set; }
        public int Progress { get; set; }
        public double Florins { get; set; }
        public int MarketTurn { get; set; }
        public Dictionary<int, double> Market { get; set; } = new();
        public Dictionary<int, int> StockPile { get; set; } = new();
    }

    internal class Enterprise
    {
        public int Port { get; set; }
        public double Florins { get; set; }
        public int StockPile { get; set; }
        public bool AutoSell { get; set; }
        public int Good { get; set; }
        public int ProductionSpeed { get; set;}
    }

    internal class ShipType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Speed { get; set; }
        public int Cost { get; set; }
        public int Health { get; set; }

        public int Capacity { get; set; }
    }

    internal class Captain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public CaptainSpec CaptainSpec { get; set; }
    }

    internal class Good
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double FairPrice { get; set; }
    }


    public enum CaptainSpec
    {
        Trade = 1,
        Exploration = 2,
    }

    public enum Disasters
    {
        Minor = 0,
        Delay = 1,
        Jetsam = 2,
        Damage = 3,
    }

}
