using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderApp
{
    public static class ModelSampleFunDep1
    {
        public static Model CreateERModel()
        {
            Entity funcionario = new Entity( "Funcionario" );
            funcionario.AddAttribute( "id" );
            funcionario.AddAttribute( "cpf" );
            funcionario.AddAttribute( "nome_funcionario" );
            funcionario.AddAttribute( "sexo" );

            Entity departamento = new Entity( "Departamento" );
            departamento.AddAttribute( "id" );
            departamento.AddAttribute( "numero_dep" );
            departamento.AddAttribute( "nome_departamento" );
            departamento.AddAttribute( "numero_funcionario" );

            Relationship gerencia = new Relationship( "Gerencia", RelationshipCardinality.OneToOne );
            gerencia.AddAttribute( "data_inicio" );

            RelationshipConnection conn = new RelationshipConnection( funcionario, funcionario.GetAttribute( "id" ),
                departamento, departamento.GetAttribute( "numero_funcionario" ), RelationshipCardinality.OneToOne );
            gerencia.Relations.Add( conn );

            Model ermodel = new Model( "dep", new List<BaseERElement> { funcionario, departamento, gerencia } );

            return ermodel;
        }

        public static MongoSchema CreateSchema()
        {
            Collection docTypeFuncionario = new Collection( "DocTypeFuncionario" );
            docTypeFuncionario.DocumentSchema.AddAttribute( "_id" );
            docTypeFuncionario.DocumentSchema.AddAttribute( "fCpf" );
            docTypeFuncionario.DocumentSchema.AddAttribute( "fNome_funcionario" );
            docTypeFuncionario.DocumentSchema.AddAttribute( "fSexo" );

            Collection docTypeDepartamento = new Collection( "DocTypeDepartamento" );
            docTypeDepartamento.DocumentSchema.AddAttribute( "_id" );
            docTypeDepartamento.DocumentSchema.AddAttribute( "fNumero_dep" );
            docTypeDepartamento.DocumentSchema.AddAttribute( "fNome_departamento" );
            docTypeDepartamento.DocumentSchema.AddAttribute( "fNumero_funcionario" );

            Collection docTypeGerencia = new Collection( "DocTypeGerencia" );
            docTypeGerencia.DocumentSchema.AddAttribute( "data_inicio" );

            MongoSchema schema = new MongoSchema( "dep", new List<Collection> { docTypeFuncionario, docTypeDepartamento, docTypeGerencia } );

            return schema;
        }

        public static ModelMapping CreateMap(Model Ermodel, MongoSchema Schema)
        {
            MapRule funcionarioRule = new MapRule( Ermodel.FindByName( "Funcionario" ), Schema.FindByName( "DocTypeFuncionario" ) );
            funcionarioRule.AddRule( "id", "_id" );
            funcionarioRule.AddRule( "cpf", "fCpf" );
            funcionarioRule.AddRule( "nome_funcionario", "fNome_funcionario" );
            funcionarioRule.AddRule( "sexo", "fSexo" );

            MapRule departamentoRule = new MapRule( Ermodel.FindByName( "Departamento" ), Schema.FindByName( "DocTypeDepartamento" ) );
            departamentoRule.AddRule( "id", "_id" );
            departamentoRule.AddRule( "numero_dep", "fNumero_dep" );
            departamentoRule.AddRule( "nome_departamento", "fNome_departamento" );
            departamentoRule.AddRule( "numero_funcionario", "fNumero_funcionario" );

            MapRule gerenciaRule = new MapRule( Ermodel.FindByName( "Gerencia" ), Schema.FindByName( "DocTypeGerencia" ) );
            gerenciaRule.AddRule( "data_inicio", "data_inicio" );

            return new ModelMapping( "FuncionaroDep", new List<MapRule> { funcionarioRule, departamentoRule, gerenciaRule } );
        }

        public static void Main()
        {
            Model ermodel = CreateERModel();
            MongoSchema schema = CreateSchema();
            ModelMapping map = CreateMap( ermodel, schema );

            RelationshipJoinOperator join = new RelationshipJoinOperator( (Entity)ermodel.FindByName( "Funcionario" ), (Relationship)ermodel.FindByName( "Gerencia" ),
                new List<Entity> { (Entity)ermodel.FindByName( "Departamento" ) }, map );

            List<AlgebraOperator> Operations = new List<AlgebraOperator>();
            Operations.Add( join );

            Pipeline pipeline = new Pipeline( Operations );

            QueryGenerator query = new QueryGenerator( pipeline )
            {
                CollectionName = "DocTypeFuncionario"
            };

            Console.WriteLine( $"Query output: {query.Run()}" );
            Console.Read();
        }
    }
}
