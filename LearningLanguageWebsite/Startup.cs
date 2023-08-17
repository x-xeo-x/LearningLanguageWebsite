using LearningLanguageWebsite.Interfaces;
using LearningLanguageWebsite.Services;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LearningLanguageWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = "session";
                options.IdleTimeout = TimeSpan.FromMinutes(555);
            });

			services.AddAntiforgery(options =>
			{
				options.FormFieldName = "csrfToken";
				options.HeaderName = "X-Csrf-Token-Value";
			});

			services.AddControllersWithViews();
            services.AddResponseCaching();
            services.AddHttpClient();

			services.AddSingleton<DatabaseService>();

			services.AddSingleton<IAccountRepository, AccountRepositoryService>();
			services.AddSingleton<IEmailProvider, EmailProviderService>();
			services.AddSingleton<IPasswordHasher, PasswordHasher>();
			services.AddSingleton<IUserAuthentication, UserAuthenticationService>();
            services.AddSingleton<ILanguageRepository, LanguageRepositoryService>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseResponseCaching();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            var wsOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };
            app.UseWebSockets(wsOptions);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}
