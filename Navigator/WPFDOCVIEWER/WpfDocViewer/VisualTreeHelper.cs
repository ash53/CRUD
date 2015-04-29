using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfDocViewer
{
    /// <summary>
    /// Mark Lane
    /// wow amazing code!!! allows you to traverse the objects in any container. 
    /// can navigate through the visual tree this way using linq or lambda.
    /// http://geekswithblogs.net/codingbloke/archive/2010/12/19/visual-tree-enumeration.aspx
    /// </summary>
    public static class VisualTreeEnumeration
    {
        public static IEnumerable<DependencyObject> Descendents(this DependencyObject root, int depth)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                yield return child;
                if (depth > 0)
                {
                    foreach (var descendent in Descendents(child, --depth))
                        yield return descendent;
                }
            }
        }

        public static IEnumerable<DependencyObject> Descendents(this DependencyObject root)
        {
            return Descendents(root, Int32.MaxValue);
        }

        public static IEnumerable<DependencyObject> Ancestors(this DependencyObject root)
        {
            DependencyObject current = VisualTreeHelper.GetParent(root);
            while (current != null)
            {
                yield return current;
                current = VisualTreeHelper.GetParent(current);
            }
        }
    }

}