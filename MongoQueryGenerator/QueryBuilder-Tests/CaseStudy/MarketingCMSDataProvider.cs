using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data for MarketingCMS Tests
    /// </summary>
    public static class MarketingCMSDataProvider
    {
        public static ERModel CreateERModel()
        {
            Entity User = new Entity( "User" );
            User.AddAttributes( "user_id", "user_name", "user_email", "user_access", "user_registered_at", "user_newsletter" );

            Entity Store = new Entity( "Store" );
            Store.AddAttributes( "store_id", "store_name", "store_logo" );

            Entity Category = new Entity( "Category" );
            Category.AddAttributes( "category_id", "category_name" );

            Entity Product = new Entity( "Product" );
            Product.AddAttributes( "product_id", "product_title", "product_description", "product_short_description", "product_url",
                "product_published_at", "product_category_id", "product_store_id", "product_user_id", "product_published", "product_image" );

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
            UserCol.AddAttributes( "_id", "name", "email", "access", "registered_at", "newsletter" );

            MongoDBCollection ProductCol = new MongoDBCollection( "Product" );
            ProductCol.AddAttributes( "_id", "title", "description", "short_description", "url", "published_at",
                "category_id", "store_id", "user_id", "published", "image" );

            MongoDBCollection CategoryCol = new MongoDBCollection( "Category" );
            CategoryCol.AddAttributes( "_id", "name" );

            MongoDBCollection StoreCol = new MongoDBCollection( "Store" );
            StoreCol.AddAttributes( "_id", "name", "logo" );

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
            UserRules.AddRule( "user_registered_at", "user_registered_at" );
            UserRules.AddRule( "user_newsletter", "user_newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "product_title" );
            ProductRules.AddRule( "product_description", "product_description" );
            ProductRules.AddRule( "product_short_description", "product_short_description" );
            ProductRules.AddRule( "product_url", "product_url" );
            ProductRules.AddRule( "product_published_at", "product_published_at" );
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
            ProductCol.AddAttributes( "_id", "title", "description", "short_description", "url", "published_at",
                "category_id", "store_id", "user_id", "published", "image",
                "user._id", "user._name", "user.email", "user.access", "user.registered_at", "user.newsletter",
                "category._id", "category.name",
                "store._id", "store.name", "store.logo");


            MongoSchema Schema = new MongoSchema( "CMSSchema", new List<MongoDBCollection>() { ProductCol } );

            // Retrieve ER Model
            ERModel Model = CreateERModel();

            // Build Mapping
            Entity User = (Entity)Model.FindByName( "User" );

            MapRule UserRules = new MapRule( User, ProductCol );
            UserRules.AddRule( "user_id", "user._id" );
            UserRules.AddRule( "user_name", "user.name" );
            UserRules.AddRule( "user_email", "user.email" );
            UserRules.AddRule( "user_access", "user.access" );
            UserRules.AddRule( "user_registered_at", "user.registered_at" );
            UserRules.AddRule( "user_newsletter", "user.newsletter" );

            Entity Product = (Entity)Model.FindByName( "Product" );

            MapRule ProductRules = new MapRule( Product, ProductCol );
            ProductRules.AddRule( "product_id", "_id" );
            ProductRules.AddRule( "product_title", "title" );
            ProductRules.AddRule( "product_description", "description" );
            ProductRules.AddRule( "product_short_description", "short_description" );
            ProductRules.AddRule( "product_url", "url" );
            ProductRules.AddRule( "product_published_at", "published_at" );
            ProductRules.AddRule( "product_category_id", "category_id" );
            ProductRules.AddRule( "product_store_id", "store_id" );
            ProductRules.AddRule( "product_user_id", "user_id" );
            ProductRules.AddRule( "product_published", "published" );
            ProductRules.AddRule( "product_image", "image" );

            Entity Category = (Entity)Model.FindByName( "Category" );

            MapRule CategoryRules = new MapRule( Category, ProductCol );
            CategoryRules.AddRule( "category_id", "category._id" );
            CategoryRules.AddRule( "category_name", "category.name" );

            Entity Store = (Entity)Model.FindByName( "Store" );

            MapRule StoreRules = new MapRule( Store, ProductCol );
            StoreRules.AddRule( "store_id", "store._id" );
            StoreRules.AddRule( "store_name", "store.name" );
            StoreRules.AddRule( "store_logo", "store.logo" );

            ModelMapping Map = new ModelMapping( "CMSMapDuplicates", new List<MapRule>() { UserRules,
                ProductRules, CategoryRules, StoreRules } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}
