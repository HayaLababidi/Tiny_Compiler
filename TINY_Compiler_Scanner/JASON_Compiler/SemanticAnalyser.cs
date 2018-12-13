﻿using System;
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
        //p1_____________
        //endP1
        
        //p2_____________
        public static void handelIdentifier(Node root) { }
        public static void handelTerm(Node root) { }

        //public static void getValue(Node root) { }//dont handel strings ;int /real only 

        //endP2

        //Mai-p3_____________
        /*
        Condition_Statement → Condition Boolean_Exp
        Boolean_Exp → Boolean_Operator Condition Boolean_Exp | ε
        If_Statement → if Condition_Statement then Statements Else_part
        Else_part → Else_If_Statment | Else_Statment | end
        Else_If_Statement → elseif Condition_Statement then Statements Else_part
        Else_Statment → else Statements end
        Repeat_Statement→ repeat Statements until Condition_Statement*/
        public static void handelCondition(Node root)
        {//Condition → Identifier Condition_Operator Term 
            Node identifier = root.children[0];
            Node rightHS = root.children[2];
            handelIdentifier(identifier);
            handelTerm(rightHS);
            if (rightHS.datatype == identifier.datatype)
            {
                root.datatype = identifier.datatype;
                //setting the value   
                evaluateCondition(root);
            }
            else
            {
                string error = @"wrong data type:can't compare {0} {1} (left hand side) to {2} right hand side ";
                Errors.Analyser_Error_List.Add(string.Format(error, identifier.datatype ,identifier.token.lex,root , rightHS.datatype));
            }
        }
        public static void evaluateCondition(Node condition)
        {//true/false
            Node Cond_op = condition.children[1];
            Node RHS = condition.children[0];
            Node LHS = condition.children[2];
            bool boolvalue = true;
            if (condition.datatype == "string")
            {
                switch (Cond_op.token.lex)
                {
                    case "<>":
                        boolvalue = LHS.token.lex != RHS.token.lex;
                        condition.value = boolvalue == true ? 1 : 0;
                        //if boolvalue=true condition.value=1 else 0 
                        break;

                    case "=":
                        boolvalue = LHS.token.lex == RHS.token.lex;
                        condition.value = boolvalue == true ? 1 : 0;
                        break;

                    default:
                        string error = "operator {0} can't be used with string condtion";
                        Errors.Analyser_Error_List.Add(string.Format(error, Cond_op.token.lex));
                        break;
                }
                 
                
            }
            else//real / string 
            {
                switch (Cond_op.token.lex)
                {
                    case "<>":
                        boolvalue = LHS.value != RHS.value;
                        condition.value = boolvalue == true ? 1 : 0;
                        break;

                    case "=":
                        boolvalue = LHS.value == RHS.value;
                        condition.value = boolvalue == true ? 1 : 0;
                        break;

                    case ">":
                        boolvalue = LHS.value > RHS.value;
                        condition.value = boolvalue == true ? 1 : 0;
                        break;
                    case "<":
                        boolvalue = LHS.value < RHS.value;
                        condition.value = boolvalue == true ? 1 : 0;
                        break;
                    default:
                        string error = "wrong operator";
                        Errors.Analyser_Error_List.Add(string.Format(error, Cond_op.token.lex));
                        break;
                }
                
            }
        }
        //endMai-p3

        //this section was already here 
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