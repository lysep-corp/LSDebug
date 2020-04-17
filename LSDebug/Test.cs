using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace LSDebug
{
    public partial class Test : Form
    {
        LSDebugUI LSRichDebug = new LSDebugUI();
        Button cbbutton_ = new Button();
        TextBox txbox = new TextBox();
        Button cbbutton = new Button();
        Timer ASTest = new Timer();
        public Test()
        {
            InitializeComponent();
            LSRichDebug.Debug();
            cbbutton.Text = "B1";
            this.Controls.Add(cbbutton);
            cbbutton.Click += ButtonClick;
            cbbutton_.Location = new Point(cbbutton.Width+5,0);
            cbbutton_.Text = "B2";
            this.Controls.Add(cbbutton_);
            cbbutton_.Click += ButtonClick_;
            txbox.Location = new Point(cbbutton_.Size.Width + cbbutton.Width + 5 + 5, 0);
            this.Controls.Add(txbox);
            ASTest.Tick += ASTestTimer;
            ASTest.Interval = 500;
            //ASTest.Start();
        }
        private void ASTestTimer(object sender,EventArgs e)
        {
            this.txbox.Location = new Point(this.txbox.Location.X, this.txbox.Location.Y + 3);
        }
        private void ButtonClick_(object sender,EventArgs e)
        {
            byte[] testbytes = File.ReadAllBytes("testbytes.exe");
            LSRichDebug.DumpBytes(testbytes);
        }
        private void ButtonClick(object sender,EventArgs e)
        {
            LSRichDebug.PrintLine("Info", TextType.Info);
            LSRichDebug.PrintLine("Danger", TextType.Danger);
            LSRichDebug.PrintLine("Failed", TextType.Failed);
            LSRichDebug.PrintLine("Warning", TextType.Warning);
            LSRichDebug.PrintLine("Safe", TextType.Safe);
            LSRichDebug.PrintLine("Success", TextType.Success);
            LSRichDebug.PrintLine("Hi");
            LSRichDebug.Symbolizator = false;
            LSRichDebug.PrintLine("Info", TextType.Info);
            LSRichDebug.PrintLine("Danger", TextType.Danger);
            LSRichDebug.PrintLine("Failed", TextType.Failed);
            LSRichDebug.PrintLine("Warning", TextType.Warning);
            LSRichDebug.PrintLine("Safe", TextType.Safe);
            LSRichDebug.PrintLine("Success", TextType.Success);
            LSRichDebug.PrintLine("Hi");
            LSRichDebug.TimeBool = false;
            LSRichDebug.PrintLine("Info", TextType.Info);
            LSRichDebug.PrintLine("Danger", TextType.Danger);
            LSRichDebug.PrintLine("Failed", TextType.Failed);
            LSRichDebug.PrintLine("Warning", TextType.Warning);
            LSRichDebug.PrintLine("Safe", TextType.Safe);
            LSRichDebug.PrintLine("Success", TextType.Success);
            LSRichDebug.PrintLine("Hi");
            LSRichDebug.AddVariable("Test", 0XDEAD);
            //LSRichDebug.DumpBytes(testbytes,0x70000000);
            //LSRichDebug.DumpBytes(testbytes,0x128F0000,"HighAddress");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}

