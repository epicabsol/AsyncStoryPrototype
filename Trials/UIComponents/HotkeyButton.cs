using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Trials.UIComponents
{
    public class HotkeyButton : Button
    {
        public static readonly DependencyProperty KeyCodeProperty = DependencyProperty.Register(nameof(KeyCode), typeof(Key), typeof(HotkeyButton), new PropertyMetadata());
        public Key KeyCode
        {
            get => (Key)GetValue(KeyCodeProperty);
            set => SetValue(KeyCodeProperty, value);
        }

        public static readonly DependencyProperty KeyNameProperty = DependencyProperty.Register(nameof(KeyName), typeof(string), typeof(HotkeyButton), new PropertyMetadata(""));
        public string KeyName
        {
            get => (string)GetValue(KeyNameProperty);
            set => SetValue(KeyNameProperty, value);
        }

        // TODO: Implement key listening
    }
}
