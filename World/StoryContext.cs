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
        public Player Player { get; set; }

        public StoryContext(IStoryHost storyHost)
        {
            this.StoryHost = storyHost;
            this.Player = new Player();
        }

        public async Task RunExploration()
        {
            // The outermost exploration loop ought to be non-recursive, so players don't stack-overflow.
            while (true)
            {
                StoryHost.WriteLine("[Room Description]");

                ActionMenuChoice choice = await StoryHost.ShowMenu(
                    new ActionMenuChoice("Eat Bread", () => { StoryHost.WriteLine("You eat the bread."); return Task.CompletedTask; }));

                await choice.Handler();
            }
        }
    }
}
