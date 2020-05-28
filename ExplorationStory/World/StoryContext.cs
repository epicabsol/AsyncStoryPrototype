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
        public bool HasMetStella { get; private set; } = false;

        public Dictionary<string, Room> Rooms { get; }

        public StoryContext(IExplorationStoryHost storyHost)
        {
            this.StoryHost = storyHost;

            Rooms = new Dictionary<string, Room>
            {
                { "southshore", new Room("southshore", "South Shore", "You are standing on the warm sands of an unknown island, amongst the smoldering wreckage of your escape pod.", "", "southpath", "", "",
                    new ActionMenuChoice("Gather Starfish", async () => { StoryHost.AdvancePage(); StoryHost.WriteLine("You pick up the starfish off the ground"); HasGatheredStarfish = true; await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask, MenuChoiceSemantic.Continue)); }, MenuChoiceSemantic.None, () => !HasGatheredStarfish)) },
                { "southpath", new Room("southpath", "South Path", "Tropical foliage graces the edges of a soft sandy path that spans from the edge of the ocean to further inland. You see that someone has made camp in a grassy field off to the side of the path.", "southshore", "", "", "southcamp") },
                { "southcamp", new Room("southcamp", "South Camp", "", "", "southpath", "", null, (context) => (context.HasMetStella ? "Stella" : "An unknown figure") + " is sharpening her blade by a campfire.",
                    new ActionMenuChoice("Approach", async () => {
                        StoryHost.AdvancePage();
                        if (this.HasMetStella)
                        {
                            StoryHost.WriteLine("You walk up to Stella.\n\nShe greets you with a nod.\n\n'What's up?' she asks.");
                        }
                        else
                        {
                            StoryHost.WriteLine("You walk over to the figure. She stops sharpening her weapon and looks straight at you.\n");
                            StoryHost.WriteLine("'Hello', you begin. 'I've crash-landed on this planet, and need to find transport to the Ning system as soon as possible.'\n");
                            StoryHost.WriteLine("'Well, you could have picked a better spot to crash. Nearest civilization is 1000 ku across the ocean,' she says. 'The only people on this island are wannabe adventurers trying to push through the Ancient Dungeon to supposed treasure'.");
                            StoryHost.WriteLine("'I've made a few attempts on the Dungeon myself, but no luck so far,' she continues.");
                            StoryHost.WriteLine("'You look like you've seen some adventure. Wanna team up?'");
                            this.HasMetStella = true;
                            await StoryHost.RunMenu(new ActionMenuChoice("Team Up", () => Task.CompletedTask, MenuChoiceSemantic.None));
                            StoryHost.AdvancePage();
                            StoryHost.WriteLine("'Great! Let me know when you want to give the ol' Dungeon a go and I'll get us on our way.'");
                            StoryHost.WriteLine("'Anything else I can do for you?'");
                        }
                        bool done = false;
                        while (!done)
                        {
                            await StoryHost.RunMenu(new ActionMenuChoice("Talk", async () =>
                            {
                                StoryHost.AdvancePage();
                                StoryHost.WriteLine("You consider what to ask Stella about.");
                                StoryHost.WriteLine("A few options come to mind.\n");
                                StoryHost.WriteLine("You could ask about how she learned of this island and its Dungeon,");
                                StoryHost.WriteLine("You could ask about the Ancient Dungeon itself,");
                                StoryHost.WriteLine("Or, you could ask about Stella's adventuring credentials.");
                                await StoryHost.RunMenu(new ActionMenuChoice("Dungeon Publicity", async () =>
                                {
                                    StoryHost.AdvancePage();
                                    StoryHost.WriteLine("'How did you come to know of this island, and the Ancient Dungeon?' you ask.\n");
                                    StoryHost.WriteLine("'I got caught up with an adventuring gang, we tried our luck in the Dungeon, and when the gang left the island, they tied me to a tree so I couldn't follow them. They thought I wasn't trying as hard as I could have been in the Dungeon, and that I was the reason the gang never found the treasure.'");
                                    await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask, MenuChoiceSemantic.Continue));
                                }, MenuChoiceSemantic.None), new ActionMenuChoice("Dungeon Details", async () =>
                                {
                                    StoryHost.AdvancePage();
                                    StoryHost.WriteLine("'What is this Dungeon, anyways?' you ask.\n");
                                    StoryHost.WriteLine("'The indigenous people in the area have all sorts of legends of its deep expanses, treacherous traps, and unimaginable treasure. They say it's been here for all of known history,' Stella replies.");
                                    await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask, MenuChoiceSemantic.Continue));
                                }, MenuChoiceSemantic.None),
                                new ActionMenuChoice("Credentials", async () =>
                                {
                                    StoryHost.AdvancePage();
                                    StoryHost.WriteLine("'So I take it you have some adventuring experience?' you ask.\n");
                                    StoryHost.WriteLine("'Yeah, I ran adventures with a gang, 'till they dumped me here and left,' Stella replies.");
                                    await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask, MenuChoiceSemantic.Continue));
                                }, MenuChoiceSemantic.None));
                            }, MenuChoiceSemantic.None), new ActionMenuChoice("Done", () => { done = true; return Task.CompletedTask; }, MenuChoiceSemantic.Cancel));
                            if (!done)
                            {
                                StoryHost.AdvancePage();
                                StoryHost.WriteLine("You and Stella are standing around her campfire.");
                            }
                        }
                    }, MenuChoiceSemantic.None)) }
            };

            this.Player = new Player("southshore");
        }

        public async Task RunExploration()
        {
            // The outermost exploration loop ought to be non-recursive, so players don't stack-overflow.
            while (true)
            {
                Room currentRoom = Rooms[Player.CurrentRoomID];

                // Draw map
                Room[,] map = BuildMap(Player.CurrentRoomID);
                StoryHost.UpdateMap(map);

                // Clear
                StoryHost.AdvancePage();
                
                // Write room description
                StoryHost.WriteLine(currentRoom.GetBodyText(this));

                await StoryHost.RunMenu(currentRoom.ListActions(this));
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
            yield return new ActionMenuChoice("Inventory", async () => { StoryHost.WriteLine("Not yet implemented."); await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask, MenuChoiceSemantic.Continue)); }, MenuChoiceSemantic.None);
            yield return new ActionMenuChoice("Team", async () => { StoryHost.WriteLine("Not yet implemented."); await StoryHost.RunMenu(new ActionMenuChoice("Continue", () => Task.CompletedTask, MenuChoiceSemantic.Continue)); }, MenuChoiceSemantic.None);
        }
    }
}
