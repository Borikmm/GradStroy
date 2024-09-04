using System.Windows.Controls;

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