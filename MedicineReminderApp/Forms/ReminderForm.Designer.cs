namespace MedicineReminderApp
{
    partial class ReminderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblToday = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.dgvReminders = new System.Windows.Forms.DataGridView();
            this.btnTaken = new System.Windows.Forms.Button();
            this.btnMissed = new System.Windows.Forms.Button();
            this.btnRefreshNow = new System.Windows.Forms.Button();
            this.reminderTimer = new System.Windows.Forms.Timer(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Today\'s medicine reminders";
            // 
            // lblToday
            // 
            this.lblToday.AutoSize = true;
            this.lblToday.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblToday.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblToday.Location = new System.Drawing.Point(16, 40);
            this.lblToday.Name = "lblToday";
            this.lblToday.Size = new System.Drawing.Size(48, 19);
            this.lblToday.TabIndex = 1;
            this.lblToday.Text = "Today:";
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHint.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblHint.Location = new System.Drawing.Point(16, 66);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(420, 15);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = "Select a row and press Taken or Missed. The list refreshes automatically.";
            // 
            // dgvReminders
            // 
            this.dgvReminders.AllowUserToAddRows = false;
            this.dgvReminders.AllowUserToDeleteRows = false;
            this.dgvReminders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReminders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReminders.BackgroundColor = System.Drawing.Color.White;
            this.dgvReminders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReminders.Location = new System.Drawing.Point(12, 120);
            this.dgvReminders.MultiSelect = false;
            this.dgvReminders.Name = "dgvReminders";
            this.dgvReminders.ReadOnly = true;
            this.dgvReminders.RowHeadersVisible = false;
            this.dgvReminders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReminders.Size = new System.Drawing.Size(660, 260);
            this.dgvReminders.TabIndex = 3;
            // 
            // btnTaken
            // 
            this.btnTaken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTaken.BackColor = UiTheme.Accent;
            this.btnTaken.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaken.ForeColor = System.Drawing.Color.White;
            this.btnTaken.Location = new System.Drawing.Point(12, 395);
            this.btnTaken.Name = "btnTaken";
            this.btnTaken.Size = new System.Drawing.Size(120, 32);
            this.btnTaken.TabIndex = 4;
            this.btnTaken.Text = "Mark as Taken";
            this.btnTaken.UseVisualStyleBackColor = false;
            this.btnTaken.Click += new System.EventHandler(this.btnTaken_Click);
            // 
            // btnMissed
            // 
            this.btnMissed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMissed.BackColor = UiTheme.PrimaryDark;
            this.btnMissed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMissed.ForeColor = System.Drawing.Color.White;
            this.btnMissed.Location = new System.Drawing.Point(145, 395);
            this.btnMissed.Name = "btnMissed";
            this.btnMissed.Size = new System.Drawing.Size(120, 32);
            this.btnMissed.TabIndex = 5;
            this.btnMissed.Text = "Mark as Missed";
            this.btnMissed.UseVisualStyleBackColor = false;
            this.btnMissed.Click += new System.EventHandler(this.btnMissed_Click);
            // 
            // btnRefreshNow
            // 
            this.btnRefreshNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshNow.Location = new System.Drawing.Point(552, 395);
            this.btnRefreshNow.Name = "btnRefreshNow";
            this.btnRefreshNow.Size = new System.Drawing.Size(120, 32);
            this.btnRefreshNow.TabIndex = 6;
            this.btnRefreshNow.Text = "Refresh now";
            this.btnRefreshNow.UseVisualStyleBackColor = true;
            this.btnRefreshNow.Click += new System.EventHandler(this.btnRefreshNow_Click);
            // 
            // reminderTimer
            // 
            this.reminderTimer.Interval = 60000;
            this.reminderTimer.Tick += new System.EventHandler(this.reminderTimer_Tick);
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = UiTheme.PrimaryDark;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblToday);
            this.panelHeader.Controls.Add(this.lblHint);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(684, 100);
            this.panelHeader.TabIndex = 7;
            // 
            // ReminderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = UiTheme.LightBackground;
            this.ClientSize = new System.Drawing.Size(684, 441);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.btnRefreshNow);
            this.Controls.Add(this.btnMissed);
            this.Controls.Add(this.btnTaken);
            this.Controls.Add(this.dgvReminders);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ReminderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reminders";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReminderForm_FormClosed);
            this.Load += new System.EventHandler(this.ReminderForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblToday;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.DataGridView dgvReminders;
        private System.Windows.Forms.Button btnTaken;
        private System.Windows.Forms.Button btnMissed;
        private System.Windows.Forms.Button btnRefreshNow;
        private System.Windows.Forms.Timer reminderTimer;
        private System.Windows.Forms.Panel panelHeader;
    }
}
