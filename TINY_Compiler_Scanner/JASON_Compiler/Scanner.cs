using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Token_Class
{
    Int, Float, String, Read, Write, Repeat, Until, If, Elseif, Else, Then, Return, Endl, End, Main,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, LCurlyBraces, RCurlyBraces, OrOp,
    AndOp, AssignmentOp, NotEqual, IsEqual, LessThanOp, GreaterThanOp, PlusOp, MinusOp, MultiplyOp,
    DivideOp, Constant, Identifier
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
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
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
            ReservedWords.Add("main", Token_Class.Main);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurlyBraces);
            Operators.Add("}", Token_Class.RCurlyBraces);

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

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }

                    else
                    {
                        CurrentChar = SourceCode[j];
                        while ((CurrentChar >= 'A' && CurrentChar <= 'z') || (CurrentChar >= '0' && CurrentChar <= '9') || CurrentChar == '_')
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

                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }
                        
                    else
                    {
                        CurrentChar = SourceCode[j];
                        while (CurrentChar >= '0' && CurrentChar <= '9')
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
                        if (CurrentChar == '.')
                        {
                            CurrentLexeme += CurrentChar;
                            j++;
                            CurrentChar = SourceCode[j];
                            while (CurrentChar >= '0' && CurrentChar <= '9')
                            {
                                CurrentLexeme += CurrentChar;
                                j++;
                                CurrentChar = SourceCode[j];
                            }
                        }
                        else if (!(Operators.ContainsKey(CurrentChar.ToString())) && !(CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n'))
                        {
                            while (!(Operators.ContainsKey(CurrentChar.ToString())) && !(CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n'))
                            {
                                CurrentLexeme += CurrentChar;
                                j++;
                                if(j == SourceCode.Length)
                                {
                                    Errors.Error_List.Add(CurrentLexeme + " is not an identifier or constant");
                                    break;
                                }
                                else
                                    CurrentChar = SourceCode[j];
                                //error
                            }
                        }
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
                        j++;
                        while (SourceCode[j] != '*' && SourceCode[j + 1] != '/' && j < SourceCode.Length)
                        {
                            j++;
                        }
                        j++;
                    }
                    if (j == SourceCode.Length)//end of file no }
                    {

                        //error } expected

                    }
                    i = j;
                }
                else
                {
                    if (CurrentChar == '<')
                    {
                        if (SourceCode[j + 1] == '>')
                        {
                            FindTokenClass("<>");
                            j++;
                        }
                        else
                            FindTokenClass(CurrentChar.ToString());
                    }
                    else if (CurrentChar == '|')
                    {
                        if (SourceCode[j + 1] == '|')
                        {
                            FindTokenClass("||");
                            j++;
                        }

                    }
                    else if (CurrentChar == '&')
                    {
                        if (SourceCode[j + 1] == '&')
                        {
                            FindTokenClass("&&");
                            j++;
                        }
                    }
                    else if (CurrentChar == ':')
                    {
                        if (SourceCode[j + 1] == '=')
                        {
                            FindTokenClass(":=");
                            j++;
                        }
                    }
                    else if (Operators.ContainsKey(CurrentChar.ToString()))
                    {
                        FindTokenClass(CurrentChar.ToString());

                    }
                    i = j;
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            
            //Is it an identifier?
            if (Lex[0] >= 'A' && Lex[0] <= 'z')
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
                    Tok.token_type = Token_Class.Float;
                else
                    Tok.token_type = Token_Class.Constant;
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
            }
            Tokens.Add(Tok);
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
