using Gradostroy;
using Gradostroy.Main_mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

public class Bullet : RenderElement, IGameLoop
{
    public Ellipse BulletSprite;

    private RenderElement _target;

    private float speed = 5f;


    private int _damage = 5;


    private float CollisionRadius = 10f;

    public Bullet(RenderElement target, double x, double y)
    {
        CreateBullet(x, y);
        _target = target;

        MainLoopMech.AFixedUpdate += FixedUpdate;
    }

    ~Bullet()
    {
        MainLoopMech.AFixedUpdate -= FixedUpdate;
    }

    public override void UnSub()
    {
        MainLoopMech.AFixedUpdate -= FixedUpdate;
    }

    public void FixedUpdate()
    {
        GoToTarget();
    }

    private void GoToTarget()
    {
        if (_target.position.y == 0 && _target.position.x == 0)
        {
            
            return;
        }
        // Получаем координаты цели
        double targetX = _target.PivotPosition.x;
        double targetY = _target.PivotPosition.y;

        // Вычисляем разницу между текущими координатами и координатами цели
        double deltaX = targetX - PivotPosition.x;
        double deltaY = targetY - PivotPosition.y;

        // Вычисляем расстояние до цели
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // Проверка на столкновение с башней
        if (distance < CollisionRadius)
        {
            HandleCollision(); // Метод для обработки столкновения
            return;
        }

        // Нормализуем вектор направления
        double normalizedX = deltaX / distance;
        double normalizedY = deltaY / distance;

        // Обновляем позицию зомби с учетом направления
        position.x += normalizedX * speed;
        position.y += normalizedY * speed;

        UpdatePivot();

        // Устанавливаем новую позицию зомби
        Canvas.SetLeft(BulletSprite, position.x);
        Canvas.SetTop(BulletSprite, position.y);
    }

    private void HandleCollision()
    {

        if (_target is Enemy)
        {
            ((Enemy)_target).GetDamage(_damage);
        }

        // Delete from canvas
        Destroy(this);
    }

    private void CreateBullet(double x, double y)
    {
        position.x = x; position.y = y;
        CanvasRendered = new Canvas();
         
        Ellipse circle = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = Brushes.Blue // Цвет круга
        };

        Canvas.SetLeft(circle, x);
        Canvas.SetTop(circle, y);

        BulletSprite = circle;
        CanvasRendered.Children.Add(circle);
    }
}
