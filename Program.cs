using System;

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
			}
		}
	}
}
