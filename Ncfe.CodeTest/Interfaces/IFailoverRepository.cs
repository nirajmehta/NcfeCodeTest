using Ncfe.CodeTest.Models;
using System.Collections.Generic;

namespace Ncfe.CodeTest.Interfaces
{
    public interface IFailoverRepository
    {
        List<FailoverEntry> GetFailOverEntries();
    }
}
