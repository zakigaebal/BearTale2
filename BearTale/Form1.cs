using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BearTale
{
	public partial class Form1 : Form
	{
		//데이터그리드뷰 선언
		DataGridView dgv = new DataGridView();
		//탭페이지 선언
		TabPage page = new TabPage();
		//파일경로
		string filePath = string.Empty;
		//파일이름
		string fileName = string.Empty;


		public Form1()
		{
			InitializeComponent();
			this.FormClosed += Form_Closing;
		}

		//파일 이름 json 클래스
		public class Fileinfo
		{
			public string name { get; set; }
		}







		private void Form_Closing(object sender, FormClosedEventArgs e)
		{
			//폼이 닫히면 저장되게 만들어야함
			//탭페이지이름들을 저장하기
		}

		void SaveString()
		{
			string save_route = @"C:\saveString\";
			if (!Directory.Exists(save_route))
			{
				Directory.CreateDirectory(save_route);
			}
			FileInfo[] files = new DirectoryInfo(@"C:\saveString\").GetFiles("*.txt");

			if (files.Length != 0)
			{
				FileInfo[] last_ten = files.OrderBy(fi => fi.LastWriteTime).Take(10).ToArray();

				foreach (FileInfo fi in last_ten)
				{
					Console.WriteLine(fi.LastWriteTime);
					File.Delete(fi.FullName);
				}
			}
		}

		private List<string> ReadTextFileToList(string fileName)
		{
			List<string> readStreamReaderList = new List<string>();
		
						readStreamReaderList.Add(fileName);
		
			return readStreamReaderList;
		}



		private void OpenFileDialog()
		{
			List<string> rList = new List<string>();
			using (OpenFileDialog opd = new OpenFileDialog())
			{
				opd.DefaultExt = "All files";                               // 기본 파일타입 설정
				opd.Filter = "All files (*.*)|*.*"; // 파일타입
				opd.Multiselect = false;            // 다중선택되지 않도록.               
				string strAppDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				opd.InitialDirectory = strAppDir;   // 파일불러오기를 했을 때 제일 처음에 열리는 디렉토리 설정
				if (opd.ShowDialog() == DialogResult.OK)
				{
					try
					{
						if (opd.SafeFileName.LastIndexOf(".") > -1)
						{
							MessageBox.Show("확장자가 존재하는 파일은 선택하실 수 없습니다.");
							return;
						}
						string fileName = opd.FileName;
						// 선택한 파일을 Open
						rList = ReadTextFileToList(fileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
					}
				}
			}
		}

		private void RemoveTab_Click(object sender, EventArgs e)
		{
			tabControl1.TabPages.Remove(tabControl1.SelectedTab);
		}

		private void showDatagridview()
		{
			List<string> rList = new List<string>();

			using (OpenFileDialog fd = new OpenFileDialog())
			{
				fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //바탕화면으로 기본폴더 설정
				if (fd.ShowDialog() == DialogResult.OK)
				{
					//전체 경로와 파일명 
					filePath = fd.FileName;
					//선택한 파일명은 fd.SafeFileName
					fileName = fd.SafeFileName;
					//경로텍스트박스 초기화														
					toolStripTextBoxPath.Clear();
					//경로텍스트박스 경로내용추가
					toolStripTextBoxPath.AppendText(filePath);

					//탭추가
					string title = "TabPage " + (tabControl1.TabCount + 1).ToString();
					//탭페이지 선언
					TabPage myTabPage = new TabPage(fileName);
					//탭페이지이름
					myTabPage.Name = "TabPage" + (tabControl1.TabCount + 1).ToString();



					//데이터그리드뷰선언
					DataGridView dgv = new DataGridView();
					//데이터그리드뷰 컬럼 추가
					dgv.Columns.Add("content", "");
					//컬럼모드 fill
					dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
					//컬럼헤더 숨기기
					dgv.ColumnHeadersVisible = false;
					//데이터그리드뷰 숫자메소드
					dgv.RowPostPaint += dgv_RowPostPaint;
					//마지막행 삭제
					dgv.AllowUserToAddRows = false;
					//그리드칼라 화이트
					dgv.GridColor = Color.White;
					//데이터그리드뷰 전체선택
					dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					//데이터그리드뷰 네임지정
					dgv.Name = "DataGridView" + (tabControl1.TabCount + 1).ToString();
					//데이터그리드뷰 높이지정
					dgv.Width = 250;
					//데이터그리드뷰 텍스트지정
					dgv.Text = "DataGridView" + (tabControl1.TabCount + 1).ToString();
					//데이터그리드뷰 fill채우기
					dgv.Dock = DockStyle.Fill;
					//datagridview suspendlayout
					dgv.SuspendLayout();
					//한줄씩 읽고 추가하기
					string[] contents = System.IO.File.ReadAllLines(filePath);
					if (contents.Length > 0)
					{
						for (int i = 0; i < contents.Length; i++)
						{
							dgv.Rows.Add(contents[i]);
						}
					}
					//datagrudvuew resumelayout
					dgv.ResumeLayout();
					//탭페이지에 데이터그리드뷰 추가
					myTabPage.Controls.Add(dgv);
					//탭컨트롤에 탭페이지 추가
					tabControl1.TabPages.Add(myTabPage);
					//데이터그리드뷰 현재셀 취소
					dgv.ClearSelection();
					//탭페이지 추가된페이지로 선택이동
					tabControl1.SelectedTab = myTabPage;
					//감시프로그램 시작
					fileSystemWatcher1.Filter = toolStripTextBoxPath.Text.Substring(toolStripTextBoxPath.Text.LastIndexOf('\\') + 1);
					fileSystemWatcher1.Path = toolStripTextBoxPath.Text.Substring(0, toolStripTextBoxPath.Text.Length - fileSystemWatcher1.Filter.Length);
					//tail 기능 
					if (checkBoxTail.Checked)
					{
						//맨뒤로
						dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
					}
					//더블 클릭시 탭페이지 제거
					tabControl1.DoubleClick += TabControl1_DoubleClick;
				}

				rList = ReadTextFileToList(filePath);
			}
		}

		private void TabControl1_DoubleClick(object sender, EventArgs e)
		{
			//tabControl1.TabPages.Remove(page);
			closeCurrentTab(tabControl1);
		}

		private void Dgv_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				DataGridView grid = sender as DataGridView;
				ContextMenuStrip menu = new ContextMenuStrip();
				menu.Items.Add("Task1", null, new EventHandler(RemoveTab_Click));
				Point pt = grid.PointToClient(Control.MousePosition);
				menu.Show(dgv, pt.X, pt.Y);
			}
		}

		//탭제거 메소드
		public static void closeCurrentTab(TabControl tc)
		{
			int curTabIndex = tc.SelectedIndex;
			TabPage tp = tc.SelectedTab;
			tc.TabPages.Remove(tp);
			if (curTabIndex > 0)
			{
				tc.SelectedTab = tc.TabPages[(curTabIndex - 1)];
			}
		}

		//탭페이지생성후 데이터그리드뷰 생성
		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			showDatagridview();
		}


		//하이라이팅 폼 추가
		private void highLighting()
		{
			try
			{
				Form2 newform2 = new Form2();
				newform2.ShowDialog(this);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			highLighting();
		}


		private void Form1_Load(object sender, EventArgs e)
		{
			dgv.ReadOnly = true;
			comboBoxUtf.Items.Add("UTF-8");
			comboBoxUtf.SelectedIndex = 0;
			dgv.CellFormatting += dgv_CellFormatting;
		}




		private void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			numberCount(e);
		}


		void numberCount(DataGridViewRowPostPaintEventArgs e)
		{
			try
			{
				// row header 에 자동 일련번호 넣기
				StringFormat drawFormat = new StringFormat();
				//drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
				drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
				using (Brush brush = new SolidBrush(Color.Red))
				{
					e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, brush, e.RowBounds.Location.X + 35, e.RowBounds.Location.Y + 4, drawFormat);
				}
			}
			catch (Exception)
			{
			}
		}
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStripButton1_Click(sender, e);
		}
		private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

		}

		Thread threadFileView = null; //파일조회 스레드 개체 생성
		private delegate void OnDelegateFile(string fn, string fl, string fc);
		private OnDelegateFile OnFile = null; //델리게이트 생성
		bool HiddenFile = true; //전체 조회, False 일때는 숨김파일이 아닌경우만 조회

		private void button1_Click(object sender, EventArgs e)
		{
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				this.dgv.Rows.Clear();
				this.toolStripTextBoxPath.Text = this.folderBrowserDialog1.SelectedPath;

				threadFileView = new Thread(new ParameterizedThreadStart(FileView));
				threadFileView.Start(this.folderBrowserDialog1.SelectedPath); ///선택된 디렉토리를 매개변수로 넘기자.
			}

		}


		private void FileView(object dir)
		{
			DirectoryInfo di = new DirectoryInfo((string)dir);
			DirectoryInfo[] dti = di.GetDirectories();

			foreach (var f in di.GetFiles())
			{
				if (HiddenFile == true) //전체 파일을 모두 조회한다.
				{
					Invoke(OnFile, f.Name, f.Length.ToString(),
									f.CreationTime.ToString());
				}
				else
				{
					if (!f.Attributes.ToString().Contains(FileAttributes.Hidden.ToString())) // 속성이 숨김파일이 아닐 때만 조회한다.
					{
						Invoke(OnFile, f.Name, f.Length.ToString(),
										f.CreationTime.ToString());
					}
				}
			}

			for (int i = 0; i < di.GetDirectories().Length; i++) ///하위 디렉토리 파일을 조회 하자.
			{
				try
				{
					FileView(dti[i].FullName);
				}
				catch
				{
					continue;
				}
			}
		}



		private void clearToolStripMenuItemClear_Click(object sender, EventArgs e)
		{
			tabControl1.Controls.Remove(tabControl1.SelectedTab);
		}

		private void highlightingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			highLighting();
		}

		private void checkBoxTail_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxTail.Checked)
			{
				if (dgv.Rows.Count <= 0)
				{
					return;
				}
				//맨뒤로
				dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
			}
		}


		private void tabControl1_DragDrop(object sender, DragEventArgs e)
		{
			Point clientPoint = dgv.PointToClient(new Point(e.X, e.Y));
			if (e.Effect == DragDropEffects.Move)
			{
				// get 한 행을 삭제하고 원하는 위치에 넣어줍니다.
				DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
			}
		}

		private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
		{
			//탭제거
			tabControl1.TabPages.RemoveAt(0);
			//경로텍스트박스 초기화
			toolStripTextBoxPath.Clear();
			//경로텍스트박스 경로내용추가
			toolStripTextBoxPath.AppendText(filePath);
			//탭페이지 선언
			TabPage myTabPage = new TabPage(fileName);
			//데이터그리드뷰선언
			DataGridView dgv = new DataGridView();
			dgv.Rows.Clear();
			//데이터그리드뷰 컬럼 추가
			dgv.Columns.Add("content", "");
			//컬럼모드 fill
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			//컬럼헤더 숨기기
			dgv.ColumnHeadersVisible = false;
			//데이터그리드뷰 숫자메소드
			dgv.RowPostPaint += dgv_RowPostPaint;
			//마지막행 삭제
			dgv.AllowUserToAddRows = false;
			dgv.Name = "DataGridView" + (tabControl1.TabCount + 1).ToString();
			dgv.Width = 250;
			dgv.Text = "DataGridView" + (tabControl1.TabCount + 1).ToString();
			dgv.Dock = DockStyle.Fill;
			dgv.GridColor = Color.White;
			//datagridview suspendlayout
			dgv.SuspendLayout();
			//한줄씩 읽고 추가하기
			string[] contents = System.IO.File.ReadAllLines(filePath);
			if (contents.Length > 0)
			{
				for (int i = 0; i < contents.Length; i++)
				{
					dgv.Rows.Add(contents[i]);
				}
			}
			//datagrudvuew resumelayout
			dgv.ResumeLayout();
			myTabPage.Controls.Add(dgv);
			tabControl1.TabPages.Add(myTabPage);
			//감시프로그램 시작
			fileSystemWatcher1.Filter = toolStripTextBoxPath.Text.Substring(toolStripTextBoxPath.Text.LastIndexOf('\\') + 1);
			fileSystemWatcher1.Path = toolStripTextBoxPath.Text.Substring(0, toolStripTextBoxPath.Text.Length - fileSystemWatcher1.Filter.Length);
			//tail 기능
			if (checkBoxTail.Checked)
			{
				//맨뒤로
				dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
			}
		}



		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}


