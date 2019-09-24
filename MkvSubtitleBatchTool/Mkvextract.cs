using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkvSubtitleBatchTool
{
    /// <summary>
    /// 导出轨道类
    /// </summary>
    public class Mkvextract
    {
        #region 私有字段
        private int numProgressRate = 0;
        private string ShellPath;
        private CmdHelper Cmd;
        #endregion

        #region 属性
        public string MkvFilePath { get; set; }
        public string SaveFilePath { get; set; }
        public MkvinfoTrack Track { get; set; }
        public string Error { get; private set; }
        #endregion

        #region 构造函数
        public Mkvextract(DelegateMkvExtract e)
        {
            numProgressRate = 0;
            EventMkvExtarct += e;
            Cmd = new CmdHelper(p_OutputDataReceived, CmdProcess_Exited);
            ShellPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"mkvtoolnix\mkvextract.exe";
        }
        #endregion

        #region 公共函数
        /// <summary>
        /// 开启导出
        /// </summary>
        /// <returns></returns>
        public bool Start(out string err)
        {
            if (!File.Exists(MkvFilePath))
            {
                err = "MKV文件不存在，无法导出字幕。";
                return false;
            }
            else if (Path.GetFileNameWithoutExtension(MkvFilePath).ToLower() != ".mkv")
            {
                err = "不正确的MKV文件，无法导出字幕。";
                return false;
            }
            else if (File.Exists(SaveFilePath))
            {
                err = "保存文件已存在，输出文件冲突。";
                return false;
            }
            else if (Track == null || Track.TrackID < 0)
            {
                err = "设定的轨道存在问题，无法获取轨道信息。";
                return false;
            }
            else
            {
                numProgressRate = 1;
                ActionMkvExtarct();
                Task.Run(()=>
                {
                    Cmd.Send(ShellPath, @"--ui-language en """ + MkvFilePath + @""" " + Track.TrackID + @":""" + SaveFilePath + @""" ");
                });
                err = "开始导出轨道文件...";
                return true;
            }
        }
        #endregion

        #region 委托事件
        /// <summary>
        /// 导出轨道事件函数
        /// </summary>
        /// <param name="sender">this</param>
        /// <param name="rate">当前进度：-1错误，0未开始，1-99转换中，100完成</param>
        public delegate void DelegateMkvExtract(object sender, int rate);
        public event DelegateMkvExtract EventMkvExtarct;
        private void ActionMkvExtarct(string error = "")
        {
            if (error != "")
            {
                Error = error;
                numProgressRate = -1;
            }
            EventMkvExtarct?.Invoke(this, numProgressRate);
        }
        #endregion

        #region 命令行接口
        /// <summary>
        /// 接收到的CMD返回数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// 命令结束时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdProcess_Exited(object sender, EventArgs e)
        {
            // 执行结束后触发
            numProgressRate = 100;
            ActionMkvExtarct();
        }
        #endregion
    }
}
