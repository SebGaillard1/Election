using Election;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowElection.StepDefinitions
{
    [Binding]
    public sealed class ElectionStepDefinitions
    {
        private Election.Election election = new Election.Election();
        private Candidate winner;

        [Given(@"a candidate has more than 50% of the vote")]
        public void GivenACandidateHasMoreThanOfTheVote()
        {
            var candidate = new Candidate("Seb");
            for (int i = 0; i < 60; i++)
            {
                election.AddVote(candidate);
            }
        }

        [Given(@"the first round of the election is closed")]
        public void GivenTheFirstRoundOfTheElectionIsClosed()
        {
            election.CloseElection();
        }

        [When(@"we check the result of the first round")]
        public void WhenWeCheckTheResultOfTheFirstRound()
        {
            winner = election.CalculateWinner();
        }

        [Then(@"this candidate has won the election")]
        public void ThenThisCandidateHasWonTheElection()
        {
            winner.Should().NotBeNull();
            winner.Name.Should().Be("Seb");
        }
    }
}
