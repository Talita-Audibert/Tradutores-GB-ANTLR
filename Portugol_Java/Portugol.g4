grammar Portugol;

/*
 * Parser Rules
 */

compileUnit
	:	EOF
	;

programa
	: 'ALGORITMO' ID ';' decVars* 'INICIO' decVars* decFuncoes* 'FIM' '.'
	;
	
decVars : ('var'|'VAR')? listaVar	
	;
listaVar:	ID (',' ID)* ';'
	|	ID (',' ID)* ':' tipo ';'
	;

decFuncoes 
	: ID '('listaPar')' decVars* statement*
	| ID
		;

statement: 'SE' statement 'ENTAO' statement ('SENAO' statement)?
	| 'ESCOLHA''('ID')'  ('CASO' (ID|INT))+  statement 'FIMESCOLHA'
	;

tipo	:	'INTEIRO' |'STRING'|'BOOLEANO'|'REAL'|'LOGICO'
	;

listaPar: STRING|ID
	| (',' STRING|ID)*
	;


valor	:    INT
	|    STRING
	|    'VERDADEIRO'
	|    'FALSO'
	; 
	
ID  :	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;

INT :	'0'..'9'+
    ;
    
SIMBOLOS 
	: ('+'|'-'|'*'|'/'|'='|'!'|'>'|'<'|','|';'|'('|')'|':'|'.')
	;
	
WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) -> channel(HIDDEN)
    ;

STRING
    :  '"' ( ESC_SEQ | ~('\\'|'"') )* '"'
    ;

fragment
HEX_DIGIT : ('0'..'9'|'a'..'f'|'A'..'F') ;

fragment
ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   UNICODE_ESC
    |   OCTAL_ESC
    ;

fragment
OCTAL_ESC
    :   '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7')
    ;

fragment
UNICODE_ESC
    :   '\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT
    ;
