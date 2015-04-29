using System.Data;
using System.Data.OleDb;
using ADODB;

namespace Rti.EagleIqGatewayServer.Utilities
{
    class Util
    {
        static public DataTable RecordSetToDataTable(Recordset recordSet, string dataSetName, string tableName)
        {
            var dataAdapter = new OleDbDataAdapter();
            var dataSet = new DataSet(dataSetName);
            var dataTable = new DataTable();

            dataAdapter.Fill(dataSet, recordSet, tableName);

            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[tableName];
            }

            return dataTable;
        }
    }
}
