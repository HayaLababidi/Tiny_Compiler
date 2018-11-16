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

            return root;
        }

        public static Node match(Token_Class expected_tok,List<Token> Tokens)
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
