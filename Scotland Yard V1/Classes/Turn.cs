using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.Classes
{
    internal class Turn
    {
        public int Id { get; set; }
        public PlayingPiece PlayingPiece { get; set; }
        public string Vehicle { get; set; }


        public Turn(PlayingPiece playingPiece, string vehicle)
        {
            PlayingPiece = playingPiece;
            Vehicle = vehicle;
        }

        public Turn(int id, string vehicle)
        {
            Id = id;
            Vehicle = vehicle;
        }

        public void DalCreateTurn(int RoundId)
        {
            DAL.DAL dal = new DAL.DAL();
            Id = dal.CreateTurn(this, RoundId);
        }

        static public Turn GetLastXTurn(int roundId)
        {
            DAL.DAL dal = new DAL.DAL();
            dal.DataRetrievalTurns(roundId);
            return dal.turns.Last();
            
        }

    }
}
