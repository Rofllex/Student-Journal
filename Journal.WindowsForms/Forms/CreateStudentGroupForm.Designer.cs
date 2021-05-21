
namespace Journal.WindowsForms.Forms
{
    partial class CreateStudentGroupForm
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
            this.teacherListBox = new System.Windows.Forms.ListBox();
            this.specialtyListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.currentCourseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.subgroupNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.studentsListBox = new System.Windows.Forms.ListBox();
            this.selectedStudentsListBox = new System.Windows.Forms.ListBox();
            this.createStudentGroupButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupsDataGridView = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.currentCourseNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subgroupNumericUpDown)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsDataGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // teacherListBox
            // 
            this.teacherListBox.FormattingEnabled = true;
            this.teacherListBox.ItemHeight = 18;
            this.teacherListBox.Location = new System.Drawing.Point(11, 197);
            this.teacherListBox.Name = "teacherListBox";
            this.teacherListBox.Size = new System.Drawing.Size(250, 130);
            this.teacherListBox.TabIndex = 0;
            // 
            // specialtyListBox
            // 
            this.specialtyListBox.FormattingEnabled = true;
            this.specialtyListBox.ItemHeight = 18;
            this.specialtyListBox.Location = new System.Drawing.Point(12, 36);
            this.specialtyListBox.Name = "specialtyListBox";
            this.specialtyListBox.Size = new System.Drawing.Size(249, 130);
            this.specialtyListBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выбор специальности";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Выбор куратора";
            // 
            // currentCourseNumericUpDown
            // 
            this.currentCourseNumericUpDown.Location = new System.Drawing.Point(402, 202);
            this.currentCourseNumericUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.currentCourseNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.currentCourseNumericUpDown.Name = "currentCourseNumericUpDown";
            this.currentCourseNumericUpDown.Size = new System.Drawing.Size(60, 25);
            this.currentCourseNumericUpDown.TabIndex = 3;
            this.currentCourseNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.currentCourseNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(301, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Курс";
            // 
            // subgroupNumericUpDown
            // 
            this.subgroupNumericUpDown.Location = new System.Drawing.Point(402, 236);
            this.subgroupNumericUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.subgroupNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.subgroupNumericUpDown.Name = "subgroupNumericUpDown";
            this.subgroupNumericUpDown.Size = new System.Drawing.Size(60, 25);
            this.subgroupNumericUpDown.TabIndex = 3;
            this.subgroupNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.subgroupNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(301, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Подгруппа";
            // 
            // studentsListBox
            // 
            this.studentsListBox.Enabled = false;
            this.studentsListBox.FormattingEnabled = true;
            this.studentsListBox.ItemHeight = 18;
            this.studentsListBox.Location = new System.Drawing.Point(509, 36);
            this.studentsListBox.Name = "studentsListBox";
            this.studentsListBox.Size = new System.Drawing.Size(217, 130);
            this.studentsListBox.TabIndex = 6;
            // 
            // selectedStudentsListBox
            // 
            this.selectedStudentsListBox.Enabled = false;
            this.selectedStudentsListBox.FormattingEnabled = true;
            this.selectedStudentsListBox.ItemHeight = 18;
            this.selectedStudentsListBox.Location = new System.Drawing.Point(286, 36);
            this.selectedStudentsListBox.Name = "selectedStudentsListBox";
            this.selectedStudentsListBox.Size = new System.Drawing.Size(217, 130);
            this.selectedStudentsListBox.TabIndex = 6;
            // 
            // createStudentGroupButton
            // 
            this.createStudentGroupButton.Location = new System.Drawing.Point(301, 290);
            this.createStudentGroupButton.Name = "createStudentGroupButton";
            this.createStudentGroupButton.Size = new System.Drawing.Size(161, 37);
            this.createStudentGroupButton.TabIndex = 7;
            this.createStudentGroupButton.Text = "Создать";
            this.createStudentGroupButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 18);
            this.label5.TabIndex = 2;
            this.label5.Text = "Выбранные студенты";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(509, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 2;
            this.label6.Text = "Студенты";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(757, 367);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupsDataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(749, 336);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Группы";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupsDataGridView
            // 
            this.groupsDataGridView.AllowUserToAddRows = false;
            this.groupsDataGridView.AllowUserToDeleteRows = false;
            this.groupsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.groupsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groupsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupsDataGridView.Location = new System.Drawing.Point(3, 3);
            this.groupsDataGridView.Name = "groupsDataGridView";
            this.groupsDataGridView.RowHeadersVisible = false;
            this.groupsDataGridView.RowTemplate.Height = 25;
            this.groupsDataGridView.Size = new System.Drawing.Size(743, 330);
            this.groupsDataGridView.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.specialtyListBox);
            this.tabPage2.Controls.Add(this.createStudentGroupButton);
            this.tabPage2.Controls.Add(this.teacherListBox);
            this.tabPage2.Controls.Add(this.selectedStudentsListBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.studentsListBox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.subgroupNumericUpDown);
            this.tabPage2.Controls.Add(this.currentCourseNumericUpDown);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(749, 336);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Создать";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CreateStudentGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(757, 367);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "CreateStudentGroupForm";
            this.Text = "Создание группы студентов";
            ((System.ComponentModel.ISupportInitialize)(this.currentCourseNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subgroupNumericUpDown)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupsDataGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox teacherListBox;
        private System.Windows.Forms.ListBox specialtyListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown currentCourseNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown subgroupNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox studentsListBox;
        private System.Windows.Forms.ListBox selectedStudentsListBox;
        private System.Windows.Forms.Button createStudentGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button createStudentGroupButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView groupsDataGridView;
    }
}