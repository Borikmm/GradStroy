using System.Windows.Controls;

namespace Gradostroy
{

    public interface IGameLoop
    {
        void FixedUpdate();
    }

    public interface IBuild
    {
        void Start_fixed_update();
    }


    public interface IRenderObject
    {
        Canvas Render(int x, int y);
    }


    public interface IDamageble
    {
        void GetDamage(int damage_value);
        void Destroy();
    }


    public interface IXPmanager: IDamageble
    {
        bool Check_XP();
        void Change_XP(int value);
    }
}
