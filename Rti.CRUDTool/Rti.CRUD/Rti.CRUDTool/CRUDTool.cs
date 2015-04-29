using System;
using System.Linq;
using System.Windows.Forms;
using Rti.DataModel;
using System.Drawing;
using System.Data.Entity;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Data.Linq.SqlClient;
using Rti.EncryptionLib;
using System.Configuration;


namespace Rti.CRUDTool
{
public partial class CRUDTool : Form
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CadAdminModelContainer cadContext= new CadAdminModelContainer();
        RtTransBrokerAdminModelContainer rtContext= new RtTransBrokerAdminModelContainer();
        TowerModelContainer twrContext=new TowerModelContainer();
        BindingSource crudBindingSource = new BindingSource();
        CheckBox chkbox = new CheckBox();
        static int crudCount = 0; //keeps count if user has already entered credentials for accessing CRUDRULES table
        //int otherTable;
       
        public CRUDTool()
        {
            InitializeComponent();
            //removes the header of the tab control
            tabCrudTool.Appearance = TabAppearance.FlatButtons;
            tabCrudTool.ItemSize = new Size(0, 1);
            tabCrudTool.SizeMode = TabSizeMode.Fixed;
            //When the app loads up for the first time, initially select the tabPage for CRUD operation
            tabCrudTool.SelectedTab = tabCrudPage;
            //Node mouse click event for treeViewMenu
            this.treeViewMenu.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeViewMenu_NodeMouseClick);
            this.dgvLoadTable.EditingControlShowing += HandleEditShowing;
            this.dgvLoadTable.CellValueChanged += new DataGridViewCellEventHandler(dgvLoadTable_CellValueChanged);
            this.dgvLoadTable.CellValidating +=new DataGridViewCellValidatingEventHandler(dgvLoadTable_CellValidating);

