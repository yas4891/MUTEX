lexer grammar MutexCLexer;

options {
	language = CSharp3;
	filter = true;
	k = 2;
}


COMMENT
    :   '/*' (options {greedy=false;} : . )* '*/' 
        { $channel = Hidden;}
    ;
    
   
LINE_COMMENT

    : '//' ~('\n'|'\r')* '\r'? '\n' {$channel= Hidden;}

    ;
/* */

FUNCTION_DEFINITION
	:	DATATYPE IDENTIFIER '(' PARAMETER_LIST ')';

FUNCTION_CALL 	:	IDENTIFIER '(' PARAMETER (', ' PARAMETER)* ')';

fragment
PARAMETER
	:	IDENTIFIER | STRING_LITERAL;

PARAMETER_LIST
	:	DATATYPE IDENTIFIER (',' DATATYPE IDENTIFIER)*;

CONST_MODIFIER	:	'const';

DATATYPE :	VOID_DATATYPE | INTEGER_DATATYPE | FLOAT_DATATYPE { Skip();};

POINTER 
	:	'*'+;

VOID_DATATYPE :	 	'void';
FLOAT_DATATYPE :	'float' | 'double';

INTEGER_DATATYPE :	(SIGNED_UNSIGNED)? ('short' | 'int' | 'long' | 'char') ;

SIGNED_UNSIGNED :	'signed' | 'unsigned';

IDENTIFIER 
	:	LETTER (LETTER | DIGIT)*;

STRING_LITERAL
	:	'"' (~('\\' | '"'))* '"';

fragment
LETTER	:	'A'..'Z' | 'a'..'z' | '_';

HEX_DIGIT :	DIGIT | 'a'..'f' | 'A'..'F';

fragment
DIGIT :	'0'..'9';

