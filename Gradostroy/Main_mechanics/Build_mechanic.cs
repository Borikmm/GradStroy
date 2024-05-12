using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradostroy.Main_mechanics
{
    public class Build_mechanic
    {

        public static Action ANomoney;

        // buildings list
        private Dictionary<string, Building> Buildings_list = new Dictionary<string, Building>();

        // Need parametres for building
        private string cursor_fillen; //type action selected in menu
        private string type_build_selected;

        // In window
        public static Action<int> Abuild_or_destroy;
        public static Func<int, bool> Acheck_balance;

        // In Building
        public static Action<int> AEarn_money;


    }
}
