﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class VirtualMapTests
    {
        [TestMethod]
        public void RelationshipJoin()
        {
            //RequiredDataContainer ModelData = VirtualMapDataProvider.VirtualMapModel();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/virtual-map.mapping" ) );

            QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Pet = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Pet" ) );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator( Person,
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasPet" ),
                new List<QueryableEntity>() { Pet },
                ModelData.ERMongoMapping );

            VirtualMap RJoinVMap = RJoinOp.ComputeVirtualMap();
            List<string> VirtualMapRules = RJoinVMap.GetRulesAsStringList();

            Assert.IsNotNull( RJoinVMap, "Virtual map cannot be null" );

            List<string> RulesToMatch = new List<string>()
            {
                "_id",
                "name",
                "age",
                "data_HasPet.Pet_petId",
                "data_HasPet.Pet_name",
                "data_HasPet.Pet_type",
                //"data_HasPet.Pet_ownerId"
            };

            Assert.IsTrue( !VirtualMapRules.Except( RulesToMatch ).Any() && !RulesToMatch.Except( VirtualMapRules ).Any() );
        }
        [TestMethod]
        public void ProjectStageShowFields()
        {
            //RequiredDataContainer ModelData = VirtualMapDataProvider.VirtualMapModel();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/virtual-map.mapping" ) );

            QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Pet = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Pet" ) );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                Person,
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasPet" ),
                new List<QueryableEntity>() { Pet },
                ModelData.ERMongoMapping );

            VirtualMap RJoinVMap = RJoinOp.ComputeVirtualMap();
            Assert.IsNotNull( RJoinVMap, "RJOIN Virtual map cannot be null" );

            List<ProjectArgument> ProjectArguments = new List<ProjectArgument>();
            ProjectArguments.Add( new ProjectArgument( Person.GetAttribute( "name" ), Person, new BooleanExpr( true ) ) );
            ProjectArguments.Add( new ProjectArgument( Person.GetAttribute( "age" ), Person, new BooleanExpr( true ) ) );
            ProjectArguments.Add( new ProjectArgument( Pet.GetAttribute( "name" ), Pet, new BooleanExpr( true ) ) );
    
            ProjectStage ProjectOp = new ProjectStage( ProjectArguments, RJoinVMap );

            VirtualMap ProjectVirtualMap = ProjectOp.ComputeVirtualMap( RJoinVMap );
            Assert.IsNotNull( ProjectVirtualMap, "Project virtual map cannot be null" );

            List<string> VirtualMapRules = ProjectVirtualMap.GetRulesAsStringList();

            List<string> RulesToMatch = new List<string>()
            {
                "name",
                "age",
                "data_HasPet.Pet_name",
            };

            Assert.IsTrue( !VirtualMapRules.Except( RulesToMatch ).Any() && !RulesToMatch.Except( VirtualMapRules ).Any(), "Virtual maps do not match" );
        }
        [TestMethod]
        public void ProjectStageHideFields()
        {
            //RequiredDataContainer ModelData = VirtualMapDataProvider.VirtualMapModel();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/virtual-map.mapping" ) );

            QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Pet = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Pet" ) );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                Person,
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasPet" ),
                new List<QueryableEntity>() { Pet },
                ModelData.ERMongoMapping );

            VirtualMap RJoinVMap = RJoinOp.ComputeVirtualMap();
            Assert.IsNotNull( RJoinVMap, "RJOIN Virtual map cannot be null" );

            List<ProjectArgument> ProjectArguments = new List<ProjectArgument>();
            ProjectArguments.Add( new ProjectArgument( Person.GetAttribute( "name" ), Person, new BooleanExpr( false ) ) );
            ProjectArguments.Add( new ProjectArgument( Pet.GetAttribute( "type" ), Pet, new BooleanExpr( true ) ) );
            //ProjectArguments.Add( new ProjectArgument( Pet.GetAttribute( "ownerId" ), Pet, new BooleanExpr( true ) ) );

            ProjectStage ProjectOp = new ProjectStage( ProjectArguments, RJoinVMap );

            VirtualMap ProjectVirtualMap = ProjectOp.ComputeVirtualMap( RJoinVMap );
            Assert.IsNotNull( ProjectVirtualMap, "Project virtual map cannot be null" );

            List<string> VirtualMapRules = ProjectVirtualMap.GetRulesAsStringList();

            List<string> RulesToMatch = new List<string>()
            {
                "data_HasPet.Pet_type",
                //"data_HasPet.Pet_ownerId"
            };

            Assert.IsTrue( !VirtualMapRules.Except( RulesToMatch ).Any() && !RulesToMatch.Except( VirtualMapRules ).Any(), "Virtual maps do not match" );
        }
        [TestMethod]
        public void CartesianProduct()
        {
            //RequiredDataContainer ModelData = VirtualMapDataProvider.VirtualMapModel();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/virtual-map.mapping" ) );

            QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Pet = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Pet" ) );

            CartesianProductOperator Op = new CartesianProductOperator( Person, Pet, ModelData.ERMongoMapping );
            VirtualMap CartersianVMap = Op.ComputeVirtualMap();

            Assert.IsNotNull( CartersianVMap, "Cartesian Product virtual map cannot be null" );

            List<string> VirtualMapRules = CartersianVMap.GetRulesAsStringList();

            List<string> RulesToMatch = new List<string>()
            {
                "_id",
                "name",
                "age",
                "data_Pet._id",
                "data_Pet.name",
                "data_Pet.type",
                //"data_Pet.ownerId",
            };

            Assert.IsTrue( !VirtualMapRules.Except( RulesToMatch ).Any() && !RulesToMatch.Except( VirtualMapRules ).Any(), "Virtual maps do not match" );
        }
    }
}