            try
            {
                //load database names from table CRUDRULES in cboSelectDB
                cboSelectDB.Items.Clear(); //Clears the item with each click, so that same items are not loaded again
                var cboDbItem = (from a in twrContext.CRUDRULES where a.DBNAME != null select a.DBNAME).Distinct().OrderBy(x => x);
                foreach (var dbName in cboDbItem)
                    cboSelectDB.Items.Add(dbName);
            }
            catch (Exception ex)
            { MessageBox.Show("Error:" + ex); }
        }
        //end of CRUDTool() function
        /**********************************************************************************************************/

        //function for adding item to cboSelectUser when a database is selected in cboSelectDB
        private void cboSelectDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                cboSelectUser.SelectedIndex = -1; //clears combobox text upon new database selection
                cboSelectUser.Items.Clear();
                //Loads items in cboSelectUser when database in cboSelectDB is selected
                var cboSelectUserItem = (from a in twrContext.CRUDRULES where a.RTISCHEMA != null && a.DBNAME == cboSelectDB.Text select a.RTISCHEMA).Distinct().OrderBy(x => x);
                foreach (var schemaName in cboSelectUserItem)
                    cboSelectUser.Items.Add(schemaName);
            }
            catch(Exception ex)
            { MessageBox.Show("Error:"+ex); }
            
        }
        //end of cboSelectDB_SelectedIndexChanged()
        /*********************************************************************************/

        //function for adding items in cboSelectTable according to the selected user
        private void cboSelectUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                cboSelectTable.SelectedIndex = -1; //clears combobox text upon new selection
                cboSelectTable.Items.Clear();
                //loads tablenames in cmbSelectTable upon selecting DB name and user name
                var cboSelectTableItem = (from a in twrContext.CRUDRULES
                                          where a.RTITABLE != null && a.DBNAME == cboSelectDB.Text && a.RTISCHEMA == cboSelectUser.Text
                                          select a.RTITABLE).Distinct().OrderBy(x => x);
                foreach (var tableName in cboSelectTableItem)
                    cboSelectTable.Items.Add(tableName);
            }
            catch (Exception ex)
            { MessageBox.Show("Error:" + ex); }
            
        }//end of cboSelectUser_SelectedIndexChanged()
        /*****************************************************************************/

        //notify user to select DB first before selecting user name
        private void cboSelectUser_Click(object sender, EventArgs e)
        {
            
            if (cboSelectDB.SelectedIndex <= -1)
                MessageBox.Show("Please select database first");
        }//end of cboSelectUser_Click()
        /****************************************************************************/

        //notify user to select DB/user first before selecting table name
        private void cboSelectTable_Click(object sender, EventArgs e)
        {
            
            if (cboSelectDB.SelectedIndex<=-1 || cboSelectUser.SelectedIndex<=-1)
                MessageBox.Show("Please select database or user first");
        }//end of cboSelectTable_Click()
        /****************************************************************************/

        //notify user to load a table first before selecting column for search
        private void cboCrudSearchColumn_Click(object sender, EventArgs e)
        {

            if (cboSelectTable.SelectedIndex <= -1)
                MessageBox.Show("Please select a table and load it first");
        }//end of cboCrudSearchColumn_Click()
        /****************************************************************************/

        //This function displays serial number in datagridview dgvLoadTable
        private void dgvLoadTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvLoadTable.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }//end of dgvLoadTable_RowPostPaint()
        /****************************************************************************/

        //Function for loading data in datagridview dgvLoadTable
        private void crudLoadData(string RecordNum)
        {
            var context = new object();
            var TableName = cboSelectTable.Text.ToString();
            
            //CADADMIN@DEVDB01
            if (cboSelectUser.Text.ToString() == "CADADMIN" && cboSelectDB.Text.ToString() == "DEVDB01")
            {
               cadContext = new CadAdminModelContainer();
               context = cadContext; 
            }
            //RTTRANSBROKERADMIN@DEVDB01
            else if (cboSelectUser.Text.ToString() == "RTTRANSBROKERADMIN" && cboSelectDB.Text.ToString() == "DEVDB01")
            {
                rtContext = new RtTransBrokerAdminModelContainer();
                context = rtContext;
            }
            //TOWER@TWRPROD.RTI.COM
            else if (cboSelectUser.Text.ToString() == "TOWER" && cboSelectDB.Text.ToString() == "RTITSTA")
            {
                twrContext = new TowerModelContainer();
                context = twrContext;
            }
            try
            {
                var truncatedData = new object();
                var rawData = context.GetType().GetProperty(TableName).GetValue(context, null);
                if (RecordNum == "All")
                {
                    truncatedData = ((IQueryable<object>)rawData).ToList();
                }
                else
                {
                    int RecNo = Int32.Parse(RecordNum);
                    truncatedData = ((IQueryable<object>)rawData).Take(RecNo).ToList();
                }
                crudBindingSource.DataSource = new BindingSource { DataSource = truncatedData };
                dgvLoadTable.DataSource = crudBindingSource;
                dgvLoadTable.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//end of crudLoadData()
        /*****************************************************************************************************************/

        //Function for "Load Table" button click
        private void btnCRUDLoadTable_Click(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            btnCrudInsertSave.Visible = false;
            btnDelete.Visible = false;
            lblGreen.Visible = false;
            lblRed.Visible = false;
            lblRequired.Visible = false;
            lblEditable.Visible = false;
            //Automatically select item when nothing is selected in cboRecordPerPage
            if (cboRecordPerPage.SelectedIndex <= -1)
                cboRecordPerPage.SelectedIndex = 0;
            string RecordNo = cboRecordPerPage.Text.ToString();
            //prompt user when "Load Table" button is clicked without selecting any database or table or user 
            if (cboSelectDB.SelectedIndex <= -1 || cboSelectUser.SelectedIndex <= -1 || cboSelectTable.SelectedIndex <= -1)
            {
                MessageBox.Show("Please select database, user and table first");
            }
            else
            {
                dgvLoadTable.DataSource = null;
                dgvLoadTable.Columns.Clear();// clear any existing column
                clearSelection();
                dgvLoadTable.AllowUserToAddRows = false;
                addChkBox(); //call function to add checkboxes
                crudLoadData(RecordNo); //call function to load data in dgvLoadTable
                
            }//end of else statement
        }//end of btnCRUDLoadTable_Click()
        /****************************************************************************************************************************/
        //makes the combobox columns in datagridview editable
        private void HandleEditShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var cbo = e.Control as ComboBox;
            if (cbo == null)
            {
                return;
            }

            cbo.DropDownStyle = ComboBoxStyle.DropDown;
            cbo.Validating -= HandleComboBoxValidating;
            cbo.Validating += HandleComboBoxValidating;

        }//end of HandleEditShowing()

        
        private void HandleComboBoxValidating(object sender, CancelEventArgs e)
        {
            var combo = sender as DataGridViewComboBoxEditingControl;
            if (combo == null)
            {
                return;
            }
            if (!combo.Items.Contains(combo.Text)) //check if item is already in drop down, if not, add it to all
            {
                var comboColumn = this.dgvLoadTable.Columns[this.dgvLoadTable.CurrentCell.ColumnIndex] as DataGridViewComboBoxColumn;
                combo.Items.Add(combo.Text);
                comboColumn.Items.Add(combo.Text);
                this.dgvLoadTable.CurrentCell.Value = combo.Text;
            }
        }//end of HandleComboBoxValidating()
        /************************************************************************************************************************************************/
        //Function for making datagridview combobox column
        private void dgvCboColumn(dynamic item, string colName)
        {
            int i = dgvLoadTable.Columns[colName].Index;
            DataGridViewComboBoxColumn dgvCol = new DataGridViewComboBoxColumn();
            foreach (var rec in item)
                dgvCol.Items.Add(rec);
            dgvCol.DataPropertyName = colName;
            dgvLoadTable.Columns.Insert(i, dgvCol);
            dgvLoadTable.Columns[i].HeaderText = dgvLoadTable.Columns[i + 1].HeaderText;
            dgvLoadTable.Columns[i + 1].Visible = false;
            dgvLoadTable.Columns.RemoveAt(i + 1);
            var isRequired = from a in twrContext.CRUDRULES where a.RTICOLUMN == colName && a.ISCOMBOBOXCOLUMN == "Y" select a.ISREQUIRED;
            if(isRequired.Contains("Y"))
                dgvLoadTable.Columns[i].DefaultCellStyle.BackColor = Color.YellowGreen;
            var canEdit = from a in twrContext.CRUDRULES where a.RTICOLUMN == colName && a.ISCOMBOBOXCOLUMN == "Y" select a.CANEDIT;
            if (canEdit.Contains("N"))
                dgvLoadTable.Columns[i].DefaultCellStyle.BackColor = Color.LightCoral;
        }//end of  dgvCboColumn()
        /********************************************************************************************************/
        
        //Function for data insertion
        public void crudInsert()
        {
            
            //prompt user to select database,table and user 
            if (cboSelectDB.SelectedIndex <= -1 || cboSelectUser.SelectedIndex <= -1 || cboSelectTable.SelectedIndex <= -1)
            {
                MessageBox.Show("Please select database, user and table first and then click insert");
            }
            else
            {
                try
                {
                    checkCrudrules();
                    if (cboSelectTable.Text.ToString() == "CRUDRULES" && crudCount==0)
                    {}
                    else
                    {
                        dgvLoadTable.DataSource = null;
                        dgvLoadTable.Columns.Clear();// clear any existing column
                        addChkBox();
                        crudLoadData("0"); //Calls function to load data in datagridview and recordNum is set to 0 for showing the header only in dgvLoadTable
                        dgvLoadTable.ReadOnly = false;
                        dgvLoadTable.AllowUserToAddRows = true;
                        crudLocalBinding();
                        setCboColumns();
                        setNonEditableColumnColor();
                    }
                 }//end of try block
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }//end of else statement
        }//end of crudInsert()
        /*******************************************************************************************************************************************/
        //Function for setting combo box columns in the gridview
        private void setCboColumns()
        {
            var entityModel=new object();
            try
            {
                if (cboSelectUser.Text.ToString() == "CADADMIN")
                {
                    entityModel = new CadAdminModelContainer();
                }
                else if (cboSelectUser.Text.ToString() == "RTTRANSBROKERADMIN")
                {
                    entityModel = new RtTransBrokerAdminModelContainer();
                }
                else if (cboSelectUser.Text.ToString() == "TOWER")
                {
                    entityModel = new TowerModelContainer();
                }
                //get the table name from comboBox 'Select Table'    
                string tableName = cboSelectTable.Text.ToString();
                var comboBoxColumn = (from a in twrContext.CRUDRULES where a.ISCOMBOBOXCOLUMN == "Y" && a.RTITABLE==cboSelectTable.Text select a.RTICOLUMN);
                foreach (var colName in comboBoxColumn)
                {
                    var getTableName = entityModel.GetType().GetProperty(tableName).GetValue(entityModel, null);
                    var getData = ((IQueryable<object>)getTableName).Where(colName + "!=null").Select(colName).Distinct().OrderBy("it");
                    dgvCboColumn(getData, colName);
                }
                dgvLoadTable.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex);
            }
        }
        /*********************************************************************************************************************************************************************/
        //treeViewMenu click event
        private void treeViewMenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                switch (e.Node.Index)
                {
                    case 0: //create
                        tabCrudTool.SelectedTab = tabCrudPage; //selects the tabCrudPage of tabCrudTool
                        lblFoundRec.Visible = false;
                        btnCrudInsertSave.Visible = true;
                        btnDelete.Visible = false;
                        btnCancel.Visible = true;
                        clearSelection();
                        crudInsert(); //calls function for insertion operation
                        break;
                    case 1: //read
                        tabCrudTool.SelectedTab = tabCrudPage; //selects the tabCrudPage of tabCrudTool
                        lblFoundRec.Visible = false;
                        lblGreen.Visible = false;
                        lblRed.Visible = false;
                        lblRequired.Visible = false;
                        lblEditable.Visible = false;
                        btnCrudInsertSave.Visible = false;
                        btnDelete.Visible = false;
                        btnCancel.Visible = false;
                        dgvLoadTable.DataSource = null;
                        dgvLoadTable.Controls.Clear();
                        dgvLoadTable.Columns.Clear();
                        dgvLoadTable.Rows.Clear();
                        break;
                    case 2: //update
                        tabCrudTool.SelectedTab = tabCrudPage; //selects the tabCrudPage of tabCrudTool
                        lblFoundRec.Visible = false;
                        btnCrudInsertSave.Visible = true;
                        btnDelete.Visible = false;
                        btnCancel.Visible = true;
                        if (dgvLoadTable.SelectedRows.Count > 0)
                            crudUpdate();
                        else
                            MessageBox.Show("Please select records to update.");
                        break;
                    case 3://delete
                        tabCrudTool.SelectedTab = tabCrudPage; //selects the tabCrudPage of tabCrudTool
                        crudLocalBinding();
                        lblFoundRec.Visible = false;
                        lblGreen.Visible = false;
                        lblRed.Visible = false;
                        lblRequired.Visible = false;
                        lblEditable.Visible = false;
                        btnCrudInsertSave.Visible = false;
                        btnDelete.Visible = true;
                        btnCancel.Visible = true;
                        break;
                    case 4: //General information about CRUDTool
                        tabCrudTool.SelectedTab = tabViewAbout;
                        break;
                    default:
                        break;
                } //end of switch 
            }//end of try block
            catch (Exception nodeEx)
            {
                MessageBox.Show("Error: " + nodeEx);
            }
        }//end of treeViewMenu click event
        /****************************************************************************************************************************/
       
        //Search operation on dgvLoadTable
         private void btnCrudSearch_Click(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            btnCrudInsertSave.Visible = false;
            btnDelete.Visible = false;
            lblGreen.Visible = false;
            lblRed.Visible = false;
            lblRequired.Visible = false;
            lblEditable.Visible = false;
            //check if value is given for search
             if (cboCrudSearchColumn.SelectedIndex != -1 && txtCrudSearch.Text != string.Empty)
            {
                dgvLoadTable.CurrentCell = null;
                dgvLoadTable.AllowUserToAddRows = false;
                try
                {
                    addChkBox();
                    var contextSearch = new object(); ///variable for using the EDM entity
                    var TableName = cboSelectTable.Text.ToString();
                    var columnName = cboCrudSearchColumn.Text.ToString();
                    string searchValue = txtCrudSearch.Text.ToString();
                    var truncatedData = new object();
                    int n, c=0;
                    //String dataType="N";
                    bool isNumeric = int.TryParse(searchValue, out n);
                    DateTime temp;
                    var checkColType= (from colType in twrContext.CRUDRULES where colType.RTICOLUMN==cboCrudSearchColumn.Text.ToLower() &&
                                         colType.RTISCHEMA==cboSelectUser.Text && colType.RTITABLE==cboSelectTable.Text &&
                                         colType.COLUMNTYPE!=null select colType.COLUMNTYPE).FirstOrDefault();
                    
                    
                    //CADADMIN@DEVDB01
                    if (cboSelectUser.Text.ToString() == "CADADMIN" && cboSelectDB.Text.ToString() == "DEVDB01")
                    {
                        cadContext = new CadAdminModelContainer();
                        contextSearch = cadContext;
                    }
                    //RTTRANSBROKERADMIN@DEVDB01
                    else if (cboSelectUser.Text.ToString() == "RTTRANSBROKERADMIN" && cboSelectDB.Text.ToString() == "DEVDB01")
                    {
                        rtContext = new RtTransBrokerAdminModelContainer();
                        contextSearch = rtContext;
                    }
                    //TOWER@TWRPROD.RTI.COM
                    else if (cboSelectUser.Text.ToString() == "TOWER" && cboSelectDB.Text.ToString() == "RTITSTA")
                    {
                        twrContext = new TowerModelContainer();
                        contextSearch = twrContext;
                    }

                    var rawData = contextSearch.GetType().GetProperty(TableName).GetValue(contextSearch, null);

                    if (isNumeric && checkColType.Contains("NUMBER"))
                    {
                        int x = Int32.Parse(txtCrudSearch.Text);
                            truncatedData = ((IQueryable<object>)rawData).Where(columnName + "=@0", x).ToList();
                     }

                    else if (DateTime.TryParse(txtCrudSearch.Text, out temp) && checkColType.Contains("DATE"))
                    {
                        var parsedDt = DateTime.Parse(txtCrudSearch.Text);
                        var nextDay = parsedDt.AddDays(1);
                        truncatedData = ((IQueryable<object>)rawData).Where(columnName + ">= @0 && " + columnName + " < @1", parsedDt, nextDay).ToList();
                    }
                    else
                    {

                        truncatedData = ((IQueryable<object>)rawData).Where(columnName + "=@0", searchValue).ToList();

                    }

                    crudBindingSource.DataSource = new BindingSource { DataSource = truncatedData };
                    dgvLoadTable.DataSource = crudBindingSource;
                    dgvLoadTable.Refresh();

                    //dgvLoadTable.DataSource = truncatedData;
                    txtCrudSearch.Text = String.Empty;
                    foreach (DataGridViewRow row in dgvLoadTable.Rows)
                    { c++; }
                    if (c>0)
                    {
                        lblFoundRec.Visible = true;
                        lblFoundRec.Text = c + " records found";
                    }
                    else
                    {
                        lblFoundRec.Visible = false;
                        MessageBox.Show("No records are found.");
                    }
                    
                }//end of try block
             catch (Exception exc)
                        {
                           // MessageBox.Show(exc.Message);
                        }
                
                
                    }
                    else
                        MessageBox.Show("Please select column name and enter search value.");

        }//end of btnCrudSearch_Click()

         
         
        /*****************************************************************************************************/
        //autocomplete combobox---> cboCrudSearchColumn textChanged event
        private void cboCrudSearchColumn_TextChanged(object sender, EventArgs e)
        {
            int itemsIndex = 0;
            foreach (string item in cboCrudSearchColumn.Items)
            {
                if (item.IndexOf(cboCrudSearchColumn.Text) == 0)
                {
                    cboCrudSearchColumn.SelectedIndex = itemsIndex;
                    cboCrudSearchColumn.Select(cboCrudSearchColumn.Text.Length - 1, 0);
                    break;
                }
                itemsIndex++;
            }//end of for-each
        }//end of cboCrudSearchColumn textChanged event
        /***************************************************************************************************************/
       
        //Save inserted data
        private void btnCrudInsertSave_Click(object sender, EventArgs e)
        {
            //checks if any data is given as input before clicking the save button
            try
            {
                int rowNum = dgvLoadTable.CurrentCellAddress.Y;
                if (rowNum == -1)
                {
                    MessageBox.Show("There is no row selected!");
                    return;
                }

                //check if row is empty, simply return
                if (IsRowEmpty(rowNum))
                {
                    MessageBox.Show("There is no data in the selected row!");
                    return;
                }
                if (cboSelectUser.Text.ToString() == "RTTRANSBROKERADMIN")
                    rtContext.SaveChanges();
                else if (cboSelectUser.Text.ToString() == "CADADMIN")
                    cadContext.SaveChanges();
                else if (cboSelectUser.Text.ToString() == "TOWER")
                    twrContext.SaveChanges();
                MessageBox.Show("Records saved successfully.");
            }//end of try
            catch (Exception saveEx)
            {
                MessageBox.Show("Error: " + saveEx);
            }
        }//end of function btnCrudInsertSave_Click
        /******************************************************************************************/
        //function for adding new object in the entity model
        void crudLocalBinding()
        {
            var entity = new object();
            var tableName = cboSelectTable.Text.ToString();
            try
            {
                if (cboSelectUser.Text.ToString() == "RTTRANSBROKERADMIN")
                {
                    switch (cboSelectTable.Text.ToString())
                    {
                        case "RTTRANS_HL7_FTP_CONFIG":
                            crudBindingSource.DataSource = rtContext.RTTRANS_HL7_FTP_CONFIG.Local.ToBindingList();
                            break;
                        case "ETL_CONFIG":
                            crudBindingSource.DataSource = rtContext.ETL_CONFIG.Local.ToBindingList();
                            break;
                        case "CUSTOMIZESEGMENTS":
                            crudBindingSource.DataSource = rtContext.CUSTOMIZESEGMENTS.Local.ToBindingList();
                            break;
                        case "FILTERHL7":
                            crudBindingSource.DataSource = rtContext.FILTERHL7.Local.ToBindingList();
                            break;
                    }//end of switch
                   
                }//end of if user is RTTRANSBROKERADMIN
                if (cboSelectUser.Text.ToString() == "CADADMIN")
                {
                    switch (cboSelectTable.Text.ToString())
                    {
                        case "TOWERIMPORTs":
                            crudBindingSource.DataSource = cadContext.TOWERIMPORTs.Local.ToBindingList();
                            break;
                        case "HL7_FTP_CONFIG":
                            crudBindingSource.DataSource = cadContext.HL7_FTP_CONFIG.Local.ToBindingList();
                            break;
                        case "EMSCAN_BATCH":
                            crudBindingSource.DataSource = cadContext.EMSCAN_BATCH.Local.ToBindingList();
                            break;
                        case "TOWERIMPORTCLIENTs":
                            crudBindingSource.DataSource = cadContext.TOWERIMPORTCLIENTs.Local.ToBindingList();
                            break;
                        case "RTI_FILETYPE":
                            crudBindingSource.DataSource = cadContext.RTI_FILETYPE.Local.ToBindingList();
                            break;
                        case "RTI_ACCTSCENARIO":
                            crudBindingSource.DataSource = cadContext.RTI_ACCTSCENARIO.Local.ToBindingList();
                            break;

                    }//end of switch
                }//end of if user is CADADMIN
                if (cboSelectUser.Text.ToString() == "TOWER")
                {
                     switch (cboSelectTable.Text.ToString())
                     { 
                         case "RTICLIENTs":
                             crudBindingSource.DataSource = twrContext.RTICLIENTs.Local.ToBindingList();
                             break;
                         case "CRUDRULES":
                             crudBindingSource.DataSource = twrContext.CRUDRULES.Local.ToBindingList();
                             break;
                         case "BPRRULES":
                             crudBindingSource.DataSource = twrContext.BPRRULES.Local.ToBindingList();
                             break;
                     }
                    
                }
                
            }
            catch (Exception exAdd)
                    { MessageBox.Show("Error" + exAdd); }//end of catch block
        }//end of function crudBindingSource_AddingNew()
        /*****************************************************************************************************/
        //method to check if a row is empty in datagridview
        private bool IsRowEmpty(int index)
        {
            return dgvLoadTable.Rows[index].Cells.OfType<DataGridViewCell>()
                                            .All(c => c.Value == null);
        }
        
        /*******************************************************************************************/
        //make the databound comboboxes editable
        private void dgvLoadTable_EditingControlShowing(object sender,DataGridViewEditingControlShowingEventArgs e)
        {
            //autocomplete mode for all comboboxes in dgvLoadTable
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            }
        }//end of function dgvLoadTable_EditingControlShowing
        /*****************************************************************************************/
        //add checkbox column and select the checked rows
        private void addChkBox()
        {
            //checkbox
            //Below i create on check box column in the datagrid view
            dgvLoadTable.Columns.Clear();
            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            //set name for the check box column
            colCB.Name = "Select";
            colCB.HeaderText = "";
            colCB.Width = 20;
            //If you use header check box then use it 
            colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLoadTable.Columns.Add(colCB);
            //Select cell where checkbox to be display
            Rectangle rect = this.dgvLoadTable.GetCellDisplayRectangle(0, -1, true);            //0 Column index -1(header row) is row index 
            //Mention size of the checkbox
            chkbox.Size = new Size(15, 15);
            //set position of header checkbox where to places
            rect.Offset(3, 3);
            chkbox.Location = rect.Location;
            chkbox.CheckedChanged += chkBoxChange;
            //Add CheckBox control to datagridView
            this.dgvLoadTable.Controls.Add(chkbox);
        }
        private void chkBoxChange(object sender, EventArgs e)
        {
            for (int k = 0; k <= dgvLoadTable.RowCount - 1; k++)
            {
                this.dgvLoadTable[0, k].Value = this.chkbox.Checked;
                
            }
            
            this.dgvLoadTable.EndEdit();
        }

        private void dgvLoadTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLoadTable.Columns[0].Name == "Select")
            {
                foreach (DataGridViewRow row in dgvLoadTable.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[0].Value.Equals(true)) 
                    {
                        row.Selected = true;
                        row.DefaultCellStyle.SelectionBackColor = Color.Gray;
                    }
                    else
                        row.Selected = false;
                }//end of foreach
            }//end of if the first column of datagridview is the checkbox column
        }

      
        /***********************************************************************************************************************/
       //function for showing the records to be updated
        private void crudUpdate()
        {
            checkCrudrules();
            if (cboSelectTable.Text.ToString() == "CRUDRULES" && crudCount == 0)
            {
                
            }
            else
            {
                foreach (DataGridViewRow row in dgvLoadTable.Rows)
                {
                    if (row.Selected == true)
                    {
                        row.Visible = true;

                    }
                    else row.Visible = false;
                }
                setCboColumns();
                setNonEditableColumnColor();
                lblGreen.Visible = false;
                lblRed.Visible = false;
                lblRequired.Visible = false;
                lblEditable.Visible = false;
            }
        }//end of crudUpdate()
        /************************************************************************************/
        //function for deletion
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                checkCrudrules();
                if (cboSelectTable.Text.ToString() == "CRUDRULES" && crudCount == 0)
                {
                    
                }
                else
                {
                    if (dgvLoadTable.SelectedRows.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the selected records?", "To delete select Yes", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            foreach (DataGridViewRow row in this.dgvLoadTable.SelectedRows)
                            {
                                if (row.Cells["Select"].Value != null && row.Cells["Select"].Value.Equals(true))
                                {
                                    crudBindingSource.RemoveAt(row.Index);

                                }
                            }//end of foreach
                            if (cboSelectUser.Text.ToString() == "RTTRANSBROKERADMIN")
                                rtContext.SaveChanges();
                            else if (cboSelectUser.Text.ToString() == "CADADMIN")
                                cadContext.SaveChanges();
                            else if (cboSelectUser.Text.ToString() == "TOWER")
                                twrContext.SaveChanges();
                            if (dgvLoadTable.SelectedRows.Count == dgvLoadTable.Rows.Count)
                            {
                                dgvLoadTable.DataSource = null;
                                dgvLoadTable.Controls.Clear();
                            }
                            MessageBox.Show("Records Deleted!");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            clearSelection();
                        }
                    }//end of if any row is selected

                    else
                        MessageBox.Show("Please select the column you want to delete.");
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Error:" + ex); }
       }//end of btnDelete_Click()

        
       /*****************************************************************************************************************/
        //function for clearing any selection
        void clearSelection()
        {
            DataGridViewSelectionMode oldmode = dgvLoadTable.SelectionMode;

            dgvLoadTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvLoadTable.ClearSelection();

            dgvLoadTable.SelectionMode = oldmode;
        }//end of clearSelection()
        /*******************************************************************************************/   
        //Function for validating datagridview input
        private void dgvLoadTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
            
        }

        private void dgvLoadTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // though this works, but sometimes give unwanted notification while inserting data, as it validates data while entering. It will be better to find 
            //another working way

            MessageBox.Show(this, e.Exception.Message, "Error");
            e.ThrowException = false;
            e.Cancel = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvLoadTable.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel operation for the selected records?", "To cancel select Yes", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in this.dgvLoadTable.SelectedRows)
                    {
                        if (row.Cells["Select"].Value != null && row.Cells["Select"].Value.Equals(true))
                            dgvLoadTable.Rows.RemoveAt(row.Index);
                    }//end of foreach
                }
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to cancel all rows? If you want to cancel particular rows, then select them. To cancel all select Yes and to select particular rows select No.", "To cancel select Yes", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvLoadTable.DataSource = null;
                    dgvLoadTable.Columns.Clear();// clear any existing column
                    dgvLoadTable.Rows.Clear();
                    dgvLoadTable.Controls.Clear();
                }
            }
        }

        private void dgvLoadTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgvLoadTable.Rows)
            {
                if (row.IsNewRow)
                    row.Selected = true;
            }

            if (dgvLoadTable.CurrentRow.Cells[e.ColumnIndex].ReadOnly)
            {
                MessageBox.Show("This cell can not be edited.");
            }
            
        }


        
        private void dgvLoadTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //bool isSortAscending = false;
                crudLocalBinding();
                DataGridViewColumn newColumn = dgvLoadTable.Columns[e.ColumnIndex];
                DataGridViewColumn oldColumn = dgvLoadTable.SortedColumn;
                ListSortDirection direction;

                // If oldColumn is null, then the DataGridView is not sorted. 
                if (oldColumn != null)
                {
                    // Sort the same column again, reversing the SortOrder. 
                    if (oldColumn == newColumn &&
                        dgvLoadTable.SortOrder == SortOrder.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        // Sort a new column and remove the old SortGlyph.
                        direction = ListSortDirection.Ascending;
                        oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    }
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }

                // Sort the selected column.
                dgvLoadTable.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                    direction == ListSortDirection.Ascending ?
                    SortOrder.Ascending : SortOrder.Descending;
            }
            catch (Exception ex)
            { MessageBox.Show("Error:" + ex); }
        }

        private void cboSelectTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboCrudSearchColumn.SelectedIndex = -1;
            cboCrudSearchColumn.Items.Clear();
            var cboSearchItem= (from item in twrContext.CRUDRULES where item.RTITABLE==cboSelectTable.Text && item.RTISCHEMA==cboSelectUser.Text && item.RTICOLUMN!=null 
                                select item.RTICOLUMN);
            foreach (var searchColumnName in cboSearchItem)
                cboCrudSearchColumn.Items.Add(searchColumnName.ToUpper());
        }

        private void dgvLoadTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            crudLocalBinding();
            // Put each of the columns into programmatic sort mode. 
            foreach (DataGridViewColumn column in dgvLoadTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

       private void setNonEditableColumnColor()
        {
            try
            {
                lblGreen.Visible = true;
                lblRed.Visible = true;
                lblRequired.Visible = true;
                lblEditable.Visible = true;
                //non-editable columns
                var coloredColumnEdit = (from col in twrContext.CRUDRULES
                                     where col.CANEDIT == "N" && col.RTITABLE == cboSelectTable.Text && col.RTISCHEMA == cboSelectUser.Text
                                         && col.RTICOLUMN != null /*&& col.ISCOMBOBOXCOLUMN=="N"*/
                                     select col.RTICOLUMN);
                foreach (var column in coloredColumnEdit)
                {
                    dgvLoadTable.Columns[column].DefaultCellStyle.BackColor = Color.LightCoral;
                    dgvLoadTable.Columns[column].ReadOnly = true;
                    //canEdit = true;
                }
                //required columns
                var coloredColumnRequired = (from col in twrContext.CRUDRULES
                                             where col.ISREQUIRED == "Y" && col.RTITABLE == cboSelectTable.Text && col.RTISCHEMA == cboSelectUser.Text
                                                 && col.RTICOLUMN != null && col.ISCOMBOBOXCOLUMN == "N"
                                             select col.RTICOLUMN);
                foreach (var column in coloredColumnRequired)
                {
                    dgvLoadTable.Columns[column].DefaultCellStyle.BackColor = Color.YellowGreen;
                }

            }
            catch (Exception ex)
            { MessageBox.Show("Error:" + ex); }
            
        }

      public void checkCrudrules()
       {
           if (cboSelectTable.Text.ToString() == "CRUDRULES")
           {
               if (crudCount == 0)
               {
                   Credential frm = new Credential();
                   if (frm.ShowDialog() != DialogResult.OK)
                   {
                       // The user canceled.
                       frm.Close();
                   }
                   else
                   { 
                       crudCount++;
                       lblCrudAdmin.Visible = true;
                   }
               }
           }
           //else otherTable = 1;
       }

        
   }

}
