using Bogus;
using DataFaker.Models.CMS;
using DataFaker.Models.Prograd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFaker
{
    public static class MainApp
    {
        public static void Main()
        {
            Console.Write( "Enter 1 to process CMS data, 2 to process Progradweb or 3 to process Progradweb2: " );
            int dbPicked = Convert.ToInt32( Console.ReadLine() );

            switch ( dbPicked )
            {
                case 1:
                    RunCSMStuff();
                    break;
                case 2:
                    RunProgradStuff();
                    break;
                case 3:
                    RunProgradStuff2();
                    break;
                default:
                    Console.WriteLine( "Invalid choice\nPress enter to exit." );
                    break;
            }

            Console.Read();
        }

        public static void RunCSMStuff()
        {
            // Connect to db
            ResearchCMSContext dbCMS = new ResearchCMSContext();

            // Truncate tables
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE users" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE products" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE stores" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE categories" );

            Faker<User> testUsers = new Faker<User>( "pt_BR" )
                .RuleFor( u => u.UserID, ( f, u ) => ++u.UserID )
                .RuleFor( u => u.UserName, ( f, u ) => f.Name.FullName() )
                .RuleFor( u => u.UserEmail, ( f, u ) => f.Internet.Email( u.UserName ) )
                .FinishWith( ( f, u ) =>
                {
                    Console.WriteLine( "User Created! Name = {0}", u.UserName );
                } );

            int amountOfUsers = 50;

            List<User> users = testUsers.Generate( amountOfUsers );
            dbCMS.Users.AddRange( users );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current user count: {0}", dbCMS.Users.Count() );

            string[] categoryNames = new Faker( "pt_BR" ).Commerce.Categories( 30 ).Distinct().ToArray();
            List<Category> categories = new List<Category>();
            int amountOfCategories = categoryNames.Length;

            for ( int i = 0; i < amountOfCategories; i++ )
            {
                Category cat = new Category()
                {
                    CategoryName = categoryNames[ i ]
                };

                categories.Add( cat );
            }

            dbCMS.Categories.AddRange( categories );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current category count: {0}", dbCMS.Categories.Count() );

            Faker<Store> testStores = new Faker<Store>( "pt_BR" )
                .RuleFor( s => s.StoreID, ( f, s ) => ++s.StoreID )
                .RuleFor( s => s.StoreName, ( f, s ) => f.Company.CompanyName() )
                .FinishWith( ( f, s ) =>
                {
                    Console.WriteLine( "Store created! Name = {0}", s.StoreName );
                } );

            List<Store> stores = testStores.Generate( 20 );
            dbCMS.Stores.AddRange( stores );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current store count: {0}", dbCMS.Stores.Count() );

            Faker<Product> testProducts = new Faker<Product>( "pt_BR" )
                .RuleFor( p => p.ProductID, ( f, p ) => ++p.ProductID )
                .RuleFor( p => p.Title, ( f, p ) => f.Commerce.ProductName() )
                .RuleFor( p => p.Description, ( f, p ) => f.Commerce.ProductAdjective() )
                .RuleFor( p => p.UserID, ( f, p ) => f.PickRandom( users ).UserID )
                .RuleFor( p => p.CategoryID, ( f, p ) => f.PickRandom( categories ).CategoryID )
                .RuleFor( p => p.StoreID, ( f, p ) => f.PickRandom( stores ).StoreID )
                .FinishWith( ( f, p ) =>
                {
                    Console.WriteLine( "Product created! Title = {0}", p.Title );
                } );

            List<Product> products = testProducts.Generate( 50 );
            dbCMS.Products.AddRange( products );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current product count: {0}", dbCMS.Products.Count() );
        }

        public static void RunProgradStuff()
        {
            ProgradContext dbContext = new ProgradContext();

            Console.WriteLine( "Processing prograd stuff" );

            // Truncate tables
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE cursograd" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE alunograd" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE endereco" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE enfasegrad" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE gradegrad" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE matriculagrad" );

            string[] CursoNames = { "Administração", "Ciências Biológicas", "Ciência da Computação", "Enfermagem", "Engenharia Civil", "Engenharia de Computação", "Física", "Letras", "Matemática", "Medicina" };

            foreach ( string Curso in CursoNames )
            {
                CursoGrad cursoModel = new CursoGrad()
                {
                    nomecur_cur = Curso,
                    sigla_cur = getSigla( Curso )
                };

                dbContext.Cursos.Add( cursoModel );
            }

            dbContext.SaveChanges();

            Console.WriteLine( "Generating alunograd" );

            Faker<AlunoGrad> testAlunos = new Faker<AlunoGrad>( "pt_BR" )
                .RuleFor( a => a.codalu_alug, ( f, a ) => ++a.codalu_alug )
                .RuleFor( a => a.nomealu_alug, ( f, a ) => f.Name.FullName() )
                .RuleFor( a => a.cpf_alug, ( f, a ) => f.Random.ReplaceNumbers( "###########" ) )
                .RuleFor( a => a.endid_alug, ( f, a ) => f.Random.Int( 1, 50 ) );

            int amountOfAlunos = 20;

            List<AlunoGrad> alunos = testAlunos.Generate( amountOfAlunos );
            dbContext.Alunos.AddRange( alunos );
            dbContext.SaveChanges();

            Console.WriteLine( "Generating endereco" );

            Faker<Endereco> testEnderecos = new Faker<Endereco>( "pt_BR" )
                .RuleFor( e => e.codend_end, ( f, e ) => ++e.codend_end )
                .RuleFor( e => e.logradouro_end, ( f, e ) => f.Address.StreetName() )
                .RuleFor( e => e.bairro_end, ( f, e ) => f.Address.County() )
                .RuleFor( e => e.cep_end, ( f, e ) => f.Address.ZipCode( "########" ) )
                .RuleFor( e => e.codcidade_end, ( f, e ) => string.Format( "{0}{1}", f.Address.CountryCode(), f.Address.ZipCode() ) );

            int amountOfAddresses = 50;

            List<Endereco> enderecos = testEnderecos.Generate( amountOfAddresses );
            dbContext.Enderecos.AddRange( enderecos );
            dbContext.SaveChanges();

            Console.WriteLine( "Generating enfase" );

            Faker<Enfase> testEnfase = new Faker<Enfase>( "pt_BR" )
                .RuleFor( e => e.codenf_enf, ( f, e ) => ++e.codenf_enf )
                .RuleFor( e => e.nomeenf_enf, ( f, e ) => f.Lorem.Letter( 10 ) )
                .RuleFor( e => e.siglaenf_enf, ( f, e ) => getSigla( e.nomeenf_enf ) )
                .RuleFor( e => e.codcur_enf, ( f, e ) => f.Random.Int( 1, dbContext.Cursos.Count() ) );

            int amountOfEnfases = 10;

            List<Enfase> enfases = testEnfase.Generate( amountOfEnfases );
            dbContext.Enfases.AddRange( enfases );
            dbContext.SaveChanges();

            Console.WriteLine( "Generating grade grad" );

            Random r = new Random();

            for ( int i = 0; i < dbContext.Disciplinas.Count(); i++ )
            {
                Grade model = new Grade()
                {
                    discipgrad_id = i + 1,
                    enfgrad_id = r.Next(1, dbContext.Enfases.Count() + 1),
                    perfil_grd = "1",
                    userid_grd = 1
                };

                dbContext.Grades.Add( model );
            }

            dbContext.SaveChanges();

            Console.WriteLine( "Generating matriculas" );

            for ( int i = 0; i < dbContext.Alunos.Count(); i++ )
            {
                int cod_enf = r.Next( 1, dbContext.Enfases.Count() + 1 );

                Matricula model_prev = new Matricula()
                {
                    anoini_matr = 2019,
                    semiini_matr = 2,
                    codalu_matr = i + 1,
                    codenf_matr = cod_enf
                };

                Matricula model = new Matricula()
                {
                    anoini_matr = 2020,
                    semiini_matr = 1,
                    codalu_matr = i + 1,
                    codenf_matr = cod_enf
                };

                dbContext.Matriculas.Add( model_prev );
                dbContext.Matriculas.Add( model );
            }

            dbContext.SaveChanges();

            Console.WriteLine( "Finished generating progradweb data" );
        }

        public static void RunProgradStuff2()
        {
            ProgradContext2 dbContext = new ProgradContext2();

            Console.WriteLine( "Processing prograd stuff" );

            // Truncate tables
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE cursograd" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE alunograd" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE endereco" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE enfasegrad" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE gradegrad" );
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE matriculagrad" );

            string[] CursoNames = { "Administração", "Ciências Biológicas", "Ciência da Computação", "Enfermagem", "Engenharia Civil", "Engenharia de Computação", "Física", "Letras", "Matemática", "Medicina" };

            foreach ( string Curso in CursoNames )
            {
                Models.Prograd2.CursoGrad cursoModel = new Models.Prograd2.CursoGrad()
                {
                    nomecur_cur = Curso,
                    sigla_cur = getSigla( Curso )
                };

                dbContext.Cursos.Add( cursoModel );
            }

            dbContext.SaveChanges();

            Console.WriteLine( "Generating alunograd" );

            Faker<Models.Prograd2.AlunoGrad> testAlunos = new Faker<Models.Prograd2.AlunoGrad>( "pt_BR" )
                .RuleFor( a => a.codalu_alug, ( f, a ) => ++a.codalu_alug )
                .RuleFor( a => a.nomealu_alug, ( f, a ) => f.Name.FullName() )
                .RuleFor( a => a.cpf_alug, ( f, a ) => f.Random.ReplaceNumbers( "###########" ) );

            int amountOfAlunos = 20;

            List<Models.Prograd2.AlunoGrad> alunos = testAlunos.Generate( amountOfAlunos );
            dbContext.Alunos.AddRange( alunos );
            dbContext.SaveChanges();

            Console.WriteLine( "Generating endereco" );

            Faker<Models.Prograd2.Endereco> testEnderecos = new Faker<Models.Prograd2.Endereco>( "pt_BR" )
                .RuleFor( e => e.codend_end, ( f, e ) => ++e.codend_end )
                .RuleFor( e => e.logradouro_end, ( f, e ) => f.Address.StreetName() )
                .RuleFor( e => e.bairro_end, ( f, e ) => f.Address.County() )
                .RuleFor( e => e.cep_end, ( f, e ) => f.Address.ZipCode( "########" ) )
                .RuleFor( e => e.codcidade_end, ( f, e ) => string.Format( "{0}{1}", f.Address.CountryCode(), f.Address.ZipCode() ) )
                .RuleFor( e => e.aluno_id, ( f, e ) => f.Random.Int( 1, 20 ) );

            int amountOfAddresses = 50;

            List<Models.Prograd2.Endereco> enderecos = testEnderecos.Generate( amountOfAddresses );
            dbContext.Enderecos.AddRange( enderecos );
            dbContext.SaveChanges();

            Console.WriteLine( "Generating enfase" );

            Faker<Models.Prograd2.Enfase> testEnfase = new Faker<Models.Prograd2.Enfase>( "pt_BR" )
                .RuleFor( e => e.codenf_enf, ( f, e ) => ++e.codenf_enf )
                .RuleFor( e => e.nomeenf_enf, ( f, e ) => f.Lorem.Letter( 10 ) )
                .RuleFor( e => e.siglaenf_enf, ( f, e ) => getSigla( e.nomeenf_enf ) )
                .RuleFor( e => e.codcur_enf, ( f, e ) => f.Random.Int( 1, dbContext.Cursos.Count() ) );

            int amountOfEnfases = 10;

            List<Models.Prograd2.Enfase> enfases = testEnfase.Generate( amountOfEnfases );
            dbContext.Enfases.AddRange( enfases );
            dbContext.SaveChanges();

            Console.WriteLine( "Generating grade grad" );

            Random r = new Random();

            for ( int i = 0; i < dbContext.Disciplinas.Count(); i++ )
            {
                Models.Prograd2.Grade model = new Models.Prograd2.Grade()
                {
                    discipgrad_id = i + 1,
                    enfgrad_id = r.Next( 1, dbContext.Enfases.Count() + 1 ),
                    perfil_grd = "1",
                    userid_grd = 1
                };

                dbContext.Grades.Add( model );
            }

            dbContext.SaveChanges();

            Console.WriteLine( "Generating matriculas" );

            for ( int i = 0; i < dbContext.Alunos.Count(); i++ )
            {
                int cod_enf = r.Next( 1, dbContext.Enfases.Count() + 1 );

                Models.Prograd2.Matricula model_prev = new Models.Prograd2.Matricula()
                {
                    anoini_matr = 2019,
                    semiini_matr = 2,
                    codalu_matr = i + 1,
                    codenf_matr = cod_enf
                };

                Models.Prograd2.Matricula model = new Models.Prograd2.Matricula()
                {
                    anoini_matr = 2020,
                    semiini_matr = 1,
                    codalu_matr = i + 1,
                    codenf_matr = cod_enf
                };

                dbContext.Matriculas.Add( model_prev );
                dbContext.Matriculas.Add( model );
            }

            dbContext.SaveChanges();

            Console.WriteLine( "Finished generating progradweb data" );
        }

        public static string getSigla(string Source)
        {
            string[] splitted = Source.Split( new string[] { " " }, StringSplitOptions.RemoveEmptyEntries );
            if ( splitted.Length > 1 )
            {
                return string.Format( "{0}{1}", splitted.First().Substring( 0, 3 ), splitted.Last().Substring( 0, 3 ) ).ToUpper();
            }

            return Source.Substring( 0, 3 ).ToUpper();
        }
    }
}