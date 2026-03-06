using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    public class MainMenu
    {
        Form1 client;
        GlobalKeyManager keyManager;
        Button b_start;
        Button b_difficulty;
        Button b_quit;
        String game_name = "GAME NAME";
        Font game_font;
        public enum Difficulties
        {
            Easy,
            Normal,
            Hard,
            Impossible
        }

        public Difficulties current_difficulty;

        float difficulty_scaler = 1.02f;

        //put necessary variables here.

        public MainMenu(Form1 client, GlobalKeyManager kmg)
        {
            this.game_font = new Font("Impact", 32);

            this.client = client;
            this.keyManager = kmg;
            b_start = new Button(384 / 2 - 32, 216 / 2 - 8, 64, 16, "START", this.client);
            b_difficulty = new Button(384 / 2 - 32, 216 / 2 + 20, 64, 16, "NORMAL", this.client);
            b_quit = new Button(384 / 2 - 32, 216 / 2 + 48, 64, 16, "QUIT", this.client);

            current_difficulty = Difficulties.Normal;
        }

        public void Update()
        {
            b_start.Update();
            b_difficulty.Update();
            b_quit.Update();
            if (this.b_start.being_pressed)
            {
                this.b_start.being_pressed = false;
                client.start_game(difficulty_scaler, this.current_difficulty);
            }
            else if (this.b_difficulty.being_pressed)
            {
                switch (this.current_difficulty)
                {
                    case Difficulties.Impossible:
                        this.difficulty_scaler = 0.8f;
                        this.current_difficulty = Difficulties.Easy;
                        break;
                    case Difficulties.Easy:
                        this.difficulty_scaler = 1.2f;
                        this.current_difficulty = Difficulties.Normal;
                        break;
                    case Difficulties.Normal:
                        this.difficulty_scaler = 2f;
                        this.current_difficulty = Difficulties.Hard;
                        break;
                    case Difficulties.Hard:
                        this.difficulty_scaler = 4f;
                        this.current_difficulty = Difficulties.Impossible;
                        break;
                }
                this.b_difficulty.text = current_difficulty.ToString().ToUpper();
            }
            else if (this.b_quit.being_pressed)
            {
                this.client.Close();
            }
        }

        public void draw(Graphics g)
        {
            b_start.Draw(g);
            b_difficulty.Draw(g);
            b_quit.Draw(g);

            float floating_effect = ((float)Math.Sin(this.client.curr_frame * 0.02f) * 16);

            using (SolidBrush pen = new SolidBrush(Color.DarkBlue))
            {
                //I could have probably done this title a better way, but I didn't.

                g.DrawString(game_name, game_font, pen, 384 + 5, ((216 + 5) - 110) + floating_effect, client.sf);
                pen.Color = ColorTranslator.FromHtml("#000088");
                g.DrawString(game_name, game_font, pen, 384 + 4f, ((216 + 4f) - 110) + floating_effect, client.sf);
                pen.Color = ColorTranslator.FromHtml("#1010AA");
                g.DrawString(game_name, game_font, pen, 384 + 3f, ((216 + 3f) - 110) + floating_effect, client.sf);
                pen.Color = ColorTranslator.FromHtml("#2020DD");
                g.DrawString(game_name, game_font, pen, 384 + 2f, ((216 + 2f) - 110) + floating_effect, client.sf);
                pen.Color = ColorTranslator.FromHtml("#4040FF");
                g.DrawString(game_name, game_font, pen, 384 + 1f, ((216 + 1f) - 110) + floating_effect, client.sf);
            }
            using (SolidBrush pen = new SolidBrush(Color.CornflowerBlue))
            {
                g.DrawString(game_name, game_font, pen, 384, (216 - 110) + floating_effect, client.sf);
            }
        }
    }
}