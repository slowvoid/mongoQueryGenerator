// Generated from /Users/daniellucredio/GitProjects/mongoQueryGenerator/MongoQueryGenerator/QueryBuilder-Parser/QueryBuilderQueries.g4 by ANTLR 4.8
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class QueryBuilderQueriesParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.8", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, NAME=31, INTEGER=32, 
		VALUE=33, NUMERIC=34, WS=35;
	public static final int
		RULE_query = 0, RULE_entity = 1, RULE_select = 2, RULE_simpleAttribute = 3, 
		RULE_listOfAttributes = 4, RULE_alias = 5, RULE_aggregationFunction = 6, 
		RULE_where = 7, RULE_expressionList = 8, RULE_arithmeticExpression = 9, 
		RULE_otherExpression = 10, RULE_logicalExpression = 11, RULE_groupby = 12, 
		RULE_having = 13, RULE_orderby = 14;
	private static String[] makeRuleNames() {
		return new String[] {
			"query", "entity", "select", "simpleAttribute", "listOfAttributes", "alias", 
			"aggregationFunction", "where", "expressionList", "arithmeticExpression", 
			"otherExpression", "logicalExpression", "groupby", "having", "orderby"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'from'", "'rjoin'", "'<'", "'>'", "'('", "','", "')'", "'.'", 
			"'avg'", "'max'", "'min'", "'sum'", "'count'", "'(*)'", "'='", "'<>'", 
			"'>='", "'<='", "'like'", "'is not null'", "'is null'", "'between'", 
			"'and'", "'not in'", "'in'", "'not exists'", "'exists'", "'or'", "'asc'", 
			"'desc'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, null, null, null, "NAME", "INTEGER", "VALUE", 
			"NUMERIC", "WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "QueryBuilderQueries.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public QueryBuilderQueriesParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class QueryContext extends ParserRuleContext {
		public EntityContext entity() {
			return getRuleContext(EntityContext.class,0);
		}
		public QueryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_query; }
	}

	public final QueryContext query() throws RecognitionException {
		QueryContext _localctx = new QueryContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_query);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(30);
			match(T__0);
			setState(31);
			entity(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EntityContext extends ParserRuleContext {
		public QueryBuilder.Operation.Arguments.QueryableEntity qEntity;
		public EntityContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_entity; }
	 
		public EntityContext() { }
		public void copyFrom(EntityContext ctx) {
			super.copyFrom(ctx);
			this.qEntity = ctx.qEntity;
		}
	}
	public static class SimpleEntityContext extends EntityContext {
		public Token simpleEntityName;
		public Token simpleEntityAlias;
		public List<TerminalNode> NAME() { return getTokens(QueryBuilderQueriesParser.NAME); }
		public TerminalNode NAME(int i) {
			return getToken(QueryBuilderQueriesParser.NAME, i);
		}
		public SimpleEntityContext(EntityContext ctx) { copyFrom(ctx); }
	}
	public static class ComputedEntityContext extends EntityContext {
		public EntityContext computedEntityLeft;
		public Token computedEntityRelationshipName;
		public Token computedEntityRelationshipAlias;
		public EntityContext entity;
		public List<EntityContext> computedEntityRight = new ArrayList<EntityContext>();
		public List<EntityContext> entity() {
			return getRuleContexts(EntityContext.class);
		}
		public EntityContext entity(int i) {
			return getRuleContext(EntityContext.class,i);
		}
		public List<TerminalNode> NAME() { return getTokens(QueryBuilderQueriesParser.NAME); }
		public TerminalNode NAME(int i) {
			return getToken(QueryBuilderQueriesParser.NAME, i);
		}
		public ComputedEntityContext(EntityContext ctx) { copyFrom(ctx); }
	}
	public static class ParenthesisEntityContext extends EntityContext {
		public EntityContext entity() {
			return getRuleContext(EntityContext.class,0);
		}
		public ParenthesisEntityContext(EntityContext ctx) { copyFrom(ctx); }
	}

	public final EntityContext entity() throws RecognitionException {
		return entity(0);
	}

	private EntityContext entity(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		EntityContext _localctx = new EntityContext(_ctx, _parentState);
		EntityContext _prevctx = _localctx;
		int _startState = 2;
		enterRecursionRule(_localctx, 2, RULE_entity, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(40);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NAME:
				{
				_localctx = new SimpleEntityContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(34);
				((SimpleEntityContext)_localctx).simpleEntityName = match(NAME);
				setState(35);
				((SimpleEntityContext)_localctx).simpleEntityAlias = match(NAME);
				}
				break;
			case T__4:
				{
				_localctx = new ParenthesisEntityContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(36);
				match(T__4);
				setState(37);
				entity(0);
				setState(38);
				match(T__6);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(61);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new ComputedEntityContext(new EntityContext(_parentctx, _parentState));
					((ComputedEntityContext)_localctx).computedEntityLeft = _prevctx;
					pushNewRecursionContext(_localctx, _startState, RULE_entity);
					setState(42);
					if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
					setState(43);
					match(T__1);
					setState(44);
					match(T__2);
					setState(45);
					((ComputedEntityContext)_localctx).computedEntityRelationshipName = match(NAME);
					setState(46);
					((ComputedEntityContext)_localctx).computedEntityRelationshipAlias = match(NAME);
					setState(47);
					match(T__3);
					setState(48);
					match(T__4);
					setState(49);
					((ComputedEntityContext)_localctx).entity = entity(0);
					((ComputedEntityContext)_localctx).computedEntityRight.add(((ComputedEntityContext)_localctx).entity);
					setState(54);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==T__5) {
						{
						{
						setState(50);
						match(T__5);
						setState(51);
						((ComputedEntityContext)_localctx).entity = entity(0);
						((ComputedEntityContext)_localctx).computedEntityRight.add(((ComputedEntityContext)_localctx).entity);
						}
						}
						setState(56);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					setState(57);
					match(T__6);
					}
					} 
				}
				setState(63);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class SelectContext extends ParserRuleContext {
		public SimpleAttributeContext simpleAttribute() {
			return getRuleContext(SimpleAttributeContext.class,0);
		}
		public ListOfAttributesContext listOfAttributes() {
			return getRuleContext(ListOfAttributesContext.class,0);
		}
		public List<AggregationFunctionContext> aggregationFunction() {
			return getRuleContexts(AggregationFunctionContext.class);
		}
		public AggregationFunctionContext aggregationFunction(int i) {
			return getRuleContext(AggregationFunctionContext.class,i);
		}
		public SelectContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_select; }
	}

	public final SelectContext select() throws RecognitionException {
		SelectContext _localctx = new SelectContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_select);
		int _la;
		try {
			setState(82);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,5,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(64);
				simpleAttribute();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(65);
				listOfAttributes();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(66);
				aggregationFunction();
				setState(71);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__5) {
					{
					{
					setState(67);
					match(T__5);
					setState(68);
					aggregationFunction();
					}
					}
					setState(73);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(74);
				listOfAttributes();
				setState(79);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__5) {
					{
					{
					setState(75);
					match(T__5);
					setState(76);
					aggregationFunction();
					}
					}
					setState(81);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SimpleAttributeContext extends ParserRuleContext {
		public Token entityName;
		public Token attribute;
		public Token relationshipName;
		public List<TerminalNode> NAME() { return getTokens(QueryBuilderQueriesParser.NAME); }
		public TerminalNode NAME(int i) {
			return getToken(QueryBuilderQueriesParser.NAME, i);
		}
		public SimpleAttributeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simpleAttribute; }
	}

	public final SimpleAttributeContext simpleAttribute() throws RecognitionException {
		SimpleAttributeContext _localctx = new SimpleAttributeContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_simpleAttribute);
		try {
			setState(90);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(84);
				((SimpleAttributeContext)_localctx).entityName = match(NAME);
				setState(85);
				match(T__7);
				setState(86);
				((SimpleAttributeContext)_localctx).attribute = match(NAME);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(87);
				((SimpleAttributeContext)_localctx).relationshipName = match(NAME);
				setState(88);
				match(T__7);
				setState(89);
				((SimpleAttributeContext)_localctx).attribute = match(NAME);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ListOfAttributesContext extends ParserRuleContext {
		public SimpleAttributeContext simpleAttribute() {
			return getRuleContext(SimpleAttributeContext.class,0);
		}
		public ListOfAttributesContext listOfAttributes() {
			return getRuleContext(ListOfAttributesContext.class,0);
		}
		public ListOfAttributesContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_listOfAttributes; }
	}

	public final ListOfAttributesContext listOfAttributes() throws RecognitionException {
		ListOfAttributesContext _localctx = new ListOfAttributesContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_listOfAttributes);
		try {
			setState(97);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(92);
				simpleAttribute();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(93);
				simpleAttribute();
				setState(94);
				match(T__5);
				setState(95);
				listOfAttributes();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class AliasContext extends ParserRuleContext {
		public Token description;
		public TerminalNode NAME() { return getToken(QueryBuilderQueriesParser.NAME, 0); }
		public AliasContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_alias; }
	}

	public final AliasContext alias() throws RecognitionException {
		AliasContext _localctx = new AliasContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_alias);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(99);
			((AliasContext)_localctx).description = match(NAME);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class AggregationFunctionContext extends ParserRuleContext {
		public SimpleAttributeContext simpleAttribute() {
			return getRuleContext(SimpleAttributeContext.class,0);
		}
		public AliasContext alias() {
			return getRuleContext(AliasContext.class,0);
		}
		public AggregationFunctionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_aggregationFunction; }
	}

	public final AggregationFunctionContext aggregationFunction() throws RecognitionException {
		AggregationFunctionContext _localctx = new AggregationFunctionContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_aggregationFunction);
		int _la;
		try {
			setState(141);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,14,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(101);
				match(T__8);
				setState(102);
				match(T__4);
				setState(103);
				simpleAttribute();
				setState(104);
				match(T__6);
				setState(106);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAME) {
					{
					setState(105);
					alias();
					}
				}

				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(108);
				match(T__9);
				setState(109);
				match(T__4);
				setState(110);
				simpleAttribute();
				setState(111);
				match(T__6);
				setState(113);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAME) {
					{
					setState(112);
					alias();
					}
				}

				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(115);
				match(T__10);
				setState(116);
				match(T__4);
				setState(117);
				simpleAttribute();
				setState(118);
				match(T__6);
				setState(120);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAME) {
					{
					setState(119);
					alias();
					}
				}

				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(122);
				match(T__11);
				setState(123);
				match(T__4);
				setState(124);
				simpleAttribute();
				setState(125);
				match(T__6);
				setState(127);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAME) {
					{
					setState(126);
					alias();
					}
				}

				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(129);
				match(T__12);
				setState(130);
				match(T__4);
				setState(131);
				simpleAttribute();
				setState(132);
				match(T__6);
				setState(134);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAME) {
					{
					setState(133);
					alias();
					}
				}

				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(136);
				match(T__12);
				setState(137);
				match(T__13);
				setState(139);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAME) {
					{
					setState(138);
					alias();
					}
				}

				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class WhereContext extends ParserRuleContext {
		public ExpressionListContext expressionList() {
			return getRuleContext(ExpressionListContext.class,0);
		}
		public WhereContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_where; }
	}

	public final WhereContext where() throws RecognitionException {
		WhereContext _localctx = new WhereContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_where);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(143);
			expressionList();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionListContext extends ParserRuleContext {
		public SimpleAttributeContext simpleAttribute() {
			return getRuleContext(SimpleAttributeContext.class,0);
		}
		public ArithmeticExpressionContext arithmeticExpression() {
			return getRuleContext(ArithmeticExpressionContext.class,0);
		}
		public TerminalNode VALUE() { return getToken(QueryBuilderQueriesParser.VALUE, 0); }
		public TerminalNode NUMERIC() { return getToken(QueryBuilderQueriesParser.NUMERIC, 0); }
		public List<LogicalExpressionContext> logicalExpression() {
			return getRuleContexts(LogicalExpressionContext.class);
		}
		public LogicalExpressionContext logicalExpression(int i) {
			return getRuleContext(LogicalExpressionContext.class,i);
		}
		public List<ExpressionListContext> expressionList() {
			return getRuleContexts(ExpressionListContext.class);
		}
		public ExpressionListContext expressionList(int i) {
			return getRuleContext(ExpressionListContext.class,i);
		}
		public OtherExpressionContext otherExpression() {
			return getRuleContext(OtherExpressionContext.class,0);
		}
		public ExpressionListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expressionList; }
	}

	public final ExpressionListContext expressionList() throws RecognitionException {
		ExpressionListContext _localctx = new ExpressionListContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_expressionList);
		int _la;
		try {
			int _alt;
			setState(182);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NAME:
				enterOuterAlt(_localctx, 1);
				{
				setState(145);
				simpleAttribute();
				setState(146);
				arithmeticExpression();
				setState(148);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==VALUE) {
					{
					setState(147);
					match(VALUE);
					}
				}

				setState(151);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NUMERIC) {
					{
					setState(150);
					match(NUMERIC);
					}
				}

				setState(158);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(153);
						logicalExpression();
						setState(154);
						expressionList();
						}
						} 
					}
					setState(160);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
				}
				}
				break;
			case T__4:
				enterOuterAlt(_localctx, 2);
				{
				setState(161);
				match(T__4);
				setState(162);
				simpleAttribute();
				setState(163);
				otherExpression();
				setState(164);
				match(T__6);
				setState(170);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,18,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(165);
						logicalExpression();
						setState(166);
						expressionList();
						}
						} 
					}
					setState(172);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,18,_ctx);
				}
				}
				break;
			case T__21:
			case T__23:
			case T__24:
			case T__25:
			case T__26:
				enterOuterAlt(_localctx, 3);
				{
				setState(173);
				otherExpression();
				setState(179);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,19,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(174);
						logicalExpression();
						setState(175);
						expressionList();
						}
						} 
					}
					setState(181);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,19,_ctx);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArithmeticExpressionContext extends ParserRuleContext {
		public ArithmeticExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arithmeticExpression; }
	}

	public final ArithmeticExpressionContext arithmeticExpression() throws RecognitionException {
		ArithmeticExpressionContext _localctx = new ArithmeticExpressionContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_arithmeticExpression);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(184);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__2) | (1L << T__3) | (1L << T__14) | (1L << T__15) | (1L << T__16) | (1L << T__17) | (1L << T__18) | (1L << T__19) | (1L << T__20))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class OtherExpressionContext extends ParserRuleContext {
		public List<TerminalNode> NUMERIC() { return getTokens(QueryBuilderQueriesParser.NUMERIC); }
		public TerminalNode NUMERIC(int i) {
			return getToken(QueryBuilderQueriesParser.NUMERIC, i);
		}
		public QueryContext query() {
			return getRuleContext(QueryContext.class,0);
		}
		public List<TerminalNode> VALUE() { return getTokens(QueryBuilderQueriesParser.VALUE); }
		public TerminalNode VALUE(int i) {
			return getToken(QueryBuilderQueriesParser.VALUE, i);
		}
		public OtherExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_otherExpression; }
	}

	public final OtherExpressionContext otherExpression() throws RecognitionException {
		OtherExpressionContext _localctx = new OtherExpressionContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_otherExpression);
		int _la;
		try {
			setState(254);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,25,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(186);
				match(T__21);
				setState(187);
				match(NUMERIC);
				setState(188);
				match(T__22);
				setState(189);
				match(NUMERIC);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(190);
				match(T__23);
				setState(191);
				match(T__4);
				setState(192);
				query();
				setState(193);
				match(T__6);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(195);
				match(T__23);
				setState(196);
				match(T__4);
				setState(197);
				match(NUMERIC);
				setState(202);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__5) {
					{
					{
					setState(198);
					match(T__5);
					setState(199);
					match(NUMERIC);
					}
					}
					setState(204);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(205);
				match(T__6);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(206);
				match(T__23);
				setState(207);
				match(T__4);
				setState(208);
				match(VALUE);
				setState(213);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__5) {
					{
					{
					setState(209);
					match(T__5);
					setState(210);
					match(VALUE);
					}
					}
					setState(215);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(216);
				match(T__6);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(217);
				match(T__24);
				setState(218);
				match(T__4);
				setState(219);
				query();
				setState(220);
				match(T__6);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(222);
				match(T__24);
				setState(223);
				match(T__4);
				setState(224);
				match(NUMERIC);
				setState(229);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__5) {
					{
					{
					setState(225);
					match(T__5);
					setState(226);
					match(NUMERIC);
					}
					}
					setState(231);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(232);
				match(T__6);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(233);
				match(T__24);
				setState(234);
				match(T__4);
				setState(235);
				match(VALUE);
				setState(240);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__5) {
					{
					{
					setState(236);
					match(T__5);
					setState(237);
					match(VALUE);
					}
					}
					setState(242);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(243);
				match(T__6);
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(244);
				match(T__25);
				setState(245);
				match(T__4);
				setState(246);
				query();
				setState(247);
				match(T__6);
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(249);
				match(T__26);
				setState(250);
				match(T__4);
				setState(251);
				query();
				setState(252);
				match(T__6);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LogicalExpressionContext extends ParserRuleContext {
		public LogicalExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logicalExpression; }
	}

	public final LogicalExpressionContext logicalExpression() throws RecognitionException {
		LogicalExpressionContext _localctx = new LogicalExpressionContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_logicalExpression);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(256);
			_la = _input.LA(1);
			if ( !(_la==T__22 || _la==T__27) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class GroupbyContext extends ParserRuleContext {
		public ListOfAttributesContext listOfAttributes() {
			return getRuleContext(ListOfAttributesContext.class,0);
		}
		public GroupbyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_groupby; }
	}

	public final GroupbyContext groupby() throws RecognitionException {
		GroupbyContext _localctx = new GroupbyContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_groupby);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(258);
			listOfAttributes();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class HavingContext extends ParserRuleContext {
		public AggregationFunctionContext aggregationFunction() {
			return getRuleContext(AggregationFunctionContext.class,0);
		}
		public ArithmeticExpressionContext arithmeticExpression() {
			return getRuleContext(ArithmeticExpressionContext.class,0);
		}
		public TerminalNode NUMERIC() { return getToken(QueryBuilderQueriesParser.NUMERIC, 0); }
		public ExpressionListContext expressionList() {
			return getRuleContext(ExpressionListContext.class,0);
		}
		public HavingContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_having; }
	}

	public final HavingContext having() throws RecognitionException {
		HavingContext _localctx = new HavingContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_having);
		try {
			setState(265);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case T__8:
			case T__9:
			case T__10:
			case T__11:
			case T__12:
				enterOuterAlt(_localctx, 1);
				{
				setState(260);
				aggregationFunction();
				setState(261);
				arithmeticExpression();
				setState(262);
				match(NUMERIC);
				}
				break;
			case T__4:
			case T__21:
			case T__23:
			case T__24:
			case T__25:
			case T__26:
			case NAME:
				enterOuterAlt(_localctx, 2);
				{
				setState(264);
				expressionList();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class OrderbyContext extends ParserRuleContext {
		public ListOfAttributesContext listOfAttributes() {
			return getRuleContext(ListOfAttributesContext.class,0);
		}
		public List<OrderbyContext> orderby() {
			return getRuleContexts(OrderbyContext.class);
		}
		public OrderbyContext orderby(int i) {
			return getRuleContext(OrderbyContext.class,i);
		}
		public OrderbyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_orderby; }
	}

	public final OrderbyContext orderby() throws RecognitionException {
		OrderbyContext _localctx = new OrderbyContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_orderby);
		try {
			int _alt;
			setState(285);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,29,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(267);
				listOfAttributes();
				setState(268);
				match(T__28);
				setState(273);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,27,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(269);
						match(T__5);
						setState(270);
						orderby();
						}
						} 
					}
					setState(275);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,27,_ctx);
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(276);
				listOfAttributes();
				setState(277);
				match(T__29);
				setState(282);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,28,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(278);
						match(T__5);
						setState(279);
						orderby();
						}
						} 
					}
					setState(284);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,28,_ctx);
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 1:
			return entity_sempred((EntityContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean entity_sempred(EntityContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 2);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3%\u0122\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\3\2\3\2\3\2\3\3\3\3"+
		"\3\3\3\3\3\3\3\3\3\3\5\3+\n\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3"+
		"\7\3\67\n\3\f\3\16\3:\13\3\3\3\3\3\7\3>\n\3\f\3\16\3A\13\3\3\4\3\4\3\4"+
		"\3\4\3\4\7\4H\n\4\f\4\16\4K\13\4\3\4\3\4\3\4\7\4P\n\4\f\4\16\4S\13\4\5"+
		"\4U\n\4\3\5\3\5\3\5\3\5\3\5\3\5\5\5]\n\5\3\6\3\6\3\6\3\6\3\6\5\6d\n\6"+
		"\3\7\3\7\3\b\3\b\3\b\3\b\3\b\5\bm\n\b\3\b\3\b\3\b\3\b\3\b\5\bt\n\b\3\b"+
		"\3\b\3\b\3\b\3\b\5\b{\n\b\3\b\3\b\3\b\3\b\3\b\5\b\u0082\n\b\3\b\3\b\3"+
		"\b\3\b\3\b\5\b\u0089\n\b\3\b\3\b\3\b\5\b\u008e\n\b\5\b\u0090\n\b\3\t\3"+
		"\t\3\n\3\n\3\n\5\n\u0097\n\n\3\n\5\n\u009a\n\n\3\n\3\n\3\n\7\n\u009f\n"+
		"\n\f\n\16\n\u00a2\13\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\7\n\u00ab\n\n\f\n\16"+
		"\n\u00ae\13\n\3\n\3\n\3\n\3\n\7\n\u00b4\n\n\f\n\16\n\u00b7\13\n\5\n\u00b9"+
		"\n\n\3\13\3\13\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f"+
		"\7\f\u00cb\n\f\f\f\16\f\u00ce\13\f\3\f\3\f\3\f\3\f\3\f\3\f\7\f\u00d6\n"+
		"\f\f\f\16\f\u00d9\13\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\7\f"+
		"\u00e6\n\f\f\f\16\f\u00e9\13\f\3\f\3\f\3\f\3\f\3\f\3\f\7\f\u00f1\n\f\f"+
		"\f\16\f\u00f4\13\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\5\f\u0101"+
		"\n\f\3\r\3\r\3\16\3\16\3\17\3\17\3\17\3\17\3\17\5\17\u010c\n\17\3\20\3"+
		"\20\3\20\3\20\7\20\u0112\n\20\f\20\16\20\u0115\13\20\3\20\3\20\3\20\3"+
		"\20\7\20\u011b\n\20\f\20\16\20\u011e\13\20\5\20\u0120\n\20\3\20\2\3\4"+
		"\21\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36\2\4\4\2\5\6\21\27\4\2\31\31"+
		"\36\36\2\u013e\2 \3\2\2\2\4*\3\2\2\2\6T\3\2\2\2\b\\\3\2\2\2\nc\3\2\2\2"+
		"\fe\3\2\2\2\16\u008f\3\2\2\2\20\u0091\3\2\2\2\22\u00b8\3\2\2\2\24\u00ba"+
		"\3\2\2\2\26\u0100\3\2\2\2\30\u0102\3\2\2\2\32\u0104\3\2\2\2\34\u010b\3"+
		"\2\2\2\36\u011f\3\2\2\2 !\7\3\2\2!\"\5\4\3\2\"\3\3\2\2\2#$\b\3\1\2$%\7"+
		"!\2\2%+\7!\2\2&\'\7\7\2\2\'(\5\4\3\2()\7\t\2\2)+\3\2\2\2*#\3\2\2\2*&\3"+
		"\2\2\2+?\3\2\2\2,-\f\4\2\2-.\7\4\2\2./\7\5\2\2/\60\7!\2\2\60\61\7!\2\2"+
		"\61\62\7\6\2\2\62\63\7\7\2\2\638\5\4\3\2\64\65\7\b\2\2\65\67\5\4\3\2\66"+
		"\64\3\2\2\2\67:\3\2\2\28\66\3\2\2\289\3\2\2\29;\3\2\2\2:8\3\2\2\2;<\7"+
		"\t\2\2<>\3\2\2\2=,\3\2\2\2>A\3\2\2\2?=\3\2\2\2?@\3\2\2\2@\5\3\2\2\2A?"+
		"\3\2\2\2BU\5\b\5\2CU\5\n\6\2DI\5\16\b\2EF\7\b\2\2FH\5\16\b\2GE\3\2\2\2"+
		"HK\3\2\2\2IG\3\2\2\2IJ\3\2\2\2JU\3\2\2\2KI\3\2\2\2LQ\5\n\6\2MN\7\b\2\2"+
		"NP\5\16\b\2OM\3\2\2\2PS\3\2\2\2QO\3\2\2\2QR\3\2\2\2RU\3\2\2\2SQ\3\2\2"+
		"\2TB\3\2\2\2TC\3\2\2\2TD\3\2\2\2TL\3\2\2\2U\7\3\2\2\2VW\7!\2\2WX\7\n\2"+
		"\2X]\7!\2\2YZ\7!\2\2Z[\7\n\2\2[]\7!\2\2\\V\3\2\2\2\\Y\3\2\2\2]\t\3\2\2"+
		"\2^d\5\b\5\2_`\5\b\5\2`a\7\b\2\2ab\5\n\6\2bd\3\2\2\2c^\3\2\2\2c_\3\2\2"+
		"\2d\13\3\2\2\2ef\7!\2\2f\r\3\2\2\2gh\7\13\2\2hi\7\7\2\2ij\5\b\5\2jl\7"+
		"\t\2\2km\5\f\7\2lk\3\2\2\2lm\3\2\2\2m\u0090\3\2\2\2no\7\f\2\2op\7\7\2"+
		"\2pq\5\b\5\2qs\7\t\2\2rt\5\f\7\2sr\3\2\2\2st\3\2\2\2t\u0090\3\2\2\2uv"+
		"\7\r\2\2vw\7\7\2\2wx\5\b\5\2xz\7\t\2\2y{\5\f\7\2zy\3\2\2\2z{\3\2\2\2{"+
		"\u0090\3\2\2\2|}\7\16\2\2}~\7\7\2\2~\177\5\b\5\2\177\u0081\7\t\2\2\u0080"+
		"\u0082\5\f\7\2\u0081\u0080\3\2\2\2\u0081\u0082\3\2\2\2\u0082\u0090\3\2"+
		"\2\2\u0083\u0084\7\17\2\2\u0084\u0085\7\7\2\2\u0085\u0086\5\b\5\2\u0086"+
		"\u0088\7\t\2\2\u0087\u0089\5\f\7\2\u0088\u0087\3\2\2\2\u0088\u0089\3\2"+
		"\2\2\u0089\u0090\3\2\2\2\u008a\u008b\7\17\2\2\u008b\u008d\7\20\2\2\u008c"+
		"\u008e\5\f\7\2\u008d\u008c\3\2\2\2\u008d\u008e\3\2\2\2\u008e\u0090\3\2"+
		"\2\2\u008fg\3\2\2\2\u008fn\3\2\2\2\u008fu\3\2\2\2\u008f|\3\2\2\2\u008f"+
		"\u0083\3\2\2\2\u008f\u008a\3\2\2\2\u0090\17\3\2\2\2\u0091\u0092\5\22\n"+
		"\2\u0092\21\3\2\2\2\u0093\u0094\5\b\5\2\u0094\u0096\5\24\13\2\u0095\u0097"+
		"\7#\2\2\u0096\u0095\3\2\2\2\u0096\u0097\3\2\2\2\u0097\u0099\3\2\2\2\u0098"+
		"\u009a\7$\2\2\u0099\u0098\3\2\2\2\u0099\u009a\3\2\2\2\u009a\u00a0\3\2"+
		"\2\2\u009b\u009c\5\30\r\2\u009c\u009d\5\22\n\2\u009d\u009f\3\2\2\2\u009e"+
		"\u009b\3\2\2\2\u009f\u00a2\3\2\2\2\u00a0\u009e\3\2\2\2\u00a0\u00a1\3\2"+
		"\2\2\u00a1\u00b9\3\2\2\2\u00a2\u00a0\3\2\2\2\u00a3\u00a4\7\7\2\2\u00a4"+
		"\u00a5\5\b\5\2\u00a5\u00a6\5\26\f\2\u00a6\u00ac\7\t\2\2\u00a7\u00a8\5"+
		"\30\r\2\u00a8\u00a9\5\22\n\2\u00a9\u00ab\3\2\2\2\u00aa\u00a7\3\2\2\2\u00ab"+
		"\u00ae\3\2\2\2\u00ac\u00aa\3\2\2\2\u00ac\u00ad\3\2\2\2\u00ad\u00b9\3\2"+
		"\2\2\u00ae\u00ac\3\2\2\2\u00af\u00b5\5\26\f\2\u00b0\u00b1\5\30\r\2\u00b1"+
		"\u00b2\5\22\n\2\u00b2\u00b4\3\2\2\2\u00b3\u00b0\3\2\2\2\u00b4\u00b7\3"+
		"\2\2\2\u00b5\u00b3\3\2\2\2\u00b5\u00b6\3\2\2\2\u00b6\u00b9\3\2\2\2\u00b7"+
		"\u00b5\3\2\2\2\u00b8\u0093\3\2\2\2\u00b8\u00a3\3\2\2\2\u00b8\u00af\3\2"+
		"\2\2\u00b9\23\3\2\2\2\u00ba\u00bb\t\2\2\2\u00bb\25\3\2\2\2\u00bc\u00bd"+
		"\7\30\2\2\u00bd\u00be\7$\2\2\u00be\u00bf\7\31\2\2\u00bf\u0101\7$\2\2\u00c0"+
		"\u00c1\7\32\2\2\u00c1\u00c2\7\7\2\2\u00c2\u00c3\5\2\2\2\u00c3\u00c4\7"+
		"\t\2\2\u00c4\u0101\3\2\2\2\u00c5\u00c6\7\32\2\2\u00c6\u00c7\7\7\2\2\u00c7"+
		"\u00cc\7$\2\2\u00c8\u00c9\7\b\2\2\u00c9\u00cb\7$\2\2\u00ca\u00c8\3\2\2"+
		"\2\u00cb\u00ce\3\2\2\2\u00cc\u00ca\3\2\2\2\u00cc\u00cd\3\2\2\2\u00cd\u00cf"+
		"\3\2\2\2\u00ce\u00cc\3\2\2\2\u00cf\u0101\7\t\2\2\u00d0\u00d1\7\32\2\2"+
		"\u00d1\u00d2\7\7\2\2\u00d2\u00d7\7#\2\2\u00d3\u00d4\7\b\2\2\u00d4\u00d6"+
		"\7#\2\2\u00d5\u00d3\3\2\2\2\u00d6\u00d9\3\2\2\2\u00d7\u00d5\3\2\2\2\u00d7"+
		"\u00d8\3\2\2\2\u00d8\u00da\3\2\2\2\u00d9\u00d7\3\2\2\2\u00da\u0101\7\t"+
		"\2\2\u00db\u00dc\7\33\2\2\u00dc\u00dd\7\7\2\2\u00dd\u00de\5\2\2\2\u00de"+
		"\u00df\7\t\2\2\u00df\u0101\3\2\2\2\u00e0\u00e1\7\33\2\2\u00e1\u00e2\7"+
		"\7\2\2\u00e2\u00e7\7$\2\2\u00e3\u00e4\7\b\2\2\u00e4\u00e6\7$\2\2\u00e5"+
		"\u00e3\3\2\2\2\u00e6\u00e9\3\2\2\2\u00e7\u00e5\3\2\2\2\u00e7\u00e8\3\2"+
		"\2\2\u00e8\u00ea\3\2\2\2\u00e9\u00e7\3\2\2\2\u00ea\u0101\7\t\2\2\u00eb"+
		"\u00ec\7\33\2\2\u00ec\u00ed\7\7\2\2\u00ed\u00f2\7#\2\2\u00ee\u00ef\7\b"+
		"\2\2\u00ef\u00f1\7#\2\2\u00f0\u00ee\3\2\2\2\u00f1\u00f4\3\2\2\2\u00f2"+
		"\u00f0\3\2\2\2\u00f2\u00f3\3\2\2\2\u00f3\u00f5\3\2\2\2\u00f4\u00f2\3\2"+
		"\2\2\u00f5\u0101\7\t\2\2\u00f6\u00f7\7\34\2\2\u00f7\u00f8\7\7\2\2\u00f8"+
		"\u00f9\5\2\2\2\u00f9\u00fa\7\t\2\2\u00fa\u0101\3\2\2\2\u00fb\u00fc\7\35"+
		"\2\2\u00fc\u00fd\7\7\2\2\u00fd\u00fe\5\2\2\2\u00fe\u00ff\7\t\2\2\u00ff"+
		"\u0101\3\2\2\2\u0100\u00bc\3\2\2\2\u0100\u00c0\3\2\2\2\u0100\u00c5\3\2"+
		"\2\2\u0100\u00d0\3\2\2\2\u0100\u00db\3\2\2\2\u0100\u00e0\3\2\2\2\u0100"+
		"\u00eb\3\2\2\2\u0100\u00f6\3\2\2\2\u0100\u00fb\3\2\2\2\u0101\27\3\2\2"+
		"\2\u0102\u0103\t\3\2\2\u0103\31\3\2\2\2\u0104\u0105\5\n\6\2\u0105\33\3"+
		"\2\2\2\u0106\u0107\5\16\b\2\u0107\u0108\5\24\13\2\u0108\u0109\7$\2\2\u0109"+
		"\u010c\3\2\2\2\u010a\u010c\5\22\n\2\u010b\u0106\3\2\2\2\u010b\u010a\3"+
		"\2\2\2\u010c\35\3\2\2\2\u010d\u010e\5\n\6\2\u010e\u0113\7\37\2\2\u010f"+
		"\u0110\7\b\2\2\u0110\u0112\5\36\20\2\u0111\u010f\3\2\2\2\u0112\u0115\3"+
		"\2\2\2\u0113\u0111\3\2\2\2\u0113\u0114\3\2\2\2\u0114\u0120\3\2\2\2\u0115"+
		"\u0113\3\2\2\2\u0116\u0117\5\n\6\2\u0117\u011c\7 \2\2\u0118\u0119\7\b"+
		"\2\2\u0119\u011b\5\36\20\2\u011a\u0118\3\2\2\2\u011b\u011e\3\2\2\2\u011c"+
		"\u011a\3\2\2\2\u011c\u011d\3\2\2\2\u011d\u0120\3\2\2\2\u011e\u011c\3\2"+
		"\2\2\u011f\u010d\3\2\2\2\u011f\u0116\3\2\2\2\u0120\37\3\2\2\2 *8?IQT\\"+
		"clsz\u0081\u0088\u008d\u008f\u0096\u0099\u00a0\u00ac\u00b5\u00b8\u00cc"+
		"\u00d7\u00e7\u00f2\u0100\u010b\u0113\u011c\u011f";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}