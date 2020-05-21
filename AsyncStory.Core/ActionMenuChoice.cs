using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStory.Core
{
    public class ActionMenuChoice : MenuChoice
    {
        private Func<Task> Handler { get; }

        public ActionMenuChoice(string text, Func<Task> handler) : base(text)
        {
            this.Handler = handler;
        }

        public async Task Invoke()
        {
            if (Handler != null)
            {
                await Handler();
            }
        }
    }
}
