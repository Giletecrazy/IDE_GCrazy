grammar myC;

tipo : 'int' | 'float' | 'void' | 'string' ;

programa: decls 'main' '(' ')' bloco;

atribuicao: identificador '=' (string | INT | FLOAT) ';';

bloco: '{' stmt+ '}';

stmt : incremento | saida | atribuicao | expressao | decls | bloco | condicional | loop | retorno | chamada_funcao | entrada  | selecao ;

incremento : identificador '++';

decls : ( declaracao | funcao | array_decl )+ ;

funcao : tipo identificador '(' ')' '{' bloco '}' ;

declaracao: tipo identificador ('=' (string | INT | FLOAT))? ';';

argumentos : ( expressao ( ',' expressao )* ) ; 

string: '"' ( LETTER | DIGIT )+ '"' ;

identificador : LETTER ( LETTER | DIGIT )* ;

expr : termo ( operador expr )* ;


FLOAT : DIGIT+ '.' DIGIT+ ;

INT: DIGIT+;

DIGIT: [0-9];
LETTER : [a-zA-Z];

negacao: '!';

expressao: expr_aritmetica | expr_logica | expr_relacional;

operador: '+' | '-' | '*' | '/' | '=';

termo: identificador | INT | FLOAT | string | '(' expressao ')';

expr_aritmetica : expr ( operador expr )+ ;
expr_relacional : expr operador_relacional expr ;
operador_logico: '&&' | '||';

condicao : expressao operador_relacional expressao | negacao expressao ;

expr_logica: expr_logica operador_logico expr_logica | negacao expr_logica | expr_relacional;

condicional: 'if' '(' condicao ')' bloco;

loop: 'while' '(' condicao ')' bloco;

retorno: 'return' (INT | FLOAT | string) ';'; 

chamada_funcao: identificador '(' argumentos ')'; 

entrada: 'scanf' '(' identificador ')' ';';

saida: 'printf' '(' (string | identificador) ')' ';'; 

selecao: 'switch' '(' identificador | acesso_array ')' '{' casos '}';

casos: ('case' (INT | FLOAT | string)  ':' stmt+ 'break;')+ | 'default' ':' stmt+ ;

operador_relacional: '==' | '!=' | '<' | '<=' | '>' | '>=';

comparacao: expressao operador_relacional expressao;

array_decl: tipo identificador '[' INT? ']' ';';

acesso_array: identificador '[' INT? ']';

for: 'for' '(' atribuicao ';' condicao ';' incremento ')' bloco;

if_else: 'if' '(' condicao ')' bloco 'else' bloco;

Whitespace
    : [ \t\r\n]+ -> skip
    ;

LineComment
    : '//' ~[\r\n]* -> skip
    ;

BlockComment
    : '/*' .*? '*/' -> skip
    ;