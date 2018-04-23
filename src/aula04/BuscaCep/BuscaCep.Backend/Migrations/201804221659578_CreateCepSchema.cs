namespace BuscaCep.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCepSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cep",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Cep = c.String(),
                        Logradouro = c.String(),
                        Complemento = c.String(),
                        Bairro = c.String(),
                        Localidade = c.String(),
                        UF = c.String(),
                        Unidade = c.String(),
                        IBGE = c.String(),
                        GIA = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cep");
        }
    }
}
