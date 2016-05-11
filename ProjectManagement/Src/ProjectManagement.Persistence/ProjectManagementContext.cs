using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Hermes.EntityFramework;
using ProjectManagement.Persistence.Models;

namespace ProjectManagement.Persistence
{
    public class ProjectManagementContext : FrameworkContext
    {
        public IDbSet<Test> Test { get; set; }

        public ProjectManagementContext()
        {
        }

        public ProjectManagementContext(string databaseName) : base(databaseName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
