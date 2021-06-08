using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class ComputedEntityTests
    {
        [TestMethod]
        public void OneToOneComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage)) select *";
            // FIXED?
            // Consulta: from person rjoin <Drives> (Car rjoin <Repaired> (Garage)) select *
            // Problema: Parser gera duas operações rjoin (car rjoin garage) e (person rjoin (car rjoin garage)).
            // Solução: Nesse caso basta gerar apenas uma operação (person rjoin (car rjoin garage)).
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOne" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneComputedEntityMultiple()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-computed-entity-multiple.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne-Multiple.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) select *";
            // FIXED?
            // Consulta: from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) select *
            // Problema: Parser gera duas operações (car rjoin garage) e (person rjoin (car rjoin garage)) a primeira operação não é necessária, e a segunda não incluiu a entidade Supplier na junção.
            // Solução: Junções aninhadas devem gerar apenas uma operação, nesse exemplo a junção entre Car, Garage e Supplier é representada por uma entidade computada.
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOneMultiple" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneComputedEntityMultiple2()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-computed-entity-multiple-2.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne-Multiple2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select *";
            // FIXED?
            // Consulta: from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select *
            // Problema: Neste existem dois problemas, o primeiro é o mesmo do exemplo anterior (Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier))) e o segundo problema é ao gerar a junção de Insurance com o resultado da junção anterior.
            // Solução: Na situação atual do algoritmo quando houver várias junções de mesmo nível, ou seja, quando o resultado de uma junção é o ponto de partida para a próxima junção, pode-se usar a entidade Person (a mesma do argumento From). Uma das limitações do algoritmo é a falta de suporte para herança de relacionamentos (da forma que foi proposto na álgebra) e por isso as junções subsequentes devem ser entre entidades relacionadas a entidade de entrada.
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOneMultiple2" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-many-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage)) select *";
            // FIXED?
            // Consulta: from person rjoin <Drives> (Car rjoin <Repaired> (Garage)) select *
            // Problema: Parser gera duas operações rjoin (car rjoin garage) e (person rjoin (car rjoin garage)).
            // Solução: Nesse caso basta gerar apenas uma operação (person rjoin (car rjoin garage)).
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyComputedEntityMultipleEntities()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-many-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToMany-2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select *";
            // FIXED?
            // Consulta: from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select *
            // Problema: Neste existem dois problemas, o primeiro é o mesmo do exemplo anterior (Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier))) e o segundo problema é ao gerar a junção de Insurance com o resultado da junção anterior.
            // Solução: Na situação atual do algoritmo quando houver várias junções de mesmo nível, ou seja, quando o resultado de uma junção é o ponto de partida para a próxima junção, pode-se usar a entidade Person (a mesma do argumento From). Uma das limitações do algoritmo é a falta de suporte para herança de relacionamentos (da forma que foi proposto na álgebra) e por isso as junções subsequentes devem ser entre entidades relacionadas a entidade de entrada.
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyComputedEntityMultipleEntities()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceManyToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Owns> (Car rjoin <Repaired> (Garage, Supplier), Insurance) select *";
            // FIXED?
            // Consulta: from Person rjoin <Owns> (Car rjoin <Repaired> (Garage, Supplier), Insurance) select *
            // Comentário: Nesse caso o parser gerou uma operação rjoin entre Car e Garage (mesmo problema dos exemplos anteriores) e também gerou uma junção entre Person, (Car e Garage) e Insurance, a última operação incluiu multiplas entidades apenas falhou ao não incluir Supplier na junção com Car e Garage.            
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceManyToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyComputedEntityOneToOne()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-computed-entity-2.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceManyToMany-2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person rjoin <Owns> (Car rjoin <ManufacturedBy> (Manufacturer)) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );
            // FIXED?

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceManyToMany2" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
    }
}
