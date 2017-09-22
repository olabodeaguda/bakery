using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BakeryPR.Utilities
{
    public class GridViewColumnVisibilityManager
    {
        public static Visibility GetVisibility(DependencyObject o)
        {
            return (Visibility)o.GetValue(VisibilityProperty);
        }

        public static void SetVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(VisibilityProperty, value);
        }

        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.RegisterAttached("Visibility", typeof(Visibility),
            typeof(GridViewColumnVisibilityManager),
            new FrameworkPropertyMetadata(Visibility.Visible,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            new PropertyChangedCallback(OnVisibilityPropertyChanged)));

        private static void OnVisibilityPropertyChanged(DependencyObject d,
                                         DependencyPropertyChangedEventArgs e)
        {
            var column = d as GridViewColumn;
            if (column != null)
            {
                var visibility = GetVisibility(column);
                if (visibility == Visibility.Visible)
                {
                    // set the with back to the original
                    column.Width = GetVisibleWidth(column);
                }
                else
                {
                    // store the original width
                    SetVisibleWidth(column, column.Width);
                    // set the column width to 0 to hide it
                    column.Width = 0.0;
                }
            }
        }

        public static double GetVisibleWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(VisibleWidthProperty);
        }

        public static void SetVisibleWidth(DependencyObject obj, double value)
        {
            obj.SetValue(VisibleWidthProperty, value);
        }

        /// <summary>
        /// dpenendency property that stores the last visible width
        /// whenever the visibility changes this propert is used to set or get the width
        /// </summary>
        public static readonly DependencyProperty VisibleWidthProperty =
            DependencyProperty.RegisterAttached("VisibleWidth",
                    typeof(double),
                    typeof(GridViewColumnVisibilityManager),
                    new UIPropertyMetadata(double.NaN));

    }
}
