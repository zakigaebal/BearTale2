using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BearTale
{
	public partial class Form2 : Form
	{
		DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
		public Form2()
		{
			InitializeComponent();
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			강조클래스 강조 = new 강조클래스();
			강조.칼라 = textBoxString.Text;
			강조.내용 = textBoxString.Text;
			Color cd = Color.FromArgb(Color.FromName(colorComboBox1.Text).A, Color.FromName(colorComboBox1.Text).R, Color.FromName(colorComboBox1.Text).G, Color.FromName(colorComboBox1.Text).B);
			Color cb = Color.FromArgb(Color.FromName(colorComboBox2.Text).A, Color.FromName(colorComboBox2.Text).R, Color.FromName(colorComboBox2.Text).G, Color.FromName(colorComboBox2.Text).B);
			강조.글자색 = cd.Name.ToString();
			강조.배경색 = cb.Name.ToString();
			//강조.글자색 = colorComboBox1.SelectedColor.Name.ToString();
			//강조.배경색 = colorComboBox2.SelectedColor.Name.ToString();
			강조.무시 = checkBoxIgnore.Checked;
			강조.전환 = checkBoxInvert.Checked;
			강조.진하게 = checkBoxBold.Checked;
			강조.기울이게 = checkBoxItalic.Checked;

			//custom 칼라이면
			if (colorComboBox1.SelectedItem.ToString() == "Custom")
			{
				강조.글자색 = customColor.Name.ToString();
				강조.배경색 = customColor2.Name.ToString();
			}


			bindingSource.Add(강조);
			jsonSave();
		}


		private void buttonDelete_Click(object sender, EventArgs e)
		{
			if (강조내용.Count > 0)
			{
				bindingSource.RemoveAt(dataGridView1.CurrentRow.Index);
			}
		}



		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			DataGridView dgv = dataGridView1;
			try
			{
				int totalRows = dgv.Rows.Count;
				// get index of the row for the selected cell
				int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
				if (rowIndex == 0)
					return;
				// get index of the column for the selected cell
				int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
				DataGridViewRow selectedRow = dgv.Rows[rowIndex];
				dgv.Rows.Remove(selectedRow);
				dgv.Rows.Insert(rowIndex - 1, selectedRow);
				dgv.ClearSelection();
				dgv.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
			}
			catch (Exception ex) { Console.WriteLine(ex); }
		}

		private int GetSelectedRowIndex(DataGridView dgv)
		{
			if (dgv.Rows.Count == 0)
			{
				return 0;
			}
			foreach (DataGridViewRow row in dgv.Rows)
			{
				if (row.Selected)
				{
					return row.Index;
				}
			}
			return 0;
		}

		private void TaskViewEditHelper_OnUpStep(object sender, EventArgs e)
		{

			if (this.dataGridView1.SelectedRows == null || this.dataGridView1.SelectedRows.Count == 0)
			{
			}
			else
			{
				if (this.dataGridView1.SelectedRows[0].Index <= 0)
				{

				}
				else
				{
					//Note: This is the move up line for unbound data
					//Selected line number  
					int selectedRowIndex = GetSelectedRowIndex(this.dataGridView1);
					if (selectedRowIndex >= 1)
					{
						//Copy selected lines  
						DataGridViewRow newRow = dataGridView1.Rows[selectedRowIndex];
						//Delete selected rows  
						dataGridView1.Rows.Remove(dataGridView1.Rows[selectedRowIndex]);
						//Insert the copied row into the previous row selected  
						dataGridView1.Rows.Insert(selectedRowIndex - 1, newRow);
						dataGridView1.ClearSelection();
						//Select the initially selected row 
						dataGridView1.Rows[selectedRowIndex - 1].Selected = true;
					}
				}
			}
		}


		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			DataGridView dgv = dataGridView1;
			try
			{
				int totalRows = dgv.Rows.Count;
				// get index of the row for the selected cell
				int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
				if (rowIndex == totalRows - 1)
					return;
				// get index of the column for the selected cell
				int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
				DataGridViewRow selectedRow = dgv.Rows[rowIndex];
				dgv.Rows.Remove(selectedRow);
				dgv.Rows.Insert(rowIndex + 1, selectedRow);
				dgv.ClearSelection();
				dgv.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
			}
			catch { }
		}

		public 강조클래스 강조들 { get; set; }

		private void buttonSave_Click(object sender, EventArgs e)
		{
			//강조들.칼라 = ((강조클래스))
		}

		private void buttonLoad_Click(object sender, EventArgs e)
		{
			Color cd = Color.FromArgb(Color.FromName(colorComboBox1.Text).A, Color.FromName(colorComboBox1.Text).R, Color.FromName(colorComboBox1.Text).G, Color.FromName(colorComboBox1.Text).B);
			Color cb = Color.FromArgb(Color.FromName(colorComboBox2.Text).A, Color.FromName(colorComboBox2.Text).R, Color.FromName(colorComboBox2.Text).G, Color.FromName(colorComboBox2.Text).B);

			System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			//강조클래스 data = new 강조클래스
			//{
			//	칼라 = textBoxString.Text,
			//	내용 = textBoxString.Text,
			//	글자색 = cd.Name.ToString(),
			//	배경색 = cb.Name.ToString(),
			//	무시 = checkBoxIgnore.Checked,
			//	전환 = checkBoxInvert.Checked,
			//	진하게 = checkBoxBold.Checked,
			//	기울이게 = checkBoxItalic.Checked
			//};
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string jsonName = @"" + currentPath + "highlightJson" + ".json";
			var serializedStr = File.ReadAllText(jsonName);
			강조들 = JsonConvert.DeserializeObject<강조클래스>(serializedStr, new JsonSerializerSettings());
			if (강조들 == null)
			{
				강조들 = new 강조클래스()
				{
					칼라 = textBoxString.Text,
					내용 = textBoxString.Text,
					글자색 = cd.Name.ToString(),
					배경색 = cb.Name.ToString(),
					무시 = checkBoxIgnore.Checked,
					전환 = checkBoxInvert.Checked,
					진하게 = checkBoxBold.Checked,
					기울이게 = checkBoxItalic.Checked
				};
			}



			//string jsonText = File.ReadAllText(jsonName);

			//var data = JsonConvert.DeserializeObject<강조클래스>(jsonText);
			//강조내용.Add(data);
			bindingSource.DataSource = 강조들;
			dataGridView1.DataSource = bindingSource;
		}



		private void buttonPageUp_Click(object sender, EventArgs e)
		{
			DataGridView dgv = dataGridView1;
			try
			{
				int totalRows = dgv.Rows.Count;
				// get index of the row for the selected cell
				int rowNumber = 0;
				int rowIndex = dgv.SelectedCells[rowNumber].OwningRow.Index;
				if (rowIndex == 0)
					return;
				// get index of the column for the selected cell
				int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
				DataGridViewRow selectedRow = dgv.Rows[rowIndex];
				dgv.ClearSelection();
				if (rowIndex - 10 < 0)
				{
					dgv.Rows[0].Cells[colIndex].Selected = true;
				}
				else dgv.Rows[rowIndex - 10].Cells[colIndex].Selected = true;
			}
			catch { }

		}

		private void buttonPageDown_Click(object sender, EventArgs e)
		{
			DataGridView dgv = dataGridView1;
			try
			{
				int totalRows = dgv.Rows.Count;
				// get index of the row for the selected cell
				int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
				if (rowIndex == totalRows - 1)
					return;
				// get index of the column for the selected cell
				int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
				DataGridViewRow selectedRow = dgv.Rows[rowIndex];
				dgv.Rows.Remove(selectedRow);
				dgv.Rows.Insert(rowIndex + 1, selectedRow);
				dgv.ClearSelection();
				if (dgv.Rows[dataGridView1.Rows.Count - 1].Cells[colIndex].Value == null)
				{
					dgv.Rows[dataGridView1.Rows.Count - 1].Cells[colIndex].Selected = true;
				}
				else dgv.Rows[rowIndex + 10].Cells[colIndex].Selected = true;
			}
			catch { }
		}


		public class 강조클래스
		{
			public string 칼라 { get; set; }  //get , set을 써주어야만 리스트 상태로 DataGridView에 바인딩 할 수가 있다.
			public string 내용 { get; set; }
			public string 글자색 { get; set; }
			public string 배경색 { get; set; }
			public bool 무시 { get; set; }
			public bool 전환 { get; set; }
			public bool 진하게 { get; set; }
			public bool 기울이게 { get; set; }
		}
		BindingSource bindingSource = new BindingSource();
		List<강조클래스> 강조내용 = new List<강조클래스>();


		private void Form2_Load(object sender, EventArgs e)
		{
			bindingSource.DataSource = 강조내용;
			dataGridView1.DataSource = bindingSource;

			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.Columns[0].Width = 100;
			dataGridView1.Columns[1].Width = 100;
			dataGridView1.GridColor = Color.White;
			dataGridView1.Columns[0].Resizable = DataGridViewTriState.False;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridView1.ReadOnly = true;
			dataGridView1.ColumnHeadersVisible = true;

			colorComboBox1.Items.Add("Custom");
			colorComboBox1.Items.Add("White");
			colorComboBox1.Items.Add("Red");
			colorComboBox1.Items.Add("Green");
			colorComboBox1.Items.Add("Blue");
			colorComboBox1.Items.Add("Yellow");
			colorComboBox1.Items.Add("Cyan");
			colorComboBox1.Items.Add("Magenta");
			colorComboBox1.Items.Add("DarkRed");
			colorComboBox1.Items.Add("DarkGreen");
			colorComboBox1.Items.Add("DarkBlue");
			colorComboBox1.Items.Add("DarkCyan");
			colorComboBox1.Items.Add("DarkMagenta");

			colorComboBox2.Items.Add("Custom");
			colorComboBox2.Items.Add("White");
			colorComboBox2.Items.Add("Red");
			colorComboBox2.Items.Add("Green");
			colorComboBox2.Items.Add("Blue");
			colorComboBox2.Items.Add("Yellow");
			colorComboBox2.Items.Add("Cyan");
			colorComboBox2.Items.Add("Magenta");
			colorComboBox2.Items.Add("DarkRed");
			colorComboBox2.Items.Add("DarkGreen");
			colorComboBox2.Items.Add("DarkBlue");
			colorComboBox2.Items.Add("DarkCyan");
			colorComboBox2.Items.Add("DarkMagenta");

			colorComboBox1.SelectedIndex = 3;
			colorComboBox2.SelectedIndex = 5;

			//데이터그리드뷰 RowHeader 숨기기
			dataGridView1.RowHeadersVisible = false;
		}

		private void jsonSave()
		{
			try
			{
				Color cd = Color.FromArgb(Color.FromName(colorComboBox1.Text).A, Color.FromName(colorComboBox1.Text).R, Color.FromName(colorComboBox1.Text).G, Color.FromName(colorComboBox1.Text).B);
				Color cb = Color.FromArgb(Color.FromName(colorComboBox2.Text).A, Color.FromName(colorComboBox2.Text).R, Color.FromName(colorComboBox2.Text).G, Color.FromName(colorComboBox2.Text).B);

				System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string currentPath = System.IO.Directory.GetCurrentDirectory();


				//직렬화

				var data = new 강조클래스
				{
					칼라 = textBoxString.Text,
					내용 = textBoxString.Text,
					글자색 = cd.Name.ToString(),
					배경색 = cb.Name.ToString(),
					무시 = checkBoxIgnore.Checked,
					전환 = checkBoxInvert.Checked,
					진하게 = checkBoxBold.Checked,
					기울이게 = checkBoxItalic.Checked
				};

				var jsonText = JsonConvert.SerializeObject(data);
				Console.WriteLine(jsonText);



				string json1 = JsonConvert.SerializeObject(data, Formatting.Indented);
				string jsonName = @"" + currentPath + "highlightJson" + ".json";

				using (FileStream fs = new FileStream(jsonName, FileMode.Append, FileAccess.Write))
				using (StreamWriter Write = new StreamWriter(fs))
				{
					Write.WriteLine(json1);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}


		private void colorComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowIndex = dataGridView1.CurrentCell.RowIndex;
			dataGridView1.Rows[rowIndex].Cells[2].Value = colorComboBox1.Name.ToString();
			//			dataGridView1.Rows[rowIndex].Cells[0].Style.ForeColor = Color.FromName(colorComboBox1.Text);
			//textboxColumn.DefaultCellStyle.ForeColor = Color.FromName(colorComboBox1.Text);
		}

		private void colorComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowIndex = dataGridView1.CurrentCell.RowIndex;
			dataGridView1.Rows[rowIndex].Cells[3].Value = colorComboBox2.Name.ToString();

			//		dataGridView1.Rows[rowIndex].Cells[0].Style.BackColor = Color.FromName(colorComboBox2.Text);
			//textboxColumn.DefaultCellStyle.BackColor = Color.FromName(colorComboBox2.Text);
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			//string dgv1 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			//string dgv2 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
			//Color cd = Color.FromArgb(colorComboBox1.SelectedColor.A, colorComboBox1.SelectedColor.R, colorComboBox1.SelectedColor.G, colorComboBox1.SelectedColor.B);
			//Color cb = Color.FromArgb(colorComboBox2.SelectedColor.A, colorComboBox2.SelectedColor.R, colorComboBox2.SelectedColor.G, colorComboBox2.SelectedColor.B);
			textBoxString.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

			string colorcode = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			string colorcode2 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
			int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
			int argb2 = Int32.Parse(colorcode2.Replace("#", ""), NumberStyles.HexNumber);

			Color clr1 = Color.FromArgb(argb);
			Color clr2 = Color.FromArgb(argb2);

			colorComboBox1.Items.Add(clr1.Name.ToString());
			Console.WriteLine(clr2);

			//dataGridView1.Rows[e.RowIndex].Cells[0].Style.ForeColor = clr1;
			//dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = clr2;
			//	ColorTranslator.ToHtml(Color.FromArgb(Color.Tomato.ToArgb()))

			//colorComboBox1.SelectedColor.Name.ToString() = Color.FromName(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
			//colorComboBox1.SelectedColor = Color.FromName(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
			//colorComboBox2.SelectedColor = Color.FromName(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());

		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			Form1 Mainform = (Form1)Owner;
			//데이터그리드뷰내용저장하기
			buttonSave_Click(sender, e);
			//int rowindex = dataGridView1.CurrentRow.Index;
			//string rowValue = dataGridView1.Rows[rowindex].Cells[1].Value.ToString().Trim();
			//if (rowValue.Contains("1"))
			//{
			//	dataGridView1.Rows[rowindex].Cells[0].Style.ForeColor = Color.FromName(colorComboBox1.Text);
			//	dataGridView1.Rows[rowindex].Cells[0].Style.BackColor = Color.FromName(colorComboBox2.Text);
			//}
			this.Close();
		}

		private void textBoxString_TextChanged(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowIndex = dataGridView1.CurrentCell.RowIndex;


			dataGridView1.Rows[rowIndex].Cells[0].Value = textBoxString.Text;
			dataGridView1.Rows[rowIndex].Cells[1].Value = textBoxString.Text;

		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		private void buttonCustom1_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				if (dataGridView1.CurrentCell == null)
				{
					return;
				}
				int rowIndex = dataGridView1.CurrentCell.RowIndex;
				dataGridView1.Rows[rowIndex].Cells[2].Value = colorDialog1.Color.Name.ToString();
			}
		}

		private void buttonCustom2_Click(object sender, EventArgs e)
		{
			if (colorDialog2.ShowDialog() == DialogResult.OK)
			{
				if (dataGridView1.CurrentCell == null)
				{
					return;
				}
				dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor = colorDialog1.Color;

			}
		}


		//컬러클래스
		public class MyListBoxItem
		{
			public MyListBoxItem(TextBox textcolor, Color c, string m)
			{
				textboxColor = textcolor;
				ItemColor = c;
				Message = m;
			}
			public TextBox textboxColor { get; set; }
			public Color ItemColor { get; set; }
			public string Message { get; set; }
		}

		private void checkBoxBold_CheckedChanged(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowindex = dataGridView1.CurrentCell.RowIndex;

			//체크박스볼드가 체크되었을때
			if (checkBoxBold.Checked == true && checkBoxItalic.Checked == false)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
			//체크박스이테릭 체크되었을때
			if (checkBoxBold.Checked == false && checkBoxItalic.Checked == true)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Italic);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
			//체크박스가 둘다 체크되었을때
			if (checkBoxBold.Checked == true && checkBoxItalic.Checked == true)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold | FontStyle.Italic);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
			//체크박스가 둘다 체크안되었을때
			if (checkBoxBold.Checked == false && checkBoxItalic.Checked == false)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
		}


		private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			//		 dataGridView1.Columns[0].DefaultHeaderCellType.BorderColor = System.Drawing.Color.Red;
		}


		private void checkBoxItalic_CheckedChanged(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowindex = dataGridView1.CurrentCell.RowIndex;

			//체크박스이테릭만 체크되었을때
			if (checkBoxBold.Checked == false && checkBoxItalic.Checked == true)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Italic);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
			//체크박스볼드만 체크일때
			if (checkBoxBold.Checked == true && checkBoxItalic.Checked == false)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
			//체크박스가 둘다 체크되었을때
			if (checkBoxBold.Checked == true && checkBoxItalic.Checked == true)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold | FontStyle.Italic);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();

			}
			//체크박스가 둘다 체크안되었을때
			if (checkBoxBold.Checked == false && checkBoxItalic.Checked == false)
			{
				dataGridView1.Rows[rowindex].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
				dataGridView1.Rows[rowindex].Cells[6].Value = checkBoxBold.Checked.ToString();
				dataGridView1.Rows[rowindex].Cells[7].Value = checkBoxItalic.Checked.ToString();
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
		}



		private void checkBoxInvert_CheckedChanged(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}

			int rowindex = dataGridView1.CurrentCell.RowIndex;

			dataGridView1.Rows[rowindex].Cells[5].Value = checkBoxInvert.Checked.ToString();

		}

		private void checkBoxIgnore_CheckedChanged(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowindex = dataGridView1.CurrentCell.RowIndex;
			dataGridView1.Rows[rowindex].Cells[4].Value = checkBoxIgnore.Checked.ToString();
		}

		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				//Color co = Color.FromName(dataGridView1.Rows[i].Cells[2].Value.ToString());
				//Color back = Color.FromName(dataGridView1.Rows[i].Cells[3].Value.ToString());

				string colorcode = dataGridView1.Rows[i].Cells[2].Value.ToString();
				string colorcode2 = dataGridView1.Rows[i].Cells[3].Value.ToString();
				int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
				int argb2 = Int32.Parse(colorcode2.Replace("#", ""), NumberStyles.HexNumber);

				Color clr = Color.FromArgb(argb);
				Color clr2 = Color.FromArgb(argb2);

				dataGridView1.Rows[i].Cells[0].Style.ForeColor = clr;
				dataGridView1.Rows[i].Cells[0].Style.BackColor = clr2;

				// 체크박스가 볼드가 True이고 체크박스 이태릭이 False일때
				if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "True" && dataGridView1.Rows[i].Cells[7].Value.ToString() == "False")
				{
					dataGridView1.Rows[i].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
				}
				//체크박스가 이태릭이 트루이고 볼드는 false일때
				else if (dataGridView1.Rows[i].Cells[7].Value.ToString() == "True" && dataGridView1.Rows[i].Cells[6].Value.ToString() == "False")
				{
					dataGridView1.Rows[i].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Italic);
				}
				//체크박스 둘다 트루
				else if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "True" && dataGridView1.Rows[i].Cells[7].Value.ToString() == "True")
				{
					dataGridView1.Rows[i].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold | FontStyle.Italic);
				}
				//체크박스가 둘다 아닐때
				else if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "False" && dataGridView1.Rows[i].Cells[7].Value.ToString() == "False")
				{
					dataGridView1.Rows[i].Cells[0].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
				}
			}
		}



		private void colorComboBox1_ColorChanged(object sender, ColorChangeArgs e)
		{
			Console.WriteLine(e.color.Name.ToString(), "Selected color", MessageBoxButtons.OK, MessageBoxIcon.Information);

			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowIndex = dataGridView1.CurrentCell.RowIndex;
			//			dataGridView1.Rows[rowIndex].Cells[2].Value = e.color.Name.ToString();

			Color cd = Color.FromArgb(Color.FromName(colorComboBox1.Text).A, Color.FromName(colorComboBox1.Text).R, Color.FromName(colorComboBox1.Text).G, Color.FromName(colorComboBox1.Text).B);
			Color cb = Color.FromArgb(Color.FromName(colorComboBox2.Text).A, Color.FromName(colorComboBox2.Text).R, Color.FromName(colorComboBox2.Text).G, Color.FromName(colorComboBox2.Text).B);

			dataGridView1.Rows[rowIndex].Cells[2].Value = cd.Name.ToString();
			dataGridView1.Rows[rowIndex].Cells[3].Value = cb.Name.ToString();
			//강조.글자색 = colorComboBox1.SelectedColor.Name.ToString();
			//강조.배경색 = colorComboBox2.SelectedColor.Name.ToString();


		}

		private void colorComboBox1_Load(object sender, EventArgs e)
		{
		}

		Color customColor = new Color();
		Color customColor2 = new Color();

		private void button1_Click(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				int argb = dialog.Color.ToArgb();
				customColor = Color.FromArgb(argb);
				//color name이 나옴
				//color 16진수로 변환해서 데이터그리드뷰에 넣어야됨.
			}
			if (!colorComboBox1.Items.Contains("Custom"))
			{
				colorComboBox1.Items.Add("Custom");
			}
			else
			{
				colorComboBox1.SelectedItem = "Custom";
				Console.WriteLine(customColor);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				int argb = dialog.Color.ToArgb();
				customColor2 = Color.FromArgb(argb);
				//color name이 나옴
				//color 16진수로 변환해서 데이터그리드뷰에 넣어야됨.
			}
			if (!colorComboBox2.Items.Contains("Custom"))
			{
				colorComboBox2.Items.Add("Custom");
			}
			else
			{
				colorComboBox2.SelectedItem = "Custom";
				Console.WriteLine(customColor2);
			}
		}
	}
}



