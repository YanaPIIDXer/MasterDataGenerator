using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;

namespace MasterDataGenerator
{
	/// <summary>
	/// プロパティデータ
	/// </summary>
	public class PropertyData
	{
		/// <summary>
		/// プロパティ名
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 型名
		/// </summary>
		public string TypeName { get; private set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <param name="typeName">型名</param>
		public PropertyData(string name, string typeName)
		{
			Name = name;
			TypeName = typeName;
		}
	}

	/// <summary>
	/// パースされたデータ
	/// </summary>
	public interface IParsedData
	{
		/// <summary>
		/// プロパティリスト
		/// </summary>
		List<PropertyData> Properties { get; }

		/// <summary>
		/// カラムリスト
		/// </summary>
		List<object[]> Columns { get; }
	}

	/// <summary>
	/// Excelファイルのパーサ
	/// </summary>
	public class ExcelParser : IParsedData
	{
		/// <summary>
		/// 生成
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		/// <returns>パースしたデータ</returns>
		public static IParsedData Create(string filePath)
		{
			ExcelParser parser = new ExcelParser();
			parser.ParseFile(filePath);
			return parser;
		}

		/// <summary>
		/// プロパティリスト
		/// </summary>
		public List<PropertyData> Properties { get; private set; } = new List<PropertyData>();

		/// <summary>
		/// カラムリスト
		/// </summary>
		public List<object[]> Columns { get; private set; } = new List<object[]>();

		/// <summary>
		/// ファイルを読み込んでパース
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		private void ParseFile(string filePath)
		{
			var fileStream = File.OpenRead(filePath);
			using (var package = new ExcelPackage(fileStream))
			{
				var workSheet = package.Workbook.Worksheets[0];
				int itemBeginRow = -1;
				for (int row = 1; itemBeginRow == -1; row++)
				{
					switch (workSheet.Cells[row, 1].Text)
					{
						case "$ITEMS":

							// データ開始行
							itemBeginRow = row;
							break;

						case "$MEMBERS":

							// メンバ定義
							ParseMember(workSheet, row);
							break;
					}
				}

				ParseItemColumns(workSheet, itemBeginRow);
			}
		}

		/// <summary>
		/// メンバのパース
		/// </summary>
		/// <param name="workSheet">ワークシート</param>
		/// <param name="row">行</param>
		private void ParseMember(ExcelWorksheet workSheet, int row)
		{
			for(int col = 2; !string.IsNullOrEmpty(workSheet.Cells[row, col].Text); col++)
			{
				var name = workSheet.Cells[row, col].Text;				// プロパティ名
				var typeName = workSheet.Cells[row + 1, col].Text;      // 型名
				Properties.Add(new PropertyData(name, typeName));
			}
		}
		
		/// <summary>
		/// アイテムカラムのパース
		/// </summary>
		/// <param name="workSheet">ワークシート</param>
		/// <param name="beginRow">開始行</param>
		private void ParseItemColumns(ExcelWorksheet workSheet, int beginRow)
		{
			int emptyCount = 0;
			int columnCount = Properties.Count;
			for (int row = beginRow; ; row++)
			{
				if (string.IsNullOrEmpty(workSheet.Cells[row, 2].Text))
				{
					emptyCount++;
					if (emptyCount >= 3) { break; }     // 一定行数左端のデータが空なら終了
					continue;
				}
				else
				{
					emptyCount = 0;
				}

				object[] columnDatas = new object[columnCount];
				for (int column = 2, i = 0; i < columnCount; column++, i++)
				{
					columnDatas[i] = workSheet.Cells[row, column].Value;
				}
				Columns.Add(columnDatas);
			}
		}

		/// <summary>
		/// コンストラクタ
		/// ※Createメソッド経由でインスタンス化するのでprivate
		/// </summary>
		private ExcelParser() { }
	}
}
