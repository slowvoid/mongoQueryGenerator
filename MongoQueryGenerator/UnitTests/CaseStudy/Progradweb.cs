using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Query;
using Newtonsoft.Json.Linq;
using FluentAssertions;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class Progradweb
    {
        /// <summary>
        /// Run Tests for the following query
        /// 
        /// FROM Aluno a
        /// RJOIN <AlunoMora> (Endereco e)
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void AlunosWithEndereco()
        {
            RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

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

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container2.ERMongoMapping );

            SortArgument SortArg2 = new SortArgument( Aluno, Aluno.Element.GetIdentifierAttribute(), MongoDBSort.Ascending );
            SortStage SortOp2 = new SortStage( new List<SortArgument>() { SortArg2 }, Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2, SortOp2 };

            FromArgument FromArg2 = new FromArgument( Aluno, Container2.ERMongoMapping );


            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();

            Assert.IsNotNull( Query1, "Query1 cannot be null" );
            Assert.IsNotNull( Query2, "Query2 cannot be null" );

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            QueryRunner QueryRunner2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_2" );

            string QueryResult1 = QueryRunner1.GetJSON( Query1 );
            string QueryResult2 = QueryRunner2.GetJSON( Query2 );

            Assert.IsNotNull( QueryResult1, "QueryResult1 cannot be null" );
            Assert.IsNotNull( QueryResult2, "QueryResult2 cannot be null" );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
        }
    }
}
