using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Gradostroy.Main_mechanics
{
    public class Main_loop
    {

        private DispatcherTimer Main_loop_timer;

        // Action main loop
        public static Action AFixedUpdate;

        public void Start_loop()
        {
            Main_loop_setter();
            Main_loop_timer.Start();
        }

        private void Main_loop_setter()
        {
            // Main loop settings
            Main_loop_timer = new DispatcherTimer();
            Main_loop_timer.Interval = TimeSpan.FromSeconds(Convert.ToDouble(Service.Game_Settings["Main_loop_FPS"]) / 100);
            Main_loop_timer.Tick += FixedUpdate;
        }


        private void FixedUpdate(object sender, EventArgs e)
        {
            AFixedUpdate?.Invoke();
        }

    }
}
