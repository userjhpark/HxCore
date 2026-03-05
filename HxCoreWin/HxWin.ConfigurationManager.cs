using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    partial class HxWin
    {
        //참조 : https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.appsettings?view=netframework-4.6.1
        //참조 : https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.connectionstrings?view=netframework-4.6.1
        public static Dictionary<string, string> GetConfigurationManagerAppSettingsReadAll()
        {
            Dictionary<string, string> Result = null;
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                if (appSettings != null)
                {
                    if (appSettings.Count == 0)
                    {
                        Debug.WriteLine("AppSettings is empty.");
                    }
                    else
                    {
                        Result = new Dictionary<string, string>();
                        foreach (var key in appSettings.AllKeys)
                        {
                            //Debug.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                            Result.Add(key, appSettings[key]);
                        }
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                Debug.WriteLine("Error reading app settings : " + ex.Message);
            }
            return Result;
        }

        public static string GetConfigurationManagerAppSettingsReadValue(string key)
        {
            string Result = null;
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                Result = appSettings[key] ?? null;
                //Console.WriteLine(result);
            }
            catch (ConfigurationErrorsException ex)
            {
                Debug.WriteLine("Error reading app settings : " +ex.Message);
            }
            return Result;
        }
        public static void SetConfigurationManagerAppSettingsAddUpdate(string key, string value)
        {
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                Debug.WriteLine("Error writing app settings : " + ex.Message);
            }
        }

        public static string GetConfigurationManagerConnectionString(string connName)
        {
            string Result = null;
            try
            {
                if(connName.IsNullOrWhiteSpaceEx() != true && ConfigurationManager.ConnectionStrings != null)
                    Result = ConfigurationManager.ConnectionStrings[connName]?.ConnectionString;
            }
            catch (ConfigurationErrorsException ex)
            {
                Debug.WriteLine("Error reading app settings : " + ex.Message);
            }
            return Result;
        }
    }
}
