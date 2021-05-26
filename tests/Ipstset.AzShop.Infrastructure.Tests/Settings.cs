using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;

namespace Ipstset.AzShop.Infrastructure.Tests
{
    public class Settings
    {
        public static DbSettings GetDbSettings()
        {
            var config = ConfigurationHelper.InitConfiguration();
            var db = new DbSettings
            {
                Connection = config["DbSettings:Connection"],
                Schema = config["DbSettings:Schema"],
            };

            return db;
        }
    }
}
