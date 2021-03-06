﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using FluentAssertions.Json;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Map;
using QueryBuilder.Javascript;
using QueryBuilder.Parser;

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
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-endereco-embedded.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Endereco" ) );

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
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco2 },
                Container2.ERMongoMapping );

            SortArgument SortArg2 = new SortArgument( Aluno2, Aluno2.Element.GetIdentifierAttribute(), MongoDBSort.Ascending );
            SortStage SortOp2 = new SortStage( new List<SortArgument>() { SortArg2 }, Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2, SortOp2 };

            FromArgument FromArg2 = new FromArgument( Aluno2, Container2.ERMongoMapping );


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
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();
            //RequiredDataContainer Container3 = ProgradWebDataProvider.MapEntitiesToCollectionsCursoEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-endereco-embedded.mapping" ) );
            var Container3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-curso-embedded.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Endereco2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Endereco" ) );

            QueryableEntity Aluno3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Endereco3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Endereco" ) );

            ComputedEntity EnfaseCursoCE = new ComputedEntity( "EnfaseCursoCE",
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso } );

            ComputedEntity MatriculaEnfaseCE = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseCursoCE ) } );

            ComputedEntity EnfaseCursoCE2 = new ComputedEntity( "EnfaseCursoCE",
                Enfase2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso2 } );

            ComputedEntity MatriculaEnfaseCE2 = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseCursoCE2 ) } );

            ComputedEntity EnfaseCursoCE3 = new ComputedEntity( "EnfaseCursoCE",
                Enfase3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso3 } );

            ComputedEntity MatriculaEnfaseCE3 = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseCursoCE3 ) } );

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
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE2 ) },
                Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_B = new RelationshipJoinOperator(
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco2 },
                Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2_A, RJoinOp2_B };
            FromArgument FromArg2 = new FromArgument( Aluno2, Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_A = new RelationshipJoinOperator(
                Aluno3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE3 ) },
                Container3.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_B = new RelationshipJoinOperator(
                Aluno3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco3 },
                Container3.ERMongoMapping );

            List<AlgebraOperator> OperatorList3 = new List<AlgebraOperator>() { RJoinOp3_A, RJoinOp3_B };
            FromArgument FromArg3 = new FromArgument( Aluno3, Container3.ERMongoMapping );

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
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();
            //RequiredDataContainer Container3 = ProgradWebDataProvider.MapEntitiesToCollectionsDisciplinaEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-endereco-embedded.mapping" ) );
            var Container3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-disciplina-embedded.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );
            QueryableEntity Disciplina = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Disciplina" ) );

            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Endereco2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Endereco" ) );
            QueryableEntity Disciplina2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Disciplina" ) );

            QueryableEntity Aluno3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Endereco3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Endereco" ) );
            QueryableEntity Disciplina3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Disciplina" ) );

            ComputedEntity EnfaseDisciplinasCE = new ComputedEntity( "EnfaseCursoCE",
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina } );

            ComputedEntity MatriculaEnfaseCE = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseDisciplinasCE ) } );

            ComputedEntity EnfaseDisciplinasCE2 = new ComputedEntity( "EnfaseCursoCE",
                Enfase2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina2 } );

            ComputedEntity MatriculaEnfaseCE2 = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseDisciplinasCE2 ) } );

            ComputedEntity EnfaseDisciplinasCE3 = new ComputedEntity( "EnfaseCursoCE",
                Enfase3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina3 } );

            ComputedEntity MatriculaEnfaseCE3 = new ComputedEntity( "MatriculaEnfaseCE",
                Matricula3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( EnfaseDisciplinasCE3 ) } );

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
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE2 ) },
                Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_B = new RelationshipJoinOperator(
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco2 },
                Container2.ERMongoMapping );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2_A, RJoinOp2_B };
            FromArgument FromArg2 = new FromArgument( Aluno2, Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_A = new RelationshipJoinOperator(
                Aluno3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaEnfaseCE3 ) },
                Container3.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_B = new RelationshipJoinOperator(
                Aluno3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco3 },
                Container3.ERMongoMapping );

            List<AlgebraOperator> OperatorList3 = new List<AlgebraOperator>() { RJoinOp3_A, RJoinOp3_B };
            FromArgument FromArg3 = new FromArgument( Aluno3, Container3.ERMongoMapping );

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
            QueryRunner QueryRunner3 = new QueryRunner( "mongodb://localhost:27017", "progradweb_5" );

            string QueryResult1 = QueryRunner1.GetJSON( Query1 );
            string QueryResult2 = QueryRunner2.GetJSON( Query2 );
            string QueryResult3 = QueryRunner3.GetJSON( Query3 );

            Assert.IsNotNull( QueryResult1, "QueryResult1 cannot be null" );
            Assert.IsNotNull( QueryResult2, "QueryResult1 cannot be null" );
            Assert.IsNotNull( QueryResult3, "QueryResult1 cannot be null" );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );
            JToken JSONResult3 = JToken.Parse( QueryResult3 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
            JSONResult1.Should().BeEquivalentTo( JSONResult3 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Enfase e
        /// RJOIN <VinculoCurso> ( Curso c )
        /// RJOIN <Grade> ( Disciplina )
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void EnfaseWithCursoAndDisciplina()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsCursoEmbedded();
            //RequiredDataContainer Container3 = ProgradWebDataProvider.MapEntitiesToCollectionsDisciplinaEmbedded();
            //RequiredDataContainer Container4 = ProgradWebDataProvider.MapEntitiesToCollectionsCursoDisciplinaEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-curso-embedded.mapping" ) );
            var Container3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-disciplina-embedded.mapping" ) );
            var Container4 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-curso-disciplina-embedded.mapping" ) );

            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Disciplina = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Disciplina" ) );

            QueryableEntity Enfase2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Disciplina2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Disciplina" ) );

            QueryableEntity Enfase3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Disciplina3 = new QueryableEntity( Container3.EntityRelationshipModel.FindByName( "Disciplina" ) );

            QueryableEntity Enfase4 = new QueryableEntity( Container4.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso4 = new QueryableEntity( Container4.EntityRelationshipModel.FindByName( "Curso" ) );
            QueryableEntity Disciplina4 = new QueryableEntity( Container4.EntityRelationshipModel.FindByName( "Disciplina" ) );

            RelationshipJoinOperator RJoinOp1_A = new RelationshipJoinOperator(
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso },
                Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp1_B = new RelationshipJoinOperator(
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina },
                Container1.ERMongoMapping );

            FromArgument FromArg1 = new FromArgument( Enfase, Container1.ERMongoMapping );
            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1_A, RJoinOp1_B };

            RelationshipJoinOperator RJoinOp2_A = new RelationshipJoinOperator(
                Enfase2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso2 },
                Container2.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2_B = new RelationshipJoinOperator(
                Enfase2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina2 },
                Container2.ERMongoMapping );

            FromArgument FromArg2 = new FromArgument( Enfase2, Container2.ERMongoMapping );
            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2_A, RJoinOp2_B };

            RelationshipJoinOperator RJoinOp3_A = new RelationshipJoinOperator(
                Enfase3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso3 },
                Container3.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3_B = new RelationshipJoinOperator(
                Enfase3,
                (Relationship)Container3.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina3 },
                Container3.ERMongoMapping );

            FromArgument FromArg3 = new FromArgument( Enfase3, Container3.ERMongoMapping );
            List<AlgebraOperator> OperatorList3 = new List<AlgebraOperator>() { RJoinOp3_A, RJoinOp3_B };

            RelationshipJoinOperator RJoinOp4_A = new RelationshipJoinOperator(
                Enfase4,
                (Relationship)Container4.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso4 },
                Container4.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4_B = new RelationshipJoinOperator(
                Enfase4,
                (Relationship)Container4.EntityRelationshipModel.FindByName( "Grade" ),
                new List<QueryableEntity>() { Disciplina4 },
                Container4.ERMongoMapping );

            FromArgument FromArg4 = new FromArgument( Enfase4, Container4.ERMongoMapping );
            List<AlgebraOperator> OperatorList4 = new List<AlgebraOperator>() { RJoinOp4_A, RJoinOp4_B };

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );
            QueryGenerator QueryGen3 = new QueryGenerator( FromArg3, OperatorList3 );
            QueryGenerator QueryGen4 = new QueryGenerator( FromArg4, OperatorList4 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();
            string Query3 = QueryGen3.Run();
            string Query4 = QueryGen4.Run();

            Assert.IsNotNull( Query1 );
            Assert.IsNotNull( Query2 );
            Assert.IsNotNull( Query3 );
            Assert.IsNotNull( Query4 );

            QueryRunner QueryRun1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            QueryRunner QueryRun2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_4" );
            QueryRunner QueryRun3 = new QueryRunner( "mongodb://localhost:27017", "progradweb_5" );
            QueryRunner QueryRun4 = new QueryRunner( "mongodb://localhost:27017", "progradweb_10" );

            string QueryResult1 = QueryRun1.GetJSON( Query1 );
            string QueryResult2 = QueryRun2.GetJSON( Query2 );
            string QueryResult3 = QueryRun3.GetJSON( Query3 );
            string QueryResult4 = QueryRun4.GetJSON( Query4 );

            Assert.IsNotNull( QueryResult1 );
            Assert.IsNotNull( QueryResult2 );
            Assert.IsNotNull( QueryResult3 );
            Assert.IsNotNull( QueryResult4 );

            JToken JSONQuery1 = JToken.Parse( QueryResult1 );
            JToken JSONQuery2 = JToken.Parse( QueryResult2 );
            JToken JSONQuery3 = JToken.Parse( QueryResult3 );
            JToken JSONQuery4 = JToken.Parse( QueryResult4 );

            JSONQuery1.Should().BeEquivalentTo( JSONQuery2 );
            JSONQuery1.Should().BeEquivalentTo( JSONQuery3 );
            JSONQuery1.Should().BeEquivalentTo( JSONQuery4 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Aluno a
        /// RJOIN <Mora> ( Endereco e )
        /// SELECT a.nomealu_alug, e.logradouro_end, e.bairro_end, e.cep_end
        /// </summary>
        [TestMethod]
        public void AlunosWithEnderecoOnlyNameAndBaseAddress()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-endereco-embedded.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Endereco" ) );

            List<ProjectArgument> ProjectArgs = new List<ProjectArgument>();
            ProjectArgs.Add( new ProjectArgument( Aluno.GetAttribute( "nomealu_alug" ), Aluno, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Endereco.GetAttribute( "logradouro_end" ), Endereco, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Endereco.GetAttribute( "bairro_end" ), Endereco, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Endereco.GetAttribute( "cep_end" ), Endereco, new BooleanExpr( true ) ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container1.ERMongoMapping );

            ProjectStage ProjectOp1 = new ProjectStage( ProjectArgs, RJoinOp1.ComputeVirtualMap() );

            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1, ProjectOp1 };
            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator(
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco2 },
                Container2.ERMongoMapping );

            ProjectStage ProjectOp2 = new ProjectStage( ProjectArgs, RJoinOp2.ComputeVirtualMap() );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2, ProjectOp2 };
            FromArgument FromArg2 = new FromArgument( Aluno2, Container2.ERMongoMapping );

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();

            Assert.IsNotNull( Query1 );
            Assert.IsNotNull( Query2 );

            QueryRunner QueryRun1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            QueryRunner QueryRun2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_2" );

            string QueryResult1 = QueryRun1.GetJSON( Query1 );
            string QueryResult2 = QueryRun2.GetJSON( Query2 );

            Assert.IsNotNull( QueryResult1 );
            Assert.IsNotNull( QueryResult2 );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Aluno a
        /// RJOIN <Mora> ( Endereco e )
        /// WHERE e.bairro_end = 'Buckinghamshire'
        /// SELECT a.nomealu_alug, e.logradouro_end, e.bairro_end, e.cep_end
        /// </summary>
        [TestMethod]
        public void AlunosWithEnderecoOnlyNameAndBaseAddressWhereBairro()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsEnderecoEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-endereco-embedded.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Endereco" ) );

            List<ProjectArgument> ProjectArgs = new List<ProjectArgument>();
            ProjectArgs.Add( new ProjectArgument( Aluno.GetAttribute( "nomealu_alug" ), Aluno, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Endereco.GetAttribute( "logradouro_end" ), Endereco, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Endereco.GetAttribute( "bairro_end" ), Endereco, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Endereco.GetAttribute( "cep_end" ), Endereco, new BooleanExpr( true ) ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container1.ERMongoMapping );

            ProjectStage ProjectOp1 = new ProjectStage( ProjectArgs, RJoinOp1.ComputeVirtualMap() );

            VirtualMap Vmap1 = RJoinOp1.ComputeVirtualMap();
            SelectArgument SelectArg1 = new SelectArgument( new InFieldArrayExpr( $"${Vmap1.GetRuleValue( "Endereco", "bairro_end" )}", "Buckinghamshire" ) );
            SelectStage SelectOp1 = new SelectStage( SelectArg1, Vmap1 );

            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1, SelectOp1, ProjectOp1 };
            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator(
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco2 },
                Container2.ERMongoMapping );

            ProjectStage ProjectOp2 = new ProjectStage( ProjectArgs, RJoinOp2.ComputeVirtualMap() );

            VirtualMap Vmap2 = RJoinOp2.ComputeVirtualMap();
            SelectArgument SelectArg2 = new SelectArgument( new InFieldArrayExpr( $"${Vmap2.GetRuleValue( "Endereco", "bairro_end" )}", "Buckinghamshire" ) );
            SelectStage SelectOp2 = new SelectStage( SelectArg2, Vmap2 );

            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2, SelectOp2, ProjectOp2 };
            FromArgument FromArg2 = new FromArgument( Aluno, Container2.ERMongoMapping );

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();

            Assert.IsNotNull( Query1 );
            Assert.IsNotNull( Query2 );

            QueryRunner QueryRun1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            QueryRunner QueryRun2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_2" );

            string QueryResult1 = QueryRun1.GetJSON( Query1 );
            string QueryResult2 = QueryRun2.GetJSON( Query2 );

            Assert.IsNotNull( QueryResult1 );
            Assert.IsNotNull( QueryResult2 );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Enfase e
        /// RJOIN <VinculoEnfase> ( Matricula m  
        ///                         RJOIN <Vinculo> (Aluno a) )
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void EnfaseWithMatriculaAndAlunos()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsAlunoEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-aluno-embedded.mapping" ) );

            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );

            QueryableEntity Enfase2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Matricula2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );

            ComputedEntity MatriculaAluno = new ComputedEntity( "MatriculaAluno",
                Matricula,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { Aluno } );

            ComputedEntity MatriculaAluno2 = new ComputedEntity( "MatriculaAluno",
                Matricula2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "Vinculo" ),
                new List<QueryableEntity>() { Aluno2 } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaAluno ) },
                Container1.ERMongoMapping );

            FromArgument FromArg1 = new FromArgument( Enfase, Container1.ERMongoMapping );
            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1 };

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator(
                Enfase2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "VinculoEnfase" ),
                new List<QueryableEntity>() { new QueryableEntity( MatriculaAluno2 ) },
                Container2.ERMongoMapping );

            FromArgument FromArg2 = new FromArgument( Enfase2, Container2.ERMongoMapping );
            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2 };

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();

            Assert.IsNotNull( Query1 );
            Assert.IsNotNull( Query2 );

            QueryRunner QueryRun1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_1" );
            QueryRunner QueryRun2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_6" );

            string QueryResult1 = QueryRun1.GetJSON( Query1 );
            string QueryResult2 = QueryRun2.GetJSON( Query2 );

            Assert.IsNotNull( QueryResult1 );
            Assert.IsNotNull( QueryResult2 );

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/Progradweb/EnfaseMatriculaALuno.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            string QueryResultHandcrafted = QueryRun1.GetJSON( HandcraftedQuery );

            Assert.IsNotNull( QueryResultHandcrafted );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResultHandcrafted );
            JToken JSONResult3 = JToken.Parse( QueryResult2 );

            JSONResult1.Should().BeEquivalentTo( JSONResult2 );
            JSONResult1.Should().BeEquivalentTo( JSONResult3 );
        }
        /// <summary>
        /// Run tests for the following query
        /// But the relationship cardinality is 1:N
        /// 
        /// FROM Aluno 
        /// RJOIN <AlunoMora> ( Endereco )
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void AlunoWithManyEndereco()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollectionsManyEndereco();
            //RequiredDataContainer Container2 = ProgradWebDataProvider.MapEntitiesToCollectionsManyEnderecoEmbedded();

            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-many-endereco.mapping" ) );
            var Container2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-many-endereco-embedded.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Endereco" ) );

            QueryableEntity Aluno2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Endereco2 = new QueryableEntity( Container2.EntityRelationshipModel.FindByName( "Endereco" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco },
                Container1.ERMongoMapping );

            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );
            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1 };

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator(
                Aluno2,
                (Relationship)Container2.EntityRelationshipModel.FindByName( "AlunoMora" ),
                new List<QueryableEntity>() { Endereco2 },
                Container2.ERMongoMapping );

            FromArgument FromArg2 = new FromArgument( Aluno2, Container2.ERMongoMapping );
            List<AlgebraOperator> OperatorList2 = new List<AlgebraOperator>() { RJoinOp2 };

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( FromArg2, OperatorList2 );

            string Query1 = QueryGen1.Run();
            string Query2 = QueryGen2.Run();

            Assert.IsNotNull( Query1 );
            Assert.IsNotNull( Query2 );

            QueryRunner QueryRun1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_7" );
            QueryRunner QueryRun2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_8" );

            string QueryResult1 = QueryRun1.GetJSON( Query1 );
            string QueryResult2 = QueryRun2.GetJSON( Query2 );

            Assert.IsNotNull( QueryResult1 );
            Assert.IsNotNull( QueryResult2 );

            JToken ResultJSON1 = JToken.Parse( QueryResult1 );
            JToken ResultJSON2 = JToken.Parse( QueryResult2 );

            ResultJSON1.Should().BeEquivalentTo( ResultJSON2 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// FROM Aluno a
        /// RJOIN <Matriculado> ( Matricula m, Enfase e )
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void AlunosWithMatriculaEnfase()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections3();
            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-3.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Curso" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Matriculado" ),
                new List<QueryableEntity>() { Matricula, Enfase },
                Container1.ERMongoMapping );

            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );
            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1 };

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );

            string Query1 = QueryGen1.Run();
            string Query2 = Utils.ReadQueryFromFile( "HandcraftedQueries/Progradweb/AlunoMatriculaEnfase.js" );

            Assert.IsNotNull( Query1 );
            Assert.IsNotNull( Query2 );

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_9" );
            QueryRunner QueryRunner2 = new QueryRunner( "mongodb://localhost:27017", "progradweb_9" );

            string QueryResult1 = QueryRunner1.GetJSON( Query1 );
            string QueryResult2 = QueryRunner2.GetJSON( Query2 );

            Assert.IsNotNull( QueryResult1 );
            Assert.IsNotNull( QueryResult2 );

            JToken JSONResult1 = JToken.Parse( QueryResult1 );
            JToken JSONResult2 = JToken.Parse( QueryResult2 );

            JToken.DeepEquals( JSONResult1, JSONResult2 );
        }
        /// <summary>
        /// Run tests for the following query
        /// 
        /// THIS IS A SAMPLE OF HOW IT CAN WORK WITH MULTIPLE ENTITIES INCLUDING A COMPUTED ENTITY
        /// 
        /// FROM Aluno a
        /// RJOIN <Matriculado> ( Matricula m, Enfase e RJOIN <VinculoCurso> (Curso c))
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void AlunosWithMatriculaEnfaseAndCurso()
        {
            //RequiredDataContainer Container1 = ProgradWebDataProvider.MapEntitiesToCollections3();
            var Container1 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/Progradweb/entities-to-collections-3.mapping" ) );

            QueryableEntity Aluno = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Aluno" ) );
            QueryableEntity Matricula = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Matricula" ) );
            QueryableEntity Enfase = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Enfase" ) );
            QueryableEntity Curso = new QueryableEntity( Container1.EntityRelationshipModel.FindByName( "Curso" ) );

            ComputedEntity EnfaseCurso = new ComputedEntity( "EnfaseCurso",
                Enfase,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "VinculoCurso" ),
                new List<QueryableEntity>() { Curso } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator(
                Aluno,
                (Relationship)Container1.EntityRelationshipModel.FindByName( "Matriculado" ),
                new List<QueryableEntity>() { Matricula, new QueryableEntity(EnfaseCurso) },
                Container1.ERMongoMapping );

            FromArgument FromArg1 = new FromArgument( Aluno, Container1.ERMongoMapping );
            List<AlgebraOperator> OperatorList1 = new List<AlgebraOperator>() { RJoinOp1 };

            QueryGenerator QueryGen1 = new QueryGenerator( FromArg1, OperatorList1 );

            string Query1 = QueryGen1.Run();

            Assert.IsNotNull( Query1 );

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "progradweb_9" );

            string QueryResult1 = QueryRunner1.GetJSON( Query1 );

            Assert.IsNotNull( QueryResult1 );
        }
    }
}
