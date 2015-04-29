using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDocViewer.Model
{
   public class EnumerateSearch
    {
      
       public enum Get500More
       {
           FirstRow = 500,
           SecondRow = 1000,
           ThirdRow = 1500,
           FourthRow = 2000,
           FifthRow = 2500,
           SixthRow = 3000,
           SeventhRow = 3500,
           EighthRow = 4000,
           NinthRow = 4500,
           TenthRow = 5000
       }
       //1 based
       public int GetStart500(int IterateSearch)
       {
           IterateSearch--;
           return IterateSearch * 500;
       }
       //range to search on
       public int GetEnd500(int IterateSearch)
       {
           return 500;
       }
    }
}
