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
      this.SuspendLayout();
      // 
      // _lblTimeStart
      // 
      this._lblTimeStart.AutoSize = true;
      this._lblTimeStart.Location = new System.Drawing.Point(20, 30);
      this._lblTimeStart.Name = "_lblTimeStart";
      this._lblTimeStart.Size = new System.Drawing.Size(49, 13);
      this._lblTimeStart.TabIndex = 0;
      this._lblTimeStart.Text = "00:00:00";
      // 
      // _lblTextStart
      // 
      this._lblTextStart.AutoSize = true;
      this._lblTextStart.Location = new System.Drawing.Point(90, 30);
      this._lblTextStart.Name = "_lblTextStart";
      this._lblTextStart.Size = new System.Drawing.Size(119, 13);
      this._lblTextStart.TabIndex = 1;
      this._lblTextStart.Text = "Computer usage started";
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnOk.Location = new System.Drawing.Point(87, 90);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 2;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // TimeReport
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.CancelButton = this._btnOk;
      this.ClientSize = new System.Drawing.Size(244, 125);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._lblTextStart);
      this.Controls.Add(this._lblTimeStart);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TimeReport";
      this.ShowIcon = false;
      this.Text = "Time Report";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label _lblTimeStart;
    private System.Windows.Forms.Label _lblTextStart;
    private System.Windows.Forms.Button _btnOk;
  }
}

