namespace UI_V2
{
    partial class AppWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            WebView_Main = new Microsoft.Web.WebView2.WinForms.WebView2();
            WebView_TopBar = new Microsoft.Web.WebView2.WinForms.WebView2();
            AppLayout = new TableLayoutPanel();
            TopBarBottomBorder = new Panel();
            ((System.ComponentModel.ISupportInitialize)WebView_Main).BeginInit();
            ((System.ComponentModel.ISupportInitialize)WebView_TopBar).BeginInit();
            AppLayout.SuspendLayout();
            SuspendLayout();
            // 
            // WebView_Main
            // 
            WebView_Main.AccessibleRole = AccessibleRole.None;
            WebView_Main.AllowExternalDrop = false;
            WebView_Main.BackColor = Color.FromArgb(33, 33, 33);
            WebView_Main.CreationProperties = null;
            WebView_Main.DefaultBackgroundColor = Color.FromArgb(33, 33, 33);
            WebView_Main.Dock = DockStyle.Fill;
            WebView_Main.ForeColor = Color.White;
            WebView_Main.Location = new Point(0, 31);
            WebView_Main.Margin = new Padding(0);
            WebView_Main.Name = "WebView_Main";
            WebView_Main.Size = new Size(1280, 689);
            WebView_Main.TabIndex = 0;
            WebView_Main.TabStop = false;
            WebView_Main.ZoomFactor = 1D;
            // 
            // WebView_TopBar
            // 
            WebView_TopBar.AllowExternalDrop = false;
            WebView_TopBar.BackColor = Color.FromArgb(26, 26, 26);
            WebView_TopBar.CreationProperties = null;
            WebView_TopBar.DefaultBackgroundColor = Color.FromArgb(26, 26, 26);
            WebView_TopBar.Dock = DockStyle.Fill;
            WebView_TopBar.ForeColor = Color.White;
            WebView_TopBar.Location = new Point(0, 0);
            WebView_TopBar.Margin = new Padding(0);
            WebView_TopBar.Name = "WebView_TopBar";
            WebView_TopBar.Size = new Size(1280, 30);
            WebView_TopBar.TabIndex = 1;
            WebView_TopBar.TabStop = false;
            WebView_TopBar.ZoomFactor = 1D;
            // 
            // AppLayout
            // 
            AppLayout.BackColor = Color.Transparent;
            AppLayout.ColumnCount = 1;
            AppLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            AppLayout.Controls.Add(WebView_Main, 0, 2);
            AppLayout.Controls.Add(WebView_TopBar, 0, 0);
            AppLayout.Controls.Add(TopBarBottomBorder, 0, 1);
            AppLayout.Dock = DockStyle.Fill;
            AppLayout.Location = new Point(0, 0);
            AppLayout.Margin = new Padding(0);
            AppLayout.Name = "AppLayout";
            AppLayout.RowCount = 3;
            AppLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            AppLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            AppLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            AppLayout.Size = new Size(1280, 720);
            AppLayout.TabIndex = 2;
            // 
            // TopBarBottomBorder
            // 
            TopBarBottomBorder.BackColor = Color.FromArgb(68, 68, 68);
            TopBarBottomBorder.CausesValidation = false;
            TopBarBottomBorder.Dock = DockStyle.Fill;
            TopBarBottomBorder.ForeColor = Color.White;
            TopBarBottomBorder.Location = new Point(0, 30);
            TopBarBottomBorder.Margin = new Padding(0);
            TopBarBottomBorder.Name = "TopBarBottomBorder";
            TopBarBottomBorder.Size = new Size(1280, 1);
            TopBarBottomBorder.TabIndex = 2;
            // 
            // AppWindow
            // 
            AccessibleRole = AccessibleRole.None;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(33, 33, 33);
            ClientSize = new Size(1280, 720);
            Controls.Add(AppLayout);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1280, 720);
            Name = "AppWindow";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)WebView_Main).EndInit();
            ((System.ComponentModel.ISupportInitialize)WebView_TopBar).EndInit();
            AppLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 WebView_Main;
        private Microsoft.Web.WebView2.WinForms.WebView2 WebView_TopBar;
        private TableLayoutPanel AppLayout;
        private Panel TopBarBottomBorder;
    }
}
