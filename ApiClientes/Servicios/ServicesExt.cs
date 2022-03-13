using System;
using System.Collections.Generic;
using System.Text;

using ApiClientes.DataModel;
using ApiClientes.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiClientes.Servicios
{
   public static  class ServicesExt
    {
        public static IServiceCollection AddAPIClientes(this IServiceCollection services, IConfiguration cfg)
        {
            var sec = cfg.GetSection("ApiCliente");
            var setting = sec.Get<ModelConfg>();

            services.AddDbContext<DBClientesContext>((IServiceProvider sp, DbContextOptionsBuilder op) =>
                {
                    op.UseSqlServer(setting .ConnectionStrings .ConnectionString, op2 => { });
                });

            services.AddLogging();
            services.AddScoped<IGlobal, Global>();
            services.AddScoped<IClienteHelper, ClienteHelper>();
            services.AddScoped<ICliente, ClienteManager>();
         
            return services;
        }
    }
}
