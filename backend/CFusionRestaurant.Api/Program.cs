using CFusionRestaurant.Api.Infrastructure;
using CFusionRestaurant.BusinessLayer.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog.Events;
using Serilog;
using System.IO.Compression;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;


// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Set logging level for Microsoft namespaces
    .Enrich.FromLogContext()
    .WriteTo.Console() // Write log events to the console
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) // Write log events to log.txt file
    .CreateLogger();

try
{
    Log.Information("Starting up");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<RequestResponseLoggingFilter>();
    builder.Services.AddDependencyServices(builder.Configuration);

    //This is used the compress responses
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
        {
        "application/octet-stream"
    });
    })
    .Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Fastest;
    })
    .AddControllers();


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            p =>
            {
                p.WithOrigins(
                        "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
    }).AddAutoMapper(Assembly.GetEntryAssembly());

    //Swagger configuration
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "CFusion Restaurant Order API",
        Version = "v1",
        Description = "An API to perform Restaurant operations",
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = $"JWT Authorization header using the Bearer scheme. Example: Bearer token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

    });

    builder.Services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8
                .GetBytes(builder.Configuration["AppSettings:SecretKey"] ?? string.Empty)
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseCors("AllowAllOrigins");

    //Custom middleware to handle exceptions
    app.UseMiddleware<CustomExceptionMiddleware>();

    app.UseResponseCompression();
    app.UseResponseCaching();

    // This will make the HTTP requests log as rich logs instead of plain text.
    app.UseSerilogRequestLogging(); 

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally {
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


