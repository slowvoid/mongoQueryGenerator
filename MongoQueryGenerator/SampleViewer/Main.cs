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
            GenerateQuery( Container1, "progradweb_1" );
        }

        private void Map2Btn_Click( object sender, EventArgs e )
        {
            queryBox.Clear();
            resultBox.Clear();

            RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

            GenerateQuery( Container2, "progradweb_2" );
        }

        private void GenerateQuery( RequiredDataContainer Container, string Database )
        {
            QueryableEntity Aluno = new QueryableEntity( Container.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container.EntityRelationshipModel.FindByName( "Endereco" ) );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Aluno, Aluno.Element.GetIdentifierAttribute(), MongoDBSort.Ascending );
            SortStage SortOp = new SortStage( new List<SortArgument>() { SortArg }, Container.ERMongoMapping );

            List<AlgebraOperator> OperatorList = new List<AlgebraOperator>() { RJoinOp, SortOp };

            FromArgument FromArg = new FromArgument( Aluno, Container.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( FromArg, OperatorList );

            string Query = QueryGen.Run();

            Beautifier jsB = new Beautifier();
            queryBox.Text = jsB.Beautify( Query );

            QueryRunner QueryRunner = new QueryRunner( "mongodb://localhost:27017", Database );

            string Result = QueryRunner.GetJSON( Query );

            resultBox.Text = Result.PrettyPrintJson();
        }

        
    }
}
