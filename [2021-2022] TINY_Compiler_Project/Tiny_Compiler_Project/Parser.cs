using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler_Project
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            //program.Children.Add(Header());
            //program.Children.Add(DeclSec());
            //program.Children.Add(Block());
            //program.Children.Add(match(Token_Class.Dot));
            MessageBox.Show("Success");
            return program;
        }

        Node Header()
        {
            Node header = new Node("Header");
            //header.Children.Add(match(Token_Class.Idenifier));
            //header.Children.Add(match(Token_Class.Semicolon));
            // write your code here to check the header sructure
            return header;
        }
        Node DeclSec()
        {
            Node declsec = new Node("DeclSec");

            // write your code here to check atleast the declare sturcure 
            // without adding procedures
            return declsec;
        }
        Node Block()
        {
            Node block = new Node("block");
            //block.Children.Add(Statements());
            //block.Children.Add(match(Token_Class.End));
            // write your code here to match statements
            return block;
        }

        // Implement your logic here
        Node Statements()
        {
            Node statements = new Node("Statements");
            //statements.Children.Add(Statement());
            //statements.Children.Add(State());
            return statements;
        }
        Node Statement()
        {
            Node statement = new Node("statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                //statement.Children.Add(match(Token_Class.Write));
                //statement.Children.Add(match(Token_Class.Idenifier));
                return statement;
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                //.Children.Add(match(Token_Class.Read));
                //statement.Children.Add(match(Token_Class.Idenifier));
                return statement;
            }
            else return null;
        }

        Node Expression()
        {
            Node expression = new Node("Expression");
            //expression.Children.Add(Term());
            //expression.Children.Add(Exp());
            return expression;
        }
        Node State()
        {
            Node state = new Node("State");
            if (TokenStream[InputPointer].token_type == Token_Class.Semicolon)
            {
                //state.Children.Add(match(Token_Class.Semicolon));
                //state.Children.Add(Statement());
                //state.Children.Add(State());
                return state;
            }
            else return null;
        }
        Node Term()
        {
            Node term = new Node("Term");
            //term.Children.Add(Factor());
            //term.Children.Add(Ter());
            return term;
        }
        Node Exp()
        {
            Node exp = new Node("Exp");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
                //exp.Children.Add(AddOp());
                //exp.Children.Add(Term());
                //exp.Children.Add(Exp());
                return exp;
            }
            else return null;
        }
        Node Factor()
        {
            Node factor = new Node("Factor");
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                //factor.Children.Add(match(Token_Class.Idenifier));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                //factor.Children.Add(match(Token_Class.Number));
            }
            return factor;
        }
        Node Ter()
        {
            Node ter = new Node("Ter");
            if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
               // ter.Children.Add(MultOp());
               // ter.Children.Add(Factor());
               // ter.Children.Add(Ter());
                return ter;
            }
            else return null;
        }
        Node MultOp()
        {
            Node multop = new Node("MultOp");
            if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                multop.Children.Add(match(Token_Class.MultiplyOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                multop.Children.Add(match(Token_Class.DivideOp));
            }
            return multop;
        }
        Node AddOp()
        {
            Node addop = new Node("AddOp");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
            {
                addop.Children.Add(match(Token_Class.PlusOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
                addop.Children.Add(match(Token_Class.MinusOp));
            }
            return addop;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

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
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
