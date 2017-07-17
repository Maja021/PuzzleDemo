using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;


namespace Puzzle
{


	public class formPuzzle : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		public int iCols;
		public int iRows;


		Shapes[] piece = new Shapes[12];
		private System.Windows.Forms.MenuItem menuItem1;

		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MainMenu PentominosMenu;
		private System.Windows.Forms.MenuItem mnuNew;

		public System.Windows.Forms.StatusBar stBar;
		private Shapes draggedpiece;

		public formPuzzle()
		{

			InitializeComponent();

			draggedpiece = null;

			iCols = 10;
			iRows = 5;

			InitForm();


		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.PentominosMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.stBar = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// PentominosMenu
			// 
			this.PentominosMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuItem1,
																						   });
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuNew,
																					  this.menuItem2,
																					});
			this.menuItem1.Text = "Game";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Text = "New...";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";



			this.stBar.Location = new System.Drawing.Point(0, 387);
			this.stBar.Name = "stBar";
			this.stBar.ShowPanels = true;
			this.stBar.Size = new System.Drawing.Size(512, 22);
			this.stBar.SizingGrip = false;
			this.stBar.TabIndex = 0;

			//
			// formPuzzle
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 409);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.stBar});
			this.Menu = this.PentominosMenu;
			this.Name = "formPuzzle";
			this.Text = "Puzzle";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formPuzzle_MouseDown);
			this.Load += new System.EventHandler(this.formPuzzle_Load);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.formPuzzle_MouseMove);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new formPuzzle());
		}

		private void formPuzzle_Load(object sender, System.EventArgs e)
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.Opaque, true);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{



			Graphics grPaint = e.Graphics;

			grPaint.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);

			SolidBrush brushWhite = new SolidBrush(Color.LightSkyBlue);
			Pen blackpen = new Pen(Color.LightYellow, 4);

			// Clear the screen
			grPaint.FillRectangle(brushWhite, e.ClipRectangle);

			// Draw the grid
			grPaint.DrawString("Left - Click on a piece do drag or drop it", new Font("Arial", 12), new SolidBrush(Color.LightYellow), 20, 10);
			grPaint.DrawString("Right - Click to rotate and flip it (some pieces doesn´t flip)", new Font("Arial", 12), new SolidBrush(Color.LightYellow), 20, 30);

			for (int i = 0; i < iCols; i++)
			{
				for (int j = 0; j < iRows; j++)
				{
					grPaint.DrawRectangle(blackpen, 100 + (i * 20), 60 + (j * 20), 20, 20);
				}
			}

			for (int i = 0; i < 12; i++)
			{
				piece[i].Draw(grPaint);
			}
			brushWhite.Dispose();
			blackpen.Dispose();

		}

		protected override void OnDoubleClick(System.EventArgs e)
		{

		}

		private void formPuzzle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && draggedpiece != null)
			{
				draggedpiece.RotatePiece();
				Invalidate(new Rectangle(draggedpiece.GetX() - 2, draggedpiece.GetY() - 2, 120, 120));
			}

			if (e.Button == MouseButtons.Left && draggedpiece != null)
			{
				if (draggedpiece != null)
				{
					draggedpiece = null;

					int iCount = 0;

					// Check if the user has finished the game

					for (int iPosX = 100; iPosX < 100 + (iCols * 20); iPosX += 20)
					{
						for (int iPosY = 60; iPosY < 60 + (iRows * 20); iPosY += 20)
						{
							for (int i = 0; i < 12; i++)
							{
								if (piece[i].Hitted(iPosX, iPosY))
								{
									iCount++;
									break;
								}
							}
						}
					}

					if (iCount >= 50)
					{
						MessageBox.Show("Congratulations ! You´ve finished !", "Puzzle", MessageBoxButtons.OK, MessageBoxIcon.Information);

					}
					return;
				}
			}

			if (e.Button == MouseButtons.Left && draggedpiece == null)
			{


				int iPosX = e.X / 20;
				int iPosY = e.Y / 20;

				iPosX *= 20;
				iPosY *= 20;

				for (int i = 0; i < 12; i++)
				{
					if (piece[i].Hitted(iPosX, iPosY))
					{
						draggedpiece = piece[i];
						break;
					}
				}
			}
		}

		private void formPuzzle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (draggedpiece != null)
			{

				int iPosX = e.X / 20;
				int iPosY = e.Y / 20;

				iPosX *= 20;
				iPosY *= 20;

				if (iPosX != draggedpiece.GetX() || iPosY != draggedpiece.GetY())
				{
					Rectangle rect = new Rectangle(draggedpiece.GetX() - 2, draggedpiece.GetY() - 2, draggedpiece.GetWidth() + 4, draggedpiece.GetHeight() + 4);
					draggedpiece.SetPos(iPosX, iPosY);
					Region rgn = new Region(rect);
					rect = new Rectangle(iPosX - 2, iPosY - 2, draggedpiece.GetWidth() + 4, draggedpiece.GetHeight() + 4);
					rgn.Union(rect);
					Invalidate(rgn, false);
				}


			}
			else
			{
				int iPosX = e.X / 20;
				int iPosY = e.Y / 20;

				iPosX *= 20;
				iPosY *= 20;

				for (int i = 0; i < 12; i++)
				{
					if (piece[i].Hitted(iPosX, iPosY))
					{
						break;
					}
				}
			}
		}

		public void InitForm()
		{
			System.Reflection.Assembly asm = GetType().Assembly;
			System.IO.Stream stm = asm.GetManifestResourceStream("Puzzle.Hand.cur");


			stm = asm.GetManifestResourceStream("Puzzle.Handpc.cur");



			stBar.Panels.Clear();

			StatusBarPanel pane1 = new StatusBarPanel();


			pane1.AutoSize = StatusBarPanelAutoSize.Spring;
			pane1.Text = "10x5 board";


			stBar.Panels.Add(pane1);


			for (int i = 0; i < 12; i++)
			{
				piece[i] = new Shapes();
				piece[i].CreatePiece(i + 1);
				piece[i].SetPos(20 + ((i % 6) * 80), i >= 6 ? 280 : 200);
			}


		}


		private void mnuNew_Click(object sender, System.EventArgs e)
		{
			InitForm();
			Invalidate(false);
		}




	}
}
