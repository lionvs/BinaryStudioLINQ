using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudioLINQ
{
    class Test
    {
        public string TestName { get; set; }
        public Category TestCategory { get; set; }
        public List<Question> Questions { get; set; }
        public TimeSpan MaxTime { get; set; }
        public int MarkNeeded { get; set; }
    }
}
