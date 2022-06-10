using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Challenge17ApiPeliculas.LoggerCreator;
using Serilog;

namespace Challenge17ApiPeliculas
{
    public class Program
    {
        public static void Main(string[] args)
        {

            
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            try
            {
                Log.Information("Iniciando la aplicacion");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex,"La aplicacion tuvo un error");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            } 
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
            
        
                //.ConfigureLogging((ctx,loggin) =>{
                //    loggin.AddConfiguration(ctx.Configuration.GetSection("Loggin"));
        
                //});
               
                
    }
    
}
