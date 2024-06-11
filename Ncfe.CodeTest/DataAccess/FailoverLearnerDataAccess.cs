using Ncfe.CodeTest.Interfaces;
using Ncfe.CodeTest.Models;

namespace Ncfe.CodeTest.DataAccess
{
    public class FailoverLearnerDataAccess : IFailoverLearnerDataAccess
    {
        public LearnerResponse GetLearnerById(int id)
        {
            // retrieve learner from database
            return new LearnerResponse();
        }
    }
}
