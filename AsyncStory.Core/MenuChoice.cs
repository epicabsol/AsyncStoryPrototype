using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStory.Core
{
    public enum MenuChoiceSemantic
    {
        None,
        MoveNorth,
        MoveSouth,
        MoveEast,
        MoveWest,
        MoveUp,
        MoveDown,
        Cancel,
        Continue,
    }

    public class MenuChoice
    {
        public string Text { get; }
        public MenuChoiceSemantic Semantic { get; }
        public Func<bool> IsEnabledHandler { get; }

        public bool IsEnabled => (IsEnabledHandler != null) ? IsEnabledHandler() : true;

        public MenuChoice(string text, MenuChoiceSemantic semantic = MenuChoiceSemantic.None, Func<bool> isEnabledHandler = null)
        {
            this.Text = text;
            this.Semantic = semantic;
            this.IsEnabledHandler = isEnabledHandler;
        }
    }
}
