using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MkvSubtitleBatchTool
{
    public class Mkvinfo
    {
        #region 私有字段
        private string ShellPath;
        private CmdHelper Cmd;
        private StringBuilder sbTreak;
        #endregion

        #region 属性
        public string FilePath { get; private set; }
        public MkvinfoTrack[] Tracks { get; set; }
        #endregion

        #region 构造函数
        public Mkvinfo(DelegateGetMkvinfoDone e)
        {
            ShellPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"mkvtoolnix\mkvinfo.exe";
            Cmd = new CmdHelper(p_OutputDataReceived, CmdProcess_Exited);
            sbTreak = new StringBuilder();
            EventGetMkvinfoDone += e;
        }

        #endregion

        #region 委托获取完成事件
        public delegate void DelegateGetMkvinfoDone(object sender, EventArgs e);
        public DelegateGetMkvinfoDone EventGetMkvinfoDone;
        private void ActionGetMkvinfoDone()
        {
            EventGetMkvinfoDone?.Invoke(this, new EventArgs());
        }

        #endregion

        #region 公共函数
        /// <summary>
        /// 获取MKV文件信息
        /// </summary>
        /// <param name="mkvPath">MKV文件路径</param>
        /// <returns></returns>
        public bool Get(string mkvPath)
        {
            if (File.Exists(mkvPath) && Path.GetExtension(mkvPath).ToLower() == ".mkv")
            {
                sbTreak.Clear();
                FilePath = mkvPath;
                try
                {
                    Cmd.Send(ShellPath, @"--ui-language en -q """ + mkvPath + "\"");
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
                sbTreak.AppendLine(e.Data);
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
            string strTrack = sbTreak.ToString();
            Console.WriteLine("执行结束");
            string[] arrTrack = strTrack.Split(new[] { @"| + Track" }, StringSplitOptions.None);
            List<MkvinfoTrack> listTrack = new List<MkvinfoTrack>();
            for (int i = 1; i < arrTrack.Length; i++)
            {
                listTrack.Add(new MkvinfoTrack(arrTrack[i]));
            }
            Tracks = listTrack.ToArray();
            ActionGetMkvinfoDone();
        }
        #endregion
    }

    /// <summary>
    /// MKV文件轨道信息
    /// </summary>
    public class MkvinfoTrack
    {
        public int TrackNumber { get; set; }
        public int TrackID { get; set; }
        public decimal TrackUID { get; set; }
        public string TrackType { get; set; }
        public bool IsDefault { get; set; }
        public string CodecID { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }

        public MkvinfoTrack(string strTrack)
        {
            TrackNumber = Convert.ToInt32(Regex.Match(strTrack, @"(?<=Track\snumber:\s+)\d+").Value);
            TrackID = Convert.ToInt32(Regex.Match(strTrack, @"(?<=mkvextract:\s+)\d+").Value);
            TrackUID = Convert.ToDecimal(Regex.Match(strTrack, @"(?<=Track UID:\s+)\d+").Value);
            TrackType = Regex.Match(strTrack, @"(?<=Track\stype:\s+)\w+").Value.ToString();
            IsDefault = !Regex.IsMatch(strTrack, @"Default\strack\sflag:\s0");
            CodecID = Regex.Match(strTrack, @"(?<=Codec\sID:\s+)[\w\/]+").Value.ToString();
            Language = Regex.Match(strTrack, @"(?<=Language:\s+)\w+").Value.ToString();
            Name = Regex.Match(strTrack, @"(?<=Name:\s+)[\S]+").Value.ToString();
        }
    }
}
