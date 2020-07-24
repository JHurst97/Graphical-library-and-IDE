using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    class ShapeFactory
    {
        string input;
        /// <summary>Gets the shape.</summary>
        /// <param name="input_">The input.</param>
        /// <returns></returns>
        public Shape getShape(string input_)
        {
            input = input_;
            if (input_.Equals("circle"))
            {
                return new Circle();
            }

            else if (input_.Equals("square"))
            {
                return new Square();
            }
            else if (input_.Equals("rect"))
            {
                return new Rectangle();
            }

            else
            {

                return null;
            }
        }

    }
}
