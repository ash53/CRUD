using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentViewer
{
    public static class StringExtensions
    {
        public static IEnumerable<string> Split(this string s, string startToken, string endToken)
        {
            int index = s.IndexOf(startToken);

            //If no startToken just return the entire string
            if (index == -1)
                return new List<string>() { s };

            var list = new List<string>();

            do
            {
                //Move the index to after the start token so we do not return it
                index += startToken.Length;

                //Throw away everything before the token. The string will shrink as we build our list. 
                s = s.Substring(index, s.Length - index);

                //Find the next startToken
                index = s.IndexOf(startToken);

                string item;

                //Get all the text up to the next token and add it to the list. If no token add all the text.
                if(index > 0)
                {
                    item = s.Substring(0, index + startToken.Length);
                }
                else
                {
                    item = s;
                }

                //Find the end token (if it exists) and add the text before it.
                int endIndex = item.IndexOf(endToken);

                if(endIndex != -1)
                {
                    item = item.Substring(0, endIndex);
                }

                if(string.IsNullOrEmpty(item) == false)
                {
                    list.Add(item);
                }

            } while (index != -1);

            return list;
        }
    }
}
