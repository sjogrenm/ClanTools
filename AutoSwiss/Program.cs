using System.Text.Json;

namespace AutoSwiss;

public class Program
{
    public static void Main(string[] args)
    {
        var swiss = new SwissGenerator();

        Console.WriteLine("== ROUND 1 ==");
        var round1 =
            "GOON\tDVLA\r\nBBQ3\tLMC\r\nDREDD\tEGG\r\nBBQ\tDUSTY\r\nWINE\tBOSNA\r\nBLOOD\tFIST\r\nDVLB\tCINCH\r\nKDOM\tRAVE\r\nATHNS\tNAME\r\nBBT\tBARDS\r\nFABBL\tBBQ2\r\nGODS\tSALTY\r\nHUGE\tADMIN"
                .Split("\r\n").Select(s =>
                {
                    var x = s.Split("\t");
                    return new Meetup(x[0], x[1]);
                }).ToArray();
        swiss.AddSeededRound(round1);
        Console.WriteLine(JsonSerializer.Serialize(round1));
    }

    private static IEnumerable<(string, int)> GetTeamScores(string clanData)
    {
        return clanData.Split("\r\n").Select(row => row.Split("\t")).Select(row =>
        {
            var clan = row[1];
            var points = int.Parse(row[2]);
            var matchD = int.Parse(row[7]);
            var tdD = int.Parse(row[10]);
            return (clan, points * 10000 + matchD * 100 + tdD);
        });
    }
}
