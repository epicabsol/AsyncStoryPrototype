using AsyncStory.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStoryPrototype.World
{
    public class Room
    {
        public string ID { get; }
        public string Name { get; }
        public string BodyText { get; }

        public string SouthRoomID { get; set; } = "";
        public string NorthRoomID { get; set; } = "";
        public string EastRoomID { get; set; } = "";
        public string WestRoomID { get; set; } = "";

        public Func<StoryContext, ActionMenuChoice[]> ListActionsHandler { get; }

        public Room(string id, string name, string bodyText, string southRoomID = "", string northRoomID = "", string eastRoomID = "", string westRoomID = "")
        {
            this.ID = id;
            this.Name = name;
            this.BodyText = bodyText;
            this.SouthRoomID = southRoomID;
            this.NorthRoomID = northRoomID;
            this.EastRoomID = eastRoomID;
            this.WestRoomID = westRoomID;

            this.ListActionsHandler = this.ListActions;
        }

        protected virtual ActionMenuChoice[] ListActions(StoryContext context)
        {
            List<ActionMenuChoice> choices = new List<ActionMenuChoice>();
            if (NorthRoomID != "")
            {
                choices.Add(new ActionMenuChoice("North", async () => await context.MoveToRoom(NorthRoomID)));
            }
            if (SouthRoomID != "")
            {
                choices.Add(new ActionMenuChoice("South", async () => await context.MoveToRoom(SouthRoomID)));
            }
            if (EastRoomID != "")
            {
                choices.Add(new ActionMenuChoice("East", async () => await context.MoveToRoom(EastRoomID)));
            }
            if (WestRoomID != "")
            {
                choices.Add(new ActionMenuChoice("West", async () => await context.MoveToRoom(WestRoomID)));
            }

            return choices.ToArray();
        }
    }
}
