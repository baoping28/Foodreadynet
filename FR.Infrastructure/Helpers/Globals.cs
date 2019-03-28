using System.Web.Configuration;
using FR.Infrastructure.Config;

namespace FR.Infrastructure.Helpers
{
    public sealed class Globals
    {
        public static readonly FoodReadySection Settings =
             ((FoodReadySection)WebConfigurationManager.GetSection("FRWebSettings"));
    }
}
