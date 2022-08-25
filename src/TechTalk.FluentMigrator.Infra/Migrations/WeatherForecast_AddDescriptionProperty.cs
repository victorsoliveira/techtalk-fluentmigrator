using FluentMigrator.Domain.Entities;
using FluentMigrator.Infra.Attributes;
using Marten;
using System;
using System.Linq;

namespace FluentMigrator.Infra.Migrations
{
    [GproMigration(2022,08, 25, 16, 42, "Adding new Description property", BreakingChange = false)]
    public class WeatherForecast_AddDescriptionProperty : Migration
    {
        private readonly IDocumentSession _documentSession;

        public WeatherForecast_AddDescriptionProperty(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public override void Down()
        {
            // Do nothing!
        }

        public override void Up()
        {
            var toUpdate = _documentSession.Query<KeyValue>(
                    @"select json_build_object('Id', wf.data->>'Id', 'Summary', wf.data->>'Summary')
                        from mt_doc_weatherforecast wf
                        where wf.data->>'Description' is null").ToList();

            if (toUpdate != null && toUpdate.Any())
            {
                foreach (var item in toUpdate)
                {
                    var wf = _documentSession.Load<WeatherForecast>(item.Id);
                    
                    wf.SetDescription($"{item.Summary} new description !");
                    
                    _documentSession.Update(wf);
                }

                _documentSession.SaveChanges();
            }
        }

        private class KeyValue
        {
            public Guid Id { get; set; }
            public string Summary { get; set; }
        }
    }

}
