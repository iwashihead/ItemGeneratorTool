using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ItemGenerator
{
    public partial class IconSelectDialog : Form
    {
        public int selectedIndex;
        private Bitmap bmpIcon;

        // コンストラクタ
        public IconSelectDialog(Bitmap bmp, int iconW, int iconH)
        {
            InitializeComponent();
            bmpIcon = bmp;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            pictureBoxIconSelect.Size = new System.Drawing.Size(bmpIcon.Width, bmpIcon.Height);
            pictureBoxIconSelect.Image = bmpIcon;
            pictureBoxIcon.Size = new Size(iconW, iconH);
            pictureBoxIcon.Visible = false;
            Color col = Color.FromArgb(100, Color.Red);
            pictureBoxIcon.Image = MakeBitmap(col, pictureBoxIcon.Width, pictureBoxIcon.Height);
        }

        Bitmap MakeBitmap(Color color, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);
            //g.DrawEllipse(new Pen(Color.DarkGray), 3, 3, width - 6, height - 6);
            g.Dispose();

            return bmp;
        }

        void SetBitmap(int index)
        {
            int maxw, maxh, w, h;
            w = pictureBoxIcon.Width;
            h = pictureBoxIcon.Height;
            maxw = bmpIcon.Size.Width / w;
            maxh = bmpIcon.Size.Height / h;

            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(w, h);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);
            //画像ファイルのImageオブジェクトを作成する
            Bitmap img = bmpIcon;

            //切り取る部分の範囲を決定する。
            Rectangle srcRect = new Rectangle((index % maxw) * w, (index / maxw) * h, w, h);
            //描画する部分の範囲を決定する。
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            //画像の一部を描画する
            g.DrawImage(img, desRect, srcRect, GraphicsUnit.Pixel);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                    {
                        canvas.SetPixel(x, y, Color.Red);
                    }
                    Color c = canvas.GetPixel(x, y);
                    Color cc;
                    if (c.A == 0)
                    {
                        cc = Color.FromArgb(100, Color.Red);
                    }
                    else
                    {
                        cc = Color.FromArgb(
                            255,
                            ((int)c.R + 130) >= 254 ? byte.MaxValue : c.R + 130,
                            ((int)c.G - 20) < 0 ? 0 : c.G - 20,
                            ((int)c.B - 20) < 0 ? 0 : c.B - 20);
                    }
                    canvas.SetPixel(x, y, cc);
                }
            }

            // 画像を設定
            pictureBoxIcon.Image = canvas;

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
        }

        private void pictureBoxIconSelect_Click(object sender, EventArgs e)
        {
            Point pos = pictureBoxIconSelect.PointToClient(MousePosition);
            //Console.WriteLine("Mouse" + MousePosition.ToString());
            //Console.WriteLine("Pos" + pos.ToString());

            // 選択アイコンを求める
            int w, h, texW, texH, iconW, iconH, maxW, maxH;
            texW = bmpIcon.Size.Width;
            texH = bmpIcon.Size.Height;
            iconW = pictureBoxIcon.Width;
            iconH = pictureBoxIcon.Height;
            maxW = texW / iconW;
            maxH = texH / iconH;
            w = pos.X / iconW;
            h = pos.Y / iconH;

            // 範囲外判定
            if (w >= maxW || h >= maxH)
            {
                return;
            }

            selectedIndex = h * maxW + w;
            Console.WriteLine("selectedIndex " + selectedIndex);

            // ボタン位置
            SetBitmap(selectedIndex);
            pictureBoxIcon.Visible = true;
            pictureBoxIcon.Location = new Point(w * iconW, h * iconH);
        }

        private void pictureBoxIcon_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Dispose();
        }
    }
}
