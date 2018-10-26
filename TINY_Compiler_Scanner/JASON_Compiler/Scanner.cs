using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Token_Class
{
    DataTypeInt, DataTypeFloat, DataTypeString, Read, Write, Repeat, Until, If, Elseif, Else, Then, Return, Endl, End, Main,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, LCurlyBraces, RCurlyBraces, OrOp,
    AndOp, AssignmentOp, NotEqual, IsEqual, LessThanOp, GreaterThanOp, PlusOp, MinusOp, MultiplyOp,
    DivideOp, Number, Identifier, DoubleQuotes, String, comment, FloatNumber
}
namespace JASON_Compiler
{

    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        bool end;

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.DataTypeInt);
            ReservedWords.Add("float", Token_Class.DataTypeFloat);
            ReservedWords.Add("string", Token_Class.DataTypeString);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            //ReservedWords.Add("main", Token_Class.Main);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurlyBraces);
            Operators.Add("}", Token_Class.RCurlyBraces);
            Operators.Add("\"", Token_Class.DoubleQuotes);

            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOp);

            Operators.Add(":=", Token_Class.AssignmentOp);

            Operators.Add("<>", Token_Class.NotEqual);
            Operators.Add("=", Token_Class.IsEqual);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);

            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);

        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                //if space delimeter skip
                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                    continue;
                //if starts with letters check if its all letters|| numbers --> identifier 
                if ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z')) //if you read a character
                {
                    j++;
                    if (j == SourceCode.Length)//reached end of file
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }

                    else
                    {
                        CurrentChar = SourceCode[j];
                        while ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') || (CurrentChar >= '0' && CurrentChar <= '9') || CurrentChar == '_')
                        {
                            CurrentLexeme += CurrentChar;
                            j++;
                            if (j == SourceCode.Length)
                            {
                                FindTokenClass(CurrentLexeme);
                                end = true;
                                break;
                            }
                            else
                                CurrentChar = SourceCode[j];
                        }
                        if (end)
                            break;
                        FindTokenClass(CurrentLexeme);
                        i = j - 1;
                    }

                }
                else if (CurrentChar == '"')
                {
                    j++;
                    while (j < SourceCode.Length && SourceCode[j] != '"' && SourceCode[j] != '\n')
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        j++;
                    }
                    if (j == SourceCode.Length)
                    {
                        Errors.Error_List.Add(CurrentLexeme + " End-of-file found, '\"' expected\n");
                        end = true;
                        break;
                    }
                    else if (SourceCode[j] == '\n')
                    {
                        Errors.Error_List.Add(CurrentLexeme + " ; expected (a string was not closed with\") \n");
                    }
                    else
                    {
                        CurrentLexeme += '"';
                        FindTokenClass(CurrentLexeme);
                    }

                    i = j;
                }
                //if its a number (constant)
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    if (j == SourceCode.Length)//only one digit and code ended 
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }

                    else
                    {
                        CurrentChar = SourceCode[j];
                        while (CurrentChar >= '0' && CurrentChar <= '9')//multiple digits
                        {
                            CurrentLexeme += CurrentChar;
                            j++;
                            if (j == SourceCode.Length)
                            {
                                FindTokenClass(CurrentLexeme);
                                end = true;
                                break;
                            }
                            else
                                CurrentChar = SourceCode[j];
                        }
                        if (CurrentChar == '.')//float number
                        {
                            CurrentLexeme += CurrentChar;
                            j++;
                            if (j == SourceCode.Length)//not complete float and end of file
                            {
                                Errors.Error_List.Add(CurrentLexeme + "is not an identifier or constant (not complete float)\n");
                                end = true;
                                break;
                            }
                            else
                                CurrentChar = SourceCode[j];
                            if (!(CurrentChar >= '0' && CurrentChar <= '9'))//(example 5.) not complete float
                            {// to get the lexeme detected by error detection and getting the full error lexeme
                                // by declaring that the dot found not for a float number
                                j--;
                                CurrentChar = SourceCode[j];
                                CurrentLexeme = CurrentLexeme.Remove(CurrentLexeme.Count() - 1);
                            }
                            else
                            {
                                while (CurrentChar >= '0' && CurrentChar <= '9')//one or more digits after dot -->float
                                {
                                    CurrentLexeme += CurrentChar;
                                    j++;
                                    if (j == SourceCode.Length)
                                    {
                                        FindTokenClass(CurrentLexeme);
                                        end = true;
                                        break;
                                    }
                                    else
                                        CurrentChar = SourceCode[j];
                                }
                            }
                        }

                        if (end)
                            break;

                        //ERROR DETECTION IN LEXEMES STARTING WITH NUMBERS
                        string currentop; // not all operators comes after numbers and cuase no errors only the numerical operators 
                        if (j + 1 < SourceCode.Length)
                        {
                            currentop = "" + (char)SourceCode[j] + (char)SourceCode[j + 1];
                        }
                        else currentop = (char)SourceCode[j] + " ";
                        if (!isNumericalOperator(currentop) && !(CurrentChar == ';' || CurrentChar == ',' || CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t'))
                        {

                            while (!isNumericalOperator(currentop) && !(CurrentChar == ';' || CurrentChar == ',' || CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t'))
                            {
                                CurrentLexeme += CurrentChar;
                                j++;
                                if (j == SourceCode.Length)
                                {
                                    //Errors.Error_List.Add(CurrentLexeme + " is not an identifier or constant");
                                    break;
                                }
                                else
                                    CurrentChar = SourceCode[j];
                                //error
                                if (j + 1 < SourceCode.Length)
                                {
                                    currentop = "" + (char)SourceCode[j] + (char)SourceCode[j + 1];
                                }
                                else currentop = (char)SourceCode[j] + " ";
                            }
                            Errors.Error_List.Add(CurrentLexeme + " is not an identifier or constant\n");
                        }
                        else //if not error add to tokens
                            FindTokenClass(CurrentLexeme);
                        i = j - 1;
                    }
                }
                else if (CurrentChar == '/')
                {
                    j++;
                    CurrentChar = SourceCode[j];
                    if (CurrentChar == '*')
                    {
                        CurrentLexeme += CurrentChar;
                        j++;
                        while (j + 1 < SourceCode.Length && !(SourceCode[j] == '*' && SourceCode[j + 1] == '/'))
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                        }
                        if (j + 1 >= SourceCode.Length)
                        {
                            int len = 30;
                            if (len >= CurrentLexeme.Length)
                                len = CurrentLexeme.Length;
                            Errors.Error_List.Add(CurrentLexeme.Substring(0,len) + " End-of-file found, '*/' expected\n");
                            end = true;
                            break;
                        }
                        else
                        {
                            CurrentLexeme += "*/";
                        }
                        j++;
                        FindTokenClass(CurrentLexeme);
                    }
                    else// divid operator not a comment
                    {
                        FindTokenClass("/");
                        j--;
                    }
                    i = j;
                }
                else if (CurrentChar == '<')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '>')
                    {
                        FindTokenClass("<>");
                        j++;
                    }
                    else
                    {
                        FindTokenClass(CurrentChar.ToString());
                    }
                    i = j;
                }
                else if (CurrentChar == '|')
                {
                    if (SourceCode[j + 1] == '|')
                    {
                        FindTokenClass("||");
                        j++;
                    }
                    i = j;
                }
                else if (CurrentChar == '&')
                {
                    if (SourceCode[j + 1] == '&')
                    {
                        FindTokenClass("&&");
                        j++;
                    }
                    i = j;
                }
                else if (CurrentChar == ':')
                {
                    if (SourceCode[j + 1] == '=')
                    {
                        FindTokenClass(":=");
                        j++;
                    }
                    i = j;
                }
                else if (Operators.ContainsKey(CurrentChar.ToString()))
                {
                    FindTokenClass(CurrentChar.ToString());

                }
                else//all operators
                {
                    
                    while (!Operators.ContainsKey(CurrentChar.ToString()) && !(CurrentChar == ';' || CurrentChar == ',' || CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n'))
                    {
                        if (j == SourceCode.Length)
                        {
                            //Errors.Error_List.Add(CurrentLexeme + " is not an identifier or constant");
                            break;
                        }
                        j++;
                        if (j != SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                        }
                        if (!Operators.ContainsKey(CurrentChar.ToString()) && !(CurrentChar == ';' || CurrentChar == ',' || CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n'))
                        {
                            CurrentLexeme += CurrentChar;
                        }
                    }
                    Errors.Error_List.Add(CurrentLexeme + " Unrecognized token \n");
                    i = j-1;
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;

            //Is it a string?
            if (Lex.Contains('"'))
            {
                Tok.token_type = Token_Class.String;
            }
            //Is it a comment?
            if (Lex.Length>1 && Lex[0]=='/'&&Lex[1]=='*')
            {
                Tok.token_type = Token_Class.comment;

            }
            //Is it a reserved word?
            
            //Is it an identifier?
            else if ((Lex[0] >= 'A' && Lex[0] <= 'Z') || (Lex[0] >= 'a' && Lex[0] <= 'z'))
            {
                if (ReservedWords.ContainsKey(Lex))
                {
                    Tok.token_type = ReservedWords[Lex];
                }
                else
                {
                    Tok.token_type = Token_Class.Identifier;

                }
            }

            //Is it a Constant?
            else if (Lex[0] >= '0' && Lex[0] <= '9')
            {
                if (Lex.Contains('.'))
                    Tok.token_type = Token_Class.FloatNumber;
                else
                    Tok.token_type = Token_Class.Number;
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
            }
            Tokens.Add(Tok);
        }

        bool isNumericalOperator(string lex)
        {
            if ((lex == "||") || (lex == "&&") || (lex == ":=") || (lex == "<>") || (lex[0] == ')') || (lex[0] == '=') || (lex[0] == '<') || (lex[0] == '>') || (lex[0] == '+') || (lex[0] == '-') || (lex[0] == '/') || (lex[0] == '*'))
                return true;
            return false;
        }


        bool isIdentifier(string lex)
        {
            bool isValid = true;


            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;

            return isValid;
        }
    }
}
