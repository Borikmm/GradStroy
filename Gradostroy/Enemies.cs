using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradostroy
{
    public class Zombie: Enemy
    {
        public Zombie(int XP = -1, int Damage = -1, object Target = null) 
        {
            _damage= Damage != -1 ? Damage : _damage;
            _XP = XP != -1 ? XP : _XP;
            _target = Target;
        }

        ~Zombie()
        {

        }

        public override void FixedUpdate()
        {
            
        }
    }
}
