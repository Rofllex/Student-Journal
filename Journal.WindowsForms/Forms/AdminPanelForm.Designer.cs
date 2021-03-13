
namespace Journal.WindowsForms.Forms
{
    partial class AdminPanelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.usersGridView = new System.Windows.Forms.DataGridView();
            this.usersGridViewIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usersGridViewFirstNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usersGridViewSurnameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usersGridViewLastNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usersGridViewPhoneColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usersGridViewRoleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loadUsersButton = new System.Windows.Forms.Button();
            this.offsetTextBox = new System.Windows.Forms.TextBox();
            this.countTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.usersTabPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.usersTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // usersGridView
            // 
            this.usersGridView.AllowUserToAddRows = false;
            this.usersGridView.AllowUserToDeleteRows = false;
            this.usersGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.usersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usersGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.usersGridViewIdColumn,
            this.usersGridViewFirstNameColumn,
            this.usersGridViewSurnameColumn,
            this.usersGridViewLastNameColumn,
            this.usersGridViewPhoneColumn,
            this.usersGridViewRoleColumn});
            this.usersGridView.Location = new System.Drawing.Point(8, 3);
            this.usersGridView.Name = "usersGridView";
            this.usersGridView.ReadOnly = true;
            this.usersGridView.RowTemplate.Height = 25;
            this.usersGridView.Size = new System.Drawing.Size(970, 244);
            this.usersGridView.TabIndex = 0;
            // 
            // usersGridViewIdColumn
            // 
            this.usersGridViewIdColumn.HeaderText = "Id";
            this.usersGridViewIdColumn.Name = "usersGridViewIdColumn";
            this.usersGridViewIdColumn.ReadOnly = true;
            // 
            // usersGridViewFirstNameColumn
            // 
            this.usersGridViewFirstNameColumn.HeaderText = "Имя";
            this.usersGridViewFirstNameColumn.Name = "usersGridViewFirstNameColumn";
            this.usersGridViewFirstNameColumn.ReadOnly = true;
            // 
            // usersGridViewSurnameColumn
            // 
            this.usersGridViewSurnameColumn.HeaderText = "Фамилия";
            this.usersGridViewSurnameColumn.Name = "usersGridViewSurnameColumn";
            this.usersGridViewSurnameColumn.ReadOnly = true;
            // 
            // usersGridViewLastNameColumn
            // 
            this.usersGridViewLastNameColumn.HeaderText = "Отчество";
            this.usersGridViewLastNameColumn.Name = "usersGridViewLastNameColumn";
            this.usersGridViewLastNameColumn.ReadOnly = true;
            // 
            // usersGridViewPhoneColumn
            // 
            this.usersGridViewPhoneColumn.HeaderText = "Телефон";
            this.usersGridViewPhoneColumn.Name = "usersGridViewPhoneColumn";
            this.usersGridViewPhoneColumn.ReadOnly = true;
            // 
            // usersGridViewRoleColumn
            // 
            this.usersGridViewRoleColumn.HeaderText = "Роли";
            this.usersGridViewRoleColumn.Name = "usersGridViewRoleColumn";
            this.usersGridViewRoleColumn.ReadOnly = true;
            // 
            // loadUsersButton
            // 
            this.loadUsersButton.Location = new System.Drawing.Point(20, 318);
            this.loadUsersButton.Name = "loadUsersButton";
            this.loadUsersButton.Size = new System.Drawing.Size(124, 23);
            this.loadUsersButton.TabIndex = 1;
            this.loadUsersButton.Text = "Загрузить";
            this.loadUsersButton.UseVisualStyleBackColor = true;
            // 
            // offsetTextBox
            // 
            this.offsetTextBox.Location = new System.Drawing.Point(93, 259);
            this.offsetTextBox.Name = "offsetTextBox";
            this.offsetTextBox.Size = new System.Drawing.Size(51, 23);
            this.offsetTextBox.TabIndex = 2;
            // 
            // countTextBox
            // 
            this.countTextBox.Location = new System.Drawing.Point(93, 289);
            this.countTextBox.Name = "countTextBox";
            this.countTextBox.Size = new System.Drawing.Size(51, 23);
            this.countTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Смещение";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Количество";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.usersTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(994, 621);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // usersTabPage
            // 
            this.usersTabPage.BackColor = System.Drawing.Color.White;
            this.usersTabPage.Controls.Add(this.usersGridView);
            this.usersTabPage.Controls.Add(this.label2);
            this.usersTabPage.Controls.Add(this.loadUsersButton);
            this.usersTabPage.Controls.Add(this.label1);
            this.usersTabPage.Controls.Add(this.offsetTextBox);
            this.usersTabPage.Controls.Add(this.countTextBox);
            this.usersTabPage.Location = new System.Drawing.Point(4, 24);
            this.usersTabPage.Name = "usersTabPage";
            this.usersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.usersTabPage.Size = new System.Drawing.Size(986, 593);
            this.usersTabPage.TabIndex = 0;
            this.usersTabPage.Text = "Пользователи";
            // 
            // AdminPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 621);
            this.Controls.Add(this.tabControl1);
            this.Name = "AdminPanelForm";
            this.Text = "Панель администратора";
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.usersTabPage.ResumeLayout(false);
            this.usersTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView usersGridView;
        private System.Windows.Forms.Button loadUsersButton;
        private System.Windows.Forms.TextBox offsetTextBox;
        private System.Windows.Forms.TextBox countTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage usersTabPage;
        private System.Windows.Forms.DataGridViewTextBoxColumn usersGridViewIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usersGridViewFirstNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usersGridViewSurnameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usersGridViewLastNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usersGridViewPhoneColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usersGridViewRoleColumn;
    }
}