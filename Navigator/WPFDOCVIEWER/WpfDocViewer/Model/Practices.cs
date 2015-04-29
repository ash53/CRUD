using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
   public static class Practices
    {
       private static List<string> practiceList = new List<string>();
       private static List<string> practiceDivisionList = new List<string>();

       public static List<string> PracticeList
       {
           get { return practiceList; }
           set { practiceList = value; }
       }
       public static List<string> PracticeDivisionList
       {
           get { return practiceDivisionList; }
           set { practiceDivisionList = value; }
       }
    }
}
