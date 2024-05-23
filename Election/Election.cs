namespace Election
{
    public class Election
    {
        private Dictionary<Candidate, int> votes = new Dictionary<Candidate, int>();
        private bool isClosed = false;

        public void AddVote(Candidate candidate)
        {
            if (isClosed)
            {
                throw new InvalidOperationException("The election is already closed.");
            }
            if (votes.ContainsKey(candidate))
            {
                votes[candidate]++;
            }
            else
            {
                votes.Add(candidate, 1);
            }
        }

        public void CloseElection()
        {
            isClosed = true;
        }

        public Candidate CalculateWinner()
        {
            if (!isClosed)
            {
                throw new InvalidOperationException("Election is not yet closed.");
            }

            int totalVotes = votes.Values.Sum();
            foreach (var pair in votes)
            {
                double percentage = (double)pair.Value / totalVotes * 100;
                if (percentage > 50)
                    return pair.Key;
            }

            return null; // No winner if no one has more than 50%
        }
    }
}
