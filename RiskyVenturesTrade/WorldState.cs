namespace RiskyVenturesTrade
{
    internal class WorldState
    {
        public List<Port> Ports { get; set; } = new List<Port>();
        public HashSet<Ship> Ships { get; set; } = new HashSet<Ship>();
        public List<ShipType> ShipTypes { get; set; } = new List<ShipType>();
        public HashSet<Captain> Captains { get; set; } = new HashSet<Captain>();
    }


    internal class Port
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public double Danger { get; set; }
        public int ProductionType { get; set; }
        public Dictionary<int, double> Market { get; set; } = new();
        public Dictionary<int, double> Appetites { get; set; } = new();
    }

    internal class Ship
    {
        public string Name { get; set; }
        public Dictionary<int, int> Cargo { get; set; } = new();
        public int Captain { get; set; }
        public ShipType Type { get; set; }
        public List<Disasters> Disasters { get; set; } = new();
        public int? Destination { get; set; }
        public int Progress { get; set; }
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
