using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfDocViewer.ViewModel
{
    public static class SearchTypes
    {
        /// <summary>
        /// Shared class for Quick Search to Main Search UserControl
        /// </summary>
        /// <returns></returns>
        public static List<string> SearchableTypes()
        {
            List<string> searchabletypes = new List<string>();
            searchabletypes.Add("Account");
            searchabletypes.Add("Docno");
            searchabletypes.Add("Parent Docno");
            searchabletypes.Add("Check Amount");
            searchabletypes.Add("Check Number");
            return searchabletypes;
        }
        public static string SearchValue
        {
            get;
            set;
        }
 
        public static Dictionary<string, string> SearchKeyValue(string key, string value)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(key, value);
            return dictionary;
        }
    }
}
