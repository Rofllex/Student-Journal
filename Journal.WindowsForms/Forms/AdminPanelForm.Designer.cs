
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.usersTabPage = new System.Windows.Forms.TabPage();
            this.usersCountLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.currentPageTextBox = new System.Windows.Forms.TextBox();
            this.nextPageButton = new System.Windows.Forms.Button();
            this.predPageButton = new System.Windows.Forms.Button();
            this.databaseTabPage = new System.Windows.Forms.TabPage();
            this.subjectsButton = new System.Windows.Forms.Button();
            this.specialtiesButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.logoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.usersTabPage.SuspendLayout();
            this.databaseTabPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // usersGridView
            // 
            this.usersGridView.AllowUserToAddRows = false;
            this.usersGridView.AllowUserToDeleteRows = false;
            this.usersGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usersGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.usersGridView.Location = new System.Drawing.Point(10, 4);
            this.usersGridView.Margin = new System.Windows.Forms.Padding(4);
            this.usersGridView.MultiSelect = false;
            this.usersGridView.Name = "usersGridView";
            this.usersGridView.RowHeadersVisible = false;
            this.usersGridView.RowTemplate.Height = 25;
            this.usersGridView.Size = new System.Drawing.Size(1102, 520);
            this.usersGridView.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.usersTabPage);
            this.tabControl1.Controls.Add(this.databaseTabPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 26);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1129, 698);
            this.tabControl1.TabIndex = 5;
            // 
            // usersTabPage
            // 
            this.usersTabPage.BackColor = System.Drawing.Color.White;
            this.usersTabPage.Controls.Add(this.usersCountLabel);
            this.usersTabPage.Controls.Add(this.label2);
            this.usersTabPage.Controls.Add(this.label1);
            this.usersTabPage.Controls.Add(this.currentPageTextBox);
            this.usersTabPage.Controls.Add(this.nextPageButton);
            this.usersTabPage.Controls.Add(this.predPageButton);
            this.usersTabPage.Controls.Add(this.usersGridView);
            this.usersTabPage.Location = new System.Drawing.Point(4, 27);
            this.usersTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.usersTabPage.Name = "usersTabPage";
            this.usersTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.usersTabPage.Size = new System.Drawing.Size(1121, 667);
            this.usersTabPage.TabIndex = 0;
            this.usersTabPage.Text = "Пользователи";
            // 
            // usersCountLabel
            // 
            this.usersCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.usersCountLabel.AutoSize = true;
            this.usersCountLabel.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.usersCountLabel.Location = new System.Drawing.Point(206, 631);
            this.usersCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.usersCountLabel.Name = "usersCountLabel";
            this.usersCountLabel.Size = new System.Drawing.Size(26, 18);
            this.usersCountLabel.TabIndex = 3;
            this.usersCountLabel.Text = "-1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(10, 631);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Всего пользователей:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(119, 548);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Навигация";
            // 
            // currentPageTextBox
            // 
            this.currentPageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentPageTextBox.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentPageTextBox.Location = new System.Drawing.Point(114, 578);
            this.currentPageTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.currentPageTextBox.Name = "currentPageTextBox";
            this.currentPageTextBox.Size = new System.Drawing.Size(100, 25);
            this.currentPageTextBox.TabIndex = 2;
            this.currentPageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.currentPageTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.currentPageTextBox_KeyPress);
            // 
            // nextPageButton
            // 
            this.nextPageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextPageButton.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nextPageButton.Location = new System.Drawing.Point(222, 578);
            this.nextPageButton.Margin = new System.Windows.Forms.Padding(4);
            this.nextPageButton.Name = "nextPageButton";
            this.nextPageButton.Size = new System.Drawing.Size(96, 25);
            this.nextPageButton.TabIndex = 1;
            this.nextPageButton.Text = ">";
            this.nextPageButton.UseVisualStyleBackColor = true;
            // 
            // predPageButton
            // 
            this.predPageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.predPageButton.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.predPageButton.Location = new System.Drawing.Point(10, 578);
            this.predPageButton.Margin = new System.Windows.Forms.Padding(4);
            this.predPageButton.Name = "predPageButton";
            this.predPageButton.Size = new System.Drawing.Size(96, 25);
            this.predPageButton.TabIndex = 1;
            this.predPageButton.Text = "<";
            this.predPageButton.UseVisualStyleBackColor = true;
            // 
            // databaseTabPage
            // 
            this.databaseTabPage.Controls.Add(this.subjectsButton);
            this.databaseTabPage.Controls.Add(this.specialtiesButton);
            this.databaseTabPage.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.databaseTabPage.Location = new System.Drawing.Point(4, 27);
            this.databaseTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.databaseTabPage.Name = "databaseTabPage";
            this.databaseTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.databaseTabPage.Size = new System.Drawing.Size(1121, 667);
            this.databaseTabPage.TabIndex = 1;
            this.databaseTabPage.Text = "БД";
            this.databaseTabPage.UseVisualStyleBackColor = true;
            // 
            // subjectsButton
            // 
            this.subjectsButton.Location = new System.Drawing.Point(43, 78);
            this.subjectsButton.Name = "subjectsButton";
            this.subjectsButton.Size = new System.Drawing.Size(208, 28);
            this.subjectsButton.TabIndex = 1;
            this.subjectsButton.Text = "Предметы";
            this.subjectsButton.UseVisualStyleBackColor = true;
            this.subjectsButton.Click += new System.EventHandler(this.subjectsButton_Click);
            // 
            // specialtiesButton
            // 
            this.specialtiesButton.Location = new System.Drawing.Point(43, 43);
            this.specialtiesButton.Margin = new System.Windows.Forms.Padding(4);
            this.specialtiesButton.Name = "specialtiesButton";
            this.specialtiesButton.Size = new System.Drawing.Size(208, 28);
            this.specialtiesButton.TabIndex = 0;
            this.specialtiesButton.Text = "Специальности";
            this.specialtiesButton.UseVisualStyleBackColor = true;
            this.specialtiesButton.Click += new System.EventHandler(this.specialtiesButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1129, 26);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // logoutMenuItem
            // 
            this.logoutMenuItem.Name = "logoutMenuItem";
            this.logoutMenuItem.Size = new System.Drawing.Size(65, 22);
            this.logoutMenuItem.Text = "Выйти";
            // 
            // AdminPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 724);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(694, 419);
            this.Name = "AdminPanelForm";
            this.Text = "Панель администратора";
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.usersTabPage.ResumeLayout(false);
            this.usersTabPage.PerformLayout();
            this.databaseTabPage.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView usersGridView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage usersTabPage;
        private System.Windows.Forms.TextBox currentPageTextBox;
        private System.Windows.Forms.Button nextPageButton;
        private System.Windows.Forms.Button predPageButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label usersCountLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage databaseTabPage;
        private System.Windows.Forms.Button specialtiesButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logoutMenuItem;
        private System.Windows.Forms.Button subjectsButton;
    }
}