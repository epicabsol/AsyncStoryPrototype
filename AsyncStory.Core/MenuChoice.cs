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
        public Func<bool> IsEnabledHandler { get; }

        public bool IsEnabled => (IsEnabledHandler != null) ? IsEnabledHandler() : true;

        public MenuChoice(string text, Func<bool> isEnabledHandler = null)
        {
            this.Text = text;
            this.IsEnabledHandler = isEnabledHandler;
        }
    }
}
