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
    public partial class Parser_Form : Form
    {
        public Parser_Form()
        {
            InitializeComponent();
        }

        private void Parser_Form_Load(object sender, EventArgs e)
        {
            Node root = SyntaxAnalyser.Parse(JASON_Compiler.Jason_Scanner.Tokens);
            treeView1.Nodes.Add(SyntaxAnalyser.PrintParseTree(root));
            PrintErrors();
        }
        void PrintErrors()
        {
            for (int i = 0; i < Errors.Parser_Error_List.Count; i++)
            {
                textBox2.Text += "ERROR " + Convert.ToString(i + 1) + ": " + Errors.Parser_Error_List[i];
                textBox2.AppendText(Environment.NewLine);
            }
        }
    }
}
