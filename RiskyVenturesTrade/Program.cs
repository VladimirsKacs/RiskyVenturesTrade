using System.Runtime.InteropServices.ComTypes;

namespace RiskyVenturesTrade
{
    using Newtonsoft.Json;
    using System;

    public class Program
    {

        const string fileLocation = "world.dat";
        static Random Rand = new();

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
                Name = "New Haven",
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
                Distance = 20,
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
            WorldState.Turn = 0;
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
            ship.HpMax = WorldState.ShipTypes[type].Health;
            ship.Speed = WorldState.ShipTypes[type].Speed;
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
            ship.Progress = 0;
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
                if (amount == 0)
                {
                    amount = ship.Cargo[goodId];

                }
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
            WorldState.Turn++;
            AdvanceShips();
            AdvanceMarkets();
        }

        static void AdvanceShips()
        {
            foreach (var ship in WorldState.Ships)
            {
                if (ship.Hp <= 0)
                {
                    WorldState.Captains.RemoveWhere(c => c.Id == ship.Captain);
                    WorldState.Ships.Remove(ship);
                }
                if (ship.Destination == null) continue;
                if (ship.Destination.Value == 0) {
                    Discovery(ship);
                    continue;
                }

                var port = WorldState.Ports[ship.Destination.Value];
                var speed = ship.Speed;
                switch (ProblemTrade(ship, port))
                {
                    case Disasters.Damage: ship.Hp--;
                        break;
                    case Disasters.Delay: speed = Rand.Next(0, speed); break;
                    case Disasters.Jetsam: 
                        //TODO
                    default:
                        break;
                }
                

                if (ship.Progress < port.Distance / 2 && ship.Progress + speed >= port.Distance / 2)
                    Trade(ship, port);
                ship.Progress += speed;
                if (ship.Progress >= port.Distance)
                    Arrival(ship);
            }
        }

        static Disasters ProblemTrade(Ship ship, Port port)
        {
            var cap = WorldState.Captains.FirstOrDefault(c => c.Id == ship.Captain);
            var adjuster = CaptainExpertise.ProblemAversion[cap.Level - 1 * (cap.CaptainSpec == CaptainSpec.Exploration ? 1 : 0)];
            var probability = adjuster * port.Danger;
            if (Rand.NextDouble() < probability)
            {
                switch (Rand.Next(0, 3))
                {
                    case 0:
                        return Disasters.Damage;
                    case 1:
                        return Disasters.Delay;
                    case 2:
                        return Disasters.Jetsam;
                }
            }
            return Disasters.None;
        }

        static Disasters ProblemExploration(Ship ship)
        {
            var cap = WorldState.Captains.FirstOrDefault(c => c.Id == ship.Captain);
            var adjuster = CaptainExpertise.ProblemAversion[cap.Level - 1 * (cap.CaptainSpec == CaptainSpec.Trade ? 1 : 0)];
            var probability = adjuster * 0.5;
            if (Rand.NextDouble() < probability)
            {
                switch (Rand.Next(0, 3))
                {
                    case 0:
                        return Disasters.Damage;
                    case 1:
                    case 2:
                        return Disasters.Delay;
                }
            }
            return Disasters.None;
        }

        static void Discovery(Ship ship)
        {
            var target = Rand.Next((WorldState.PortCount + 5)*5,(WorldState.PortCount + 5) * 25);
            if (ship.Progress > target)
                NewLands(ship, target / 2 );
            else
            {
                var speed = ship.Speed;
                switch (ProblemExploration(ship))
                {
                    case Disasters.Delay:
                        speed = Rand.Next(speed);
                        break;
                    case Disasters.Damage:
                        ship.Hp--;
                        break;
                }

                ship.Progress += speed;
            }
        }

        static void NewLands(Ship ship, int distance)
        {
            //idea: repeating goods
            string name;
            do
            {
                name = GoodNames[Rand.Next(GoodNames.Count)];
            } while (WorldState.Goods.Exists(p => p.Name == name));
            var good = new Good
            {
                FairPrice = Rand.Next(distance / 4, distance / 2),
                Id = WorldState.GoodCount++,
                Name = name
            };
            WorldState.Goods.Add(good);
            foreach (var oldPort in WorldState.Ports)
            {
                oldPort.Market[good.Id] = good.FairPrice;
                oldPort.StockPile[good.Id] = 0;
            }
            var port = new Port();
            do
            {
                name = LandNameGen();
            } while (WorldState.Ports.Exists(p=>p.Name == name));
            port.Distance = distance;
            port.Name = name;
            port.Id = WorldState.PortCount++;
            port.ProductionSpeed = Rand.Next(10, 20);
            port.Danger = Rand.NextDouble() * 0.5;
            port.ProductionType = good.Id;
            foreach (var oldGood in WorldState.Goods)
            {
                port.Market[oldGood.Id] = oldGood.FairPrice;
                port.StockPile[oldGood.Id] = 0;
            }

            port.Market[good.Id] = good.FairPrice / 5;
            port.StockPile[good.Id] = 100;
            WorldState.Ports.Add(port);
            ship.Market = port.Market;
            ship.StockPile = port.StockPile;
            ship.MarketTurn = WorldState.Turn;
            ship.Destination = null;
            ship.Progress = 0;
            Console.WriteLine(
                ship.Name + " has returned from it's voyage of exploration, bringing news of " + port.Name);
        }

