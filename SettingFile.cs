using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MasterDataGenerator
{
	/// <summary>
	/// 設定ファイル
	/// </summary>
	[DataContract]
	public class SettingFile
	{
		/// <summary>
		/// ソースファイルの保存先
		/// </summary>
		[DataMember]
		public string SourcePath { get; set; }

		/// <summary>
		/// バイナリファイルの保存先
		/// </summary>
		[DataMember]
		public string BinaryPath { get; set; }

		/// <summary>
		/// Excelファイルの入ったルートディレクトリ
		/// </summary>
		[DataMember]
		public string MasterExcelFileRoot { get; set; }

		/// <summary>
		/// namespace
		/// </summary>
		[DataMember]
		public string NameSpace { get; private set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="sourcePath">ソースファイルの保存先</param>
		/// <param name="binaryPath">バイナリファイルの保存先</param>
		/// <param name="masterExcelFileRoot">Excelファイルの入ったルートディレクトリ</param>
		/// <param name="nameSpace">namespace</param>
		public SettingFile(string sourcePath = "out/sources", string binaryPath = "out/binaries", string masterExcelFileRoot = "master", string nameSpace = "Common.Master")
		{
			SourcePath = sourcePath;
			BinaryPath = binaryPath;
			MasterExcelFileRoot = masterExcelFileRoot;
			NameSpace = nameSpace;
		}

		/// <summary>
		/// 読み込み
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		public static SettingFile Load(string filePath)
		{
			if (!File.Exists(filePath)) { return null; }

			var jsonText = File.ReadAllText(filePath);
			var serializer = new DataContractJsonSerializer(typeof(SettingFile));
			SettingFile setting = null;
			using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
			{
				memoryStream.Seek(0, SeekOrigin.Begin);
				setting = serializer.ReadObject(memoryStream) as SettingFile;
			}
			return setting;
		}

		/// <summary>
		/// 保存
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		public void Save(string filePath)
		{
			var serializer = new DataContractJsonSerializer(typeof(SettingFile));
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				serializer.WriteObject(writer.BaseStream, this);
			}
		}
	}
}
