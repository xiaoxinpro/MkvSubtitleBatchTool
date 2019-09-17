using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace MkvSubtitleBatchTool
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 当前目录含"\"
        /// </summary>
        private string MainPath;

        #region 初始化相关
        /// <summary>
        /// 初始化
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            MainPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        /// <summary>
        /// 加载窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text += @" V" + Application.ProductVersion.ToString();

            InitFileListView(listViewTrack);
        }
        #endregion

        #region 列表相关函数
        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="listView"></param>
        private void InitFileListView(ListView listView)
        {
            //基本属性设置
            listView.Clear();
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.CheckBoxes = true;
            listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView.View = View.Details;

            //创建列表头
            listView.Columns.Add("序号", 60, HorizontalAlignment.Center);
            listView.Columns.Add("文件名称", 100, HorizontalAlignment.Left);
            listView.Columns.Add("文件路径", 300, HorizontalAlignment.Left);

            //自动列宽
            listView.Columns[2].Width = -2;//根据标题设置宽度
        }
        #endregion

        #region 窗体控件相关

        /// <summary>
        /// 混流按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMixedFlow_Click(object sender, EventArgs e)
        {
            Mkvinfo mkvinfo = new Mkvinfo();
            mkvinfo.Get(MainPath + @"video\max.mkv");
        }

        #endregion

    }
}
