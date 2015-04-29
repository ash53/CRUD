namespace Rti.CRUDTool
{
    partial class CRUDTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRUDTool));
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Insert");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Read");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Update");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Delete");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("About");
            this.tabCrudTool = new System.Windows.Forms.TabControl();
            this.tabCrudPage = new System.Windows.Forms.TabPage();
            this.lblEditable = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblRed = new System.Windows.Forms.Label();
            this.lblGreen = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvLoadTable = new System.Windows.Forms.DataGridView();
            this.lblFoundRec = new System.Windows.Forms.Label();
            this.btnCrudInsertSave = new System.Windows.Forms.Button();
            this.cboCrudSearchColumn = new System.Windows.Forms.ComboBox();
            this.lblCrudSearch = new System.Windows.Forms.Label();
            this.btnCrudSearch = new System.Windows.Forms.Button();
            this.txtCrudSearch = new System.Windows.Forms.TextBox();
            this.cboRecordPerPage = new System.Windows.Forms.ComboBox();
            this.lblSelectRecordno = new System.Windows.Forms.Label();
            this.grpBoxSelectDB = new System.Windows.Forms.GroupBox();
            this.btnCRUDLoadTable = new System.Windows.Forms.Button();
            this.cboSelectTable = new System.Windows.Forms.ComboBox();
            this.lblSelectTable = new System.Windows.Forms.Label();
            this.cboSelectUser = new System.Windows.Forms.ComboBox();
            this.lblSelectUser = new System.Windows.Forms.Label();
            this.cboSelectDB = new System.Windows.Forms.ComboBox();
            this.lblSelectDB = new System.Windows.Forms.Label();
            this.tabViewAbout = new System.Windows.Forms.TabPage();
            this.lblAbout = new System.Windows.Forms.Label();
            this.treeViewMenu = new System.Windows.Forms.TreeView();
            this.lblCrudAdmin = new System.Windows.Forms.Label();
            this.tabCrudTool.SuspendLayout();
            this.tabCrudPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadTable)).BeginInit();
            this.grpBoxSelectDB.SuspendLayout();
            this.tabViewAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCrudTool
            // 
            this.tabCrudTool.Controls.Add(this.tabCrudPage);
            this.tabCrudTool.Controls.Add(this.tabViewAbout);
            this.tabCrudTool.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabCrudTool.Location = new System.Drawing.Point(86, 0);
            this.tabCrudTool.Name = "tabCrudTool";
            this.tabCrudTool.SelectedIndex = 0;
            this.tabCrudTool.Size = new System.Drawing.Size(901, 581);
            this.tabCrudTool.TabIndex = 0;
            // 
            // tabCrudPage
            // 
            this.tabCrudPage.BackColor = System.Drawing.Color.White;
            this.tabCrudPage.Controls.Add(this.lblCrudAdmin);
            this.tabCrudPage.Controls.Add(this.lblEditable);
            this.tabCrudPage.Controls.Add(this.lblRequired);
            this.tabCrudPage.Controls.Add(this.lblRed);
            this.tabCrudPage.Controls.Add(this.lblGreen);
            this.tabCrudPage.Controls.Add(this.btnCancel);
            this.tabCrudPage.Controls.Add(this.btnDelete);
            this.tabCrudPage.Controls.Add(this.dgvLoadTable);
            this.tabCrudPage.Controls.Add(this.lblFoundRec);
            this.tabCrudPage.Controls.Add(this.btnCrudInsertSave);
            this.tabCrudPage.Controls.Add(this.cboCrudSearchColumn);
            this.tabCrudPage.Controls.Add(this.lblCrudSearch);
            this.tabCrudPage.Controls.Add(this.btnCrudSearch);
            this.tabCrudPage.Controls.Add(this.txtCrudSearch);
            this.tabCrudPage.Controls.Add(this.cboRecordPerPage);
            this.tabCrudPage.Controls.Add(this.lblSelectRecordno);
            this.tabCrudPage.Controls.Add(this.grpBoxSelectDB);
            this.tabCrudPage.Location = new System.Drawing.Point(4, 22);
            this.tabCrudPage.Name = "tabCrudPage";
            this.tabCrudPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabCrudPage.Size = new System.Drawing.Size(893, 555);
            this.tabCrudPage.TabIndex = 0;
            this.tabCrudPage.Text = "crudPage";
            // 
            // lblEditable
            // 
            this.lblEditable.AutoSize = true;
            this.lblEditable.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEditable.Location = new System.Drawing.Point(741, 521);
            this.lblEditable.Name = "lblEditable";
            this.lblEditable.Size = new System.Drawing.Size(70, 15);
            this.lblEditable.TabIndex = 16;
            this.lblEditable.Text = "- Do not edit";
            this.lblEditable.Visible = false;
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequired.Location = new System.Drawing.Point(741, 495);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(88, 15);
            this.lblRequired.TabIndex = 15;
            this.lblRequired.Text = "- Required Field";
            this.lblRequired.Visible = false;
            // 
            // lblRed
            // 
            this.lblRed.AutoSize = true;
            this.lblRed.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRed.ForeColor = System.Drawing.Color.LightCoral;
            this.lblRed.Location = new System.Drawing.Point(704, 520);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(30, 16);
            this.lblRed.TabIndex = 14;
            this.lblRed.Text = "Red";
            this.lblRed.Visible = false;
            // 
            // lblGreen
            // 
            this.lblGreen.AutoSize = true;
            this.lblGreen.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreen.ForeColor = System.Drawing.Color.YellowGreen;
            this.lblGreen.Location = new System.Drawing.Point(704, 494);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(42, 16);
            this.lblGreen.TabIndex = 13;
            this.lblGreen.Text = "Green";
            this.lblGreen.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(104, 506);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(23, 506);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvLoadTable
            // 
            this.dgvLoadTable.AllowUserToAddRows = false;
            this.dgvLoadTable.AllowUserToDeleteRows = false;
            this.dgvLoadTable.AllowUserToOrderColumns = true;
            this.dgvLoadTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoadTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvLoadTable.Location = new System.Drawing.Point(3, 123);
            this.dgvLoadTable.Name = "dgvLoadTable";
            this.dgvLoadTable.Size = new System.Drawing.Size(890, 346);
            this.dgvLoadTable.TabIndex = 10;
            this.dgvLoadTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLoadTable_CellClick);
            this.dgvLoadTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvLoadTable_ColumnHeaderMouseClick);
            this.dgvLoadTable.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvLoadTable_DataBindingComplete);
            this.dgvLoadTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLoadTable_DataError);
            this.dgvLoadTable.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvLoadTable_RowPostPaint);
            // 
            // lblFoundRec
            // 
            this.lblFoundRec.AutoSize = true;
            this.lblFoundRec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFoundRec.Location = new System.Drawing.Point(11, 483);
            this.lblFoundRec.Name = "lblFoundRec";
            this.lblFoundRec.Size = new System.Drawing.Size(0, 15);
            this.lblFoundRec.TabIndex = 9;
            this.lblFoundRec.Visible = false;
            // 
            // btnCrudInsertSave
            // 
            this.btnCrudInsertSave.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnCrudInsertSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCrudInsertSave.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrudInsertSave.ForeColor = System.Drawing.Color.White;
            this.btnCrudInsertSave.Location = new System.Drawing.Point(14, 506);
            this.btnCrudInsertSave.Name = "btnCrudInsertSave";
            this.btnCrudInsertSave.Size = new System.Drawing.Size(75, 30);
            this.btnCrudInsertSave.TabIndex = 8;
            this.btnCrudInsertSave.Text = "Save";
            this.btnCrudInsertSave.UseVisualStyleBackColor = false;
            this.btnCrudInsertSave.Visible = false;
            this.btnCrudInsertSave.Click += new System.EventHandler(this.btnCrudInsertSave_Click);
            // 
            // cboCrudSearchColumn
            // 
            this.cboCrudSearchColumn.FormattingEnabled = true;
            this.cboCrudSearchColumn.Location = new System.Drawing.Point(650, 41);
            this.cboCrudSearchColumn.Name = "cboCrudSearchColumn";
            this.cboCrudSearchColumn.Size = new System.Drawing.Size(161, 21);
            this.cboCrudSearchColumn.TabIndex = 7;
            this.cboCrudSearchColumn.Click += new System.EventHandler(this.cboCrudSearchColumn_Click);
            // 
            // lblCrudSearch
            // 
            this.lblCrudSearch.AutoSize = true;
            this.lblCrudSearch.Location = new System.Drawing.Point(459, 41);
            this.lblCrudSearch.Name = "lblCrudSearch";
            this.lblCrudSearch.Size = new System.Drawing.Size(128, 13);
            this.lblCrudSearch.TabIndex = 6;
            this.lblCrudSearch.Text = "Select Column To Search";
            // 
            // btnCrudSearch
            // 
            this.btnCrudSearch.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnCrudSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCrudSearch.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrudSearch.ForeColor = System.Drawing.Color.White;
            this.btnCrudSearch.Location = new System.Drawing.Point(650, 76);
            this.btnCrudSearch.Name = "btnCrudSearch";
            this.btnCrudSearch.Size = new System.Drawing.Size(75, 29);
            this.btnCrudSearch.TabIndex = 5;
            this.btnCrudSearch.Text = "Search";
            this.btnCrudSearch.UseVisualStyleBackColor = false;
            this.btnCrudSearch.Click += new System.EventHandler(this.btnCrudSearch_Click);
            // 
            // txtCrudSearch
            // 
            this.txtCrudSearch.Location = new System.Drawing.Point(462, 80);
            this.txtCrudSearch.Name = "txtCrudSearch";
            this.txtCrudSearch.Size = new System.Drawing.Size(182, 20);
            this.txtCrudSearch.TabIndex = 4;
            // 
            // cboRecordPerPage
            // 
            this.cboRecordPerPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecordPerPage.FormattingEnabled = true;
            this.cboRecordPerPage.Items.AddRange(new object[] {
            "500",
            "1000",
            "1500",
            "2500",
            "3500",
            "5000",
            "All"});
            this.cboRecordPerPage.Location = new System.Drawing.Point(650, 14);
            this.cboRecordPerPage.Name = "cboRecordPerPage";
            this.cboRecordPerPage.Size = new System.Drawing.Size(57, 21);
            this.cboRecordPerPage.TabIndex = 3;
            // 
            // lblSelectRecordno
            // 
            this.lblSelectRecordno.AutoSize = true;
            this.lblSelectRecordno.Location = new System.Drawing.Point(458, 17);
            this.lblSelectRecordno.Name = "lblSelectRecordno";
            this.lblSelectRecordno.Size = new System.Drawing.Size(76, 13);
            this.lblSelectRecordno.TabIndex = 2;
            this.lblSelectRecordno.Text = "View Records ";
            // 
            // grpBoxSelectDB
            // 
            this.grpBoxSelectDB.Controls.Add(this.btnCRUDLoadTable);
            this.grpBoxSelectDB.Controls.Add(this.cboSelectTable);
            this.grpBoxSelectDB.Controls.Add(this.lblSelectTable);
            this.grpBoxSelectDB.Controls.Add(this.cboSelectUser);
            this.grpBoxSelectDB.Controls.Add(this.lblSelectUser);
            this.grpBoxSelectDB.Controls.Add(this.cboSelectDB);
            this.grpBoxSelectDB.Controls.Add(this.lblSelectDB);
            this.grpBoxSelectDB.Location = new System.Drawing.Point(3, 6);
            this.grpBoxSelectDB.Name = "grpBoxSelectDB";
            this.grpBoxSelectDB.Size = new System.Drawing.Size(434, 100);
            this.grpBoxSelectDB.TabIndex = 0;
            this.grpBoxSelectDB.TabStop = false;
            this.grpBoxSelectDB.Text = "DB";
            // 
            // btnCRUDLoadTable
            // 
            this.btnCRUDLoadTable.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnCRUDLoadTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCRUDLoadTable.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCRUDLoadTable.ForeColor = System.Drawing.Color.White;
            this.btnCRUDLoadTable.Location = new System.Drawing.Point(290, 65);
            this.btnCRUDLoadTable.Name = "btnCRUDLoadTable";
            this.btnCRUDLoadTable.Size = new System.Drawing.Size(75, 29);
            this.btnCRUDLoadTable.TabIndex = 6;
            this.btnCRUDLoadTable.Text = "Load Table";
            this.btnCRUDLoadTable.UseVisualStyleBackColor = false;
            this.btnCRUDLoadTable.Click += new System.EventHandler(this.btnCRUDLoadTable_Click);
            // 
            // cboSelectTable
            // 
            this.cboSelectTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectTable.FormattingEnabled = true;
            this.cboSelectTable.Location = new System.Drawing.Point(92, 70);
            this.cboSelectTable.Name = "cboSelectTable";
            this.cboSelectTable.Size = new System.Drawing.Size(125, 21);
            this.cboSelectTable.TabIndex = 5;
            this.cboSelectTable.SelectedIndexChanged += new System.EventHandler(this.cboSelectTable_SelectedIndexChanged);
            this.cboSelectTable.Click += new System.EventHandler(this.cboSelectTable_Click);
            // 
            // lblSelectTable
            // 
            this.lblSelectTable.AutoSize = true;
            this.lblSelectTable.Location = new System.Drawing.Point(0, 70);
            this.lblSelectTable.Name = "lblSelectTable";
            this.lblSelectTable.Size = new System.Drawing.Size(67, 13);
            this.lblSelectTable.TabIndex = 4;
            this.lblSelectTable.Text = "Select Table";
            // 
            // cboSelectUser
            // 
            this.cboSelectUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectUser.FormattingEnabled = true;
            this.cboSelectUser.Location = new System.Drawing.Point(290, 27);
            this.cboSelectUser.Name = "cboSelectUser";
            this.cboSelectUser.Size = new System.Drawing.Size(138, 21);
            this.cboSelectUser.TabIndex = 3;
            this.cboSelectUser.SelectedIndexChanged += new System.EventHandler(this.cboSelectUser_SelectedIndexChanged);
            this.cboSelectUser.Click += new System.EventHandler(this.cboSelectUser_Click);
            // 
            // lblSelectUser
            // 
            this.lblSelectUser.AutoSize = true;
            this.lblSelectUser.Location = new System.Drawing.Point(222, 27);
            this.lblSelectUser.Name = "lblSelectUser";
            this.lblSelectUser.Size = new System.Drawing.Size(62, 13);
            this.lblSelectUser.TabIndex = 2;
            this.lblSelectUser.Text = "Select User";
            // 
            // cboSelectDB
            // 
            this.cboSelectDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectDB.FormattingEnabled = true;
            this.cboSelectDB.Location = new System.Drawing.Point(92, 27);
            this.cboSelectDB.Name = "cboSelectDB";
            this.cboSelectDB.Size = new System.Drawing.Size(125, 21);
            this.cboSelectDB.TabIndex = 1;
            this.cboSelectDB.SelectedIndexChanged += new System.EventHandler(this.cboSelectDB_SelectedIndexChanged);
            // 
            // lblSelectDB
            // 
            this.lblSelectDB.AutoSize = true;
            this.lblSelectDB.Location = new System.Drawing.Point(0, 27);
            this.lblSelectDB.Name = "lblSelectDB";
            this.lblSelectDB.Size = new System.Drawing.Size(86, 13);
            this.lblSelectDB.TabIndex = 0;
            this.lblSelectDB.Text = "Select Database";
            // 
            // tabViewAbout
            // 
            this.tabViewAbout.BackColor = System.Drawing.Color.White;
            this.tabViewAbout.Controls.Add(this.lblAbout);
            this.tabViewAbout.Location = new System.Drawing.Point(4, 22);
            this.tabViewAbout.Name = "tabViewAbout";
            this.tabViewAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabViewAbout.Size = new System.Drawing.Size(893, 555);
            this.tabViewAbout.TabIndex = 1;
            this.tabViewAbout.Text = "About";
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout.Location = new System.Drawing.Point(17, 17);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(761, 290);
            this.lblAbout.TabIndex = 0;
            this.lblAbout.Text = resources.GetString("lblAbout.Text");
            // 
            // treeViewMenu
            // 
            this.treeViewMenu.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.treeViewMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewMenu.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewMenu.ForeColor = System.Drawing.SystemColors.Window;
            this.treeViewMenu.LineColor = System.Drawing.Color.White;
            this.treeViewMenu.Location = new System.Drawing.Point(0, 0);
            this.treeViewMenu.Name = "treeViewMenu";
            treeNode6.Name = "NodeCrudInsert";
            treeNode6.Text = "Insert";
            treeNode7.Name = "NodeCrudRead";
            treeNode7.Text = "Read";
            treeNode8.Name = "NodeCrudUpdate";
            treeNode8.Text = "Update";
            treeNode9.Name = "NodeCrudDelete";
            treeNode9.Text = "Delete";
            treeNode10.Name = "abt";
            treeNode10.Text = "About";
            this.treeViewMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            this.treeViewMenu.Size = new System.Drawing.Size(87, 581);
            this.treeViewMenu.TabIndex = 11;
            // 
            // lblCrudAdmin
            // 
            this.lblCrudAdmin.AutoSize = true;
            this.lblCrudAdmin.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCrudAdmin.ForeColor = System.Drawing.Color.Maroon;
            this.lblCrudAdmin.Location = new System.Drawing.Point(332, 512);
            this.lblCrudAdmin.Name = "lblCrudAdmin";
            this.lblCrudAdmin.Size = new System.Drawing.Size(208, 15);
            this.lblCrudAdmin.TabIndex = 17;
            this.lblCrudAdmin.Text = "**You are logged in as CRUD admin";
            this.lblCrudAdmin.Visible = false;
            // 
            // CRUDTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 581);
            this.Controls.Add(this.treeViewMenu);
            this.Controls.Add(this.tabCrudTool);
            this.Name = "CRUDTool";
            this.Text = "CRUD Tool";
            this.tabCrudTool.ResumeLayout(false);
            this.tabCrudPage.ResumeLayout(false);
            this.tabCrudPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadTable)).EndInit();
            this.grpBoxSelectDB.ResumeLayout(false);
            this.grpBoxSelectDB.PerformLayout();
            this.tabViewAbout.ResumeLayout(false);
            this.tabViewAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCrudTool;
        private System.Windows.Forms.TabPage tabCrudPage;
        private System.Windows.Forms.TabPage tabViewAbout;
        private System.Windows.Forms.TreeView treeViewMenu;
        private System.Windows.Forms.GroupBox grpBoxSelectDB;
        private System.Windows.Forms.ComboBox cboSelectUser;
        private System.Windows.Forms.Label lblSelectUser;
        private System.Windows.Forms.ComboBox cboSelectDB;
        private System.Windows.Forms.Label lblSelectDB;
        private System.Windows.Forms.Label lblSelectTable;
        private System.Windows.Forms.ComboBox cboSelectTable;
        private System.Windows.Forms.Button btnCRUDLoadTable;
        private System.Windows.Forms.ComboBox cboRecordPerPage;
        private System.Windows.Forms.Label lblSelectRecordno;
        private System.Windows.Forms.Button btnCrudSearch;
        private System.Windows.Forms.TextBox txtCrudSearch;
        private System.Windows.Forms.ComboBox cboCrudSearchColumn;
        private System.Windows.Forms.Label lblCrudSearch;
        private System.Windows.Forms.Button btnCrudInsertSave;
        private System.Windows.Forms.Label lblFoundRec;
        private System.Windows.Forms.DataGridView dgvLoadTable;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.Label lblGreen;
        private System.Windows.Forms.Label lblEditable;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.Label lblCrudAdmin;

    }
}

