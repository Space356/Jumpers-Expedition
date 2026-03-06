using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    public class Sprite
    {
        public float x = 0;
        public float y = 0;
        public int width = 32;
        public int height = 32;
        public int x_offset = 0;
        public int y_offset = 0;

        public float direction = 0;
        public float speed = 0;
        public float x_speed = 0;
        public float y_speed = 0;

        public float image_angle = 360;
        public float image_alpha = 1;

        public string name = "obj";

        Form1 client;
        Image sprite = null;

        public List<Sprite> collider_list = null;

        public Sprite constraint = null;
        public float save_constraint_x = 0;
        public float save_constraint_y = 0;

        public bool persistant = false;

        public Sprite(float x, float y, int width, int height, Form1 client)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.client = client;

            //right now, this just works as a way to manage collisions.
        }
        public Sprite(String filepath, float x, float y, int width, int height, Form1 client)
        {
            this.x = x;
            this.y = y;
            this.client = client;

            //path format: @"..\..\sprites\jesasus.png"
            set_sprite(filepath, width, height);
            //right now, this just works as a way to manage collisions.
        }
        public Sprite()
        {
            //To get errors to stop.
        }

        public bool mouse_inside(Form1 client)
        {
            return (client.mouseManager.x > this.x && client.mouseManager.y > this.y &&
                client.mouseManager.x < this.x + this.width && client.mouseManager.y < this.y + this.height);
        }

        public void draw_self(Graphics g)
        {
            ColorMatrix color_matrix = new ColorMatrix(new float[][]
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, image_alpha, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(color_matrix);

            Matrix m = new Matrix();

            m.Translate(x * 2 + this.width, y * 2 + this.height);
            m.Rotate(this.image_angle);
            m.Translate(-this.width, -this.height);
            m.Scale(2, 2);

            g.Transform = m;
            g.DrawImage(this.sprite, new Rectangle(x_offset, y_offset, sprite.Width, sprite.Height),0,0,sprite.Width,sprite.Height,GraphicsUnit.Pixel,attributes); //This does not need so many parameters for image alpha.
            g.ResetTransform();

            m.Dispose();
        }

        public void set_sprite(string file_path, int width, int height)
        {
            this.sprite = Image.FromFile(file_path);
            this.width = width;
            this.height = height;
        }

        public void update_direction()
        {
            this.x_speed = (float)Math.Cos(direction) * speed;
            this.y_speed = (float)Math.Sin(direction) * speed;

            this.x += x_speed;
            this.y += y_speed;
        }

        public void update_constraint()
        {
            if(constraint != null)
            {
                float relative_x = constraint.x - save_constraint_x;
                float relative_y = constraint.y - save_constraint_y;
                x += relative_x;
                y += relative_y;
                save_constraint_x = constraint.x;
                save_constraint_y = constraint.y;
            }
        }
        public virtual void Update() { }

        public virtual void Draw(Graphics g)
        {
            draw_self(g);
        }

        public void sprite_init_index(Form1 client, string name, bool use_index = true)
        {
            /// <summary>
            /// Initializes the object within the client's sprite dictionary.
            /// When use index is false, no specific id numbers would be siffixed to the name, which helps when referencing an object that only appears once (I.E. a player object.).
            /// </summary>
            string suffix = use_index ? $"_{client.curr_object_index}" : ""; //don't mind my fancy code.
            client.sprites[name + suffix] = this;
            this.name = name + suffix;
            //Debug.WriteLine(name + suffix);
            client.curr_object_index++;
        }

        public void instance_destroy(Form1 client, string id)
        {
            // <summary>Null means this object. Use the indexing id as stated in sprite_init_index.</summary>
            if(client.sprites[id].collider_list != null)
            {
                client.sprites[id].collider_list.Remove(client.sprites[id]);
            }
            client.instance_destroy_queue.Add(id);
        }

        public void set_constraint(Form1 client, Sprite obj)
        {
            constraint = obj;
            save_constraint_x = obj.x;
            save_constraint_y = obj.y;
        }
    }
}