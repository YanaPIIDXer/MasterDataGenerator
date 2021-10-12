using System;
using System.IO;

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
			foreach (var excel in excels)
			{
				var parsedData = ExcelParser.Create(excel);
				sourceGenerator.Generate(Path.GetFileNameWithoutExtension(excel), parsedData.Properties);
				binaryGenerator.Generate(Path.GetFileNameWithoutExtension(excel), parsedData.Columns, parsedData.Properties);
			}
		}
	}
}
