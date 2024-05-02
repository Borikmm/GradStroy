using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Gradostroy
{

    public class Cube : Building
    {
        public Canvas Buiding;


        public Cube()
        {
            Name = "Cube";
            Cost = 10;
            Sell_Cost = -5;
        }

        public override void Start_fixed_update()
        {

        }


        public override Canvas Build(int x, int y)
        {
            y -= top_margin;

            Canvas canvas = new Canvas();

            Rectangle wall = new Rectangle
            {
                Width = 150,
                Height = 100,
                Fill = Brushes.LightBlue
            };

            Canvas.SetLeft(wall, x);
            Canvas.SetTop(wall, y);
            canvas.Children.Add(wall);
            Buiding = canvas;
            return canvas;

        }

        public override void FixedUpdate()
        {

        }

        public override void UnSub()
        {
            Service.main_timers.Mining_timer.action -= FixedUpdate;
        }
    }

    public class House : Building
    {
        public Canvas Buiding;
        private int _mining_money = -1;

        public House()
        {
            Name = "House";
            Cost = 20;
            Sell_Cost = -10;
        }

        ~House()
        {
            Service.main_timers.Mining_timer.action -= FixedUpdate;
        }

        public override void Start_fixed_update()
        {
            // Subscribe for mining timer
            Service.main_timers.Mining_timer.action += FixedUpdate;
        }

        public override void UnSub()
        {
            Service.main_timers.Mining_timer.action -= FixedUpdate;
        }


        public override Canvas Build(int x, int y)
        {
            y -= top_margin;


            Canvas canvas = new Canvas();
            Panel.SetZIndex(canvas, 10);

            // Рисуем стены дома
            Rectangle wall = new Rectangle
            {
                Width = 150,
                Height = 100,
                Fill = Brushes.LightBlue
            };
            Canvas.SetLeft(wall, x);
            Canvas.SetTop(wall, y);
            canvas.Children.Add(wall);

            // Рисуем крышу
            Polygon roof = new Polygon
            {
                Points = new PointCollection() { new Point(50, 100), new Point(125, 50), new Point(200, 100) },
                Fill = Brushes.Gray
            };
            Canvas.SetLeft(roof, x - 50);
            Canvas.SetTop(roof, y - 100);
            canvas.Children.Add(roof);

            // Рисуем дверь
            Rectangle door = new Rectangle
            {
                Width = 30,
                Height = 70,
                Fill = Brushes.Brown
            };
            Canvas.SetLeft(door, x);
            Canvas.SetTop(door, y);
            canvas.Children.Add(door);

            // Рисуем окна
            Rectangle window1 = new Rectangle
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.LightYellow
            };
            Canvas.SetLeft(window1, x);
            Canvas.SetTop(window1, y);
            canvas.Children.Add(window1);

            Rectangle window2 = new Rectangle
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.LightYellow
            };
            Canvas.SetLeft(window2, x);
            Canvas.SetTop(window2, y);
            canvas.Children.Add(window2);
            Buiding = canvas;
            return canvas;
        }

        public override void FixedUpdate()
        {
            Earn_money();
        }


        private void Earn_money()
        {
            MainWindow.AEarn_money?.Invoke(_mining_money);
        }
    }
}
