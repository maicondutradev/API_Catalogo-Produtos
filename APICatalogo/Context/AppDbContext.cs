using APICatalogo.Models; // Importa o namespace que contém os modelos Categoria e Produto
using Microsoft.EntityFrameworkCore; // Importa as funcionalidades do Entity Framework Core

namespace APICatalogo.Context
{
    // Define a classe de contexto que gerencia a conexão com o banco de dados
    public class AppDbContext : DbContext
    {
        // Construtor da classe AppDbContext, recebe as opções de configuração do contexto
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Propriedade que representa a tabela Categorias no banco de dados
        public DbSet<Categoria>? Categorias { get; set; }

        // Propriedade que representa a tabela Produtos no banco de dados
        public DbSet<Produto>? Produtos { get; set; }
    }
}
