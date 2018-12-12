using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JASON_Compiler
{

    class SemanticAnalyser
    {
        //            Dictionary<KeyValuePair<variable, scope>, List<KeyValuePair<attribute, value>>>  (ex:attribute is "datatype" or "value")                           
        public static Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, object>>> SymbolTable = new Dictionary<KeyValuePair<string,string>,List<KeyValuePair<string,object>>>();


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////SEMANTIC CODE HERE///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void handelidlist(Node root)
        {
            List<KeyValuePair<string, object>> DicList = new List<KeyValuePair<string, object>>();
            DicList.Add(new KeyValuePair<string, object>("DataType", root.datatype));
            if (root.children[0].Name.ToLower() == "idlist")
            {
                root.children[0].datatype = root.datatype;
                handelidlist(root.children[0]);
                root.children[2].datatype = root.datatype;
                SymbolTable.Add(root.children[2].Name, DicList);
            }
            else
            {
                root.children[0].datatype = root.datatype;
                SymbolTable.Add(root.children[0].Name, DicList);
            }

        }
        public static void handelVardecl(Node root)
        {
            root.children[0].datatype = root.children[0].children[0].Name;
            root.children[1].datatype = root.children[0].children[0].Name;
            handelidlist(root.children[1]);
        }
        //public static void handelparamdecl(Node root)
        //{
        //    root.datatype = root.children[0].children[0].Name;
        //}
        public static void traverseTree(Node root)
        {
            if (root.Name.ToLower() == "vardecl")
            {
                handelVardecl(root);
            }
            //else if (root.Name.ToLower() == "param decl")
            //{
            //    handelparamdecl(root);
            //}

            else
            {
                foreach (Node child in root.children)
                {
                    traverseTree(child);
                }
            }

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static TreeNode PrintSemanticTree(Node root)
        {
            TreeNode tree = new TreeNode("Annotated Tree");
            TreeNode treeRoot = PrintAnnotatedTree(root);
            tree.Expand();
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintAnnotatedTree(Node root)
        {
            traverseTree(root);
            if (root == null)
                return null;

            TreeNode tree;
            if (root.value == Int32.MinValue && root.datatype == "")
                tree = new TreeNode(root.Name);
            else if (root.value != Int32.MinValue && root.datatype == "")
                tree = new TreeNode(root.Name + " & its value is: " + root.value);
            else if (root.value == Int32.MinValue && root.datatype != "")
                tree = new TreeNode(root.Name + " & its datatype is: " + root.datatype);
            else
                tree = new TreeNode(root.Name + " & its value is: " + root.value + " & datatype is: " + root.datatype);
            tree.Expand();
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintAnnotatedTree(child));
            }
            return tree;
        }
    }
}
