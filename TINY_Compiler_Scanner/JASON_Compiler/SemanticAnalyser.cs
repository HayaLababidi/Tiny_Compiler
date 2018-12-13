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
        //            Dictionary<funName,List<parameters<name, datatype>>> (num of parameters is the length of the list)
        public static Dictionary<string, List<KeyValuePair<string, string>>> FunctionTable = new Dictionary<string,List<KeyValuePair<string,string>>>();


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////SEMANTIC CODE HERE///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //p1_____________
        /*  Declaration_Statement . Datatype Declared_Var_list;
            Declared_Var . identifier Declared_Var’
            Declared_Var’ . Assignment_op Expression | .
            Declared_Vars . , Declared_Var Declared_Vars | .
            Declared_Var_list . Declared_Var Declared_Vars
        */
        public static void handle_Main_Function(Node root)
        {
            root.children[4].scope = "main";//function body
        }
        public static void handle_Function_Statement(Node root)
        {
            root.children[1].scope = root.children[0].children[1].token.lex;//function body scope = function name
        }
        public static void handle_Function_body(Node root)
        {
            foreach (Node child in root.children)
            {
                child.scope = root.scope;
            }
        }
        public static void handle_Statements(Node root)
        {
            root.children[0].children[0].scope = root.scope;
            traverseTree(root.children[0].children[0]);
            traverseTree(root.children[1]);
            
        }
        public static void handle_Declaration_Statement(Node root)
        {
            root.children[0].datatype = root.children[0].children[0].token.lex;//Datatype
            root.children[1].datatype = root.children[0].children[0].token.lex;//Declared_Var_list
            root.children[1].scope = root.scope;
            handle_Declared_Var_list(root.children[1]);
        }
        public static void handle_Declared_Var_list(Node root)
        {

            root.children[0].datatype = root.datatype;//Declared_Var
            root.children[0].scope = root.scope;//Declared_Var
            handle_Declared_Var(root.children[0]);
            if (root.children.Count > 1)
            {
                root.children[1].datatype = root.datatype;//Declared_Vars
                root.children[1].scope = root.scope;//Declared_Vars
                handle_Declared_Vars(root.children[1]);
            }
        }
        public static void handle_Declared_Vars(Node root)
        {
            root.children[1].datatype = root.datatype;//Declared_Var
            root.children[1].scope = root.scope;//Declared_Var
            handle_Declared_Var(root.children[1]);
            if (root.children.Count > 2)
            {
                root.children[2].datatype = root.datatype;//Declared_Vars
                root.children[2].scope = root.scope;//Declared_Vars
                handle_Declared_Vars(root.children[2]);
            }
        }
        public static void handle_Declared_Var(Node root)
        {

            root.children[0].datatype = root.datatype;//identifier
            root.children[0].scope = root.scope;
            KeyValuePair<string, string> var = new KeyValuePair<string, string>(root.children[0].token.lex, root.children[0].scope);
            List<KeyValuePair<string, object>> prop=new List<KeyValuePair<string,object>>();
            prop.Add(new KeyValuePair<string,object>("Datatype",root.datatype));
            if (SymbolTable.ContainsKey(var))
            {
                Errors.Analyser_Error_List.Add("A local variable named " + var.Key + " is already defined in this scope");
            }
            else
            {
                SymbolTable.Add(var,prop);
            }
            if (root.children.Count > 1)
            {
                root.children[1].datatype = root.datatype;//Declared_Var’
                object value=handle_Declared_Var_Dash(root.children[1]);
                if (value != null)
                {
                    SymbolTable[var].Add(new KeyValuePair<string, object>("Value", value));
                }
            }
        }
        public static object handle_Declared_Var_Dash(Node root) 
        {
            KeyValuePair<string, object> Assignedvalue = EvaluateExpression(root.children[1]);//expression
            if (Assignedvalue.Key == root.datatype)
            {
                if (Assignedvalue.Value != null)
                {
                    return Assignedvalue.Value;
                }
            }
            else
            {
                Errors.Analyser_Error_List.Add("Data type of Expression is not the same as Declared Variable dataType");
            }
            return null;
        }
        //endP1
        
        //p2_____________
        public static void handleIdentifier(Node root) { }
        public static void handleTerm(Node root) { }

        //variables of type object can accept values of any data type (the value)
        //string is must be the datatype 
        //if the value can't be computed return null (for example expression=var and var does not have a value)
        public static KeyValuePair<string, object> EvaluateExpression/*get value*/(Node root)//must evaluate the value of the expression and its datatype
        { return new KeyValuePair<string, object>(); }

        //public static void getValue(Node root) { }//dont handle strings ;int /real only 

        //endP2

        //Mai-p3_____________
        /*

        Else_Statment → else Statements end
        */
        public static void handleCondition(Node root)
        {//Condition → Identifier Condition_Operator Term 
            Node identifier = root.children[0];
            Node rightHS = root.children[2];
            handleIdentifier(identifier);
            handleTerm(rightHS);
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
                        Errors.Analyser_Error_List.Add(error);
                        break;
                }
                
            }
        }
        public static void handleCondition_Statement(Node condition_statment)
        {//Condition_Statement → Condition Boolean_Exp
         // x==5    (|| y==7 && z>5)
            Node condition = condition_statment.children[0];
            Node boolean_exp = condition_statment.children[1];
            handleCondition(condition);
            handleBool_Exp(boolean_exp);
            evaluateCondition_Statement(condition_statment);
            //check if it actually works 
        }
        public static void handleBool_Exp(Node boolean_exp)
        {   // x==5       ||        y==7      &&    z>5
            // condition  0operator 1condition  2.0oprator 2.1condition  ε
            //boolean_opertor || condition y==7 boolean expression &&z>5
            if (boolean_exp.children.Count>0)
            {//Boolean_Exp → Boolean_Operator Condition Boolean_Exp
                handleCondition(boolean_exp.children[1]);
                handleBool_Exp(boolean_exp.children[2]);
                string child_boolean_operator = boolean_exp.children[2].children[0].token.lex;
                bool condition = boolean_exp.children[1].value == 1 ? true : false;
                bool child_condition = boolean_exp.children[2].children[1].value == 1 ? true : false;
                bool value;
                switch (child_boolean_operator)
                {
                    case "||":
                        value = condition || child_condition;
                        boolean_exp.value = value == true ? 1 : 0;
                        break;
                    case "&&":
                        value = condition && child_condition;
                        boolean_exp.value = value == true ? 1 : 0;
                        break;
                    default:
                        string error = "wrong operator :{0} should be a boolean operator";
                        Errors.Analyser_Error_List.Add(string.Format(error, child_boolean_operator));
                        break;
                }

            }
            //else Boolean_Exp →ε

        }
        public static void evaluateCondition_Statement(Node condition_statment)
        {
            // x==5    (|| y==7 && z>5)
            //Boolean_Exp → Boolean_Operator Condition Boolean_Exp | ε
            //boolean_opertor || condition y==7 boolean expression &&z>5
            Node boolean_exp = condition_statment.children[1];
            string boolean_operator = boolean_exp.children[0].token.lex;
            bool bool_expval = boolean_exp.value == 1 ? true : false;
            bool conditionval = condition_statment.children[0].value == 1 ? true : false;
            bool value;//=true;
            switch (boolean_operator)
            {
                case "||":
                    value = conditionval|| (bool_expval);
                    condition_statment.value = value == true ? 1 : 0;
                    break;
                case "&&":
                    value = conditionval && (bool_expval);
                    condition_statment.value = value == true ? 1 : 0;
                    break;
                default:
                    string error = "wrong operator :{0} should be a boolean operator";
                    Errors.Analyser_Error_List.Add(string.Format(error,boolean_operator));
                    break;

            }
        }
        public static void handleIf(Node root)
        {//If_Statement → if Condition_Statement then Statements Else_part
            handleCondition_Statement(root.children[1]);
            handleElse_Part(root.children[4]);
                //else part
        }
        public static void handleElse_Part(Node else_part)
        {// Else_part → Else_If_Statment | Else_Statment | end
            if (else_part.children[0].children[0].token.lex=="elseif")
            {
                handleElse_if_statment(else_part.children[0]);
            }
            
        }
        public static void handleElse_if_statment(Node elseifstatment)
        {// Else_If_Statement → elseif Condition_Statement then Statements Else_part
            handleCondition_Statement(elseifstatment.children[1]);
            handleElse_Part(elseifstatment.children[4]);
        }
        public static void handleRepeat_Statement(Node elseifstatment)
        {//Repeat_Statement→ repeat Statements until Condition_Statement
            handleCondition_Statement(elseifstatment.children[3]);
        }

        //endMai-p3


        public static void traverseTree(Node root)
        {
            if (root.Name.ToLower() == "vardecl")
            {
                
            }
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
