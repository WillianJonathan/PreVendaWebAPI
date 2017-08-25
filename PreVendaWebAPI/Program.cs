using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

namespace PreVendaWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.Title = "WEB API";

            ConfigurationManager.AppSettings["connectionString"] = " Server=(local)\\SQLEXPRESS;Database=TESTE;User Id=sa;Password=sa; ";


            var host = new WebHostBuilder()
                .UseKestrel() 
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
