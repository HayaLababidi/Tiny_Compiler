using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public partial class SemanticAnalyserForm : Form
    {
        public SemanticAnalyserForm()
        {
            InitializeComponent();
        }

        private void SemanticAnalyserForm_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Add(SemanticAnalyser.PrintSemanticTree(SyntaxAnalyser.Parse(JASON_Compiler.Jason_Scanner.Tokens)));
            listBox1.Items.Add("Symbol Table:");
            foreach (var item in SemanticAnalyser.SymbolTable)
            {
                string val = "Varible: " + item.Key.Key + "\t\tScope: " + item.Key.Value ;
                if (item.Key.Key.Length == 1)
                {
                    val = "Varible: " + item.Key.Key+" " + "\t\tScope: " + item.Key.Value;
                }
                foreach (var i in item.Value)
                {
                    val = val + "\t\t" + i.Key + " = " + i.Value;
                }
                listBox1.Items.Add(val);
            }
        }
    }
}
