using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.Classes
{
    internal class Location
    {
        public int Id { get; set; }
        public bool BusStation { get; set; }
        public List<int>? ConnectedBusLocations { get; set; } 
        public List<int> ConnectedTaxiLocations { get; set; } 

        public Location(int id, List<int> taxiList, List<int> busList)
        {
            Id = id;
            BusStation = true;
            ConnectedBusLocations = busList;
            ConnectedTaxiLocations = taxiList;
        }
        public Location(int id, List<int> taxiLis) 
        {
            Id = id;
            BusStation = false;
            ConnectedTaxiLocations = taxiLis;
        }

        public static List<Location> MakeBoard()
        {

            List<Location> loctions = new List<Location>();
            loctions.Add(new Location(1, new List<int> { 2, 6, 13 }, new List<int> { 7, 14 }));
            loctions.Add(new Location(2, new List<int> { 1, 4, 7 }));
            loctions.Add(new Location(3, new List<int> { 4, 9, 10 }));
            loctions.Add(new Location(4, new List<int> { 2, 3, 5, }, new List<int> { 7, 11 }));
            loctions.Add(new Location(5, new List<int> { 4, 12 }));
            loctions.Add(new Location(6, new List<int> { 1, 7 }));
            loctions.Add(new Location(7, new List<int> { 2, 6, 8 }, new List<int> { 1, 4, 14 }));
            loctions.Add(new Location(8, new List<int> { 7, 13 }));
            loctions.Add(new Location(9, new List<int> { 3, 10, 15 }));
            loctions.Add(new Location(10, new List<int> { 3, 9, 11 }));
            loctions.Add(new Location(11, new List<int> { 10, 12, 16 }, new List<int> { 4, 14 }));
            loctions.Add(new Location(12, new List<int> { 5, 11 }));
            loctions.Add(new Location(13, new List<int> { 1, 8, 14 }));
            loctions.Add(new Location(14, new List<int> { 13, 15 }, new List<int> { 7, 11 }));
            loctions.Add(new Location(15, new List<int> { 9, 14, 16 }));
            loctions.Add(new Location(16, new List<int> { 11, 15 }));
            return loctions;

        }
        public List<int> CheckTaxiLocations()
        {
            return ConnectedTaxiLocations;
        }
        public List<int> CheckBusLocations()
        {
            return ConnectedBusLocations;
        }
    }
}
