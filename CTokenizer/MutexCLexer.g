lexer grammar MutexCLexer;

options {
	language = CSharp2;
	filter = true;
	k = 2;
}

FUNCTION_DEFINITION
	:	DATATYPE IDENTIFIER '(' PARAMETER_LIST ')';

FUNCTION_CALL 	:	IDENTIFIER '(' IDENTIFIER (', ' IDENTIFIER)* ')';


PARAMETER_LIST
	:	DATATYPE IDENTIFIER (',' DATATYPE IDENTIFIER)*;



CONST_MODIFIER
	:	'const';

DATATYPE :	VOID_DATATYPE | INTEGER_DATATYPE | FLOAT_DATATYPE;

POINTER 
	:	'*'+;

VOID_DATATYPE :	 	'void';
FLOAT_DATATYPE :	'float' | 'double';

INTEGER_DATATYPE :	(SIGNED_UNSIGNED)? ('short' | 'int' | 'long' | 'char') ;

SIGNED_UNSIGNED :	'signed' | 'unsigned';

IDENTIFIER 
	:	LETTER (LETTER | DIGIT)*;

LETTER	:	'A'..'Z' | 'a'..'z' | '_';

HEX_DIGIT :	DIGIT | 'a'..'f' | 'A'..'F';

DIGIT :	'0'..'9';

