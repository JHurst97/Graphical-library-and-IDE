using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASEProject
{
    class Method
    {
        public String contents, name;
        public String[] parameters;
        List<VarInt> methodInts = new List<VarInt>();

        /// <summary>Initializes a new instance of the <see cref="Method"/> class.</summary>
        /// <param name="name_">The name.</param>
        /// <param name="params_">The parameters.</param>
        /// <param name="contents_">The contents.</param>
        public Method(String name_, String[] params_, String contents_)
        {
            contents = contents_;
            parameters = params_;
            name = name_;

            foreach (string str in parameters)
            {
                VarInt newVar = new VarInt(str, 0);
                methodInts.Add(newVar);
            }
        }

        /// <summary>Gets the contents.</summary>
        /// <returns></returns>
        String getContents()
        {
            return contents;
        }

        /// <summary>Gets the name.</summary>
        /// <returns></returns>
        String getName()
        {
            return name;
        }

        /// <summary>Gets the parameters.</summary>
        /// <returns></returns>
        String[] getParameters()
        {
            return parameters;
        }

        /// <summary>Sets the contents.</summary>
        /// <param name="cont_">The cont.</param>
        void setContents(String cont_)
        {
            contents = cont_;
        }

        /// <summary>Sets the name.</summary>
        /// <param name="name_">The name.</param>
        void setName(String name_)
        {
            name = name_;
        }

        public void setParameters(String[] params_)
        {
            parameters = params_;
        }

        /// <summary>Replaces the parameters.</summary>
        /// <param name="newParams">The new parameters.</param>
        public void replaceParams(string[] newParams)
        {
            int count = 0;
            //compare each parameter to every element in the VarInt List
            foreach (VarInt obj in methodInts)
            {
                //replace the value in methodInts list
                int newInt = Int32.Parse(newParams[count]);
                methodInts[count].value = newInt;

                //replace the names with the values in contents
                if (contents.Contains(obj.name))
                {
                    string tempName = obj.name;
                    string tempVal = obj.value.ToString();
                    contents = contents.Replace(tempName, tempVal);

                    Console.WriteLine(contents);
                    // Console.WriteLine(contents);
                }
                count++;
            }
        }
    }
}
