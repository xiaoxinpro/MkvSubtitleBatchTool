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
using System.IO;

namespace MkvSubtitleBatchTool
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 当前目录含"\"
        /// </summary>
        private string MainPath;
        private Mkvinfo ObjMkvinfo;

        #region 初始化相关
        /// <summary>
        /// 初始化
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            MainPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ObjMkvinfo = new Mkvinfo(GetMkvinfoDoneCallback);
        }

        /// <summary>
        /// 加载窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text += @" V" + Application.ProductVersion.ToString();

            InitTrackListView(listViewTrack);
        }
        #endregion

        #region 列表相关函数
        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="listView"></param>
        private void InitTrackListView(ListView listView)
        {
            //基本属性设置
            listView.Clear();
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.CheckBoxes = true;
            listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView.View = View.Details;

            //创建列表头
            listView.Columns.Add("轨道ID", 60, HorizontalAlignment.Center);
            listView.Columns.Add("类型", 80, HorizontalAlignment.Left);
            listView.Columns.Add("编码格式", 130, HorizontalAlignment.Left);
            listView.Columns.Add("语言", 50, HorizontalAlignment.Left);
            listView.Columns.Add("默认", 50, HorizontalAlignment.Left);
            listView.Columns.Add("名称", 100, HorizontalAlignment.Left);

            //自动列宽
            listView.Columns[5].Width = -2;//根据标题设置宽度
        }

        /// <summary>
        /// 添加数据到轨道列表中
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="track"></param>
        private void AddTrackListView(ListView listView, params MkvinfoTrack[] track)
        {
            listView.BeginUpdate();
            foreach (MkvinfoTrack item in track)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = item.TrackID.ToString();
                listViewItem.SubItems.Add(item.TrackType);
                listViewItem.SubItems.Add(item.CodecID);
                listViewItem.SubItems.Add(item.Language);
                listViewItem.SubItems.Add(item.IsDefault.ToString());
                listViewItem.SubItems.Add(item.Name);
                listViewItem.Checked = true;
                listView.Items.Add(listViewItem);
            }
            listView.EndUpdate();
        }
        #endregion

        #region 窗体控件相关

        /// <summary>
        /// 打开文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string filePath = txtPath.Text.Trim();
            if (!File.Exists(filePath) || ObjMkvinfo.FilePath == filePath)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件对话框
                openFileDialog.Filter = "MKV文件(*.mkv)|*.mkv|所有文件(*.*)|*.*";
                openFileDialog.RestoreDirectory = true;//对话框关闭时恢复原目录
                openFileDialog.Title = "打开MKV文件";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName.Trim();
                    txtPath.Text = filePath;
                }
                else
                {
                    return;
                }
            }
            Button button = (Button)sender;
            button.Enabled = false;
            button.Text = "加载中";
            ObjMkvinfo.Get(filePath);
        }

        /// <summary>
        /// 混流按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMixedFlow_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region MKV信息获取
        /// <summary>
        /// 获取MKV信息回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetMkvinfoDoneCallback(object sender, EventArgs e)
        {
            Mkvinfo mkvinfo = (Mkvinfo)sender;
            Invoke(new Action(() =>
            {
                btnOpenFile.Enabled = true;
                btnOpenFile.Text = "打开文件";
                InitTrackListView(listViewTrack);
                AddTrackListView(listViewTrack, mkvinfo.Tracks);
            }));
        }
        #endregion

    }
}
