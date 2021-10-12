using System;
using System.IO;
using System.Text;
using System.Linq;

namespace MasterDataGenerator
{
	class Program
	{
		/// <summary>
		/// 設定ファイルパス
		/// </summary>
		private static readonly string SettingFilePath = "./Setting.json";
		
		static void Main(string[] args)
		{
			var settings = SettingFile.Load(SettingFilePath);
			if (settings == null)
			{
				Console.WriteLine("設定ファイルが無かったので新規作成します");
				settings = new SettingFile();
				settings.Save(SettingFilePath);
				return;
			}

			// Excelファイルの列挙
			var excels = Directory.GetFiles(settings.MasterExcelFileRoot, "*.xlsx", SearchOption.AllDirectories);
			SourceGenerator sourceGenerator = new SourceGenerator(settings.SourcePath, settings.NameSpace);
			BinaryGenerator binaryGenerator = new BinaryGenerator(settings.BinaryPath);
			string versionText = "";
			foreach (var excel in excels)
			{
				var parsedData = ExcelParser.Create(excel);
				sourceGenerator.Generate(Path.GetFileNameWithoutExtension(excel), parsedData.Properties);
				var hash = binaryGenerator.Generate(Path.GetFileNameWithoutExtension(excel), parsedData.Columns, parsedData.Properties);
				versionText += Path.GetFileNameWithoutExtension(excel) + "Master.byte" + "," + string.Join("", hash.Select(x => $"{x:x2}")) + "\n";
			}
			File.WriteAllText(Path.Combine(settings.BinaryPath, "Version.csv"), versionText);
		}
	}
}
