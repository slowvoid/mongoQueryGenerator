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
                Console.WriteLine(q.query);
                Console.WriteLine(QueryGen.SummarizeToString());
            }
        }
    }
}