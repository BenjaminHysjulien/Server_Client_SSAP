
namespace Server_Client_SSAP
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtbox_IPAddress = new System.Windows.Forms.TextBox();
            this.TxtBox_URI_Info = new System.Windows.Forms.TextBox();
            this.TxtBox_UserName = new System.Windows.Forms.TextBox();
            this.TxtBox_Password = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.button1.Location = new System.Drawing.Point(54, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(300, 234);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Recording Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.button2.Location = new System.Drawing.Point(411, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(295, 234);
            this.button2.TabIndex = 1;
            this.button2.Text = "Collect Data ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtbox_IPAddress
            // 
            this.txtbox_IPAddress.Location = new System.Drawing.Point(7, 47);
            this.txtbox_IPAddress.Name = "txtbox_IPAddress";
            this.txtbox_IPAddress.Size = new System.Drawing.Size(100, 20);
            this.txtbox_IPAddress.TabIndex = 2;
            this.txtbox_IPAddress.Text = "192.168.1.65";
            // 
            // TxtBox_URI_Info
            // 
            this.TxtBox_URI_Info.Location = new System.Drawing.Point(12, 406);
            this.TxtBox_URI_Info.Name = "TxtBox_URI_Info";
            this.TxtBox_URI_Info.Size = new System.Drawing.Size(100, 20);
            this.TxtBox_URI_Info.TabIndex = 3;
            this.TxtBox_URI_Info.Text = "URI";
            // 
            // TxtBox_UserName
            // 
            this.TxtBox_UserName.Location = new System.Drawing.Point(191, 47);
            this.TxtBox_UserName.Name = "TxtBox_UserName";
            this.TxtBox_UserName.Size = new System.Drawing.Size(100, 20);
            this.TxtBox_UserName.TabIndex = 4;
            // 
            // TxtBox_Password
            // 
            this.TxtBox_Password.Location = new System.Drawing.Point(384, 47);
            this.TxtBox_Password.Name = "TxtBox_Password";
            this.TxtBox_Password.Size = new System.Drawing.Size(100, 20);
            this.TxtBox_Password.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtBox_Password);
            this.Controls.Add(this.TxtBox_UserName);
            this.Controls.Add(this.TxtBox_URI_Info);
            this.Controls.Add(this.txtbox_IPAddress);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtbox_IPAddress;
        private System.Windows.Forms.TextBox TxtBox_URI_Info;
        private System.Windows.Forms.TextBox TxtBox_UserName;
        private System.Windows.Forms.TextBox TxtBox_Password;
    }
}

