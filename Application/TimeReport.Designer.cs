namespace ComputerTimeTracker
{
  partial class TimeReport
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
      this._lblTimeStart = new System.Windows.Forms.Label();
      this._lblTextStart = new System.Windows.Forms.Label();
      this._btnOk = new System.Windows.Forms.Button();
      this._lblTimeWork = new System.Windows.Forms.Label();
      this._lblTextWork = new System.Windows.Forms.Label();
      this._lblTextCurrent = new System.Windows.Forms.Label();
      this._lblTimeCurrent = new System.Windows.Forms.Label();
      this._pnlPeriod1 = new System.Windows.Forms.Panel();
      this._pnlPeriod2 = new System.Windows.Forms.Panel();
      this._lblPeriodDuration1 = new System.Windows.Forms.Label();
      this._lblPeriodDuration2 = new System.Windows.Forms.Label();
      this._pnlPeriod1.SuspendLayout();
      this._pnlPeriod2.SuspendLayout();
      this.SuspendLayout();
      // 
      // _lblTimeStart
      // 
      this._lblTimeStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._lblTimeStart.AutoSize = true;
      this._lblTimeStart.Location = new System.Drawing.Point(100, 15);
      this._lblTimeStart.Name = "_lblTimeStart";
      this._lblTimeStart.Size = new System.Drawing.Size(57, 14);
      this._lblTimeStart.TabIndex = 0;
      this._lblTimeStart.Text = "00:00:00";
      // 
      // _lblTextStart
      // 
      this._lblTextStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._lblTextStart.AutoSize = true;
      this._lblTextStart.Location = new System.Drawing.Point(160, 15);
      this._lblTextStart.Name = "_lblTextStart";
      this._lblTextStart.Size = new System.Drawing.Size(83, 14);
      this._lblTextStart.TabIndex = 1;
      this._lblTextStart.Text = "Usage started";
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnOk.Location = new System.Drawing.Point(97, 90);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 2;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // _lblTimeWork
      // 
      this._lblTimeWork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._lblTimeWork.AutoSize = true;
      this._lblTimeWork.Location = new System.Drawing.Point(100, 60);
      this._lblTimeWork.Name = "_lblTimeWork";
      this._lblTimeWork.Size = new System.Drawing.Size(57, 14);
      this._lblTimeWork.TabIndex = 3;
      this._lblTimeWork.Text = "00:00:00";
      // 
      // _lblTextWork
      // 
      this._lblTextWork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._lblTextWork.AutoSize = true;
      this._lblTextWork.Location = new System.Drawing.Point(160, 60);
      this._lblTextWork.Name = "_lblTextWork";
      this._lblTextWork.Size = new System.Drawing.Size(64, 14);
      this._lblTextWork.TabIndex = 4;
      this._lblTextWork.Text = "Work time";
      // 
      // _lblTextCurrent
      // 
      this._lblTextCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._lblTextCurrent.AutoSize = true;
      this._lblTextCurrent.Location = new System.Drawing.Point(160, 35);
      this._lblTextCurrent.Name = "_lblTextCurrent";
      this._lblTextCurrent.Size = new System.Drawing.Size(76, 14);
      this._lblTextCurrent.TabIndex = 6;
      this._lblTextCurrent.Text = "Current time";
      // 
      // _lblTimeCurrent
      // 
      this._lblTimeCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._lblTimeCurrent.AutoSize = true;
      this._lblTimeCurrent.Location = new System.Drawing.Point(100, 35);
      this._lblTimeCurrent.Name = "_lblTimeCurrent";
      this._lblTimeCurrent.Size = new System.Drawing.Size(57, 14);
      this._lblTimeCurrent.TabIndex = 5;
      this._lblTimeCurrent.Text = "00:00:00";
      // 
      // _pnlPeriod1
      // 
      this._pnlPeriod1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pnlPeriod1.BackColor = System.Drawing.Color.Green;
      this._pnlPeriod1.Controls.Add(this._lblPeriodDuration1);
      this._pnlPeriod1.Location = new System.Drawing.Point(17, 22);
      this._pnlPeriod1.Name = "_pnlPeriod1";
      this._pnlPeriod1.Size = new System.Drawing.Size(75, 21);
      this._pnlPeriod1.TabIndex = 7;
      this._pnlPeriod1.Visible = false;
      // 
      // _pnlPeriod2
      // 
      this._pnlPeriod2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pnlPeriod2.BackColor = System.Drawing.Color.YellowGreen;
      this._pnlPeriod2.Controls.Add(this._lblPeriodDuration2);
      this._pnlPeriod2.Location = new System.Drawing.Point(17, 42);
      this._pnlPeriod2.Name = "_pnlPeriod2";
      this._pnlPeriod2.Size = new System.Drawing.Size(75, 21);
      this._pnlPeriod2.TabIndex = 8;
      this._pnlPeriod2.Visible = false;
      // 
      // _lblPeriodDuration1
      // 
      this._lblPeriodDuration1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblPeriodDuration1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
      this._lblPeriodDuration1.Location = new System.Drawing.Point(0, 0);
      this._lblPeriodDuration1.Name = "_lblPeriodDuration1";
      this._lblPeriodDuration1.Size = new System.Drawing.Size(75, 21);
      this._lblPeriodDuration1.TabIndex = 0;
      this._lblPeriodDuration1.Text = "1:30:30";
      this._lblPeriodDuration1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _lblPeriodDuration2
      // 
      this._lblPeriodDuration2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblPeriodDuration2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this._lblPeriodDuration2.Location = new System.Drawing.Point(0, 0);
      this._lblPeriodDuration2.Name = "_lblPeriodDuration2";
      this._lblPeriodDuration2.Size = new System.Drawing.Size(75, 21);
      this._lblPeriodDuration2.TabIndex = 1;
      this._lblPeriodDuration2.Text = "10:26";
      this._lblPeriodDuration2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // TimeReport
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.CancelButton = this._btnOk;
      this.ClientSize = new System.Drawing.Size(264, 125);
      this.Controls.Add(this._pnlPeriod2);
      this.Controls.Add(this._pnlPeriod1);
      this.Controls.Add(this._lblTextCurrent);
      this.Controls.Add(this._lblTimeCurrent);
      this.Controls.Add(this._lblTextWork);
      this.Controls.Add(this._lblTimeWork);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._lblTextStart);
      this.Controls.Add(this._lblTimeStart);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TimeReport";
      this.ShowIcon = false;
      this.Text = "Time Report";
      this._pnlPeriod1.ResumeLayout(false);
      this._pnlPeriod2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label _lblTimeStart;
    private System.Windows.Forms.Label _lblTextStart;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Label _lblTimeWork;
    private System.Windows.Forms.Label _lblTextWork;
    private System.Windows.Forms.Label _lblTextCurrent;
    private System.Windows.Forms.Label _lblTimeCurrent;
    private System.Windows.Forms.Panel _pnlPeriod1;
    private System.Windows.Forms.Panel _pnlPeriod2;
    private System.Windows.Forms.Label _lblPeriodDuration1;
    private System.Windows.Forms.Label _lblPeriodDuration2;
  }
}

