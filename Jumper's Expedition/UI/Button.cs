using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal class Button : Sprite
    {
        public String text;
        public bool being_pressed = false;
        Form1 client;
        Color color;

        float curr_add_width = 0;
        float target_add_width = 0;
        public Button(int x, int y, int width, int height, String text, Form1 client)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.text = text;

            this.client = client;

            this.color = Color.Black;
        }
        public override void Update()
        {
            if (this.mouse_inside(this.client))
            {
                this.target_add_width = 16;
                this.color = Color.Gray;

                if (this.client.mouseManager.clicked)
                {
                    this.being_pressed = true;
                }
                else
                {
                    this.being_pressed = false;
                }
            }
            else
            {
                this.target_add_width = 0;
                this.color = Color.Black;
            }
            if (this.curr_add_width != this.target_add_width)
            {
                this.curr_add_width = GameMath.lerp(this.curr_add_width, this.target_add_width, 0.5f);
            }
        }
        public override void Draw(Graphics g)
        {
            using (SolidBrush pen = new SolidBrush(Color.White))
            {
                g.FillRectangle(pen, ((this.x * 2) - 2) - curr_add_width, (this.y * 2) - 2, ((this.width * 2) + 4) + (curr_add_width * 2), (this.height * 2) + 4);
            }
            using (SolidBrush pen = new SolidBrush(this.color))
            {
                g.FillRectangle(pen, (this.x * 2) - curr_add_width, this.y * 2, (this.width * 2) + (curr_add_width * 2), this.height * 2);
            }
            using (SolidBrush pen = new SolidBrush(Color.White))
            {
                g.DrawString(this.text, new Font(this.client.font_collection.Families[0], 8), pen, (this.x * 2) + this.width, (this.y * 2) + this.height, client.sf);
            }
        }
    }
}