using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStoryPrototype.StoryCore
{
    public class ActionMenuChoice : MenuChoice
    {
        public Func<Task> Handler { get; }

        public ActionMenuChoice(string text, Func<Task> handler) : base(text)
        {
            this.Handler = handler;
        }
    }
}
