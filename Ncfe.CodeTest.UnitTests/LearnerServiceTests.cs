using NUnit.Framework;
using NSubstitute;
using Ncfe.CodeTest.Interfaces;
using Ncfe.CodeTest.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace Ncfe.CodeTest.UnitTests
{
    public class LearnerServiceTests
    {
        private IArchivedDataService _archivedDataService;
        private IFailoverRepository _failoverRepository;
        private ILearnerDataAccess _learnerDataAccess;
        private IFailoverLearnerDataAccess _failoverLearnerDataAccess;
        private int _learnerId;        
        private LearnerService _sut;

        [SetUp]
        public void Setup()
        {
            _archivedDataService = Substitute.For<IArchivedDataService>();
            _failoverRepository = Substitute.For<IFailoverRepository>();
            _learnerDataAccess = Substitute.For<ILearnerDataAccess>();
            _failoverLearnerDataAccess = Substitute.For<IFailoverLearnerDataAccess>();

            _learnerId = 1;   

            _sut = new LearnerService(_archivedDataService, _failoverRepository, _learnerDataAccess, _failoverLearnerDataAccess);
        }

        [Test]
        public void GetLearner_If_IsLearnerArchived_Returns_Learner()
        {
            //Arrange
            bool isLearnerArchived = true;

            var archivedLearner = new Learner();
            _archivedDataService.GetArchivedLearner(Arg.Any<int>()).Returns(archivedLearner);

            //Act
            var learner = _sut.GetLearner(_learnerId, isLearnerArchived);

            //Assert
            learner.Should().BeSameAs(archivedLearner);
        }

        [Test]
        public void GetLearner_FailedRequests_LessThan100_Calls_LearnerDataAccess()
        {
            //Arrange
            bool isLearnerArchived = false;
            int learnerId = 1;           

            var failoverEntries = new List<FailoverEntry>();
            _failoverRepository.GetFailOverEntries().Returns(failoverEntries);

            _learnerDataAccess.LoadLearner(Arg.Any<int>()).Returns(new LearnerResponse { Learner = new Learner() });

            //Act
            var learner = _sut.GetLearner(learnerId, isLearnerArchived);

            //Assert
            learner.Should().NotBeNull();
            _learnerDataAccess.Received(1).LoadLearner(learnerId);
        }

        [Test]
        public void GetLearner_LearnerResponse_IsArchived_Calls_GetArchivedLearner()
        {
            //Arrange
            bool isLearnerArchived = false;
            int learnerId = 1;

            var failoverEntries = new List<FailoverEntry>();
            _failoverRepository.GetFailOverEntries().Returns(failoverEntries);

            _learnerDataAccess.LoadLearner(Arg.Any<int>()).Returns(new LearnerResponse { IsArchived = true , Learner = new Learner() });
            _archivedDataService.GetArchivedLearner(Arg.Any<int>()).Returns(new Learner());

            //Act
            var learner = _sut.GetLearner(learnerId, isLearnerArchived);

            //Assert
            learner.Should().NotBeNull();
            _learnerDataAccess.Received(1).LoadLearner(learnerId);
            _archivedDataService.Received(1).GetArchivedLearner(learnerId);
        }
    }
}