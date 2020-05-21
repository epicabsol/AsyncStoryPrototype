using AsyncStory.Core;
using AsyncStoryPrototype.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncStoryPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IStoryHost
    {
        private interface IWPFMenuTask
        {
            void Resume(MenuChoice result);
        }

        private class WPFMenuTask<TResult> : StoryTask<TResult>, IWPFMenuTask where TResult : MenuChoice
        {
            public void Resume(MenuChoice result)
            {
                this.Result = (TResult)result;
                this.Continuation();
            }
        }

        public StoryContext Context { get; private set; }

        private IWPFMenuTask PendingTask;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        #region IStoryHost Implementation
        public StoryTask<TChoice> ShowMenu<TChoice>(TChoice[] options) where TChoice : MenuChoice
        {
            if (PendingTask != null)
                throw new InvalidOperationException("Cannot show another menu while one is pending!");

            MenuItemsControl.ItemsSource = options;

            PendingTask = new WPFMenuTask<TChoice>();

            return (WPFMenuTask<TChoice>)PendingTask;
        }

        public async Task RunMenu(ActionMenuChoice[] choices)
        {
            ActionMenuChoice choice = await ShowMenu(choices);
            await choice.Invoke();
        }

        public void WriteLine(string text)
        {
            StoryTextBlock.Text += text + "\n";
        }

        public void AdvancePage()
        {
            StoryTextBlock.Text = "";
        }
        #endregion

        #region UI Events
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Context = new StoryContext(this);
            await Context.RunExploration();
        }

        private void MenuChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            MenuChoice o = (sender as Button).Tag as MenuChoice;

            IWPFMenuTask task = PendingTask;
            PendingTask = null;

            task.Resume(o);
        }
        #endregion
    }
}
