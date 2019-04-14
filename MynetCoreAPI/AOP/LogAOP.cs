using Castle.DynamicProxy;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MynetCoreAPI.AOP
{
    public class LogAOP : IInterceptor
    {
        /// <summary>
        /// 读写文件锁
        /// </summary>
        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        public void Intercept(IInvocation invocation)
        {
            var dataIntercept = $"{DateTime.Now.ToString("yyyyMMddHHmmss")} " + $"当前执行方法：{ invocation.Method.Name} " + $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n"; //在被拦截的方法执行完毕后 继续执行当前方法
            invocation.Proceed();

            dataIntercept += ($"被拦截方法执行完毕，返回结果：{invocation.ReturnValue}");
            #region 输出到当前项目日志
            var path = Directory.GetCurrentDirectory() + @"\Log"; if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(dataIntercept);
            sw.Close();
            #endregion 
        }
    }
}
