using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace VaCant.Common
{
    /// <summary>
    /// JSON操作帮助类
    /// auth:wanglq
    /// date:2018-8-3
    /// </summary>
    public static class JsonOperator
    {
        /// <summary>
        /// Json 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <param name="converters"></param>
        /// <returns></returns>
        public static string JsonSerialize(object value, params JsonConverter[] converters)
        {
            if (value != null)
            {
                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                if (converters != null && converters.Length > 0)
                {
                    var list = converters.Append(timeConverter);
                    return JsonConvert.SerializeObject(value, list.ToArray());
                }
                else
                {
                    if (value is DataSet)
                        return JsonConvert.SerializeObject(value, new DataSetConverter(), timeConverter);
                    else if (value is DataTable)
                        return JsonConvert.SerializeObject(value, new DataTableConverter(), timeConverter);
                    else
                        return JsonConvert.SerializeObject(value, timeConverter);
                }
            }
            return string.Empty;
        }




        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="converters"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string value, params JsonConverter[] converters)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            if (converters != null && converters.Length > 0)
            {
                return JsonConvert.DeserializeObject<T>(value, converters);
            }
            else
            {
                Type type = typeof(T);


                if (type == typeof(DataSet))
                {
                    return JsonConvert.DeserializeObject<T>(value, new DataSetConverter());
                }
                else if (type == typeof(DataTable))
                {
                    return JsonConvert.DeserializeObject<T>(value, new DataTableConverter());
                }
                return JsonConvert.DeserializeObject<T>(value);
            }
        }
    }
}
