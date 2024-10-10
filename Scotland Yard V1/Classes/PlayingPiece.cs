using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.Classes
{
    abstract class PlayingPiece
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location? Location { get; set; }
        public Player Player { get; set; }
        public int TaxiTicketAmount { get; set; }
        public int BusTicketAmount { get; set; }


        public PlayingPiece(int id, string name, Player player)
        {
            Id = id;
            Name = name;
            Player = player;
            TaxiTicketAmount = 10;
            BusTicketAmount = 10;
        }

        public PlayingPiece(int id, string name, Location location, Player player, int taxiTicketAmount, int busTicketAmount)
        {
            Id = id;
            Name = name;
            Location = location;
            Player = player;
            TaxiTicketAmount = taxiTicketAmount;
            BusTicketAmount = busTicketAmount;
        }


        public void GiveLocation(Location location)
        {
            Location = location;
        }

        static public List<PlayingPiece> GetPlayingPiecesStats()
        {
            DAL.DAL dal = new DAL.DAL();
            dal.DataRetrievalPlayingPiecesStats();
            return dal.playingPieces;
        }
        public virtual Turn PlayTurn(List<Location> board, Round round, Location location)
        {
            // made to make override possible
            return null;
        }
        public virtual Turn PlayTurn(List<Location> board, Round round, Location location, Location lastXLocation, Turn xTurn, MisterX misterX)
        {
            // made to make override possible
            return null;
        }

        static public bool CheckCatch(List<PlayingPiece> pieces, Game game)
        {
            int xLocation = 0;
            foreach (PlayingPiece piece in pieces)
            {
                if (piece is MisterX)
                {
                    xLocation = piece.Location.Id;
                }
            }
            foreach (PlayingPiece piece in pieces)
            {
                if (piece is Agent)
                {
                    if (piece.Location.Id == xLocation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Mister X is gepakt door {piece.Name}");
                        Console.WriteLine($"De agenten winnen het spel");
                        Console.ReadLine();
                        game.DalGiveWinner(piece);
                        Console.Clear();
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdatePlayingPiece()
        {
            DAL.DAL dal = new DAL.DAL();
            dal.UpdatePlayingPiece(this);
        }

        static public List<PlayingPiece> GetPlayingPiecesContinu(int gameId, List<Location> board)
        {
            DAL.DAL dal = new DAL.DAL();
            dal.DataRetrievalPlayingPiecesContinu(gameId, board);
            return dal.playingPieces;
        }

    }
}
