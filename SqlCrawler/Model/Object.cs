using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCrawler.Model
{
    class Object
    {
        public string Definition { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string TypeDesc { get; set; }
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
        public IEnumerable<ObjectReference> ObjectReferences { get; set; }
    }
}
