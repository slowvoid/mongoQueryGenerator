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
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Aluno a
        /// RJOIN <Vinculo> ( Matricula m
        ///                   RJOIN <VinculoEnfase> ( Enfase e
        ///                                           RJOIN <VinculoCurso> ( Curso c ) ) )
        /// RJOIN <AlunoMora> ( Endereco end )
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void AlunosWithEnderecoMatriculaEnfaseAndCurso()
        {
            RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();
            RequiredDataContainer Container3 = ProgradWebDataProvider.MapEntitiesToCollectionsCursoEmbedded();

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            ComputedEntity EnfaseCursoCE = new ComputedEntity( "EnfaseCursoCE",
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso } );

            ComputedEntity MatriculaEnfaseCE = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseCursoCE ) } );

            RelationshipJoinOperator RJoinOp1_A = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE ) },
                Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp1_B = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container1.ERMongoMapping );

            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1_A, RJoinOp1_B };
            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_A = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE ) },
                Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_B = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2_A, RJoinOp2_B };
            FromArgument FromArg2 = new FromArgument( Aluno, Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_A = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE ) },
                Container3.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_B = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container3.ERMongoMapping );

            List<AlgebraOperator> OperatorList3 = new List<AlgebraOperator>() { RJoinOp3_A, RJoinOp3_B };
            FromArgument FromArg3 = new FromArgument( Aluno, Container3.ERMongoMapping );

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );
            QueryGenerator QueryGen3 = new QueryGenerator( FromArg3, OperatorList3 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();
            string Query3 = QueryGen3.Run();

            Assert.IsNotNull( Query1, "Query1 cannot be null" );
            Assert.IsNotNull( Query2, "Query2 cannot be null" );
            Assert.IsNotNull( Query3, "Query3 cannot be null" );

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            QueryRunner QueryRunner2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_2" );
            QueryRunner QueryRunner3 = new QueryRunner( "mongodb://localhost:27017", "progradweb_4" );

            string QueryResult1 = QueryRunner1.GetJSON( Query1 );
            string QueryResult2 = QueryRunner2.GetJSON( Query2 );
            string QueryResult3 = QueryRunner3.GetJSON( Query3 );

            Assert.IsNotNull( QueryResult1, "QueryResult1 cannot be null" );
            Assert.IsNotNull( QueryResult2, "QueryResult2 cannot be null" );
            Assert.IsNotNull( QueryResult3, "QueryResult3 cannot be null" );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );
            JToken JSONResult3 = JToken.Parse( QueryResult3 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
            JSONResult1.Should().BeEquivalentTo( JSONResult3 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Aluno a
        /// RJOIN <Vinculo> ( Matricula m
        ///                   RJOIN <VinculoEnfase> ( Enfase e
        ///                                           RJOIN <Grade> ( Disciplina d ) ) )
        /// RJOIN <AlunoMora> ( Endereco end )
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void AlunosWithEnderecoMatriculaEnfaseAndDisciplinas()
        {
            RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );
            QueryableEntity Disciplina = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Disciplina" ) );

            ComputedEntity EnfaseDisciplinasCE = new ComputedEntity( "EnfaseCursoCE",
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina } );

            ComputedEntity MatriculaEnfaseCE = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseDisciplinasCE ) } );

            RelationshipJoinOperator RJoinOp1_A = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE ) },
                Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp1_B = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container1.ERMongoMapping );

            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1_A, RJoinOp1_B };
            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_A = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE ) },
                Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_B = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2_A, RJoinOp2_B };
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
            Assert.IsNotNull( QueryResult2, "QueryResult1 cannot be null" );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
        }
    }
}
