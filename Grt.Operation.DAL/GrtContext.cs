using System.Data.Entity;
using CMP.Operation.DAL.Models;

using System.Diagnostics;
using CMP.Operation.DAL.Functions;


namespace CMP.Operation.DAL
{
    /// <summary>
    /// DBACCount to initalize Database by Code First
    /// </summary>
    public class CMPContext : DbContext
    {
        public CMPContext() : base("name=CMPContext")
        {
            //write Sql Scripts in Debug
            Database.Log = s => Debug.WriteLine(s);
            // Database.Log = s => Errors.SaveLastSQLScript(s);
            //Configuration.ProxyCreationEnabled = false;

            this.Configuration.LazyLoadingEnabled = false;
            base.Configuration.ProxyCreationEnabled = false;
            //not dropcreate use the current Data whithout Change it
            Database.SetInitializer<CMPContext>(null);
            //for derbug-dropCreateDB
           // Database.SetInitializer<CMPContext>(new PREPDBInitializer());
        }

        /// <summary>
        /// Override base Model to change the defalt Properties of EntityFramework 
        /// This method is called when the model for a derived context has been initialized,
        ///  but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder that defines the model for the context being created.
        /// </param>
        /// <remarks>
        /// Reslash the Remarks of the method before adding Migrations
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //To the Migrations 
            modelBuilder.Entity<Employee>();
            modelBuilder.Entity<Job>();
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<Department>();
            modelBuilder.Entity<FocalPoint>();
            modelBuilder.Entity<ProjectType>();
            modelBuilder.Entity<Technology>();
            modelBuilder.Entity<Project>();
            modelBuilder.Entity<ProjectTechnology>();
            modelBuilder.Entity<Country>();
            modelBuilder.Entity<State>();
            modelBuilder.Entity<City>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<Experience>();
            modelBuilder.Entity<ExperienceTechnology>();
            base.OnModelCreating(modelBuilder);
        }

    }

    /// <summary>
    /// ovverload Config of initialize of Context, Only For debug!
    /// </summary>
    public class PREPDBInitializer : DropCreateDatabaseIfModelChanges<CMPContext>
    {
        protected override void Seed(CMPContext context)
        {
            base.Seed(context);
        }
    }
}
