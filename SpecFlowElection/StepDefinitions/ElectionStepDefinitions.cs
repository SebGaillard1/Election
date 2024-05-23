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
        private Candidate? winner;
        private Dictionary<Candidate, (int Votes, double Percentage)> results;


        [Given(@"a candidate has more than 50% of the vote")]
        public void GivenACandidateHasMoreThan50PercentOfTheVote()
        {
            var candidate = new Candidate("Seb");
            for (int i = 0; i < 60; i++)
            {
                election.AddVote(candidate);
            }
        }

        [Given(@"no candidate has more than 50% of the vote")]
        public void GivenNoCandidateHasMoreThan50PercentOfTheVote()
        {
            var candidate1 = new Candidate("Alice");
            var candidate2 = new Candidate("Bob");
            var candidate3 = new Candidate("Seb");
            for (int i = 0; i < 35; i++)
            {
                election.AddVote(candidate1);
            }
            for (int i = 0; i < 40; i++)
            {
                election.AddVote(candidate2);
            }
            for (int i = 0; i < 25; i++)
            {
                election.AddVote(candidate3);
            }
        }

        [Given(@"candidates have contested the second round")]
        public void GivenCandidatesHaveContestedTheSecondRound()
        {
            election.OpenNextRound();
            var candidate1 = new Candidate("Alice");
            var candidate2 = new Candidate("Bob");
            for (int i = 0; i < 40; i++)
            {
                election.AddVote(candidate1);
            }
            for (int i = 0; i < 60; i++)
            {
                election.AddVote(candidate2);
            }
        }

        [Given(@"candidates have the same percentage of votes in the second round")]
        public void GivenCandidatesHaveTheSamePercentageOfVotesInTheSecondRound()
        {
            election.OpenNextRound();
            var candidate1 = new Candidate("Alice");
            var candidate2 = new Candidate("Bob");
            for (int i = 0; i < 50; i++)
            {
                election.AddVote(candidate1);
                election.AddVote(candidate2);
            }
        }

        [Given(@"the first round of the election is closed")]
        public void GivenTheFirstRoundOfTheElectionIsClosed()
        {
            election.CloseElection();
        }

        [Given(@"the second round of the election is closed")]
        public void GivenTheSecondRoundOfTheElectionIsClosed()
        {
            election.CloseElection();
        }

        [When(@"we check the result of the first round")]
        public void WhenWeCheckTheResultOfTheFirstRound()
        {
            winner = election.CalculateWinner();
        }

        [When(@"we check the result of the second round")]
        public void WhenWeCheckTheResultOfTheSecondRound()
        {
            winner = election.CalculateWinner();
        }

        [Then(@"this candidate has won the election")]
        public void ThenThisCandidateHasWonTheElection()
        {
            winner.Should().NotBeNull();
            winner.Name.Should().Be("Seb");
        }

        [Then(@"the top two candidates should proceed to a second round")]
        public void ThenTheTopTwoCandidatesShouldProceedToASecondRound()
        {
            winner.Should().BeNull();
            election.Round.Should().Be(2);
        }

        [Then(@"the candidate with the highest percentage of votes wins the election")]
        public void ThenTheCandidateWithTheHighestPercentageOfVotesWinsTheElection()
        {
            winner.Should().NotBeNull();
            winner.Name.Should().Be("Bob");
        }

        [Then(@"no winner is declared due to a tie")]
        public void ThenNoWinnerIsDeclaredDueToATie()
        {
            winner.Should().BeNull();
        }

        [Given(@"votes are cast as follows")]
        public void GivenVotesAreCastAsFollows(Table table)
        {
            foreach (var row in table.Rows)
            {
                var candidate = new Candidate(row["Candidate"]);
                int votes = int.Parse(row["Votes"]);
                for (int i = 0; i < votes; i++)
                {
                    election.AddVote(candidate);
                }
            }
            election.CloseElection();
        }

        [When(@"we check the election results")]
        public void WhenWeCheckTheElectionResults()
        {
            results = election.GetResults();
        }

        [Then(@"the election results should show the following details")]
        public void ThenTheElectionResultsShouldShowTheFollowingDetails(Table table)
        {
            foreach (var row in table.Rows)
            {
                var candidateName = row["Candidate"];
                var expectedVotes = int.Parse(row["Votes"]);
                var expectedPercentage = double.Parse(row["Percentage"]);

                var candidateResult = results.FirstOrDefault(r => r.Key.Name == candidateName).Value;
                candidateResult.Votes.Should().Be(expectedVotes);
                candidateResult.Percentage.Should().BeApproximately(expectedPercentage, 0.1);
            }
        }
    }
}

