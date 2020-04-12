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

        public Test()
        {
            InitializeComponent();
            LSRichDebug.Debug();
            Button cbbutton = new Button();
            this.Controls.Add(cbbutton);
            cbbutton.Click += ButtonClick;
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
            byte[] testbytes = File.ReadAllBytes("testbytes.exe");
            LSRichDebug.DumpBytes(testbytes);
            LSRichDebug.DumpBytes(testbytes);
            LSRichDebug.DumpBytes(testbytes);
            //LSRichDebug.DumpBytes(testbytes,0x70000000);
            //LSRichDebug.DumpBytes(testbytes,0x128F0000,"HighAddress");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            

        }
    }
}

