using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkvSubtitleBatchTool
{
    /// <summary>
    /// MKV混流类
    /// </summary>
    public class Mkvmerge
    {
        #region 字段
        private string ShellPath;
        #endregion

        #region 属性

        #endregion

        #region 构造函数
        public Mkvmerge()
        {
            ShellPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"mkvtoolnix\mkvmerge.exe";
        }

        #endregion

        #region 公共函数
        /// <summary>
        /// 启动混流
        /// </summary>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="tracks">输出轨道数组</param>
        public void MixedFlow(string savePath, MkvinfoTrack[] tracks)
        {
            string strCmdHead = ShellPath + " --ui-language en --output \"" + savePath +"\" ";
            Console.WriteLine((strCmdHead + getMixedFlowCmd(tracks)).Replace(" --", "\r\n --"));
        }
        #endregion

        #region 静态函数
        /// <summary>
        /// 获取混流命令行
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        private string getMixedFlowCmd(MkvinfoTrack[] tracks)
        {
            StringBuilder sb = new StringBuilder();
            bool isNoSubtitle = true;
            //判断是否有no项目
            foreach (MkvinfoTrack itemTrack in tracks)
            {
                if (itemTrack.TrackType == "subtitles" && itemTrack.IsDelete == false)
                {
                    isNoSubtitle = false;
                }
            }
            if (isNoSubtitle)
            {
                sb.Insert(0, " --no-subtitles ");
            }

            //
            return sb.ToString();
        }
        #endregion

    }
}
