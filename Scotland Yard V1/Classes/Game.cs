using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.Classes
{
    internal class Game
    {
        public int Id { get; set; }
        public List<PlayingPiece> PlayingPieces { get; set; } = new List<PlayingPiece>();
        public PlayingPiece? Winner { get; set; }
        public List<Round> Rounds { get; set; } = new List<Round>();
        public string Date { get; set; }

        public Game() 
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public Game(int id,PlayingPiece? winner, string date)
        {
            Id = id;
            Winner = winner;
            Date = date;
        }

        public void AddRound(Round round)
        {
            Rounds.Add(round);
        }

        public void VieuwResult()
        {
            Console.WriteLine($"Spel gespeeld op: {Date}, GameId: {Id}");
            if (Winner == null)
            {
                Console.WriteLine("dit spel is (nog) niet afgerond");
                Console.WriteLine();
            }
            else if (Winner is MisterX)
            {
                Console.WriteLine($"{Winner.Player.Name} heeft gewonnen als Mister X");
                Console.WriteLine();
            }
            else if (Winner is Agent)
            {
                Console.WriteLine($"{Winner.Player.Name} heeft Mister X gepakt als agent");
                Console.WriteLine();
            }

        }

        public void DalGiveWinner(PlayingPiece playingPiece)
        {
            Winner = playingPiece;
            DAL.DAL dal = new DAL.DAL();
            dal.UpdateGame(this);
        }
        public void DalCreateGame()
        {
            DAL.DAL dal = new DAL.DAL();
            Id = dal.CreateGame(this);
        }

        public void GivePlayingPieces(List<PlayingPiece> playingPieces)
        {
            PlayingPieces = playingPieces;
        }
        static public List<Game> GetGameStats()
        {             
                DAL.DAL dal = new DAL.DAL();
                   dal.DataRetrievalGamesStats();
                   return dal.games;
        }
        static public List<Game> GetGamesToContinu()
        {
            DAL.DAL dAL = new DAL.DAL();
            dAL.DataRetrievalGamesToContinu();
            return dAL.games;
        }


    }
}

