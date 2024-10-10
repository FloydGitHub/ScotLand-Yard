using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.Classes
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Player(string name)
        {
            Name = name;
        }
        public Player(int id, string name)
        {
            Id = id;
            Name = name;
        }

        static public List<Player> GetPlayers()
        {
            DAL.DAL dal = new DAL.DAL();
            dal.DataRetrievalPlayers();
            return dal.players;
        }
        public void DalCreatePlayer()
        {
            DAL.DAL dal = new DAL.DAL();
            dal.CreatePlayer(this);
        }
    }
}
