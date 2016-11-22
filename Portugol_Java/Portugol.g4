grammar Portugol;

/*
 * Parser Rules
 */

compileUnit
	:	EOF
	;

programa
	: 'ALGORITMO' ID ';' decVars* 'INICIO' (decVars|decFuncoes|statement)*  'FIM' '.'
	;
	
decVars 
	: ('var'|'VAR')? listaVar	
	;
listaVar
	: ID (',' ID)* ':' tipo
	;

decFuncoes 
	: ID
	| ID '('listaPar')'
	;

statement
	: blocos
	| expression
	| operacao
	| decFuncoes
	;

blocos
	: if
	| switch
	;

if
	: 'SE' expression ('ENTAO' (statement)+)+  else 'FIMSE'
	;
else
	: ('SENAO' (statement)+)?
	;

switch
	: 'ESCOLHA' '('valor')'  ('CASO' valor statement)+ 'FIMESCOLHA'
	;

tipo	
	: 'INTEIRO' |'STRING'|'BOOLEANO'|'REAL'|'LOGICO'|'CARACTERE'
	;

listaPar
	: ((',')? (STRING|ID))+
	;

expression
	: valor ('='|'!='|'>'|'<'|'<-') (valor|operacao)+
	;

operacao
	: (ID|INT) (SIMBOLOS) (ID|INT)
	;

valor
	: INT
	| STRING
	| 'VERDADEIRO'
	| 'FALSO'
	| ID
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
	| '"' (ID|INT|SIMBOLOS|' '|'\t'|'\r'|'\n')+ '"'
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
