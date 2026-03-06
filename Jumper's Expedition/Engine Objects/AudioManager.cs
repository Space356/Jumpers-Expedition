using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    public class AudioManager
    {
        WaveOutEvent sound_out;
        Form1 client;
        public MixingSampleProvider mixer;
        public AudioManager(Form1 client)
        {
            this.client = client;
            this.sound_out = new WaveOutEvent();
            this.sound_out.DesiredLatency = 50;
            this.mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)) { ReadFully = true };
            this.sound_out.Init(mixer);
            this.sound_out.Play();
        }

        public void play_sound(string path, bool loop = false)
        {
            AudioFileReader file = new AudioFileReader(path);
            ISampleProvider sound;

            if (loop)
            {
                sound = new LoopingSampleProvider(file);
            }
            else
            {
                sound = file.ToSampleProvider();
            }

            this.mixer.AddMixerInput(sound);
        }
    }
}
