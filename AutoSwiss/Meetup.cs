using System.Text.Json.Serialization;

namespace AutoSwiss;

record struct Meetup(string Home, string Away)
{
    /// <inheritdoc />
    public bool Equals(Meetup other)
    {
        return (this.Home, this.Away) == (other.Home, other.Away) || (this.Home, this.Away) == (other.Away, other.Home);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return this.Home.GetHashCode() + this.Away.GetHashCode();
    }

    [JsonPropertyName("home")]
    public string Home { get; } = Home;

    [JsonPropertyName("away")]
    public string Away { get; } = Away;
}