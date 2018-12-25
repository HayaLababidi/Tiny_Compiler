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
        public struct functionTableValue
        {
            public string datatype;
            public List<KeyValuePair<string, string>> parameters;
            public int numOFparam;
        }
        public struct matchParameters
        {
            public bool numOFArguments;
            public bool dataTypeOFArguments;
        }
        public class Value_Type
        {
            public object datatype;
            public object value;
        }
        //            Dictionary<KeyValuePair<variable, scope>, List<KeyValuePair<attribute, value>>>  (ex:attribute is "datatype" or "value")                           
        public static Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, object>>> SymbolTable = new Dictionary<KeyValuePair<string,string>,List<KeyValuePair<string,object>>>();
        //            Dictionary<funName,List<parameters<name, datatype>>> (num of parameters is the length of the list)
        public static Dictionary<string, functionTableValue> FunctionTable = new Dictionary<string, functionTableValue>();

        //to maintain the sequance number of each subscope 
        public static Dictionary<String, int> scopeCount = new Dictionary<string, int>();


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////SEMANTIC CODE HERE///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //utitlity function 
        static void setscope(Node root) 
        {
            if (root != null)
            {
                for (int i = 0; i < root.children.Count; i++)
                {
                    if (root.children[i] != null)
                        root.children[i].scope = root.scope;
                }
            }
        }
        static bool sameScope(string parentscope, string currentscope)
        {
            return currentscope.StartsWith(parentscope);
        }
        # region p1_____________
        /*  Declaration_Statement . Datatype Declared_Var_list;
            Declared_Var . identifier Declared_Var’
            Declared_Var’ . Assignment_op Expression | .
            Declared_Vars . , Declared_Var Declared_Vars | .
            Declared_Var_list . Declared_Var Declared_Vars
        */

        public static void handle_Main_Function(Node root)
        {
            root.scope = "main";
            root.datatype = root.children[0].children[0].token.lex;
            root.children[4].datatype = root.datatype;

            root.children[4].scope = "main";//function body
            handle_Function_body(root.children[4]);

            functionTableValue value = new functionTableValue();
            string funName = root.children[1].token.lex;
            value.datatype = root.datatype;
            value.numOFparam = 0;
            if (!FunctionTable.ContainsKey(funName))
                FunctionTable.Add(funName, value);
            else
                Errors.Analyser_Error_List.Add(funName + " Already declared function");
        }

        public static void handle_Function_Statement(Node root)
        {
            root.scope = root.children[0].children[1].token.lex;//function body scope = function name
            root.datatype = root.children[0].children[0].children[0].token.lex;
            root.children[1].datatype = root.datatype;
            setscope(root);
            handle_function_declaration(root.children[0]);//H
            handle_Function_body(root.children[1]);//H
        }
        public static void handle_Function_body(Node root)
        {
            setscope(root);
            if (root.children[1]!=null)
            {
                handle_Statements(root.children[1]);
                if(root.datatype !="void")
                {
                    root.children[2].datatype = root.datatype;
                    handle_Function_return(root.children[2]);
                }
            }
            else
            {
                if (root.datatype != "void")
                {
                    root.children[2].datatype = root.datatype;
                    handle_Function_return(root.children[2]);
                }
            }
        }
        public static void handle_Function_return(Node root)
        {
            setscope(root);
            setscope(root.children[1]);
            Value_Type ret = handle_Expression(root.children[1]);
            if (ret != null)
            {
                root.children[1].value = ret.value;
            }
            if (root.datatype != (string)ret.datatype)
            {
                Errors.Analyser_Error_List.Add(root.children[1].datatype + " return datatype incompatible" + (string)ret.datatype);
            }
        }
        public static void handle_Statements(Node root)
        {
            root.children[0].children[0].scope = root.scope;
            setscope(root.children[0].children[0]);
            if (root.children[0].children[0].token.lex.ToLower() == "declaration_statement")
            {
                handle_Declaration_Statement(root.children[0].children[0]);
            }
            else if (root.children[0].children[0].token.lex.ToLower() == "assignment statement")
            {
                handle_Assignment_statement(root.children[0].children[0]);
            }
            else if (root.children[0].children[0].token.lex.ToLower() == "write_statement")
            {
                handle_Write(root.children[0].children[0]);
            }
            else if (root.children[0].children[0].token.lex.ToLower() == "read_statement")
            {
                //read_statement
            }
            else if (root.children[0].children[0].token.lex.ToLower() == "if_statement")
            {
                handleIf(root.children[0].children[0]);
            }
            else if (root.children[0].children[0].token.lex.ToLower() == "repeat_statement")
            {
                handleRepeat_Statement(root.children[0].children[0]);
            }
            else if (root.children[0].children[0].token.lex.ToLower() == "function_call")
            {
                handle_function_call(root.children[0].children[0]);
            }
            if (root.children[1] != null)
            {
                root.children[1].scope = root.scope;
                handle_Statements(root.children[1]);
            }
            
        }

        public static void handle_Declaration_Statement(Node root)
        {
            root.children[0].datatype = root.children[0].children[0].token.lex;//Datatype
            root.children[1].datatype = root.children[0].children[0].token.lex;//Declared_Var_list
            root.children[1].scope = root.scope;
            setscope(root);
            handle_Declared_Var_list(root.children[1]);
        }

        public static void handle_Declared_Var_list(Node root)
        {

            root.children[0].datatype = root.datatype;//Declared_Var
            root.children[0].scope = root.scope;//Declared_Var
            setscope(root);
            handle_Declared_Var(root.children[0]);
            if (root.children[1] != null)
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
            setscope(root);
            handle_Declared_Var(root.children[1]);
            if (root.children[2]!=null)
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
            setscope(root);
            KeyValuePair<string, string> var = new KeyValuePair<string, string>(root.children[0].token.lex, root.children[0].scope);
            List<KeyValuePair<string, object>> prop=new List<KeyValuePair<string,object>>();
            prop.Add(new KeyValuePair<string,object>("Datatype",root.datatype));
            bool check=false;
            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Key == var.Key)//same variable check scope
                {
                    if (sameScope(variable.Value, var.Value))
                    {
                        Errors.Analyser_Error_List.Add("A local variable named " + var.Key + " is already defined in this scope");
                        check=true;
                    }
                }
            }
            if(!check)
            {
                SymbolTable.Add(var,prop);
            }
            if (root.children[1]!=null)
            {
                root.children[1].datatype = root.datatype;//Declared_Var’
                object value=handle_Declared_Var_Dash(root.children[1]);
                if (value != null)
                {
                    SymbolTable[var].Add(new KeyValuePair<string, object>("Value", value));
                    root.children[0].value = value;
                }
            }
        }

        public static object handle_Declared_Var_Dash(Node root) 
        {
            Value_Type returned_Exp;
            setscope(root);
            if (root.children[1].token.lex == "Expression")
            {
                returned_Exp = handle_Expression(root.children[1]);
            }
            else if (root.children[1].children[1].token.lex == "Expression")
            {
                returned_Exp = handle_Expression(root.children[1].children[1]);
            }
            else
            {
                Errors.Analyser_Error_List.Add("unhandeled exception can't find Expression");
                return null;
            }
            if (returned_Exp.datatype.ToString().ToLower() == root.datatype.ToString().ToLower())
            {
                if (returned_Exp.value != null)
                {
                    return returned_Exp.value;
                }
            }
            else
            {
                Errors.Analyser_Error_List.Add("Data type of Expression is not the same as Declared Variable dataType");
            }
            return null;
        }
        public static void handle_Write(Node root)
        {
            setscope(root);
            if (root.children[1].children[0].token.token_type == Token_Class.Endl)
            {
                root.children[1].value = root.children[1].children[0].token.lex;
            }
            else//expression
            {
                setscope(root.children[1]);
                Value_Type ret =handle_Expression(root.children[1].children[0]);
                if (ret != null)
                {
                    root.children[1].value = ret.value;
                    root.children[1].datatype = (string)ret.datatype;
                }
            }
        }

        //endP1
