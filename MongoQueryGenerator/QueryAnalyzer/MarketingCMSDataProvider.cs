using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryAnalyzer
{
    /// <summary>
    /// Provides data for MarketingCMS tests
    /// </summary>
    public static class MarketingCMSDataProvider
    {
        private static ERModel StoredModel { get; set; }

        public static ERModel GetERModel()
        {
            if ( StoredModel == null )
            {
                StoredModel = CreateERModel();
            }

            return StoredModel;
        }
        /// <summary>
        /// Create ER Model
        /// </summary>
        /// <returns></returns>
        public static ERModel CreateERModel()
        {
            Entity User = new Entity( "User" );
            User.AddAttribute( "UserID", true );
            User.AddAttributes( "UserName", "UserEmail" );

            Entity Store = new Entity( "Store" );
            Store.AddAttribute( "StoreID", true );
            Store.AddAttributes( "StoreName" );

            Entity Category = new Entity( "Category" );
            Category.AddAttribute( "CategoryID", true );
            Category.AddAttributes( "CategoryName" );

            Entity Product = new Entity( "Product" );
            Product.AddAttribute( "ProductID", true );
            Product.AddAttributes( "Title", "Description", "Price" );

            Relationship UserProducts = new Relationship( "UserProducts" );
            UserProducts.AddRelationshipEnd( new RelationshipEnd( User ) );
            UserProducts.AddRelationshipEnd( new RelationshipEnd( Product ) );

            Relationship StoreProducts = new Relationship( "StoreProducts" );
            StoreProducts.AddRelationshipEnd( new RelationshipEnd( Store ) );
            StoreProducts.AddRelationshipEnd( new RelationshipEnd( Product ) );

            Relationship CategoryProducts = new Relationship( "CategoryProducts" );
            CategoryProducts.AddRelationshipEnd( new RelationshipEnd( Category ) );
            CategoryProducts.AddRelationshipEnd( new RelationshipEnd( Product ) );

            return new ERModel( "CMSModel", new List<BaseERElement>() { User, Store, Category, Product, UserProducts, StoreProducts, CategoryProducts } );
        }
        /// <summary>
        /// Maps entities to collection and return all data (ERModel, MongoSchema and ModelMapping)
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer MapEntitiesToCollections()
        {
            // Create schema
            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "UserName", "UserEmail" );

            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "Title", "Description", "CategoryID", "StoreID", "UserID", "Price" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "CategoryName" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "StoreName" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { UserCol, ProductCol,
                StoreCol, CategoryCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, UserCol );
            UserRules.AddRule( "UserID", "_id" );
            UserRules.AddRule( "UserName", "UserName" );
            UserRules.AddRule( "UserEmail", "UserEmail" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "ProductID", "_id" );
            ProductRules.AddRule( "Title", "Title" );
            ProductRules.AddRule( "Description", "Description" );
            ProductRules.AddRule( "Price", "Price" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, CategoryCol );
            CategoryRules.AddRule( "CategoryID", "_id" );
            CategoryRules.AddRule( "CategoryName", "CategoryName" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, StoreCol );
            StoreRules.AddRule( "StoreID", "_id" );
            StoreRules.AddRule( "StoreName", "StoreName" );

            MapRule UserProductRule = new MapRule( User, ProductCol, false );
            UserProductRule.AddRule( "UserID", "UserID" );

            MapRule StoreProductRule = new MapRule( Store, ProductCol, false );
            StoreProductRule.AddRule( "StoreID", "StoreID" );

            MapRule CategoryProductRule = new MapRule( Category, ProductCol, false );
            CategoryProductRule.AddRule( "CategoryID", "CategoryID" );

            ModelMapping Map = new ModelMapping( "CMSMap11", new List<MapRule>() { UserRules,
                ProductRules, CategoryRules, StoreRules, UserProductRule, StoreProductRule, CategoryProductRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionDuplicates()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "Title", "Description", "Price",
                "user._id", "user.name", "user.email",
                "category._id", "category.name",
                "store._id", "store.name" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "UserName", "UserEmail" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "CategoryName" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "StoreName" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, ProductCol, false );
            UserRules.AddRule( "UserID", "user._id" );
            UserRules.AddRule( "UserName", "user.name" );
            UserRules.AddRule( "UserEmail", "user.email" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "UserID", "_id" );
            UserRulesMain.AddRule( "UserName", "UserName" );
            UserRulesMain.AddRule( "UserEmail", "UserEmail" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "ProductID", "_id" );
            ProductRules.AddRule( "Title", "Title" );
            ProductRules.AddRule( "Description", "Description" );
            ProductRules.AddRule( "Price", "Price" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, ProductCol, false );
            CategoryRules.AddRule( "CategoryID", "category._id" );
            CategoryRules.AddRule( "CategoryName", "category.name" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "CategoryID", "_id" );
            CategoryRulesMain.AddRule( "CategoryName", "CategoryName" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, ProductCol, false );
            StoreRules.AddRule( "StoreID", "store._id" );
            StoreRules.AddRule( "StoreName", "store.name" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "StoreID", "_id" );
            StoreRulesMain.AddRule( "StoreName", "StoreName" );

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRules,
                ProductRules, CategoryRules, StoreRules } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionCategoryDuplicated()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "Title", "Description", "Price",
                "StoreID", "UserID",
                "category._id", "category.name" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "UserName", "UserEmail" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "CategoryName" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "StoreName" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "UserID", "_id" );
            UserRulesMain.AddRule( "UserName", "UserName" );
            UserRulesMain.AddRule( "UserEmail", "UserEmail" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "ProductID", "_id" );
            ProductRules.AddRule( "Title", "Title" );
            ProductRules.AddRule( "Description", "Description" );
            ProductRules.AddRule( "Price", "Price" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, ProductCol, false );
            CategoryRules.AddRule( "CategoryID", "category._id" );
            CategoryRules.AddRule( "CategoryName", "category.name" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "CategoryID", "_id" );
            CategoryRulesMain.AddRule( "CategoryName", "CategoryName" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "StoreID", "_id" );
            StoreRulesMain.AddRule( "StoreName", "StoreName" );

            MapRule UserProductRule = new MapRule( User, ProductCol, false );
            UserProductRule.AddRule( "UserID", "UserID" );

            MapRule StoreProductRule = new MapRule( Store, ProductCol, false );
            StoreProductRule.AddRule( "StoreID", "StoreID" );

            ModelMapping Map = new ModelMapping( "CMSCategoryDuplicates", new List<MapRule>() { UserRulesMain,
                ProductRules, CategoryRules, StoreRulesMain, UserProductRule, StoreProductRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsStoreDuplicated()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "Title", "Description", "Price",
                "CategoryID", "UserID",
                "store._id", "store.name" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "UserName", "UserEmail" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "CategoryName" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "StoreName" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "UserID", "_id" );
            UserRulesMain.AddRule( "UserName", "UserName" );
            UserRulesMain.AddRule( "UserEmail", "UserEmail" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "ProductID", "_id" );
            ProductRules.AddRule( "Title", "Title" );
            ProductRules.AddRule( "Description", "Description" );
            ProductRules.AddRule( "Price", "Price" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "CategoryID", "_id" );
            CategoryRulesMain.AddRule( "CategoryName", "CategoryName" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, ProductCol, false );
            StoreRules.AddRule( "StoreID", "store._id" );
            StoreRules.AddRule( "StoreName", "store.name" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "StoreID", "_id" );
            StoreRulesMain.AddRule( "StoreName", "StoreName" );

            MapRule UserProductRule = new MapRule( User, ProductCol, false );
            UserProductRule.AddRule( "UserID", "UserID" );

            MapRule CategoryProductRule = new MapRule( Category, ProductCol, false );
            CategoryProductRule.AddRule( "CategoryID", "CategoryID" );

            ModelMapping Map = new ModelMapping( "CMSStoreDuplicates", new List<MapRule>() { UserRulesMain,
                ProductRules, CategoryRulesMain, StoreRules, UserProductRule, CategoryProductRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsUserDuplicated()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "Title", "Description", "Price",
                "CategoryID", "StoreID",
                "user._id", "user.name", "user.email" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "UserName", "UserEmail" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "CategoryName" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "StoreName" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, ProductCol, false );
            UserRules.AddRule( "UserID", "user._id" );
            UserRules.AddRule( "UserName", "user.name" );
            UserRules.AddRule( "UserEmail", "user.email" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "UserID", "_id" );
            UserRulesMain.AddRule( "UserName", "UserName" );
            UserRulesMain.AddRule( "UserEmail", "UserEmail" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "ProductID", "_id" );
            ProductRules.AddRule( "Title", "Title" );
            ProductRules.AddRule( "Description", "Description" );
            ProductRules.AddRule( "Price", "Price" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "CategoryID", "_id" );
            CategoryRulesMain.AddRule( "CategoryName", "CategoryName" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "StoreID", "_id" );
            StoreRulesMain.AddRule( "StoreName", "StoreName" );

            MapRule StoreProductRule = new MapRule( Store, ProductCol, false );
            StoreProductRule.AddRule( "StoreID", "StoreID" );

            MapRule CategoryProductRule = new MapRule( Category, ProductCol, false );
            CategoryProductRule.AddRule( "CategoryID", "CategoryID" );

            ModelMapping Map = new ModelMapping( "CMSUserDuplicates", new List<MapRule>() { UserRules,
                ProductRules, CategoryRulesMain, StoreRulesMain, StoreProductRule, CategoryProductRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}