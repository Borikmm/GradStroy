using Gradostroy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using Gradostroy.Windows;


public abstract class GameEntity : RenderElement, IXPmanager, IGameLoop, IRenderObject
{
    public Canvas CanvasRendered;
    public string Name;
    public int _XP = 10;


    public void GetDamage(int damage_value)
    {
        Change_XP(damage_value);
        ChangeColor(MyColors.Red);
        //InvokeAfterDelay(() => ChangeColor(MyColors.White), 2);
    }


    protected virtual void ChangeColor(MyColors color)
    {
        if (MainGameWindow.Buildings_list.TryGetValue(Name, out Building build))
        {
            build.CanvasRendered.Background = new SolidColorBrush(Service.MyColorsList[color]);
        }
        //Console.WriteLine("stop");
    }

    public bool Check_XP()
    {
        if (_XP <= 0)
        {
            return false;
        }
        return true;
    }

    private void Change_XP(int damage_value)
    {
        _XP -= damage_value;
    }

    public virtual void FixedUpdate()
    {

    }



    public void Destroy()
    {
        throw new NotImplementedException();
    }

    public virtual Canvas Render(int x, int y)
    {
        return null;
    }

    void IXPmanager.Change_XP(int value)
    {
        throw new NotImplementedException();
    }
}

