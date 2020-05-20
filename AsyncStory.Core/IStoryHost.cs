using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStory.Core
{
    /// <summary>
    /// The interface implemented by a User Interface that hosts a story.
    /// </summary>
    public interface IStoryHost
    {
        /// <summary>
        /// Presents a menu to the user and returns the choice that was chosen.
        /// </summary>
        /// <param name="options">The possible choices for the user to choose from.</param>
        /// <returns>The choice that was chosen.</returns>
        StoryTask<TChoice> ShowMenu<TChoice>(TChoice[] choices) where TChoice : MenuChoice;

        /// <summary>
        /// Presents a menu of actions to the user, and runs the one that is chosen.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task RunMenu(ActionMenuChoice[] options);

        /// <summary>
        /// Writes a line of text to the output.
        /// </summary>
        /// <param name="text">The text to write.</param>
        void WriteLine(string text);

        /// <summary>
        /// Advances the text output to the next page.
        /// 
        /// This should clear the visible text output.
        /// </summary>
        void AdvancePage();
    }
}
