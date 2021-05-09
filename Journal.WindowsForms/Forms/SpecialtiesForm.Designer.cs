
namespace Journal.WindowsForms.Forms
{
    partial class SpecialtiesForm
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
            this.specialtiesDataGrid = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.specialtyNameTextBox = new System.Windows.Forms.TextBox();
            this.specialtyCodeTextBox = new System.Windows.Forms.TextBox();
            this.specialtyCoursesTextBox = new System.Windows.Forms.TextBox();
            this.subjectListBox = new System.Windows.Forms.ListBox();
            this.availableSubjectsListBox = new System.Windows.Forms.ListBox();
            this.addSubjectButton = new System.Windows.Forms.Button();
            this.removeSubjectButton = new System.Windows.Forms.Button();
            this.createSpecialtyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.specialtiesDataGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // specialtiesDataGrid
            // 
            this.specialtiesDataGrid.AllowUserToAddRows = false;
            this.specialtiesDataGrid.AllowUserToDeleteRows = false;
            this.specialtiesDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.specialtiesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.specialtiesDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.specialtiesDataGrid.Location = new System.Drawing.Point(3, 3);
            this.specialtiesDataGrid.Name = "specialtiesDataGrid";
            this.specialtiesDataGrid.ReadOnly = true;
            this.specialtiesDataGrid.RowHeadersVisible = false;
            this.specialtiesDataGrid.RowTemplate.Height = 25;
            this.specialtiesDataGrid.Size = new System.Drawing.Size(786, 480);
            this.specialtiesDataGrid.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 517);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.specialtiesDataGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 486);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Специальности";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.createSpecialtyButton);
            this.tabPage2.Controls.Add(this.removeSubjectButton);
            this.tabPage2.Controls.Add(this.addSubjectButton);
            this.tabPage2.Controls.Add(this.availableSubjectsListBox);
            this.tabPage2.Controls.Add(this.subjectListBox);
            this.tabPage2.Controls.Add(this.specialtyCoursesTextBox);
            this.tabPage2.Controls.Add(this.specialtyCodeTextBox);
            this.tabPage2.Controls.Add(this.specialtyNameTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Конструктор";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // specialtyNameTextBox
            // 
            this.specialtyNameTextBox.Location = new System.Drawing.Point(152, 78);
            this.specialtyNameTextBox.Name = "specialtyNameTextBox";
            this.specialtyNameTextBox.Size = new System.Drawing.Size(100, 25);
            this.specialtyNameTextBox.TabIndex = 0;
            // 
            // specialtyCodeTextBox
            // 
            this.specialtyCodeTextBox.Location = new System.Drawing.Point(152, 109);
            this.specialtyCodeTextBox.Name = "specialtyCodeTextBox";
            this.specialtyCodeTextBox.Size = new System.Drawing.Size(100, 25);
            this.specialtyCodeTextBox.TabIndex = 0;
            // 
            // specialtyCoursesTextBox
            // 
            this.specialtyCoursesTextBox.Location = new System.Drawing.Point(152, 140);
            this.specialtyCoursesTextBox.Name = "specialtyCoursesTextBox";
            this.specialtyCoursesTextBox.Size = new System.Drawing.Size(100, 25);
            this.specialtyCoursesTextBox.TabIndex = 0;
            // 
            // subjectListBox
            // 
            this.subjectListBox.FormattingEnabled = true;
            this.subjectListBox.ItemHeight = 18;
            this.subjectListBox.Location = new System.Drawing.Point(314, 71);
            this.subjectListBox.Name = "subjectListBox";
            this.subjectListBox.Size = new System.Drawing.Size(203, 166);
            this.subjectListBox.TabIndex = 1;
            // 
            // availableSubjectsListBox
            // 
            this.availableSubjectsListBox.FormattingEnabled = true;
            this.availableSubjectsListBox.ItemHeight = 18;
            this.availableSubjectsListBox.Location = new System.Drawing.Point(569, 71);
            this.availableSubjectsListBox.Name = "availableSubjectsListBox";
            this.availableSubjectsListBox.Size = new System.Drawing.Size(203, 166);
            this.availableSubjectsListBox.TabIndex = 1;
            // 
            // addSubjectButton
            // 
            this.addSubjectButton.Location = new System.Drawing.Point(523, 112);
            this.addSubjectButton.Name = "addSubjectButton";
            this.addSubjectButton.Size = new System.Drawing.Size(42, 23);
            this.addSubjectButton.TabIndex = 2;
            this.addSubjectButton.Text = "<";
            this.addSubjectButton.UseVisualStyleBackColor = true;
            // 
            // removeSubjectButton
            // 
            this.removeSubjectButton.Location = new System.Drawing.Point(523, 160);
            this.removeSubjectButton.Name = "removeSubjectButton";
            this.removeSubjectButton.Size = new System.Drawing.Size(42, 23);
            this.removeSubjectButton.TabIndex = 2;
            this.removeSubjectButton.Text = ">";
            this.removeSubjectButton.UseVisualStyleBackColor = true;
            // 
            // createSpecialtyButton
            // 
            this.createSpecialtyButton.Location = new System.Drawing.Point(128, 196);
            this.createSpecialtyButton.Name = "createSpecialtyButton";
            this.createSpecialtyButton.Size = new System.Drawing.Size(124, 23);
            this.createSpecialtyButton.TabIndex = 2;
            this.createSpecialtyButton.Text = "Добавить";
            this.createSpecialtyButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Название";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Код";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Кол-во курсов";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(314, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Предметы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(569, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "Доступные предметы";
            // 
            // SpecialtiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 517);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "SpecialtiesForm";
            this.Text = "Специальности";
            ((System.ComponentModel.ISupportInitialize)(this.specialtiesDataGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView specialtiesDataGrid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button createSpecialtyButton;
        private System.Windows.Forms.Button removeSubjectButton;
        private System.Windows.Forms.Button addSubjectButton;
        private System.Windows.Forms.ListBox availableSubjectsListBox;
        private System.Windows.Forms.ListBox subjectListBox;
        private System.Windows.Forms.TextBox specialtyCoursesTextBox;
        private System.Windows.Forms.TextBox specialtyCodeTextBox;
        private System.Windows.Forms.TextBox specialtyNameTextBox;
    }
}