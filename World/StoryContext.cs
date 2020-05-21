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
                StoryHost.AdvancePage();
                StoryHost.WriteLine("You are standing on the warm sands of an unknown island, amongst the smoldering wreckage of your escape pod.");
                StoryHost.WriteLine("Your sidekick and childhood friend Emma is scavenging through the debris.\n");
                StoryHost.WriteLine("She finds a loaf of bread, tears it in two, and hands you one of the pieces.\n");
                StoryHost.WriteLine("'Well?', Emma asks.");

                ActionMenuChoice choice = await StoryHost.ShowMenu(
                    new ActionMenuChoice("Eat Bread", async () => 
                    { 
                        StoryHost.WriteLine("You eat the bread.\n\n'Are you going to answer me?', Emma asks.");
                        await StoryHost.RunMenu(new ActionMenuChoice("Ignore Her", null));
                    }),
                    new ActionMenuChoice("Ask Emma", async () =>
                    {
                        StoryHost.WriteLine("You ask Emma what to do.");
                        StoryHost.WriteLine("'Well, we should probably search for water, or try to start a fire,' Emma says.");
                        await StoryHost.RunMenu(
                            new ActionMenuChoice("Why?", async () =>
                            {
                                StoryHost.WriteLine("'Why is that?', you ask her.");
                                StoryHost.WriteLine("'Are you stupid or something?', she replies.");
                                await StoryHost.RunMenu(new ActionMenuChoice("Sorry", null));
                            }),
                            new ActionMenuChoice("OK", async () => { StoryHost.WriteLine("'Sounds good to me', you reply."); await StoryHost.RunMenu(new ActionMenuChoice("Return", null)); }));
                    }));

                await choice.Invoke();
            }
        }
    }
}
