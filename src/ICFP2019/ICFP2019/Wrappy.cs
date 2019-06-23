using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFP2019
{
    partial class Wrappy
    {
        public List<Action> executeds = new List<Action>();
        public int remainingFastWheel = 0;
        public int remainingDrill = 0;

        public Action nextAction() {

            throw new Exception();
        }

    }
}
