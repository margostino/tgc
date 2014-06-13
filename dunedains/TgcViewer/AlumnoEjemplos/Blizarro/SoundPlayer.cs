using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;

namespace AlumnoEjemplos.Blizarro
{
    class SoundPlayer
    {
        private Device device;
        private System.Collections.Generic.Dictionary<string, Buffer> array_buffer;

        public SoundPlayer(Form owner)
        {
            device = new Device();
            device.SetCooperativeLevel(owner, CooperativeLevel.Normal);

            array_buffer = new Dictionary<string, Buffer>();
        }

        public void AddSound(string name, string location)
        {
            Buffer buffer = new SecondaryBuffer(location, device);
            array_buffer.Add(name, buffer);
        }

        public void Play(string name)
        {
            array_buffer[name].Play(0, BufferPlayFlags.Default);
        }

        public void PlayLoop(string name)
        {
            if (!array_buffer[name].Status.Playing)
            {
                array_buffer[name].Play(0, BufferPlayFlags.Looping);
            }
        }

        public void StopLoop(string name)
        {
            array_buffer[name].Stop();
        }

    }
}
