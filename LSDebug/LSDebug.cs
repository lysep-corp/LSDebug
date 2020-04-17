using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
namespace LSDebug
{
    public enum TextType
    {
        Info = 0,
        Warning = 1,
        Danger = 2,
        Safe=3,
        Success=4,
        Failed=5
    }
    public enum ResizeType
    {
        Percent = 0,
        Pixel = 1
    }
    class LSSplitterPanel : Panel
    {
        public Panel Panel1, Panel2;
        public ResizeType ResizeType=ResizeType.Pixel;
        private int WidthPercentPanel_ = 30, WidthPanel_;
        public int WidthPercentPanel {
            get
            {
                return WidthPercentPanel_;
            }
            set
            {
                WidthPercentPanel_ = value;
                ResizeType = ResizeType.Percent;
                InitializePanelStyle();
            }
        }
        public int WidthPanel{
            get
            {
                return WidthPanel_;
            }
            set
            {
               WidthPanel_ = value;
                ResizeType = ResizeType.Pixel;
                InitializePanelStyle();
            }
        }
        private bool Reverse_=false;
        public bool Reverse {
            get {
                return Reverse_;
            }
            set
            {
                Reverse_ = value;
                InitializePanelStyle();
            }
        }
        public LSSplitterPanel(Panel panel1,Panel panel2) : base()
        {
            InitializeStyle();
            this.Panel1 = panel1;
            this.Panel2 = panel2;
            WidthPanel = this.Width / 2;
            this.Controls.Add(Panel1);
            this.Controls.Add(Panel2);
        }
        public LSSplitterPanel() : base()
        {
            InitializeStyle();
            this.Panel1 = new Panel();
            this.Panel2 = new Panel();
            WidthPanel = this.Width / 2;

            this.Panel1.BackColor = Color.Red;
            this.Panel2.BackColor = Color.Green;
            this.Controls.Add(Panel1);
            this.Controls.Add(Panel2);
        }

