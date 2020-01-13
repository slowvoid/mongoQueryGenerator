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

            Assert.IsNotNull( Query1, "Query1 cannot be null" );

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            string QueryResult1 = QueryRunner1.GetJSON( Query1 );

            Assert.IsNotNull( QueryResult1, "QueryResult1 cannot be null" );

            JToken.Parse( QueryResult1 ).Should().BeEquivalentTo( JToken.Parse( QueryResult1 ) );
        }
    }
}