        static readonly List<string> GoodNames = new List<string> { "Apples", "Bananas", "Copper", "Diamonds", "Escargot", "Figs", "Gold", "Hemp", "Iron", "Jelly", "Knick-knacks",
            "Lemons", "Melons", "Nylons", "Opiates", "Pearls", "Quartz", "Resin", "Silver", "Tangerines", "Uranium"};

        static readonly List<string> Beginnings = new List<string> { "New", "Port", "Cape", "Great"};
        static readonly List<string> Endings = new List<string> { "Isle", "Shore", "Springs", "Rock", "Island", "Haven" };
        static readonly List<string> Names = new List<string> { "Aral", "Bean", "Cage", "Deft", "Envin", "Frakes", "Golen",
            "Hyun", "Iverson", "Jackson", "Kath", "Leguin", "Minster", "O'Mann", "Price", "Questor", "Reiff", "St.Quinn",
            "Teviegh", "Vronsky", "West", "Xanadu", "Zapata", "Azer", "Bizzt", "Cerg", "Dreary", "Esthman", "Firm", "Grimm", "Heiger",
            "Iskra", "Jay", "Kell", "L'Etarto","Magimann","Orkin", "Pigmen", "Roald", "Severus", "Trata", "Urist", "Vexler", "Worclaw", "Xander",
            "Yersika", "Zeit" };

        static string LandNameGen()
        {
            if(Rand.Next() > 0.5)
                return Beginnings[Rand.Next(Beginnings.Count)] + " " + Names[Rand.Next(Names.Count)];
            return Names[Rand.Next(Names.Count)] + " " + Endings[Rand.Next(Endings.Count)];
        }


        static void Trade(Ship ship, Port port)
        {
            foreach (var good in WorldState.Goods)
            {
                var cargo = ship.Cargo[good.Id];
                ship.Cargo[good.Id] = 0;
                ship.Florins += port.Market[good.Id] * cargo*(1-WorldState.PriceAdjustment);
                port.StockPile[good.Id] += cargo;
            }
            var price = port.Market[port.ProductionType]*(1+WorldState.PriceAdjustment);
            var amount = WorldState.ShipTypes[ship.Type].Capacity;
            if (price*amount>ship.Florins)
                amount = (int) (ship.Florins/price);
            if(amount>port.StockPile[port.ProductionType])
                amount = port.StockPile[port.ProductionType];
            ship.Cargo[port.ProductionType] = amount;
            ship.Florins -= price * amount;

            ship.Market=port.Market;
            ship.StockPile=port.StockPile;
            ship.MarketTurn = WorldState.Turn;
        }

        static void Arrival(Ship ship)
        {
            var port = WorldState.Ports[ship.Destination.Value];
            Console.WriteLine(
                ship.Name + " has returned from " + port.Name + ", bringing " + ship.Cargo[port.ProductionType] + " " + WorldState.Goods[port.ProductionType].Name);
            ship.Destination = null;
            ship.Progress = 0;
        }

        static void AdvanceMarkets()
        {
            foreach (var port in WorldState.Ports)
            {
                var market = port.Market;
                var stockpile = port.StockPile;
                var appetites = port.Appetites;
                stockpile[port.Id] += port.ProductionSpeed;
                foreach ( var good in WorldState.Goods)
                {
                    if (port.ProductionType == good.Id)
                    {
                        var priceTarget = good.FairPrice * (1 + good.FairPrice) / (stockpile[good.Id] + good.FairPrice);
                        market[good.Id] = (priceTarget + market[good.Id] * 4) / 5 + Rand.NextDouble()*0.2 - 0.1;
                    }
                    else
                    {
                        if (stockpile[good.Id] < (int)appetites[good.Id])
                            stockpile[good.Id] = 0;
                        else
                            stockpile[good.Id] -= (int)appetites[good.Id];
                        var priceTarget = good.FairPrice * (appetites[good.Id] + good.FairPrice) /
                                          (stockpile[good.Id] + good.FairPrice);
                        market[good.Id] = (priceTarget + market[good.Id] * 4) / 5 + Rand.NextDouble() - 0.5;
                        var appetiteAdjustment = good.FairPrice - market[good.Id];
                        if (appetites[good.Id] + appetiteAdjustment < 0)
                            appetites[good.Id] = 0;
                        else
                            appetites[good.Id] += appetiteAdjustment;
                        if (stockpile[good.Id] == 0)
                            appetites[good.Id] /= 2;
                    }
                }
            }
        }
    }
}