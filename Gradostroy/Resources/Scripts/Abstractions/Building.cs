using Gradostroy;
using System.Windows.Controls;

public abstract class Building : IBuild, IXPmanager, IGameLoop, IRenderObject
{
    public string Name;
    public int Cost;
    public int Sell_Cost;
    public int _XP = 5;
    public int top_margin = 50;

    public virtual Canvas Render(int x, int y)
    {
        return null;
    }

    public bool Check_XP()
    {
        if (_XP <= 0)
        {
            return false;
        }
        return true;
    }

    public virtual void FixedUpdate()
    {

    }

    public void GetDamage(int damage_value)
    {
        Change_XP(damage_value);
    }

    public virtual void UnSub()
    {

    }

    public virtual void Change_XP(int damage_value)
    {
        _XP -= damage_value;
    }

    public virtual void Destroy()
    {

    }

    public virtual void Start_fixed_update()
    {

    }
}
