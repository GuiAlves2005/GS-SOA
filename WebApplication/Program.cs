using Asp.Versioning;
using HarmonyWork.Data;
using Microsoft.EntityFrameworkCore;

// Usamos o caminho completo aqui para evitar aquele erro de ambiguidade com o nome do projeto
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Configuração do Versionamento da API (Requisito da Matéria)
builder.Services.AddApiVersioning(options =>
{
    // Define a versão padrão como 1.0
    options.DefaultApiVersion = new ApiVersion(1, 0);
    // Se o usuário não mandar versão, assume a padrão
    options.AssumeDefaultVersionWhenUnspecified = true;
    // Retorna no cabeçalho a versão da API
    options.ReportApiVersions = true;
    // Lê a versão da URL (ex: api/v1/...)
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc() // Integra com o MVC do .NET
.AddApiExplorer(options =>
{
    // Formata o grupo de versão como 'v1', 'v2'
    options.GroupNameFormat = "'v'VVV";
    // Substitui o {version} na rota automaticamente
    options.SubstituteApiVersionInUrl = true;
});

// 3. Serviços Padrão (Controllers e Swagger)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Configuração do Pipeline de Requisição (Como a API responde)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();