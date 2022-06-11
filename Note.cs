using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeFleche
{
    class Note
    {
        /*********************************************************\
         *                 Private Attributes                    *
        \*********************************************************/

        private int id;
        private int idMatiere;
        private double noteBrute;
        private int coefficient;


        /*********************************************************\
         *                      Get/Set                          *
        \*********************************************************/

        public int Id { get; set; }
        public double IdMatiere { get; set; }
        public double NoteBrute { get; set; }
        public int Coefficient { get; set; }
    }
}
