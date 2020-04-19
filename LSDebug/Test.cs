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
        public static int integerVar = 0xDEAA;
        public static float floatVar= 0.1f;
        public static double doubleVar = 0.1;
        public static short shortVar = 0;
        public static byte byteVar = 0;
        public static byte charVar = 0;
        public static string stringVar = "";
        public static int seed = 15684;
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
            ASTest.Interval = 50;
            ASTest.Start();
        }
        private void ASTestTimer(object sender,EventArgs e)
        {
            //this.txbox.Location = new Point(this.txbox.Location.X, this.txbox.Location.Y + 3);
            LSRichDebug.SetVariable("Integer", integerVar);
            LSRichDebug.SetVariable("Float", floatVar);
            LSRichDebug.SetVariable("Double", doubleVar);
            LSRichDebug.SetVariable("Short", shortVar);
            LSRichDebug.SetVariable("Byte", byteVar);
            LSRichDebug.SetVariable("Char", (char)byteVar);
            LSRichDebug.SetVariable("String", stringVar);
            
            //LSRichDebug.PrintLine("TT0:" + LSRichDebug.LSTV.CheckInRow("TestVariable"));
            //LSRichDebug.PrintLine("TT1:" + LSRichDebug.LSTV.CheckInRow("TestVariable1"));
            //LSRichDebug.PrintLine("TT2:" + LSRichDebug.LSTV.CheckInRow("TestVariable2"));

            //LSRichDebug.SetVariable("TestVariable3", TestVariable3);
            //LSRichDebug.PrintLine(String.Format("TestVariable {0}", TestVariable), TextType.Info);
            //LSRichDebug.PrintLine(String.Format("TestVariable1 {0}", TestVariable1), TextType.Info);
            //LSRichDebug.PrintLine(String.Format("TestVariable2 {0}", TestVariable2), TextType.Info);
            //LSRichDebug.PrintLine(String.Format("TestVariable3 {0}", TestVariable3), TextType.Info);

            integerVar++;
            shortVar++;
            byteVar++;
            charVar++;
            floatVar = floatVar + 0.1f;
            doubleVar= doubleVar + 0.1;
            stringVar = "";
            
            for (int i = 0; i < 10; i++)
            {
                stringVar += Convert.ToString(Convert.ToChar((byte)(255*Math.Abs(Math.Sin(i*Math.PI/6+byteVar * Math.PI * 2 / 256)))));
            }

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
            LSRichDebug.PrintLine("TT:"+LSRichDebug.LSTV.GetIndexByVarName("TestVariable1"));
            
            //LSRichDebug.DumpBytes(testbytes,0x70000000);
            //LSRichDebug.DumpBytes(testbytes,0x128F0000,"HighAddress");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}

