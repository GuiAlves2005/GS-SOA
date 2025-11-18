using Asp.Versioning;
using HarmonyWork.Data;
using HarmonyWork.Middlewares;
using HarmonyWork.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Removida a linha using Microsoft.OpenApi.Models; que causava a falha CS0234

// >> CORRIGIDO O ERRO CS0134 DE COLISÃO DE NOMES:
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// --- 1. Configuração do Banco de Dados (SQL Server) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. Injeção de Dependência dos Serviços (Regra de Negócio e Auth) ---
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- 3. Configuração de Autenticação JWT ---
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured in appsettings.json.");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
    };
});

// --- 4. Configuração do Versionamento da API ---
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// --- 5. Serviços Padrão (Controllers e Swagger SIMPLES) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // << Versão mais simples, deve resolver a falha de assembly


// ==========================================
// CONSTRUÇÃO DO APP
// ==========================================
var app = builder.Build();

// --- 6. Configuração do Pipeline de Requisição ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();