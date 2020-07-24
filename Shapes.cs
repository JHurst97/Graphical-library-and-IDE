using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    interface Shapes
    {
        void set(params int[] list);
        void draw(Graphics g);
        double calcArea();
        double calcPerimeter();
    }
}
