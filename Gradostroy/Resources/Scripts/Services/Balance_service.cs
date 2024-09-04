using Gradostroy;
using Gradostroy.Windows;
using System;

public class Balance_service
{
    private int _balance;

    // For balance block update in Blocks_service
    public static Action<int> AUpdate_balance { get; set; }

    public Balance_service()
    {
        MainGameWindow.Abuild_or_destroy += Change_Balance;
        MainGameWindow.Acheck_balance += Check_Balance;
        MainGameWindow.AEarn_money += Change_Balance;
    }

    ~Balance_service()
    {
        MainGameWindow.Abuild_or_destroy -= Change_Balance;
        MainGameWindow.Acheck_balance -= Check_Balance;
        MainGameWindow.AEarn_money -= Change_Balance;
    }

    public void Set_balance(int balance)
    {
        _balance = balance;
    }

    private bool Check_Balance(int cost)
    {
        if (_balance < cost)
            return true;
        else return false;
    }

    private void Change_Balance(int number)
    {
        _balance -= number;
        AUpdate_balance?.Invoke(_balance);
    }

}