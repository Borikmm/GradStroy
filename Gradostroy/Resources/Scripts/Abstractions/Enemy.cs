using Gradostroy;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


public abstract class Enemy : BattleEntity
{

    public Building _target;
    public Image Image;
    public float speed;
    public double CollisionRadius = 10.0; // Радиус столкновения


}
