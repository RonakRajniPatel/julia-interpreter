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
    public class Scanner
    {
        public StreamReader _reader;
        public Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>{
            {"function" , TokenType.FUNCTION},
            {"if" , TokenType.IF },
            { "end" , TokenType.END },
            { "then"  , TokenType.THEN },
            { "else" , TokenType.ELSE },
            { "while" , TokenType.WHILE },
            { "do" , TokenType.DO },
            { "repeat" , TokenType.REPEAT },
            {  "until" , TokenType.UNTIL },
            { "print" , TokenType.PRINT },
            { "<=" , TokenType.LESSEQUAL_OP },
            { "<" , TokenType.LESSTHAN_OP },
            { ">=" , TokenType.GREATEREQUAL_OP } ,
            { ">" , TokenType.GREATERTHAN_OP },
            { "=" , TokenType.ASSIGNMENT },
            { "==" , TokenType.EQUALTO_OP },
            { "!=" , TokenType.NOTEQUAL_OP },
            { "(" , TokenType.LEFT_PARANTHESIS },
            { ")" , TokenType.RIGHT_PARANTHESIS },
            { "+" , TokenType.ADD_OP },
            { "-" , TokenType.SUB_OP },
            { "*" , TokenType.MUL_OP },
            { "/" , TokenType.ASSIGNMENT },
            { "\t", TokenType.BLOCK }
        };

        public List<Tuple<string, TokenType>> tokens_scanned = new List<Tuple<string, TokenType>>();
        public Scanner(string filePath)
        {
            _reader = new StreamReader(filePath);

        }
        public void findNextLexeme() //skips whitespace + comments
        {
            while (_reader.Peek() == ' '
                || _reader.Peek() == '\n'
                || _reader.Peek() == '/')
            {
                skipWhiteSpace();
                skipComments();
            }
        } 
        public void skipWhiteSpace()
        {
            while (_reader.Peek() == ' ' || _reader.Peek() == '\n')
            {
                _reader.Read();
            }
        }
        public void skipComments()
        {
            if (_reader.Peek() == '/')
            {
                _reader.Read();
                if (_reader.Peek() == '/')
                {
                    while (_reader.Read() != '\n') ;
                }
            }
        }

        public string buildNextLexeme() //builds lexeme from chars and returns string
        {
            StringBuilder lexeme = new StringBuilder();
            findNextLexeme();
            char c = (char)_reader.Peek();

            if (Char.IsLetter(c))
            {
                do
                {
                    c = (char)_reader.Read();
                    lexeme.Append(c);
                } while (Char.IsLetter((char)_reader.Peek()));
            }
            else if (Char.IsNumber(c))
            {
                do
                {
                    c = (char)_reader.Read();
                    lexeme.Append(c);
                } while (Char.IsNumber((char)_reader.Peek()));
            }
            else
            {
                lexeme.Append((char)_reader.Read());
            }
            return lexeme.ToString();
        }
        public void lexloop() // main loop of program
        {
            while (!_reader.EndOfStream)
            {
                string lexeme = buildNextLexeme();
                if (isKeyword(lexeme)) 
                {
                    tokens_scanned.Add(Tuple.Create(lexeme, keywords[lexeme]));
                }
                else if (isIndentifier(lexeme))
                {
                    tokens_scanned.Add(Tuple.Create(lexeme, TokenType.IDENTIFIER));
                }
                else if (isLiteral(lexeme)) {

                    tokens_scanned.Add(Tuple.Create(lexeme, TokenType.LITERAL_INT));
                }
                else // Unknown
                {
                    tokens_scanned.Add(Tuple.Create(lexeme, TokenType.NULL));
                }
            }
        }

        public bool isKeyword(string lexeme) //checks if keyword
        {
            if (keywords.ContainsKey(lexeme))
            {
                return true;
            }
            return false;
        }
        public bool isIndentifier(string lexeme) //checks if id
        {
            if (lexeme.Length == 1 && Char.IsLetter(lexeme[0]))
            {
                return true;
            }
            return false;
        }
        public bool isLiteral(string lexeme) //checks if literal
        {
            foreach (char c in lexeme)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
