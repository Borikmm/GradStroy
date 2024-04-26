using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
