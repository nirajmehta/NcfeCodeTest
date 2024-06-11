using Ncfe.CodeTest.Interfaces;
using Ncfe.CodeTest.Models;

namespace Ncfe.CodeTest.Services
{
    public class LearnerDataAccess : ILearnerDataAccess
    {
        public LearnerResponse LoadLearner(int learnerId)
        {
            // rettrieve learner from 3rd party webservice
            return new LearnerResponse();
        }
    }
}
