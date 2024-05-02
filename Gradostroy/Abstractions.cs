using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gradostroy
{
    public abstract class Building : IBuild, IXPmanager, IGameLoop
    {
        public string Name;
        public int Cost;
        public int Sell_Cost;
        public int _XP = 5;
        public int top_margin = 50;

        public virtual Canvas Build(int x, int y)
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


    public abstract class Enemy : IXPmanager, IGameLoop
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
    }


    public abstract class My_Text_Block
    {
        public string Spliter;
        public string Name;
        public string Text;
        public TextBlock link;
        public bool reverse_spliter;
    }
}
