using System.Windows.Controls;

namespace Gradostroy
{

    public interface IGameLoop
    {
        void FixedUpdate();
    }

    public interface IBuild
    {
        Canvas Build(int x, int y);
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
