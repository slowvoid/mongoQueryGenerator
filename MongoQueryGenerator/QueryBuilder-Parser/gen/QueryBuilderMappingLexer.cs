//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /Users/daniellucredio/GitProjects/mongoQueryGenerator/MongoQueryGenerator/QueryBuilder-Parser/QueryBuilderMapping.g4 by ANTLR 4.8

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
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class QueryBuilderMappingLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, CARDINALITY_ITEM=19, ID=20, STRING=21, DIVIDER=22, WS=23;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "CARDINALITY_ITEM", "ID", "STRING", "DIVIDER", "WS"
	};


	public QueryBuilderMappingLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public QueryBuilderMappingLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'Solution'", "':'", "'Description'", "'Version'", "'ERModel'", 
		"'MongoDBSchema'", "'{'", "'}'", "'('", "','", "')'", "'>'", "'[]'", "'<'", 
		"'*'", "'['", "']'", "'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, "CARDINALITY_ITEM", "ID", "STRING", 
		"DIVIDER", "WS"
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

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static QueryBuilderMappingLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '\x19', '\x9C', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x3', 
		'\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', 
		'\x5', '\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\t', 
		'\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\v', '\x3', '\v', '\x3', 
		'\f', '\x3', '\f', '\x3', '\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', '\x3', '\x10', 
		'\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x3', '\x12', '\x3', '\x13', 
		'\x3', '\x13', '\x3', '\x14', '\x3', '\x14', '\x3', '\x15', '\x3', '\x15', 
		'\x6', '\x15', '\x84', '\n', '\x15', '\r', '\x15', '\xE', '\x15', '\x85', 
		'\x3', '\x16', '\x3', '\x16', '\a', '\x16', '\x8A', '\n', '\x16', '\f', 
		'\x16', '\xE', '\x16', '\x8D', '\v', '\x16', '\x3', '\x16', '\x3', '\x16', 
		'\x3', '\x17', '\x6', '\x17', '\x92', '\n', '\x17', '\r', '\x17', '\xE', 
		'\x17', '\x93', '\x3', '\x18', '\x6', '\x18', '\x97', '\n', '\x18', '\r', 
		'\x18', '\xE', '\x18', '\x98', '\x3', '\x18', '\x3', '\x18', '\x2', '\x2', 
		'\x19', '\x3', '\x3', '\x5', '\x4', '\a', '\x5', '\t', '\x6', '\v', '\a', 
		'\r', '\b', '\xF', '\t', '\x11', '\n', '\x13', '\v', '\x15', '\f', '\x17', 
		'\r', '\x19', '\xE', '\x1B', '\xF', '\x1D', '\x10', '\x1F', '\x11', '!', 
		'\x12', '#', '\x13', '%', '\x14', '\'', '\x15', ')', '\x16', '+', '\x17', 
		'-', '\x18', '/', '\x19', '\x3', '\x2', '\a', '\x4', '\x2', '\x33', '\x33', 
		'P', 'P', '\x5', '\x2', '\x43', '\\', '\x61', '\x61', '\x63', '|', '\x6', 
		'\x2', '\x32', ';', '\x43', '\\', '\x61', '\x61', '\x63', '|', '\x5', 
		'\x2', '\f', '\f', '\xF', '\xF', '$', '$', '\x5', '\x2', '\v', '\f', '\xF', 
		'\xF', '\"', '\"', '\x2', '\x9F', '\x2', '\x3', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x5', '\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x13', '\x3', '\x2', '\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x17', '\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x1D', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', 
		'\x2', '\x2', '\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'/', '\x3', '\x2', '\x2', '\x2', '\x3', '\x31', '\x3', '\x2', '\x2', '\x2', 
		'\x5', ':', '\x3', '\x2', '\x2', '\x2', '\a', '<', '\x3', '\x2', '\x2', 
		'\x2', '\t', 'H', '\x3', '\x2', '\x2', '\x2', '\v', 'P', '\x3', '\x2', 
		'\x2', '\x2', '\r', 'X', '\x3', '\x2', '\x2', '\x2', '\xF', '\x66', '\x3', 
		'\x2', '\x2', '\x2', '\x11', 'h', '\x3', '\x2', '\x2', '\x2', '\x13', 
		'j', '\x3', '\x2', '\x2', '\x2', '\x15', 'l', '\x3', '\x2', '\x2', '\x2', 
		'\x17', 'n', '\x3', '\x2', '\x2', '\x2', '\x19', 'p', '\x3', '\x2', '\x2', 
		'\x2', '\x1B', 'r', '\x3', '\x2', '\x2', '\x2', '\x1D', 'u', '\x3', '\x2', 
		'\x2', '\x2', '\x1F', 'w', '\x3', '\x2', '\x2', '\x2', '!', 'y', '\x3', 
		'\x2', '\x2', '\x2', '#', '{', '\x3', '\x2', '\x2', '\x2', '%', '}', '\x3', 
		'\x2', '\x2', '\x2', '\'', '\x7F', '\x3', '\x2', '\x2', '\x2', ')', '\x81', 
		'\x3', '\x2', '\x2', '\x2', '+', '\x87', '\x3', '\x2', '\x2', '\x2', '-', 
		'\x91', '\x3', '\x2', '\x2', '\x2', '/', '\x96', '\x3', '\x2', '\x2', 
		'\x2', '\x31', '\x32', '\a', 'U', '\x2', '\x2', '\x32', '\x33', '\a', 
		'q', '\x2', '\x2', '\x33', '\x34', '\a', 'n', '\x2', '\x2', '\x34', '\x35', 
		'\a', 'w', '\x2', '\x2', '\x35', '\x36', '\a', 'v', '\x2', '\x2', '\x36', 
		'\x37', '\a', 'k', '\x2', '\x2', '\x37', '\x38', '\a', 'q', '\x2', '\x2', 
		'\x38', '\x39', '\a', 'p', '\x2', '\x2', '\x39', '\x4', '\x3', '\x2', 
		'\x2', '\x2', ':', ';', '\a', '<', '\x2', '\x2', ';', '\x6', '\x3', '\x2', 
		'\x2', '\x2', '<', '=', '\a', '\x46', '\x2', '\x2', '=', '>', '\a', 'g', 
		'\x2', '\x2', '>', '?', '\a', 'u', '\x2', '\x2', '?', '@', '\a', '\x65', 
		'\x2', '\x2', '@', '\x41', '\a', 't', '\x2', '\x2', '\x41', '\x42', '\a', 
		'k', '\x2', '\x2', '\x42', '\x43', '\a', 'r', '\x2', '\x2', '\x43', '\x44', 
		'\a', 'v', '\x2', '\x2', '\x44', '\x45', '\a', 'k', '\x2', '\x2', '\x45', 
		'\x46', '\a', 'q', '\x2', '\x2', '\x46', 'G', '\a', 'p', '\x2', '\x2', 
		'G', '\b', '\x3', '\x2', '\x2', '\x2', 'H', 'I', '\a', 'X', '\x2', '\x2', 
		'I', 'J', '\a', 'g', '\x2', '\x2', 'J', 'K', '\a', 't', '\x2', '\x2', 
		'K', 'L', '\a', 'u', '\x2', '\x2', 'L', 'M', '\a', 'k', '\x2', '\x2', 
		'M', 'N', '\a', 'q', '\x2', '\x2', 'N', 'O', '\a', 'p', '\x2', '\x2', 
		'O', '\n', '\x3', '\x2', '\x2', '\x2', 'P', 'Q', '\a', 'G', '\x2', '\x2', 
		'Q', 'R', '\a', 'T', '\x2', '\x2', 'R', 'S', '\a', 'O', '\x2', '\x2', 
		'S', 'T', '\a', 'q', '\x2', '\x2', 'T', 'U', '\a', '\x66', '\x2', '\x2', 
		'U', 'V', '\a', 'g', '\x2', '\x2', 'V', 'W', '\a', 'n', '\x2', '\x2', 
		'W', '\f', '\x3', '\x2', '\x2', '\x2', 'X', 'Y', '\a', 'O', '\x2', '\x2', 
		'Y', 'Z', '\a', 'q', '\x2', '\x2', 'Z', '[', '\a', 'p', '\x2', '\x2', 
		'[', '\\', '\a', 'i', '\x2', '\x2', '\\', ']', '\a', 'q', '\x2', '\x2', 
		']', '^', '\a', '\x46', '\x2', '\x2', '^', '_', '\a', '\x44', '\x2', '\x2', 
		'_', '`', '\a', 'U', '\x2', '\x2', '`', '\x61', '\a', '\x65', '\x2', '\x2', 
		'\x61', '\x62', '\a', 'j', '\x2', '\x2', '\x62', '\x63', '\a', 'g', '\x2', 
		'\x2', '\x63', '\x64', '\a', 'o', '\x2', '\x2', '\x64', '\x65', '\a', 
		'\x63', '\x2', '\x2', '\x65', '\xE', '\x3', '\x2', '\x2', '\x2', '\x66', 
		'g', '\a', '}', '\x2', '\x2', 'g', '\x10', '\x3', '\x2', '\x2', '\x2', 
		'h', 'i', '\a', '\x7F', '\x2', '\x2', 'i', '\x12', '\x3', '\x2', '\x2', 
		'\x2', 'j', 'k', '\a', '*', '\x2', '\x2', 'k', '\x14', '\x3', '\x2', '\x2', 
		'\x2', 'l', 'm', '\a', '.', '\x2', '\x2', 'm', '\x16', '\x3', '\x2', '\x2', 
		'\x2', 'n', 'o', '\a', '+', '\x2', '\x2', 'o', '\x18', '\x3', '\x2', '\x2', 
		'\x2', 'p', 'q', '\a', '@', '\x2', '\x2', 'q', '\x1A', '\x3', '\x2', '\x2', 
		'\x2', 'r', 's', '\a', ']', '\x2', '\x2', 's', 't', '\a', '_', '\x2', 
		'\x2', 't', '\x1C', '\x3', '\x2', '\x2', '\x2', 'u', 'v', '\a', '>', '\x2', 
		'\x2', 'v', '\x1E', '\x3', '\x2', '\x2', '\x2', 'w', 'x', '\a', ',', '\x2', 
		'\x2', 'x', ' ', '\x3', '\x2', '\x2', '\x2', 'y', 'z', '\a', ']', '\x2', 
		'\x2', 'z', '\"', '\x3', '\x2', '\x2', '\x2', '{', '|', '\a', '_', '\x2', 
		'\x2', '|', '$', '\x3', '\x2', '\x2', '\x2', '}', '~', '\a', '\x30', '\x2', 
		'\x2', '~', '&', '\x3', '\x2', '\x2', '\x2', '\x7F', '\x80', '\t', '\x2', 
		'\x2', '\x2', '\x80', '(', '\x3', '\x2', '\x2', '\x2', '\x81', '\x83', 
		'\t', '\x3', '\x2', '\x2', '\x82', '\x84', '\t', '\x4', '\x2', '\x2', 
		'\x83', '\x82', '\x3', '\x2', '\x2', '\x2', '\x84', '\x85', '\x3', '\x2', 
		'\x2', '\x2', '\x85', '\x83', '\x3', '\x2', '\x2', '\x2', '\x85', '\x86', 
		'\x3', '\x2', '\x2', '\x2', '\x86', '*', '\x3', '\x2', '\x2', '\x2', '\x87', 
		'\x8B', '\a', '$', '\x2', '\x2', '\x88', '\x8A', '\n', '\x5', '\x2', '\x2', 
		'\x89', '\x88', '\x3', '\x2', '\x2', '\x2', '\x8A', '\x8D', '\x3', '\x2', 
		'\x2', '\x2', '\x8B', '\x89', '\x3', '\x2', '\x2', '\x2', '\x8B', '\x8C', 
		'\x3', '\x2', '\x2', '\x2', '\x8C', '\x8E', '\x3', '\x2', '\x2', '\x2', 
		'\x8D', '\x8B', '\x3', '\x2', '\x2', '\x2', '\x8E', '\x8F', '\a', '$', 
		'\x2', '\x2', '\x8F', ',', '\x3', '\x2', '\x2', '\x2', '\x90', '\x92', 
		'\a', '%', '\x2', '\x2', '\x91', '\x90', '\x3', '\x2', '\x2', '\x2', '\x92', 
		'\x93', '\x3', '\x2', '\x2', '\x2', '\x93', '\x91', '\x3', '\x2', '\x2', 
		'\x2', '\x93', '\x94', '\x3', '\x2', '\x2', '\x2', '\x94', '.', '\x3', 
		'\x2', '\x2', '\x2', '\x95', '\x97', '\t', '\x6', '\x2', '\x2', '\x96', 
		'\x95', '\x3', '\x2', '\x2', '\x2', '\x97', '\x98', '\x3', '\x2', '\x2', 
		'\x2', '\x98', '\x96', '\x3', '\x2', '\x2', '\x2', '\x98', '\x99', '\x3', 
		'\x2', '\x2', '\x2', '\x99', '\x9A', '\x3', '\x2', '\x2', '\x2', '\x9A', 
		'\x9B', '\b', '\x18', '\x2', '\x2', '\x9B', '\x30', '\x3', '\x2', '\x2', 
		'\x2', '\a', '\x2', '\x85', '\x8B', '\x93', '\x98', '\x3', '\b', '\x2', 
		'\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace QueryBuilder.Parser
