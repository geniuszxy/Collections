using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChmHelper
{
	internal class HhpHelper(string inputFolder)
	{
		private StreamWriter swhhp;
		private StreamWriter swhhc;
		private DirectoryInfo rootDir;

		public void Process()
		{
			var fn = Path.GetFileName(inputFolder);
			var enc = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);

            swhhp = new StreamWriter(Path.Combine(inputFolder, $"{fn}.hhp"), false, enc);
			rootDir = new DirectoryInfo(inputFolder);

			WriteOptions(fn);
			WriteFiles();
			FinishWriteHHP();

			swhhc = new StreamWriter(Path.Combine(inputFolder, $"{fn}.hhc"), false, enc);
			WriteContentHeader();
			WriteContentItems();
			FinishWriteHHC();
		}

		string FindDefaultTopic()
		{
			var index = Path.Combine(inputFolder, "index.html");
			if (File.Exists(index))
				return "index.html";

			index = Path.Combine(inputFolder, "index.htm");
			if (File.Exists(index))
				return "index.htm";

			var first = rootDir.EnumerateFiles("*.html").FirstOrDefault();
			if (first != null)
				return first.Name;

			first = rootDir.EnumerateFiles("*.htm").FirstOrDefault();
			if (first != null)
				return first.Name;

			return null;
		}

		void WriteOptions(string fn)
		{
			swhhp.WriteLine($"""
				[OPTIONS]
				Compatibility=1.1 or later
				Compiled file={fn}.chm
				Contents file={fn}.hhc
				Display compile progress=Yes
				Full-text search=Yes
				Language=0x804 中文(简体，中国)
				""");

			var defaultTopic = FindDefaultTopic();
			if (defaultTopic != null)
				swhhp.WriteLine($"Default topic={defaultTopic}");

			swhhp.WriteLine();
		}

		void WriteFiles()
		{
			swhhp.WriteLine("[FILES]");

			foreach (var fi in rootDir.EnumerateFiles("*.htm?", SearchOption.AllDirectories))
			{
				switch(fi.Extension.ToLowerInvariant())
				{
					case ".html":
					case ".htm":
						var relpath = Path.GetRelativePath(rootDir.FullName, fi.FullName);
						Console.WriteLine(relpath);
						swhhp.WriteLine(relpath);
						break;
				}
			}

			swhhp.WriteLine();
		}

		void FinishWriteHHP()
		{
			swhhp.WriteLine("[INFOTYPES]");
			swhhp.WriteLine();
			swhhp.Close();
		}

		void WriteContentHeader()
		{
			swhhc.WriteLine($"""
				<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML//EN">
				<HTML>
				<HEAD>
				<meta name="GENERATOR" content="Microsoft&reg; HTML Help Workshop 4.1">
				<!-- Sitemap 1.0 -->
				</HEAD><BODY>
				<UL>
				""");
		}

		static bool GetDirsAndFiles(string dir, out string[] dirs, out string[] files)
		{
			dirs = Directory.GetDirectories(dir);
			files = Directory.GetFiles(dir, "*.htm").Concat(Directory.GetFiles(dir, "*.html")).ToArray();
			return files.Length != 0 || dirs.Length != 0;
		}

		void WriteContentItems()
		{
			if (GetDirsAndFiles(rootDir.FullName, out var dirs, out var files))
				WriteHHCContent(dirs, files);
		}

		void WriteHHCContent(string[] directories, string[] files)
		{
			foreach (var dir in directories)
			{
				if (!GetDirsAndFiles(dir, out var dirs2, out var files2))
					continue;

				swhhc.WriteLine($"<LI> <OBJECT type=\"text/sitemap\">");
				swhhc.WriteLine($"<param name=\"Name\" value=\"{Path.GetFileName(dir)}\">");
				swhhc.WriteLine("</OBJECT>");
				swhhc.WriteLine("<UL>");
				WriteHHCContent(dirs2, files2);
				swhhc.WriteLine("</UL>");
			}

			foreach (var file in files)
			{
				var relativePath = Path.GetRelativePath(rootDir.FullName, file);
				var title = ExtractTitleFromHtml(file);
				swhhc.WriteLine($"<LI> <OBJECT type=\"text/sitemap\">");
				swhhc.WriteLine($"<param name=\"Name\" value=\"{title}\">");
				swhhc.WriteLine($"<param name=\"Local\" value=\"{relativePath}\">");
				swhhc.WriteLine("</OBJECT>");
			}
		}

		void FinishWriteHHC()
		{
			swhhc.WriteLine("""
				</UL>
				</BODY></HTML>
				""");
			swhhc.Close();
		}

		static string ExtractTitleFromHtml(string filePath)
		{
			var htmlDoc = new HtmlDocument();
			htmlDoc.Load(filePath);
			var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
			return titleNode != null ? titleNode.InnerText.Trim() : Path.GetFileNameWithoutExtension(filePath);
		}
	}
}
