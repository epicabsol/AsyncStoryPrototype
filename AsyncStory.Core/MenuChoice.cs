using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStory.Core
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
