using AsyncStory.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorationStory.World
{
    public class StoryContext
    {
        public IExplorationStoryHost StoryHost { get; }
        public Player Player { get; }

        public bool HasGatheredStarfish { get; private set; } = false;

        public Dictionary<string, Room> Rooms { get; }

        public StoryContext(IExplorationStoryHost storyHost)
        {
            this.StoryHost = storyHost;

            Rooms = new Dictionary<string, Room>
            {
                { "southshore", new Room("southshore", "South Shore", "You are standing on the warm sands of an unknown island, amongst the smoldering wreckage of your escape pod.", "", "southpath", "", "",
                    new ActionMenuChoice("Gather Starfish", async () => { StoryHost.WriteLine("You pick up the starfish off the ground"); HasGatheredStarfish = true; await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask)); }, () => !HasGatheredStarfish)) },
                { "southpath", new Room("southpath", "South Path", "Tropical foliage graces the edges of a soft sandy path that spans from the edge of the ocean to further inland.", "southshore", "", "", "") }
            };

            this.Player = new Player("southshore");
        }

        public async Task RunExploration()
        {
            // The outermost exploration loop ought to be non-recursive, so players don't stack-overflow.
            while (true)
            {
                StoryHost.AdvancePage();
                Room currentRoom = Rooms[Player.CurrentRoomID];

                // Draw map
                Room[,] map = BuildMap(Player.CurrentRoomID);
                StoryHost.UpdateMap(map);
                
                // Write room description
                StoryHost.WriteLine(currentRoom.BodyText);

                await StoryHost.RunMenu(currentRoom.ListActionsHandler(this));
            }
        }

        private Room[,] BuildMap(string startingRoomID)
        {
            const int MAP_SIZE = 5;
            HashSet<string> mappedRooms = new HashSet<string>();
            Stack<(string, int, int)> nextRooms = new Stack<(string, int, int)>();

            Room[,] result = new Room[MAP_SIZE, MAP_SIZE];

            nextRooms.Push((startingRoomID, MAP_SIZE / 2, MAP_SIZE / 2)); // Start the room off in the center

            while (nextRooms.Count > 0)
            {
                (string roomID, int x, int y) = nextRooms.Pop();
                Room room = Rooms[roomID];
                result[x, y] = room;

                // Add the relevant children
                if (!String.IsNullOrEmpty(room.NorthRoomID) && !mappedRooms.Contains(room.NorthRoomID))
                {
                    mappedRooms.Add(room.NorthRoomID);
                    nextRooms.Push((room.NorthRoomID, x, y - 1));
                }
                if (!String.IsNullOrEmpty(room.SouthRoomID) && !mappedRooms.Contains(room.SouthRoomID))
                {
                    mappedRooms.Add(room.SouthRoomID);
                    nextRooms.Push((room.SouthRoomID, x, y + 1));
                }
                if (!String.IsNullOrEmpty(room.EastRoomID) && !mappedRooms.Contains(room.EastRoomID))
                {
                    mappedRooms.Add(room.EastRoomID);
                    nextRooms.Push((room.EastRoomID, x + 1, y));
                }
                if (!String.IsNullOrEmpty(room.WestRoomID) && !mappedRooms.Contains(room.WestRoomID))
                {
                    mappedRooms.Add(room.WestRoomID);
                    nextRooms.Push((room.WestRoomID, x - 1, y));
                }
            }

            return result;
        }

        public Task MoveToRoom(string roomID)
        {
            Player.CurrentRoomID = roomID;

            return Task.CompletedTask;
        }

        public IEnumerable<ActionMenuChoice> ListPlayerActions()
        {
            yield return new ActionMenuChoice("Inventory", async () => { StoryHost.WriteLine("Not yet implemented."); await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask)); });
            yield return new ActionMenuChoice("Team", async () => { StoryHost.WriteLine("Not yet implemented."); await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask)); });
        }
    }
}
