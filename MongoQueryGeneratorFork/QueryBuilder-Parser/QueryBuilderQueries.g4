grammar QueryBuilderQueries;

NAME: [a-zA-Z][a-zA-Z0-9_]*;
INTEGER: [0-9]+;
VALUE: NAME;
NUMERIC: INTEGER;
WS: (' ' | '\t' | '\n' | '\r') -> skip;

query:
	'from' entity 'select' select ('where' where)? (
		'group by' groupby
	)? ('having' having)? ('order by' orderby)?;

entity: simpleEntity | entity rjoin '(' listOfEntities ')';

simpleEntity: entityName = NAME entityAlias = NAME;

listOfEntities:
	entity
	| entity ',' listOfEntities
	| entity ',' listOfEntities rjoin '(' entity ')';

rjoin: 'rjoin' relationshipName = NAME relationshipAlias = NAME;

select:
	simpleAttribute
	| listOfAttributes
	| aggregationFunction (',' aggregationFunction)*
	| listOfAttributes (',' aggregationFunction)*;

simpleAttribute:
	entityName = NAME '.' attribute = NAME
	| relationshipName = NAME '.' attribute = NAME;

listOfAttributes:
	simpleAttribute
	| simpleAttribute ',' listOfAttributes;

alias: description = NAME;

aggregationFunction:
	'avg' '(' simpleAttribute ')' alias?
	| 'max' '(' simpleAttribute ')' alias?
	| 'min' '(' simpleAttribute ')' alias?
	| 'sum' '(' simpleAttribute ')' alias?
	| 'count' '(' simpleAttribute ')' alias?
	| 'count' '(*)' alias?;

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