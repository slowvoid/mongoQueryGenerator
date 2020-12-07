grammar QueryBuilderMapping;

CARDINALITY_ITEM: [1N];
ID: [a-zA-Z_][a-zA-Z0-9_]+;
STRING: '"' ~["\n\r]* '"';
DIVIDER: '#'+;
WS: [ \t\r\n]+ -> skip;

program:
 	'__Solution__' ':' name = STRING '__Description__' ':' description = STRING '__Version__' ':' version =
 		STRING DIVIDER 'ERModel' DIVIDER ermodel DIVIDER 'MongoDBSchema' DIVIDER mongoschema;

ermodel: erelement*;

erelement: entity | relationship;

entity: name = ID '{' attribute* '}';

relationship:
	name = ID '(' relationshipEnd ',' relationshipEnd (
		',' relationshipEnd
	)* ')' '{' attribute* '}';

relationshipEnd: name = ID;

attribute: (primarykey = '>')? name = ID ':' type = ID  (multivalued = '[]')?;

mongoschema: collection*;

collection: name = ID '<' erRefs? '>' '{' field* '}';

erRefs: erRef (',' erRef)*;

erRef: refName = ID (main = '*')?;

field: name = ID ':' fieldType erAttributeRef?;

fieldType: simpleType | complexType;

simpleType: monovaluedType = ID | '[' multivaluedType = ID ']';

complexType:
	'{' monovaluedFields += field* '}'
	| '[' '{' multivaluedFields += field* '}' ']';

erAttributeRef: '<' refName = ID '.' attributeName = ID '>';

