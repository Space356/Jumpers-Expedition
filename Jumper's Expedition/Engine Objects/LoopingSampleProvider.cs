using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal class LoopingSampleProvider : ISampleProvider
    {
        private AudioFileReader file;

        public LoopingSampleProvider(AudioFileReader file)
        {
            this.file = file;
        }

        public WaveFormat WaveFormat => file.WaveFormat;

        //This required a bit of research and a few hours to figure out.
        public int Read(float[] buffer, int offset, int count)
        {
            int read_lines = 0;
            while (read_lines < count)
            {
                //This basically detects when the reader in the file reaches the last line, and sets the read position to 0, creating a loop.
                int read = file.Read(buffer, offset + read_lines, count - read_lines);
                if (read == 0)
                {
                    file.Position = 0;
                    continue;
                }
                read_lines += read;
            }
            return read_lines;
        }
    }
}
