using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnippetOrganizer.Ui;

namespace SnippetOrganizer
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
         // for use within ConfigureServices
         var appSettings = new AppSettings();
         Configuration.GetSection("ApplicationSettings").Bind(appSettings);

         // app config settings
         services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));

         // Query Factories
         services.AddTransient<Business.Snippet.Queries.IFactory, Business.Snippet.Queries.Factory>();

         // Command Factories
         services.AddTransient<Business.Snippet.Commands.IFactory, Business.Snippet.Commands.Factory>();

         // DTO Factories
         services.AddTransient<Business.Snippet.Dto.IFactory, Business.Snippet.Dto.Factory>();

         services.AddControllersWithViews();         
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
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }       

         app.UseHttpsRedirection();
         app.UseStaticFiles();

         app.UseRouting();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
         });
      }
   }
}
