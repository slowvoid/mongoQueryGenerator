grammar QueryBuilderQueries;

NAME: [a-zA-Z_][a-zA-Z0-9_]*;
INTEGER: [0-9]+;
REAL: INTEGER '.' INTEGER;
STRING: '\'' (~["\\\r\n])* '\'';
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
	elementName = NAME '.' attribute = NAME;

alias: description = NAME;

aggregationFunction:
	'avg' '(' simpleAttribute ')' alias? # averageFunction
	| 'max' '(' simpleAttribute ')' alias? # maxFunction
	| 'min' '(' simpleAttribute ')' alias? # minFunction
	| 'sum' '(' simpleAttribute ')' alias? # sumFunction
	| 'count' '(' simpleAttribute ')' alias? # countFunction
	| 'count' '(*)' alias? # countAllFunction
    ;

where: logicalExpression;

logicalExpression:
    logicalTerm (logicalOperator logicalTerm)*;

logicalTerm:
    simpleAttribute relationalOperator value
    |  simpleAttribute rangeOperator
    | '(' logicalExpression ')'
    ;

value:
    STRING | INTEGER | REAL | 'not' 'null' | 'null';

// expressionList:
// 	simpleAttribute arithmeticExpression VALUE? NUMERIC? (
// 		logicalExpression expressionList
// 	)*
// 	| '(' simpleAttribute otherExpression ')' (
// 		logicalExpression expressionList
// 	)*
// 	| otherExpression (logicalExpression expressionList)*;

relationalOperator:
	'='
	| '<>'
	| '>='
	| '<='
	| '>'
	| '<'
	| 'like'
	| 'is'
    ;
rangeOperator returns [ string type ]:
	'between' value 'and' value { $type = "BETWEEN"; }
	| 'not' 'in' '(' query ')' { $type = "NOT_IN_QUERY"; }
	| 'not' 'in' '(' value (',' value)* ')' { $type = "NOT_IN_VALUES"; }
	| 'in' '(' query ')' { $type = "IN_QUERY"; }
	| 'in' '(' value (',' value)* ')' { $type = "IN_VALUES"; }
	| 'not' 'exists' '(' query ')' { $type = "NOT_EXISTS_QUERY"; }
	| 'exists' '(' query ')' { $type = "EXISTS_QUERY"; }
    ;

logicalOperator: 'and' | 'or';

groupby: listOfAttributes;
having:
	aggregationFunction relationalOperator value
	| logicalExpression;
orderby:
	listOfAttributes 'asc' (',' orderby)*
	| listOfAttributes 'desc' (',' orderby)*;