namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<a7D.PDV.BigData.WebAPI.Models.BDContext>
    {
        public Configuration()
        {
            // Não usar migrações automatica, fazer manual, antes de publicar a aplicação
            // Veja os comandos na definição do contexto
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Models.BDContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
