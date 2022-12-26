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
            if (InputPointer + 1 < TokenStream.Count && Token_Class.Main == TokenStream[InputPointer + 1].token_type)
            {
                program.Children.Add(Main());
            }
            else
            {
                program.Children.Add(before_main());
                program.Children.Add(Main());
            }
            MessageBox.Show("Success");
            return program;
        }

        Node Main() 
        {
            Node main = new Node("Main");
            main.Children.Add(Datatype());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.LParanthesis));
            main.Children.Add(match(Token_Class.RParanthesis));
            main.Children.Add(Function_Body());
            return main;
        }

        Node before_main()
        {
            Node before = new Node("Functions Decleration Section");
            if (InputPointer + 1 < TokenStream.Count && Token_Class.Idenifier == TokenStream[InputPointer + 1].token_type)
            {
                before.Children.Add(Function_Statements());
            }
            return before;
        }

        Node Function_Statements() 
        {
            Node Function = new Node("Functions");
            Function.Children.Add(Function_Statement());
            Function.Children.Add(Fun_Ss());
            return Function;
        }

        
        Node Fun_Ss()
        {
            Node Function = new Node("Function");
            if (InputPointer + 1 < TokenStream.Count && Token_Class.Idenifier == TokenStream[InputPointer + 1].token_type)
            {
                Function.Children.Add(Function_Statement());
                Function.Children.Add(Fun_Ss());
            }
            return Function;
        }
        Node Function_Statement()
        {
            Node Function = new Node("Function Statement");
            Function.Children.Add(Function_Decleration());
            Function.Children.Add(Function_Body());
            return Function;
        }

        Node Function_Decleration()
        {
            Node decleration = new Node("Function Decleration");
            decleration.Children.Add(Datatype());
            decleration.Children.Add(match(Token_Class.Idenifier));
            decleration.Children.Add(match(Token_Class.LParanthesis));
            if (InputPointer < TokenStream.Count && Token_Class.RParanthesis == TokenStream[InputPointer].token_type)
                decleration.Children.Add(match(Token_Class.RParanthesis));
            else
            {
                decleration.Children.Add(Argument());
                decleration.Children.Add(match(Token_Class.RParanthesis));
            }
            return decleration;
        }

        Node Argument() 
        {
            Node arguments = new Node("Arguments");
            arguments.Children.Add(Datatype());
            arguments.Children.Add(match(Token_Class.Idenifier));
            if (InputPointer < TokenStream.Count && Token_Class.Comma == TokenStream[InputPointer].token_type)
                arguments.Children.Add(Args());
            return arguments;
        }
        Node Args()
        {
            Node args = new Node("Argument ");
            if (InputPointer < TokenStream.Count && Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                args.Children.Add(match(Token_Class.Comma));
                args.Children.Add(Datatype());
                args.Children.Add(match(Token_Class.Idenifier));
                args.Children.Add(Args());
            }
            return args;
        }

        Node Function_Body()
        {
            Node func_body = new Node("Function Body");
            func_body.Children.Add(match(Token_Class.LCurly));
            func_body.Children.Add(Statements());
            func_body.Children.Add(Return_Statement());
            func_body.Children.Add(match(Token_Class.RCurly));
            return func_body;
        }
        Node Datatype()
        {
            Node datatype = new Node("Data type");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Integer == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.Integer));
                }
                else if (Token_Class.Float == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.Float));
                }
                else
                {
                    datatype.Children.Add(match(Token_Class.String));
                }
            }
            else
                datatype.Children.Add(match(Token_Class.Integer));
            return datatype;
        }

        // Implement your logic here
        Node Statements()
        {
            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(State());
            return statements;
        }
        Node Statement()
        {
            Node statement = new Node("statement");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Write)
                {
                    statement.Children.Add(Write_Statement());
                    return statement;
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Read)
                {
                    statement.Children.Add(Read_Statement());
                    return statement;
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.If)
                {
                    statement.Children.Add(If_Statement());
                    return statement;
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    statement.Children.Add(Assignment_Statement());
                    statement.Children.Add(match(Token_Class.Semicolon));
                    return statement;
                }
                else if (Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type || Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(Declaration_Statement());
                    return statement;
                }
                //else if (Token_Class.Return == TokenStream[InputPointer].token_type)
                //{
                //    statement.Children.Add(Return_Statement());
                //    return statement;
                //}
                else if (Token_Class.Repeat == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(Repeat_Statement());
                    return statement;
                }
                else return null;

            }
            else return null;
        }
        Node State()
        {
            Node state = new Node("State");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Write || TokenStream[InputPointer].token_type == Token_Class.Read
                   || TokenStream[InputPointer].token_type == Token_Class.If || TokenStream[InputPointer].token_type == Token_Class.Idenifier
                   || Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type 
                   || Token_Class.String == TokenStream[InputPointer].token_type //|| Token_Class.Return == TokenStream[InputPointer].token_type
                   || Token_Class.Repeat == TokenStream[InputPointer].token_type)
                {
                    state.Children.Add(Statement());
                    state.Children.Add(State());
                    return state;
                }
                else return null;

            }
            else return null;
        }

        Node Return_Statement() 
        {
            Node statement = new Node("Return Statement");
            statement.Children.Add(match(Token_Class.Return));
            statement.Children.Add(Expression());
            statement.Children.Add(match(Token_Class.Semicolon));
            return statement;
        }

        Node Write_Statement() 
        {
            Node statement = new Node("Write Statement");
            statement.Children.Add(match(Token_Class.Write));
            if (InputPointer < TokenStream.Count && Token_Class.Endl == TokenStream[InputPointer].token_type)
                statement.Children.Add(match(Token_Class.Endl));
            else statement.Children.Add(Expression());
            statement.Children.Add(match(Token_Class.Semicolon));
            return statement;
        }

        Node Read_Statement()
        {
            Node statement = new Node("Read Statement");
            statement.Children.Add(match(Token_Class.Read));
            statement.Children.Add(match(Token_Class.Idenifier));
            statement.Children.Add(match(Token_Class.Semicolon));
            return statement;
        }

        Node If_Statement()
        {
            Node statement = new Node("If Statement");
            statement.Children.Add(match(Token_Class.If));
            statement.Children.Add(Condition_Statement());
            statement.Children.Add(match(Token_Class.Then));
            statement.Children.Add(Statements());
            if (InputPointer < TokenStream.Count && Token_Class.Elseif == TokenStream[InputPointer].token_type)
                statement.Children.Add(Else_If_Statement());
            else if (InputPointer < TokenStream.Count && Token_Class.Else == TokenStream[InputPointer].token_type)
                statement.Children.Add(Else_Statement());
            else statement.Children.Add(match(Token_Class.End));
            return statement;
        }

        Node Else_If_Statement()
        {
            Node elseif = new Node("Else if Statment");
            elseif.Children.Add(match(Token_Class.Elseif));
            elseif.Children.Add(Condition_Statement());
            elseif.Children.Add(match(Token_Class.Then));
            elseif.Children.Add(Statements());
            if (InputPointer < TokenStream.Count && Token_Class.Elseif == TokenStream[InputPointer].token_type)
                elseif.Children.Add(Else_If_Statement());
            else if (InputPointer < TokenStream.Count && Token_Class.Else == TokenStream[InputPointer].token_type)
                elseif.Children.Add(Else_Statement());
            else elseif.Children.Add(match(Token_Class.End));
            return elseif;
        }

        Node Else_Statement() 
        {
            Node Else = new Node("Else Statment");
            Else.Children.Add(match(Token_Class.Else));
            Else.Children.Add(Statements());
            Else.Children.Add(match(Token_Class.End));
            return Else;
        }

        Node Repeat_Statement()
        {
            Node statement = new Node("Repeat Statement");
            // repeat Statements until  Condition_Statement
            statement.Children.Add(match(Token_Class.Repeat));
            statement.Children.Add(Statements());
            statement.Children.Add(match(Token_Class.Until));
            statement.Children.Add(Condition_Statement());
            return statement;
        }

        Node Assignment_Statement()
        {
            Node statement = new Node("Assignment Statement");
            // identifier := Expression   
            statement.Children.Add(match(Token_Class.Idenifier));
            statement.Children.Add(match(Token_Class.AssignOp));
            statement.Children.Add(Expression());

            return statement;
        }

        Node Declaration_Statement()
        {
            Node statement = new Node("Declaration Statement");
            // Datatype Assignment_Statements;
            statement.Children.Add(Datatype());
            statement.Children.Add(Assignment_Statements());
            statement.Children.Add((match(Token_Class.Semicolon)));
            return statement;
        }

        Node Assignment_Statements()
        {
            Node statement = new Node("Assignment Statements");
            if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.AssignOp)
            {
                statement.Children.Add(Assignment_Statement());
                statement.Children.Add(Assign_Comma("assign"));
            }
            else
            {
                statement.Children.Add(Identifiers());
                statement.Children.Add(Assign_Comma("idens"));
            }
            return statement;
        }

        Node Assign_Comma(string s)
        {
            Node assign;
            if ( s == "assign")
                assign = new Node("More Assignments");
            else
                assign = new Node("More Identifiers");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                assign.Children.Add(match(Token_Class.Comma));
                assign.Children.Add(Assignment_Statements());
                return assign;
            }
            else return null;
        }
        Node Identifiers() 
        {
            Node identifiers = new Node("Identifiers");
            identifiers.Children.Add(match(Token_Class.Idenifier));
            identifiers.Children.Add(Assign_Comma("idens"));
            return identifiers;
        }

        Node Condition_Statement() 
        {
            Node statment = new Node("Condition Statement");
            statment.Children.Add(Condition());
            statment.Children.Add(More_conds());
            return statment;
        }

        Node Condition() 
        {
            Node condtion = new Node("Condition");
            // Identifier Condition_Operator Term
            condtion.Children.Add(match(Token_Class.Idenifier));
            condtion.Children.Add(Condition_Operator());
            condtion.Children.Add(Term());
            return condtion;
        }

        Node More_conds() 
        {
            Node More_conds = new Node("More Conditions");
            // Boolean_op condition_statems | Ɛ 
            if (InputPointer < TokenStream.Count && 
               (TokenStream[InputPointer].token_type == Token_Class.AndOp || TokenStream[InputPointer].token_type == Token_Class.OrOp))
            {
                More_conds.Children.Add(Boolean_op());
                More_conds.Children.Add(Condition_Statement());
                return More_conds;
            }
            else return null;
        }

        Node Condition_Operator()
        {
            Node condition_op = new Node("Condition Operator");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LessThanOp == TokenStream[InputPointer].token_type)
                {
                    condition_op.Children.Add(match(Token_Class.LessThanOp));
                }
                else if (Token_Class.GreaterThanOp == TokenStream[InputPointer].token_type)
                {
                    condition_op.Children.Add(match(Token_Class.GreaterThanOp));
                }
                else if (Token_Class.EqualConditionOp == TokenStream[InputPointer].token_type)
                {
                    condition_op.Children.Add(match(Token_Class.EqualConditionOp));
                }
                else
                    condition_op.Children.Add(match(Token_Class.NotEqualOp));
            }
            else condition_op.Children.Add(match(Token_Class.LessThanOp));
            return condition_op;
        }

        Node Boolean_op()
        {
            Node boolean = new Node("Boolean Operator");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.AndOp)
                    boolean.Children.Add(match(Token_Class.AndOp));
                else boolean.Children.Add(match(Token_Class.OrOp));
            }
            else boolean.Children.Add(match(Token_Class.AndOp));
            return boolean;
        }

        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
            {
                term.Children.Add(Function_Call());
            }
            else
            {
                term.Children.Add(match(Token_Class.Idenifier));
            }
            
            return term;
        }

        Node Function_Call()
        {
            Node call = new Node("Function Call");
            // Identifier (Term  paramter| Ɛ )
            call.Children.Add(match(Token_Class.Idenifier));
            call.Children.Add(match(Token_Class.LParanthesis));
            if (InputPointer < TokenStream.Count && Token_Class.RParanthesis != TokenStream[InputPointer].token_type)
            {
                call.Children.Add(Term());
                call.Children.Add(Paramter());
            }
            call.Children.Add(match(Token_Class.RParanthesis));
            return call;
        }

        Node Paramter() 
        {
            Node paramter = new Node("Paramter");
            // , Term  parameter | Ɛ
            if (InputPointer < TokenStream.Count && Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                paramter.Children.Add(match(Token_Class.Comma));
                paramter.Children.Add(Term());
                paramter.Children.Add(Paramter());
                return paramter;
            }
            else return null;
            
        }

        Node Expression()
        {
            Node expression = new Node("Expression");
            if (InputPointer < TokenStream.Count && Token_Class.varString == TokenStream[InputPointer].token_type)
            {
                expression.Children.Add(match(Token_Class.varString));
            }
            else 
            {
                expression.Children.Add(Equation());
            }
            return expression;
        }

        Node Equation()
        {
            Node equation = new Node("Equation");
            equation.Children.Add(Ter());
            equation.Children.Add(Equation2());
            return equation;
        }

        Node Equation2()
        {
            Node equation = new Node("Rest of Equation");
            if (InputPointer < TokenStream.Count && 
                (Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type))
            {
                equation.Children.Add(AddOp());
                equation.Children.Add(Equation());
                equation.Children.Add(Equation2());
                return equation;
            }
            else return null;
        }

        Node Ter()
        {
            Node ter = new Node("Ter");
            ter.Children.Add(Factor());
            ter.Children.Add(Ter2());
            return ter;
        }

        Node Ter2()
        {
            Node ter = new Node("Rest of Ter");
            if (InputPointer < TokenStream.Count &&
                (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type || Token_Class.DivideOp == TokenStream[InputPointer].token_type))
            {
                ter.Children.Add(MultOp());
                ter.Children.Add(Ter());
                ter.Children.Add(Ter2());
                return ter;
            }
            else return null;
        }

        Node Factor()
        {
            Node factor = new Node("Factor");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                factor.Children.Add(match(Token_Class.LParanthesis));
                factor.Children.Add(Equation());
                factor.Children.Add(match(Token_Class.RParanthesis));
            }
            else 
            {
                factor.Children.Add(Term());
            }
            return factor;
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
            else multop.Children.Add(match(Token_Class.MultiplyOp));
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
            else addop.Children.Add(match(Token_Class.PlusOp));
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
