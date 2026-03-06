using Jumper_s_Expedition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jumper_s_Expedition
{
    public partial class Form1 : Form
    {
        Bitmap frame_buffer;
        Graphics g;

        Timer timer;
        public GlobalKeyManager keyManager = new GlobalKeyManager();
        public MouseManager mouseManager = new MouseManager();
        public int points = 10;

        public MainMenu menu;
        public PauseMenu pause_menu;
        public DeathMenu death_menu;

        public bool ingame = false;

        public StringFormat sf;

        public CollisionGroups colliders = new CollisionGroups();

        public Dictionary<string, Sprite> sprites = new Dictionary<string ,Sprite>();

        public Random rand = new Random();

        public PrivateFontCollection font_collection = new PrivateFontCollection();

        public bool paused = false;

        public float difficulty_scaler = 1.2f;

        public int curr_frame = 0;

        public tile_manager tiles;

        public int curr_object_index = 0;

        List<Keys> exclude_repeating_keys = new List<Keys>();

        public Player player;

        public List<string> instance_destroy_queue = new List<string>();
        public Form1()
        {
            //sprite  = Image.FromFile("sprites\\c0c0.png");
            InitializeComponent();
            this.BackColor = Color.SkyBlue;
            this.DoubleBuffered = true;
            this.Visible = true;
            this.Width = 1280;
            this.Height = 720;

            this.frame_buffer = new Bitmap(384 * 2, 216 * 2);
            this.g = Graphics.FromImage(this.frame_buffer);

            font_collection.AddFontFile(@"..\..\fonts\highquality-pixel-font.ttf");

            //for centering the font
            sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //starts the game loop in 60 fps.
            timer = new Timer();
            timer.Interval = 1000 / 60;
            timer.Tick += new EventHandler(step);
            timer.Start();

            //makes the main menu
            this.menu = new MainMenu(this, this.keyManager);
            this.pause_menu = new PauseMenu(this, this.keyManager);
            this.death_menu = new DeathMenu(this, this.keyManager);
        }

        public void start_game(float difficulty_scaler, MainMenu.Difficulties difficulty)
        {
            //List<Sprite> collisions = new List<Sprite>(); Just in case.

            this.ingame = true;
            this.sprites.Clear();
            //this.sprites.Add(new enemy(this, player));
            this.paused = false;
            this.difficulty_scaler = difficulty_scaler;

            player = new Player(this); //this better work

            this.tiles = new tile_manager(this, this.sprites["obj_player"]);
        }

        public void step(Object myObject, EventArgs myEventArgs)
        {
            Invalidate();
            Application.DoEvents();

            this.curr_frame++;

            foreach (string inst in instance_destroy_queue)
            {
                sprites.Remove(inst);
            }
            instance_destroy_queue.Clear();

            if (ingame)
            {
                int coin_count = 0;
                foreach (Sprite inst in this.sprites.Values)
                {
                    inst.Update();
                    if(inst.name.StartsWith("coin"))
                    {
                        coin_count++;
                    }
                }

                //checks if all coins are collected
                if(coin_count <= 0)
                {
                    foreach (Sprite inst in this.sprites.Values)
                    {
                        if(inst.name.StartsWith("enemy"))
                        {
                            inst.instance_destroy(this,inst.name);
                        }
                    }
                    this.tiles.next_room(rand.Next(this.tiles.level_count));
                }

                if (this.keyManager.possible_keys[4].pressed_down && player.health > 0)
                {
                    this.paused = true;
                    //this.player.health = 20;
                }

                pause_menu.Update();
                death_menu.Update();
            }
            else
            {
                menu.Update();
            }

            this.mouseManager.Update();
            this.keyManager.Update();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics Form_g = e.Graphics;

            g.Clear(this.BackColor);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            if (ingame)
            {
                tiles.Draw(g);

                foreach (Sprite inst in this.sprites.Values)
                {
                    inst.update_constraint();
                    inst.Draw(g);
                }

                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;

                //Change this for a points display.
                /*using (SolidBrush pen = new SolidBrush(Color.Gray))
                {
                    g.DrawString("HEALTH " + inst_player.health.ToString(), new Font(font_collection.Families[0], 15), pen, 16, 16 + 4, sf);
                    g.DrawString("COINS " + inst_player.currency.ToString(), new Font(font_collection.Families[0], 15), pen, 16, 52 + 4, sf);
                }*/
                using (SolidBrush pen = new SolidBrush(Color.White))
                {
                    g.DrawString("HEALTH " + player.health.ToString(), new Font(font_collection.Families[0], 15), pen, 16+4, 16, sf);
                    g.DrawString("COINS " + player.currency.ToString(), new Font(font_collection.Families[0], 15), pen, 16+4, 52, sf);
                }
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                pause_menu.draw(g);
                death_menu.draw(g);
            }
            else
            {
                menu.draw(g);
            }
            Form_g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            float scaler = this.ClientSize.Width / 384f;
            Form_g.ScaleTransform(scaler / 2, scaler / 2);
            Form_g.DrawImage(frame_buffer, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ignore this
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(!exclude_repeating_keys.Contains(e.KeyCode))
            {
                exclude_repeating_keys.Add(e.KeyCode);
                foreach (var key in keyManager.possible_keys)
                {
                    if (e.KeyData == key.key)
                    {
                        key.init_pressed_down = true;
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            exclude_repeating_keys.Remove(e.KeyCode);
            foreach (var key in keyManager.possible_keys)
            {
                if (e.KeyData == key.key)
                {
                    key.init_pressed_up = true;
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseManager.is_down = true;
            this.mouseManager.clicked = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseManager.is_down = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            float scaler = this.ClientSize.Width / 384f;
            this.mouseManager.x = (int)(e.X / scaler);
            this.mouseManager.y = (int)(e.Y / scaler);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        public void end_game()
        {
            //menu.Update();
            ingame = false;

            this.points = 0;
            this.sprites.Clear();
            this.colliders.collisions_damage.Clear();
            this.colliders.collisions_player_damage.Clear();
            this.colliders.collisions_movers.Clear();
            this.tiles = null;
            this.paused = false;
            this.player = null;
        }
    }
    public class MouseManager
    {
        public int x;
        public int y;
        public bool is_down;
        public bool clicked;
        public MouseManager()
        {

        }

        public void Update()
        {
            if (this.clicked)
            {
                this.clicked = false;
            }
        }
    }
}