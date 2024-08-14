using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Utils
{
    public class JsonUtil
    {
        public static string GetFromAppSettings(string key)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            return configuration[key];
        }

        public static T GetFromAppSettings<T>(string sectionName) where T : class
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();
            return configuration.GetSection(sectionName).Get<T>();
        }
    }
}
