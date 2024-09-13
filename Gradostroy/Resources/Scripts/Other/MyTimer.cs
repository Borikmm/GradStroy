using System.Windows.Threading;
using System;

public class MyTimer
{
    public DispatcherTimer main_timer;
    public Action action;
    public MyTimer(double tick)
    {
        main_timer = new DispatcherTimer();
        main_timer.Interval = TimeSpan.FromSeconds(tick);
        main_timer.Tick += Timer_function;
    }

    public void Start()
    {
        main_timer.Start();
    }


    public void Stop()
    {
        main_timer.Stop();
    }


    private void Timer_function(object sender, EventArgs e)
    {
        action?.Invoke();
    }

}