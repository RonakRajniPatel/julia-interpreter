using System;
/*
 * Class:       CS 4308 Section 
 * Term:        Spring 2022
 * Name:       Ronak Patel
 * Instructor:   Sharon Perry
 * Project:     Deliverable P3 Interpreter  
 */
namespace Scanner // Note: actual namespace depends on the project name.
{
    class Program
    {
        private static bool blockRemover(Tuple<string, TokenType> T) // used for List.RemoveAll function
        {
            if (T.Item2 == TokenType.BLOCK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void Main(string[] args)
        {
            Scanner scan = new Scanner("Julia-Files/Test3.jl");
            scan.lexloop();

            scan.tokens_scanned.RemoveAll(blockRemover); //removes all BLOCK tokens

            foreach (Tuple<string, TokenType> T in scan.tokens_scanned) // writes the tokens to console
            {
                Console.WriteLine(T);
            }
            foreach (int value in Enumerable.Range(1, 3)) // creates white space between scanner and parser
            {
                Console.WriteLine();
            }

            Parser parse = new Parser(scan.tokens_scanned);
            List<string> parseTree = parse.parseStart(); // begins parser and returns parse tree to parseTree

            Console.WriteLine("\n\nFULL PARSE TREE\n\n");

            foreach (string parsed in parseTree)
            {
                Console.WriteLine(parsed);
            }

            Console.WriteLine("\n\nInterpreter RESULT\n\n");
            for (int i = 0; i < parseTree.Count(); i++) 
            {
                if (parseTree[i] == "<print_statement>")
                {
                    if (parseTree[i + 3] == "<id>") 
                    {
                        Console.WriteLine(parseTree[i + 5]);
                    }
                    else if (parseTree[i + 3] == "<literal_integer>")
                    {
                        Console.WriteLine(parseTree[i + 4]);
                    }
                }

            }
        }
    }

}