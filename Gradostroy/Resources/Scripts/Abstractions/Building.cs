using Gradostroy;
using System.Windows.Controls;

public abstract class Building : GameEntity
{

    public int Cost;
    public int Sell_Cost;
    public int top_margin = 50;


    public virtual void UnSub()
    {

    }

    public virtual void Start_fixed_update()
    {

    }
}
