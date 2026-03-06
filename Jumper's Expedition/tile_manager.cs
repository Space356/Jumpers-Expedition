using Jumper_s_Expedition.General_Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    /*[
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" ],
            [ "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1" ]
        ]*/
    public class tile_manager
    {
        Form1 client;
        RoomData room_data_getter;
        public List<List<String>> room_data;

        Bitmap chunk_1;
        Point chunk_1_position = new Point(0,0);
        Sprite player;
        public int level_count = 0;
 
        public tile_manager(Form1 client, Sprite player)
        {
            this.client = client;
            String file_data = File.ReadAllText(@"..\..\jsons\level1.json");
            room_data_getter = JsonSerializer.Deserialize<RoomData>(file_data);
            room_data = room_data_getter.data[2];

            level_count = room_data_getter.data.Count;

            chunk_1 = new Bitmap(384,216);

            this.player = player;

            Load_Chunk(chunk_1,chunk_1_position);
        }

        public void Draw(Graphics g)
        {
            Matrix m = new Matrix();
            m.Scale(2, 2);

            g.Transform = m;
            g.DrawImage(this.chunk_1, 0, 0);
            g.ResetTransform();

            m.Dispose();
        }

        public void next_room(int room_number)
        {
            room_data = room_data_getter.data[room_number];
            foreach (Sprite inst in this.client.sprites.Values)
            {
                if(inst.name.StartsWith("moving_platform"))
                {
                    inst.instance_destroy(this.client,inst.name);
                }
            }

            Load_Chunk(chunk_1, chunk_1_position);
        }

        public void Load_Chunk(Bitmap chunk, Point chunk_position)
        {
            Graphics chunk_1_graphics = Graphics.FromImage(chunk);
            chunk_1_graphics.Clear(Color.FromArgb(0));
            using (SolidBrush pen = new SolidBrush(Color.DarkBlue))
            {
                for (int i = chunk_position.Y; i < this.room_data.Count && i < 216/16; i++)
                {
                    for (int j = chunk_position.X; j < this.room_data[i].Count && j < 384/16; j++)
                    {
                        int parse_result = 0;
                        if (int.TryParse(this.room_data[i][j], out parse_result))
                        {
                            if (parse_result > 0)
                            {
                                chunk_1_graphics.FillRectangle(pen, j * 16, i * 16, 16, 16);
                            }
                        }
                        else
                        {
                            string tile_descriptor = this.room_data[i][j];
                            if (tile_descriptor == "sp")
                            {
                                player.x = j * 16;
                                player.y = i * 16;
                            }
                            else if (tile_descriptor == "en")
                            {
                                new Enemy(this.client, j * 16, i * 16);
                            }
                            else if (tile_descriptor == "co")
                            {
                                new Coin(this.client, j * 16, i * 16);
                            }
                            else if(tile_descriptor.StartsWith("v"))
                            {
                                int size_parse = 0;
                                int.TryParse(tile_descriptor.Substring(1, 1), out size_parse);
                                new FloatingPlatform(this.client, j * 16, i * 16, 0, 2, this, size_parse);
                            }
                            else if (tile_descriptor.StartsWith("h"))
                            {
                                int size_parse = 0;
                                int.TryParse(tile_descriptor.Substring(1, 1), out size_parse);
                                new FloatingPlatform(this.client, j * 16, i * 16, 2, 0, this, size_parse);
                            }
                        }
                    }
                }
            }
        }
    }
    public class RoomData
    {
        public List<List<List<String>>> data { get; set; }
    }
}