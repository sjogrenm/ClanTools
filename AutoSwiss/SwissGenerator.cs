namespace AutoSwiss;

internal class SwissGenerator
{
    private readonly Random rng = new(42);
    private readonly HashSet<Meetup> meetups = new();
    private readonly Dictionary<string, int> homeGames = new();
    private readonly Dictionary<string, int> awayGames = new();

    public void AddSeededRound(IEnumerable<Meetup> round)
    {
        round.ForEach(m =>
        {
            if (!this.meetups.Add(m))
            {
                throw new ArgumentException();
            }
            this.homeGames.AddOrUpdate(m.Home, 1, c => c + 1);
            this.awayGames.AddOrUpdate(m.Away, 1, c => c + 1);
        });
    }

    public IEnumerable<Meetup> GeneratePairings(IList<(string Name, int Score)> teams)
    {
        var reverseRound = this.GeneratePairingsImpl(teams);
        if (reverseRound == null)
        {
            throw new InvalidOperationException("Panic");
        }

        reverseRound.Reverse();
        reverseRound.ForEach(m =>
        {
            this.homeGames.AddOrUpdate(m.Home, 1, c => c + 1);
            this.awayGames.AddOrUpdate(m.Away, 1, c => c + 1);
        });
        return reverseRound;
    }

    private List<Meetup>? GeneratePairingsImpl(IList<(string Name, int Score)> teamList)
    {
        if (teamList.Count == 0)
        {
            return new List<Meetup>();
        }

        var t1 = teamList[0];
        foreach (var t2 in teamList.Skip(1))
        {
            var m = new Meetup(t1.Name, t2.Name);
            if (!this.meetups.Add(m))
            {
                continue;
            }

            var t1HomeBias = this.homeGames.GetValueOrDefault(t1.Name) - this.awayGames.GetValueOrDefault(t1.Name);
            var t2HomeBias = this.homeGames.GetValueOrDefault(t2.Name) - this.awayGames.GetValueOrDefault(t2.Name);
            if (t1HomeBias > t2HomeBias || (t1HomeBias == t2HomeBias && this.rng.NextBool()))
            {
                m = new Meetup(t2.Name, t1.Name);
            }

            var teamListCopy = teamList.Skip(1).ToList();
            teamListCopy.Remove(t2);
            var round = this.GeneratePairingsImpl(teamListCopy);
            if (round != null)
            {
                round.Add(m);
                return round;
            }

            this.meetups.Remove(m);
        }

        return null;
    }
}