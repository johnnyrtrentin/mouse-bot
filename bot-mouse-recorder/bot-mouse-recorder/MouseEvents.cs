using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace bot_mouse_recorder
{
    public partial class MouseEvents : Form
    {
        ListViewItem lv;
        private readonly ListView listView1 = new ListView();
        private int a, b;


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public MouseEvents()
        {
            InitializeComponent();
        }

        private void MouseEvents_Load(object sender, EventArgs e)
        {
            a = 0;
            b = 0;
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            this.SetRecordButtonText("Recording");
            timer1.Start();

            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            Cursor.Position = new System.Drawing.Point(x, y);
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            this.SetRecordButtonText("Record");
            timer1.Stop();
            timer2.Stop();
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            this.SetRecordButtonText("Record");
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RecordMousePosition();
            this.RecordMouseClicks();
            this.IncreaseHandler();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (a != b)
            {
                Cursor.Position = new System.Drawing.Point(
                    int.Parse(listView1.Items[a].SubItems[0].Text),
                    int.Parse(listView1.Items[a].SubItems[1].Text)
                );

                if (listView1.Items[a].SubItems[2].Text == "Left")
                {
                    this.ReproduceMouseClick();
                    Thread.Sleep(500);
                }

                this.IncreaseMainHandler();
            }
        }
        private void RecordMousePosition()
        {
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            listView1.Items.Add(lv);

        }
 
        private void RecordMouseClicks()
        {
            lv.SubItems.Add(MouseButtons.ToString());
        }

        private void ReproduceMouseClick()
        {
            try
            {
                var x = Convert.ToInt32(listView1.Items[a].SubItems[0].Text);
                var y = Convert.ToInt32(listView1.Items[a].SubItems[1].Text);

                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SetRecordButtonText(String value)
        {
            Btn1.Text = value;
        }

        private void IncreaseMainHandler()
        {
            a++;
        }

        private void IncreaseHandler()
        {
            b++;
        }
    }
}