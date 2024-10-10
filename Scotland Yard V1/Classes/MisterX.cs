using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scotland_Yard_V1.Classes
{
    internal class MisterX : PlayingPiece
    {
        public MisterX(int id,string name, Player player) : base(id,name, player)
        {
        }

        public MisterX(int id, string name, Location location, Player player, int taxiTicketAmount, int busTicketAmount) : base(id, name, location, player, taxiTicketAmount, busTicketAmount)
        {
        }
        public override Turn PlayTurn(List<Location> board, Round round, Location location)
        {
            Console.WriteLine($"huidige ronde: {round.RoundCount}");
            Console.WriteLine($"Het is de beurt van Mister X ({Name})");
            int roundreveal = round.RoundCount % 2;
            if (round.RoundCount == 0)
            {
                roundreveal = 2;
            }
            Console.WriteLine($"aantal rondes tot onthulling: {roundreveal}");
            Console.WriteLine($"U bevind zich op locatie {location.Id}");
            Console.WriteLine($"Aantal TaxiTickets in bezit: {TaxiTicketAmount}");
            Console.WriteLine($"Aantal BusTickets in bezit: {BusTicketAmount}");
            Console.WriteLine("----------------------------------------");
            if (location.BusStation)
            {
                Console.WriteLine("Kies u voertuig:");
                Console.WriteLine("1: Taxi");
                Console.WriteLine("2: Bus");
                int choiseVehicle = int.Parse(Console.ReadLine() ?? string.Empty);
                string vehicle = "";
                while (choiseVehicle != 1 && choiseVehicle != 2)
                {
                    Console.WriteLine("Ongeldige invoer, kies opnieuw");
                    choiseVehicle = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                Console.Clear();
                List<int> protentialLocations = new List<int>();
                if (choiseVehicle == 1)
                {
                    protentialLocations = location.CheckTaxiLocations();
                    vehicle = "Taxi";
                    TaxiTicketAmount--;
                }
                else if (choiseVehicle == 2)
                {
                    protentialLocations = location.CheckBusLocations();
                    vehicle = "Bus";
                    BusTicketAmount--;
                }
                Console.WriteLine("Voertuig : " + vehicle);
                Console.WriteLine("Kies een locatie:");
                int option = 1;
                foreach (int loc in protentialLocations)
                {
                    Console.WriteLine($"{option}. {loc} ");
                    option++;
                }
                int choiseLocation = int.Parse(Console.ReadLine() ?? string.Empty);
                while (choiseLocation < 1 || choiseLocation > protentialLocations.Count)
                {
                    Console.WriteLine("Ongeldige invoer, kies opnieuw");
                    choiseLocation = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                int intNewLocation = protentialLocations[choiseLocation - 1];
                Location newLocation = board.FirstOrDefault(l => l.Id == intNewLocation);
                GiveLocation(newLocation);
                Turn turn = new Turn(this, vehicle);
                Console.Clear();
                return turn;


            }
            else
            {
                List<int> protentialLocations = location.CheckTaxiLocations();
                TaxiTicketAmount--;
                Console.WriteLine("Voertuig : Taxi");
                Console.WriteLine("Kies een locatie:");
                int option = 1;
                foreach (int loc in protentialLocations)
                {
                    Console.WriteLine($"{option}. {loc} ");
                    option++;
                }
                int choiseLocation = int.Parse(Console.ReadLine() ?? string.Empty);
                while (choiseLocation < 1 || choiseLocation > protentialLocations.Count)
                {
                    Console.WriteLine("Ongeldige invoer, kies opnieuw");
                    choiseLocation = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                int intNewLocation = protentialLocations[choiseLocation - 1];
                Location newLocation = board.FirstOrDefault(l => l.Id == intNewLocation);
                GiveLocation(newLocation);
                Turn turn = new Turn(this, "Taxi");
                Console.Clear();
                return turn;
            }


        }

        public void DalCreatePiece(int gameId)
        {
            DAL.DAL dal = new DAL.DAL();
            Id = dal.CreateMisterX(this, gameId);
        }
    }
}
