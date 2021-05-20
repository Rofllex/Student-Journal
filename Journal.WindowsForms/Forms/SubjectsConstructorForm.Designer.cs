
namespace Journal.WindowsForms.Forms
{
    partial class SubjectsConstructorForm
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
            System.Windows.Forms.Label label1;
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.subjectsListBox = new System.Windows.Forms.ListBox();
            this.newSubjectNameTextBox = new System.Windows.Forms.TextBox();
            this.createSubjectButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 385);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.subjectsListBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(353, 354);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Предметы";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.createSubjectButton);
            this.tabPage2.Controls.Add(label1);
            this.tabPage2.Controls.Add(this.newSubjectNameTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(353, 354);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Конструктор";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // subjectsListBox
            // 
            this.subjectsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectsListBox.FormattingEnabled = true;
            this.subjectsListBox.ItemHeight = 18;
            this.subjectsListBox.Location = new System.Drawing.Point(8, 13);
            this.subjectsListBox.Name = "subjectsListBox";
            this.subjectsListBox.Size = new System.Drawing.Size(330, 328);
            this.subjectsListBox.TabIndex = 0;
            // 
            // newSubjectNameTextBox
            // 
            this.newSubjectNameTextBox.Location = new System.Drawing.Point(70, 141);
            this.newSubjectNameTextBox.Name = "newSubjectNameTextBox";
            this.newSubjectNameTextBox.Size = new System.Drawing.Size(212, 25);
            this.newSubjectNameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(70, 109);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(161, 18);
            label1.TabIndex = 1;
            label1.Text = "Название предмета";
            // 
            // createSubjectButton
            // 
            this.createSubjectButton.Location = new System.Drawing.Point(70, 172);
            this.createSubjectButton.Name = "createSubjectButton";
            this.createSubjectButton.Size = new System.Drawing.Size(212, 32);
            this.createSubjectButton.TabIndex = 2;
            this.createSubjectButton.Text = "Добавить";
            this.createSubjectButton.UseVisualStyleBackColor = true;
            // 
            // SubjectsConstructorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 385);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "SubjectsConstructorForm";
            this.Text = "SubjectsConstructorForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox subjectsListBox;
        private System.Windows.Forms.Button createSubjectButton;
        private System.Windows.Forms.TextBox newSubjectNameTextBox;
    }
}