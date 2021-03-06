﻿namespace JASON_Compiler
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
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabSymbolTable = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabs.SuspendLayout();
            this.tabTree.SuspendLayout();
            this.tabFunction.SuspendLayout();
            this.tabSymbolTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabTree);
            this.tabs.Controls.Add(this.tabFunction);
            this.tabs.Controls.Add(this.tabSymbolTable);
            this.tabs.Location = new System.Drawing.Point(3, 2);
            this.tabs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(898, 804);
            this.tabs.TabIndex = 0;
            // 
            // tabTree
            // 
            this.tabTree.Controls.Add(this.treeView1);
            this.tabTree.Location = new System.Drawing.Point(4, 25);
            this.tabTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabTree.Name = "tabTree";
            this.tabTree.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabTree.Size = new System.Drawing.Size(890, 775);
            this.tabTree.TabIndex = 0;
            this.tabTree.Text = "Annotated Tree";
            this.tabTree.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(7, 7);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(874, 756);
            this.treeView1.TabIndex = 1;
            // 
            // tabFunction
            // 
            this.tabFunction.Controls.Add(this.listBox2);
            this.tabFunction.Location = new System.Drawing.Point(4, 25);
            this.tabFunction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabFunction.Name = "tabFunction";
            this.tabFunction.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabFunction.Size = new System.Drawing.Size(890, 775);
            this.tabFunction.TabIndex = 1;
            this.tabFunction.Text = "Function Table";
            this.tabFunction.UseVisualStyleBackColor = true;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(6, 7);
            this.listBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(878, 756);
            this.listBox2.TabIndex = 1;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // tabSymbolTable
            // 
            this.tabSymbolTable.Controls.Add(this.listBox1);
            this.tabSymbolTable.Location = new System.Drawing.Point(4, 25);
            this.tabSymbolTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSymbolTable.Name = "tabSymbolTable";
            this.tabSymbolTable.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSymbolTable.Size = new System.Drawing.Size(890, 775);
            this.tabSymbolTable.TabIndex = 2;
            this.tabSymbolTable.Text = "Symbol Table";
            this.tabSymbolTable.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(3, 4);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(878, 756);
            this.listBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(908, 30);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(595, 776);
            this.textBox2.TabIndex = 2;
            // 
            // SemanticAnalyserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1517, 815);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.tabs);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SemanticAnalyserForm";
            this.Text = "SemanticAnalyserForm";
            this.Load += new System.EventHandler(this.SemanticAnalyserForm_Load);
            this.tabs.ResumeLayout(false);
            this.tabTree.ResumeLayout(false);
            this.tabFunction.ResumeLayout(false);
            this.tabSymbolTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabTree;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabPage tabFunction;
        private System.Windows.Forms.TabPage tabSymbolTable;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ListBox listBox2;
    }
}