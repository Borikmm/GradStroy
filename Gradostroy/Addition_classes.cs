using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gradostroy
{
    public class Main_block : My_Text_Block
    {
        public Main_block(string Name, string Text, string Spliter, TextBlock link, bool reverse = false)
        {
            this.Name = Name;
            this.Text = Text;
            this.Spliter = Spliter;
            this.link = link;
            this.reverse_spliter = reverse;
        }
    }


    public class MyTimer
    {
        public DispatcherTimer main_timer;
        public Action action;
        public MyTimer(double tick)
        {
            main_timer = new DispatcherTimer();
            main_timer.Interval = TimeSpan.FromSeconds(tick);
            main_timer.Tick += Timer_function;
        }

        public void Start()
        {
            main_timer.Start();
        }


        private void Timer_function(object sender, EventArgs e)
        {
            action?.Invoke();
        }

    }

}
