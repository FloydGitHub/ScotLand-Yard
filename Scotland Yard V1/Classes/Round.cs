using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.Classes
{
    internal class Round
    {
        public int Id { get; set; }
        public int RoundCount { get; set; }
        public List<Turn> Turns { get; set; } = new List<Turn>();

        public Round(int roundCount)
        {
            RoundCount = roundCount;
            List<Turn> turns = new List<Turn>();
        }

        public Round(int id, int roundCount)
        {
            Id = id;
            RoundCount = roundCount;
            List<Turn> turns = new List<Turn>();
        }

        public Round(int id, int roundCount, List<Turn> turns)
        {
            Id = id;
            RoundCount = roundCount;
            Turns = turns;
        }

        public void AddTurn(Turn turn)
        {
            Turns.Add(turn);
        }

        public void DalCreateRound(int GameId)
        {
            DAL.DAL dal = new DAL.DAL();
            Id = dal.CreateRound(this, GameId);
        }
        
        static public Round GetLastRound(int gameId)
        {
            DAL.DAL dal = new DAL.DAL();
            dal.DataRetrievalRounds(gameId);
            return dal.rounds.Last();
            
        }
    }
}
