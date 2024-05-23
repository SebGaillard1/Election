namespace Election
{
    public class Election
    {
        private Dictionary<Candidate, int> votes = new Dictionary<Candidate, int>();
        private bool isClosed = false;
        public int Round { get; private set; } = 1;

        public void AddVote(Candidate candidate)
        {
            if (isClosed)
                throw new InvalidOperationException("The election is already closed.");

            if (votes.ContainsKey(candidate))
                votes[candidate]++;
            else
                votes.Add(candidate, 1);
        }

        public void CloseElection()
        {
            isClosed = true;
        }

        public void OpenNextRound()
        {
            if (Round >= 2)
                throw new InvalidOperationException("No more than two rounds are allowed.");

            isClosed = false;
            // Only keep top two candidates if this is the transition from first to second round
            if (Round == 1)
            {
                var topTwo = votes.OrderByDescending(v => v.Value).Take(2).ToDictionary(pair => pair.Key, pair => 0);
                votes = topTwo;
            }
            Round++;
        }

        public Candidate? CalculateWinner()
        {
            if (!isClosed)
                throw new InvalidOperationException("Election is not yet closed.");

            int totalVotes = votes.Values.Sum();
            var sortedResults = votes
                .Select(v => new { Candidate = v.Key, Percentage = (double)v.Value / totalVotes * 100 })
                .OrderByDescending(v => v.Percentage)
                .ToList();

            // Direct winner in the first round
            if (Round == 1 && sortedResults.FirstOrDefault()?.Percentage > 50)
            {
                return sortedResults.First().Candidate;
            }

            // Check if we need a second round
            if (Round == 1 && sortedResults.FirstOrDefault()?.Percentage <= 50)
            {
                OpenNextRound();
                return null; // No winner yet, need a second round
            }

            // Second round results
            if (Round == 2)
            {
                if (sortedResults[0].Percentage == sortedResults[1].Percentage)
                    return null; // Tie in the second round, no winner declared

                return sortedResults.First().Candidate; // Winner of the second round
            }

            return null;
        }

        public Dictionary<Candidate, (int Votes, double Percentage)> GetResults()
        {
            if (!isClosed)
                throw new InvalidOperationException("Election is not yet closed.");

            int totalVotes = votes.Values.Sum();
            return votes.ToDictionary(
                pair => pair.Key,
                pair => (pair.Value, (double)pair.Value / totalVotes * 100));
        }
    }
}
