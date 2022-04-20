using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BearTale
{
	public partial class Form2 : Form
	{
		DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
		public Form2()
		{
			InitializeComponent();
		}

		private BindingSource oBS = new BindingSource();
		public static int Compare(String strA, String strB, bool ignoreCase)
		{
			if (ignoreCase)
			{
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			}
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}
		//		bool output = Regex.IsMatch("foo", "FOO", RegexOptions.IgnoreCase);

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			강조클래스 강조 = new 강조클래스();
			강조.칼라 = textBoxString.Text;
			강조.내용 = textBoxString.Text;
			Color cd = Color.FromArgb(colorCombo1.SelectedColor.A, colorCombo1.SelectedColor.R, colorCombo1.SelectedColor.G, colorCombo1.SelectedColor.B);
			Color cb = Color.FromArgb(colorCombo2.SelectedColor.A, colorCombo2.SelectedColor.R, colorCombo2.SelectedColor.G, colorCombo2.SelectedColor.B);

			강조.글자색 = cd.Name.ToString();
			강조.배경색 = cb.Name.ToString();
			//강조.글자색 = colorCombo1.SelectedColor.Name.ToString();
			//강조.배경색 = colorCombo2.SelectedColor.Name.ToString();
			강조.무시 = checkBoxIgnore.Checked;
			강조.전환 = checkBoxInvert.Checked;
			강조.진하게 = checkBoxBold.Checked;
			강조.기울이게 = checkBoxItalic.Checked;
			bindingSource.Add(강조);
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
			catch { }

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

		private void buttonSave_Click(object sender, EventArgs e)
		{
			string filename = "high.xml";
			List<List<string>> data = new List<List<string>>();
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				List<string> rowData = new List<string>();
				foreach (DataGridViewCell cell in row.Cells)
					//		rowData.Add(cell)
					rowData.Add(cell.FormattedValue.ToString());
				data.Add(rowData);
			}
			XmlSerializer xs = new XmlSerializer(data.GetType());
			using (TextWriter tw = new StreamWriter(filename))
			{
				xs.Serialize(tw, data);
				tw.Close();
			}
		}

		private void buttonLoad_Click(object sender, EventArgs e)
		{
		

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
			public string 칼라 { get; set; }	//get , set을 써주어야만 리스트 상태로 DataGridView에 바인딩 할 수가 있다.
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
			dataGridView1.Rows.Clear();
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.Columns[0].Width = 50;
			dataGridView1.Columns[1].Width = 100;
			dataGridView1.GridColor = Color.White;
			dataGridView1.Columns[0].Resizable = DataGridViewTriState.False;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridView1.ReadOnly = true;
			dataGridView1.ColumnHeadersVisible = true;
			//buttonLoad_Click(sender, e);
		}
		private void 삭제_Click(object sender, EventArgs e)
		{
			if (강조내용.Count > 0)
			{
				bindingSource.RemoveAt(dataGridView1.CurrentRow.Index);
			}
		}

		private void colorComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowIndex = dataGridView1.CurrentCell.RowIndex;
			dataGridView1.Rows[rowIndex].Cells[2].Value = colorCombo1.Name.ToString();
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
			dataGridView1.Rows[rowIndex].Cells[3].Value = colorCombo2.Name.ToString();

	//		dataGridView1.Rows[rowIndex].Cells[0].Style.BackColor = Color.FromName(colorComboBox2.Text);
	//textboxColumn.DefaultCellStyle.BackColor = Color.FromName(colorComboBox2.Text);
		}


		private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{// row header 에 자동 일련번호 넣기
			StringFormat drawFormat = new StringFormat();
			//drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
			drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
			using (Brush brush = new SolidBrush(Color.Black))
			{
				e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, brush, e.RowBounds.Location.X + 30, e.RowBounds.Location.Y + 4, drawFormat);
			}
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			//string dgv1 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			//string dgv2 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
			//Color cd = Color.FromArgb(colorCombo1.SelectedColor.A, colorCombo1.SelectedColor.R, colorCombo1.SelectedColor.G, colorCombo1.SelectedColor.B);
			//Color cb = Color.FromArgb(colorCombo2.SelectedColor.A, colorCombo2.SelectedColor.R, colorCombo2.SelectedColor.G, colorCombo2.SelectedColor.B);

			string colorcode = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			string colorcode2 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
			int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
			int argb2 = Int32.Parse(colorcode2.Replace("#", ""), NumberStyles.HexNumber);

			Color clr1 = Color.FromArgb(argb);
			Color clr2 = Color.FromArgb(argb2);

			//dataGridView1.Rows[e.RowIndex].Cells[0].Style.ForeColor = clr;
			//dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = clr2;


			textBoxString.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
			//colorCombo1.SelectedColor.Name.ToString() = Color.FromName(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
			//colorCombo1.SelectedColor = Color.FromName(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
			//colorCombo2.SelectedColor = Color.FromName(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());

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



		private void colorCombo1_ColorChanged(object sender, ColorChangeArgs e)
		{
			//MessageBox.Show(this,e.color.Name.ToString(),"Selected color",MessageBoxButtons.OK,MessageBoxIcon.Information);

			if (dataGridView1.CurrentCell == null)
			{
				return;
			}
			int rowIndex = dataGridView1.CurrentCell.RowIndex;
			dataGridView1.Rows[rowIndex].Cells[2].Value = colorCombo1.Name.ToString();
		}

		private void colorCombo1_Load(object sender, EventArgs e)
		{
			
		}
	}
}



