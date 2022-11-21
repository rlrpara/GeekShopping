using GeekShopping.Shared.Auth;
using GeekShopping.Shared.Database;
using GeekShopping.Shared.Ioc;
using GeekShopping.Shared.Services.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace GeekShopping.ProductApi;

public class Program
{
    public static void Main(string[] args)
    {
        DotEnvLoad.Load();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors();
        builder.Services.AddControllers()
            .AddJsonOptions(x => { x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; })
            .AddNewtonsoftJson(x => { x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Geek Shopping",
                Version = "v1",
                Description = "Loja virtual de produtos geek",
                Contact = new OpenApiContact
                {
                    Name = "Rodrigo Lima",
                    Email = "rodrigo.ribeiro@questores.com.br",
                    Url = new Uri("http://www.rodrigolima.com.br")
                }
            });
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api-doc.xml"));
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Informe **_APENAS_** seu JWT Bearer token na caixa abaixo.",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            x.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            x.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
        });

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigAuth.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        new DatabaseConfiguration().GerenciarBanco();

        NativeInjector.RegisterServices(builder.Services);

        builder.Services.AddSingleton(new AutoMapperSetup());

        builder.Services.AddAutoMapper(typeof(AutoMapperSetup));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
        }
        app.UseSwaggerUI();

        app.UseCors(x =>
        {
            x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}