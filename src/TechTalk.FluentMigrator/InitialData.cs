using FluentMigrator.Domain.Entities;
using Marten;
using Marten.Schema;
using System;


namespace FluentMigrator.Api
{
    public class InitialData : IInitialData
    {
        public void Populate(IDocumentStore store)
        {
            using var session = store.LightweightSession();
            session.Store(WeatherForecasts);
            session.SaveChanges();
        }

        public static readonly WeatherForecast[] WeatherForecasts =
        {
            new WeatherForecast { Id = Guid.Parse("2219b6f7-7883-4629-95d5-1a8a6c74b244"), Date = DateTime.Now, TemperatureC = 10, Summary = "Cold" },
            new WeatherForecast { Id = Guid.Parse("642a3e95-5875-498e-8ca0-93639ddfebcd"), Date = DateTime.Now, TemperatureC = 40, Summary = "Hot" }
        };
    }
}
