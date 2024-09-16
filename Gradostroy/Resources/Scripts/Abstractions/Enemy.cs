using Gradostroy;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


public abstract class Enemy : BattleEntity
{
    public int GoldEarned = Service.Random.Next(0, 5);

    public Building _target;
    public float speed;
    public double CollisionRadius = 10.0; // Радиус столкновения


}
