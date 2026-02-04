using System.Text;
using ApiEcommerce.Constants;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Conexion a Bb
var DBConnectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DBConnectionString));

// opciones de almacenamiento en cache
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024;
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Servicio de Autentificacion
var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("SecretKey no esta configurada");
}
var key = Convert.FromBase64String(secretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // en prod pasarlo a true
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Agregando perfiles de cache
builder.Services.AddControllers(option =>
    {
        option.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.Profile10);
        option.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Profile20);
    }
);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "Nuestra API utiliza autenticación JWT con el esquema Bearer.\n\n" +
            "Ingresa **Bearer** seguido de un espacio y el token JWT.\n\n" +
            "Ejemplo: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",          // ← minúscula, es el estándar
        BearerFormat = "JWT"        // ← recomendado
    });

    // ¡Esta es la parte que cambia en v10!
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()   // scopes → vacío para JWT Bearer típico
            // o Array.Empty<string>() si prefieres
        }
    });
    // agregando documentacion de versionamiento
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Ecommerce",
        Description = "API para gestioner productos y usuarios",
        TermsOfService = new Uri("http://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "DevCris",
            Url = new Uri("https://Crist.com")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia de uso",
            Url = new Uri("http://example.com/license"),
        }
    });
});

// versionamiendo de la api
var apiVersioningBuilder = builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
    // option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version")); //?api-version
});
// api explorer que perimite que swagger pueda mostrar  las versiones correctamente
apiVersioningBuilder.AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV"; // v1,v2,v3...
    option.SubstituteApiVersionInUrl = true; //api/v{api_version}/products (faltaria configurar el controlador)
});

// configuracion de Cors
builder.Services.AddCors(options =>
    {
        options.AddPolicy(PolicyNames.AllowSpecificOrigin,
        builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); // en WithOrigins("http://localhost:8000") ejemplo si solo quiero un origen
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); 
    });
}

app.UseHttpsRedirection();
// agregando la configuracion
app.UseCors(PolicyNames.AllowSpecificOrigin);

//agregando cache
app.UseResponseCaching();

// clasificacion de endpoint 
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
