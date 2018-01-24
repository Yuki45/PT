using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsApplication3
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form2()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 160);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "About";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(120, 160);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(152, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Dedications";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(32, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 32);
			this.label1.TabIndex = 3;
			this.label1.Text = "  Mira Reader (OCR)";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(56, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(184, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "\"We Help Computers Read\"";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Levenim MT", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label3.Location = new System.Drawing.Point(16, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(256, 72);
			this.label3.TabIndex = 5;
			this.label3.Text = "\" this charecter recogniton program is based on the Freeman,s chain-code (base se" +
				"ctors tracks) \"";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(64, 216);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(168, 24);
			this.button3.TabIndex = 6;
			this.button3.Text = "Recognize image";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// Form2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.Load += new System.EventHandler(this.Form2_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void button2_Click(object sender, System.EventArgs e)
		{
		}

		private void label3_Click(object sender, System.EventArgs e)
		{
		
		}

		public void Form2_Load(object sender, System.EventArgs e)
		{
			
			
		
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			Form1 f1=new Form1() ;
			f1.Visible=true ;
			Form2 f2=new Form2();
			f2.Close();
		}
	}
}
