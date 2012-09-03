/*
 * this grammar is used to tokenize C/C++ source code. 
 * In contrast to normal lexers, which try to aim for correctness of the 
 * resulting token stream, this lexer is used to fight plagiarism in source 
 * code. 
 * This lexer is used at university to make plagiarism more difficult between students
 * who have been assigned the same task during CS courses. 
 *
 * To fight plagiarism it merges different expressions that have the same results into the same
 * token. 
 * e.g. 
      i++
      ++i
      i += 1
      i = i + 1
 * those four expressions all create the same token ==> INCREMENT       
 *
 */

lexer grammar MutexCLexer;

options {
	language = CSharp3;
	filter = true;
}
 
// ignore multi line comment
COMMENT
    :   '/*' (options {greedy=false;} : . )* '*/' 
        { $channel = Hidden;}
    ;
    
// ignore single line comment   
LINE_COMMENT
//    : '//' ~('\n'|'\r')* ('\r' | '\n')+ {$channel= Hidden;}
    : '//' ~('\n'|'\r')* '\r'? '\n' {$channel= Hidden;}
    ;
/* */

// ignore pre-processor as well
LINE_COMMAND 
//    : '#' ~('\n'|'\r')* ('\r' |'\n')+ {$channel=Hidden;}
    : '#' ~('\n'|'\r')* '\r'? '\n' {$channel=Hidden;}
    ;

FOR_LOOP : 
  FOR WS LPARENTHESIS (~')')* RPARENTHESIS;
  
WHILE_LOOP : 
  WHILE WS LPARENTHESIS (~')')* RPARENTHESIS;
  
COMPARISON: 
  (IDENTIFIER) WS COMPARISONOPERATOR WS (LITERAL);
  
COMPARISONOPERATOR:
  (COMPARISONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL); 
  
DECLARATION_ASSIGNMENT: 
  (INTEGER_DATATYPE | POINTER_DATATYPE | FLOAT_DATATYPE) WS IDENTIFIER WS ASSIGN 
  WS (INTEGER_LITERAL | STRING_LITERAL | DATATYPE);

fragment
DATATYPE:
  INTEGER_DATATYPE | POINTER_DATATYPE | FLOAT_DATATYPE ;
  
  
POINTER_DATATYPE: (INTEGER_DATATYPE | FLOAT_DATATYPE | VOID) WS STAR+;

FLOAT_DATATYPE :	CONST? WS 'float' | 'double';

/*
 * ignore 'const' and 'signed' | 'unsigned' keywords
 */
INTEGER_DATATYPE :	CONST? WS (SIGNED_UNSIGNED)? WS ('short' | 'int' | 'long' | 'char') ;

INCREMENT : IDENTIFIER (PLUSPLUS | WS ADDEQUAL WS '1') | PLUSPLUS IDENTIFIER ;

fragment
LITERAL: STRING_LITERAL | INTEGER_LITERAL;
/*
 * covers both decimal and hex integer literals
 */
INTEGER_LITERAL : 
   DIGIT+ | '0x' HEX_DIGIT+;
  
STRING_LITERAL
	:	'"' (~('"'))* '"'; 
	// : '"' (~('\\' | '"'))* '"';


/*
 * the elementary tokens need to be below the aggregated tokens
 * else the aggregated tokens will not work
 */
LCURLYBRACE         : '{';
RCURLYBRACE         : '}';
LSQUAREBRACKET      : '[';
RSQUAREBRACKET      : ']';
LPARENTHESIS        : '('; 
RPARENTHESIS        : ')';
SCOPE               : '::';
QUESTIONMARK        : '?';
COLON               : ':';
ADDEQUAL            : '+=';
MINUSEQUAL          : '-=';
TIMESEQUAL          : '*=';
DIVIDEEQUAL         : '/=';
MODEQUAL            : '%=';
SHIFTLEFTEQUAL      : '<<=';
SHIFTRIGHTEQUAL     : '>>=';
ANDEQUAL            : '&=';
OREQUAL             : '|=';
XOREQUAL            : '^=';
SHORTCIRCUITOR      : '||';
SHORTCIRCUITAND     : '&&';
BITWISEOR           : '|';
BITWISEAND          : '&';
COMPARISONEQUAL     : '==';
NOTEQUAL            : '!=';
LESSTHANOREQUAL     : '<=';
GREATERTHANOREQUAL  : '>=';
SHIFTLEFT           : '<<';
SHIFTRIGHT          : '>>';
ASSIGN              : '=';
LESSTHAN            : '<';
GREATERTHAN         : '>';
PLUS                : '+';
MINUS               : '-';
STAR                : '*';
DIVIDE              : '/';
MOD                 : '%';
PLUSPLUS            : '++';
MINUSMINUS          : '--';
TILDE               : '~';
NOT                 : '!';
DOT                 : '.';
POINTERTO           : '->';
BREAK               : 'break';
CASE                : 'case';
CATCH               : 'catch';
//CHAR                : 'char';
CLASS               : 'class';
CONST               : 'const';
CONTINUE            : 'continue';
_DEFAULT            : 'default';
DELETE              : 'delete';
DO                  : 'do';
//DOUBLE              : 'double';
ELSE                : 'else';
ENUM                : 'enum';
EXTERN              : 'extern';
//FLOAT               : 'float';
FOR                 : 'for';
FRIEND              : 'friend';
GOTO                : 'goto';
IF                  : 'if';
INLINE              : 'inline';
//INT                 : 'int';
//LONG                : 'long';
NEW                 : 'new';
PRIVATE             : 'private';
PROTECTED           : 'protected';
PUBLIC              : 'public';
REDECLARED          : 'redeclared';
REGISTER            : 'register';
RETURN              : 'return';
//SHORT               : 'short';
//SIGNED              : 'signed';
SIZEOF              : 'sizeof';
STATIC              : 'static';
STRUCT              : 'struct';
SWITCH              : 'switch';
TEMPLATE            : 'template'; 
THIS                : 'this';
TRY                 : 'try';
TYPEDEF             : 'typedef';
UNION               : 'union';
UNSIGNED            : 'unsigned';
VIRTUAL             : 'virtual';
VOID                : 'void';
VOLATILE            : 'volatile';
WHILE               : 'while';
OPERATOR            : 'operator';
TRUETOK             : 'true';
FALSETOK            : 'false';
BACKSLASH           : '\\';


// place this below the keywords
IDENTIFIER 
  : LETTER (LETTER | DIGIT)*;
  
  
  
fragment
LETTER	:	'A'..'Z' | 'a'..'z';

fragment
HEX_DIGIT :	DIGIT | 'a'..'f' | 'A'..'F';

fragment
DIGIT :	'0'..'9';

fragment 
WS : (' ' | '\n' | '\r' | '\t')*;

fragment
CONST_MODIFIER	:	'const';

//fragment
//VOID_DATATYPE :	 	'void';


fragment
SIGNED_UNSIGNED :	'signed' | 'unsigned';
