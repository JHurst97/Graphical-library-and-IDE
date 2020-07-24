using System.Drawing;

namespace ASEProject
{
    internal class Circle : Shape
    {

        int width, height;
        public Circle() : base()
        {
            width = 100;
            height = 100;
        }
        public Circle(int x, int y, int width, int height) : base(x, y)
        {

            this.width = width; 
            this.height = height;
        }

        public override void set(params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is width, list[3] is height
            base.set(list[0], list[1]);
            this.width = list[2];
            this.height = list[3];

        }

        public override void draw(Graphics g)
        {
            Pen p = new Pen(Color.Black, 2);
            SolidBrush b = new SolidBrush(colour);
            g.DrawEllipse(p, x, y, width, height);
        }

        public override double calcArea()
        {
            return width * height;
        }

        public override double calcPerimeter()
        {
            return 2 * width + 2 * height;
        }

    }
}