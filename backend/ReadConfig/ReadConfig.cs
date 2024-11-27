using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using ToolBox.DataAccess.DataBase;

namespace ReadConfig
{
    public sealed class GetConfig
    {
        public string ConnectionString;
        public string stationID = "9999";
        readonly Command requete;
        public AppConfig appConfig = new AppConfig();
        public string machineName = Environment.MachineName;

        public GetConfig()
        {
            
            //Console.WriteLine(ConfigurationManager.AppSettings());

            this.ConnectionString = ConfigurationManager.ConnectionStrings[machineName].ConnectionString;

            Connection connect = new Connection(this.ConnectionString);

            requete = new Command($"select * from appConfig where stationID = {stationID}");

            appConfig = connect.ExecuteReader(requete, r => new AppConfig()
            {
                StationID = (string)r["stationID"],
                ImportPath = (string)r["importPath"],
                BackupPath = (string)r["backupPath"],
                LogPath = (string)r["logPath"],
                ImagePath = (string)r["imagePath"],
                LastImportNumber = (string)r["lastImportNumber"]
            }).FirstOrDefault();
        }
    }
}
