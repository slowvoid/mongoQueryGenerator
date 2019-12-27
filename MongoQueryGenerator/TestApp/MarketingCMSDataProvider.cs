using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace TestApp
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
            User.AddAttribute( "user_id", true );
            User.AddAttributes( "user_name", "user_email", "user_access", "user_newsletter" );

            Entity Store = new Entity( "Store" );
            Store.AddAttribute( "store_id", true );
            Store.AddAttributes( "store_name", "store_logo" );

            Entity Category = new Entity( "Category" );
            Category.AddAttribute( "category_id", true );
            Category.AddAttributes( "category_name" );

            Entity Product = new Entity( "Product" );
            Product.AddAttribute( "product_id", true );
            Product.AddAttributes( "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image" );

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
        public static DataContainer MapEntitiesToCollections()
        {
            // Create schema
            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "user_name", "user_email", "user_access", "user_newsletter" );

            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "category_name" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "store_name", "store_logo" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { UserCol, ProductCol,
                StoreCol, CategoryCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, UserCol );
            UserRules.AddRule( "user_id", "_id" );
            UserRules.AddRule( "user_name", "user_name" );
            UserRules.AddRule( "user_email", "user_email" );
            UserRules.AddRule( "user_access", "user_access" );
            UserRules.AddRule( "user_newsletter", "user_newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "product_title" );
            ProductRules.AddRule( "product_description", "product_description" );
            ProductRules.AddRule( "product_short_description", "product_short_description" );
            ProductRules.AddRule( "product_url", "product_url" );
            ProductRules.AddRule( "product_published", "product_published" );
            ProductRules.AddRule( "product_image", "product_image" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, CategoryCol );
            CategoryRules.AddRule( "category_id", "_id" );
            CategoryRules.AddRule( "category_name", "category_name" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, StoreCol );
            StoreRules.AddRule( "store_id", "_id" );
            StoreRules.AddRule( "store_name", "store_name" );
            StoreRules.AddRule( "store_logo", "store_logo" );

            MapRule UserProductRule = new MapRule( User, ProductCol, false );
            UserProductRule.AddRule( "user_id", "product_user_id" );

            MapRule StoreProductRule = new MapRule( Store, ProductCol, false );
            StoreProductRule.AddRule( "store_id", "product_store_id" );

            MapRule CategoryProductRule = new MapRule( Category, ProductCol, false );
            CategoryProductRule.AddRule( "category_id", "product_category_id" );

            ModelMapping Map = new ModelMapping( "CMSMap11", new List<MapRule>() { UserRules,
                ProductRules, CategoryRules, StoreRules, UserProductRule, StoreProductRule, CategoryProductRule } );

            return new DataContainer( Model, Schema, Map );
        }

        public static DataContainer MapEntitiesToCollectionDuplicates()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image",
                "user._id", "user._name", "user.email", "user.access", "user.newsletter",
                "category._id", "category.name",
                "store._id", "store.name", "store.logo" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "user_name", "user_email", "user_access", "user_registered_at", "user_newsletter" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "category_name" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "store_name", "store_logo" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, ProductCol, false );
            UserRules.AddRule( "user_id", "user._id" );
            UserRules.AddRule( "user_name", "user.name" );
            UserRules.AddRule( "user_email", "user.email" );
            UserRules.AddRule( "user_access", "user.access" );
            UserRules.AddRule( "user_newsletter", "user.newsletter" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "user_id", "_id" );
            UserRulesMain.AddRule( "user_name", "user_name" );
            UserRulesMain.AddRule( "user_email", "user_email" );
            UserRulesMain.AddRule( "user_access", "user_access" );
            UserRulesMain.AddRule( "user_newsletter", "user_newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "product_title" );
            ProductRules.AddRule( "product_description", "product_description" );
            ProductRules.AddRule( "product_short_description", "product_short_description" );
            ProductRules.AddRule( "product_url", "product_url" );
            ProductRules.AddRule( "product_published", "product_published" );
            ProductRules.AddRule( "product_image", "product_image" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, ProductCol, false );
            CategoryRules.AddRule( "category_id", "category._id" );
            CategoryRules.AddRule( "category_name", "category.name" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "category_id", "_id" );
            CategoryRulesMain.AddRule( "category_name", "category_name" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, ProductCol, false );
            StoreRules.AddRule( "store_id", "store._id" );
            StoreRules.AddRule( "store_name", "store.name" );
            StoreRules.AddRule( "store_logo", "store.logo" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "store_id", "_id" );
            StoreRulesMain.AddRule( "store_name", "store_name" );
            StoreRulesMain.AddRule( "store_logo", "store_logo" );

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRules,
                ProductRules, CategoryRules, StoreRules } );

            return new DataContainer( Model, Schema, Map );
        }

        public static DataContainer MapEntitiesToCollectionCategoryDuplicated()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image",
                "category._id", "category.name" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "user_name", "user_email", "user_access", "user_newsletter" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "category_name" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "store_name", "store_logo" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "user_id", "_id" );
            UserRulesMain.AddRule( "user_name", "user_name" );
            UserRulesMain.AddRule( "user_email", "user_email" );
            UserRulesMain.AddRule( "user_access", "user_access" );
            UserRulesMain.AddRule( "user_newsletter", "user_newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "product_title" );
            ProductRules.AddRule( "product_description", "product_description" );
            ProductRules.AddRule( "product_short_description", "product_short_description" );
            ProductRules.AddRule( "product_url", "product_url" );
            ProductRules.AddRule( "product_published", "product_published" );
            ProductRules.AddRule( "product_image", "product_image" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, ProductCol, false );
            CategoryRules.AddRule( "category_id", "category._id" );
            CategoryRules.AddRule( "category_name", "category.name" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "category_id", "_id" );
            CategoryRulesMain.AddRule( "category_name", "category_name" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "store_id", "_id" );
            StoreRulesMain.AddRule( "store_name", "store_name" );
            StoreRulesMain.AddRule( "store_logo", "store_logo" );

            MapRule UserProductRule = new MapRule( User, ProductCol, false );
            UserProductRule.AddRule( "user_id", "product_user_id" );

            MapRule StoreProductRule = new MapRule( Store, ProductCol, false );
            StoreProductRule.AddRule( "store_id", "product_store_id" );

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRulesMain,
                ProductRules, CategoryRules, StoreRulesMain, UserProductRule, StoreProductRule } );

            return new DataContainer( Model, Schema, Map );
        }

        public static DataContainer MapEntitiesToCollectionsStoreDuplicated()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image",
                "store._id", "store.name", "store.logo" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "user_name", "user_email", "user_access", "user_newsletter" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "category_name" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "store_name", "store_logo" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "user_id", "_id" );
            UserRulesMain.AddRule( "user_name", "user_name" );
            UserRulesMain.AddRule( "user_email", "user_email" );
            UserRulesMain.AddRule( "user_access", "user_access" );
            UserRulesMain.AddRule( "user_newsletter", "user_newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "product_title" );
            ProductRules.AddRule( "product_description", "product_description" );
            ProductRules.AddRule( "product_short_description", "product_short_description" );
            ProductRules.AddRule( "product_url", "product_url" );
            ProductRules.AddRule( "product_published", "product_published" );
            ProductRules.AddRule( "product_image", "product_image" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "category_id", "_id" );
            CategoryRulesMain.AddRule( "category_name", "category_name" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, ProductCol, false );
            StoreRules.AddRule( "store_id", "store._id" );
            StoreRules.AddRule( "store_name", "store.name" );
            StoreRules.AddRule( "store_logo", "store.logo" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "store_id", "_id" );
            StoreRulesMain.AddRule( "store_name", "store_name" );
            StoreRulesMain.AddRule( "store_logo", "store_logo" );

            MapRule UserProductRule = new MapRule( User, ProductCol, false );
            UserProductRule.AddRule( "user_id", "product_user_id" );

            MapRule CategoryProductRule = new MapRule( Category, ProductCol, false );
            CategoryProductRule.AddRule( "category_id", "product_category_id" );

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRulesMain,
                ProductRules, CategoryRulesMain, StoreRules, UserProductRule, CategoryProductRule } );

            return new DataContainer( Model, Schema, Map );
        }

        public static DataContainer MapEntitiesToCollectionsUserDuplicated()
        {
            // Create Schema
            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image",
                "user._id", "user._name", "user.email", "user.access", "user.newsletter" );

            MongoDBCollection UserCol = new MongoDBCollection( "User" );
            UserCol.AddAttributes( "_id", "user_name", "user_email", "user_access", "user_registered_at", "user_newsletter" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "category_name" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "store_name", "store_logo" );

            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol, UserCol, CategoryCol, StoreCol } );

            // Retrieve ER Model
            ERModel Model = GetERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, ProductCol, false );
            UserRules.AddRule( "user_id", "user._id" );
            UserRules.AddRule( "user_name", "user.name" );
            UserRules.AddRule( "user_email", "user.email" );
            UserRules.AddRule( "user_access", "user.access" );
            UserRules.AddRule( "user_newsletter", "user.newsletter" );

            MapRule UserRulesMain = new MapRule( User, UserCol );
            UserRulesMain.AddRule( "user_id", "_id" );
            UserRulesMain.AddRule( "user_name", "user_name" );
            UserRulesMain.AddRule( "user_email", "user_email" );
            UserRulesMain.AddRule( "user_access", "user_access" );
            UserRulesMain.AddRule( "user_newsletter", "user_newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "product_title" );
            ProductRules.AddRule( "product_description", "product_description" );
            ProductRules.AddRule( "product_short_description", "product_short_description" );
            ProductRules.AddRule( "product_url", "product_url" );
            ProductRules.AddRule( "product_published", "product_published" );
            ProductRules.AddRule( "product_image", "product_image" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRulesMain = new MapRule( Category, CategoryCol );
            CategoryRulesMain.AddRule( "category_id", "_id" );
            CategoryRulesMain.AddRule( "category_name", "category_name" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRulesMain = new MapRule( Store, StoreCol );
            StoreRulesMain.AddRule( "store_id", "_id" );
            StoreRulesMain.AddRule( "store_name", "store_name" );
            StoreRulesMain.AddRule( "store_logo", "store_logo" );

            MapRule StoreProductRule = new MapRule( Store, ProductCol, false );
            StoreProductRule.AddRule( "store_id", "product_store_id" );

            MapRule CategoryProductRule = new MapRule( Category, ProductCol, false );
            CategoryProductRule.AddRule( "category_id", "product_category_id" );

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRules,
                ProductRules, CategoryRulesMain, StoreRulesMain, StoreProductRule, CategoryProductRule } );

            return new DataContainer( Model, Schema, Map );
        }
    }
}