using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorationStory.World
{
    public class Player
    {
        public string Name { get; set; } = "PlayerName";
        public string CurrentRoomID { get; set; }

        public Player(string currentRoomID)
        {
            this.CurrentRoomID = currentRoomID;
        }
    }
}
