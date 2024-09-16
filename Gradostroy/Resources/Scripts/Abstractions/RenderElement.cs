using Gradostroy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


public abstract class RenderElement : BaseGameClass
{
    public position position;
    public Image sprite;
    public Canvas CanvasRendered;


    public position PivotPosition;


    private int width;
    private int height;


    protected void UpdatePivot()
    {
        PivotPosition.x = position.x + width / 2;
        PivotPosition.y = position.y + height / 2;
    }


    public virtual Canvas Render(int x, int y, int width = 100, int height = 100, string Image_path = null)
    {
        position.x = x; position.y = y;


        this.width = width;
        this.height = height;

        PivotPosition.x = x + width / 2; 
        PivotPosition.y = y + height / 2;


        Canvas canvas = new Canvas();

        Image image = new Image
        {
            Width = width,
            Height = height,
            Source = new BitmapImage(new Uri(Image_path, UriKind.Relative)),
        };

        sprite = image;
        Canvas.SetLeft(image, x);
        Canvas.SetTop(image, y);

        canvas.Children.Add(image);
        CanvasRendered = canvas;

        return canvas;
    }


    public virtual void UnSub()
    {

    }

}

