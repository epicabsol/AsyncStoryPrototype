using AsyncStory.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStoryPrototype.World
{
    public class StoryContext
    {
        public IStoryHost StoryHost { get; }
        public Player Player { get; }

        public bool HasGatheredStarfish { get; private set; } = false;

        public Dictionary<string, Room> Rooms { get; }

        public StoryContext(IStoryHost storyHost)
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
                StoryHost.WriteLine(currentRoom.BodyText);

                await StoryHost.RunMenu(currentRoom.ListActionsHandler(this));
            }
        }

        public Task MoveToRoom(string roomID)
        {
            Player.CurrentRoomID = roomID;

            return Task.CompletedTask;
        }
    }
}
