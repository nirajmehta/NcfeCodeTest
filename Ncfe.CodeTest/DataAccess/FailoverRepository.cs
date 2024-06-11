using Ncfe.CodeTest.Interfaces;
using Ncfe.CodeTest.Models;
using System.Collections.Generic;

namespace Ncfe.CodeTest.DataAccess
{
    public class FailoverRepository : IFailoverRepository
    {
        public List<FailoverEntry> GetFailOverEntries()
        {
            // return all from fail entries from database
            return new List<FailoverEntry>();
        }
    }

}
