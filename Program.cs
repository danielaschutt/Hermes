using System;
using System.Collections.Generic;
using System.Text;
using Argos.Data.Context;
using dotenv.net;
using Hermes.Helpers;
using System.Linq;
using Argos.Domain.CameraLogRoot;
using Argos.Domain.Interfaces.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes
{
    class Program
    {
        static void Main(string[] args)
        {
 
            DotEnv.Config(false); //fails silently
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            IEnumerable<CameraLog> alertas;
            do
            {
                using (var context = new DataContext(optionsBuilder.Options))
                {
                    alertas = context.DbSetCameraLog.Include(i => i.Camera).Include(i => i.Alerta)
                        .Include(i => i.Alerta.Tipo).Where(i => i.Status == 0).AsEnumerable();
                }

                Location.getDistancias(optionsBuilder.Options, alertas).GetAwaiter().GetResult();
            } while (alertas != null);

        }
    }
}