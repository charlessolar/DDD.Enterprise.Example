using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Whats
{
    public class Context : IWhat
    {
        private IList<IWhere> _wheres;

        public Context()
        {
            _wheres = new List<IWhere>();
        }

        public String Description { get { return "Context"; } }
        public void AddWhere(IWhere where)
        {
            _wheres.Add(where);
        }

        public void Authorize()
        {
        }
    }
}