        private void InitializeStyle()
        {
            this.BorderStyle = BorderStyle.None;
            this.BackColor = Color.Purple;// Color.FromArgb(23, 23, 23);
            this.ForeColor = Color.FromArgb(238, 238, 238);
            this.Dock = DockStyle.Fill;
            this.Resize += this.OnResizePanel;
        }
        public void InitializePanelStyle()
        {
            Panel p1, p2;
            p1 = Reverse_ ? Panel2 : Panel1;
            p2 = Reverse_ ? Panel1 : Panel2;
            if(p1 != null && p2 != null) { 
                int p1w, p2w;
                p1.Height = this.Height;
                p2.Height = this.Height;
                p1w = ResizeType == ResizeType.Pixel ? WidthPanel_ : this.Width * WidthPercentPanel_ / 100;
                p2w = this.Width - p1w;
                p1.Width = p1w;
                p2.Left = p1w;
                p2.Width = p2w;
            }
        }
        private void OnResizePanel(object sender,EventArgs e)
        {
            InitializePanelStyle();
        }
       

    }
    class LSDebugUI : Form{
        private LSDebug LST = new LSDebug();
        private LSDebugVariable LSTV = new LSDebugVariable();
        private LSSplitterPanel SPC = new LSSplitterPanel();
        public LSDebugUI() : base()
        {
            SPC.BackColor = Color.FromArgb(23, 23, 23);
            SPC.Dock = DockStyle.Fill;
            SPC.Visible = true;
            SPC.BorderStyle = BorderStyle.None;
            this.Controls.Add(SPC);
            SPC.Panel1.Controls.Add(LSTV);
            SPC.Panel2.Controls.Add(LST);
            SPC.WidthPanel= 290;
            this.Size = new Size(870, 460);

            LST.Visible = true;
            LST.Dock = DockStyle.Fill;

            LSTV.Visible = true;
            LSTV.Dock = DockStyle.Fill;

        }
        public bool Symbolizator
        {
            get
            {
                return LST.Symbolizator;
            }
            set
            {
                LST.Symbolizator = value;
            }
        }
        public bool TimeBool
        {
            get
            {
                return LST.TimeBool;
            }
            set
            {
                LST.TimeBool = value;
            }
        }
        #region VariableDebugger
        public async void AddVariable(string VariableName,int value)
        {
            LSTV.AddVariable(VariableName, value);
        }
        #endregion
        #region ConsoleDebugger
        public void Debug()
        {
            this.Show();
        }
        public async void Print(String text)
        {
                this.LST.Print(text);
        }
        public async void PrintLine(String text)
        {
                this.LST.PrintLine(text);
        }
        public async void Print(String text,Color color)
        {
                this.LST.Print(text, color);
        }
        public async void PrintLine(String text, Color color)
        {
                this.LST.PrintLine(text, color);
        }
        public async void Print(String text, TextType type)
        {
                this.LST.Print(text, type);
        }
        public async void PrintLine(String text, TextType type)
        {
                this.LST.PrintLine(text, type);
        }
        public async void DumpBytes(byte[] bytes)
        {
                this.LST.DumpBytes(bytes);
        }
        public async void DumpBytes(byte[] bytes,int offset)
        {
                this.LST.DumpBytes(bytes, offset);
        }
        public async void DumpBytes(byte[] bytes,int offset,string label)
        {
                this.LST.DumpBytes(bytes, offset, label);
        }
        #endregion
    }
    #region Build Of ConsoleDebugger
    class LSDebug : RichTextBox
    {
        public static Color MainThemeColor = Color.FromArgb(9, 9, 7);
        public static Color SecondaryThemeColor = Color.FromArgb(150, 0, 0);
        public static Color TextColor = Color.FromArgb(238, 238, 238);
        public static Color WarningColor = Color.FromArgb(237,194,0);
        public static Color DangerColor = Color.FromArgb(255,0,0);
        public static Color InfoColor = Color.FromArgb(173, 173, 173);
        public static Color SafeColor = Color.FromArgb(0, 141, 242);
        public static Color SuccessColor = Color.FromArgb(0, 245, 41);
        public static Color FailedColor = Color.FromArgb(252, 16, 13);
        public LSDebug() : base()
        {
            this.BackColor = MainThemeColor;
            this.ForeColor = TextColor;
            this.Margin = new Padding(8, 8, 8, 8);
            this.Font = new Font(new FontFamily("Consolas"),
                                 9f,
                                 FontStyle.Regular,
                                 GraphicsUnit.Point
            );
            this.BorderStyle = BorderStyle.None;
            this.ReadOnly = true;
            this.Cursor = Cursors.Default;
        }
        #region Hex Dumping
        public int H_BYTESOFLINE=16;
        public int H_MARGINON=0X7;
        public Color AddressColor = Color.FromArgb(255, 149, 0);
        public Color ByteColor = Color.FromArgb(0, 255, 0);
        public void DumpBytes(byte[] bytes)
        {
            for (int i = 0; i <= bytes.Length/H_BYTESOFLINE; i++)
            {
                int LineOffset = i * H_BYTESOFLINE;
                int GetTopOfSize = bytes.Length-LineOffset <= 16 ? bytes.Length - LineOffset : 16;
                this.Write(String.Format("0x{0}\t", LineOffset.ToString("X8")),AddressColor);
                for (int j = 0; j < H_BYTESOFLINE; j++)
                {
                    if(j<GetTopOfSize)
                    {
                        this.Write(bytes[LineOffset + j].ToString("X2"),ByteColor);
                        if (!(GetTopOfSize == j - 1))
                        {
                            this.Write(" ");
                        }
                    }
                    else
                    {
                        this.Write("  ");
                        if (!(H_BYTESOFLINE == j - 1))
                        {
                            this.Write(" ");
                        }
                    }
                    if (H_MARGINON == j)
                    {
                        this.Write(" ");
                    }

                }
                this.Write("  ");
                for (int j = 0; j < H_BYTESOFLINE; j++)
                {
                    if (j < GetTopOfSize)
                    {
                        byte tb = bytes[LineOffset + j];
                        this.Write(tb < 32 ? "." : ((char)tb).ToString());
                    }
                }
                this.Write("\n");
            }
            GotoBottom();
        }
        public void DumpBytes(byte[] bytes,int offset)
        {
            this.Write("Dumping from ");
            this.Write(String.Format("0x{0}", offset.ToString("X8")),AddressColor);
            this.Write("\n");
            for (int i = 0; i <= bytes.Length / H_BYTESOFLINE; i++)
            {
                int LineOffset = i * H_BYTESOFLINE;
                int GetTopOfSize = bytes.Length - LineOffset <= 16 ? bytes.Length - LineOffset : 16;
                this.Write("\t");
                this.Write(String.Format("0x{0}\t", (offset+LineOffset).ToString("X8")), AddressColor);
                for (int j = 0; j < H_BYTESOFLINE; j++)
                {
                    if (j < GetTopOfSize)
                    {
                        this.Write(bytes[LineOffset + j].ToString("X2"), ByteColor);
                        if (!(GetTopOfSize == j - 1))
                        {
                            this.Write(" ");
                        }
                    }
                    else
                    {
                        this.Write("  ");
                        if (!(H_BYTESOFLINE == j - 1))
                        {
                            this.Write(" ");
                        }
                    }
                    if (H_MARGINON == j)
                    {
                        this.Write(" ");
                    }

                }
                this.Write("  ");
                for (int j = 0; j < H_BYTESOFLINE; j++)
                {
                    if (j < GetTopOfSize)
                    {
                        byte tb = bytes[LineOffset + j];
                        this.Write(tb < 32 ? "." : ((char)tb).ToString());
                    }
                }
                this.Write("\n");
            }
            GotoBottom();
        }
        public void DumpBytes(byte[] bytes, int offset,string label)
        {
            this.Write(" [");
            this.Write(String.Format("0x{0}", offset.ToString("X8")), AddressColor);
            this.Write(String.Format("]{0}\n",label));
            for (int i = 0; i <= bytes.Length / H_BYTESOFLINE; i++)
            {
                int LineOffset = i * H_BYTESOFLINE;
                int GetTopOfSize = bytes.Length - LineOffset <= 16 ? bytes.Length - LineOffset : 16;
                this.Write("\t");
                this.Write(String.Format("0x{0}\t", (offset + LineOffset).ToString("X8")), AddressColor);
                for (int j = 0; j < H_BYTESOFLINE; j++)
                {
                    if (j < GetTopOfSize)
                    {
                        this.Write(bytes[LineOffset + j].ToString("X2"), ByteColor);
                        if (!(GetTopOfSize == j - 1))
                        {
                            this.Write(" ");
                        }
                    }
                    else
                    {
                        this.Write("  ");
                        if (!(H_BYTESOFLINE == j - 1))
                        {
                            this.Write(" ");
                        }
                    }
                    if (H_MARGINON == j)
                    {
                        this.Write(" ");
                    }

                }
                this.Write("  ");
                for (int j = 0; j < H_BYTESOFLINE; j++)
                {
                    if (j < GetTopOfSize)
                    {
                        byte tb = bytes[LineOffset + j];
                        this.Write(tb < 32 ? "." : ((char)tb).ToString());
                    }
                }
                this.Write("\n");
            }
        }
        #endregion
        #region Date And Symbolization
        public bool TimeBool = true, Symbolizator = true;
        private void LogDate()
        {
            this.AppendText("[");
            this.SelectionColor = WarningColor;
            this.AppendText(DateTime.Now.ToShortDateString());
            this.AppendText(" ");
            this.AppendText(DateTime.Now.ToLongTimeString());
            this.SelectDefaultColor();
            this.AppendText("]");
        }
        public void AppendSymbolizator(TextType type)
        {
            if (!Symbolizator)
            {
                return;
            }
            Color tempcolor = this.SelectionColor;
            this.SelectionColor = TextColor;
            this.AppendText("[");
            this.SelectionColor = tempcolor;
            if (TextType.Warning.Equals(type))
            {
                this.SelectionColor = WarningColor;
                this.AppendText("?");
            }
            else if (TextType.Danger.Equals(type))
            {
                this.SelectionColor = DangerColor;
                this.AppendText("!");
            }
            else if (TextType.Failed.Equals(type))
            {
                this.SelectionColor = FailedColor;
                this.AppendText("-");
            }
            else if (TextType.Success.Equals(type))
            {
                this.SelectionColor = SuccessColor;

                this.AppendText("+");
            }
            else
            {
                this.SelectionColor = SafeColor;
                this.AppendText("*");

            }
            this.SelectionColor = TextColor;
            this.AppendText("]");
            this.SelectionColor = tempcolor;
        }
        #endregion
        #region Console Raw Printing
        public void Write(string Text)
        {
            this.AppendText(Text);
        }
        public void GotoBottom()
        {
            this.SelectionStart = this.Text.Length;
            this.ScrollToCaret();
        }
        public void Write(string Text,Color color)
        {
            Color tmpcolor = this.SelectionColor;
            this.SelectionColor = color;
            this.AppendText(Text);
            this.SelectionColor = tmpcolor;
        }
        #endregion
        #region Console Printing
        public void Print(String text)
        {
            if (TimeBool)
            {
                this.LogDate();
            }
            this.Write(text);
            GotoBottom();
        }
        public void PrintLine(String text)
        {
            if (TimeBool)
            {
                this.LogDate();
            }
            this.Write(text);
            this.AppendText("\n");
            GotoBottom();
        }
        public void Print(String text,Color color)
        {
            if (TimeBool)
            {
                this.LogDate();
            }
            this.Write(text,color);
            GotoBottom();
        }
        public void PrintLine(String text,Color color)
        {
            if (TimeBool)
            {
                this.LogDate();
            }
            this.Write(text,color);
            this.AppendText("\n");
            GotoBottom();
        }
        public void Print(String text,TextType type)
        {
            if (TimeBool)
            {
                this.LogDate();
            }
            SelectTypeOfColor(type);
            this.AppendSymbolizator(type);
            this.Write(text);
            this.SelectDefaultColor();
            GotoBottom();
        }
        public void PrintLine(String text, TextType type)
        {
            if (TimeBool)
            {
                this.LogDate();
            }
            SelectTypeOfColor(type);
            this.AppendSymbolizator(type);
            this.Write(text);
            this.SelectDefaultColor();
            this.Write("\n");
            GotoBottom();
        }
        #endregion
        #region Color Selection
        public void SelectTypeOfColor(TextType type)
        {
            if (TextType.Warning.Equals(type))
            {
                this.SelectionColor = WarningColor;

            }
            else if (TextType.Info.Equals(type))
            {
                this.SelectionColor = InfoColor;

            }
            else if (TextType.Danger.Equals(type))
            {
                this.SelectionColor = DangerColor;

            }
            else if (TextType.Safe.Equals(type))
            {
                this.SelectionColor = SafeColor;

            }
            else if (TextType.Success.Equals(type))
            {
                this.SelectionColor = SuccessColor;
            }
            else
            {
                this.SelectionColor = InfoColor;
            }
        }
        public void SelectDefaultColor()
        {
            this.SelectionColor = TextColor;
        }
        #endregion
    }
    #endregion
    #region Build Of VariableDebugger
    class LSDebugVariable : DataGridView
    {
        public static Color MainThemeColor = Color.FromArgb(9, 9, 7);
        public static Color MainGridColor = Color.FromArgb(15, 15, 13);
        public static Color SecondaryThemeColor = Color.FromArgb(150, 0, 0);
        public static Color TextColor = Color.FromArgb(238, 238, 238);
        public static Color CellStyle_BackgroundColor = Color.FromArgb(21, 21, 21);
        public static Color CellStyle_SelectionColor = Color.FromArgb(29, 29, 29);
        public static Color CellStyleHeader_BackgroundColor = Color.FromArgb(25, 25, 25);
        public static Color CellStyleHeader_SelectionColor = Color.FromArgb(29, 29, 29);
        public LSDebugVariable() : base()
        {
            this.BackgroundColor = MainThemeColor;
            this.GridColor = MainGridColor;
            this.ForeColor = TextColor;
            this.Margin = new Padding(8, 8, 8, 8);
            this.Font = new Font(new FontFamily("Consolas"),
                                 9f,
                                 FontStyle.Regular,
                                 GraphicsUnit.Point
            );
            this.BorderStyle = BorderStyle.None;
            this.DefaultCellStyle.BackColor = CellStyle_BackgroundColor;
            this.DefaultCellStyle.SelectionBackColor = CellStyle_SelectionColor;
            this.DefaultCellStyle.ForeColor = TextColor;
            this.ColumnHeadersDefaultCellStyle.BackColor = CellStyleHeader_BackgroundColor;
            this.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            this.RowHeadersDefaultCellStyle.BackColor = CellStyleHeader_BackgroundColor;
            this.RowHeadersDefaultCellStyle.ForeColor = TextColor;
            this.RowHeadersDefaultCellStyle.Padding = new Padding(3, 2, 3, 2);
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.BorderStyle = BorderStyle.None;
            this.ColumnHeadersBorderStyle= DataGridViewHeaderBorderStyle.None;
            this.ReadOnly = true;
            this.EnableHeadersVisualStyles = false;
            DataGridViewCell cellTemplate = new DataGridViewTextBoxCell();

            DataGridViewColumn nameColumn = new DataGridViewColumn();
            nameColumn.Name = "Name";
            nameColumn.HeaderText= "Name";
            nameColumn.CellTemplate = cellTemplate;
            DataGridViewColumn valueColumn = new DataGridViewColumn();
            valueColumn.Name = "Value";
            valueColumn.HeaderText = "Value";
            valueColumn.CellTemplate = cellTemplate;
            DataGridViewColumn typeColumn = new DataGridViewColumn();
            typeColumn.Name = "Type";
            typeColumn.HeaderText = "Type";
            typeColumn.CellTemplate = cellTemplate;
            typeColumn.Width = 50;
            this.Columns.Add(nameColumn);
            this.Columns.Add(valueColumn);
            this.Columns.Add(typeColumn);
        }
        public void AddVariable(string VariableName,int value)
        {
            DataGridViewRow row = (DataGridViewRow)this.Rows[0].Clone();
            row.Tag = VariableName;
            row.Cells[0].Value = VariableName;
            row.Cells[1].Value = String.Format("0x{0}", value.ToString("X8"));
            row.Cells[2].Value = "int";
            this.Rows.Add(row);
            //ListViewItem item = new ListViewItem(VariableName);
            //item.SubItems.Add(VariableName);
            //item.SubItems.Add(String.Format("0x{0}", value.ToString("X8")));
            //item.SubItems.Add("int");
        }

    }
    #endregion
}
