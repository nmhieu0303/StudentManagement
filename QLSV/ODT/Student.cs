using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV
{
    public class Student
    {
        public string id { get; set; }
        public string name { get; set; }
        public string born { get; set; }
        public DateTime birth { get; set; }
        public float score { get; set; }


        public string ToString()
        {
           return id + ";" + name + ";" + birth + ";" + born + ";" + score;
        }
    }
}
