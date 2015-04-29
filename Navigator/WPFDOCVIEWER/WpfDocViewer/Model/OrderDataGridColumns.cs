using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace WpfDocViewer.Model
{
   public class OrderDataGridColumns
    {
       /// <summary>
       /// Mark Lane
       /// 2/17/2015 use this to allow saving and ordering of columns based
       /// on user preferences.
       /// if there is no string "," to reorder it will not reorder but return the original.
       /// </summary>
       /// <param name="TableToReorder"></param>
       /// <returns></returns>
       public DataTable Reorder(DataTable TableToReorder)
       {
           try
           {
               int originalColumns = 0;
               var splitstring = Properties.Settings.Default["SaveColumnOrder"].ToString().Split(',');

               foreach (string columnName in splitstring)
               {
                   TableToReorder.Columns[columnName].SetOrdinal(originalColumns);
                   originalColumns++;
               }    
           }
           catch(Exception ex)
           {
               
           }
           return TableToReorder;
       }

       public DataTable ReorderNavC2PColumns(DataTable TableToReorder)
       {
           try
           {
               int originalColumns = 0;
               var splitstring = Properties.Settings.Default["SaveNavC2PColumnOrder"].ToString().Split(',');

               foreach (string columnName in splitstring)
               {
                   TableToReorder.Columns[columnName].SetOrdinal(originalColumns);
                   originalColumns++;
               }
           }
           catch (Exception ex)
           {

           }
           return TableToReorder;
       }
       public DataTable ReorderEmbillzColumns(DataTable TableToReorder)
       {
           try
           {
               int originalColumns = 0;
               var splitstring = Properties.Settings.Default["SaveEmbillzColumnOrder"].ToString().Split(',');

               foreach (string columnName in splitstring)
               {
                   TableToReorder.Columns[columnName].SetOrdinal(originalColumns);
                   originalColumns++;
               }
           }
           catch (Exception ex)
           {

           }
           return TableToReorder;
       }

       public void ClearOrder()
       {
           Properties.Settings.Default["SaveColumnOrder"] = "";
           Properties.Settings.Default.Save();
       }
    }
}
