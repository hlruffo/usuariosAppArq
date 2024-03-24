using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UsuariosApp.Api.Identity.Entities;
using UsuariosApp.API.Identity.Contexts;
using UsuariosApp.API.Settings;

namespace UsuariosApp.Api.Extensions
{
    /// <summary>
    /// classe de extensão para o AspNetCore.Identity / EntityFrameworkCore
    /// </summary>
    public static class IdentityContextExtension
    {
        public static IServiceCollection AddIdentityContext(this IServiceCollection services, IConfiguration configuration)
        {
            //configurar os pararametros do appsettings.json
            var identitySettings = new IdentitySettings();
            new ConfigureFromConfigurationOptions<IdentitySettings>(configuration.GetSection("IdentitySettings"))
                .Configure(identitySettings);

            services.AddSingleton(identitySettings);    

            //injecao de dependencia para o contexto do banco de dados
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("BDUsuariosAppArq"));
            });

            //configurar as preferencias para o Identity
            services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // email de confirmação de cadastro de usuario -> false
                options.User.RequireUniqueEmail = true; // email de usuario unico -> true
                options.Password.RequireDigit = true; // requer um digito numerico na senha -> true
                options.Password.RequiredLength = 8; // requer comprimento mínimo de 8 digitos
                options.Password.RequireLowercase = true; // requer um caractere minusculo na senha -> true
                options.Password.RequireUppercase = true; // requer um caractere maiusculo na senha -> true
                options.Password.RequireNonAlphanumeric = true; // requer um caractere especial na senha -> true
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            return services;
        }
    }
}
