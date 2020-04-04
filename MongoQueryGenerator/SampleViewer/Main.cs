using Jsbeautifier;
using JsonPrettyPrinterPlus;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleViewer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Map1Btn_Click( object sender, EventArgs e )
        {
            queryBox.Clear();
            resultBox.Clear();

            RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container1.ERMongoMapping );

            SortArgument SortArg1 = new SortArgument( Aluno, Aluno.Element.GetIdentifierAttribute(), MongoDBSort.Ascending );
            SortStage SortOp1 = new SortStage( new List<SortArgument>() { SortArg1 }, Container1.ERMongoMapping );

            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1, SortOp1 };

            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );

            string Query1 = QueryGen1.Run();

            Beautifier jsB = new Beautifier();
            queryBox.Text = jsB.Beautify( Query1 ); 

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );

            string QueryResult1 = QueryRunner1.GetJSON( Query1 );

            resultBox.Text = QueryResult1.PrettyPrintJson();
        }

        private void Map2Btn_Click( object sender, EventArgs e )
        {
            queryBox.Clear();
            resultBox.Clear();

            RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container2.ERMongoMapping );

            SortArgument SortArg2 = new SortArgument( Aluno, Aluno.Element.GetIdentifierAttribute(), MongoDBSort.Ascending );
            SortStage SortOp2 = new SortStage( new List<SortArgument>() { SortArg2 }, Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2, SortOp2 };

            FromArgument FromArg2 = new FromArgument( Aluno, Container2.ERMongoMapping );

            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );

            string Query2 = QueryGen2.Run();

            Beautifier jsB = new Beautifier();
            queryBox.Text = jsB.Beautify( Query2 );

            QueryRunner QueryRunner2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_2" );

            string QueryResult2 = QueryRunner2.GetJSON( Query2 );
            resultBox.Text = QueryResult2.PrettyPrintJson();
        }
    }
}
