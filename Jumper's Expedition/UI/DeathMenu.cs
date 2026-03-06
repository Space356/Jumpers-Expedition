using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jumper_s_Expedition
{
    public class DeathMenu
    {
        Form1 client;
        GlobalKeyManager keyManager;
        Button b_difficulty;
        Button b_quit;
        String game_name = "YOU DIED!!!";
        Font game_font;

        //float difficulty_scaler = 1.02f;

        //put necessary variables here.

        public DeathMenu(Form1 client, GlobalKeyManager kmg)
        {
            this.game_font = new Font("Impact", 32);

            this.client = client;
            this.keyManager = kmg;
            b_difficulty = new Button(384 / 2 - 32, 216 / 2 + 20, 64, 16, "RESTART", this.client);
            b_quit = new Button(384 / 2 - 32, 216 / 2 + 48, 64, 16, "MAIN MENU", this.client);
        }

        public void Update()
        {
            if (this.client.player.health <= 0)
            {
                if(!this.client.paused)
                {
                    this.client.paused = true;
                }
                
                b_difficulty.Update();
                b_quit.Update();
                if (this.b_difficulty.being_pressed)
                {
                    this.client.end_game();
                    this.client.start_game(client.difficulty_scaler,MainMenu.Difficulties.Normal);
                    client.paused = false;
                }
                else if (this.b_quit.being_pressed)
                {
                    this.client.end_game();
                    client.paused = false;
                }
            }
            else
            {
                this.b_quit.being_pressed = false;
                this.b_difficulty.being_pressed = false;
            }

        }

        public void draw(Graphics g)
        {
            if (this.client.player.health <= 0)
            {
                b_difficulty.Draw(g);
                b_quit.Draw(g);

                float floating_effect = ((float)Math.Sin(this.client.curr_frame * 0.02f) * 16);

                using (SolidBrush pen = new SolidBrush(Color.White))
                {
                    g.DrawString(game_name, game_font, pen, 384, (216 - 110) + floating_effect, client.sf);
                }
            }
        }
    }
}