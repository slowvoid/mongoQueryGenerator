using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Tests;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data for tests related to ProgradWeb
    /// </summary>
    public static class ProgradWebDataProvider
    {
        #region Properties
        private static ERModel StoredModel { get; set; }
        private static ERModel StoredModel2 { get; set; }
        #endregion
        /// <summary>
        /// Return or creates a ERModel
        /// </summary>
        /// <returns></returns>
        public static ERModel GetERModel()
        {
            if ( StoredModel == null )
            {
                StoredModel = CreateERModel();
            }

            return StoredModel;
        }
        /// <summary>
        /// Returns or creates a ERModel2
        /// </summary>
        /// <returns></returns>
        public static ERModel GetERModel2()
        {
            if ( StoredModel2 == null )
            {
                StoredModel2 = CreateERModel2();
            }

            return StoredModel2;
        }
        /// <summary>
        /// ProgradWeb ER Model
        /// </summary>
        /// <returns></returns>
        public static ERModel CreateERModel()
        {
            // This is a simplified version of the original data
            Entity AlunoGrad = new Entity( "Aluno" );
            AlunoGrad.AddAttribute( "codalu_alug", true );
            AlunoGrad.AddAttributes( "nomealu_alug", "cpf_alug" );

            Entity Endereco = new Entity( "Endereco" );
            Endereco.AddAttribute( "codend_end", true );
            Endereco.AddAttributes( "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            Entity MatriculaGrad = new Entity( "Matricula" );
            MatriculaGrad.AddAttribute( "codmatr_matr", true );
            // codalu_matr -> codalu_alug
            // codenf_matr -> codenf_enf
            MatriculaGrad.AddAttributes( "anoini_matr", "semiini_matr" );

            Entity DisciplinaGrad = new Entity( "Disciplina" );
            DisciplinaGrad.AddAttribute( "coddiscip_discip", true );
            DisciplinaGrad.AddAttributes( "nome_discip" );

            Entity EnfaseGrad = new Entity( "Enfase" );
            EnfaseGrad.AddAttribute( "codenf_enf", true );
            // codcur_enf -> codcur_cur
            EnfaseGrad.AddAttributes( "nomeenf_enf", "siglaenf_enf" );

            Entity CursoGrad = new Entity( "Curso" );
            CursoGrad.AddAttribute( "codcur_cur", true );
            CursoGrad.AddAttributes( "sigla_cur", "nomecur_cur" );

            Relationship AlunoMora = new Relationship( "AlunoMora" );
            AlunoMora.AddRelationshipEnd( new RelationshipEnd( AlunoGrad ) );
            AlunoMora.AddRelationshipEnd( new RelationshipEnd( Endereco ) );

            Relationship Vinculo = new Relationship( "Vinculo" );
            Vinculo.AddRelationshipEnd( new RelationshipEnd( AlunoGrad ) );
            Vinculo.AddRelationshipEnd( new RelationshipEnd( MatriculaGrad ) );

            Relationship VinculoEnfase = new Relationship( "VinculoEnfase" );
            VinculoEnfase.AddRelationshipEnd( new RelationshipEnd( MatriculaGrad ) );
            VinculoEnfase.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );

            Relationship VinculoCurso = new Relationship( "VinculoCurso" );
            VinculoCurso.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );
            VinculoCurso.AddRelationshipEnd( new RelationshipEnd( CursoGrad ) );

            Relationship GradeGrad = new Relationship( "Grade" );
            GradeGrad.AddRelationshipEnd( new RelationshipEnd( DisciplinaGrad ) );
            GradeGrad.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );
            GradeGrad.AddAttributes( "gradegrad_id", "perfil_grd", "userid_grd" );

            return new ERModel( "ProgradWebERModel", new List<BaseERElement>() { AlunoGrad, Endereco, MatriculaGrad, DisciplinaGrad, EnfaseGrad, CursoGrad, AlunoMora, Vinculo, VinculoEnfase, VinculoCurso, GradeGrad } );
        }
        /// <summary>
        /// Progradweb ER Model2
        /// </summary>
        /// <returns></returns>
        public static ERModel CreateERModel2()
        {
            // This is a simplified version of the original data
            Entity AlunoGrad = new Entity( "Aluno" );
            AlunoGrad.AddAttribute( "codalu_alug", true );
            AlunoGrad.AddAttributes( "nomealu_alug", "cpf_alug" );

            Entity Endereco = new Entity( "Endereco" );
            Endereco.AddAttribute( "codend_end", true );
            Endereco.AddAttributes( "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            Entity MatriculaGrad = new Entity( "Matricula" );
            MatriculaGrad.AddAttribute( "codmatr_matr", true );
            MatriculaGrad.AddAttributes( "anoini_matr", "semiini_matr" );

            Entity DisciplinaGrad = new Entity( "Disciplina" );
            DisciplinaGrad.AddAttribute( "coddiscip_discip", true );
            DisciplinaGrad.AddAttributes( "nome_discip" );

            Entity EnfaseGrad = new Entity( "Enfase" );
            EnfaseGrad.AddAttribute( "codenf_enf", true );
            EnfaseGrad.AddAttributes( "nomeenf_enf", "siglaenf_enf" );

            Entity CursoGrad = new Entity( "Curso" );
            CursoGrad.AddAttribute( "codcur_cur", true );
            CursoGrad.AddAttributes( "sigla_cur", "nomecur_cur" );

            Relationship Matriculado = new Relationship( "Matriculado" );
            Matriculado.AddAttribute( "matriculadoId" );
            Matriculado.AddRelationshipEnd( new RelationshipEnd( AlunoGrad ) );
            Matriculado.AddRelationshipEnd( new RelationshipEnd( MatriculaGrad ) );
            Matriculado.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );

            Relationship AlunoMora = new Relationship( "AlunoMora" );
            AlunoMora.AddRelationshipEnd( new RelationshipEnd( AlunoGrad ) );
            AlunoMora.AddRelationshipEnd( new RelationshipEnd( Endereco ) );

            Relationship VinculoEnfase = new Relationship( "VinculoEnfase" );
            VinculoEnfase.AddRelationshipEnd( new RelationshipEnd( MatriculaGrad ) );
            VinculoEnfase.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );

            Relationship VinculoCurso = new Relationship( "VinculoCurso" );
            VinculoCurso.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );
            VinculoCurso.AddRelationshipEnd( new RelationshipEnd( CursoGrad ) );

            Relationship GradeGrad = new Relationship( "Grade" );
            GradeGrad.AddRelationshipEnd( new RelationshipEnd( DisciplinaGrad ) );
            GradeGrad.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );
            GradeGrad.AddAttributes( "gradegrad_id", "perfil_grd", "userid_grd" );

            return new ERModel( "ProgradWebERModel", new List<BaseERElement>() { AlunoGrad, Endereco, MatriculaGrad, DisciplinaGrad, EnfaseGrad, CursoGrad, AlunoMora, Matriculado, VinculoEnfase, VinculoCurso, GradeGrad } );
        }

        public static RequiredDataContainer MapEntitiesToCollections()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "datanasc_alug", "cpf_alug", "endif_alug" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "datanasc_alug", "datanasc_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), EnderecoCol );
            EnderecoRule.AddRule( "codend_end", "_id" );
            EnderecoRule.AddRule( "logradouro_end", "logradouro_end" );
            EnderecoRule.AddRule( "bairro_end", "bairro_end" );
            EnderecoRule.AddRule( "compl_end", "compl_end" );
            EnderecoRule.AddRule( "cep_end", "cep_end" );
            EnderecoRule.AddRule( "codcidade_end", "codcidade_end" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "codalu_matr" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, CursoRule, DisciplinaRule, EnderecoRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsEnderecoEmbedded()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "datanasc_alug", "cpf_alug", "endif_alug" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "datanasc_alug", "datanasc_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endereco.enderecoid" );
            EnderecoAlunoRule.AddRule( "logradouro_end", "endereco.logradouro" );
            EnderecoAlunoRule.AddRule( "bairro_end", "endereco.bairro" );
            EnderecoAlunoRule.AddRule( "compl_end", "endereco.complemento" );
            EnderecoAlunoRule.AddRule( "cep_end", "endereco.cep" );
            EnderecoAlunoRule.AddRule( "codcidade_end", "endereco.codcidade" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "codalu_matr" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, CursoRule, DisciplinaRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsCursoEmbedded()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "datanasc_alug", "cpf_alug", "endif_alug" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "curso.cursoId", "curso.sigla", "curso.nome" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "datanasc_alug", "datanasc_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), EnderecoCol );
            EnderecoRule.AddRule( "codend_end", "_id" );
            EnderecoRule.AddRule( "logradouro_end", "logradouro_end" );
            EnderecoRule.AddRule( "bairro_end", "bairro_end" );
            EnderecoRule.AddRule( "compl_end", "compl_end" );
            EnderecoRule.AddRule( "cep_end", "cep_end" );
            EnderecoRule.AddRule( "codcidade_end", "codcidade_end" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "curso.cursoId" );
            CursoEnfaseRule.AddRule( "sigla_cur", "curso.sigla" );
            CursoEnfaseRule.AddRule( "nomecur_cur", "curso.nome" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "codalu_matr" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, DisciplinaRule, EnderecoRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsDisciplinaEmbedded()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "datanasc_alug", "cpf_alug", "endif_alug" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf", "disciplinas_multivalued_" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "datanasc_alug", "datanasc_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), EnderecoCol );
            EnderecoRule.AddRule( "codend_end", "_id" );
            EnderecoRule.AddRule( "logradouro_end", "logradouro_end" );
            EnderecoRule.AddRule( "bairro_end", "bairro_end" );
            EnderecoRule.AddRule( "compl_end", "compl_end" );
            EnderecoRule.AddRule( "cep_end", "cep_end" );
            EnderecoRule.AddRule( "codcidade_end", "codcidade_end" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "codalu_matr" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            MapRule DisciplinaEnfaseRule = new MapRule( Model.FindByName( "Disciplina" ), EnfaseCol, false );
            DisciplinaEnfaseRule.AddRule( "coddiscip_discip", "disciplinas_multivalued_.disciplinaId" );
            DisciplinaEnfaseRule.AddRule( "nome_discip", "disciplinas_multivalued_.nome_discip" );

            MapRule GradeEnfaseRule = new MapRule( Model.FindByName( "Grade" ), EnfaseCol, false );
            GradeEnfaseRule.AddRule( "gradegrad_id", "disciplinas_multivalued_.gradeId" );
            GradeEnfaseRule.AddRule( "perfil_grd", "disciplinas_multivalued_.perfil_grd" );
            GradeEnfaseRule.AddRule( "userid_grd", "disciplinas_multivalued_.userid_grd" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, CursoRule, EnderecoRule, EnfaseRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule, DisciplinaEnfaseRule, GradeEnfaseRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsAlunoEmbedded()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "datanasc_alug", "cpf_alug", "endif_alug" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr",
                "aluno.alunoId", "aluno.nomeAluno", "aluno.cpf_alug", "aluno.enderecoId" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "aluno.alunoId" );
            AlunoMatriculaRule.AddRule( "nomealu_alug", "aluno.nomeAluno" );
            AlunoMatriculaRule.AddRule( "cpf_alug", "aluno.cpf_alug" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), EnderecoCol );
            EnderecoRule.AddRule( "codend_end", "_id" );
            EnderecoRule.AddRule( "logradouro_end", "logradouro_end" );
            EnderecoRule.AddRule( "bairro_end", "bairro_end" );
            EnderecoRule.AddRule( "compl_end", "compl_end" );
            EnderecoRule.AddRule( "cep_end", "cep_end" );
            EnderecoRule.AddRule( "codcidade_end", "codcidade_end" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { CursoRule, DisciplinaRule, EnderecoRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsManyEndereco()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "cpf_alug" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end", "aluno_id" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule AlunoEnderecoRule = new MapRule( Model.FindByName( "Aluno" ), EnderecoCol, false );
            AlunoEnderecoRule.AddRule( "codalu_alug", "aluno_id" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), EnderecoCol );
            EnderecoRule.AddRule( "codend_end", "_id" );
            EnderecoRule.AddRule( "logradouro_end", "logradouro_end" );
            EnderecoRule.AddRule( "bairro_end", "bairro_end" );
            EnderecoRule.AddRule( "compl_end", "compl_end" );
            EnderecoRule.AddRule( "cep_end", "cep_end" );
            EnderecoRule.AddRule( "codcidade_end", "codcidade_end" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "codalu_matr" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, CursoRule, DisciplinaRule, EnderecoRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule, AlunoEnderecoRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsManyEnderecoEmbedded()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "cpf_alug", "endif_alug",
                "enderecos_multivalued_.enderecoId", "enderecos_multivalued_.logradouro", "enderecos_multivalued_.bairro", "enderecos_multivalued_.complemento", "enderecos_multivalued_.cep", "enderecos_multivalued_.codcidade" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr", "codalu_matr", "codenf_matr" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoRule.AddRule( "codend_end", "enderecos_multivalued_.enderecoId" );
            EnderecoRule.AddRule( "logradouro_end", "enderecos_multivalued_.logradouro" );
            EnderecoRule.AddRule( "bairro_end", "enderecos_multivalued_.bairro" );
            EnderecoRule.AddRule( "compl_end", "enderecos_multivalued_.complemento" );
            EnderecoRule.AddRule( "cep_end", "enderecos_multivalued_.cep" );
            EnderecoRule.AddRule( "codcidade_end", "enderecos_multivalued_.codcidade" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculaCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "codalu_matr" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculaCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "codenf_matr" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, CursoRule, DisciplinaRule, EnderecoRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollections3()
        {
            // MongoDB Schema
            MongoDBCollection AlunoCol = new MongoDBCollection( "Aluno" );
            AlunoCol.AddAttributes( "_id", "nomealu_alug", "cpf_alug" );

            MongoDBCollection CursoCol = new MongoDBCollection( "Curso" );
            CursoCol.AddAttributes( "_id", "sigla_cur", "nomecur_cur" );

            MongoDBCollection DisciplinaCol = new MongoDBCollection( "Disciplina" );
            DisciplinaCol.AddAttributes( "_id", "nome_discip" );

            MongoDBCollection EnderecoCol = new MongoDBCollection( "Endereco" );
            EnderecoCol.AddAttributes( "_id", "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end", "aluno_id" );

            MongoDBCollection EnfaseCol = new MongoDBCollection( "Enfase" );
            EnfaseCol.AddAttributes( "_id", "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            MongoDBCollection GradeCol = new MongoDBCollection( "Grade" );
            GradeCol.AddAttributes( "_id", "perfil_grd", "userid_grd", "discipgrad_id", "enfgrad_id" );

            MongoDBCollection MatriculaCol = new MongoDBCollection( "Matricula" );
            MatriculaCol.AddAttributes( "_id", "anoini_matr", "semiini_matr" );

            MongoDBCollection MatriculadosCol = new MongoDBCollection( "Matriculados" );
            MatriculadosCol.AddAttributes( "_id", "cod_aluno", "cod_enfase", "cod_matricula" );

            MongoSchema Schema = new MongoSchema( "ProgradwebSchema", new List<MongoDBCollection>() { AlunoCol, CursoCol, DisciplinaCol, EnderecoCol, EnfaseCol, GradeCol, MatriculaCol } );

            // Get ER Model
            ERModel Model = GetERModel2();

            MapRule AlunoRule = new MapRule( Model.FindByName( "Aluno" ), AlunoCol );
            AlunoRule.AddRule( "codalu_alug", "_id" );
            AlunoRule.AddRule( "nomealu_alug", "nomealu_alug" );
            AlunoRule.AddRule( "cpf_alug", "cpf_alug" );

            MapRule AlunoEnderecoRule = new MapRule( Model.FindByName( "Aluno" ), EnderecoCol, false );
            AlunoEnderecoRule.AddRule( "codalu_alug", "aluno_id" );

            MapRule CursoRule = new MapRule( Model.FindByName( "Curso" ), CursoCol );
            CursoRule.AddRule( "codcur_cur", "_id" );
            CursoRule.AddRule( "sigla_cur", "sigla_cur" );
            CursoRule.AddRule( "nomecur_cur", "nomecur_cur" );

            MapRule DisciplinaRule = new MapRule( Model.FindByName( "Disciplina" ), DisciplinaCol );
            DisciplinaRule.AddRule( "coddiscip_discip", "_id" );
            DisciplinaRule.AddRule( "nome_discip", "nome_discip" );

            MapRule EnderecoRule = new MapRule( Model.FindByName( "Endereco" ), EnderecoCol );
            EnderecoRule.AddRule( "codend_end", "_id" );
            EnderecoRule.AddRule( "logradouro_end", "logradouro_end" );
            EnderecoRule.AddRule( "bairro_end", "bairro_end" );
            EnderecoRule.AddRule( "compl_end", "compl_end" );
            EnderecoRule.AddRule( "cep_end", "cep_end" );
            EnderecoRule.AddRule( "codcidade_end", "codcidade_end" );

            MapRule EnfaseRule = new MapRule( Model.FindByName( "Enfase" ), EnfaseCol );
            EnfaseRule.AddRule( "codenf_enf", "_id" );
            EnfaseRule.AddRule( "nomeenf_enf", "nomeenf_enf" );
            EnfaseRule.AddRule( "siglaenf_enf", "siglaenf_enf" );

            MapRule GradeRule = new MapRule( Model.FindByName( "Grade" ), GradeCol );
            GradeRule.AddRule( "gradegrad_id", "_id" );
            GradeRule.AddRule( "perfil_grd", "perfil_grd" );
            GradeRule.AddRule( "userid_grd", "userid_grd" );

            MapRule MatriculaRule = new MapRule( Model.FindByName( "Matricula" ), MatriculaCol );
            MatriculaRule.AddRule( "codmatr_matr", "_id" );
            MatriculaRule.AddRule( "anoini_matr", "anoini_matr" );
            MatriculaRule.AddRule( "semiini_matr", "semiini_matr" );

            MapRule EnderecoAlunoRule = new MapRule( Model.FindByName( "Endereco" ), AlunoCol, false );
            EnderecoAlunoRule.AddRule( "codend_end", "endid_alug" );

            MapRule CursoEnfaseRule = new MapRule( Model.FindByName( "Curso" ), EnfaseCol, false );
            CursoEnfaseRule.AddRule( "codcur_cur", "codcur_enf" );

            MapRule DisciplinaGradeRule = new MapRule( Model.FindByName( "Disciplina" ), GradeCol, false );
            DisciplinaGradeRule.AddRule( "coddiscip_discip", "discipgrad_id" );

            MapRule EnfaseGradeRule = new MapRule( Model.FindByName( "Enfase" ), GradeCol, false );
            EnfaseGradeRule.AddRule( "codenf_enf", "enfgrad_id" );

            MapRule AlunoMatriculaRule = new MapRule( Model.FindByName( "Aluno" ), MatriculadosCol, false );
            AlunoMatriculaRule.AddRule( "codalu_alug", "cod_aluno" );

            MapRule EnfaseMatriculaRule = new MapRule( Model.FindByName( "Enfase" ), MatriculadosCol, false );
            EnfaseMatriculaRule.AddRule( "codenf_enf", "cod_enfase" );

            MapRule MatriculaMatriculadosRule = new MapRule( Model.FindByName( "Matricula" ), MatriculadosCol, false );
            MatriculaMatriculadosRule.AddRule( "codmatr_matr", "cod_matricula" );

            MapRule MatriculadosRule = new MapRule( Model.FindByName( "Matriculado" ), MatriculadosCol );
            MatriculadosRule.AddRule( "matriculadoId", "_id" );

            ModelMapping Map = new ModelMapping( "ProgradwebMap", new List<MapRule>() { AlunoRule, CursoRule, DisciplinaRule, EnderecoRule, EnfaseRule, GradeRule, MatriculaRule, EnderecoAlunoRule, CursoEnfaseRule, DisciplinaGradeRule, EnfaseGradeRule, AlunoMatriculaRule, EnfaseMatriculaRule, AlunoEnderecoRule, MatriculaMatriculadosRule, MatriculadosRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}
