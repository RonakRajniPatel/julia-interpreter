using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Class:       CS 4308 Section 
 * Term:        Spring 2022
 * Name:       Ronak Patel
 * Instructor:   Sharon Perry
 * Project:     Deliverable P3 Interpreter  
 */
namespace Scanner
{
    public class Parser
    {
        public List<Tuple<string, TokenType>> tokens = new List<Tuple<string, TokenType>>();
        public int index;
        public List<Tuple<int, string>> vars = new List<Tuple<int, string>>();
        public List<string> parseTree = new List<string>();
        public Parser(List<Tuple<string, TokenType>> tokens_scanned)
        {
            this.tokens = tokens_scanned;
            index = 0;
        }

        public List<string> parseStart()
        {
            parseProgram();
            return parseTree;
        }
        public void parseProgram() // determines of beginning of program is set correctly
        {
            if (tokens[index].Item2 == TokenType.FUNCTION)
            {
                index++;
                if (tokens[index].Item2 == TokenType.IDENTIFIER)
                {
                    index++;
                    if (tokens[index].Item2 == TokenType.LEFT_PARANTHESIS)
                    {

                        index++;
                        if (tokens[index].Item2 == TokenType.RIGHT_PARANTHESIS)
                        {
                            Console.WriteLine("<program> -> function id() <block> end");
                            parseTree.Add("<program>");
                            parseTree.Add("function id()");
                            parseTree.Add("<block>");
                            parseTree.Add("end");
                            parseBlock();
                            Console.WriteLine("end");
                        }
                        else
                        {
                            Console.WriteLine("FUNCTION failed");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("FUNCTION failed");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("FUNCTION failed");
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("FUNCTION failed");
                Environment.Exit(0);
            }
        }

        public void parseBlock()
        {
            Console.WriteLine("<block> -> <statement>");
            parseTree.Add("<block>");
            parseTree.Add("<statement>");
            index++;
            parseStatement();

            if (tokens.Count - 1 > index) // determines if there is more blocks left
            {
                parseBlock();
            }
        }

        public void parseStatement()
        {
            Console.WriteLine("<statement> -> <assignment>");
            parseTree.Add("<statement>");
            parseTree.Add("<assignment>");

            if (tokens[index].Item2 == TokenType.IF)
            {
                parseIf();
            }
            else if (tokens[index + 1].Item2 == TokenType.ASSIGNMENT)
            {
                parseAssignment();
            }
            else if (tokens[index].Item2 == TokenType.WHILE)
            {
                parseWhile();
            }
            else if (tokens[index].Item2 == TokenType.PRINT)
            {
                index++;
                parsePrint();
            }
            else
            {
                Console.WriteLine("UNABLE TO PARSE STATEMENT");
                Environment.Exit(0);
            }
        }

        public void parseIf()
        {
            Console.WriteLine("<if_statement> -> if <boolean_expression> then <block> else <block> end");
            parseTree.Add("<if_statement>");
            parseTree.Add("if <boolean_expression");
            parseTree.Add("then <block");
            parseTree.Add("else <block>");
            parseTree.Add("end");
            bool flag; // used to specify block parse later on
            if (tokens[index].Item2 == TokenType.IF)
            {
                index++;
                if (tokens[index].Item2 == TokenType.LEFT_PARANTHESIS)
                {
                    index++;
                    flag = parseBoolExpr();
                    if (tokens[index].Item2 == TokenType.RIGHT_PARANTHESIS)
                    {
                        index++;
                        if (flag)
                        {
                            parseBlock();
                        }
                        else
                        {
                            parseBlock();
                        }
                    }

                }
                
            }
            if (tokens[index].Item2 != TokenType.END)
            {
                Console.WriteLine("IF STATEMENT ERROR");
                Environment.Exit(0);
            }
        }

        public void parseWhile()
        {
            Console.WriteLine("<while_statement> -> while <boolean_expression> then <block> else <block> end");
            parseTree.Add("<while_statement>");
            parseTree.Add("while <boolean_expression>");
            parseTree.Add("then <block>");
            parseTree.Add("else <block>");
            while (parseBoolExpr())
            {
                if (tokens[index + 1].Item2 == TokenType.DO)
                {
                    parseBlock();
                }
                else
                {
                    Console.WriteLine("While Loop Failed");
                    Environment.Exit(0);
                }
            }
            Console.WriteLine("end");
            parseTree.Add("end");
        }

        public void parseAssignment()
        {
            int index_anchor = index;
            index++;

            Console.WriteLine("<assignment_statement> -> id <assignment operator> <arithmetic_expression>");
            vars.Add(Tuple.Create(parseArithmeticExpr(), tokens[index_anchor].Item1));
            
        }

        public void parsePrint()
        {
            if (tokens[index].Item2 == TokenType.LEFT_PARANTHESIS)
            {
                Console.WriteLine("<print_statement> -> print(<arithmetic_expression>)");
                parseTree.Add("<print_statement>");
                parseTree.Add("print(<arithmetic_expression>)");
                parseArithmeticExpr();
                index++;
                if (tokens[index].Item2 == TokenType.RIGHT_PARANTHESIS)
                {
                    index++;
                }
                else
                {
                    Console.WriteLine("PRINT ERROR OCCURED");
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("PRINT ERROR OCCURED");
                Environment.Exit(0);
            }
        }

        public bool parseBoolExpr()
        {
            index++;
            TokenType op = tokens[index].Item2; // keeps TokenType witout having to write whole expression each time

            if (op == TokenType.LESSEQUAL_OP)
            {
                if (parseArithmeticExpr() <= parseArithmeticExpr())
                {
                    return true;
                }
            }
            if (op == TokenType.LESSTHAN_OP)
            {
                if (parseArithmeticExpr() < parseArithmeticExpr())
                {
                    return true;
                }
            }
            if (op == TokenType.GREATEREQUAL_OP)
            {
                if (parseArithmeticExpr() >= parseArithmeticExpr())
                {
                    return true;
                }
            }
            if (op == TokenType.GREATERTHAN_OP)
            {
                if (parseArithmeticExpr() > parseArithmeticExpr())
                {
                    return true;
                }
            }
            if (op == TokenType.EQUALTO_OP)
            {
                if (parseArithmeticExpr() == parseArithmeticExpr())
                {
                    return true;
                }
            }
            if (op == TokenType.NOTEQUAL_OP)
            {
                if (parseArithmeticExpr() != parseArithmeticExpr())
                {
                    return true;
                }
            }
            return false;
        }
        public int parseArithmeticExpr()
        {
            if (tokens[index + 1].Item2 == TokenType.IDENTIFIER)
            {
                index++;
                Console.WriteLine("<arithmetic_expression> -> <id>");
                parseTree.Add("<arithmetic_expression>");
                parseTree.Add("<id>");
                return findMyInt(); // uses vars list to find int and return it upwards
            }
            else if (tokens[index + 1].Item2 == TokenType.LITERAL_INT)
            {
                index++;
                Console.WriteLine("<arithmetic_expression> -> <literal_integer>");
                parseTree.Add("arithmetic_expression");
                parseTree.Add("literal_integer>");
                return Int32.Parse(tokens[index].Item1);
            }
            else
            {
                Console.WriteLine("Error Occured. Expression couldn't be evaluated");
                Environment.Exit(0);
                return -1;
            }
        }

        public int findMyInt()
        {
            foreach (Tuple<int, string> i in vars)
            {
                if (tokens[index].Item1 == i.Item2) { //iterates through list until item is found
                    Console.WriteLine("<id> -> " + i.Item1);
                    parseTree.Add("<id>");
                    parseTree.Add(i.Item1.ToString());
                    return i.Item1;
                }
            }
            Console.WriteLine("Error Occured. Int not found");
            Environment.Exit(0);
            return -1;

        }
    }
}
