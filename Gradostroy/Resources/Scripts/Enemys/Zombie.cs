using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

public class Zombie : Enemy
{


    public Canvas CanvasRendered;

    public Zombie(int XP = -1, int Damage = -1, object Target = null)
    {
        _damage = Damage != -1 ? Damage : _damage;
        _XP = XP != -1 ? XP : _XP;
        _target = Target;
    }

    ~Zombie()
    {

    }


    public override Canvas Render(int x, int y)
    {
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
        CanvasRendered = canvas;
        return canvas;

    }

    public override void FixedUpdate()
    {

    }
}
