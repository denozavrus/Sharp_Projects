using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GemBox.Document;

namespace Certificates_App
{
	public partial class Form1 : Form
	{

		string path_to_certificates = System.IO.Directory.GetCurrentDirectory();

		public Form1 ()
		{
			InitializeComponent();
			listBox1.SelectedIndex = 0;
		}

		private void Select_Excel_Click (object sender, EventArgs e)
		{
			OpenFileDialog OPF = new OpenFileDialog();
			OPF.Filter = "Файлы с расширением xls|*.xls|Файлы с расширением xlsx|*.xlsx|Файлы с макросом xlsm|*.xlsm";

			if (OPF.ShowDialog() == DialogResult.OK)
			{
				Excel_file excel = new Excel_file(OPF.FileName);

				List<Person> people = excel.Load_sheet();

				Grid_View grid_View = new Grid_View(dataGridView1);
				grid_View.Import_data(people); 

			}
		}

		private void Add_Click (object sender, EventArgs e)
		{
			string name = textBox1.Text.ToString();
			string cvalif = null;
			try { cvalif = listBox1.SelectedItem.ToString(); }
			catch
			{

			}
			string date = dateTimePicker1.Text.ToString();
			dataGridView1.Rows.Add(name, cvalif, date); 
		}

		private void textBox1_TextChanged (object sender, EventArgs e)
		{

		}

		private void button1_Click (object sender, EventArgs e)
		{
			FolderBrowserDialog FBD = new FolderBrowserDialog();
			if (FBD.ShowDialog() == DialogResult.OK)
			{
				path_to_certificates = FBD.SelectedPath;
			}
		}

		private void button2_Click (object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear(); 
		}

		private void button3_Click (object sender, EventArgs e)
		{
			int ind = dataGridView1.SelectedCells[0].RowIndex;
			try
			{
				dataGridView1.Rows.RemoveAt(ind);
			}
			catch
			{
				MessageBox.Show("Выберите элемент для удаления");
			}
		}

		private void button4_Click (object sender, EventArgs e)
		{
			Grid_View grid_View = new Grid_View(dataGridView1);
			var people = grid_View.Export_data(); 
			for (int i = 1; i < people.Count + 1; i++)
			{
				string path = path_to_certificates + String.Format(@"\Certificate{0}.docx", i);
				Word_file.Create_certificate(path, people[i - 1]); 
			}
		}
	}

	public class Person
	{
		public Person(string name, string cvalif, string date)
		{
			this.name = name;
			this.Cvalif = cvalif;
			this.date = date;
		}

		public Person(List<string> strings)
		{
			try
			{
				this.name = strings[0];
				this.Cvalif = strings[1];
				this.date = strings[2];
			}
			catch
			{
				throw new ArgumentException("Not enough parameters to create Person");
			}
		}

		private List<string> cvalifications = new List<string>() { "Парикмахер III класса", 
			"Парикмахер II класса", 
			"Парикмахер I класса", 
			"Парикмахер-модельер" };

		public string name;
		private string cvalif;
		public string date; 
		public string Cvalif
		{
			get
			{
				return cvalif; 
			}
			set
			{
				if (cvalifications.Contains(value))
				{
					cvalif = value; 
				}
				else
				{
					cvalif = null; 
				}
			}
		}
		public List<string> Prepare_for_load ()
		{
			if (this.cvalif == null)
			{
				Cvalif = "Парикмахер III класса";
			}
			if (this.date == null)
			{
				this.date = DateTime.Today.ToString("d.MM.yyyy");
			}
			if (this.name == null)
			{
				throw new Exception("Error getting name of person"); 
			}
			List<string> result = new List<string>();
			result.Add(this.name);
			result.Add(this.cvalif);
			result.Add(this.date);
			return result; 
		}

	}

	public class Excel_file
	{
		public string file_path;

		public Excel_file (string path)
		{
			this.file_path = path;
		}
		public static SharedStringItem GetSharedStringItemById (WorkbookPart workbookPart, int id)
		{
			return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
		}

