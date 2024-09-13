
using System.Windows.Threading;
using System;

public abstract class BaseGameClass
{
    /// <summary>
    /// Метод для вызова другого метода через определенное количество секунд
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delayInSeconds"></param>
    protected void InvokeAfterDelay(Action action, int delayInSeconds)
    {
        DispatcherTimer _timer;
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(delayInSeconds);
        _timer.Tick += (sender, args) =>
        {
            _timer.Stop(); // Остановить таймер после первого срабатывания
            action(); // Вызов переданного метода
        };
        _timer.Start();
    }
}

