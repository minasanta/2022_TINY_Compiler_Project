using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Integer, Float, String, Read, Write, Repeat, Until, If, Elseif, Else, Then, Return, End, Endl, Main,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, LCurly, RCurly, EqualConditionOp, AssignOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, OrOp, AndOp, Idenifier, Number, varString
}
namespace Tiny_Compiler_Project
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
        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.Integer);
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
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("main", Token_Class.Main);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurly);
            Operators.Add("}", Token_Class.RCurly);
            Operators.Add("=", Token_Class.EqualConditionOp);
            Operators.Add(":=", Token_Class.AssignOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOp);


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

                if (SourceCode[j] >= 'A' && SourceCode[j] <= 'Z' || SourceCode[j] >= 'a' && SourceCode[j] <= 'z') //if you read a character
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        if (char.IsLetterOrDigit(SourceCode[j]))
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                        }
                        else
                        {
                            break;
                        }
                        j++;
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                else if (CurrentChar == '\"')
                {
                    j++;
                    while (j < SourceCode.Length && SourceCode[j] != '\"')
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        j++;
                    }
                    if (j < SourceCode.Length && SourceCode[j] == '\"')
                        CurrentLexeme += SourceCode[j].ToString();
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }

                else if ((CurrentChar >= '0' && CurrentChar <= '9') || CurrentChar == '.')
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        if (char.IsLetterOrDigit(SourceCode[j]) || SourceCode[j] == '.')
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                        }
                        else
                        {
                            break;
                        }
                        j++;
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else if (CurrentChar == '/' && j + 1 < SourceCode.Length && SourceCode[i + 1] == '*')
                {
                    CurrentLexeme += "*";
                    j += 2;
                    while (j < SourceCode.Length)
                    {

                        if (SourceCode[j] == '*' && j + 1 < SourceCode.Length && SourceCode[j + 1] == '/')
                        {
                            CurrentLexeme += "*/";
                            j++;
                            break;
                        }
                        else
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                            j++;
                        }

                    }
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }

                else if (CurrentChar == '>' || CurrentChar == '<')
                {

                    j++;
                    if (j < SourceCode.Length && SourceCode[j] == '=')
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        FindTokenClass(CurrentLexeme);
                    }
                    else if (j < SourceCode.Length && CurrentChar == '<' && SourceCode[j] == '>')
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        FindTokenClass(CurrentLexeme);
                    }
                    else if (j < SourceCode.Length && CurrentChar == '>' && SourceCode[j] == '<')
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        FindTokenClass(CurrentLexeme);
                    }
                    else
                    {
                        FindTokenClass(CurrentLexeme);
                    }
                    i = j - 1;

                }

                else if (CurrentChar == ':')
                {
                    j++;
                    if (SourceCode[j] == '=')
                        CurrentLexeme += SourceCode[j].ToString();
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }
                else if (CurrentChar == '&')
                {
                    j++;
                    if (SourceCode[j] == '&')
                        CurrentLexeme += SourceCode[j].ToString();
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }
                else if (CurrentChar == '|')
                {
                    j++;
                    if (SourceCode[j] == '|')
                        CurrentLexeme += SourceCode[j].ToString();
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }
                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }

            Tiny_Compiler_Project.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            //Is it a string
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.varString;
                Tokens.Add(Tok);
            }
            //Is it a Constant?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            //Is it an comment?
            else if (isComment(Lex))
            {
                return;
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }



        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.
            var identiferRE = new Regex("^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
            if (identiferRE.IsMatch(lex))
                return isValid;
            return false;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var constRE = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            if (constRE.IsMatch(lex))
                return isValid;
            return false;
        }
        bool isString(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var constRE = new Regex("^\"[^\"]*\"$", RegexOptions.Compiled);
            if (constRE.IsMatch(lex))
                return isValid;
            return false;
        }
        bool isComment(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var constRE = new Regex(@"^/\*([^*]|(\*+[^/]))*\*/$", RegexOptions.Compiled);
            if (constRE.IsMatch(lex))
                return isValid;
            return false;
        }
    }
}
