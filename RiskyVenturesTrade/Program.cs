namespace RiskyVenturesTrade
{
    using Newtonsoft.Json;
    using System;

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
                Console.WriteLine("6.Ship Cargo");
                Console.WriteLine("7.Enterprises");
                Console.WriteLine("8.Advance Turn");
                Console.WriteLine("0.Save and Exit");
                Console.WriteLine("-.Exit Without Saving");
                var option = Console.ReadKey();
                switch (option.KeyChar)
                {
                    case '1':
                        NewShip();
                        break;
                    case '2':
                        NewCaptain();
                        break;
                    case '3':
                        SendShip();
                        break;
                    case '4':
                        Sell();
                        break;
                    case '5':
                        Buy();
                        break;

                    case '8':
                        Advance();
                        break;
                    case '0':
                        Save();
                        return;
                    case '-':
                        return;
                }
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
                SetUpNewWorld();
        }

        static void SetUpNewWorld()
        {
            WorldState = new();
            WorldState.Ports.Add(new Port
            {
                Name = "HomeLands",
                Appetites = new Dictionary<int, double>
                    {
                        {
                            0, 0
                        },
                        {
                            1, 0
                        },
                        {
                            99, 0
                        },
                    },
                Danger = 0,
                Distance = 0,
                Market = new Dictionary<int, double>
                    {
                        {
                            0, 1
                        },
                        {
                            1, 10
                        },
                        {
                            99, 10
                        },
                    },
                ProductionType = 0,
                ProductionSpeed = 100,
                StockPile = new Dictionary<int, int>
                    {
                        {
                            0, 1000
                        },
                        {
                            1, 0
                        },
                        {
                            99, 100
                        }
                    },
                Id = 0
            });
            WorldState.Ports.Add(new Port
            {
                Name = "ChannelCross",
                Appetites = new Dictionary<int, double>
                    {
                        {
                            0, 0
                        },
                        {
                            1, 0
                        },
                        {
                            99, 0
                        },

                    },
                Danger = 0.01,
                Distance = 1,
                Market = new Dictionary<int, double>
                    {
                        {
                            0, 5
                        },
                        {
                            1, 1
                        },
                        {
                            99, 20
                        },
                    },
                ProductionType = 1,
                ProductionSpeed = 10,
                Id = 1,
                StockPile = new Dictionary<int, int>
                    {
                        {
                            0,10
                        },
                        {
                            1, 100
                        },
                        {
                            99, 0
                        },
                    }
            });
            WorldState.PortCount = 2;
            WorldState.ShipTypes.Add(new ShipType { Cost = 100, Name = "Cog", Description = "Trade Ship", Health = 2, Speed = 10, Capacity = 10, Id = 0 });
            WorldState.ShipTypes.Add(new ShipType { Cost = 100, Name = "Caravel", Description = "Exploration Ship", Health = 3, Speed = 15, Capacity = 5, Id = 1 });
            WorldState.ShipCount = 2;
            WorldState.CapCount = 0;
            WorldState.Goods.Add(new Good { Id = 0, Name = "Lumber", FairPrice=10 });
            WorldState.Goods.Add(new Good { Id = 1, Name = "Sand", FairPrice=10 });
            WorldState.Goods.Add( new Good {Id = 99, Name = "Tools", FairPrice = 20});
            WorldState.GoodCount = 2;
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
            ship.Type = WorldState.ShipTypes[type].Id;
            ship.Hp = WorldState.ShipTypes[type].Health;
            WorldState.Ships.Add(ship);
            Console.WriteLine(WorldState.ShipTypes[ship.Type].Name+" "+ship.Name+" created");
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
            cap.Id = WorldState.CapCount++;
            cap.Level = 1;
            cap.Xp = 0;
            WorldState.Captains.Add(cap);
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
            for (var i = 0; i < caps.Count; i++)
            {
                Console.WriteLine(i + "." + caps[i].Name);
            }
            var stringOption = Console.ReadLine();
            if (int.TryParse(stringOption, out var cap) || cap >= caps.Count)
                return;
            ship.Captain = caps[cap].Id;
        }

        static void Buy()
        {
            Console.WriteLine("Which good would you like to buy");
            var goods = WorldState.Goods;
            for (var i = 0;i < goods.Count;i++)
            {
                Console.WriteLine(i + "." + goods[i].Name);
            }
            var option = Console.ReadKey();
            if (int.TryParse(option.ToString(), out var good) || good >= goods.Count)
                return;
            var goodId= goods[good].Id;
            Console.WriteLine("How much would you like to buy?");
            var amount = int.Parse(Console.ReadLine());
            if (amount > WorldState.Ports[0].StockPile[goodId])
            {
                Console.WriteLine("too much");
                return;
            }
            Console.WriteLine("Would you like to place on a ship? y/n");
            option = Console.ReadKey();
            if (option.KeyChar == 'y')
            {
                var shipsInPort = WorldState.Ships.Where(s => s.Destination == null).ToList();
                for (var i = 0; i < shipsInPort.Count; i++)
                {
                    Console.WriteLine(i + "." + shipsInPort[i].Name);
                }
                option = Console.ReadKey();
                if (int.TryParse(option.ToString(), out var num) || num >= shipsInPort.Count)
                    return;
                var ship = shipsInPort[num];
                if(ship.CargoTotal + amount > WorldState.ShipTypes[ship.Type].Capacity)
                {
                    Console.WriteLine("too much");
                    return;
                }
                ship.Cargo[goodId] += amount;
            }
            WorldState.Ports[0].StockPile[goodId]-=amount;
        }

        static void Sell()
        {
            Console.WriteLine("Which good would you like to sell");
            var goods = WorldState.Goods;
            for (var i = 0; i < goods.Count; i++)
            {
                Console.WriteLine(i + "." + goods[i].Name);
            }
            var option = Console.ReadKey();
            if (int.TryParse(option.ToString(), out var good) || good >= goods.Count)
                return;
            var goodId = goods[good].Id;
            Console.WriteLine("Would you like to take from a ship? y/n");
            option = Console.ReadKey();
            var amount = 0;
            if (option.KeyChar == 'y')
            {
                var shipsInPort = WorldState.Ships.Where(s => s.Destination == null).ToList();
                for (var i = 0; i < shipsInPort.Count; i++)
                {
                    Console.WriteLine(i + "." + shipsInPort[i].Name);
                }
                option = Console.ReadKey();
                if (int.TryParse(option.ToString(), out var num) || num >= shipsInPort.Count)
                    return;
                var ship = shipsInPort[num];
                Console.WriteLine("how much would you like to sell?");
                amount = int.Parse(Console.ReadLine());
                if (amount == 0) { amount = ship.Cargo[goodId]}
                if (ship.Cargo[goodId] < amount)
                {
                    Console.WriteLine("too much");
                    return;
                }
                ship.Cargo[goodId] -= amount;
            }
            else
            {
                Console.WriteLine("how much would you like to sell?");
                amount = int.Parse(Console.ReadLine());
            }
            WorldState.Ports[0].StockPile[goodId] += amount;
        }

        static void Advance()
        {
            
        }

        static void AdvanceShips()
        {
            foreach (var ship in WorldState.Ships)
            {
                if (ship.Destination == null) continue;
                if (ship.Destination.Value == 0) {
                    Discovery(ship);
                    continue;
                }

                var port = WorldState.Ports[ship.Destination.Value];
                var speed = WorldState.ShipTypes[ship.Type].Speed;
                if (ship.Progress < port.Distance / 2 && ship.Progress + speed > port.Distance / 2)
                    Trade(ship, port);
                ship.Progress += speed;
                if (ship.Progress > port.Distance)
                    Arrival(ship);
            }
        }

        static void Discovery(Ship ship)
        { 
            throw new NotImplementedException();
        }

        static void Trade(Ship ship, Port port)
        {
            throw new NotImplementedException();
        }

        static void Arrival(Ship ship)
        {
            throw new NotImplementedException();
        }

        static void AdvanceMarkets()
        {
            foreach (var port in WorldState.Ports)
            {
                var market = port.Market;
                var stockpile = port.StockPile;
                var appetite = port.Appetites;
                stockpile[port.Id] += port.ProductionSpeed;
                foreach ( var good in WorldState.Goods)
                {
                    stockpile[good.Id] -= (int)appetite[good.Id];
                }
            }
        }
    }
}