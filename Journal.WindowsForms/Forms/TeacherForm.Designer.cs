
namespace Journal.WindowsForms.Forms
{
    partial class TeacherForm
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
            System.Windows.Forms.Label label1;
            this.userNameLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.logoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subjectsListBox = new System.Windows.Forms.ListBox();
            this.groupsComboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(24, 44);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(44, 18);
            label1.TabIndex = 0;
            label1.Text = "ФИО:";
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(74, 44);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(125, 18);
            this.userNameLabel.TabIndex = 1;
            this.userNameLabel.Text = "userNameLabel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "Выбор группы";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(476, 26);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // logoutMenuItem
            // 
            this.logoutMenuItem.Name = "logoutMenuItem";
            this.logoutMenuItem.Size = new System.Drawing.Size(65, 22);
            this.logoutMenuItem.Text = "Выйти";
            // 
            // subjectsListBox
            // 
            this.subjectsListBox.FormattingEnabled = true;
            this.subjectsListBox.ItemHeight = 18;
            this.subjectsListBox.Location = new System.Drawing.Point(24, 135);
            this.subjectsListBox.Name = "subjectsListBox";
            this.subjectsListBox.Size = new System.Drawing.Size(425, 292);
            this.subjectsListBox.TabIndex = 11;
            // 
            // groupsComboBox
            // 
            this.groupsComboBox.FormattingEnabled = true;
            this.groupsComboBox.Location = new System.Drawing.Point(24, 103);
            this.groupsComboBox.Name = "groupsComboBox";
            this.groupsComboBox.Size = new System.Drawing.Size(425, 26);
            this.groupsComboBox.TabIndex = 12;
            // 
            // TeacherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(476, 436);
            this.Controls.Add(this.groupsComboBox);
            this.Controls.Add(this.subjectsListBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(label1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeacherForm";
            this.Text = "Учитель";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logoutMenuItem;
        private System.Windows.Forms.ListBox subjectsListBox;
        private System.Windows.Forms.ComboBox groupsComboBox;
    }
}