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
			string text = "using Stream;\n\n";
			text += string.Format("namespace {0}\n", nameSpace);
			text += "{\n";
			text += string.Format("\tpublic class {0}Data\n", masterName);
			text += "\t{\n";
			foreach (var prop in properties)
			{
				text += string.Format("\t\tpublic {0} {1} ", prop.TypeName, prop.Name) + "{ get; private set; }\n";
			}
			text += "\n";
			text += "\t\tpublic void Serialize(IMemoryStream stream)\n\t\t{\n";
			foreach (var prop in properties)
			{
				text += "\t\t\t" + prop.TypeName + " " + prop.Name + ";\n";
				text += "\t\t\tstream.Serialize(ref " + prop.Name + ");\n";
				text += "\t\t\tthis." + prop.Name + " = " + prop.Name + ";\n";
			}
			text += "\t\t}\n\n";
			text += "\t\tpublic static " + masterName + "Data[] SerializeAll(byte[] buffer)\n";
			text += "\t\t{\n";
			text += "\t\t\tMemoryStreamReader reader = new MemoryStreamReader(buffer);\n";
			text += "\t\t\tint size = 0;\n";
			text += "\t\t\treader.Serialize(ref size);\n";
			text += "\t\t\t" + masterName + "Data[] datas = new " + masterName + "Data[size];\n";
			text += "\t\t\tfor (int i = 0; i < size; i++)\n";
			text += "\t\t\t{\n";
			text += "\t\t\t\t" + masterName + "Data data = new " + masterName + "Data();\n";
			text += "\t\t\t\tdata.Serialize(reader);\n";
			text += "\t\t\t\tdatas[i] = data;\n";
			text += "\t\t\t}\n";
			text += "\t\t\treturn datas;\n";
			text += "\t\t}\n";
			text += "\t}\n}\n";

			if (!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}

			var filePath = Path.Combine(outputDir, masterName + "Data.cs");
			File.WriteAllText(filePath, text);
		}
	}
}
