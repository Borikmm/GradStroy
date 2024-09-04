using Gradostroy;
using System;
using System.Windows.Controls;


public abstract class Enemy : IXPmanager, IGameLoop, IRenderObject
{
    public int _XP = 10;
    public int _damage = 1;
    public object _target;

    public void GetDamage(int damage_value)
    {
        Change_XP(damage_value);
    }

    public bool Check_XP()
    {
        if (_XP <= 0)
        {
            return false;
        }
        return true;
    }

    public void Change_XP(int damage_value)
    {
        _XP -= damage_value;
    }

    public virtual void FixedUpdate()
    {

    }

    protected virtual void Attack_target()
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
}
