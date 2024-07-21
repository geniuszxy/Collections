namespace EncodingConverter
{
	partial class MainForm
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
			cbEncodingList = new ComboBox();
			btnConvert = new Button();
			lbTip = new Label();
			tbFilter = new TextBox();
			cbEncodingFrom = new ComboBox();
			lbFiles = new ListBox();
			lb1 = new Label();
			SuspendLayout();
			// 
			// cbEncodingList
			// 
			cbEncodingList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			cbEncodingList.FormattingEnabled = true;
			cbEncodingList.Location = new Point(264, 379);
			cbEncodingList.Name = "cbEncodingList";
			cbEncodingList.Size = new Size(226, 25);
			cbEncodingList.TabIndex = 4;
			// 
			// btnConvert
			// 
			btnConvert.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			btnConvert.Location = new Point(496, 379);
			btnConvert.Name = "btnConvert";
			btnConvert.Size = new Size(84, 23);
			btnConvert.TabIndex = 1;
			btnConvert.Text = "Convert";
			btnConvert.UseVisualStyleBackColor = true;
			btnConvert.Click += OnConvertClick;
			// 
			// lbTip
			// 
			lbTip.AutoSize = true;
			lbTip.ForeColor = SystemColors.ControlDark;
			lbTip.Location = new Point(12, 38);
			lbTip.Name = "lbTip";
			lbTip.Size = new Size(249, 17);
			lbTip.TabIndex = 7;
			lbTip.Text = "Drop files or folders to the listview below";
			// 
			// tbFilter
			// 
			tbFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			tbFilter.Location = new Point(12, 12);
			tbFilter.Name = "tbFilter";
			tbFilter.PlaceholderText = "File Filter (e.g. *.html;*.txt)";
			tbFilter.Size = new Size(568, 23);
			tbFilter.TabIndex = 8;
			// 
			// cbEncodingFrom
			// 
			cbEncodingFrom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			cbEncodingFrom.FormattingEnabled = true;
			cbEncodingFrom.Location = new Point(12, 379);
			cbEncodingFrom.Name = "cbEncodingFrom";
			cbEncodingFrom.Size = new Size(226, 25);
			cbEncodingFrom.TabIndex = 4;
			// 
			// lbFiles
			// 
			lbFiles.AllowDrop = true;
			lbFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			lbFiles.IntegralHeight = false;
			lbFiles.ItemHeight = 17;
			lbFiles.Location = new Point(12, 58);
			lbFiles.Name = "lbFiles";
			lbFiles.Size = new Size(568, 315);
			lbFiles.TabIndex = 9;
			lbFiles.DragDrop += OnFilesDragDrop;
			lbFiles.DragEnter += OnFilesDragEnter;
			// 
			// lb1
			// 
			lb1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lb1.AutoSize = true;
			lb1.Location = new Point(242, 382);
			lb1.Name = "lb1";
			lb1.Size = new Size(18, 17);
			lb1.TabIndex = 10;
			lb1.Text = "▶";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(592, 414);
			Controls.Add(lb1);
			Controls.Add(lbFiles);
			Controls.Add(tbFilter);
			Controls.Add(lbTip);
			Controls.Add(btnConvert);
			Controls.Add(cbEncodingFrom);
			Controls.Add(cbEncodingList);
			Name = "MainForm";
			Text = "Encoding Converter";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private ComboBox cbEncodingList;
		private Button btnConvert;
		private Label lbTip;
		private TextBox tbFilter;
		private ComboBox cbEncodingFrom;
		private ListBox lbFiles;
		private Label lb1;
	}
}
