using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;

namespace ECWebMonitor
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (!IsAccessible("http://sales.donper.com/"))
            {
                RestartHttpService();
            }
        }

        private void RestartHttpService()
        {
            Execute("net stop http", 0);
            Execute("net start http", 0);
        }

        /// <summary>  
        /// 执行DOS命令，返回DOS命令的输出  
        /// </summary>  
        /// <param name="dosCommand">dos命令</param>  
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），  
        /// 如果设定为0，则无限等待</param>  
        /// <returns>返回DOS命令的输出</returns>  
        public static string Execute(string dosCommand, int milliseconds)
        {
            string output = ""; //输出字符串  
            if (dosCommand != null && !dosCommand.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + dosCommand;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                        if (milliseconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束  
                        }
                        else
                        {
                            process.WaitForExit(milliseconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    process.Close();
                }
            }
            return output;
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }

        protected override void OnStop()
        {
        }

        public bool IsAccessible(string webUrl)
        {
            var myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(webUrl);
            if (myStream != null)
            {
                var sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                string str = sr.ReadToEnd();
                myStream.Close();
                if (!String.IsNullOrEmpty(str))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
