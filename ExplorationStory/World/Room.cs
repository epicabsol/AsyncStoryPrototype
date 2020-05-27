using AsyncStory.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorationStory.World
{
    public class Room
    {
        public string ID { get; }
        public string Name { get; }
        public List<ActionMenuChoice> AdditionalActions { get; }

        public string SouthRoomID { get; set; } = "";
        public string NorthRoomID { get; set; } = "";
        public string EastRoomID { get; set; } = "";
        public string WestRoomID { get; set; } = "";

        // Callbacks for individual instances to override behavior
        private Func<StoryContext, ActionMenuChoice[]> ListActionsHandler { get; }
        private Func<StoryContext, string> BodyTextHandler { get; }

        public Room(string id, string name, string bodyText, string southRoomID = "", string northRoomID = "", string eastRoomID = "", string westRoomID = "", params ActionMenuChoice[] additionalActions) : this(id, name, southRoomID, northRoomID, eastRoomID, westRoomID, null, (_) => bodyText, additionalActions)
        {

        }

        public Room(string id, string name, string southRoomID, string northRoomID, string eastRoomID, string westRoomID, Func<StoryContext, ActionMenuChoice[]> listActionsHandler = null, Func<StoryContext, string> bodyTextHandler = null, params ActionMenuChoice[] additionalActions)
        {
            this.ID = id;
            this.Name = name;
            this.AdditionalActions = new List<ActionMenuChoice>();
            this.SouthRoomID = southRoomID;
            this.NorthRoomID = northRoomID;
            this.EastRoomID = eastRoomID;
            this.WestRoomID = westRoomID;
            this.ListActionsHandler = listActionsHandler ?? DefaultListActions;
            this.BodyTextHandler = bodyTextHandler ?? DefaultBodyText;
            this.AdditionalActions = new List<ActionMenuChoice>(additionalActions);
        }

        public ActionMenuChoice[] ListActions(StoryContext context)
        {
            return ListActionsHandler(context);
        }

        protected virtual ActionMenuChoice[] DefaultListActions(StoryContext context)
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

            choices.AddRange(this.AdditionalActions);

            choices.AddRange(context.ListPlayerActions());

            return choices.ToArray();
        }

        public string GetBodyText(StoryContext context)
        {
            return BodyTextHandler(context);
        }

        protected virtual string DefaultBodyText(StoryContext context)
        {
            return "";
        }
    }
}
