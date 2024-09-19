using Gradostroy.Main_mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradostroy.Resources.Scripts.GameObjects.Others
{
    public class GameManager : IGameLoop
    {


        public GameManager() 
        {
            MainLoopMech.AFixedUpdate += FixedUpdate;
        }

        public void FixedUpdate()
        {
            throw new NotImplementedException();
        }


        private void CheckLose()
        {

        }
    }
}
