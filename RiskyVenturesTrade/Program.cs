namespace RiskyVenturesTrade
{
    using Newtonsoft.Json;

    public class Program
    {

        const string fileLocation = "world.dat";

        static WorldState WorldState;
        static void Main(string[] args)
        {
            Load();
            Console.WriteLine("Welcome to Risky Ventures: Trade!");
            while (true)
            {
                Console.WriteLine("Please choose an action:");
                Console.WriteLine("1.Create New Ship");
                Console.WriteLine("2.Create New Captain");
                Console.WriteLine("3.Send Ship");
                Console.WriteLine("4.Sell Goods");
                Console.WriteLine("5.Buy Goods");
                Console.WriteLine("6.Advance Turn");
                Console.WriteLine("0.Save and Exit");
                Console.WriteLine("-.Exit Without Saving");
                var option = Console.ReadKey();
            }
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

        static void NewShip()
        {
            var ship = new Ship();
            Console.WriteLine("What is the name of this ship?");
            var name = Console.ReadLine();
            ship.Name = name;
            Console.WriteLine("What is the type of this ship?");
            for (var i = 0; i < WorldState.ShipTypes.Count; i++)
            {
                Console.WriteLine(i + "." + WorldState.ShipTypes[i].Name);
            }
            var option = Console.ReadKey();
            if (int.TryParse(option.ToString(), out var type) || type >= WorldState.ShipTypes.Count)
                return;
            ship.Type = WorldState.ShipTypes[type];
            Console.WriteLine(ship.Type.Name+" "+ship.Name+" created");
        }

        static void NewCaptain()
        {
            var cap = new Captain();
            Console.WriteLine("What is the name of this captain?");
            var name = Console.ReadLine();
            cap.Name = name;
            Console.WriteLine("What is the type of this Captain? (e/t)");
            var option = Console.ReadKey();
            if (option.KeyChar != 'e' && option.KeyChar != 't')
                return;
            cap.CaptainSpec = option.KeyChar == 'e' ? CaptainSpec.Exploration : CaptainSpec.Trade;
            Console.WriteLine(cap.CaptainSpec + " " + cap.Name + " created");
        }

        static void SendShip()
        {
            Console.WriteLine("What ship do you want to send?");
            var shipsInPort = WorldState.Ships.Where(s => s.Destination == null).ToList();
            for (var i = 0; i < shipsInPort.Count; i++)
            {
                Console.WriteLine(i + "." + shipsInPort[i].Name);
            }
            var option = Console.ReadKey();
            if (int.TryParse(option.ToString(), out var num) || num >= shipsInPort.Count)
                return;
            var ship = shipsInPort[num];
            Console.WriteLine("Where do you want to send the ship?");
            var ports = WorldState.Ports;
            for (var i = 1; i < ports.Count; i++)
            {
                Console.WriteLine(i + "." + ports[i].Name);
            }
            Console.WriteLine("0.Exploration");
            option = Console.ReadKey();
            if (int.TryParse(option.ToString(), out var port) || port >= ports.Count)
                return;
            ship.Destination = ports[port].Id;
            Console.WriteLine("Who will captain the ship?");
            var caps = WorldState.Captains.ToList();
        }
    }
}