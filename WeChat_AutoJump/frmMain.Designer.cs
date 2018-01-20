namespace WeChat_AutoJump
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.lblRatio = new System.Windows.Forms.Label();
            this.tbxRatio = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(20, 12);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(184, 74);
            this.btnAuto.TabIndex = 0;
            this.btnAuto.Text = "自动";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(20, 103);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(184, 74);
            this.btnStep.TabIndex = 1;
            this.btnStep.Text = "分析(&A)";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(20, 194);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(184, 74);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // picBox
            // 
            this.picBox.Location = new System.Drawing.Point(255, 12);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(324, 256);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBox.TabIndex = 3;
            this.picBox.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus.Location = new System.Drawing.Point(16, 289);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(154, 24);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "状态：未就绪";
            // 
            // timerStatus
            // 
            this.timerStatus.Enabled = true;
            this.timerStatus.Interval = 1000;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // lblRatio
            // 
            this.lblRatio.AutoSize = true;
            this.lblRatio.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRatio.Location = new System.Drawing.Point(251, 289);
            this.lblRatio.Name = "lblRatio";
            this.lblRatio.Size = new System.Drawing.Size(82, 24);
            this.lblRatio.TabIndex = 5;
            this.lblRatio.Text = "比率：";
            // 
            // tbxRatio
            // 
            this.tbxRatio.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxRatio.Location = new System.Drawing.Point(326, 289);
            this.tbxRatio.Name = "tbxRatio";
            this.tbxRatio.Size = new System.Drawing.Size(81, 29);
            this.tbxRatio.TabIndex = 6;
            this.tbxRatio.Text = "1.35";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 332);
            this.Controls.Add(this.tbxRatio);
            this.Controls.Add(this.lblRatio);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.btnAuto);
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "微信跳一跳";
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.Label lblRatio;
        private System.Windows.Forms.TextBox tbxRatio;
    }
}