		public List<Person> Load_sheet ()
		{
			using (SpreadsheetDocument doc = SpreadsheetDocument.Open(file_path, false))
			{
				WorkbookPart workbookPart = doc.WorkbookPart;

				List<Person> people = new List<Person>();

				List<string> collected_strings = new List<string>();

				Sheet theSheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();

				if (theSheet == null)
				{
					throw new ArgumentException("sheetName");
				}

				WorksheetPart wsPart =
				(WorksheetPart)( workbookPart.GetPartById(theSheet.Id) );

				var rows = wsPart.Worksheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();
				foreach (var row in rows)
				{
					collected_strings.Clear();

					foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
					{

						string cellValue = string.Empty;

						if (c.DataType != null)
						{
							if (c.DataType == CellValues.SharedString)
							{
								int id = -1;

								if (Int32.TryParse(c.InnerText, out id))
								{
									SharedStringItem item = GetSharedStringItemById(workbookPart, id);

									if (item.Text != null)
									{
										cellValue = item.Text.Text;
									}
									else if (item.InnerText != null)
									{
										cellValue = item.InnerText;
									}
									else if (item.InnerXml != null)
									{
										cellValue = item.InnerXml;
									}
								}
							}
						}
						else
						{
							DateTime start_date = new DateTime(1900, 1, 1);
							try
							{
								int days = Convert.ToInt32(c.CellValue.Text);
								start_date = start_date.AddDays(days - 2);
								string str_date = start_date.ToString("d.MM.yyyy");
								cellValue = str_date;
							}
							catch
							{
								cellValue = null;
							}
						}

						collected_strings.Add(cellValue);

					}
					people.Add(new Person(collected_strings));
				}
				people.RemoveAt(0);
				return people;
			}
		}
	}

	public class Grid_View
	{
		public DataGridView gridView; 

		public Grid_View(DataGridView gridView)
		{
			this.gridView = gridView; 
		}

		public void Import_data(List<Person> people)
		{
			foreach (Person person in people)
			{
				var loaded_person = person.Prepare_for_load();
				string name = loaded_person[0];
				string cvalif = loaded_person[1];
				string date = loaded_person[2];
				gridView.Rows.Add(name, cvalif, date);
			}
		}
		public List<Person> Export_data()
		{
			List<Person> people = new List<Person>();
			string name;
			string cvalif;
			string date; 
			for (int i = 0; i< gridView.Rows.Count-1; i++)
			{
				var row = gridView.Rows[i]; 
				try
				{
					name = row.Cells[0].FormattedValue.ToString();
					cvalif = row.Cells[1].FormattedValue.ToString();
					date = row.Cells[2].FormattedValue.ToString();
				}
				catch
				{
					throw new Exception("Error reading table"); 
				}
				people.Add(new Person(name, cvalif, date));
			}
			return people; 
		}
	}

	public class Word_file
	{
		public static void Create_certificate (string path, Person person)
		{
			string startupPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + @"\Samples\shablon-sertificata-03.docx";
			foreach (var process in Process.GetProcessesByName("winword"))
			{
				process.Kill();
			}

			DocumentModel doc = null;
			ComponentInfo.SetLicense("FREE-LIMITED-KEY");
			var loaded_person = person.Prepare_for_load();
			try
			{
				File.Copy(startupPath, path, true);
					if (File.Exists(path))
					{
						doc = DocumentModel.Load(path);
						BookmarkCollection wBookmarks = doc.Bookmarks;
						ContentRange wRange;
						int i = 0;
						foreach (var mark in wBookmarks)
						{
							wRange = mark.GetContent(false);
							wRange.LoadText(loaded_person[i].ToString());
							i++;
						}
					doc.Save(path);
					doc = null;
					}
			}
			catch 
			{
				MessageBox.Show("Во время выполнения произошла ошибка!");
			}
		}
	}

}
