using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MasterDataGenerator
{
	/// <summary>
	/// ソース生成
	/// </summary>
	public class SourceGenerator
	{
		/// <summary>
		/// 生成するディレクトリ
		/// </summary>
		private string outputDir = "";

		/// <summary>
		/// namespace
		/// </summary>
		private string nameSpace = "";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="outputDir">生成するディレクトリ</param>
		/// <param name="nameSpace">namespace</param>
		public SourceGenerator(string outputDir, string nameSpace)
		{
			this.outputDir = outputDir;
			this.nameSpace = nameSpace;
		}

		/// <summary>
		/// 生成
		/// </summary>
		/// <param name="masterName">マスタ名</param>
		/// <param name="properties">プロパティリスト</param>
		public void Generate(string masterName, List<PropertyData> properties)
		{
			string text = string.Format("namespace {0}\n", nameSpace);
			text += "{\n";
			text += string.Format("\tpublic class {0}Data\n", masterName);
			text += "\t{\n";
			foreach (var prop in properties)
			{
				text += string.Format("\t\tpublic {0} {1} ", prop.TypeName, prop.Name) + "{ get; private set; }\n";
			}
			text += "\n";
			text += string.Format("\t\tpublic {0}Data(", masterName);
			for (int i = 0; i < properties.Count; i++)
			{
				var prop = properties[i];
				text += string.Format("{0} {1}", prop.TypeName, prop.Name);
				if (i + 1 < properties.Count)
				{
					text += ", ";
				}
			}
			text += ")\n\t\t{\n";
			foreach (var prop in properties)
			{
				text += "\t\t\tthis." + prop.Name + " = " + prop.Name + ";\n";
			}
			text += "\t\t}\n\t}\n}\n";

			if (!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}

			var filePath = Path.Combine(outputDir, masterName + "Data.cs");
			File.WriteAllText(filePath, text);
		}
	}
}
