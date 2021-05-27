grammar QueryBuilderQueries;

NAME: [a-zA-Z_][a-zA-Z0-9_]*;
INTEGER: [0-9]+;
VALUE: NAME;
NUMERIC: INTEGER;
WS: (' ' | '\t' | '\n' | '\r') -> skip;

query: 'from' entity 
        'select' select
        ('where' where)? 
        ('group by' groupby)? 
        ('having' having)? 
        ('order by' orderby)?;

entity
	returns[ QueryBuilder.Operation.Arguments.QueryableEntity qEntity ]:
	simpleEntityName = NAME (simpleEntityAlias = NAME)? # simpleEntity
	| computedEntityLeft = entity 'rjoin' '<' computedEntityRelationshipName = NAME
		(computedEntityRelationshipAlias = NAME)? '>' '(' computedEntityRight += entity
		(',' computedEntityRight += entity)* ')' # computedEntity
	| '(' entity ')' # parenthesisEntity;

select:
    '*' # selectAll
    | attributeOrFunction (',' attributeOrFunction)* # selectAttributeOrFunction
    ;

attributeOrFunction:
    simpleAttribute | aggregationFunction;
    
listOfAttributes:
	simpleAttribute (',' simpleAttribute)*;

simpleAttribute:
	entityName = NAME '.' attribute = NAME
	| relationshipName = NAME '.' attribute = NAME;

alias: description = NAME;

aggregationFunction:
	'avg' '(' simpleAttribute ')' alias? # averageFunction
	| 'max' '(' simpleAttribute ')' alias? # maxFunction
	| 'min' '(' simpleAttribute ')' alias? # minFunction
	| 'sum' '(' simpleAttribute ')' alias? # sumFunction
	| 'count' '(' simpleAttribute ')' alias? # countFunction
	| 'count' '(*)' alias? # countAllFunction
    ;

where: expressionList;

expressionList:
	simpleAttribute arithmeticExpression VALUE? NUMERIC? (
		logicalExpression expressionList
	)*
	| '(' simpleAttribute otherExpression ')' (
		logicalExpression expressionList
	)*
	| otherExpression (logicalExpression expressionList)*;

arithmeticExpression:
	'='
	| '<>'
	| '>='
	| '<='
	| '>'
	| '<'
	| 'like'
	| 'is not null'
	| 'is null';
otherExpression:
	'between' NUMERIC 'and' NUMERIC
	| 'not in' '(' query ')'
	| 'not in' '(' NUMERIC (',' NUMERIC)* ')'
	| 'not in' '(' VALUE (',' VALUE)* ')'
	| 'in' '(' query ')'
	| 'in' '(' NUMERIC (',' NUMERIC)* ')'
	| 'in' '(' VALUE (',' VALUE)* ')'
	| 'not exists' '(' query ')'
	| 'exists' '(' query ')';
logicalExpression: 'and' | 'or';
groupby: listOfAttributes;
having:
	aggregationFunction arithmeticExpression NUMERIC
	| expressionList;
orderby:
	listOfAttributes 'asc' (',' orderby)*
	| listOfAttributes 'desc' (',' orderby)*;