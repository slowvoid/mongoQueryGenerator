using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryBuilderApp
{
    public static class MarketingCMSDataProvider
    {
        public static ERModel CreateERModel()
        {
            Entity User = new Entity( "User" );
            User.AddAttributes( "user_id", "user_name", "user_email", "user_access", "user_newsletter" );

            Entity Store = new Entity( "Store" );
            Store.AddAttributes( "store_id", "store_name", "store_logo" );

            Entity Category = new Entity( "Category" );
            Category.AddAttributes( "category_id", "category_name" );

            Entity Product = new Entity( "Product" );
            Product.AddAttributes( "product_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image" );

            // User Relationships
            Relationship UserHasManyProducts = new Relationship( "HasManyProducts", RelationshipCardinality.OneToMany );
            RelationshipConnection UserHasManyProductsConnection = new RelationshipConnection(
                User,
                User.GetAttribute( "user_id" ),
                Product,
                Product.GetAttribute( "product_user_id" ) );

            UserHasManyProducts.AddRelation( UserHasManyProductsConnection );

            // Store relationships
            Relationship StoreHasManyProducts = new Relationship( "StoreHasManyProducts", RelationshipCardinality.OneToMany );
            RelationshipConnection StoreHasManyProductsConnection = new RelationshipConnection(
                Store,
                Store.GetAttribute( "store_id" ),
                Product,
                Product.GetAttribute( "product_store_id" ) );

            StoreHasManyProducts.AddRelation( StoreHasManyProductsConnection );

            // Category relationships
            Relationship CategoryHasManyProducts = new Relationship( "CategoryHasManyProducts", RelationshipCardinality.OneToMany );
            RelationshipConnection CategoryHasManyProductsConnection = new RelationshipConnection(
                Category,
                Category.GetAttribute( "category_id" ),
                Product,
                Product.GetAttribute( "product_category_id" ) );

            CategoryHasManyProducts.AddRelation( CategoryHasManyProductsConnection );

            // Product relationships
            Relationship ProductBelongsToUser = new Relationship( "ProductBelongsToUser", RelationshipCardinality.OneToOne );
            RelationshipConnection ProductBelongsToUserConnection = new RelationshipConnection(
                Product,
                Product.GetAttribute( "product_user_id" ),
                User,
                User.GetAttribute( "user_id" ) );

            ProductBelongsToUser.AddRelation( ProductBelongsToUserConnection );

            Relationship ProductBelongsToStore = new Relationship( "ProductBelongsToStore", RelationshipCardinality.OneToOne );
            RelationshipConnection ProductBelongsToStoreConnection = new RelationshipConnection(
                Product,
                Product.GetAttribute( "product_store_id" ),
                Store,
                Store.GetAttribute( "store_id" ) );

            ProductBelongsToStore.AddRelation( ProductBelongsToStoreConnection );

            Relationship ProductBelongsToCategory = new Relationship( "ProductBelongsToCategory", RelationshipCardinality.OneToOne );
            RelationshipConnection ProductBelongsToCategoryConnection = new RelationshipConnection(
                Product,
                Product.GetAttribute( "product_category_id" ),
                Category,
                Category.GetAttribute( "category_id" ) );

            ProductBelongsToCategory.AddRelation( ProductBelongsToCategoryConnection );

            return new ERModel( "CMSModel", new List<BaseERElement>() { User, Store, Category, Product,
                UserHasManyProducts, StoreHasManyProducts, CategoryHasManyProducts, ProductBelongsToUser,
                ProductBelongsToStore, ProductBelongsToCategory} );
        }

        /// <summary>
        /// Creates a MongoDBSchema and Mapping the is a 1-1 map with the ER model
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer MapEntitiesToCollections()
        {
            // Create Schema
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
            ERModel Model = CreateERModel();

            // Build Mapping
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
            ProductRules.AddRule( "product_category_id", "product_category_id" );
            ProductRules.AddRule( "product_store_id", "product_store_id" );
            ProductRules.AddRule( "product_user_id", "product_user_id" );
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

            ModelMapping Map = new ModelMapping( "CMSMap11", new List<MapRule>() { UserRules,
                ProductRules, CategoryRules, StoreRules } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionDuplicates()
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
            ERModel Model = CreateERModel();

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
            ProductRules.AddRule( "product_category_id", "product_category_id" );
            ProductRules.AddRule( "product_store_id", "product_store_id" );
            ProductRules.AddRule( "product_user_id", "product_user_id" );
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

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRules, UserRulesMain,
                ProductRules, CategoryRules, CategoryRulesMain, StoreRules, StoreRulesMain } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionCategoryDuplicated()
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
            ERModel Model = CreateERModel();

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
            ProductRules.AddRule( "product_category_id", "product_category_id" );
            ProductRules.AddRule( "product_store_id", "product_store_id" );
            ProductRules.AddRule( "product_user_id", "product_user_id" );
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

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRulesMain,
                ProductRules, CategoryRules, CategoryRulesMain, StoreRulesMain } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsStoreDuplicated()
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
            ERModel Model = CreateERModel();

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
            ProductRules.AddRule( "product_category_id", "product_category_id" );
            ProductRules.AddRule( "product_store_id", "product_store_id" );
            ProductRules.AddRule( "product_user_id", "product_user_id" );
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

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRulesMain,
                ProductRules, CategoryRulesMain, StoreRules, StoreRulesMain } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        public static RequiredDataContainer MapEntitiesToCollectionsUserDuplicated()
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
            ERModel Model = CreateERModel();

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
            ProductRules.AddRule( "product_category_id", "product_category_id" );
            ProductRules.AddRule( "product_store_id", "product_store_id" );
            ProductRules.AddRule( "product_user_id", "product_user_id" );
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

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRules, UserRulesMain,
                ProductRules, CategoryRulesMain, StoreRulesMain } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}