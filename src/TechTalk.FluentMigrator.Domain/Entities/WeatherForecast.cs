using System;

namespace FluentMigrator.Domain.Entities
{
    public class WeatherForecast
    {
        public WeatherForecast()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}
