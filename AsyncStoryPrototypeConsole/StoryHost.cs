using System;
using System.Threading.Tasks;
using AsyncStory.Core;
using ExplorationStory;
using ExplorationStory.World;

namespace AsyncStoryPrototypeConsole
{
    public class StoryHost : IExplorationStoryHost
    {
        private class ConsoleMenuTask<TResult> : StoryTask<TResult> where TResult : MenuChoice
        {
            private TResult[] Options { get; }

            public ConsoleMenuTask(TResult[] options)
            {
                this.Options = options;
            }

            public void Resume(MenuChoice result)
            {
                this.Result = (TResult)result;
                this.Continuation();
            }

            public override void OnCompleted(Action continuation)
            {
                base.OnCompleted(continuation);

                int i = 0;
                Console.WriteLine();
                foreach (MenuChoice choice in Options)
                {
                    if (!choice.IsEnabled)
                    {
                        using (ColorContext cc = new ColorContext(246))
                        {
                            Console.WriteLine("   " + choice.Text);
                        }
                    }
                    else
                    {
                        Console.WriteLine((i + 1) + ": " + choice.Text);
                    }

                    i++;
                }

                Console.Write("? ");
                if (Int32.TryParse(Console.ReadLine(), out int choiceIndex))
                {
                    Console.WriteLine();
                    this.Resume(Options[choiceIndex - 1]);
                }
            }
        }

        public StoryContext StoryContext { get; private set; }

        private Room[,] Map;

        public StoryHost()
        {
            StoryContext = new StoryContext(this);
        }

        public async void RunStory()
        {
            await StoryContext.RunExploration();
        }

        private void PrintMap()
        {
            for (int line = 0; line < Map.GetLength(1); line++)
            {
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    if (Map[x, line] != null)
                    {
                        ColorContext cc = null;

                        if (Map[x, line].ID != StoryContext.Player.CurrentRoomID)
                            cc = new ColorContext(247);

                        Console.Write("[" + (Map[x, line].ID == StoryContext.Player.CurrentRoomID ? "X" : " ") + "]");

                        cc?.Dispose();
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }
                Console.WriteLine();
            }
        }

        #region IStoryHost Implementation
        public StoryTask<TChoice> ShowMenu<TChoice>(TChoice[] options) where TChoice : MenuChoice
        {
            return new ConsoleMenuTask<TChoice>(options);
        }

        public async Task RunMenu(ActionMenuChoice[] choices)
        {
            ActionMenuChoice choice = await ShowMenu(choices);
            await choice.Invoke();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void AdvancePage()
        {
            Console.Clear();
            PrintMap();
        }
        #endregion

        #region IExplorationStoryHost Implementation
        public void UpdateMap(Room[,] map)
        {
            this.Map = map;
            //PrintMap();
        }
        #endregion
    }
}
