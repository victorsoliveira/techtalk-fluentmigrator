using FluentMigrator;

namespace FluentMigrator.Infra.Attributes
{
    public class GproMigrationAttribute : MigrationAttribute
    {
        public GproMigrationAttribute(int year, int month, int day, int hour, int minute, string description = null)
           : base(CalculateValue(year, month, day, hour, minute), description: description)
        {

        }
        private static long CalculateValue(int year, int month, int day, int hour, int minute)
        {
            return year * 100000000L + month * 1000000L + day * 10000L + hour * 100L + minute;
        }
    }
}
