grammar QueryBuilderMapping;
 
CARDINALITY_ITEM: [1N];
ID: [a-zA-Z_][a-zA-Z0-9]+;
STRING: '"' ~["\n\r]* '"';
DIVIDER: '#'+;
WS: [ \t\r\n]+ -> skip;
 
program:
	'Solution' ':' name=STRING
	'Description' ':' descritpion=STRING
	'Version' ':' version=STRING
	DIVIDER 'ERModel' DIVIDER
	ermodel 
	DIVIDER 'MongoDBSchema' DIVIDER
	mongoschema;

ermodel: erelement*;

erelement: entity | relationship;

entity: name = ID '{' attributes? '}';

relationship:
	name = ID '(' relationshipEnd ',' relationshipEnd (
		',' relationshipEnd
	)* ')' '{' attributes? '}';

relationshipEnd: name = ID ':' cardinality = CARDINALITY_ITEM;

attributes: attribute (',' attribute)*;

attribute: name = ID ':' type = ID;

mongoschema: collection*;

collection: name = ID '[' erRefs? ']' '{' field* '}';

erRefs: erRef (',' erRef)*;

erRef: refName = ID (main = '*')?;

field: name = ID ':' type = ID '[' erAttributeRef? ']';

erAttributeRef: refName = ID '.' attributeName = ID;

