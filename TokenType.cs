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
    public enum TokenType
    {
        NULL,
        IDENTIFIER,
        LITERAL_INT,
        RIGHT_PARANTHESIS,
        LEFT_PARANTHESIS,
        ASSIGNMENT,
        LESSEQUAL_OP,
        LESSTHAN_OP,
        GREATEREQUAL_OP,
        GREATERTHAN_OP, 
        EQUALTO_OP,
        NOTEQUAL_OP,
        ADD_OP,
        SUB_OP,
        MUL_OP,
        DIV_OP,
        FUNCTION,
        END,
        PRINT,
        IF,
        THEN,
        ELSE,
        WHILE,
        DO,
        REPEAT,
        UNTIL,
        BLOCK
    }
}
