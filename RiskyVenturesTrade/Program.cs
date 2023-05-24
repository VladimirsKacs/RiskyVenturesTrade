namespace RiskyVenturesTrade
{
    using Newtonsoft.Json;

    public class Program
    {

        const string fileLocation = "world.dat";

        static WorldState WorldState;
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Risky Ventures: Trade!");
            Console.WriteLine("Please choose an action:");
        }

        static void Load()
        {
            if (File.Exists(fileLocation))
            {
                // Read entire text file content in one string
                var text = File.ReadAllText(fileLocation);
                WorldState = JsonConvert.DeserializeObject<WorldState>(text);
            }
            else
            {
                WorldState = new();
                WorldState.Ports.Add(new Port { Name = "HomeLands", Appetites = new Dictionary<int, double> { { 0, 0 }, { 1, 0 } }, Danger = 0, Distance = 0, Market = new Dictionary<int, double> { { 0, 1 }, { 1, 10 } }, ProductionType = 0 });
                WorldState.Ports.Add(new Port { Name = "ChannelCross", Appetites = new Dictionary<int, double> { { 0, 0 }, { 1, 0 } }, Danger = 0.01, Distance = 10, Market = new Dictionary<int, double> { { 0, 10 }, { 1, 1 } }, ProductionType = 1 });
                WorldState.ShipTypes.Add(new ShipType { Cost = 100, Name = "Cog", Description = "Trade Ship", Health = 2, Speed = 10, Capacity = 10 });
                WorldState.ShipTypes.Add(new ShipType { Cost = 100, Name = "Caravel", Description = "Exploration Ship", Health = 3, Speed = 15, Capacity = 5 });
            }
        }

        static void Save()
        {
            var text = JsonConvert.SerializeObject(WorldState);
            File.WriteAllText(fileLocation, text);
        }
    }
}