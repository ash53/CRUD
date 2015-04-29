using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfDocViewer.ViewModel
{
    /// <summary>
    /// Author: Mark Lane
    /// Keep math functions in the view model
    /// </summary>
    class MathClass
    {

        /// <summary>
        /// Round for resizing
        /// </summary>
        /// Mark Lane keep all the math functions in one class
        /// <param name="p"></param>
        /// <returns></returns>
        public static int Round(double p)
        {
            return Convert.ToInt32(Math.Round(p, 0));
        }
    }
}
