using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MyCoreMVC.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace MyCoreMvc.Initialization
{
    /// <summary>
    /// 注入所有Service服务
    /// </summary>
    public static class InjectionServices
    {

        /// <summary>
        /// 扩展方法 对IServiceCollection进行扩展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyNames"></param>
        public static void RegisterServices(this IServiceCollection services, List<string> assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                services.RegisterService(assemblyName);
            }
        }


        public static void AddAssembly(this IServiceCollection service, string assemblyName
            , ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var assembly = RuntimeHelper.GetAssemblyByName(assemblyName);

            var types = assembly.GetTypes();
            var list = types.Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType).ToList();

            foreach (var type in list)
            {
                var interfaceList = type.GetInterfaces();
                if (interfaceList.Any())
                {
                    var inter = interfaceList.First();

                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Transient:
                            service.AddTransient(inter, type);
                            break;
                        case ServiceLifetime.Scoped:
                            service.AddScoped(inter, type);
                            break;
                        case ServiceLifetime.Singleton:
                            service.AddSingleton(inter, type);
                            break;
                        default:
                            service.AddScoped(inter, type);
                            break;
                    }

                }
            }
        }


        public static void Injection(this IServiceCollection services, string assemblyName
            , ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            #region 批量注入Services
            //加载程序集MyApplication
            var serviceAsm = Assembly.Load(new AssemblyName(assemblyName));


            foreach (Type serviceType in serviceAsm.GetTypes().Where(t => typeof(IBaseService).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            {
                var interfaceTypes = serviceType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    //services.AddScoped(interfaceType, serviceType);
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Transient:
                            services.AddTransient(interfaceType, serviceType);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(interfaceType, serviceType);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(interfaceType, serviceType);
                            break;
                        default:
                            services.AddScoped(interfaceType, serviceType);
                            break;
                    }
                }
            }

            #endregion
        }
    }


    public class RuntimeHelper
    {
        //通过程序集的名称加载程序集
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }
    }
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static class ServiceCollectionExtension
    {
        public static Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            Dictionary<Type, Type[]> dics = new Dictionary<Type, Type[]>();
            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> list = assembly.GetTypes().ToList();
                foreach (var item in list.Where(r => !r.IsInterface))
                {
                    var interfaceTypes = item.GetInterfaces();
                    dics.Add(item, interfaceTypes);
                }

            }
            return dics;
        }
        /// <summary>
        /// 注册单个单个程序集服务
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterService(this IServiceCollection services, string assemblyName)
        {
            foreach (var item in GetClassName(assemblyName))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
        }
    }

}


