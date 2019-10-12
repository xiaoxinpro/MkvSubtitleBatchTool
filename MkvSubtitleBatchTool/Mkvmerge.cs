using System;
using System.Collections.Generic;
using System.IO;
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
            StringBuilder sbSubtitleTrack = new StringBuilder(" --subtitle-tracks ");
            bool isDeleteItem = false;
            //判断是否有no项目
            string bakPath = tracks.Length > 0 ? tracks[0].Path : "";
            foreach (MkvinfoTrack itemTrack in tracks)
            {
                if (bakPath != itemTrack.Path)
                {
                    sb.Append(" ( \"" + bakPath + "\" ) ");
                    bakPath = itemTrack.Path;
                }
                if (itemTrack.TrackType == "subtitles")
                {
                    if (Path.GetExtension(itemTrack.Path).ToLower() == ".mkv")
                    {
                        if (itemTrack.IsDelete)
                        {
                            isDeleteItem = true;
                        }
                        else
                        {
                            sb.Append("--language " + itemTrack.TrackID + ":" + itemTrack.Language + " ");
                            if (itemTrack.IsDefault)
                            {
                                sb.Append("--default-track " + itemTrack.TrackID + ":yes ");
                            }
                            sbSubtitleTrack.Append(itemTrack.TrackID + " ");
                            isNoSubtitle = false;
                        }
                    }
                    else
                    {
                        if (!itemTrack.IsDelete)
                        {
                            sb.Append("--language " + itemTrack.TrackID + ":" + itemTrack.Language + " ");
                            if (itemTrack.IsDefault)
                            {
                                sb.Append("--default-track " + itemTrack.TrackID + ":yes ");
                            }
                        }
                    }
                }
            }
            sb.Append(" ( \"" + bakPath + "\" ) ");
            if (isNoSubtitle)
            {
                sb.Insert(0, " --no-subtitles ");
            }
            else if (isDeleteItem)
            {
                sb.Insert(0, sbSubtitleTrack.ToString());
            }

            //
            return sb.ToString();
        }
        #endregion

    }
}
