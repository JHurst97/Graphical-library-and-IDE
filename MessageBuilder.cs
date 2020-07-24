using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    class MessageBuilder
    {
        string message = "";

        void addToStr(string params_)
        {
            message += params_;
        }

        string getFullStr()
        {
            return message;
        }
    }
}
