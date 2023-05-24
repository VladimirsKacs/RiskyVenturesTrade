namespace RiskyVenturesTrade
{
    internal class WorldState
    {
        public HashSet<Port> Ports { get; set; }
        public HashSet<Ship> Ships { get; set; }

    }


    internal class Port
    {
        public string Name { get; set; }
        public int Distance { get; set; }
        public float Danger { get; set; }
        public int ProductionType { get; set; }
        public Dictionary<int, float> Market { get; set; } = new();
        public Dictionary<int, float> Appetites { get; set; } = new();
    }

    internal class Ship
    {
        public string Name { get; set; }
        public Dictionary<int, int> Cargo { get; set; } = new();
        public Captain Captain { get; set; }
        public int Type { get; set; }
        public List<Disasters> Disasters { get; set; } = new();
        public Port Destination { get; set; }
        public int Progress { get; set; }
    }

    internal class Captain
    {
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
