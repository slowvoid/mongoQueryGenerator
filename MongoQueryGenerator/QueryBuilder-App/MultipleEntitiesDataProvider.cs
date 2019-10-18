using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;

namespace QueryBuilderApp
{
    public static class MultipleEntitiesDataProvider
    {
        public static ERModel CreateModel()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "age", "name", "petId", "jobId" );

            Entity Pet = new Entity( "Pet" );
            Pet.AddAttributes( "petId", "name" );

            Relationship HasPet = new Relationship( "HasPet", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonHasPet = new RelationshipConnection(
                Person,
                Person.GetAttribute( "petId" ),
                Pet,
                Pet.GetAttribute( "petId" ) );
            HasPet.AddRelation( PersonHasPet );

            Entity Job = new Entity( "Job" );
            Job.AddAttributes( "jobId", "name" );

            Relationship HasJob = new Relationship( "HasJob", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonHasJob = new RelationshipConnection(
                Person,
                Person.GetAttribute( "jobId" ),
                Job,
                Job.GetAttribute( "jobId" ) );
            HasJob.AddRelation( PersonHasJob );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model" );

            Entity Company = new Entity( "Company" );
            Company.AddAttributes( "companyId", "name" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.ManyToMany );
            Drives.AddAttributes( "personId", "carId", "companyId" );
            RelationshipConnection PersonDrivesCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Drives.GetAttribute( "personId" ),
                Car,
                Car.GetAttribute( "carId" ),
                Drives.GetAttribute( "carId" ) );
            Drives.AddRelation( PersonDrivesCar );

            RelationshipConnection PersonDrivesCompany = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Drives.GetAttribute( "personId" ),
                Company,
                Company.GetAttribute( "companyId" ),
                Drives.GetAttribute( "companyId" ) );
            Drives.AddRelation( PersonDrivesCompany );

            Entity Contract = new Entity( "Contract" );
            Contract.AddAttributes( "contractId", "name" );

            Relationship HasContract = new Relationship( "HasContract", RelationshipCardinality.ManyToMany );
            HasContract.AddAttributes( "personId", "contractId", "companyId" );
            RelationshipConnection PersonHasContract = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                HasContract.GetAttribute( "personId" ),
                Contract,
                Contract.GetAttribute( "contractId" ),
                HasContract.GetAttribute( "contractId" ) );
            HasContract.AddRelation( PersonHasContract );

            RelationshipConnection PersonHasCompany = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                HasContract.GetAttribute( "personId" ),
                Company,
                Company.GetAttribute( "companyId" ),
                HasContract.GetAttribute( "companyId" ) );
            HasContract.AddRelation( PersonHasCompany );

            ERModel Model = new ERModel( "Model", new List<BaseERElement>() { Person, Pet, Job,
                HasPet, HasJob, Car, Company, Drives, Contract, HasContract} );
            return Model;
        }

        public static MongoSchema CreateSchema()
        {
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "age", "name", "petId", "jobId" );

            MongoDBCollection PetCol = new MongoDBCollection( "Pet" );
            PetCol.AddAttributes( "_id", "name" );

            MongoDBCollection JobCol = new MongoDBCollection( "Job" );
            JobCol.AddAttributes( "_id", "name" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model" );

            MongoDBCollection CompanyCol = new MongoDBCollection( "Company" );
            CompanyCol.AddAttributes( "_id", "name" );

            MongoDBCollection DrivesCol = new MongoDBCollection( "Drives" );
            DrivesCol.AddAttributes( "_id", "personId", "carId", "companyId" );

            MongoDBCollection ContractCol = new MongoDBCollection( "Contract" );
            ContractCol.AddAttributes( "_id", "name" );

            MongoDBCollection HasContractCol = new MongoDBCollection( "HasContract" );
            HasContractCol.AddAttributes( "_id", "personId", "contractId", "companyId" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection>() { PersonCol, PetCol, JobCol,
                CarCol, CompanyCol, DrivesCol, ContractCol, HasContractCol } );
            return Schema;
        }

        public static ModelMapping CreateMap(ERModel Model, MongoSchema Schema)
        {
            MapRule PersonRules = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRules.AddRule( "personId", "_id" );
            PersonRules.AddRule( "age", "age" );
            PersonRules.AddRule( "name", "name" );
            PersonRules.AddRule( "petId", "petId" );
            PersonRules.AddRule( "jobId", "jobId" );

            MapRule PetRules = new MapRule( Model.FindByName( "Pet" ), Schema.FindByName( "Pet" ) );
            PetRules.AddRule( "petId", "_id" );
            PetRules.AddRule( "name", "name" );

            MapRule JobRules = new MapRule( Model.FindByName( "Job" ), Schema.FindByName( "Job" ) );
            JobRules.AddRule( "jobId", "_id" );
            JobRules.AddRule( "name", "name" );

            MapRule CarRules = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRules.AddRule( "carId", "_id" );
            CarRules.AddRule( "model", "model" );

            MapRule CompanyRules = new MapRule( Model.FindByName( "Company" ), Schema.FindByName( "Company" ) );
            CompanyRules.AddRule( "companyId", "_id" );
            CompanyRules.AddRule( "name", "name" );

            MapRule DrivesCarsRules = new MapRule( Model.FindByName( "Drives" ), Schema.FindByName( "Drives" ) );
            DrivesCarsRules.AddRule( "personId", "personId" );
            DrivesCarsRules.AddRule( "carId", "carId" );
            DrivesCarsRules.AddRule( "companyId", "companyId" );

            MapRule ContractRules = new MapRule( Model.FindByName( "Contract" ), Schema.FindByName( "Contract" ) );
            ContractRules.AddRule( "contractId", "_id" );
            ContractRules.AddRule( "name", "name" );

            MapRule HasContractRules = new MapRule( Model.FindByName( "HasContract" ), Schema.FindByName( "HasContract" ) );
            HasContractRules.AddRule( "personId", "personId" );
            HasContractRules.AddRule( "contractId", "contractId" );
            HasContractRules.AddRule( "companyId", "companyId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule>() { PersonRules, PetRules, JobRules,
                CarRules, CompanyRules, DrivesCarsRules, ContractRules, HasContractRules } );
            return Map;
        }
    }
}