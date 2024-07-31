namespace Projeto
{
    partial class FormMain
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
            Panel PanelMain;
            Panel PanelContent;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            Panel PanelTopbar;
            Panel PanelFilter;
            Label LblFilter;
            Label _LblStatus;
            PanelCountry = new ControlCountry();
            PanelContentSeparator = new Panel();
            PanelCountries = new Panel();
            ScrollPanelCountries = new FlowLayoutPanel();
            PanelTopbarNameSeparator = new Panel();
            LblSelectedCountry = new Label();
            PanelTopbarSeparator = new Panel();
            PanelFilterSeparator = new Panel();
            TxtFilter = new TextBox();
            PanelBottomBar = new Panel();
            LblStatus = new Label();
            LblProgress = new Label();
            ProgressBar = new ProgressBar();
            PanelBottombarSeparator = new Panel();
            PanelMain = new Panel();
            PanelContent = new Panel();
            PanelTopbar = new Panel();
            PanelFilter = new Panel();
            LblFilter = new Label();
            _LblStatus = new Label();
            PanelMain.SuspendLayout();
            PanelContent.SuspendLayout();
            PanelCountries.SuspendLayout();
            PanelTopbar.SuspendLayout();
            PanelFilter.SuspendLayout();
            PanelBottomBar.SuspendLayout();
            SuspendLayout();
            // 
            // PanelMain
            // 
            PanelMain.BorderStyle = BorderStyle.FixedSingle;
            PanelMain.Controls.Add(PanelContent);
            PanelMain.Controls.Add(PanelContentSeparator);
            PanelMain.Controls.Add(PanelCountries);
            PanelMain.Controls.Add(PanelTopbar);
            PanelMain.Controls.Add(PanelBottomBar);
            PanelMain.Dock = DockStyle.Fill;
            PanelMain.Location = new Point(0, 0);
            PanelMain.Margin = new Padding(0);
            PanelMain.Name = "PanelMain";
            PanelMain.Size = new Size(1120, 630);
            PanelMain.TabIndex = 0;
            // 
            // PanelContent
            // 
            PanelContent.AutoScroll = true;
            PanelContent.BackColor = Color.White;
            PanelContent.Controls.Add(PanelCountry);
            PanelContent.Dock = DockStyle.Fill;
            PanelContent.Location = new Point(202, 60);
            PanelContent.Name = "PanelContent";
            PanelContent.Size = new Size(916, 528);
            PanelContent.TabIndex = 7;
            // 
            // PanelCountry
            // 
            PanelCountry.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PanelCountry.AutoScroll = true;
            PanelCountry.BackColor = Color.White;
            PanelCountry.Location = new Point(6, 7);
            PanelCountry.Name = "PanelCountry";
            PanelCountry.Size = new Size(899, 528);
            PanelCountry.TabIndex = 6;
            // 
            // PanelContentSeparator
            // 
            PanelContentSeparator.BackColor = Color.Black;
            PanelContentSeparator.Dock = DockStyle.Left;
            PanelContentSeparator.Location = new Point(200, 60);
            PanelContentSeparator.Name = "PanelContentSeparator";
            PanelContentSeparator.Size = new Size(2, 528);
            PanelContentSeparator.TabIndex = 4;
            // 
            // PanelCountries
            // 
            PanelCountries.Controls.Add(ScrollPanelCountries);
            PanelCountries.Dock = DockStyle.Left;
            PanelCountries.Location = new Point(0, 60);
            PanelCountries.Name = "PanelCountries";
            PanelCountries.Size = new Size(200, 528);
            PanelCountries.TabIndex = 3;
            PanelCountries.Resize += PanelCountries_Resize;
            // 
            // ScrollPanelCountries
            // 
            ScrollPanelCountries.Location = new Point(0, 0);
            ScrollPanelCountries.Name = "ScrollPanelCountries";
            ScrollPanelCountries.Size = new Size(200, 568);
            ScrollPanelCountries.TabIndex = 1;
            // 
            // PanelTopbar
            // 
            PanelTopbar.Controls.Add(PanelTopbarNameSeparator);
            PanelTopbar.Controls.Add(LblSelectedCountry);
            PanelTopbar.Controls.Add(PanelTopbarSeparator);
            PanelTopbar.Controls.Add(PanelFilter);
            PanelTopbar.Dock = DockStyle.Top;
            PanelTopbar.Location = new Point(0, 0);
            PanelTopbar.Name = "PanelTopbar";
            PanelTopbar.Size = new Size(1118, 60);
            PanelTopbar.TabIndex = 0;
            // 
            // PanelTopbarNameSeparator
            // 
            PanelTopbarNameSeparator.BackColor = Color.Black;
            PanelTopbarNameSeparator.Dock = DockStyle.Bottom;
            PanelTopbarNameSeparator.Location = new Point(202, 58);
            PanelTopbarNameSeparator.Name = "PanelTopbarNameSeparator";
            PanelTopbarNameSeparator.Size = new Size(916, 2);
            PanelTopbarNameSeparator.TabIndex = 7;
            // 
            // LblSelectedCountry
            // 
            LblSelectedCountry.Dock = DockStyle.Fill;
            LblSelectedCountry.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblSelectedCountry.Location = new Point(202, 0);
            LblSelectedCountry.Name = "LblSelectedCountry";
            LblSelectedCountry.Size = new Size(916, 60);
            LblSelectedCountry.TabIndex = 6;
            LblSelectedCountry.Text = "(Select a Country)";
            LblSelectedCountry.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PanelTopbarSeparator
            // 
            PanelTopbarSeparator.BackColor = Color.Black;
            PanelTopbarSeparator.Dock = DockStyle.Left;
            PanelTopbarSeparator.Location = new Point(200, 0);
            PanelTopbarSeparator.Name = "PanelTopbarSeparator";
            PanelTopbarSeparator.Size = new Size(2, 60);
            PanelTopbarSeparator.TabIndex = 5;
            // 
            // PanelFilter
            // 
            PanelFilter.Controls.Add(PanelFilterSeparator);
            PanelFilter.Controls.Add(LblFilter);
            PanelFilter.Controls.Add(TxtFilter);
            PanelFilter.Dock = DockStyle.Left;
            PanelFilter.Location = new Point(0, 0);
            PanelFilter.Name = "PanelFilter";
            PanelFilter.Size = new Size(200, 60);
            PanelFilter.TabIndex = 0;
            // 
            // PanelFilterSeparator
            // 
            PanelFilterSeparator.BackColor = Color.Black;
            PanelFilterSeparator.Dock = DockStyle.Bottom;
            PanelFilterSeparator.Location = new Point(0, 58);
            PanelFilterSeparator.Name = "PanelFilterSeparator";
            PanelFilterSeparator.Size = new Size(200, 2);
            PanelFilterSeparator.TabIndex = 8;
            // 
            // LblFilter
            // 
            LblFilter.AutoSize = true;
            LblFilter.Font = new Font("Segoe UI", 12F);
            LblFilter.Location = new Point(3, 22);
            LblFilter.Name = "LblFilter";
            LblFilter.Size = new Size(48, 21);
            LblFilter.TabIndex = 4;
            LblFilter.Text = "Filter:";
            // 
            // TxtFilter
            // 
            TxtFilter.Font = new Font("Segoe UI", 12F);
            TxtFilter.Location = new Point(57, 19);
            TxtFilter.Name = "TxtFilter";
            TxtFilter.Size = new Size(137, 29);
            TxtFilter.TabIndex = 4;
            TxtFilter.TextChanged += TxtFilter_TextChanged;
            // 
            // PanelBottomBar
            // 
            PanelBottomBar.Controls.Add(LblStatus);
            PanelBottomBar.Controls.Add(_LblStatus);
            PanelBottomBar.Controls.Add(LblProgress);
            PanelBottomBar.Controls.Add(ProgressBar);
            PanelBottomBar.Controls.Add(PanelBottombarSeparator);
            PanelBottomBar.Dock = DockStyle.Bottom;
            PanelBottomBar.Location = new Point(0, 588);
            PanelBottomBar.Name = "PanelBottomBar";
            PanelBottomBar.Size = new Size(1118, 40);
            PanelBottomBar.TabIndex = 5;
            // 
            // LblStatus
            // 
            LblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LblStatus.AutoSize = true;
            LblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblStatus.ForeColor = Color.MediumSeaGreen;
            LblStatus.Location = new Point(72, 10);
            LblStatus.Name = "LblStatus";
            LblStatus.Size = new Size(70, 21);
            LblStatus.TabIndex = 9;
            LblStatus.Text = "ONLINE";
            // 
            // _LblStatus
            // 
            _LblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _LblStatus.AutoSize = true;
            _LblStatus.Font = new Font("Segoe UI", 12F);
            _LblStatus.Location = new Point(11, 10);
            _LblStatus.Name = "_LblStatus";
            _LblStatus.Size = new Size(55, 21);
            _LblStatus.TabIndex = 8;
            _LblStatus.Text = "Status:";
            // 
            // LblProgress
            // 
            LblProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            LblProgress.Font = new Font("Segoe UI", 12F);
            LblProgress.Location = new Point(304, 10);
            LblProgress.Name = "LblProgress";
            LblProgress.Size = new Size(616, 30);
            LblProgress.TabIndex = 7;
            LblProgress.TextAlign = ContentAlignment.TopRight;
            // 
            // ProgressBar
            // 
            ProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ProgressBar.Location = new Point(928, 10);
            ProgressBar.Margin = new Padding(5, 5, 5, 3);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(177, 20);
            ProgressBar.TabIndex = 6;
            // 
            // PanelBottombarSeparator
            // 
            PanelBottombarSeparator.BackColor = Color.Black;
            PanelBottombarSeparator.Dock = DockStyle.Top;
            PanelBottombarSeparator.Location = new Point(0, 0);
            PanelBottombarSeparator.Name = "PanelBottombarSeparator";
            PanelBottombarSeparator.Size = new Size(1118, 2);
            PanelBottombarSeparator.TabIndex = 5;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 630);
            Controls.Add(PanelMain);
            MinimumSize = new Size(1136, 669);
            Name = "FormMain";
            Text = "Countries";
            FormClosing += FormMain_FormClosing;
            PanelMain.ResumeLayout(false);
            PanelContent.ResumeLayout(false);
            PanelCountries.ResumeLayout(false);
            PanelTopbar.ResumeLayout(false);
            PanelFilter.ResumeLayout(false);
            PanelFilter.PerformLayout();
            PanelBottomBar.ResumeLayout(false);
            PanelBottomBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel PanelTopbar;
        private Panel PanelFilter;
        private FlowLayoutPanel ScrollPanelCountries;
        private Panel PanelCountries;
        private TextBox TxtFilter;
        private Panel PanelContentSeparator;
        private Panel PanelTopbarSeparator;
        private Panel PanelBottomBar;
        private Panel PanelBottombarSeparator;
        private Label LblSelectedCountry;
        private ControlCountry PanelCountry;
        private Panel PanelTopbarNameSeparator;
        private Panel PanelFilterSeparator;
        private ProgressBar ProgressBar;
        private Label LblStatus;
        private Label LblProgress;
    }
}
