namespace JASON_Compiler
{
    partial class SemanticAnalyserForm
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
            this.tabSymbol = new System.Windows.Forms.TabControl();
            this.tabTree = new System.Windows.Forms.TabPage();
            this.tabFunction = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabSymbol.SuspendLayout();
            this.tabTree.SuspendLayout();
            this.tabFunction.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSymbol
            // 
            this.tabSymbol.Controls.Add(this.tabTree);
            this.tabSymbol.Controls.Add(this.tabFunction);
            this.tabSymbol.Controls.Add(this.tabPage3);
            this.tabSymbol.Location = new System.Drawing.Point(3, 2);
            this.tabSymbol.Name = "tabSymbol";
            this.tabSymbol.SelectedIndex = 0;
            this.tabSymbol.Size = new System.Drawing.Size(770, 600);
            this.tabSymbol.TabIndex = 0;
            // 
            // tabTree
            // 
            this.tabTree.Controls.Add(this.treeView1);
            this.tabTree.Location = new System.Drawing.Point(4, 22);
            this.tabTree.Name = "tabTree";
            this.tabTree.Padding = new System.Windows.Forms.Padding(3);
            this.tabTree.Size = new System.Drawing.Size(762, 574);
            this.tabTree.TabIndex = 0;
            this.tabTree.Text = "tabPage1";
            this.tabTree.UseVisualStyleBackColor = true;
            // 
            // tabFunction
            // 
            this.tabFunction.Controls.Add(this.dataGridView1);
            this.tabFunction.Location = new System.Drawing.Point(4, 22);
            this.tabFunction.Name = "tabFunction";
            this.tabFunction.Padding = new System.Windows.Forms.Padding(3);
            this.tabFunction.Size = new System.Drawing.Size(762, 574);
            this.tabFunction.TabIndex = 1;
            this.tabFunction.Text = "tabPage2";
            this.tabFunction.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(762, 574);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(750, 562);
            this.treeView1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(750, 562);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(6, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(750, 562);
            this.dataGridView2.TabIndex = 1;
            // 
            // SemanticAnalyserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 604);
            this.Controls.Add(this.tabSymbol);
            this.Name = "SemanticAnalyserForm";
            this.Text = "SemanticAnalyserForm";
            this.tabSymbol.ResumeLayout(false);
            this.tabTree.ResumeLayout(false);
            this.tabFunction.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSymbol;
        private System.Windows.Forms.TabPage tabTree;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabPage tabFunction;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}