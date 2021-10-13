using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Stream;
using System.Security.Cryptography;

namespace MasterDataGenerator
{
	/// <summary>
	/// バイナリ生成
	/// </summary>
	public class BinaryGenerator
	{
		/// <summary>
		/// 出力先ディレクトリ
		/// </summary>
		private string outputDir = "";
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="outputDir">出力先ディレクトリ</param>
		public BinaryGenerator(string outputDir)
		{
			this.outputDir = outputDir;
		}

		/// <summary>
		/// 生成
		/// </summary>
		/// <param name="filePrefix">ファイルのプレフィクス</param>
		/// <param name="columnList">カラムリスト</param>
		/// <param name="properties">プロパティリスト</param>
		public byte[] Generate(string filePrefix, List<object[]> columnList, List<PropertyData> properties)
		{
			if (!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}
			MemoryStreamWriter writer = new MemoryStreamWriter();
			int columnCount = columnList.Count;
			writer.Serialize(ref columnCount);

			foreach (var columnDatas in columnList)
			{
				for (int i = 0; i < properties.Count; i++)
				{
					switch(properties[i].TypeName)
					{
						case "int":

							{
								int data = (int)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						case "uint":

							{
								uint data = (uint)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						case "short":

							{
								short data = (short)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						case "ushort":

							{
								ushort data = (ushort)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						case "char":

							{
								char data = (char)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						case "byte":

							{
								byte data = (byte)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						case "string":

							{
								string data = (string)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;

						case "float":

							{
								float data = (float)columnDatas[i];
								writer.Serialize(ref data);
							}
							break;
						default:
							throw new Exception("型名" + properties[i].TypeName + "はサポートされていません");
					}
				}

				string filePath = Path.Combine(outputDir, filePrefix + "Master.bytes");
				File.WriteAllBytes(filePath, writer.Buffer.ToArray());
			}

			var hash = MD5.Create();
			byte[] hashBytes = hash.ComputeHash(writer.Buffer.ToArray());
			return hashBytes;
		}
	}
}
