using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class BattleEntity : GameEntity
{
    public int _damage = 1;

    protected virtual void Attack_target()
    {

    }
}

