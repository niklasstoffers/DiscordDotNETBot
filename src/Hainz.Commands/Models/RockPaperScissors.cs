using static Hainz.Commands.Models.RockPaperScissorsSelection;

namespace Hainz.Commands.Models;

public static class RockPaperScissors
{
    private static readonly Dictionary<RockPaperScissorsSelection, RockPaperScissorsSelection> _winMap = new()
    {
        { Paper, Rock },
        { Rock, Scissors },
        { Scissors, Paper }
    };

    public static bool WinsAgainst(RockPaperScissorsSelection first, RockPaperScissorsSelection second) => _winMap[first] == second;
    public static bool LosesAgainst(RockPaperScissorsSelection first, RockPaperScissorsSelection second) => !WinsAgainst(first, second);

    public static RockPaperScissorsSelection GetWinningSelection(RockPaperScissorsSelection selection) => _winMap[_winMap[selection]];
    public static RockPaperScissorsSelection GetLosingSelection(RockPaperScissorsSelection selection) => _winMap[selection];
}