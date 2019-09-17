namespace MkvSubtitleBatchTool
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewTrack = new System.Windows.Forms.ListView();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSimpleFile = new System.Windows.Forms.TabPage();
            this.tabPageBatchFile = new System.Windows.Forms.TabPage();
            this.btnMixedFlow = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageSimpleFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(68, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(416, 21);
            this.txtPath.TabIndex = 0;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFile.Location = new System.Drawing.Point(490, 10);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "打开文件";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "MKV文件：";
            // 
            // listViewTrack
            // 
            this.listViewTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTrack.HideSelection = false;
            this.listViewTrack.Location = new System.Drawing.Point(6, 39);
            this.listViewTrack.Name = "listViewTrack";
            this.listViewTrack.Size = new System.Drawing.Size(559, 256);
            this.listViewTrack.TabIndex = 4;
            this.listViewTrack.UseCompatibleStateImageBehavior = false;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageSimpleFile);
            this.tabControlMain.Controls.Add(this.tabPageBatchFile);
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.Padding = new System.Drawing.Point(8, 3);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(579, 358);
            this.tabControlMain.TabIndex = 5;
            // 
            // tabPageSimpleFile
            // 
            this.tabPageSimpleFile.Controls.Add(this.btnMixedFlow);
            this.tabPageSimpleFile.Controls.Add(this.listViewTrack);
            this.tabPageSimpleFile.Controls.Add(this.btnOpenFile);
            this.tabPageSimpleFile.Controls.Add(this.txtPath);
            this.tabPageSimpleFile.Controls.Add(this.label1);
            this.tabPageSimpleFile.Location = new System.Drawing.Point(4, 22);
            this.tabPageSimpleFile.Name = "tabPageSimpleFile";
            this.tabPageSimpleFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSimpleFile.Size = new System.Drawing.Size(571, 332);
            this.tabPageSimpleFile.TabIndex = 0;
            this.tabPageSimpleFile.Text = "单文件处理";
            this.tabPageSimpleFile.UseVisualStyleBackColor = true;
            // 
            // tabPageBatchFile
            // 
            this.tabPageBatchFile.Location = new System.Drawing.Point(4, 22);
            this.tabPageBatchFile.Name = "tabPageBatchFile";
            this.tabPageBatchFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBatchFile.Size = new System.Drawing.Size(571, 332);
            this.tabPageBatchFile.TabIndex = 1;
            this.tabPageBatchFile.Text = "批量处理";
            this.tabPageBatchFile.UseVisualStyleBackColor = true;
            // 
            // btnMixedFlow
            // 
            this.btnMixedFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMixedFlow.Location = new System.Drawing.Point(490, 301);
            this.btnMixedFlow.Name = "btnMixedFlow";
            this.btnMixedFlow.Size = new System.Drawing.Size(75, 23);
            this.btnMixedFlow.TabIndex = 5;
            this.btnMixedFlow.Text = "混流";
            this.btnMixedFlow.UseVisualStyleBackColor = true;
            this.btnMixedFlow.Click += new System.EventHandler(this.btnMixedFlow_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tabControlMain);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MKV字幕批量提取与嵌入工具";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSimpleFile.ResumeLayout(false);
            this.tabPageSimpleFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewTrack;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageSimpleFile;
        private System.Windows.Forms.TabPage tabPageBatchFile;
        private System.Windows.Forms.Button btnMixedFlow;
    }
}

