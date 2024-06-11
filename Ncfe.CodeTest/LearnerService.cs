using Ncfe.CodeTest.Interfaces;
using Ncfe.CodeTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Ncfe.CodeTest
{
    public class LearnerService
    {
        private readonly IArchivedDataService _archivedDataService;
        private readonly IFailoverRepository _failoverRepository;
        private readonly ILearnerDataAccess _learnerDataAccess;
        private readonly IFailoverLearnerDataAccess _failoverLearnerDataAccess;

        public LearnerService
            (
                IArchivedDataService archivedDataService,
                IFailoverRepository failoverRepository,
                ILearnerDataAccess learnerDataAccess,
                IFailoverLearnerDataAccess failoverLearnerDataAccess
            )
        {
            _archivedDataService = archivedDataService;
            _failoverRepository = failoverRepository;
            _learnerDataAccess = learnerDataAccess;
            _failoverLearnerDataAccess = failoverLearnerDataAccess;
        }

        public Learner GetLearner(int learnerId, bool isLearnerArchived)
        {
            if (isLearnerArchived)
            {
                var archivedLearner = _archivedDataService.GetArchivedLearner(learnerId);
                return archivedLearner;
            }
            else
            {
                var failoverEntries = _failoverRepository.GetFailOverEntries();
                int failedRequests = GetFailedRequests(failoverEntries);

                LearnerResponse learnerResponse;
                Learner learner;

                if (failedRequests > 100 && ConfigurationManager.AppSettings["IsFailoverModeEnabled"]?.ToLower() == "true")
                {
                    learnerResponse = _failoverLearnerDataAccess.GetLearnerById(learnerId);
                }
                else
                {
                    learnerResponse = _learnerDataAccess.LoadLearner(learnerId);
                }

                if (learnerResponse.IsArchived)
                {
                    learner = _archivedDataService.GetArchivedLearner(learnerId);
                }
                else
                {
                    learner = learnerResponse.Learner;
                }

                return learner;
            }
        }

        public int GetFailedRequests(List<FailoverEntry> failoverEntries)
        {
            var failedRequests = 0;

            foreach (var failoverEntry in failoverEntries)
            {
                if (failoverEntry.DateTime > DateTime.Now.AddMinutes(-10))
                {
                    failedRequests++;
                }
            }

            return failedRequests;
        }
    }
}
