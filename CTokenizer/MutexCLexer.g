lexer grammar MutexCLexer;

options {
	language = CSharp2;
	filter = true;
	k = 2;
}


CONST_MODIFIER
	:	'const';

POINTER_DATATYPE 
	:	(FLOAT_DATATYPE | INTEGER_DATATYPE | VOID_DATATYPE) ' '* '*'+;

VOID_DATATYPE :	 	'void';
FLOAT_DATATYPE :	'float' | 'double';

INTEGER_DATATYPE :	(SIGNED_UNSIGNED)? ('short' | 'int' | 'long' | 'char') ;

SIGNED_UNSIGNED :	'signed' | 'unsigned';

IDENTIFIER 
	:	LETTER (LETTER | DIGIT)*;

LETTER	:	'A'..'Z' | 'a'..'z' | '_';

HEX_DIGIT :	DIGIT | 'a'..'f' | 'A'..'F';

DIGIT :	'0'..'9';

