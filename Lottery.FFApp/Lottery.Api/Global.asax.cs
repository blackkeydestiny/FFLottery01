using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lottery.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(); //启用Log4net

            JsonFormmatter();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void JsonFormmatter()
        {
            // 添加一个转换器 IsoDateTimeConverter，其用于日期数据的序列化和反序列化
            var dateTimeConverter = new IsoDateTimeConverter();
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.DefaultValueHandling = DefaultValueHandling.Include;        // 包含默认值           
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;               // 空值忽略
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.Converters.Add(dateTimeConverter);

            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings = serializerSettings;
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain")); // 默认将POST Body作为JSON字符串进行解析

            GlobalConfiguration.Configuration.Formatters.Insert(0, jsonFormatter);
        }
    }
}
