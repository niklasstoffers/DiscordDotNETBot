namespace Hainz.Commands.Models;

public static class Dice
{
    public static int Roll() => Roll(6);
    public static int Roll(int sides) => Random.Shared.Next(1, sides);
}