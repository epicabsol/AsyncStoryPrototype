using System;
using System.Threading.Tasks;
using System.Collections.Generic;
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
                Dictionary<string, MenuChoice> choices = new Dictionary<string, MenuChoice>();
                foreach (MenuChoice choice in Options)
                {
                    string key = (i + 1).ToString();
                    int colorIndex = 255;
                    if (!choice.IsEnabled)
                    {
                        key = "".PadLeft(key.Length, ' ');
                        colorIndex = 246;
                    }
                    else if (choice.Semantic == MenuChoiceSemantic.MoveNorth)
                    {
                        key = "W";
                    }
                    else if (choice.Semantic == MenuChoiceSemantic.MoveSouth)
                    {
                        key = "S";
                    }
                    else if (choice.Semantic == MenuChoiceSemantic.MoveEast)
                    {
                        key = "D";
                    }
                    else if (choice.Semantic == MenuChoiceSemantic.MoveWest)
                    {
                        key = "A";
                    }
                    else
                    {
                        i++;
                    }

                    using (ColorContext cc = new ColorContext(colorIndex))
                    {
                        Console.WriteLine(key + ": " + choice.Text);
                    }
                    choices.Add(key, choice);
                }

                string input = "";
                while (!choices.ContainsKey(input))
                {
                    Console.Write("? ");
                    input = Console.ReadLine().ToUpperInvariant();
                }
                this.Resume(choices[input]);
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
