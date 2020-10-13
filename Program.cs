using System;
using Argos.Data.Context;
using Hermes.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Hermes
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            
            Location.getDistancias(optionsBuilder.Options);
        }
    }
}