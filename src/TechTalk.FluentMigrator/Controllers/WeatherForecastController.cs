using FluentMigrator.Domain.Entities;
using Marten;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FluentMigrator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDocumentSession _documentSession;

        public WeatherForecastController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _documentSession.Query<WeatherForecast>().ToList();
        }
    }
}
