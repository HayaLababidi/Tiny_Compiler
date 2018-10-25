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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string Code=textBox1.Text.ToLower();
            JASON_Compiler.Start_Compiling(Code);
            PrintTokens();
         //   PrintLexemes();

            PrintErrors();
        }
        void PrintTokens()
        {
            for (int i = 0; i < JASON_Compiler.Jason_Scanner.Tokens.Count; i++)
            {
               dataGridView1.Rows.Add(JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).lex, JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for(int i=0; i<Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        /*  void PrintLexemes()
{
    for (int i = 0; i < JASON_Compiler.Lexemes.Count; i++)
    {
        textBox2.Text += JASON_Compiler.Lexemes.ElementAt(i);
        textBox2.Text += Environment.NewLine;
    }
}*/
    }
}
