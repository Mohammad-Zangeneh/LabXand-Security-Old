using Kms.Common;
using Kms.Security.Common.DataContract;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Kms.Security.Core
{
    public interface ISecurityConfigurationReader
    {
        SecurityConfigurationDto GetSecurityConfiguration();
    }
    public interface ISecurityConfigurationContext
    {
        SecurityConfigurationDto Instance { get; }
        void Set(SecurityConfigurationDto securityConfiguration);
    }
    public class SecurityConfigurationApiReader : ISecurityConfigurationReader
    {
        public SecurityConfigurationDto GetSecurityConfiguration()
        {
            try
            {
                var serviceRoot = ConfigurationManager.AppSettings["ServiceRoot"];
                var data = HttpUtil.PerformHttpGet(serviceRoot + "/api/SecurityConfiguration");

                using (var reader = new StreamReader(data.GetResponseStream(), Encoding.UTF8))
                {
                    var responseText = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(responseText))
                    {
                        throw new Exception();
                    }

                    return JsonConvert.DeserializeObject<SecurityConfigurationDto>(responseText);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class SecurityConfigurationContext : ISecurityConfigurationContext
    {
        private readonly ISecurityConfigurationReader _securityConfigurationReader;
        private SecurityConfigurationDto securityConfigurationDto;
        public SecurityConfigurationContext(ISecurityConfigurationReader securityConfigurationReader)
        {
            _securityConfigurationReader = securityConfigurationReader;
        }

        public SecurityConfigurationDto Instance
        {
            get
            {
                if (securityConfigurationDto == null)
                    securityConfigurationDto = _securityConfigurationReader.GetSecurityConfiguration();
                return securityConfigurationDto;
            }
        }

        public void Set(SecurityConfigurationDto securityConfiguration)
        {
            securityConfigurationDto = securityConfiguration;
        }
    }


    //public static class SecurityConfigurationManager
    //{
    //    static SecurityConfigurationDto x;
    //    public static SecurityConfigurationDto SecurityConfiguration
    //    {
    //        get
    //        {
    //            if (x == null)
    //            {
    //                SetSecurityConfigurationFromApi();
    //            }
    //            return x;
    //        }
    //    }
    //    public static void SetSecurityConfigurationFromApi()
    //    {
    //        try
    //        {
    //            var serviceRoot = ConfigurationManager.AppSettings["ServiceRoot"];
    //            var data = HttpUtil.PerformHttpGet(serviceRoot + "/api/SecurityConfiguration");

    //            using (var reader = new StreamReader(data.GetResponseStream(), Encoding.UTF8))
    //            {
    //                var responseText = reader.ReadToEnd();
    //                if (string.IsNullOrWhiteSpace(responseText))
    //                {
    //                    SecurityConfiguration = null;
    //                    return;
    //                }

    //                SecurityConfiguration = JsonConvert.DeserializeObject<SecurityConfigurationDto>(responseText);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //}
}

