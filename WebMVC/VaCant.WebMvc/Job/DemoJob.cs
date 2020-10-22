using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VaCant.WebMvc
{
    /// <summary>
    /// 使用Quartz的定时任务
    /// </summary>
    [DisallowConcurrentExecution] //禁止并发执行
    public class DemoJob : IJob
    {
        //private ILog log = LogManager.GetLogger(typeof(DemoJob));

        public  Task Execute(IJobExecutionContext context)
        {
            //参考文章 https://blog.csdn.net/xiaolu1014/article/details/103863979
            //var jobKey = context.JobDetail.Key;//获取job信息
            //var triggerKey = context.Trigger.Key;//获取trigger信息
            //log.Debug("日志开始记录定时任务"+ $"{DateTime.Now} QuartzJob:==>>自动执行.{jobKey.Name}|{triggerKey.Name}");
            //await Task.CompletedTask;

            return Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(@"E:\MM\Mes.log", true, Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                }
            });
        }


        
    }
}
