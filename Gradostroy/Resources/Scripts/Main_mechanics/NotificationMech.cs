using System;
using System.Security.Policy;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;


namespace Gradostroy.Main_mechanics
{
    internal class NotificationMech
    {

        private System.Timers.Timer _notification_lost_timer;
        private bool _notification_anim_start = false;

        private System.Timers.Timer _notificationDayTimer;
        private bool _notificationDay = false;


        private TextBlock _not_block;
        private TextBlock _dayCycleNot;


        private object win;



        public NotificationMech(TextBlock notification_block, TextBlock dayCycleNot, object win)
        {
            _not_block = notification_block;
            _dayCycleNot = dayCycleNot;


            Build_mechanic.ANomoney += StartNotMoneyAnimation;

            ActionsService.ActionStartSpawn += StartNightAnimation;




            this.win = win;
        }



        // Need write universal Animation class
        private void StartNightAnimation()
        {
            if (_notificationDay)
                return;

            _dayCycleNot.Text = $"Night: {Day_cycle_service.NumberNight}";


            int timer_notification_lost = 2000;


            _notificationDayTimer = new System.Timers.Timer(timer_notification_lost); // Устанавливаем интервал таймера в 2 секунды
            _notificationDayTimer.Elapsed += EndNightAnimation;

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(1);


            _dayCycleNot.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);

            _notificationDayTimer.Start();

            _notificationDay = true;
        }

        private void EndNightAnimation(object sender, ElapsedEventArgs e)
        {
            _notificationDayTimer.Stop();
            _notificationDay = false;

            ((Window)win).Dispatcher.Invoke(() =>
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation();
                fadeOutAnimation.From = 1;
                fadeOutAnimation.To = 0;
                fadeOutAnimation.Duration = TimeSpan.FromSeconds(1);

                _dayCycleNot.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            });
        }


        private void StartNotMoneyAnimation()
        {

            if (_notification_anim_start)
                return;

            int timer_notification_lost = 2000;


            Point currentPosition = Mouse.GetPosition(Application.Current.MainWindow);



            double x = currentPosition.X - 46;
            double y = currentPosition.Y - 50;


            _notification_lost_timer = new System.Timers.Timer(timer_notification_lost); // Устанавливаем интервал таймера в 2 секунды
            _notification_lost_timer.Elapsed += LostNotMoneyAnimation;

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(1);


            _not_block.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);

            Canvas.SetLeft(_not_block, x);
            Canvas.SetTop(_not_block, y);

            _not_block.Visibility = Visibility.Visible;

            _notification_lost_timer.Start();

            _notification_anim_start = true;
        }

        private void LostNotMoneyAnimation(object sender, ElapsedEventArgs e)
        {
            _notification_lost_timer.Stop();
            _notification_anim_start = false;

            ((Window)win).Dispatcher.Invoke(() =>
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation();
                fadeOutAnimation.From = 1;
                fadeOutAnimation.To = 0;
                fadeOutAnimation.Duration = TimeSpan.FromSeconds(1);

                _not_block.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            });
        }
    }
}
