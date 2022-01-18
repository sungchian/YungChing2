using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace YungChing.API
{
    public class GetAppSetting
    {
        public string getDataBseStr()
        {
            string envcheck = (string)Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);//取得自行設定的環境變數

            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();

            string dataBseStr = config["ConnectionStrings:StoreConnection"];
            return dataBseStr;
        }
    }
}
