﻿using System;
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
            Node root = new Node();
            root.token = new Token();
            root.token.lex = "Parse";
            root.children.Add(Program(Tokens));
            PrintParseTree(root);
            //write your parser code
            //root.children.Add(Match(Token_Class.Program, Tokens));
            //root.children.Add(Header(Tokens));
            //root.children.Add(DecSec(Tokens));
            //root.children.Add(Block(Tokens));
            return root;
        }

        public static Node Match(Token_Class expected_tok, List<Token> Tokens)
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

        //Datatype → int |string |float
        public static Node DataType(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "DataType";
            if (Tokens[index].token_type == Token_Class.DataTypeInt)
            {
                returned_node.children.Add(Match(Token_Class.DataTypeInt, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.DataTypeFloat)
            {
                returned_node.children.Add(Match(Token_Class.DataTypeFloat, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.DataTypeString)
            {
                returned_node.children.Add(Match(Token_Class.DataTypeString, Tokens));
            }
            return returned_node;
        }

        //Declaration_Statement → Datatype Declared_Var_list;
        public static Node DeclarationStatement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Declaration_Statement";
            returned_node.children.Add(DataType(Tokens));
            returned_node.children.Add(Declared_Var_list(Tokens));
            returned_node.children.Add(Match(Token_Class.Semicolon, Tokens));

            return returned_node;
        }

        //Declared_Var → identifier Declared_Var_Assignment
        public static Node Declared_Var(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Declared_Var";
            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Declared_Var_Assignment(Tokens));
            return returned_node;
        }

        //Declared_Var_Assignment → Assignment_op Expression | ε
        public static Node Declared_Var_Assignment(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Declared_Var_Assignment";
            if (Tokens[index].token_type == Token_Class.AssignmentOp)
            {
                returned_node.children.Add(Match(Token_Class.AssignmentOp, Tokens));
                returned_node.children.Add(Expression(Tokens));
            }
            else
            {
                return null;
            }
            return returned_node;
        }

        //Declared_Vars → , Declared_Var Declared_Vars | ε
        public static Node Declared_Vars(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Declared_Vars";
            if (Tokens[index].token_type == Token_Class.Comma)
            {
                returned_node.children.Add(Match(Token_Class.Comma, Tokens));
                returned_node.children.Add(Declared_Var(Tokens));
                returned_node.children.Add(Declared_Vars(Tokens));
            }
            else
            {
                return null;
            }
            return returned_node;
        }

        //Declared_Var_list → Declared_Var Declared_Vars
        public static Node Declared_Var_list(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Declared_Var_list";
            returned_node.children.Add(Declared_Var(Tokens));
            returned_node.children.Add(Declared_Vars(Tokens));
            return returned_node;
        }

        //Write_Statement→ WRITE WrittenExp ;
        public static Node WriteStatement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Write_Statement";

            returned_node.children.Add(Match(Token_Class.Write, Tokens));
            returned_node.children.Add(WrittenExp(Tokens));
            returned_node.children.Add(Match(Token_Class.Semicolon, Tokens));

            return returned_node;
        }

        //Writtenexp → Expression|endl
        public static Node WrittenExp(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "WrittenExp";

            if (Tokens[index].token_type == Token_Class.Endl)
            {
                returned_node.children.Add(Match(Token_Class.Endl, Tokens));
            }
            else
            {
                returned_node.children.Add(Expression(Tokens));
            }
            return returned_node;
        }

        //Read_Statement→ read identifier ;
        public static Node ReadStatement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Read_Statement";

            returned_node.children.Add(Match(Token_Class.Read, Tokens));
            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Match(Token_Class.Semicolon, Tokens));

            return returned_node;
        }

        //Return_Statement →return Expression ;
        public static Node Return_Statement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Return_Statement";

            returned_node.children.Add(Match(Token_Class.Return, Tokens));
            returned_node.children.Add(Expression(Tokens));
            returned_node.children.Add(Match(Token_Class.Semicolon, Tokens));

            return returned_node;
        }

        //Condition_Operator → < | >| = | <>
        public static Node Condition_Operator(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Condition_Operator";
            if (Tokens[index].token_type == Token_Class.LessThanOp)
            {
                returned_node.children.Add(Match(Token_Class.LessThanOp, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.GreaterThanOp)
            {
                returned_node.children.Add(Match(Token_Class.GreaterThanOp, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.IsEqual)
            {
                returned_node.children.Add(Match(Token_Class.IsEqual, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.NotEqual)
            {
                returned_node.children.Add(Match(Token_Class.NotEqual, Tokens));
            }
            return returned_node;
        }

        //Condition → Identifier Condition_Operator Term
        public static Node Condition(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Condition";

            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Condition_Operator(Tokens));
            returned_node.children.Add(Term(Tokens));

            return returned_node;
        }

        //Boolean_Operator → “&&” | “||”
        public static Node Boolean_Operator(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Boolean_Operator";

            if (Tokens[index].token_type == Token_Class.OrOp)
            {
                returned_node.children.Add(Match(Token_Class.OrOp, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.AndOp)
            {
                returned_node.children.Add(Match(Token_Class.AndOp, Tokens));
            }
            return returned_node;
        }

        //Term → number | floatnumber |identifier | Function_Call
        public static Node Term(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Term";
            if (Tokens[index].token_type == Token_Class.Number)
            {
                returned_node.children.Add(Match(Token_Class.Number, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.FloatNumber)
            {
                returned_node.children.Add(Match(Token_Class.FloatNumber, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.Identifier)// 2 cases Identifier or Functioncall 
            {
                if (index + 1 < Tokens.Count && Tokens[index + 1].token_type == Token_Class.LParanthesis)//function Call
                {
                    returned_node.children.Add(Function_Call(Tokens));
                }
                else
                {
                    returned_node.children.Add(Match(Token_Class.Identifier, Tokens));//identifier 
                }
            }

            return returned_node;
        }

        //Expression → string | Term | Equation
        public static Node Expression(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Expression";
            if (Tokens[index].token_type == Token_Class.String)
            {
                returned_node.children.Add(Match(Token_Class.String, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.Number || Tokens[index].token_type == Token_Class.FloatNumber || Tokens[index].token_type == Token_Class.Identifier)
            {
                if (index + 1 < Tokens.Count && (Tokens[index + 1].token_type == Token_Class.PlusOp || Tokens[index + 1].token_type == Token_Class.MinusOp || Tokens[index + 1].token_type == Token_Class.MultiplyOp || Tokens[index + 1].token_type == Token_Class.DivideOp))
                    returned_node.children.Add(Equation(Tokens));//equation
                else
                    returned_node.children.Add(Term(Tokens));//term
            }
            else if (Tokens[index].token_type == Token_Class.LParanthesis)//equation
            {
                returned_node.children.Add(Equation(Tokens));
            }

            return returned_node;
        }

        //Equation → MathTerm Equation’
        public static Node Equation(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Equation";
            returned_node.children.Add(MathTerm(Tokens));
            returned_node.children.Add(Equation_(Tokens));
            return returned_node;
        }

        //Equation ‘→ Add_op MathTerm Equation ‘| ε
        public static Node Equation_(List<Token> Tokens)
        {// Add_op Term Exp’ | ε
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Equation'";
            if (Tokens[index].token_type == Token_Class.MinusOp || Tokens[index].token_type == Token_Class.PlusOp)
            {// Add_op Term Exp’
                returned_node.children.Add(Add_op(Tokens));
                returned_node.children.Add(MathTerm(Tokens));
                returned_node.children.Add(Equation_(Tokens));
            }
            else//| ε
                return null;
            return returned_node;
        }

        //Add_op → +|-
        public static Node Add_op(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Addop";
            if (Tokens[index].token_type == Token_Class.MinusOp)
            {
                returned_node.children.Add(Match(Token_Class.MinusOp, Tokens));
            }
            else if (Tokens[index].token_type == Token_Class.PlusOp)
            {
                returned_node.children.Add(Match(Token_Class.PlusOp, Tokens));
            }
            else
            {
                returned_node.token.lex = "ERROR";
                return returned_node;
            }
            return returned_node;
        }

        //Mul-op → *|/
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

        //MathTerm → Factor MathTerm’
        public static Node MathTerm(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "MathTerm";
            returned_node.children.Add(Factor(Tokens));
            returned_node.children.Add(MathTerm_(Tokens));
            return returned_node;
        }

        //MathTerm‘ → Mul-op Factor MathTerm‘| ε
        public static Node MathTerm_(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "MathTerm'";
            if (Tokens[index].token_type == Token_Class.DivideOp || Tokens[index].token_type == Token_Class.MultiplyOp)
            {// Mul-op Factor Term’
                returned_node.children.Add(Mul_op(Tokens));
                returned_node.children.Add(Factor(Tokens));
                returned_node.children.Add(MathTerm_(Tokens));
            }
            else// ε
                return null;
            return returned_node;
        }

        //Factor → (Equation) | Term
        public static Node Factor(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Factor";
            if (Tokens[index].token_type == Token_Class.LParanthesis)
            {
                returned_node.children.Add(Match(Token_Class.LParanthesis, Tokens));
                returned_node.children.Add(Equation(Tokens));
                returned_node.children.Add(Match(Token_Class.RParanthesis, Tokens));
            }
            else
            {
                returned_node.children.Add(Term(Tokens));
            }
            return returned_node;
        }

        //Assignment_statement → identifier Assignment_operator Expression
        public static Node Assignment_stmnt(List<Token> Tokens)
        {

            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Assignment statement";
            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Assignment_op(Tokens));
            returned_node.children.Add(Expression(Tokens));

            return returned_node;
        }

        //Assignment_op → :=
        public static Node Assignment_op(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Assignment operator";
            /*the index is the index of the token list not the code input*/
            //index++;// cause match will only match to one of them 
            returned_node.children.Add(Match(Token_Class.AssignmentOp, Tokens));
            return returned_node;
        }


        //-----------------Yomna
        public static Node Prameters(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Prameters";
            returned_node.children.Add(DataType(Tokens));
            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Parametersdash(Tokens));
            return returned_node;
        }

        public static Node Parametersdash(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Parametersdash";
            if (Tokens[index].token_type == Token_Class.Comma)
            {
                returned_node.children.Add(Match(Token_Class.Comma, Tokens));
                returned_node.children.Add(DataType(Tokens));
                returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
                returned_node.children.Add(Parametersdash(Tokens));
            }
            return returned_node;
        }

        public static Node Function_body(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Function_body";
            returned_node.children.Add(Match(Token_Class.LCurlyBraces, Tokens));
            returned_node.children.Add(Statements(Tokens));
            returned_node.children.Add(Return_Statement(Tokens));
            returned_node.children.Add(Match(Token_Class.RCurlyBraces, Tokens));
            return returned_node;
        }

        public static Node Function_statement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Function_statement";
            returned_node.children.Add(Function_Declaration(Tokens));
            returned_node.children.Add(Function_body(Tokens));
            return returned_node;
        }

        public static Node Main_Function(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Main_Function";
            returned_node.children.Add(DataType(Tokens));
            returned_node.children.Add(Match(Token_Class.Main, Tokens));
            returned_node.children.Add(Match(Token_Class.LParanthesis, Tokens));
            returned_node.children.Add(Match(Token_Class.RParanthesis, Tokens));
            returned_node.children.Add(Function_body(Tokens));
            return returned_node;
        }

        public static Node Program(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Program";
            returned_node.children.Add(Function_list(Tokens));
            returned_node.children.Add(Main_Function(Tokens));
            return returned_node;
        }

        public static Node Function_list(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Function_list";
            if (Tokens[index].token_type == Token_Class.DataTypeInt || Tokens[index].token_type == Token_Class.DataTypeFloat || Tokens[index].token_type == Token_Class.DataTypeString && Tokens[index + 1].token_type != Token_Class.Main)
            {
                returned_node.children.Add(Function_statement(Tokens));
                returned_node.children.Add(Function_list(Tokens));
            }

            return returned_node;
        }
        //yasmen---------------------------------------------------------------------------------------------------------------------
       
        //Condition_Statement → Condition Boolean_Exp
        public static Node Condition_Statement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Condition_Statement";
            returned_node.children.Add(Condition(Tokens));
            returned_node.children.Add(Boolean_Exp(Tokens));
            return returned_node;
        }

        //Boolean_Exp → Boolean_Operator Condition Boolean_Exp | ε
        public static Node Boolean_Exp(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Boolean_Exp";
            if (Tokens[index].token_type == Token_Class.AndOp || Tokens[index].token_type == Token_Class.OrOp)
            {
                returned_node.children.Add(Boolean_Operator(Tokens));
                returned_node.children.Add(Condition(Tokens));
                returned_node.children.Add(Boolean_Exp(Tokens));
                return returned_node;
            }

            return null;

        }

        //If_Statement → if Condition_Statement then Statements Else_part
        public static Node If_Statement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "If_Statement";
            returned_node.children.Add(Match(Token_Class.If, Tokens));
            returned_node.children.Add(Condition_Statement(Tokens));
            returned_node.children.Add(Match(Token_Class.Then, Tokens));
            returned_node.children.Add(Statements(Tokens));
            returned_node.children.Add(Else_part(Tokens));
            return returned_node;
        }

        //Else_part → Else_If_Statment | Else_Statment | end
        public static Node Else_part(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Else_part";

            if (Tokens[index].token_type == Token_Class.Elseif)
                returned_node.children.Add(Else_If_Statement(Tokens));
            else if (Tokens[index].token_type == Token_Class.Else)
                returned_node.children.Add(Else_Statment(Tokens));
            else
                returned_node.children.Add(Match(Token_Class.End, Tokens));

            return returned_node;
        }
       
        //Else_If_Statement → elseif Condition_Statement then Statements Else_part
        public static Node Else_If_Statement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Else_If_Statement";
            returned_node.children.Add(Match(Token_Class.Elseif, Tokens));
            returned_node.children.Add(Condition_Statement(Tokens));
            returned_node.children.Add(Match(Token_Class.Then, Tokens));
            returned_node.children.Add(Statements(Tokens));
            returned_node.children.Add(Else_part(Tokens));
            return returned_node;
        }
        
        //Else_Statment → else Statements end
        public static Node Else_Statment(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Else_Statment";
            returned_node.children.Add(Match(Token_Class.Else, Tokens));
            returned_node.children.Add(Statements(Tokens));
            returned_node.children.Add(Match(Token_Class.End, Tokens));
            return returned_node;
        }

        //Repeat_Statement→ repeat Statements until Condition_Statement
        public static Node Repeat_Statement(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Repeat_Statement";
            returned_node.children.Add(Match(Token_Class.Repeat, Tokens));
            returned_node.children.Add(Statements(Tokens));
            returned_node.children.Add(Match(Token_Class.Until, Tokens));
            returned_node.children.Add(Condition_Statement(Tokens));
            return returned_node;
        }
      
        //Parameter→ Datatype identifier
        public static Node Parameter(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Parameter";
            returned_node.children.Add(DataType(Tokens));
            returned_node.children.Add(Match(Token_Class.Repeat, Tokens));
            return returned_node;
        }
        
        //Function_Declaration→ Datatype identifier (Parameters )
        public static Node Function_Declaration(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Function_Declaration";
            returned_node.children.Add(DataType(Tokens));
            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Match(Token_Class.LParanthesis, Tokens));
            returned_node.children.Add(Prameters(Tokens));
            returned_node.children.Add(Match(Token_Class.RParanthesis, Tokens));
            return returned_node;
        }
        
        //Function_Call → identifier (Argument_List)
        public static Node Function_Call(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Function_Call";
            returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
            returned_node.children.Add(Match(Token_Class.LParanthesis, Tokens));
            returned_node.children.Add(Argument_List(Tokens));
            returned_node.children.Add(Match(Token_Class.RParanthesis, Tokens));
            return returned_node;
        }
        
        //Arguments → ,identifier Arguments| ε
        public static Node Arguments(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Arguments";
            if (Tokens[index].token_type == Token_Class.Comma)
            {
                returned_node.children.Add(Match(Token_Class.Comma, Tokens));
                returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
                returned_node.children.Add(Arguments(Tokens));
                return returned_node;
            }           
            return null;
        }
        
        //Argument_List → identifier Arguments | ε
        public static Node Argument_List(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Argument_List";
            if (Tokens[index].token_type == Token_Class.Identifier)
            {
                returned_node.children.Add(Match(Token_Class.Identifier, Tokens));
                returned_node.children.Add(Arguments(Tokens));
                return returned_node;
            }
            return null;
        }
        
        //Statements → Assignment_statement | Declaration_Statement| Write_Statement | Read_Statement | If_Statement | Repeat_Statement | Function_Call
        public static Node Statements(List<Token> Tokens)
        {
            Node returned_node = new Node();
            returned_node.token = new Token();
            returned_node.token.lex = "Statements";
            if (Tokens[index].token_type == Token_Class.Identifier)
                returned_node.children.Add(Assignment_stmnt(Tokens));
            else if (Tokens[index].token_type == Token_Class.DataTypeInt || Tokens[index].token_type == Token_Class.DataTypeString ||
                     Tokens[index].token_type == Token_Class.DataTypeFloat)
                returned_node.children.Add(DeclarationStatement(Tokens));
           else if (Tokens[index].token_type == Token_Class.Write)
                returned_node.children.Add(WriteStatement(Tokens));
            else if (Tokens[index].token_type == Token_Class.Read)
                returned_node.children.Add(ReadStatement(Tokens));
            else if (Tokens[index].token_type == Token_Class.If)
                returned_node.children.Add(If_Statement(Tokens));
            else if (Tokens[index].token_type == Token_Class.Repeat)
                returned_node.children.Add(Repeat_Statement(Tokens));
            else if (Tokens[index].token_type == Token_Class.Identifier)
                returned_node.children.Add(Function_Call(Tokens));
            return returned_node;
        }
        
        //use this function to print the parse tree in TreeView Toolbox
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
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