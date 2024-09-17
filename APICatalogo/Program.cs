using APICatalogo.Context; // Importa o namespace que cont�m o contexto do banco de dados
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters; // Importa o namespace que cont�m os filtros personalizados
using APICatalogo.Logging; // Importa o namespace que cont�m o provedor de log personalizado
using APICatalogo.Repositories;
using Microsoft.EntityFrameworkCore; // Importa o Entity Framework Core para interagir com o banco de dados
using System.Globalization;
using System.Text.Json.Serialization; // Importa a funcionalidade para configurar a serializa��o JSON

var builder = WebApplication.CreateBuilder(args); // Cria um construtor de aplica��o web, configurando servi�os e middleware

// Adiciona servi�os ao cont�iner de inje��o de depend�ncia
builder.Services.AddControllers(options =>
{
    // Adiciona um filtro global de exce��es personalizado para todas as a��es do controlador
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    // Configura a serializa��o JSON para ignorar ciclos de refer�ncia (evita erros de refer�ncia circular)
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer(); // Adiciona suporte para explora��o de endpoints da API
builder.Services.AddSwaggerGen(); // Adiciona suporte para gera��o de documenta��o Swagger

// Configura o contexto do banco de dados, especificando o uso do SQL Server com a string de conex�o definida nas configura��es
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ApiLoggingFilter>(); // Adiciona o filtro de logging personalizado ao cont�iner de servi�os com ciclo de vida Scoped

//Registrando o servi�o no contender DI.
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

// Configura o sistema de logging para usar um provedor de log personalizado com o n�vel de log definido
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build(); // Constr�i a aplica��o com base na configura��o do builder

// Configura o pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o Swagger em ambiente de desenvolvimento para documenta��o interativa da API
    app.UseSwaggerUI(); // Habilita a interface do usu�rio do Swagger
}

app.UseHttpsRedirection(); // For�a o uso de HTTPS em todas as requisi��es
app.UseAuthorization(); // Adiciona suporte para autoriza��o com base em pol�ticas ou autentica��o
app.MapControllers(); // Mapeia os endpoints dos controladores para o pipeline de requisi��es
app.Run(); // Inicia a aplica��o e escuta as requisi��es
