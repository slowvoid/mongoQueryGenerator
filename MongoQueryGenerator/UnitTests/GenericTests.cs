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
    public class GenericTests
    {
        [TestMethod]
        public void TestComputedEntities()
        {

            (string query, string mapping)[] queries = {
                 ( "from Person rjoin <Drives> (Car,Garage) select *",
                   "Mappings/one-to-one-computed-entity.mapping"),
                 ( "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage)) select *",
                   "Mappings/one-to-one-computed-entity.mapping"),
                 ( "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) select *",
                   "Mappings/one-to-one-computed-entity-multiple.mapping" ),
                 ( "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select *",
                   "Mappings/one-to-one-computed-entity-multiple-2.mapping"),
                 ( "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage)) select *",
                   "Mappings/one-to-many-computed-entity.mapping"),
                 ( "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select *",
                   "Mappings/one-to-many-computed-entity.mapping"),
                 ( "from Person rjoin <Owns> (Car rjoin <Repaired> (Garage, Supplier), Insurance) select *",
                   "Mappings/many-to-many-computed-entity.mapping"),
                 ( "from Person rjoin <Owns> (Car rjoin <ManufacturedBy> (Manufacturer)) select *",
                   "Mappings/many-to-many-computed-entity-2.mapping"),
                 ( "from Person rjoin <HasInsurance> (Car, Insurance) select *",
                   "Mappings/one-to-one-not-embedded-multiple-entities.mapping"),
                 ( "from Person rjoin <HasInsurance> (Car, Insurance) select *",
                   "Mappings/one-to-one-multiple-entities-mixed.mapping"),
                 ( "from Person rjoin <HasInsurance> (Car, Insurance) select *",
                   "Mappings/one-to-one-relationship-attributes.mapping") };

            foreach ((string query, string mapping) q in queries)
            {
                var ModelData = QueryBuilderParser.ParseMapping(Utils.ReadMappingFile(q.mapping));
                QueryGenerator QueryGen = QueryBuilderParser.ParseQuery(q.query, ModelData);
                Console.WriteLine("");
                Console.WriteLine(q.query);
                Console.WriteLine(QueryGen.SummarizeToString());
            }
        }

        [TestMethod]
        public void TestProject()
        {

            (string query, string mapping)[] queries = {
                 ( "from Person select Person.name, Person.age",
                   "Mappings/project-simple.mapping"),
                 ( "from Person rjoin <Drives> (Car,Garage) select Person.name, Car.model, Car.year, Garage.name",
                   "Mappings/one-to-one-computed-entity.mapping"),
                 ( "from Person rjoin <Drives> (Car rjoin <Repaired> (Garage, Supplier)) rjoin <HasInsurance> (Insurance) select Insurance.name, Insurance.value, Supplier.name, Person.personId",
                   "Mappings/one-to-one-computed-entity-multiple-2.mapping"),
                 ( "from Person rjoin <Owns> (Car rjoin <ManufacturedBy> (Manufacturer)) select Person.name, Car.model, Car.year, Manufacturer.name",
                   "Mappings/project-computed-entity.mapping") };

            foreach ((string query, string mapping) q in queries)
            {
                var ModelData = QueryBuilderParser.ParseMapping(Utils.ReadMappingFile(q.mapping));
                QueryGenerator QueryGen = QueryBuilderParser.ParseQuery(q.query, ModelData);
                Console.WriteLine("");
                Console.WriteLine(q.query);
                Console.WriteLine(QueryGen.SummarizeToString());
            }
        }

        [TestMethod]
        public void TestSelect()
        {

            (string query, string mapping)[] queries = {
                 ( "from Person select * where Person.age = 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age between 10 and 20",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age = 27 and Person.name = 'Summer'",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age >= 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age > 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age in (26,27,28,29)",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age <= 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age < 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age <> 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age not in (26,27,28,29)",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age = 26 or Person.age = 27",
                   "Mappings/select.mapping" ),
                 ( "from Person select * where Person.age = 18 or Person.age = 21 or Person.age = 36",
                   "Mappings/select.mapping" )
            };

            foreach ((string query, string mapping) q in queries)
            {
                Console.Error.WriteLine("");
                Console.Error.WriteLine(q.query);
                var ModelData = QueryBuilderParser.ParseMapping(Utils.ReadMappingFile(q.mapping));
                QueryGenerator QueryGen = QueryBuilderParser.ParseQuery(q.query, ModelData);
                Console.Error.WriteLine(QueryGen.SummarizeToString());
            }
        }
    }
}