# endregion

        # region p2_____________
        public static Value_Type handle_Identifier(Node root)
        {
            setscope(root);
            Value_Type val = new Value_Type();
            //KeyValuePair<string, string> list_key = new KeyValuePair<string, string>(root.children[0].token.lex, root.children[0].scope);
            KeyValuePair<string, string> list_key = new KeyValuePair<string, string>(root.token.lex, root.scope);//H
            List<KeyValuePair<string, object>> attributes = new List<KeyValuePair<string, object>>();

            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Key == list_key.Key)//same variable check scope
                {
                    if (sameScope(variable.Value, list_key.Value))
                    {
                        attributes = SymbolTable[variable];
                        foreach (KeyValuePair<string, object> element in attributes)
                        {
                            if (element.Key == "Value")
                                val.value = element.Value;
                            if (element.Key == "Datatype")
                                val.datatype = element.Value;
                        }
                        root.value = val.value;
                        root.datatype = val.datatype.ToString();
                        //root.children[0].value=val.value;
                        //val.datatype = "identifier";
                        return val;
                    }
                }
            }
            //H
            Errors.Analyser_Error_List.Add("Use of undeclared variable \" " + list_key.Key + "\"");
            return null;
            
            
        }

        //Expression → string | Term | Equation
        public static Value_Type handle_Expression(Node root)
        {
            setscope(root);
            // call when want to get value and data type of expression
            Value_Type val = new Value_Type();

            if (root.children[0].token.lex == "Term")
            {
                val = handleTerm(root.children[0]);
            }

            else if (root.children[0].token.lex == "Equation")
            {
                val = handle_Equation(root.children[0]);
            }

            else if (root.children[0].token.token_type == Token_Class.String)
            {
                val.value = root.children[0].token.lex;
                val.datatype = "String";
            }

            return val;
        }

        //Term → number | floatnumber |identifier | Function_Call
        public static Value_Type handleTerm(Node root)
        {
            Value_Type val = new Value_Type();
            setscope(root);
            if (root.children[0].token.lex == "Function_Call")
            {
                val.datatype = handle_function_call(root.children[0]);
            }

            else if (root.children[0].token.token_type == Token_Class.Identifier)
            {
                bool check=false;
                foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
                {
                    if (variable.Key == root.children[0].token.lex)//same variable check scope
                    {
                        if (sameScope(variable.Value, root.children[0].scope))
                        {
                            val = handle_Identifier(root.children[0]);
                            check=true;
                        }
                    }
                }
                if (!check)
                    Errors.Analyser_Error_List.Add("Variable " + root.children[0].token.lex + " not found");
            }

            else if (root.children[0].token.token_type == Token_Class.Number)
            {
                val.value =int.Parse(root.children[0].token.lex);
                val.datatype = "int";
                
            }

            else if (root.children[0].token.token_type == Token_Class.FloatNumber)
            {
                val.value = float.Parse( root.children[0].token.lex);
                val.datatype = "float";
            }
            if (val != null)//H
            {
                root.value = val.value;
                root.datatype = (string)val.datatype;
            }
            return val;
        }

        //Equation → MathTerm Equation’ 
        public static Value_Type handle_Equation(Node root)
        {
            setscope(root);
            List<Value_Type> val = new List<Value_Type>();
            List<Value_Type> val_sorted = new List<Value_Type>();
            Value_Type result = new Value_Type();
            Scanner scan = new Scanner();
            object res = new object();

            if (root.children[0].token.lex == "MathTerm")
            {
                List<Value_Type> mathterm_list = new List<Value_Type>();
                mathterm_list = handle_MathTerm(root.children[0]);
                for (int i = 0; i < mathterm_list.Count; i++)
                    val.Add(mathterm_list[i]);
            }

            if (root.children[1].token.lex == "Equation'")
            {
                List<Value_Type> equ_list = new List<Value_Type>();
                equ_list = handle_MathTerm(root.children[0]);
                for (int i = 0; i < equ_list.Count; i++)
                    val.Add(equ_list[i]);
            }

            

            // check value DataType
            object datatype = val[0].datatype;
            result.datatype = datatype;
            for (int i = 0; i < val.Count;)
            {
                if(val[i].datatype.ToString() == "leftP")
                {
                    i++;
                }
                else if(val[i].datatype.ToString() == "rightP")
                {
                    i += 2;
                }
                else if (val[i].datatype != datatype)
                {
                    Errors.Analyser_Error_List.Add("Different Datatype");
                     i += 2;
                }
            }
               
            //------------------------ Calculations

            //after calculate each part remove it and replace with the value

            //Add in val_sorted
            for (int i = 0; i < val.Count; i++)
            {
                if (scan.Operators.ContainsKey(val[i].value.ToString()))
                {
                    if (val[i].datatype.ToString() == "leftP")
                    {
                        val.Remove(val[i]);
                        i++;
                        List<Value_Type> bracket_equ = new List<Value_Type>();
                        for(int j=i;val[j].datatype.ToString() != "rightP";j++)
                        {
                            bracket_equ.Add(val[j]);
                            val.Remove(val[j]);
                            i++;
                        }
                        val.Remove(val[i]);
                        Addin_sorted_list(bracket_equ, val_sorted);

                    }
                }
            }


            Addin_sorted_list(val, val_sorted);

            //-------------------Calculate result

            List<object> number1 = new List<object>();
            int num2 = 0;
            float f_num2 = 0;

            
            for (int i = 0; i < val_sorted.Count; i++)
            {
                # region if we have two values Consecutive
                if (val_sorted[i].datatype.ToString() == "int" || val_sorted[i].datatype.ToString() == "float")
                {
                    if(val_sorted[i+1].datatype.ToString() == "int" || val_sorted[i+1].datatype.ToString() == "float")
                    {
                        if(val_sorted[i+2].datatype.ToString() == "plus")
                        {
                            if(datatype.ToString() == "int")
                                 res = Convert.ToInt32(val_sorted[i].value) + Convert.ToInt32(val_sorted[i+1].value);
                            else if(datatype.ToString() == "float")
                                 res = (float)val_sorted[i].value + (float)val_sorted[i+1].value;

                            number1.Add(res);
                            i += 2;
                        }

                        if (val_sorted[i + 2].datatype.ToString() == "minus")
                        {
                             if(datatype.ToString() == "int")
                                 res = Convert.ToInt32(val_sorted[i].value) - Convert.ToInt32(val_sorted[i+1].value);
                            else if(datatype.ToString() == "float")
                                 res = (float)val_sorted[i].value - (float)val_sorted[i+1].value;
                             
                            number1.Add(res);
                            i += 2;
                        }

                        if (val_sorted[i + 2].datatype.ToString() == "mul")
                        {
                             if(datatype.ToString() == "int")
                                 res = Convert.ToInt32(val_sorted[i].value) * Convert.ToInt32(val_sorted[i+1].value);
                            else if(datatype.ToString() == "float")
                                 res = (float)val_sorted[i].value * (float)val_sorted[i+1].value;
                            
                            number1.Add(res);
                            i += 2;
                        }

                        if (val_sorted[i + 2].datatype.ToString() == "div")
                        {
                            if (Convert.ToInt32(val_sorted[i - 1].value) == 0)
                            {
                                Errors.Analyser_Error_List.Add("Can't divid by 0");
                                break;
                            }
                                
                            else
                            {
                                if (datatype.ToString() == "int")
                                    res = Convert.ToInt32(val_sorted[i].value) / Convert.ToInt32(val_sorted[i + 1].value);
                                else if (datatype.ToString() == "float")
                                    res = (float)val_sorted[i].value / (float)val_sorted[i + 1].value;
                               
                                number1.Add(res);
                                i += 2;
                            }
                            
                        }

                    }

                    num2 = Convert.ToInt32(number1[0]);
                    f_num2 = (float)number1[0];
                }
            # endregion
                # region if we have value followed by symbol

                else if(val_sorted[i].datatype.ToString() == "int" || val_sorted[i].datatype.ToString() == "float")
                {
                    if (val_sorted[i + 1].datatype.ToString() == "plus")
                    {
                        
                        if (datatype.ToString() == "int")
                            num2 += Convert.ToInt32(val_sorted[i].value);
                        else if (datatype.ToString() == "float")
                            f_num2 += (float)val_sorted[i].value;
                        i++;
                    }

                    if (val_sorted[i + 1].datatype.ToString() == "minus")
                    {
                        if (datatype.ToString() == "int")
                            num2 -= Convert.ToInt32(val_sorted[i].value);
                        else if (datatype.ToString() == "float")
                            f_num2 -= (float)val_sorted[i].value;
                        i++;
                    }

                    if (val_sorted[i + 1].datatype.ToString() == "mul")
                    {
                        if (datatype.ToString() == "int")
                            num2 *= Convert.ToInt32(val_sorted[i].value);
                        else if (datatype.ToString() == "float")
                            f_num2 *= (float)val_sorted[i].value;
                        i++;
                    }

                    if (val_sorted[i + 1].datatype.ToString() == "div")
                    {
                        if (Convert.ToInt32(val_sorted[i].value) == 0)
                        {
                            Errors.Analyser_Error_List.Add("Can't divid by 0");
                            break;
                        }
                        else
                        {
                            if (datatype.ToString() == "int")
                                num2 /= Convert.ToInt32(val_sorted[i].value);
                            else if (datatype.ToString() == "float")
                                f_num2 /= (float)val_sorted[i].value;
                            i++;
                        }
                    }
                }
                # endregion
                #region if we have one symbol
                else if(val_sorted[i].datatype.ToString() == "plus" || val_sorted[i].datatype.ToString() == "minus" || val_sorted[i].datatype.ToString() == "mul" || val_sorted[i].datatype.ToString() == "div")
                {
                    for (int k = 1; k < number1.Count; k++)
                    {
                        if (val_sorted[i].datatype.ToString() == "plus")
                        {
                            num2 += Convert.ToInt32(number1[k]);
                            f_num2 += (float)number1[k];
                        }

                        else if (val_sorted[i].datatype.ToString() == "minus")
                        {
                            num2 -= Convert.ToInt32(number1[k]);
                            f_num2 -= (float)number1[k];
                        }

                        else if (val_sorted[i].datatype.ToString() == "mul")
                        {
                            num2 *= Convert.ToInt32(number1[k]);
                            f_num2 *= (float)number1[k];
                        }

                        else if (val_sorted[i].datatype.ToString() == "div")
                        {
                            if (Convert.ToInt32(val_sorted[i].value) == 0)
                            {
                                Errors.Analyser_Error_List.Add("Can't divid by 0");
                                break;
                            }
                            else
                            {
                                num2 /= Convert.ToInt32(number1[k]);
                                f_num2 /= (float)number1[k];
                            }                          
                        }
                    }
                }
#endregion
            }

            if (datatype.ToString() == "int")
                result.value = num2;
            else if (datatype.ToString() == "float")
                result.value = f_num2;

                return result;
        }

        //Add in sorted list
        private static void Addin_sorted_list(List<Value_Type> val, List<Value_Type> val_sorted)
        {
            Scanner scan = new Scanner();
            bool addition_ch = false;
            for (int i = 1; i < val.Count; i += 2)
            {
                if (scan.Operators.ContainsKey(val[i].value.ToString()))
                {
                    //check if op == + || - , make bool two values ----- 4+5*7+8*10 -----> 57*(val1)810*(val2)+4+

                    if (val[i].datatype.ToString() == "plus" || val[i].datatype.ToString() == "minus")
                    {
                        addition_ch = true;
                    }

                    if (val[i].datatype.ToString() == "mul" || val[i].datatype.ToString() == "div")
                    {
                        if (i == 1)
                        {
                            val_sorted.Add(val[i - 1]);
                            val_sorted.Add(val[i + 1]);
                            val_sorted.Add(val[i]);
                            val.Remove(val[i - 1]);
                            val.Remove(val[i + 1]);
                            val.Remove(val[i]);
                        }

                        else if (addition_ch)
                        {
                            val_sorted.Add(val[i - 1]);
                            val_sorted.Add(val[i + 1]);
                            val_sorted.Add(val[i]);
                            val.Remove(val[i - 1]);
                            val.Remove(val[i + 1]);
                            val.Remove(val[i]);
                            addition_ch = false;
                        }

                        else
                        {
                            val_sorted.Add(val[i + 1]);
                            val_sorted.Add(val[i]);
                            val.Remove(val[i + 1]);
                            val.Remove(val[i]);
                        }
                    }
                }
            }

            //check if op == + || - , make bool two values ----- 4+5*7+8*10 -----> 57*(val1)810*(val2)4+(val3)+

            for (int i = 0; i < val.Count; i += 2)
            {
                if (val[i].datatype.ToString() == "plus" || val[i].datatype.ToString() == "minus")
                {
                    if (i == 1 && (val[i+1].datatype.ToString() == "int" || val[i+1].datatype.ToString() == "float"))
                    {
                        val_sorted.Add(val[i - 1]);
                        val_sorted.Add(val[i + 1]);
                        val_sorted.Add(val[i]);
                        val.Remove(val[i - 1]);
                        val.Remove(val[i + 1]);
                        val.Remove(val[i]);
                    }
                    else if(val[i+1].datatype.ToString() == "plus" || val[i+1].datatype.ToString() == "minus")
                    {
                        val_sorted.Add(val[i - 1]);
                        val_sorted.Add(val[i]);
                        val_sorted.Add(val[i + 1]);
                        val.Remove(val[i - 1]);
                        val.Remove(val[i + 1]);
                        val.Remove(val[i]);
                    }
                    else if(i == 0 && i == val.Count - 1)
                    {
                        val_sorted.Add(val[i]);
                        val.Remove(val[i]);
                    }
                    else
                    {
                        val_sorted.Add(val[i + 1]);
                        val_sorted.Add(val[i]);
                        val.Remove(val[i - 1]);
                        val.Remove(val[i]);
                    }
                }
            }
        }

        //Equation ‘→ Add_op MathTerm Equation ‘| ε  
        public static List<Value_Type> handle_Equationdash(Node root)
        {
            setscope(root);
            List<Value_Type> val = new List<Value_Type>();

            if (root.children[0].token.lex == "Addop")
                val.Add(handle_AddOp(root.children[0]));

            if (root.children[1].token.lex == "MathTerm")
            {
                List<Value_Type> mathterm_list = new List<Value_Type>();
                mathterm_list = handle_MathTermdash(root.children[1]);
                for (int i = 0; i < mathterm_list.Count; i++)
                {
                    val.Add(mathterm_list[i]);
                }

            }

            if (root.children[2].token.lex == "Equation'")
            {
                List<Value_Type> equdash_list = new List<Value_Type>();
                equdash_list = handle_Equationdash(root.children[2]);
                for (int i = 0; i < equdash_list.Count; i++)
                    val.Add(equdash_list[i]);
            }

            else
                return null;

            return val;
        }

        //Add_op → +|- 
        public static Value_Type handle_AddOp(Node root)
        {
            setscope(root);
            Value_Type val = new Value_Type();

            if (root.children[0].token.token_type == Token_Class.PlusOp)
            {
                val.value = '+';
                val.datatype = "plus";
            }
                
            else if (root.children[0].token.token_type == Token_Class.MinusOp)
            {
                val.value = '-';
                val.datatype = "minus";
            }

            return val;
        }

        //Mul-op → *|/  
        public static Value_Type handle_MulOp(Node root)
        {
            Value_Type val = new Value_Type();
            setscope(root);
            if (root.children[0].token.token_type == Token_Class.MultiplyOp)
            {
                val.value = '*';
                val.datatype = "mul";
            }
            else if (root.children[0].token.token_type == Token_Class.DivideOp)
            {
                val.value = '/';
                val.datatype = "div";
            }

            return val;
        }

        //MathTerm → Factor MathTerm’  
        public static List<Value_Type> handle_MathTerm(Node root)
        {
            setscope(root);
            List<Value_Type> val = new List<Value_Type>();

            if (root.children[0].token.lex == "Factor")
            {
                List<Value_Type> factor_list = new List<Value_Type>();
                //factor_list = handle_Factor(root.children[2]);
                factor_list = handle_Factor(root.children[0]);//H
                for (int i = 0; i < factor_list.Count; i++)
                {
                    val.Add(factor_list[i]);
                }
            }

            //if (root.children[1].token.lex == "MathTerm'")
            if (root.children[1] != null && root.children[1].token.lex == "MathTerm'")//H
            {
                List<Value_Type> mathterm_list = new List<Value_Type>();
                mathterm_list = handle_MathTermdash(root.children[1]);
                for (int i = 0; i < mathterm_list.Count; i++)
                {
                    val.Add(mathterm_list[i]);
                }

            }

            return val;
        }

        //MathTerm ‘→ Mul-op Factor MathTerm ‘| ε  
        public static List<Value_Type> handle_MathTermdash(Node root)
        {
            setscope(root);
            List<Value_Type> val = new List<Value_Type>();

            if (root.children[0].token.lex == "Mulop")
                val.Add(handle_MulOp(root.children[0]));

            if (root.children[1].token.lex == "Factor")
            {
                List<Value_Type> factor_list = new List<Value_Type>();
                factor_list = handle_Factor(root.children[2]);
                for (int i = 0; i < factor_list.Count; i++)
                {
                    val.Add(factor_list[i]);
                }
            }

            if (root.children[2].token.lex == "MathTerm'")
            {
                List<Value_Type> mathterm_list = new List<Value_Type>();
                mathterm_list = handle_MathTermdash(root.children[2]);
                for (int i = 0; i < mathterm_list.Count; i++)
                {
                    val.Add(mathterm_list[i]);
                }

            }

            else
                return null;

            return val;
        }

        //Factor → (Equation) | number  
        public static List<Value_Type> handle_Factor(Node root)
        {
            setscope(root);
            List<Value_Type> val = new List<Value_Type>();

            if (root.children[0].token.token_type == Token_Class.LParanthesis)
            {
                Value_Type leftp = new Value_Type();
                leftp.value = "(";
                leftp.datatype = "leftP";
                val.Add(leftp);

                if (root.children[1].token.lex == "Equation")
                    val.Add(handle_Equation(root.children[1]));

                if (root.children[2].token.token_type == Token_Class.RParanthesis)
                {
                    Value_Type rightp = new Value_Type();
                    rightp.value = ")";
                    rightp.datatype = "rightP";
                    val.Add(rightp);
                }
            }

            //else if (root.children[0].token.token_type == Token_Class.Number || root.children[0].token.token_type == Token_Class.FloatNumber || root.children[0].token.token_type == Token_Class.Identifier)
            else if (root.children[0].token.lex == "Term")//H
            {
                val.Add(handleTerm(root.children[0]));
            }

            return val;
        }

        //Assignment_statement → identifier Assignment_operator Expression
        public static void handle_Assignment_statement(Node root)
        {
            setscope(root);
            bool variable_Found=false;
            List<KeyValuePair<string, object>> attributes = new List<KeyValuePair<string, object>>();
            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Key == root.children[0].token.lex)//same variable check scope
                {
                    if (sameScope(variable.Value, root.children[0].scope))
                    {
                        variable_Found=true;
                        attributes = SymbolTable[variable];
                    }
                }
            }
            if (!variable_Found)
            {
                //Errors.Analyser_Error_List.Add("Variable not found");
                Errors.Analyser_Error_List.Add(root.children[0].token.lex + " Variable not found (undeclared)");//H
            }
            else//H
            {
                KeyValuePair<string, string> list_key = new KeyValuePair<string, string>(root.children[0].token.lex, root.children[0].scope);
                Value_Type returned_Exp = handle_Expression(root.children[2]);//H
                var set_val = new KeyValuePair<string, object>("Value", returned_Exp.value);
                bool novalue = true;//H
                for (int i = 0; i < attributes.Count; i++)//H
                {
                    if (attributes[i].Key == "Value")
                    {
                        attributes[i] = set_val;
                        novalue = false;
                    }
                    if (attributes[i].Key == "Datatype")
                    {
                        if ((attributes[i].Value.ToString().ToLower()) != (returned_Exp.datatype.ToString().ToLower()))
                        {
                            Errors.Analyser_Error_List.Add("Datatype of assigned value and variable are different");
                        }
                    }
                }
                if (novalue && set_val.Value!=null)//H
                {
                    attributes.Add(set_val);
                }
                //attributes[0] = set_val;//H
                root.children[0].datatype = (string)returned_Exp.datatype;
                root.children[0].value = returned_Exp.value;
            }
        }

        //Assignment_op → :=  
        public static Value_Type handle_AssOp(Node root)
        {
            setscope(root);
            Value_Type val = new Value_Type();

            if(root.children[0].token.token_type == Token_Class.AssignmentOp)
                val.value = ":=";

            return val;
        }

        //endP2
        #endregion
        #region Mai-p3_____________
        /*

        Else_Statment → else Statements end
        */
        public static void handleCondition(Node root)
        {//Condition → Identifier Condition_Operator Term 
            setscope(root);//H
            Node identifier = root.children[0];
            Node rightHS = root.children[2];
            handle_Identifier(identifier);//neglecting the return since it fills itin root //Value_Type id = handle_Identifier(identifier);

            //rightHS.value = handleTerm(rightHS);//the returned value is a struct not the actual value
            Value_Type temp = handle_Expression(rightHS);//H
            rightHS.value = temp.value;//H
            rightHS.datatype = (string)temp.datatype;//H
            if (rightHS.datatype == identifier.datatype)
            {
                root.datatype = identifier.datatype;
                //setting the value   
                evaluateCondition(root);
            }
            else
            {
                string error = @"wrong data type:can't compare {0} {1} (left hand side) to {2} right hand side ";
                //Errors.Analyser_Error_List.Add(string.Format(error, identifier.datatype ,identifier.token.lex,root , rightHS.datatype));
                Errors.Analyser_Error_List.Add(string.Format(error, identifier.datatype, identifier.token.lex, rightHS.datatype));//H
            }
        }
        public static void evaluateCondition(Node condition)
        {//true/false
            setscope(condition);
            Node Cond_op = condition.children[1];
            Node RHS = condition.children[2];
            Node LHS = condition.children[0];
            if (LHS.value != null && RHS.value != null)
            {//if none of the variables is runtime dependant
                if (condition.datatype == "string")
                {
                    switch (Cond_op.token.lex)
                    {
                        case "<>":
                            condition.value = (bool)(LHS.token.lex != RHS.token.lex);
                            //if boolvalue=true condition.value=1 else 0 
                            break;

                        case "=":
                            condition.value = (bool)(LHS.token.lex == RHS.token.lex);
                            break;

                        default:
                            string error = "operator {0} can't be used with string condtion";
                            Errors.Analyser_Error_List.Add(string.Format(error, Cond_op.token.lex));
                            break;
                    }


                }
                else if (condition.datatype == "int")//real / string 
                {
                    //switch (Cond_op.token.lex)

                    switch (Cond_op.children[0].token.lex)//H
                    {
                        case "<>":
                            condition.value = (bool)(LHS.value != RHS.value);
                            break;

                        case "=":
                            condition.value = (bool)(LHS.value == RHS.value);
                            break;

                        case ">":
                            condition.value = (bool)((int)LHS.value > (int)RHS.value);
                            break;
                        case "<":
                            condition.value = (bool)((int)LHS.value < (int)RHS.value);
                            break;
                        default:
                            string error = "wrong operator";
                            Errors.Analyser_Error_List.Add(error);
                            break;
                    }

                }
                else if (condition.datatype == "float")//real / string 
                {
                    //switch (Cond_op.token.lex)
                    switch (Cond_op.children[0].token.lex)//H
                    {
                        case "<>":
                            condition.value = (bool)(LHS.value != RHS.value);
                            break;

                        case "=":
                            condition.value = (bool)(LHS.value == RHS.value);
                            break;

                        case ">":
                            condition.value = (bool)((float)LHS.value > (float)RHS.value);
                            break;
                        case "<":
                            condition.value = (bool)((float)LHS.value < (float)RHS.value);
                            break;
                        default:
                            string error = "wrong operator";
                            Errors.Analyser_Error_List.Add(error);
                            break;
                    }

                }
            }
            else condition.value = null;
            //if one of the variables is a runtime input
        }
        public static void handleCondition_Statement(Node condition_statment)
        {//Condition_Statement → Condition Boolean_Exp
         // x==5    (|| y==7 && z>5)
            setscope(condition_statment);
            //Node condition = condition_statment.children[0];
            Node boolean_exp = condition_statment.children[1];
            handleCondition(condition_statment.children[0]);
            if (boolean_exp != null)
            {// handleBool_Exp(boolean_exp);
                handleBool_Exp(condition_statment.children[1]);
                evaluateCondition_Statement(condition_statment);
            }
            else
            {// condition_statment.value = condition.value;
                condition_statment.value = condition_statment.children[0].value;
            }
            //check if it actually works 
        }
        public static void handleBool_Exp(Node boolean_exp)
        {
            setscope(boolean_exp);
            // x==5       ||        y==7      &&    z>5
            // condition  0operator 1condition  2.0oprator 2.1condition  ε
            //boolean_opertor || condition y==7 boolean expression &&z>5
            if (boolean_exp.children[0] != null)
            {//Boolean_Exp → Boolean_Operator Condition Boolean_Exp |ε
                handleCondition(boolean_exp.children[1]);
                //bool value;//H
                if (boolean_exp.children[1].value != null)//H   // Can't Evaluate the value during compile time 
                {
                    bool condition = (bool)boolean_exp.children[1].value;
                    if (boolean_exp.children[2] != null)//H
                    {
                        handleBool_Exp(boolean_exp.children[2]);
                        setscope(boolean_exp.children[2]);
                        string child_boolean_operator = boolean_exp.children[2].children[0].children[0].token.lex;
                        bool child_condition = (bool)boolean_exp.children[2].children[1].value;
                        switch (child_boolean_operator)
                        {
                            case "||":
                                //value = condition || child_condition; //H
                                //boolean_exp.value = value == true ? 1 : 0;//H
                                boolean_exp.value = condition || child_condition;//H
                                break;
                            case "&&":
                                //value = condition && child_condition;//H
                                //boolean_exp.value = value == true ? 1 : 0;//H
                                boolean_exp.value = condition && child_condition;//H
                                break;
                            default:
                                string error = "wrong operator :{0} should be a boolean operator";
                                Errors.Analyser_Error_List.Add(string.Format(error, child_boolean_operator));
                                break;
                        }
                    }
                    else//H //eventually boolean_exp will be null
                    {
                        boolean_exp.value = condition;//H
                    }
                }
                else//H
                {
                    boolean_exp.value = null;
                }
            }
            //else Boolean_Exp →ε

        }
        public static void evaluateCondition_Statement(Node condition_statment)
        {   // x==5    (|| y==7 && z>5)
            //Boolean_Exp → Boolean_Operator Condition Boolean_Exp | ε
            //boolean_opertor || condition y==7 boolean expression &&z>5
            setscope(condition_statment);
            Node boolean_exp = condition_statment.children[1];
            string boolean_operator = boolean_exp.children[0].children[0].token.lex;

            //H //can be null so the condition_statment value is null
            if (boolean_exp.value != null && condition_statment.children[0].value != null)
            {
                bool bool_expval = (bool)boolean_exp.value;
                bool conditionval = (bool)condition_statment.children[0].value;
                bool value;//=true;
                switch (boolean_operator)
                {
                    case "||":
                        value = conditionval || (bool_expval);
                        condition_statment.value = value;
                        break;
                    case "&&":
                        value = conditionval && (bool_expval);
                        condition_statment.value = value;
                        break;
                    default:
                        string error = "wrong operator :{0} should be a boolean operator";
                        Errors.Analyser_Error_List.Add(string.Format(error, boolean_operator));
                        break;

                }
            }
            else//H
            {
                condition_statment.value = null;
            }
        }
        public static void handleIf(Node root)
        {//If_Statement → if Condition_Statement then Statements Else_part
            root.scope = root.scope + "_If";//H
            if (!scopeCount.ContainsKey(root.scope))
            {
                scopeCount[root.scope] = 0;
            }
            scopeCount[root.scope]++;
            root.scope += scopeCount[root.scope];
            setscope(root);//H
            handleCondition_Statement(root.children[1]);
            handle_Statements(root.children[3]);//H
            List<KeyValuePair<string, string>> local_Vars_to_remove = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Value== root.scope)
                {
                    local_Vars_to_remove.Add(variable);
                }
            }
            foreach (KeyValuePair<string, string> variable in local_Vars_to_remove)//H
            {
                SymbolTable.Remove(variable);
            }
            handleElse_Part(root.children[4]);
                //else part
        }
        public static void handleElse_Part(Node else_part)
        {// Else_part → Else_If_Statment | Else_Statment | end
            setscope(else_part);//H
            //if (else_part.children[0].children[0].token.lex=="elseif")
            if (else_part.children[0].children[0].token.lex == "elseif")//H
            {
                //else_part.scope = else_part.scope.Substring(0,else_part.scope.Length-3) + "_ElseIF";//H
                handleElse_if_statment(else_part.children[0]);
            }
            else if (else_part.children[0].children[0].token.lex == "else")//H
            {
                //else_part.scope = else_part.scope + "_Else";//H
                handleElse_statment(else_part.children[0]);
            }
        }
        public static void handleElse_statment(Node elsestatment)//H
        {// Else_Statment → else Statements end
            elsestatment.scope = elsestatment.scope.Substring(0,elsestatment.scope.Length-3) + "_els";//H
            if (!scopeCount.ContainsKey(elsestatment.scope))
            {
                scopeCount[elsestatment.scope] = 0;
            }
            scopeCount[elsestatment.scope]++;
            elsestatment.scope += scopeCount[elsestatment.scope];
            setscope(elsestatment);//H
            handle_Statements(elsestatment.children[1]);
            List<KeyValuePair<string, string>> local_Vars_to_remove = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Value==elsestatment.scope)
                {
                    local_Vars_to_remove.Add(variable);
                }
            }
            foreach (KeyValuePair<string, string> variable in local_Vars_to_remove)//H
            {
                SymbolTable.Remove(variable);
            }
        }

        public static void handleElse_if_statment(Node elseifstatment)
        {// Else_If_Statement → elseif Condition_Statement then Statements Else_part
            elseifstatment.scope = elseifstatment.scope.Substring(0, elseifstatment.scope.Length - 3) + "_elif";//H
            if (!scopeCount.ContainsKey(elseifstatment.scope))
            {
                scopeCount[elseifstatment.scope] = 0;
            }
            scopeCount[elseifstatment.scope]++;
            elseifstatment.scope += scopeCount[elseifstatment.scope];
            setscope(elseifstatment);
            handleCondition_Statement(elseifstatment.children[1]);
            handle_Statements(elseifstatment.children[3]);
            List<KeyValuePair<string, string>> local_Vars_to_remove = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Value== elseifstatment.scope)
                {
                    local_Vars_to_remove.Add(variable);
                }
            }
            foreach (KeyValuePair<string, string> variable in local_Vars_to_remove)//H
            {
                SymbolTable.Remove(variable);
            }
            handleElse_Part(elseifstatment.children[4]);
        }
        public static void handleRepeat_Statement(Node root)
        {//Repeat_Statement→ repeat Statements until Condition_Statement
            root.scope += "_R";//H
            if (!scopeCount.ContainsKey(root.scope))
            {
                scopeCount[root.scope] = 0;
            }
            scopeCount[root.scope]++;
            root.scope += scopeCount[root.scope];
            setscope(root);//H
            handle_Statements(root.children[1]);//H
            List<KeyValuePair<string, string>> local_Vars_to_remove = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> variable in SymbolTable.Keys)//H
            {
                if (variable.Value == root.scope)
                {
                    local_Vars_to_remove.Add(variable);
                }
            }
            foreach (KeyValuePair<string, string> variable in local_Vars_to_remove)//H
            {
                SymbolTable.Remove(variable);
            }

            handleCondition_Statement(root.children[3]);
        }

        #endregion Mai-p3
        #region p4
        public static void handle_function_declaration(Node root)
        {
            root.children[0].datatype = root.children[0].children[0].token.lex;
            root.children[1].datatype = root.children[0].children[0].token.lex;


            functionTableValue value = new functionTableValue();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            string funName = root.children[1].token.lex;
            root.scope = funName;
            setscope(root);
            value.datatype = root.children[1].datatype;

            if (root.children[3].children[0] != null)
            {
                param = handle_parameters(root.children[3]);
                value.parameters = param;
                value.numOFparam = param.Count();
                foreach (KeyValuePair<string,string> p in value.parameters)//H
                {   
                    List<KeyValuePair<string,object>> temp = new List<KeyValuePair<string,object>>();
                    temp.Add(new KeyValuePair<string, object>("Datatype", p.Value));
                    SymbolTable.Add(new KeyValuePair<string,string>(p.Key,funName),temp);
                }
            }
            else
            {
                value.numOFparam = 0;
            }
            if (!FunctionTable.ContainsKey(funName))
                FunctionTable.Add(funName, value);
            else
                Errors.Analyser_Error_List.Add(funName + " Already declared function");
        }
        public static List<KeyValuePair<string, string>> handle_parameters(Node root)
        {
            root.children[0].datatype = root.children[0].children[0].token.lex;
            root.children[1].datatype = root.children[0].children[0].token.lex;
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>(root.children[1].token.lex, root.children[1].datatype));
            if (root.children[2] != null)
            {
                param.Add(handle_parametersdash(root.children[2]));

            }

            return param;
        }
        public static KeyValuePair<string, string> handle_parametersdash(Node root)
        {
            root.children[1].datatype = root.children[1].children[0].token.lex;
            root.children[2].datatype = root.children[1].children[0].token.lex;
            if (root.children[3] != null)
            {
                handle_parametersdash(root.children[3]);
            }
            KeyValuePair<string, string> param;
            param = new KeyValuePair<string, string>(root.children[2].token.lex, root.children[2].datatype);
            return param;
        }
        public static string handle_function_call(Node root)
        {
            matchParameters matchparam;
            matchparam.dataTypeOFArguments = true;
            matchparam.numOFArguments =true;
            List<Value_Type> dataTypeOFarguments = new List<Value_Type>();
            List<KeyValuePair<string, string>> dataTypeOFparams = new List<KeyValuePair<string,string>>();
            functionTableValue funTableValue = new functionTableValue();
            
            string funName = root.children[0].token.lex;

            //if1 the fun exist in function table
            if (FunctionTable.ContainsKey(funName))
            {
               funTableValue = FunctionTable[funName];
               dataTypeOFparams = funTableValue.parameters;

                //if2 the function contains arguments
               if ((root.children[2] != null))
               {
                   dataTypeOFarguments = handle_argument_list(root.children[2]);

                   //if3 the num of arguments equal num of parameters
                   if (funTableValue.numOFparam == dataTypeOFarguments.Count)
                   {
                       for (int i = 0; i < funTableValue.numOFparam; i++)
                       {
                           //if4 the data type of arguments equal data type of parameters
                           if (dataTypeOFarguments[i].datatype != null)//H
                           {
                               if (dataTypeOFparams[i].Value.ToString() != dataTypeOFarguments[i].datatype.ToString())
                               {
                                   matchparam.dataTypeOFArguments = false;
                                   break;
                               }
                           }
                           else//H 
                           {
                               Errors.Analyser_Error_List.Add("use of undeclared variable as function argument when calling (" + funName + ")");
                           }
                       }
                   }
                   //else3 the num of arguments not equal num of parameters
                   else

                       matchparam.numOFArguments = false;
               }
               //else2 num of arguments  is zero and check if num of parameters equal zero
               else
               {
                   if (funTableValue.numOFparam != 0)
                       matchparam.numOFArguments = false;
               }

                if (matchparam.numOFArguments != true)
                {
                    Errors.Analyser_Error_List.Add("In Call Function (" + funName + ")" + " Number of arguments mismatch");
                }
                if (matchparam.dataTypeOFArguments != true)
                {
                    Errors.Analyser_Error_List.Add("In Call Function (" + funName + ")" + " arguments datatype mismatch");
                }
            }
            //else1 function name not exist in function table
            else
                Errors.Analyser_Error_List.Add(funName + " Is Undeclared function");

            return funTableValue.datatype;
        }
        public static List<Value_Type> handle_argument_list(Node root)
        {
            List<Value_Type> dataTypeParams = new List<Value_Type>();
            Value_Type paramType = new Value_Type();
            paramType = handle_Expression(root.children[0]);
            dataTypeParams.Add(paramType);
            if (root.children[1] != null)
            {
                paramType = handle_arguments(root.children[1]);
                dataTypeParams.Add(paramType);
            }

            return dataTypeParams;
        }
        public static Value_Type handle_arguments(Node root)
        {
            if (root.children[2] != null)
            {
                handle_arguments(root.children[2]);
            }
            Value_Type paramType = new Value_Type();
            paramType = handle_Expression(root.children[1]);
            return paramType;
        }
        # endregion


        public static void traverseTree(Node root)
        {
            if (root.token.lex.ToLower() == "function_list")
            {
                if (root.children[0] != null)
                {
                    handle_Function_Statement(root.children[0]);
                }
                
            }
            else if (root.token.lex.ToLower() == "main_function")
            {
                handle_Main_Function(root);
            }
            /*else
            {
                foreach (Node child in root.children)
                {
                    if (child != null) 
                    {
                        traverseTree(child);
                    }
                }
            }*/

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
            if (root.value == null && root.datatype == "")
                tree = new TreeNode(root.token.lex);
            else if (root.value != null && root.datatype == "")
                tree = new TreeNode(root.token.lex + " & its value is: " + root.value);
            else if (root.value == null && root.datatype != "")
                tree = new TreeNode(root.token.lex + " & its datatype is: " + root.datatype);
            else
                tree = new TreeNode(root.token.lex + " & its value is: " + root.value + " & datatype is: " + root.datatype);
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
