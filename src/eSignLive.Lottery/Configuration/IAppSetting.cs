using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Configuration
{
    public interface IAppSetting
    {
        Dictionary<string, string> ReadConfigurationFile();
    }
}
