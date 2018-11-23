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
            string Code=richTextBox1.Text.ToLower();
            JASON_Compiler.Lexemes.Clear();
            JASON_Compiler.TokenStream.Clear();
            Errors.Scanner_Error_List.Clear();
            JASON_Compiler.Jason_Scanner.Tokens.Clear();
            JASON_Compiler.Start_Compiling(Code);
            PrintTokens();
         //   PrintLexemes();

            PrintErrors();
        }
        private void CheckKeyword(string word, Color color, int startIndex)
        {
            // int v=word.Length;
            if (this.richTextBox1.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.richTextBox1.SelectionStart;
                while ((index = this.richTextBox1.Text.IndexOf(word, (index + 1))) != -1)
                {
                    // this.richTextBox1.Select(richTextBox1.Text.IndexOf(word), word.Length);
                    this.richTextBox1.Select((index + startIndex), word.Length);
                    this.richTextBox1.SelectionColor = color;
                    this.richTextBox1.Select(selectStart, 0);
                    this.richTextBox1.SelectionColor = Color.Black;
                }

            }
        }
        void PrintTokens()
        {            
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            for (int i = 0; i < JASON_Compiler.Jason_Scanner.Tokens.Count; i++)
            {
               dataGridView1.Rows.Add(JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).lex, JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for(int i=0; i<Errors.Scanner_Error_List.Count; i++)
            {
                textBox2.Text +="ERROR "+Convert.ToString(i+1)+": "+ Errors.Scanner_Error_List[i];
                textBox2.AppendText(Environment.NewLine);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.CheckKeyword("int", Color.Blue, 0);
            this.CheckKeyword("Int", Color.Blue, 0);
            this.CheckKeyword("float", Color.Blue, 0);
            this.CheckKeyword("string", Color.Blue, 0);
            this.CheckKeyword("read", Color.Olive, 0);
            this.CheckKeyword("write", Color.Olive, 0);
            this.CheckKeyword("repeat", Color.Orange, 0);
            this.CheckKeyword("until", Color.Orange, 0);
            this.CheckKeyword("if", Color.LightSeaGreen, 0);
            this.CheckKeyword("elseif", Color.LightSeaGreen, 0);
            this.CheckKeyword("else", Color.LightSeaGreen, 0);
            this.CheckKeyword("then", Color.LightSeaGreen, 0);
            this.CheckKeyword("return", Color.LightGreen, 0);
            this.CheckKeyword("endl", Color.Red, 0);
            this.CheckKeyword("end", Color.Red, 0);
            this.CheckKeyword("main", Color.Green, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parser_Form parse = new Parser_Form();
            parse.Show();
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
