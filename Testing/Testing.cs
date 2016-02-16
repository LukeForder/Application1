using Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Xunit;

namespace Testing
{
    public class Testing
    {
        [WpfFact]
        public void it_can_set_the_text_through_the_front_end()
        {
            var dictionary = (ResourceDictionary)Application.LoadComponent(new Uri("/Controls;component/MyResources.xaml", UriKind.Relative));

            var window = new Window();

            window.Resources.MergedDictionaries.Add(dictionary);

            var presenter = new ContentControl();
            var viewModel = new MyViewModel(); ;
            presenter.Content = viewModel;

            window.Content = presenter;

            presenter.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            presenter.Arrange(new Rect(window.DesiredSize));

            var child = FindVisualChildren<TextBox>(presenter).First();
            
            child.Text = "text";

            Assert.Equal("text", viewModel.Text);
            

        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent)
       where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                var childType = child as T;
                if (childType != null)
                {
                    yield return (T)child;
                }

                foreach (var other in FindVisualChildren<T>(child))
                {
                    yield return other;
                }
            }
        }
    }
}
