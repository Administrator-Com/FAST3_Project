namespace FAST3_ServiceUI
{
    partial class InterFaceUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InterFaceUI));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnStop = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageUpLoad = new System.Windows.Forms.TabPage();
            this.panelBox = new System.Windows.Forms.Panel();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.comboBoxMsgInterface = new System.Windows.Forms.ComboBox();
            this.dataGridViewInterface = new System.Windows.Forms.DataGridView();
            this.notifyIconMini = new System.Windows.Forms.NotifyIcon(this.components);
            this.timerCheck = new System.Windows.Forms.Timer(this.components);
            this.ColumnInterfaceKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInterfaceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabPageBelone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageUpLoad.SuspendLayout();
            this.panelBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterface)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnStart,
            this.toolStripBtnStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(621, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripBtnStart
            // 
            this.toolStripBtnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnStart.Image")));
            this.toolStripBtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnStart.Name = "toolStripBtnStart";
            this.toolStripBtnStart.Size = new System.Drawing.Size(60, 22);
            this.toolStripBtnStart.Text = "全部开启";
            // 
            // toolStripBtnStop
            // 
            this.toolStripBtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnStop.Image")));
            this.toolStripBtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnStop.Name = "toolStripBtnStop";
            this.toolStripBtnStop.Size = new System.Drawing.Size(60, 22);
            this.toolStripBtnStop.Text = "全部关闭";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageUpLoad);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(621, 425);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageUpLoad
            // 
            this.tabPageUpLoad.Controls.Add(this.panelBox);
            this.tabPageUpLoad.Controls.Add(this.dataGridViewInterface);
            this.tabPageUpLoad.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpLoad.Name = "tabPageUpLoad";
            this.tabPageUpLoad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpLoad.Size = new System.Drawing.Size(613, 399);
            this.tabPageUpLoad.TabIndex = 0;
            this.tabPageUpLoad.Text = "数据上传接口";
            this.tabPageUpLoad.UseVisualStyleBackColor = true;
            // 
            // panelBox
            // 
            this.panelBox.Controls.Add(this.textBoxMsg);
            this.panelBox.Controls.Add(this.comboBoxMsgInterface);
            this.panelBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBox.Location = new System.Drawing.Point(3, 158);
            this.panelBox.Name = "panelBox";
            this.panelBox.Size = new System.Drawing.Size(607, 238);
            this.panelBox.TabIndex = 5;
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMsg.Location = new System.Drawing.Point(0, 24);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Size = new System.Drawing.Size(607, 214);
            this.textBoxMsg.TabIndex = 2;
            // 
            // comboBoxMsgInterface
            // 
            this.comboBoxMsgInterface.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxMsgInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMsgInterface.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxMsgInterface.FormattingEnabled = true;
            this.comboBoxMsgInterface.Location = new System.Drawing.Point(0, 0);
            this.comboBoxMsgInterface.Name = "comboBoxMsgInterface";
            this.comboBoxMsgInterface.Size = new System.Drawing.Size(607, 24);
            this.comboBoxMsgInterface.TabIndex = 4;
            // 
            // dataGridViewInterface
            // 
            this.dataGridViewInterface.AllowUserToAddRows = false;
            this.dataGridViewInterface.AllowUserToDeleteRows = false;
            this.dataGridViewInterface.AllowUserToResizeColumns = false;
            this.dataGridViewInterface.AllowUserToResizeRows = false;
            this.dataGridViewInterface.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewInterface.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInterface.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnInterfaceKey,
            this.ColumnInterfaceName,
            this.ColumnState,
            this.ColumnButton,
            this.tabPageBelone});
            this.dataGridViewInterface.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewInterface.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewInterface.MultiSelect = false;
            this.dataGridViewInterface.Name = "dataGridViewInterface";
            this.dataGridViewInterface.ReadOnly = true;
            this.dataGridViewInterface.RowHeadersVisible = false;
            this.dataGridViewInterface.RowTemplate.Height = 23;
            this.dataGridViewInterface.Size = new System.Drawing.Size(607, 155);
            this.dataGridViewInterface.TabIndex = 1;
            this.dataGridViewInterface.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewInterface_CellClick);
            // 
            // notifyIconMini
            // 
            this.notifyIconMini.BalloonTipText = "接口管理系统";
            this.notifyIconMini.BalloonTipTitle = "接口系统最小化至菜单栏";
            this.notifyIconMini.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMini.Icon")));
            this.notifyIconMini.Text = "接口管理系统";
            this.notifyIconMini.Visible = true;
            this.notifyIconMini.DoubleClick += new System.EventHandler(this.InterFaceUI_SizeChanged);
            // 
            // timerCheck
            // 
            this.timerCheck.Interval = 1000;
            this.timerCheck.Tick += new System.EventHandler(this.TimerCheck_Tick);
            // 
            // ColumnInterfaceKey
            // 
            this.ColumnInterfaceKey.HeaderText = "接口主键名";
            this.ColumnInterfaceKey.Name = "ColumnInterfaceKey";
            this.ColumnInterfaceKey.ReadOnly = true;
            this.ColumnInterfaceKey.Visible = false;
            // 
            // ColumnInterfaceName
            // 
            this.ColumnInterfaceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnInterfaceName.HeaderText = "接口名称";
            this.ColumnInterfaceName.Name = "ColumnInterfaceName";
            this.ColumnInterfaceName.ReadOnly = true;
            // 
            // ColumnState
            // 
            this.ColumnState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnState.HeaderText = "接口状态";
            this.ColumnState.Name = "ColumnState";
            this.ColumnState.ReadOnly = true;
            // 
            // ColumnButton
            // 
            this.ColumnButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnButton.HeaderText = "操作";
            this.ColumnButton.Name = "ColumnButton";
            this.ColumnButton.ReadOnly = true;
            this.ColumnButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tabPageBelone
            // 
            this.tabPageBelone.HeaderText = "tabPageBelone";
            this.tabPageBelone.Name = "tabPageBelone";
            this.tabPageBelone.ReadOnly = true;
            this.tabPageBelone.Visible = false;
            // 
            // InterFaceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 450);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InterFaceUI";
            this.Text = "接口操作界面";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageUpLoad.ResumeLayout(false);
            this.panelBox.ResumeLayout(false);
            this.panelBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterface)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripBtnStart;
        private System.Windows.Forms.ToolStripButton toolStripBtnStop;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageUpLoad;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.DataGridView dataGridViewInterface;
        private System.Windows.Forms.NotifyIcon notifyIconMini;
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.Panel panelBox;
        private System.Windows.Forms.ComboBox comboBoxMsgInterface;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInterfaceKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInterfaceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnState;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn tabPageBelone;
    }
}

