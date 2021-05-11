
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.userNameLabel = new System.Windows.Forms.Label();
            this.groupsListBox = new System.Windows.Forms.ListBox();
            this.studentsListBox = new System.Windows.Forms.ListBox();
            this.pasteGradeButton = new System.Windows.Forms.Button();
            this.subjectsComboBox = new System.Windows.Forms.ComboBox();
            this.selectGradeComboBox = new System.Windows.Forms.ComboBox();
            this.selectedStudentsListBox = new System.Windows.Forms.ListBox();
            this.clearSelectedStudentsButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.logoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swapStudentsButton = new System.Windows.Forms.Button();
            this.pasteSelectedStudentsGrade = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(24, 316);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(134, 18);
            label2.TabIndex = 6;
            label2.Text = "Выбор предмета";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(24, 348);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 18);
            label3.TabIndex = 6;
            label3.Text = "Тип оценки";
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
            // groupsListBox
            // 
            this.groupsListBox.FormattingEnabled = true;
            this.groupsListBox.ItemHeight = 18;
            this.groupsListBox.Location = new System.Drawing.Point(24, 169);
            this.groupsListBox.Name = "groupsListBox";
            this.groupsListBox.Size = new System.Drawing.Size(345, 130);
            this.groupsListBox.TabIndex = 2;
            // 
            // studentsListBox
            // 
            this.studentsListBox.FormattingEnabled = true;
            this.studentsListBox.ItemHeight = 18;
            this.studentsListBox.Location = new System.Drawing.Point(375, 169);
            this.studentsListBox.Name = "studentsListBox";
            this.studentsListBox.Size = new System.Drawing.Size(345, 130);
            this.studentsListBox.TabIndex = 3;
            // 
            // pasteGradeButton
            // 
            this.pasteGradeButton.Location = new System.Drawing.Point(164, 377);
            this.pasteGradeButton.Name = "pasteGradeButton";
            this.pasteGradeButton.Size = new System.Drawing.Size(205, 31);
            this.pasteGradeButton.TabIndex = 4;
            this.pasteGradeButton.Text = "Выставить";
            this.pasteGradeButton.UseVisualStyleBackColor = true;
            // 
            // subjectsComboBox
            // 
            this.subjectsComboBox.FormattingEnabled = true;
            this.subjectsComboBox.Location = new System.Drawing.Point(164, 313);
            this.subjectsComboBox.Name = "subjectsComboBox";
            this.subjectsComboBox.Size = new System.Drawing.Size(205, 26);
            this.subjectsComboBox.TabIndex = 5;
            // 
            // selectGradeComboBox
            // 
            this.selectGradeComboBox.FormattingEnabled = true;
            this.selectGradeComboBox.Location = new System.Drawing.Point(164, 345);
            this.selectGradeComboBox.Name = "selectGradeComboBox";
            this.selectGradeComboBox.Size = new System.Drawing.Size(205, 26);
            this.selectGradeComboBox.TabIndex = 5;
            // 
            // selectedStudentsListBox
            // 
            this.selectedStudentsListBox.FormattingEnabled = true;
            this.selectedStudentsListBox.ItemHeight = 18;
            this.selectedStudentsListBox.Location = new System.Drawing.Point(726, 169);
            this.selectedStudentsListBox.Name = "selectedStudentsListBox";
            this.selectedStudentsListBox.Size = new System.Drawing.Size(345, 130);
            this.selectedStudentsListBox.TabIndex = 3;
            // 
            // clearSelectedStudentsButton
            // 
            this.clearSelectedStudentsButton.Location = new System.Drawing.Point(965, 313);
            this.clearSelectedStudentsButton.Name = "clearSelectedStudentsButton";
            this.clearSelectedStudentsButton.Size = new System.Drawing.Size(106, 31);
            this.clearSelectedStudentsButton.TabIndex = 7;
            this.clearSelectedStudentsButton.Text = "Очистить";
            this.clearSelectedStudentsButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "Выбор группы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(375, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Выбор студентов";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(726, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "Выбранные студенты";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("DejaVu Sans Mono", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(24, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(208, 22);
            this.label7.TabIndex = 9;
            this.label7.Text = "Выставление оценок";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1092, 26);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // logoutMenuItem
            // 
            this.logoutMenuItem.Name = "logoutMenuItem";
            this.logoutMenuItem.Size = new System.Drawing.Size(65, 22);
            this.logoutMenuItem.Text = "Выйти";
            // 
            // swapStudentsButton
            // 
            this.swapStudentsButton.Location = new System.Drawing.Point(688, 313);
            this.swapStudentsButton.Name = "swapStudentsButton";
            this.swapStudentsButton.Size = new System.Drawing.Size(75, 31);
            this.swapStudentsButton.TabIndex = 11;
            this.swapStudentsButton.Text = "Обмен";
            this.swapStudentsButton.UseVisualStyleBackColor = true;
            // 
            // pasteSelectedStudentsGrade
            // 
            this.pasteSelectedStudentsGrade.Location = new System.Drawing.Point(164, 414);
            this.pasteSelectedStudentsGrade.Name = "pasteSelectedStudentsGrade";
            this.pasteSelectedStudentsGrade.Size = new System.Drawing.Size(205, 31);
            this.pasteSelectedStudentsGrade.TabIndex = 4;
            this.pasteSelectedStudentsGrade.Text = "Выставить выбранным";
            this.pasteSelectedStudentsGrade.UseVisualStyleBackColor = true;
            // 
            // TeacherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1092, 461);
            this.Controls.Add(this.swapStudentsButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.clearSelectedStudentsButton);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.selectGradeComboBox);
            this.Controls.Add(this.subjectsComboBox);
            this.Controls.Add(this.pasteSelectedStudentsGrade);
            this.Controls.Add(this.pasteGradeButton);
            this.Controls.Add(this.selectedStudentsListBox);
            this.Controls.Add(this.studentsListBox);
            this.Controls.Add(this.groupsListBox);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(label1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TeacherForm";
            this.Text = "Учитель";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.ListBox groupsListBox;
        private System.Windows.Forms.ListBox studentsListBox;
        private System.Windows.Forms.Button pasteGradeButton;
        private System.Windows.Forms.ComboBox subjectsComboBox;
        private System.Windows.Forms.ComboBox selectGradeComboBox;
        private System.Windows.Forms.ListBox selectedStudentsListBox;
        private System.Windows.Forms.Button clearSelectedStudentsButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logoutMenuItem;
        private System.Windows.Forms.Button swapStudentsButton;
        private System.Windows.Forms.Button pasteSelectedStudentsGrade;
    }
}