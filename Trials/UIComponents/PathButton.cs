using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Trials.UIComponents
{
    public class PathButton : Button
    {
        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register(nameof(PathData), typeof(Geometry), typeof(PathButton), new PropertyMetadata());
        public Geometry PathData
        {
            get => (Geometry)GetValue(PathDataProperty);
            set => SetValue(PathDataProperty, value);
        }

        public static readonly DependencyProperty PathFillProperty = DependencyProperty.Register(nameof(PathFill), typeof(Brush), typeof(PathButton), new PropertyMetadata());
        public Brush PathFill
        {
            get => (Brush)GetValue(PathFillProperty);
            set => SetValue(PathFillProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PathButton), new PropertyMetadata());
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty PathScaleProperty = DependencyProperty.Register(nameof(PathScale), typeof(double), typeof(PathButton), new PropertyMetadata(1.0));
        public double PathScale
        {
            get => (double)GetValue(PathScaleProperty);
            set => SetValue(PathScaleProperty, value);
        }

        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));
        }
    }
}
