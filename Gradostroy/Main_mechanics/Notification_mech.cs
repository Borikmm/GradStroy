using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Gradostroy.Main_mechanics
{
    internal class Notification_mech
    {

        private System.Timers.Timer _notification_lost_timer;
        private bool _notification_anim_start = false;
        private TextBlock _not_block;
        private object win;


        public Notification_mech(TextBlock notification_block, object win)
        {
            _not_block = notification_block;
            Build_mechanic.ANomoney += StartAnimation;
            this.win = win;
        }


        private void StartAnimation()
        {

            if (_notification_anim_start)
                return;

            int timer_notification_lost = 2000;


            _notification_lost_timer = new System.Timers.Timer(timer_notification_lost); // Устанавливаем интервал таймера в 2 секунды
            _notification_lost_timer.Elapsed += LostAnimation;

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(1);


            _not_block.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);

            _notification_lost_timer.Start();

            _notification_anim_start = true;
        }

        private void LostAnimation(object sender, ElapsedEventArgs e)
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
