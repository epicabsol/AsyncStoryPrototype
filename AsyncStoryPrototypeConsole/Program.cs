using System;

namespace AsyncStoryPrototypeConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StoryHost host = new StoryHost();
            host.RunStory();
        }
    }
}
