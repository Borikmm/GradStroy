
using System.Windows.Threading;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Gradostroy.Windows;

public abstract class BaseGameClass
{


    public string Name;
    public string BaseName;
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


    public void Destroy(RenderElement obj)
    {
        MainGameWindow.MainGrid.Children.Remove(obj.CanvasRendered);
        obj.UnSub();
    }
}

