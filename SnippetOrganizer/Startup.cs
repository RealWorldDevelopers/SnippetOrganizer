using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Ui;
using System;

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

         // Password Generator
         services.AddTransient<IPasswordGenerator, PasswordGenerator>();

         // GitHub client
         services.AddTransient( x=> new GitHubClient(new ProductHeaderValue(appSettings.GitHubConfig.ProductHeader)));

         // enable sessions
         services.AddDistributedMemoryCache();
         services.AddSession(options =>
         {
            //options.IdleTimeout = TimeSpan.FromSeconds(10); // default is 20 min
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
         });

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

         // enable session
         app.UseSession();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
         });
      }
   }
}
