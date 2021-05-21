﻿
namespace Journal.WindowsForms.Forms
{
    partial class GradesForm
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
            this.components = new System.ComponentModel.Container();
            this.gradesGridView = new System.Windows.Forms.DataGridView();
            this.monthNameLabel = new System.Windows.Forms.Label();
            this.predMonthButton = new System.Windows.Forms.Button();
            this.nextMonthButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.subjectNameLabel = new System.Windows.Forms.Label();
            this.gradesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gradesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // gradesGridView
            // 
            this.gradesGridView.AllowUserToAddRows = false;
            this.gradesGridView.AllowUserToDeleteRows = false;
            this.gradesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gradesGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gradesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gradesGridView.Location = new System.Drawing.Point(0, 56);
            this.gradesGridView.Margin = new System.Windows.Forms.Padding(4);
            this.gradesGridView.Name = "gradesGridView";
            this.gradesGridView.ReadOnly = true;
            this.gradesGridView.RowHeadersVisible = false;
            this.gradesGridView.RowTemplate.Height = 25;
            this.gradesGridView.Size = new System.Drawing.Size(931, 454);
            this.gradesGridView.TabIndex = 0;
            // 
            // monthNameLabel
            // 
            this.monthNameLabel.AutoSize = true;
            this.monthNameLabel.Location = new System.Drawing.Point(372, 25);
            this.monthNameLabel.Name = "monthNameLabel";
            this.monthNameLabel.Size = new System.Drawing.Size(62, 18);
            this.monthNameLabel.TabIndex = 1;
            this.monthNameLabel.Text = "Январь";
            this.monthNameLabel.Click += new System.EventHandler(this.monthNameLabel_Click);
            // 
            // predMonthButton
            // 
            this.predMonthButton.Location = new System.Drawing.Point(291, 23);
            this.predMonthButton.Name = "predMonthButton";
            this.predMonthButton.Size = new System.Drawing.Size(75, 23);
            this.predMonthButton.TabIndex = 2;
            this.predMonthButton.Text = "<";
            this.predMonthButton.UseVisualStyleBackColor = true;
            this.predMonthButton.Click += new System.EventHandler(this.predMonthButton_Click);
            // 
            // nextMonthButton
            // 
            this.nextMonthButton.Location = new System.Drawing.Point(440, 23);
            this.nextMonthButton.Name = "nextMonthButton";
            this.nextMonthButton.Size = new System.Drawing.Size(75, 23);
            this.nextMonthButton.TabIndex = 2;
            this.nextMonthButton.Text = ">";
            this.nextMonthButton.UseVisualStyleBackColor = true;
            this.nextMonthButton.Click += new System.EventHandler(this.nextMonthButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Предмет: ";
            // 
            // subjectNameLabel
            // 
            this.subjectNameLabel.AutoSize = true;
            this.subjectNameLabel.Location = new System.Drawing.Point(107, 23);
            this.subjectNameLabel.Name = "subjectNameLabel";
            this.subjectNameLabel.Size = new System.Drawing.Size(71, 18);
            this.subjectNameLabel.TabIndex = 3;
            this.subjectNameLabel.Text = "subject";
            // 
            // gradesContextMenu
            // 
            this.gradesContextMenu.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gradesContextMenu.Name = "contextMenuStrip1";
            this.gradesContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // GradesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 510);
            this.Controls.Add(this.subjectNameLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nextMonthButton);
            this.Controls.Add(this.predMonthButton);
            this.Controls.Add(this.monthNameLabel);
            this.Controls.Add(this.gradesGridView);
            this.Font = new System.Drawing.Font("DejaVu Sans Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "GradesForm";
            this.Text = "Журнал";
            ((System.ComponentModel.ISupportInitialize)(this.gradesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gradesGridView;
        private System.Windows.Forms.Label monthNameLabel;
        private System.Windows.Forms.Button predMonthButton;
        private System.Windows.Forms.Button nextMonthButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label subjectNameLabel;
        private System.Windows.Forms.ContextMenuStrip gradesContextMenu;
    }
}