using Gradostroy;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class Cube : Building
{
    public Canvas Buiding;


    public Cube()
    {
        Name = "Cube";
        Cost = 10;
        Sell_Cost = -5;
    }

    public override void Start_fixed_update()
    {

    }


    public override Canvas Render(int x, int y)
    {
        y -= top_margin;

        Canvas canvas = new Canvas();

        Rectangle wall = new Rectangle
        {
            Width = 150,
            Height = 100,
            Fill = Brushes.LightBlue
        };

        Canvas.SetLeft(wall, x);
        Canvas.SetTop(wall, y);
        canvas.Children.Add(wall);
        Buiding = canvas;
        return canvas;

    }

    public override void FixedUpdate()
    {

    }

    public override void UnSub()
    {
        Service.main_timers.Mining_timer.action -= FixedUpdate;
    }
}