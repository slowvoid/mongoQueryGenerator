using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.CaseStudy
{
    /// <summary>
    /// Provides data for tests related to ProgradWeb
    /// </summary>
    public static class ProgradWebDataProvider
    {
        #region Properties
        private static ERModel StoredModel { get; set; }
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
        /// ProgradWeb ER Model
        /// </summary>
        /// <returns></returns>
        public static ERModel CreateERModel()
        {
            // This is a simplified version of the original data
            Entity AlunoGrad = new Entity( "AlunoGrad" );
            AlunoGrad.AddAttribute( "codalu_alug", true );
            AlunoGrad.AddAttributes( "nomealu_alug", "datanasc_alug", "cpf_alug" );

            Entity Endereco = new Entity( "Endereco" );
            Endereco.AddAttribute( "codend_end", true );
            Endereco.AddAttributes( "logradouro_end", "bairro_end", "compl_end", "cep_end", "codcidade_end" );

            Entity MatriculaGrad = new Entity( "MatriculaGrad" );
            MatriculaGrad.AddAttribute( "codmatr_matr", true );
            // codalu_matr -> codalu_alug
            // codenf_matr -> codenf_enf
            MatriculaGrad.AddAttributes( "anoini_matr", "semiini_matr" );

            Entity DisciplinaGrad = new Entity( "DisciplinaGrad" );
            DisciplinaGrad.AddAttribute( "coddiscip_discip", true );
            DisciplinaGrad.AddAttributes( "nome_discip" );

            Entity EnfaseGrad = new Entity( "EnfaseGrad" );
            EnfaseGrad.AddAttribute( "codenf_enf", true );
            // codcur_enf -> codcur_cur
            EnfaseGrad.AddAttributes( "nomeenf_enf", "siglaenf_enf", "codcur_enf" );

            Entity CursoGrad = new Entity( "CursoGrad" );
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

            Relationship GradeGrad = new Relationship( "GradeGrad" );
            GradeGrad.AddRelationshipEnd( new RelationshipEnd( DisciplinaGrad ) );
            GradeGrad.AddRelationshipEnd( new RelationshipEnd( EnfaseGrad ) );
            GradeGrad.AddAttributes( "gradegrad_id", "perfil_grd", "userid_grd" );

            return new ERModel( "ProgradWebERModel", new List<BaseERElement>() { AlunoGrad, Endereco, MatriculaGrad, DisciplinaGrad, EnfaseGrad, CursoGrad, AlunoMora, Vinculo, VinculoEnfase, VinculoCurso, GradeGrad } );
        }
    }
}
