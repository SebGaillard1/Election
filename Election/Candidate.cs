using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Election
{
    public class Candidate
    {
        public string Name { get; private set; }

        public Candidate(string name)
        {
            Name = name;
        }
    }
}
