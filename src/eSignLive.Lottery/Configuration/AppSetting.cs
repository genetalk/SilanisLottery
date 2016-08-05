using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace eSignLive.Lottery.Configuration
{
    public sealed class AppSetting : IAppSetting
    {
        public string Version { get; set; }

        public Dictionary<string, string> LotterySetting { get; set; }

        public Dictionary<string, string> ReadConfigurationFile()
        {
            if (LotterySetting == null || LotterySetting.Count == 0)
            {
                LotterySetting = new Dictionary<string, string>();

                IConfigurationBuilder builder = new ConfigurationBuilder();

                //load Json file, the web.config and appSetting are no longer supported in .Net Core
                builder.AddJsonFile("AppSettings.json");
                IConfigurationRoot configuration = builder.Build();

                var version = configuration["Version"];

                var parms = configuration.AsEnumerable();
                foreach (var v in parms)
                {
                    if (v.Key.StartsWith("AppSettings:LotteryParameters:"))
                        LotterySetting.Add(v.Key.Replace("AppSettings:LotteryParameters:", string.Empty), v.Value);
                }
            }
            return LotterySetting;
        }
    }
}
