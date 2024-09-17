using APICatalogo.Context; // Importa o namespace que contém o contexto do banco de dados
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters; // Importa o namespace que contém os filtros personalizados
using APICatalogo.Logging; // Importa o namespace que contém o provedor de log personalizado
using APICatalogo.Repositories;
using Microsoft.EntityFrameworkCore; // Importa o Entity Framework Core para interagir com o banco de dados
using System.Globalization;
using System.Text.Json.Serialization; // Importa a funcionalidade para configurar a serialização JSON

var builder = WebApplication.CreateBuilder(args); // Cria um construtor de aplicação web, configurando serviços e middleware

// Adiciona serviços ao contêiner de injeção de dependência
builder.Services.AddControllers(options =>
{
    // Adiciona um filtro global de exceções personalizado para todas as ações do controlador
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    // Configura a serialização JSON para ignorar ciclos de referência (evita erros de referência circular)
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer(); // Adiciona suporte para exploração de endpoints da API
builder.Services.AddSwaggerGen(); // Adiciona suporte para geração de documentação Swagger

// Configura o contexto do banco de dados, especificando o uso do SQL Server com a string de conexão definida nas configurações
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ApiLoggingFilter>(); // Adiciona o filtro de logging personalizado ao contêiner de serviços com ciclo de vida Scoped

//Registrando o serviço no contender DI.
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

// Configura o sistema de logging para usar um provedor de log personalizado com o nível de log definido
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build(); // Constrói a aplicação com base na configuração do builder

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o Swagger em ambiente de desenvolvimento para documentação interativa da API
    app.UseSwaggerUI(); // Habilita a interface do usuário do Swagger
}

app.UseHttpsRedirection(); // Força o uso de HTTPS em todas as requisições
app.UseAuthorization(); // Adiciona suporte para autorização com base em políticas ou autenticação
app.MapControllers(); // Mapeia os endpoints dos controladores para o pipeline de requisições
app.Run(); // Inicia a aplicação e escuta as requisições
