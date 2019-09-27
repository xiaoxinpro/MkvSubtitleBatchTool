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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MkvSubtitleBatchTool
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 当前目录含"\"
        /// </summary>
        private string MainPath;
        private Mkvinfo ObjMkvinfo;
        private Mkvextract ObjMkvExtract;

        #region 初始化相关
        /// <summary>
        /// 初始化
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            MainPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ObjMkvinfo = new Mkvinfo(GetMkvinfoDoneCallback);
            ObjMkvExtract = new Mkvextract(MkvExtractCallback);
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
            listView.Columns.Add("轨道ID", 55, HorizontalAlignment.Center);
            listView.Columns.Add("类型", 75, HorizontalAlignment.Left);
            listView.Columns.Add("编码格式", 110, HorizontalAlignment.Left);
            listView.Columns.Add("语言", 50, HorizontalAlignment.Left);
            listView.Columns.Add("默认", 50, HorizontalAlignment.Left);
            listView.Columns.Add("名称", 80, HorizontalAlignment.Left);
            listView.Columns.Add("路径", 70, HorizontalAlignment.Left);

            //自动列宽
            listView.Columns[6].Width = -2;//根据标题设置宽度
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
                listViewItem.SubItems.Add(string.IsNullOrWhiteSpace(item.Path) ? "" : item.Path);
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

        #region 轨道列表菜单相关函数
        /// <summary>
        /// 菜单打开时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuListViewTarck_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menuStrip = (ContextMenuStrip)sender;
            if (listViewTrack.Items.Count > 0)
            {
                int numTag = 0;
                if (listViewTrack.SelectedItems.Count > 0 && listViewTrack.SelectedItems[0].SubItems[1].Text == "subtitles")
                {
                    numTag = 2;
                }
                for (int i = 0; i < menuStrip.Items.Count; i++)
                {
                    menuStrip.Items[i].Visible = (Convert.ToInt32(menuStrip.Items[i].Tag) <= numTag);
                    if (menuStrip.Items[i].Name == "toolStripTextBoxName")
                    {
                        menuStrip.Items[i].Text = "修改名称";
                    }
                    else if (menuStrip.Items[i].Name == "toolStripComboBoxLanguage")
                    {
                        menuStrip.Items[i].Text = "修改语言";
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 触发名称修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBoxName_Click(object sender, EventArgs e)
        {
            ToolStripTextBox text = (ToolStripTextBox)sender;
            if (text.Text == "修改名称")
            {
                text.Text = listViewTrack.SelectedItems[0].SubItems[5].Text;
                text.Select(text.TextLength, 0);
            }
        }

        /// <summary>
        /// 名称修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBoxName_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text = (ToolStripTextBox)sender;
            if (text.Text != "修改名称")
            {
                listViewTrack.SelectedItems[0].SubItems[5].Text = text.Text;
            }
        }

        /// <summary>
        /// 设置语言选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox combo = (ToolStripComboBox)sender;
            string strLanguage = Regex.Match(combo.Text, @"(?<=\()\w+(?=\))").Value;
            if (strLanguage.Length > 1)
            {
                listViewTrack.SelectedItems[0].SubItems[3].Text = strLanguage.Trim();
            }
        }

        /// <summary>
        /// 设为默认按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDefault_Click(object sender, EventArgs e)
        {
            string defaultTarckId = listViewTrack.SelectedItems[0].Text;
            for (int i = 0; i < listViewTrack.Items.Count; i++)
            {
                if (listViewTrack.Items[i].SubItems[1].Text == "subtitles")
                {
                    listViewTrack.Items[i].SubItems[4].Text = (listViewTrack.Items[i].Text == defaultTarckId).ToString();
                }
            }
        }

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemReload_Click(object sender, EventArgs e)
        {
            Button button = btnOpenFile;
            button.Enabled = false;
            button.Text = "加载中";
            listViewTrack.Items.Clear();
            ObjMkvinfo.Get(ObjMkvinfo.FilePath);
        }

        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemOutput_Click(object sender, EventArgs e)
        {
            ListViewItem listViewItem = listViewTrack.SelectedItems[0];
            MkvinfoTrack mkvinfoTrack = new MkvinfoTrack(ObjMkvinfo.Tracks[Convert.ToInt32(listViewItem.Text)]);
            mkvinfoTrack.TrackID = Convert.ToInt32(listViewItem.Text);
            mkvinfoTrack.Language = listViewItem.SubItems[3].Text;
            mkvinfoTrack.IsDefault = Convert.ToBoolean(listViewItem.SubItems[4].Text);
            mkvinfoTrack.Name = listViewItem.SubItems[5].Text;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "导出字幕文件";
            sfd.InitialDirectory = Path.GetDirectoryName(ObjMkvinfo.FilePath);
            sfd.Filter = @"字幕文件|*." + Regex.Match(mkvinfoTrack.CodecID, @"(?<=\/)\w+").Value.ToLower() + "|文本文件|*.txt|所有文件|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                // 输出文件，调用导出类
                Mkvextract ObjMkvExtract = new Mkvextract(MkvExtractCallback);
                ObjMkvExtract.MkvFilePath = ObjMkvinfo.FilePath;
                ObjMkvExtract.SaveFilePath = path;
                ObjMkvExtract.Track = mkvinfoTrack;
                if (ObjMkvExtract.Start(out string msg))
                {
                    // 开始转换
                    progressTrack.Maximum = 100;
                    progressTrack.Value = 0;
                    progressTrack.Visible = true;
                    progressTrack.BringToFront();
                }
            }
        }

        /// <summary>
        /// 替换轨道按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemReplace_Click(object sender, EventArgs e)
        {
            ListView listView = listViewTrack;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择替换字幕文件";
            ofd.Filter = @"所有字幕文件|*.ass;*.ssa;*.sub;*.srt;*.usf;*.xml;*.idx;*.vtt|ASS文本字幕|*.ass|SSA文本字幕|*.ssa|SRT文本字幕|*.srt;|USF文本字幕|*.usf;*.xml|PGS/SUB字幕|*.sub";
            ofd.Multiselect = false; ;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                listView.SelectedItems[0].SubItems[6].Text = ofd.FileName.Trim();
                listView.SelectedItems[0].Checked = true;
            }
        }

        /// <summary>
        /// 添加轨道按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            ListView listView = listViewTrack;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择添加字幕文件";
            ofd.Filter = @"所有字幕文件|*.ass;*.ssa;*.sub;*.srt;*.usf;*.xml;*.idx;*.vtt|ASS文本字幕|*.ass|SSA文本字幕|*.ssa|SRT文本字幕|*.srt;|USF文本字幕|*.usf;*.xml|PGS/SUB字幕|*.sub";
            ofd.Multiselect = true; ;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<MkvinfoTrack> listMkvinfoTrack = new List<MkvinfoTrack>();
                foreach (string strPath in ofd.FileNames)
                {
                    listMkvinfoTrack.Add(new MkvinfoTrack(listView.Items.Count + listMkvinfoTrack.Count));
                    listMkvinfoTrack[listMkvinfoTrack.Count - 1].Path = strPath.Trim();
                }
                AddTrackListView(listView, listMkvinfoTrack.ToArray());
            }
        }

        #endregion

        #region 导出轨道
        /// <summary>
        /// 轨道导出流程中的回调函数
        /// </summary>
        /// <param name="sender">Mkvextract原型</param>
        /// <param name="rate">进度0-100</param>
        void MkvExtractCallback(object sender, int rate)
        {
            Mkvextract objMkvExtract = (Mkvextract)sender;
            this.Invoke(new Action(() =>
            {
                if (rate < 0)
                {
                    MessageBox.Show(objMkvExtract.Error);
                }
                else if (rate < 100)
                {
                    progressTrack.Value = rate;
                }
                else
                {
                    progressTrack.Value = progressTrack.Maximum;
                    if (MessageBox.Show("字幕文件导出完成，是否需要打开输出文件夹？", "导出成功", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Path.GetDirectoryName(objMkvExtract.SaveFilePath));
                    }
                    progressTrack.Visible = false;
                }
            }));
        }
        #endregion
    }
}
