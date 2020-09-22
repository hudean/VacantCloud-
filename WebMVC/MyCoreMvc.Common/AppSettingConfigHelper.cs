using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace VaCant.Common
{
    /// <summary>
    /// 读取appsettings.json配置信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    //public static T GetAppSettings<T>(string fileName = "appsettings.json", string key = "") where T : class, new()
    //{
    //    if (string.IsNullOrEmpty(key))
    //        key = typeof(T).Name;
    //    var directory = System.AppContext.BaseDirectory;
    //    directory = directory.Replace("\\", "/");

    //    var filePath = $"{directory}/{fileName}";
    //    if (!File.Exists(filePath))
    //    {
    //        var length = directory.IndexOf("/bin");
    //        filePath = $"{directory.Substring(0, length)}/{fileName}";
    //    }

    //    //var config = new ConfigurationBuilder()
    //    //    .AddJsonFile(filePath, false, true).Build();

    //    var config = new ConfigurationBuilder()
    //                            .SetBasePath(AppContext.BaseDirectory)
    //                            .AddJsonFile("appsettings.json").Build();

    //    var appconfig = new ServiceCollection()
    //        .Configure<T>(config.GetSection(key))
    //        .BuildServiceProvider()
    //        .GetService<IOptions<T>>()
    //        .Value;
    //    return appconfig;
    //}
}
