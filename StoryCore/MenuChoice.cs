using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStoryPrototype.StoryCore
{
    public class MenuChoice
    {
        public string Text { get; }

        public MenuChoice(string text)
        {
            this.Text = text;
        }
    }
}
