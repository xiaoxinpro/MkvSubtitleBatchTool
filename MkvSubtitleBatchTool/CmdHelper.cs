using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkvSubtitleBatchTool
{
    public class CmdHelper
    {
        private DataReceivedEventHandler p_OutputDataReceived;
        private DataReceivedEventHandler p_ErrorDataReceived;
        private EventHandler CmdProcess_Exited;

        #region 构造函数
        public CmdHelper(DataReceivedEventHandler outputDataReceived)
        {
            p_OutputDataReceived = outputDataReceived;
        }

        public CmdHelper(DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived)
        {
            p_OutputDataReceived = outputDataReceived;
            p_ErrorDataReceived = errorDataReceived;
        }

        public CmdHelper(DataReceivedEventHandler outputDataReceived, EventHandler exitedHandler)
        {
            p_OutputDataReceived = outputDataReceived;
            CmdProcess_Exited = exitedHandler;
        }

        #endregion

        /// <summary>
        /// 发送Cmd命令
        /// </summary>
        /// <param name="StartFileName">命令名称</param>
        /// <param name="StartFileArg">命令参数</param>
        public void Send(string StartFileName, string StartFileArg = "")
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入
            if (p_OutputDataReceived != null)
            {
                CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出
                CmdProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            }
            if (p_ErrorDataReceived != null)
            {
                CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出
                CmdProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            }
            if (CmdProcess_Exited != null)
            {
                CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件
                CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件
            }
            CmdProcess.Start();
            if (p_OutputDataReceived != null)
            {
                CmdProcess.BeginOutputReadLine();
            }
            if (p_ErrorDataReceived != null)
            {
                CmdProcess.BeginErrorReadLine();
            }

            // 如果打开注释，则以同步方式执行命令，此例子中用Exited事件异步执行。
            // CmdProcess.WaitForExit();     
        }
    }
}
