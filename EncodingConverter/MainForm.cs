using System.Diagnostics;
using System.Globalization;
using System.IO.Enumeration;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EncodingConverter
{
	public partial class MainForm : Form
	{
		private class Config
		{
			public string? Filter { get; set; }
			public int? FromEncoding { get; set; }
			public int? ToEncoding { get; set; }
		}

		private const string CONFIG_FILE = "config.json";

		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			SuspendLayout();

			var items = GetEncodingItems();
			cbEncodingFrom.Items.AddRange(items);
			cbEncodingList.Items.AddRange(items);

			try
			{
				Config c = JsonSerializer.Deserialize<Config>(File.ReadAllText(CONFIG_FILE))!;
				tbFilter.Text = c.Filter;
				cbEncodingFrom.SelectedItem = Encoding.GetEncoding(c.FromEncoding!.Value);
				cbEncodingList.SelectedItem = Encoding.GetEncoding(c.ToEncoding!.Value);
			}
			catch
			{
				cbEncodingFrom.SelectedItem = Encoding.UTF8;
				cbEncodingList.SelectedItem = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
			}

			ResumeLayout(true);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			//Save config
			var json = JsonSerializer.Serialize(new Config
			{
				Filter = tbFilter.Text,
				FromEncoding = (cbEncodingFrom.SelectedItem as EncodingWrapper)?.ei.CodePage,
				ToEncoding = (cbEncodingList.SelectedItem as EncodingWrapper)?.ei.CodePage,
			});
			File.WriteAllText(CONFIG_FILE, json);
		}

		private static object[] GetEncodingItems()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			return Encoding.GetEncodings()
				.OrderBy(ei => ei.DisplayName)
				.Select(ei => new EncodingWrapper(ei))
				.ToArray();
		}

		//用于在List中显示EncodingInfo
		private class EncodingWrapper(EncodingInfo ei)
		{
			public readonly EncodingInfo ei = ei;

			public override string ToString()
				=> ei.DisplayName;

			public override bool Equals(object? obj)
			{
				if (obj is EncodingInfo ei)
					return this.ei == ei;
				if (obj is Encoding en)
					return this.ei.CodePage == en.CodePage;
				return false;
			}

			public override int GetHashCode()
				=> ei.GetHashCode();
		}

		//开始转换
		private void OnConvertClick(object sender, EventArgs e)
		{
			if (MessageBox.Show("Convert?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;

			if (cbEncodingFrom.SelectedItem is EncodingWrapper ewFrom
				&& cbEncodingList.SelectedItem is EncodingWrapper ewTo)
			{
				var eFrom = ewFrom.ei.GetEncoding();
				var eTo = ewTo.ei.GetEncoding();
				foreach (string f in lbFiles.Items)
					Convert(f, eFrom, eTo);
			}

			MessageBox.Show("Convert Done");
		}

		private static void Convert(string f, Encoding eFrom, Encoding eTo)
		{
			var text = File.ReadAllText(f, eFrom);
			File.WriteAllText(f, text, eTo);
		}

		private void OnFilesDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data!.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;
		}

		private void OnFilesDragDrop(object sender, DragEventArgs e)
		{
			var d = e.Data!.GetData(DataFormats.FileDrop);
			if (d is string[] fs)
			{
				var filters = DecodeFilter();

				SuspendLayout();

				foreach (var f in fs)
				{
					if (Directory.Exists(f))
						AddDirectory(f, filters);
					else
						AddFile(f, filters);

					Trace.WriteLine(f);
				}

				ResumeLayout(true);
			}
		}

		private void AddFile(string f, IEnumerable<string>? filters)
		{
			var filename = Path.GetFileName(f);

			if (filters != null)
			{
				foreach (var filter in filters)
				{
					if (FileSystemName.MatchesSimpleExpression(filter, filename))
					{
						lbFiles.Items.Add(f);
						break;
					}
				}
			}
			else
				lbFiles.Items.Add(f);
		}

		private void AddDirectory(string d, IEnumerable<string>? filters)
		{
			var lvFileItems = lbFiles.Items;
			if (filters != null)
			{
				foreach (var filter in filters)
					foreach (var f in Directory.EnumerateFiles(d, filter))
						lvFileItems.Add(f);
			}
			else
				foreach (var f in Directory.EnumerateFiles(d))
					lvFileItems.Add(f);

			foreach (var subd in Directory.EnumerateDirectories(d))
				AddDirectory(subd, filters);
		}

		private IEnumerable<string>? DecodeFilter()
		{
			var text = tbFilter.Text;
			if (string.IsNullOrWhiteSpace(text))
				return null;

			var filters = tbFilter.Text
				.Split(';', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.Trim());
			if (!filters.Any())
				return null;

			return filters;
		}
	}
}
