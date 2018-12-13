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
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabTree = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabFunction = new System.Windows.Forms.TabPage();
            this.dgv_FunctionTable = new System.Windows.Forms.DataGridView();
            this.tabSymbolTable = new System.Windows.Forms.TabPage();
            this.dgv_SymbolTable = new System.Windows.Forms.DataGridView();
            this.tabs.SuspendLayout();
            this.tabTree.SuspendLayout();
            this.tabFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FunctionTable)).BeginInit();
            this.tabSymbolTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SymbolTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabTree);
            this.tabs.Controls.Add(this.tabFunction);
            this.tabs.Controls.Add(this.tabSymbolTable);
            this.tabs.Location = new System.Drawing.Point(3, 2);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(770, 600);
            this.tabs.TabIndex = 0;
            // 
            // tabTree
            // 
            this.tabTree.Controls.Add(this.treeView1);
            this.tabTree.Location = new System.Drawing.Point(4, 22);
            this.tabTree.Name = "tabTree";
            this.tabTree.Padding = new System.Windows.Forms.Padding(3);
            this.tabTree.Size = new System.Drawing.Size(762, 574);
            this.tabTree.TabIndex = 0;
            this.tabTree.Text = "Annotated Tree";
            this.tabTree.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(750, 562);
            this.treeView1.TabIndex = 1;
            // 
            // tabFunction
            // 
            this.tabFunction.Controls.Add(this.dgv_FunctionTable);
            this.tabFunction.Location = new System.Drawing.Point(4, 22);
            this.tabFunction.Name = "tabFunction";
            this.tabFunction.Padding = new System.Windows.Forms.Padding(3);
            this.tabFunction.Size = new System.Drawing.Size(762, 574);
            this.tabFunction.TabIndex = 1;
            this.tabFunction.Text = "Function Table";
            this.tabFunction.UseVisualStyleBackColor = true;
            // 
            // dgv_FunctionTable
            // 
            this.dgv_FunctionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_FunctionTable.Location = new System.Drawing.Point(6, 6);
            this.dgv_FunctionTable.Name = "dgv_FunctionTable";
            this.dgv_FunctionTable.Size = new System.Drawing.Size(750, 562);
            this.dgv_FunctionTable.TabIndex = 0;
            // 
            // tabSymbolTable
            // 
            this.tabSymbolTable.Controls.Add(this.dgv_SymbolTable);
            this.tabSymbolTable.Location = new System.Drawing.Point(4, 22);
            this.tabSymbolTable.Name = "tabSymbolTable";
            this.tabSymbolTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabSymbolTable.Size = new System.Drawing.Size(762, 574);
            this.tabSymbolTable.TabIndex = 2;
            this.tabSymbolTable.Text = "Symbol Table";
            this.tabSymbolTable.UseVisualStyleBackColor = true;
            // 
            // dgv_SymbolTable
            // 
            this.dgv_SymbolTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SymbolTable.Location = new System.Drawing.Point(6, 6);
            this.dgv_SymbolTable.Name = "dgv_SymbolTable";
            this.dgv_SymbolTable.Size = new System.Drawing.Size(750, 562);
            this.dgv_SymbolTable.TabIndex = 1;
            // 
            // SemanticAnalyserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 604);
            this.Controls.Add(this.tabs);
            this.Name = "SemanticAnalyserForm";
            this.Text = "SemanticAnalyserForm";
            this.tabs.ResumeLayout(false);
            this.tabTree.ResumeLayout(false);
            this.tabFunction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FunctionTable)).EndInit();
            this.tabSymbolTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SymbolTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabTree;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabPage tabFunction;
        private System.Windows.Forms.DataGridView dgv_FunctionTable;
        private System.Windows.Forms.TabPage tabSymbolTable;
        private System.Windows.Forms.DataGridView dgv_SymbolTable;
    }
}