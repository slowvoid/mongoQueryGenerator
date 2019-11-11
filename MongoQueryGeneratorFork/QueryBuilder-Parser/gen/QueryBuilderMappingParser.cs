//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /home/daniel/GitProjects/mongoQueryGenerator/MongoQueryGeneratorFork/QueryBuilder-Parser/QueryBuilderMapping.g4 by ANTLR 4.7.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace QueryBuilder.Parser {
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
[System.CLSCompliant(false)]
public partial class QueryBuilderMappingParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, CARDINALITY_ITEM=16, 
		ID=17, STRING=18, DIVIDER=19, WS=20;
	public const int
		RULE_program = 0, RULE_ermodel = 1, RULE_erelement = 2, RULE_entity = 3, 
		RULE_relationship = 4, RULE_relationshipEnd = 5, RULE_attribute = 6, RULE_mongoschema = 7, 
		RULE_collection = 8, RULE_erRefs = 9, RULE_erRef = 10, RULE_field = 11, 
		RULE_erAttributeRef = 12;
	public static readonly string[] ruleNames = {
		"program", "ermodel", "erelement", "entity", "relationship", "relationshipEnd", 
		"attribute", "mongoschema", "collection", "erRefs", "erRef", "field", 
		"erAttributeRef"
	};

	private static readonly string[] _LiteralNames = {
		null, "'Solution'", "':'", "'Description'", "'Version'", "'ERModel'", 
		"'MongoDBSchema'", "'{'", "'}'", "'('", "','", "')'", "'['", "']'", "'*'", 
		"'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, "CARDINALITY_ITEM", "ID", "STRING", "DIVIDER", 
		"WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "QueryBuilderMapping.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static QueryBuilderMappingParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public QueryBuilderMappingParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public QueryBuilderMappingParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}
	public partial class ProgramContext : ParserRuleContext {
		public IToken name;
		public IToken descritpion;
		public IToken version;
		public ITerminalNode[] DIVIDER() { return GetTokens(QueryBuilderMappingParser.DIVIDER); }
		public ITerminalNode DIVIDER(int i) {
			return GetToken(QueryBuilderMappingParser.DIVIDER, i);
		}
		public ErmodelContext ermodel() {
			return GetRuleContext<ErmodelContext>(0);
		}
		public MongoschemaContext mongoschema() {
			return GetRuleContext<MongoschemaContext>(0);
		}
		public ITerminalNode[] STRING() { return GetTokens(QueryBuilderMappingParser.STRING); }
		public ITerminalNode STRING(int i) {
			return GetToken(QueryBuilderMappingParser.STRING, i);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_program; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitProgram(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ProgramContext program() {
		ProgramContext _localctx = new ProgramContext(Context, State);
		EnterRule(_localctx, 0, RULE_program);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 26; Match(T__0);
			State = 27; Match(T__1);
			State = 28; _localctx.name = Match(STRING);
			State = 29; Match(T__2);
			State = 30; Match(T__1);
			State = 31; _localctx.descritpion = Match(STRING);
			State = 32; Match(T__3);
			State = 33; Match(T__1);
			State = 34; _localctx.version = Match(STRING);
			State = 35; Match(DIVIDER);
			State = 36; Match(T__4);
			State = 37; Match(DIVIDER);
			State = 38; ermodel();
			State = 39; Match(DIVIDER);
			State = 40; Match(T__5);
			State = 41; Match(DIVIDER);
			State = 42; mongoschema();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ErmodelContext : ParserRuleContext {
		public ErelementContext[] erelement() {
			return GetRuleContexts<ErelementContext>();
		}
		public ErelementContext erelement(int i) {
			return GetRuleContext<ErelementContext>(i);
		}
		public ErmodelContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_ermodel; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitErmodel(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ErmodelContext ermodel() {
		ErmodelContext _localctx = new ErmodelContext(Context, State);
		EnterRule(_localctx, 2, RULE_ermodel);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 47;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==ID) {
				{
				{
				State = 44; erelement();
				}
				}
				State = 49;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ErelementContext : ParserRuleContext {
		public EntityContext entity() {
			return GetRuleContext<EntityContext>(0);
		}
		public RelationshipContext relationship() {
			return GetRuleContext<RelationshipContext>(0);
		}
		public ErelementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_erelement; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitErelement(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ErelementContext erelement() {
		ErelementContext _localctx = new ErelementContext(Context, State);
		EnterRule(_localctx, 4, RULE_erelement);
		try {
			State = 52;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 50; entity();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 51; relationship();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class EntityContext : ParserRuleContext {
		public IToken name;
		public ITerminalNode ID() { return GetToken(QueryBuilderMappingParser.ID, 0); }
		public AttributeContext[] attribute() {
			return GetRuleContexts<AttributeContext>();
		}
		public AttributeContext attribute(int i) {
			return GetRuleContext<AttributeContext>(i);
		}
		public EntityContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_entity; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitEntity(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public EntityContext entity() {
		EntityContext _localctx = new EntityContext(Context, State);
		EnterRule(_localctx, 6, RULE_entity);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 54; _localctx.name = Match(ID);
			State = 55; Match(T__6);
			State = 59;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==ID) {
				{
				{
				State = 56; attribute();
				}
				}
				State = 61;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 62; Match(T__7);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class RelationshipContext : ParserRuleContext {
		public IToken name;
		public RelationshipEndContext[] relationshipEnd() {
			return GetRuleContexts<RelationshipEndContext>();
		}
		public RelationshipEndContext relationshipEnd(int i) {
			return GetRuleContext<RelationshipEndContext>(i);
		}
		public ITerminalNode ID() { return GetToken(QueryBuilderMappingParser.ID, 0); }
		public AttributeContext[] attribute() {
			return GetRuleContexts<AttributeContext>();
		}
		public AttributeContext attribute(int i) {
			return GetRuleContext<AttributeContext>(i);
		}
		public RelationshipContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_relationship; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitRelationship(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public RelationshipContext relationship() {
		RelationshipContext _localctx = new RelationshipContext(Context, State);
		EnterRule(_localctx, 8, RULE_relationship);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 64; _localctx.name = Match(ID);
			State = 65; Match(T__8);
			State = 66; relationshipEnd();
			State = 67; Match(T__9);
			State = 68; relationshipEnd();
			State = 73;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__9) {
				{
				{
				State = 69; Match(T__9);
				State = 70; relationshipEnd();
				}
				}
				State = 75;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 76; Match(T__10);
			State = 77; Match(T__6);
			State = 81;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==ID) {
				{
				{
				State = 78; attribute();
				}
				}
				State = 83;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 84; Match(T__7);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class RelationshipEndContext : ParserRuleContext {
		public IToken name;
		public IToken cardinality;
		public ITerminalNode ID() { return GetToken(QueryBuilderMappingParser.ID, 0); }
		public ITerminalNode CARDINALITY_ITEM() { return GetToken(QueryBuilderMappingParser.CARDINALITY_ITEM, 0); }
		public RelationshipEndContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_relationshipEnd; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitRelationshipEnd(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public RelationshipEndContext relationshipEnd() {
		RelationshipEndContext _localctx = new RelationshipEndContext(Context, State);
		EnterRule(_localctx, 10, RULE_relationshipEnd);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 86; _localctx.name = Match(ID);
			State = 87; Match(T__1);
			State = 88; _localctx.cardinality = Match(CARDINALITY_ITEM);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class AttributeContext : ParserRuleContext {
		public IToken name;
		public IToken type;
		public ITerminalNode[] ID() { return GetTokens(QueryBuilderMappingParser.ID); }
		public ITerminalNode ID(int i) {
			return GetToken(QueryBuilderMappingParser.ID, i);
		}
		public AttributeContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_attribute; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAttribute(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public AttributeContext attribute() {
		AttributeContext _localctx = new AttributeContext(Context, State);
		EnterRule(_localctx, 12, RULE_attribute);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 90; _localctx.name = Match(ID);
			State = 91; Match(T__1);
			State = 92; _localctx.type = Match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class MongoschemaContext : ParserRuleContext {
		public CollectionContext[] collection() {
			return GetRuleContexts<CollectionContext>();
		}
		public CollectionContext collection(int i) {
			return GetRuleContext<CollectionContext>(i);
		}
		public MongoschemaContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_mongoschema; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitMongoschema(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public MongoschemaContext mongoschema() {
		MongoschemaContext _localctx = new MongoschemaContext(Context, State);
		EnterRule(_localctx, 14, RULE_mongoschema);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 97;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==ID) {
				{
				{
				State = 94; collection();
				}
				}
				State = 99;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class CollectionContext : ParserRuleContext {
		public IToken name;
		public ITerminalNode ID() { return GetToken(QueryBuilderMappingParser.ID, 0); }
		public ErRefsContext erRefs() {
			return GetRuleContext<ErRefsContext>(0);
		}
		public FieldContext[] field() {
			return GetRuleContexts<FieldContext>();
		}
		public FieldContext field(int i) {
			return GetRuleContext<FieldContext>(i);
		}
		public CollectionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_collection; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitCollection(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public CollectionContext collection() {
		CollectionContext _localctx = new CollectionContext(Context, State);
		EnterRule(_localctx, 16, RULE_collection);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 100; _localctx.name = Match(ID);
			State = 101; Match(T__11);
			State = 103;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==ID) {
				{
				State = 102; erRefs();
				}
			}

			State = 105; Match(T__12);
			State = 106; Match(T__6);
			State = 110;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==ID) {
				{
				{
				State = 107; field();
				}
				}
				State = 112;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 113; Match(T__7);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ErRefsContext : ParserRuleContext {
		public ErRefContext[] erRef() {
			return GetRuleContexts<ErRefContext>();
		}
		public ErRefContext erRef(int i) {
			return GetRuleContext<ErRefContext>(i);
		}
		public ErRefsContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_erRefs; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitErRefs(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ErRefsContext erRefs() {
		ErRefsContext _localctx = new ErRefsContext(Context, State);
		EnterRule(_localctx, 18, RULE_erRefs);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 115; erRef();
			State = 120;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__9) {
				{
				{
				State = 116; Match(T__9);
				State = 117; erRef();
				}
				}
				State = 122;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ErRefContext : ParserRuleContext {
		public IToken refName;
		public IToken main;
		public ITerminalNode ID() { return GetToken(QueryBuilderMappingParser.ID, 0); }
		public ErRefContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_erRef; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitErRef(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ErRefContext erRef() {
		ErRefContext _localctx = new ErRefContext(Context, State);
		EnterRule(_localctx, 20, RULE_erRef);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 123; _localctx.refName = Match(ID);
			State = 125;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__13) {
				{
				State = 124; _localctx.main = Match(T__13);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FieldContext : ParserRuleContext {
		public IToken name;
		public IToken type;
		public ITerminalNode[] ID() { return GetTokens(QueryBuilderMappingParser.ID); }
		public ITerminalNode ID(int i) {
			return GetToken(QueryBuilderMappingParser.ID, i);
		}
		public ErAttributeRefContext erAttributeRef() {
			return GetRuleContext<ErAttributeRefContext>(0);
		}
		public FieldContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_field; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitField(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FieldContext field() {
		FieldContext _localctx = new FieldContext(Context, State);
		EnterRule(_localctx, 22, RULE_field);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 127; _localctx.name = Match(ID);
			State = 128; Match(T__1);
			State = 129; _localctx.type = Match(ID);
			State = 130; Match(T__11);
			State = 132;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==ID) {
				{
				State = 131; erAttributeRef();
				}
			}

			State = 134; Match(T__12);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ErAttributeRefContext : ParserRuleContext {
		public IToken refName;
		public IToken attributeName;
		public ITerminalNode[] ID() { return GetTokens(QueryBuilderMappingParser.ID); }
		public ITerminalNode ID(int i) {
			return GetToken(QueryBuilderMappingParser.ID, i);
		}
		public ErAttributeRefContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_erAttributeRef; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IQueryBuilderMappingVisitor<TResult> typedVisitor = visitor as IQueryBuilderMappingVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitErAttributeRef(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ErAttributeRefContext erAttributeRef() {
		ErAttributeRefContext _localctx = new ErAttributeRefContext(Context, State);
		EnterRule(_localctx, 24, RULE_erAttributeRef);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 136; _localctx.refName = Match(ID);
			State = 137; Match(T__14);
			State = 138; _localctx.attributeName = Match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x16', '\x8F', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', 
		'\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', '\t', '\v', 
		'\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', '\t', 
		'\xE', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', '\a', '\x3', 
		'\x30', '\n', '\x3', '\f', '\x3', '\xE', '\x3', '\x33', '\v', '\x3', '\x3', 
		'\x4', '\x3', '\x4', '\x5', '\x4', '\x37', '\n', '\x4', '\x3', '\x5', 
		'\x3', '\x5', '\x3', '\x5', '\a', '\x5', '<', '\n', '\x5', '\f', '\x5', 
		'\xE', '\x5', '?', '\v', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\a', '\x6', 'J', '\n', '\x6', '\f', '\x6', '\xE', '\x6', 
		'M', '\v', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\a', '\x6', 
		'R', '\n', '\x6', '\f', '\x6', '\xE', '\x6', 'U', '\v', '\x6', '\x3', 
		'\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\t', '\a', 
		'\t', '\x62', '\n', '\t', '\f', '\t', '\xE', '\t', '\x65', '\v', '\t', 
		'\x3', '\n', '\x3', '\n', '\x3', '\n', '\x5', '\n', 'j', '\n', '\n', '\x3', 
		'\n', '\x3', '\n', '\x3', '\n', '\a', '\n', 'o', '\n', '\n', '\f', '\n', 
		'\xE', '\n', 'r', '\v', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\v', '\x3', 
		'\v', '\x3', '\v', '\a', '\v', 'y', '\n', '\v', '\f', '\v', '\xE', '\v', 
		'|', '\v', '\v', '\x3', '\f', '\x3', '\f', '\x5', '\f', '\x80', '\n', 
		'\f', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', 
		'\x5', '\r', '\x87', '\n', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x2', '\x2', 
		'\xF', '\x2', '\x4', '\x6', '\b', '\n', '\f', '\xE', '\x10', '\x12', '\x14', 
		'\x16', '\x18', '\x1A', '\x2', '\x2', '\x2', '\x8C', '\x2', '\x1C', '\x3', 
		'\x2', '\x2', '\x2', '\x4', '\x31', '\x3', '\x2', '\x2', '\x2', '\x6', 
		'\x36', '\x3', '\x2', '\x2', '\x2', '\b', '\x38', '\x3', '\x2', '\x2', 
		'\x2', '\n', '\x42', '\x3', '\x2', '\x2', '\x2', '\f', 'X', '\x3', '\x2', 
		'\x2', '\x2', '\xE', '\\', '\x3', '\x2', '\x2', '\x2', '\x10', '\x63', 
		'\x3', '\x2', '\x2', '\x2', '\x12', '\x66', '\x3', '\x2', '\x2', '\x2', 
		'\x14', 'u', '\x3', '\x2', '\x2', '\x2', '\x16', '}', '\x3', '\x2', '\x2', 
		'\x2', '\x18', '\x81', '\x3', '\x2', '\x2', '\x2', '\x1A', '\x8A', '\x3', 
		'\x2', '\x2', '\x2', '\x1C', '\x1D', '\a', '\x3', '\x2', '\x2', '\x1D', 
		'\x1E', '\a', '\x4', '\x2', '\x2', '\x1E', '\x1F', '\a', '\x14', '\x2', 
		'\x2', '\x1F', ' ', '\a', '\x5', '\x2', '\x2', ' ', '!', '\a', '\x4', 
		'\x2', '\x2', '!', '\"', '\a', '\x14', '\x2', '\x2', '\"', '#', '\a', 
		'\x6', '\x2', '\x2', '#', '$', '\a', '\x4', '\x2', '\x2', '$', '%', '\a', 
		'\x14', '\x2', '\x2', '%', '&', '\a', '\x15', '\x2', '\x2', '&', '\'', 
		'\a', '\a', '\x2', '\x2', '\'', '(', '\a', '\x15', '\x2', '\x2', '(', 
		')', '\x5', '\x4', '\x3', '\x2', ')', '*', '\a', '\x15', '\x2', '\x2', 
		'*', '+', '\a', '\b', '\x2', '\x2', '+', ',', '\a', '\x15', '\x2', '\x2', 
		',', '-', '\x5', '\x10', '\t', '\x2', '-', '\x3', '\x3', '\x2', '\x2', 
		'\x2', '.', '\x30', '\x5', '\x6', '\x4', '\x2', '/', '.', '\x3', '\x2', 
		'\x2', '\x2', '\x30', '\x33', '\x3', '\x2', '\x2', '\x2', '\x31', '/', 
		'\x3', '\x2', '\x2', '\x2', '\x31', '\x32', '\x3', '\x2', '\x2', '\x2', 
		'\x32', '\x5', '\x3', '\x2', '\x2', '\x2', '\x33', '\x31', '\x3', '\x2', 
		'\x2', '\x2', '\x34', '\x37', '\x5', '\b', '\x5', '\x2', '\x35', '\x37', 
		'\x5', '\n', '\x6', '\x2', '\x36', '\x34', '\x3', '\x2', '\x2', '\x2', 
		'\x36', '\x35', '\x3', '\x2', '\x2', '\x2', '\x37', '\a', '\x3', '\x2', 
		'\x2', '\x2', '\x38', '\x39', '\a', '\x13', '\x2', '\x2', '\x39', '=', 
		'\a', '\t', '\x2', '\x2', ':', '<', '\x5', '\xE', '\b', '\x2', ';', ':', 
		'\x3', '\x2', '\x2', '\x2', '<', '?', '\x3', '\x2', '\x2', '\x2', '=', 
		';', '\x3', '\x2', '\x2', '\x2', '=', '>', '\x3', '\x2', '\x2', '\x2', 
		'>', '@', '\x3', '\x2', '\x2', '\x2', '?', '=', '\x3', '\x2', '\x2', '\x2', 
		'@', '\x41', '\a', '\n', '\x2', '\x2', '\x41', '\t', '\x3', '\x2', '\x2', 
		'\x2', '\x42', '\x43', '\a', '\x13', '\x2', '\x2', '\x43', '\x44', '\a', 
		'\v', '\x2', '\x2', '\x44', '\x45', '\x5', '\f', '\a', '\x2', '\x45', 
		'\x46', '\a', '\f', '\x2', '\x2', '\x46', 'K', '\x5', '\f', '\a', '\x2', 
		'G', 'H', '\a', '\f', '\x2', '\x2', 'H', 'J', '\x5', '\f', '\a', '\x2', 
		'I', 'G', '\x3', '\x2', '\x2', '\x2', 'J', 'M', '\x3', '\x2', '\x2', '\x2', 
		'K', 'I', '\x3', '\x2', '\x2', '\x2', 'K', 'L', '\x3', '\x2', '\x2', '\x2', 
		'L', 'N', '\x3', '\x2', '\x2', '\x2', 'M', 'K', '\x3', '\x2', '\x2', '\x2', 
		'N', 'O', '\a', '\r', '\x2', '\x2', 'O', 'S', '\a', '\t', '\x2', '\x2', 
		'P', 'R', '\x5', '\xE', '\b', '\x2', 'Q', 'P', '\x3', '\x2', '\x2', '\x2', 
		'R', 'U', '\x3', '\x2', '\x2', '\x2', 'S', 'Q', '\x3', '\x2', '\x2', '\x2', 
		'S', 'T', '\x3', '\x2', '\x2', '\x2', 'T', 'V', '\x3', '\x2', '\x2', '\x2', 
		'U', 'S', '\x3', '\x2', '\x2', '\x2', 'V', 'W', '\a', '\n', '\x2', '\x2', 
		'W', '\v', '\x3', '\x2', '\x2', '\x2', 'X', 'Y', '\a', '\x13', '\x2', 
		'\x2', 'Y', 'Z', '\a', '\x4', '\x2', '\x2', 'Z', '[', '\a', '\x12', '\x2', 
		'\x2', '[', '\r', '\x3', '\x2', '\x2', '\x2', '\\', ']', '\a', '\x13', 
		'\x2', '\x2', ']', '^', '\a', '\x4', '\x2', '\x2', '^', '_', '\a', '\x13', 
		'\x2', '\x2', '_', '\xF', '\x3', '\x2', '\x2', '\x2', '`', '\x62', '\x5', 
		'\x12', '\n', '\x2', '\x61', '`', '\x3', '\x2', '\x2', '\x2', '\x62', 
		'\x65', '\x3', '\x2', '\x2', '\x2', '\x63', '\x61', '\x3', '\x2', '\x2', 
		'\x2', '\x63', '\x64', '\x3', '\x2', '\x2', '\x2', '\x64', '\x11', '\x3', 
		'\x2', '\x2', '\x2', '\x65', '\x63', '\x3', '\x2', '\x2', '\x2', '\x66', 
		'g', '\a', '\x13', '\x2', '\x2', 'g', 'i', '\a', '\xE', '\x2', '\x2', 
		'h', 'j', '\x5', '\x14', '\v', '\x2', 'i', 'h', '\x3', '\x2', '\x2', '\x2', 
		'i', 'j', '\x3', '\x2', '\x2', '\x2', 'j', 'k', '\x3', '\x2', '\x2', '\x2', 
		'k', 'l', '\a', '\xF', '\x2', '\x2', 'l', 'p', '\a', '\t', '\x2', '\x2', 
		'm', 'o', '\x5', '\x18', '\r', '\x2', 'n', 'm', '\x3', '\x2', '\x2', '\x2', 
		'o', 'r', '\x3', '\x2', '\x2', '\x2', 'p', 'n', '\x3', '\x2', '\x2', '\x2', 
		'p', 'q', '\x3', '\x2', '\x2', '\x2', 'q', 's', '\x3', '\x2', '\x2', '\x2', 
		'r', 'p', '\x3', '\x2', '\x2', '\x2', 's', 't', '\a', '\n', '\x2', '\x2', 
		't', '\x13', '\x3', '\x2', '\x2', '\x2', 'u', 'z', '\x5', '\x16', '\f', 
		'\x2', 'v', 'w', '\a', '\f', '\x2', '\x2', 'w', 'y', '\x5', '\x16', '\f', 
		'\x2', 'x', 'v', '\x3', '\x2', '\x2', '\x2', 'y', '|', '\x3', '\x2', '\x2', 
		'\x2', 'z', 'x', '\x3', '\x2', '\x2', '\x2', 'z', '{', '\x3', '\x2', '\x2', 
		'\x2', '{', '\x15', '\x3', '\x2', '\x2', '\x2', '|', 'z', '\x3', '\x2', 
		'\x2', '\x2', '}', '\x7F', '\a', '\x13', '\x2', '\x2', '~', '\x80', '\a', 
		'\x10', '\x2', '\x2', '\x7F', '~', '\x3', '\x2', '\x2', '\x2', '\x7F', 
		'\x80', '\x3', '\x2', '\x2', '\x2', '\x80', '\x17', '\x3', '\x2', '\x2', 
		'\x2', '\x81', '\x82', '\a', '\x13', '\x2', '\x2', '\x82', '\x83', '\a', 
		'\x4', '\x2', '\x2', '\x83', '\x84', '\a', '\x13', '\x2', '\x2', '\x84', 
		'\x86', '\a', '\xE', '\x2', '\x2', '\x85', '\x87', '\x5', '\x1A', '\xE', 
		'\x2', '\x86', '\x85', '\x3', '\x2', '\x2', '\x2', '\x86', '\x87', '\x3', 
		'\x2', '\x2', '\x2', '\x87', '\x88', '\x3', '\x2', '\x2', '\x2', '\x88', 
		'\x89', '\a', '\xF', '\x2', '\x2', '\x89', '\x19', '\x3', '\x2', '\x2', 
		'\x2', '\x8A', '\x8B', '\a', '\x13', '\x2', '\x2', '\x8B', '\x8C', '\a', 
		'\x11', '\x2', '\x2', '\x8C', '\x8D', '\a', '\x13', '\x2', '\x2', '\x8D', 
		'\x1B', '\x3', '\x2', '\x2', '\x2', '\r', '\x31', '\x36', '=', 'K', 'S', 
		'\x63', 'i', 'p', 'z', '\x7F', '\x86',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace QueryBuilder.Parser
