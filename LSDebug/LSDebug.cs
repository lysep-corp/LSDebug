using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Windows.Media.Imaging;
using System.IO;

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
    #region LS Splitter Panel
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
    #endregion
    #region LSDebugUI Main
    class LSDebugUI : Form{
        private LSDebug LST = new LSDebug();
        public LSDebugVariable LSTV = new LSDebugVariable();
        public LSSplitterPanel SPC = new LSSplitterPanel();
        public LSDebugUI() : base()
        {
            SPC.BackColor = Color.FromArgb(23, 23, 23);
            SPC.Dock = DockStyle.Fill;
            SPC.Visible = true;
            SPC.BorderStyle = BorderStyle.None;
            this.Controls.Add(SPC);
            SPC.Panel1.Controls.Add(LSTV);
            SPC.Panel2.Controls.Add(LST);
            SPC.WidthPanel= 380;
            this.Size = new Size(980, 460);

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
        public async void SetVariable(string VariableName,int value)
        {
            LSTV.SetVariable(VariableName, value);
        }
        #endregion
        #region ConsoleDebugger
        public void Debug()
        {
            this.Show();
        }
        public async void Print(object text)
        {
                this.LST.Print(text.ToString());
        }
        public async void PrintLine(object text)
        {
                this.LST.PrintLine(text.ToString());
        }
        public async void Print(object text,Color color)
        {
                this.LST.Print(text.ToString(), color);
        }
        public async void PrintLine(object text, Color color)
        {
                this.LST.PrintLine(text.ToString(), color);
        }
        public async void Print(object text, TextType type)
        {
                this.LST.Print(text.ToString(), type);
        }
        public async void PrintLine(object text, TextType type)
        {
                this.LST.PrintLine(text.ToString(), type);
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
    #endregion
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
    #region Base64Texture
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
        public static Bitmap VariableIcon = Base64StringToBitmap("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"); 
        public static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;
            return bmpReturn;
        }
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
            this.RowHeadersWidth = 30;
            this.BorderStyle = BorderStyle.None;
            this.ColumnHeadersBorderStyle= DataGridViewHeaderBorderStyle.None;
            this.ReadOnly = true;
            this.EnableHeadersVisualStyles = false;
            this.RowPostPaint += RowPostPaintEvent;
            DataGridViewCell cellTemplate = new DataGridViewTextBoxCell();

            DataGridViewColumn nameColumn = new DataGridViewColumn();
            nameColumn.Name = "Name";
            nameColumn.HeaderText= "Name";
            nameColumn.CellTemplate = cellTemplate;
            DataGridViewColumn valueColumn = new DataGridViewColumn();
            valueColumn.Name = "Value";
            valueColumn.HeaderText = "Value";
            valueColumn.CellTemplate = cellTemplate;
            DataGridViewColumn hexColumn = new DataGridViewColumn();
            hexColumn.Name = "Hex";
            hexColumn.HeaderText = "Hex";
            hexColumn.CellTemplate = cellTemplate;
            DataGridViewColumn typeColumn = new DataGridViewColumn();
            typeColumn.Name = "Type";
            typeColumn.HeaderText = "Type";
            typeColumn.CellTemplate = cellTemplate;
            typeColumn.Width = 50;
            this.Columns.Add(nameColumn);
            this.Columns.Add(valueColumn);
            this.Columns.Add(hexColumn);
            this.Columns.Add(typeColumn);
        }
        public void RowPostPaintEvent(object sender,DataGridViewRowPostPaintEventArgs e)
        {

            if (this.Rows[e.RowIndex].Cells[0].Value != null) {
                Icon VariableIco = Icon.FromHandle(VariableIcon.GetHicon());
                Graphics graphics = e.Graphics;

                int w = 20;
                int h = 20;
                int x = e.RowBounds.X + ((this.RowHeadersWidth-w) / 2);
                int y = e.RowBounds.Y + (this.Rows[e.RowIndex].Height - h) / 2;

                Rectangle rectangle = new Rectangle(x, y, w, h);
                graphics.DrawIcon(VariableIco, rectangle);
            }

        }
        public bool CheckInRow(string VariableName)
        {
            foreach (DataGridViewRow item in this.Rows)
            {
                if(item.Cells[0].Value != null) { 
                    if (item.Cells[0].Value.ToString().Equals(VariableName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public int GetIndexByVarName(string VariableName)
        {
            for (int i = 0; i < this.Rows.Count; i++)
            {
                DataGridViewRow element = this.Rows[i];
                if(element.Cells[0].Value != null)
                {
                    if (element.Cells[0].Value.ToString().Equals(VariableName))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        private string tmpv="";
        private int tmpin;
        public void SetVariable(string VariableName,int value)
        {
            if (tmpv.Equals(VariableName))
            {
                this.Rows[tmpin].Cells[1].Value = value;
                this.Rows[tmpin].Cells[2].Value = String.Format("0x{0}", value.ToString("X8"));
                return;
            }
            tmpv = VariableName;
            if (CheckInRow(VariableName)) {
                int i = GetIndexByVarName(VariableName);
                tmpin = i;

                if (i >= 0)
                {
                    this.Rows[i].Cells[1].Value = value;
                    this.Rows[i].Cells[2].Value = String.Format("0x{0}", value.ToString("X8"));
                }
            }
            else
            {
                DataGridViewRow row = (DataGridViewRow)this.Rows[0].Clone();
                row.Tag = VariableName;
                row.Cells[0].Value = VariableName;
                row.Cells[1].Value = value;
                row.Cells[2].Value = String.Format("0x{0}", value.ToString("X8"));
                row.Cells[3].Value = "int";
                row.Height = 30;
                tmpin = this.Rows.Add(row);
                
            }
            //ListViewItem item = new ListViewItem(VariableName);
            //item.SubItems.Add(VariableName);
            //item.SubItems.Add(String.Format("0x{0}", value.ToString("X8")));
            //item.SubItems.Add("int");
        }

    }
    #endregion
}
