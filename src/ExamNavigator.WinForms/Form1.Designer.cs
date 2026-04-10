namespace ExamNavigator.WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.searchLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblSearchField = new System.Windows.Forms.Label();
            this.cmbSearchField = new System.Windows.Forms.ComboBox();
            this.lblSearchTerm = new System.Windows.Forms.Label();
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.navigationLayout = new System.Windows.Forms.TableLayoutPanel();
            this.grpRooms = new System.Windows.Forms.GroupBox();
            this.lstRooms = new System.Windows.Forms.ListBox();
            this.grpBodyParts = new System.Windows.Forms.GroupBox();
            this.lstBodyParts = new System.Windows.Forms.ListBox();
            this.grpExams = new System.Windows.Forms.GroupBox();
            this.examLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lstExams = new System.Windows.Forms.ListBox();
            this.btnConfirmExam = new System.Windows.Forms.Button();
            this.grpSelectedExams = new System.Windows.Forms.GroupBox();
            this.selectionLayout = new System.Windows.Forms.TableLayoutPanel();
            this.dgvSelectedExams = new System.Windows.Forms.DataGridView();
            this.colMinisterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInternalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExamDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBodyPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectionActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.rootLayout.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.searchLayout.SuspendLayout();
            this.navigationLayout.SuspendLayout();
            this.grpRooms.SuspendLayout();
            this.grpBodyParts.SuspendLayout();
            this.grpExams.SuspendLayout();
            this.examLayout.SuspendLayout();
            this.grpSelectedExams.SuspendLayout();
            this.selectionLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedExams)).BeginInit();
            this.selectionActionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.grpSearch, 0, 0);
            this.rootLayout.Controls.Add(this.navigationLayout, 0, 1);
            this.rootLayout.Controls.Add(this.grpSelectedExams, 0, 2);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(12, 12);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.RowCount = 3;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.rootLayout.Size = new System.Drawing.Size(1360, 757);
            this.rootLayout.TabIndex = 0;
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.searchLayout);
            this.grpSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSearch.Location = new System.Drawing.Point(3, 3);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpSearch.Size = new System.Drawing.Size(1354, 82);
            this.grpSearch.TabIndex = 0;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Ricerca esami";
            // 
            // searchLayout
            // 
            this.searchLayout.ColumnCount = 6;
            this.searchLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.searchLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.searchLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.searchLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.searchLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.searchLayout.Controls.Add(this.lblSearchField, 0, 0);
            this.searchLayout.Controls.Add(this.cmbSearchField, 1, 0);
            this.searchLayout.Controls.Add(this.lblSearchTerm, 2, 0);
            this.searchLayout.Controls.Add(this.txtSearchTerm, 3, 0);
            this.searchLayout.Controls.Add(this.btnSearch, 4, 0);
            this.searchLayout.Controls.Add(this.btnClearSearch, 5, 0);
            this.searchLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchLayout.Location = new System.Drawing.Point(12, 26);
            this.searchLayout.Name = "searchLayout";
            this.searchLayout.RowCount = 1;
            this.searchLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.searchLayout.Size = new System.Drawing.Size(1330, 44);
            this.searchLayout.TabIndex = 0;
            // 
            // lblSearchField
            // 
            this.lblSearchField.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSearchField.AutoSize = true;
            this.lblSearchField.Location = new System.Drawing.Point(3, 12);
            this.lblSearchField.Name = "lblSearchField";
            this.lblSearchField.Size = new System.Drawing.Size(92, 16);
            this.lblSearchField.TabIndex = 0;
            this.lblSearchField.Text = "Cerca nel campo";
            // 
            // cmbSearchField
            // 
            this.cmbSearchField.Anchor = ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.cmbSearchField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchField.FormattingEnabled = true;
            this.cmbSearchField.Items.AddRange(new object[] {
            "CodiceMinisteriale",
            "CodiceInterno",
            "DescrizioneEsame"});
            this.cmbSearchField.Location = new System.Drawing.Point(101, 9);
            this.cmbSearchField.Name = "cmbSearchField";
            this.cmbSearchField.Size = new System.Drawing.Size(214, 24);
            this.cmbSearchField.TabIndex = 1;
            // 
            // lblSearchTerm
            // 
            this.lblSearchTerm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSearchTerm.AutoSize = true;
            this.lblSearchTerm.Location = new System.Drawing.Point(321, 12);
            this.lblSearchTerm.Name = "lblSearchTerm";
            this.lblSearchTerm.Size = new System.Drawing.Size(88, 16);
            this.lblSearchTerm.TabIndex = 2;
            this.lblSearchTerm.Text = "Testo da cercare";
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Anchor = ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.txtSearchTerm.Location = new System.Drawing.Point(415, 10);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(725, 22);
            this.txtSearchTerm.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSearch.Location = new System.Drawing.Point(1146, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(84, 30);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Cerca";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClearSearch.Location = new System.Drawing.Point(1236, 7);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(91, 30);
            this.btnClearSearch.TabIndex = 5;
            this.btnClearSearch.Text = "Vedi tutti";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            // 
            // navigationLayout
            // 
            this.navigationLayout.ColumnCount = 3;
            this.navigationLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.navigationLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.navigationLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.navigationLayout.Controls.Add(this.grpRooms, 0, 0);
            this.navigationLayout.Controls.Add(this.grpBodyParts, 1, 0);
            this.navigationLayout.Controls.Add(this.grpExams, 2, 0);
            this.navigationLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationLayout.Location = new System.Drawing.Point(3, 91);
            this.navigationLayout.Name = "navigationLayout";
            this.navigationLayout.RowCount = 1;
            this.navigationLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.navigationLayout.Size = new System.Drawing.Size(1354, 362);
            this.navigationLayout.TabIndex = 1;
            // 
            // grpRooms
            // 
            this.grpRooms.Controls.Add(this.lstRooms);
            this.grpRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRooms.Location = new System.Drawing.Point(3, 3);
            this.grpRooms.Name = "grpRooms";
            this.grpRooms.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpRooms.Size = new System.Drawing.Size(445, 356);
            this.grpRooms.TabIndex = 0;
            this.grpRooms.TabStop = false;
            this.grpRooms.Text = "Ambulatori";
            // 
            // lstRooms
            // 
            this.lstRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRooms.FormattingEnabled = true;
            this.lstRooms.ItemHeight = 16;
            this.lstRooms.Location = new System.Drawing.Point(12, 26);
            this.lstRooms.Name = "lstRooms";
            this.lstRooms.Size = new System.Drawing.Size(421, 318);
            this.lstRooms.TabIndex = 0;
            // 
            // grpBodyParts
            // 
            this.grpBodyParts.Controls.Add(this.lstBodyParts);
            this.grpBodyParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBodyParts.Location = new System.Drawing.Point(454, 3);
            this.grpBodyParts.Name = "grpBodyParts";
            this.grpBodyParts.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpBodyParts.Size = new System.Drawing.Size(445, 356);
            this.grpBodyParts.TabIndex = 1;
            this.grpBodyParts.TabStop = false;
            this.grpBodyParts.Text = "Parti del corpo";
            // 
            // lstBodyParts
            // 
            this.lstBodyParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBodyParts.FormattingEnabled = true;
            this.lstBodyParts.ItemHeight = 16;
            this.lstBodyParts.Location = new System.Drawing.Point(12, 26);
            this.lstBodyParts.Name = "lstBodyParts";
            this.lstBodyParts.Size = new System.Drawing.Size(421, 318);
            this.lstBodyParts.TabIndex = 0;
            // 
            // grpExams
            // 
            this.grpExams.Controls.Add(this.examLayout);
            this.grpExams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpExams.Location = new System.Drawing.Point(905, 3);
            this.grpExams.Name = "grpExams";
            this.grpExams.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpExams.Size = new System.Drawing.Size(446, 356);
            this.grpExams.TabIndex = 2;
            this.grpExams.TabStop = false;
            this.grpExams.Text = "Esami";
            // 
            // examLayout
            // 
            this.examLayout.ColumnCount = 1;
            this.examLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.examLayout.Controls.Add(this.lstExams, 0, 0);
            this.examLayout.Controls.Add(this.btnConfirmExam, 0, 1);
            this.examLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.examLayout.Location = new System.Drawing.Point(12, 26);
            this.examLayout.Name = "examLayout";
            this.examLayout.RowCount = 2;
            this.examLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.examLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.examLayout.Size = new System.Drawing.Size(422, 318);
            this.examLayout.TabIndex = 0;
            // 
            // lstExams
            // 
            this.lstExams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExams.FormattingEnabled = true;
            this.lstExams.ItemHeight = 16;
            this.lstExams.Location = new System.Drawing.Point(3, 3);
            this.lstExams.Name = "lstExams";
            this.lstExams.Size = new System.Drawing.Size(416, 274);
            this.lstExams.TabIndex = 0;
            // 
            // btnConfirmExam
            // 
            this.btnConfirmExam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnConfirmExam.Location = new System.Drawing.Point(269, 283);
            this.btnConfirmExam.Name = "btnConfirmExam";
            this.btnConfirmExam.Size = new System.Drawing.Size(150, 32);
            this.btnConfirmExam.TabIndex = 1;
            this.btnConfirmExam.Text = "Conferma selezione";
            this.btnConfirmExam.UseVisualStyleBackColor = true;
            // 
            // grpSelectedExams
            // 
            this.grpSelectedExams.Controls.Add(this.selectionLayout);
            this.grpSelectedExams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSelectedExams.Location = new System.Drawing.Point(3, 459);
            this.grpSelectedExams.Name = "grpSelectedExams";
            this.grpSelectedExams.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpSelectedExams.Size = new System.Drawing.Size(1354, 295);
            this.grpSelectedExams.TabIndex = 2;
            this.grpSelectedExams.TabStop = false;
            this.grpSelectedExams.Text = "Esami selezionati";
            // 
            // selectionLayout
            // 
            this.selectionLayout.ColumnCount = 1;
            this.selectionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.selectionLayout.Controls.Add(this.dgvSelectedExams, 0, 0);
            this.selectionLayout.Controls.Add(this.selectionActionsPanel, 0, 1);
            this.selectionLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectionLayout.Location = new System.Drawing.Point(12, 26);
            this.selectionLayout.Name = "selectionLayout";
            this.selectionLayout.RowCount = 2;
            this.selectionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.selectionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.selectionLayout.Size = new System.Drawing.Size(1330, 257);
            this.selectionLayout.TabIndex = 0;
            // 
            // dgvSelectedExams
            // 
            this.dgvSelectedExams.AllowUserToAddRows = false;
            this.dgvSelectedExams.AllowUserToDeleteRows = false;
            this.dgvSelectedExams.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelectedExams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedExams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMinisterialCode,
            this.colInternalCode,
            this.colExamDescription,
            this.colBodyPart,
            this.colRoom});
            this.dgvSelectedExams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectedExams.Location = new System.Drawing.Point(3, 3);
            this.dgvSelectedExams.MultiSelect = false;
            this.dgvSelectedExams.Name = "dgvSelectedExams";
            this.dgvSelectedExams.ReadOnly = true;
            this.dgvSelectedExams.RowHeadersWidth = 51;
            this.dgvSelectedExams.RowTemplate.Height = 24;
            this.dgvSelectedExams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedExams.Size = new System.Drawing.Size(1324, 211);
            this.dgvSelectedExams.TabIndex = 0;
            // 
            // colMinisterialCode
            // 
            this.colMinisterialCode.HeaderText = "Codice ministeriale";
            this.colMinisterialCode.MinimumWidth = 6;
            this.colMinisterialCode.Name = "colMinisterialCode";
            this.colMinisterialCode.ReadOnly = true;
            // 
            // colInternalCode
            // 
            this.colInternalCode.HeaderText = "Codice interno";
            this.colInternalCode.MinimumWidth = 6;
            this.colInternalCode.Name = "colInternalCode";
            this.colInternalCode.ReadOnly = true;
            // 
            // colExamDescription
            // 
            this.colExamDescription.HeaderText = "Descrizione esame";
            this.colExamDescription.MinimumWidth = 6;
            this.colExamDescription.Name = "colExamDescription";
            this.colExamDescription.ReadOnly = true;
            // 
            // colBodyPart
            // 
            this.colBodyPart.HeaderText = "Parte del corpo";
            this.colBodyPart.MinimumWidth = 6;
            this.colBodyPart.Name = "colBodyPart";
            this.colBodyPart.ReadOnly = true;
            // 
            // colRoom
            // 
            this.colRoom.HeaderText = "Ambulatorio";
            this.colRoom.MinimumWidth = 6;
            this.colRoom.Name = "colRoom";
            this.colRoom.ReadOnly = true;
            // 
            // selectionActionsPanel
            // 
            this.selectionActionsPanel.AutoSize = true;
            this.selectionActionsPanel.Controls.Add(this.btnRemoveSelected);
            this.selectionActionsPanel.Controls.Add(this.btnMoveDown);
            this.selectionActionsPanel.Controls.Add(this.btnMoveUp);
            this.selectionActionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectionActionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.selectionActionsPanel.Location = new System.Drawing.Point(3, 220);
            this.selectionActionsPanel.Name = "selectionActionsPanel";
            this.selectionActionsPanel.Size = new System.Drawing.Size(1324, 34);
            this.selectionActionsPanel.TabIndex = 1;
            this.selectionActionsPanel.WrapContents = false;
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.AutoSize = true;
            this.btnRemoveSelected.Location = new System.Drawing.Point(1220, 3);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(101, 28);
            this.btnRemoveSelected.TabIndex = 0;
            this.btnRemoveSelected.Text = "Elimina riga";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.AutoSize = true;
            this.btnMoveDown.Location = new System.Drawing.Point(1128, 3);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(86, 28);
            this.btnMoveDown.TabIndex = 1;
            this.btnMoveDown.Text = "Sposta giù";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.AutoSize = true;
            this.btnMoveUp.Location = new System.Drawing.Point(1035, 3);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(87, 28);
            this.btnMoveUp.TabIndex = 2;
            this.btnMoveUp.Text = "Sposta su";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 781);
            this.Controls.Add(this.rootLayout);
            this.MinimumSize = new System.Drawing.Size(1200, 720);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exam Navigator";
            this.rootLayout.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.searchLayout.ResumeLayout(false);
            this.searchLayout.PerformLayout();
            this.navigationLayout.ResumeLayout(false);
            this.grpRooms.ResumeLayout(false);
            this.grpBodyParts.ResumeLayout(false);
            this.grpExams.ResumeLayout(false);
            this.examLayout.ResumeLayout(false);
            this.grpSelectedExams.ResumeLayout(false);
            this.selectionLayout.ResumeLayout(false);
            this.selectionLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedExams)).EndInit();
            this.selectionActionsPanel.ResumeLayout(false);
            this.selectionActionsPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel rootLayout;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.TableLayoutPanel searchLayout;
        private System.Windows.Forms.Label lblSearchField;
        private System.Windows.Forms.ComboBox cmbSearchField;
        private System.Windows.Forms.Label lblSearchTerm;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.TableLayoutPanel navigationLayout;
        private System.Windows.Forms.GroupBox grpRooms;
        private System.Windows.Forms.ListBox lstRooms;
        private System.Windows.Forms.GroupBox grpBodyParts;
        private System.Windows.Forms.ListBox lstBodyParts;
        private System.Windows.Forms.GroupBox grpExams;
        private System.Windows.Forms.TableLayoutPanel examLayout;
        private System.Windows.Forms.ListBox lstExams;
        private System.Windows.Forms.Button btnConfirmExam;
        private System.Windows.Forms.GroupBox grpSelectedExams;
        private System.Windows.Forms.TableLayoutPanel selectionLayout;
        private System.Windows.Forms.DataGridView dgvSelectedExams;
        private System.Windows.Forms.FlowLayoutPanel selectionActionsPanel;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMinisterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInternalCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExamDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBodyPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoom;
    }
}
