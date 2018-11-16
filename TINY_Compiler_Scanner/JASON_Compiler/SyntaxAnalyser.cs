using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> children = new List<Node>();
        public Token token;
    }
    class SyntaxAnalyser
    {
        static int index = 0;
        public static Node Parse(List<Token> Tokens)
        {
            Node root= new Node();
            root.token = new Token();

            //write your parser code
            root.children.Add(Match(Token_Class.Program, Tokens));
            root.children.Add(Header(Tokens));
            root.children.Add(DecSec(Tokens));
            root.children.Add(Block(Tokens));
            return root;
        }

        public static Node Match(Token_Class expected_tok,List<Token> Tokens)
        {
            if (expected_tok == Tokens[index].token_type)
            {
                Node terminal = new Node();
                terminal.token = Tokens[index];
                index++;
                return terminal;
            }
            else
            {
                //error
                Node terminal = new Node();
                return null;
            }
        }

        public static Node Exp(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Exp";
            returned_node.children.Add(Term(Tokens));
            returned_node.children.Add(Exp_(Tokens));
            return returned_node;
        }
        public static Node Exp_(List<Token> Tokens)
        {// Add_op Term Exp’ | ε
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Exp";
            if (Tokens[index].token_type == Token_Class.MinusOp|| Tokens[index].token_type == Token_Class.PlusOp)
            {// Add_op Term Exp’
                returned_node.children.Add(Add_op(Tokens));
                returned_node.children.Add(Term(Tokens));
                returned_node.children.Add(Exp_(Tokens));
            }
            //| ε
            return returned_node;
        }
        public static Node Add_op(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Addop";
            if (Tokens[index].token_type == Token_Class.MinusOp )
            {
                returned_node.children.Add(Match(Token_Class.MinusOp, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.PlusOp)
            {
                returned_node.children.Add(Match(Token_Class.MinusOp, Tokens));
            }
            else
            {
                returned_node.token.lex = "ERROR";
                return returned_node;
            }
            return returned_node;
        }
        public static Node Mul_op(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Mulop";
            if (Tokens[index].token_type == Token_Class.MultiplyOp)
            {
                returned_node.children.Add(Match(Token_Class.MultiplyOp, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.DivideOp)
            {
                returned_node.children.Add(Match(Token_Class.DivideOp, Tokens));
            }
            else
            {
                returned_node.token.lex = "ERROR";
                return returned_node;
            }
            return returned_node;
        }
        public static Node Term(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Term";
            returned_node.children.Add(Factor(Tokens));
            returned_node.children.Add(Term_(Tokens));
            return returned_node;
        }
        public static Node Term_(List<Token> Tokens)
        {// Term’ → Mul-op Factor Term’ | ε 
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Exp";
            if (Tokens[index].token_type == Token_Class.DivideOp|| Tokens[index].token_type == Token_Class.MultiplyOp)
            {// Mul-op Factor Term’
                returned_node.children.Add(Mul_op(Tokens));
                returned_node.children.Add(Factor(Tokens));
                returned_node.children.Add(Term_(Tokens));
            }
            //| ε
            return returned_node;
        }
        public static Node Factor(List<Token> Tokens)
        {// Factor → (Exp) | number
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Factor";
            if (Tokens[index].token_type == Token_Class.Number)
            {// Mul-op Factor Term’
                returned_node.children.Add(Match(Token_Class.Number,Tokens));
            }
            else
            {
                returned_node.children.Add(Match(Token_Class.LParanthesis, Tokens));
                returned_node.children.Add(Exp(Tokens));
                returned_node.children.Add(Match(Token_Class.RParanthesis, Tokens));
            }
            //| ε
            return returned_node;
        }
        public static Node Assignment_stmnt(List<Token> Tokens)
        {//Assignment statement →  Identifier Assignment_operator exp 

            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Assignment statement";
            returned_node.children.Add(Match(Token_Class.Identifier,Tokens));
            returned_node.children.Add(Assignment_op(Tokens));
            returned_node.children.Add(Exp(Tokens));

            return returned_node;
        }
        public static Node Assignment_op(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Assignment operator";
            index++;// cause match will only match to one of them 
            returned_node.children.Add(Match(Token_Class.AssignmentOp, Tokens));
            return returned_node;
        }
        //use this function to print the parse tree in TreeView Toolbox
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if(treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.token == null)
                return null;
            TreeNode tree = new TreeNode(root.token.lex);
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
