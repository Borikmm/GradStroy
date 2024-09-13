using System;
using System.Windows.Threading;

namespace Gradostroy.Main_mechanics
{
    public class MainLoopMech
    {

        private DispatcherTimer MainLoopMech_timer;

        // Action main loop
        public static Action AFixedUpdate;

        public void Start_loop()
        {
            MainLoopMech_setter();
            MainLoopMech_timer.Start();
        }

        private void MainLoopMech_setter()
        {
            // Main loop settings
            MainLoopMech_timer = new DispatcherTimer();
            MainLoopMech_timer.Interval = TimeSpan.FromSeconds(1 / Convert.ToDouble(Service.Game_Settings["MainLoopMech_FPS"]));
            MainLoopMech_timer.Tick += FixedUpdate;
        }


        private void FixedUpdate(object sender, EventArgs e)
        {
            AFixedUpdate?.Invoke();
        }

    }
}
