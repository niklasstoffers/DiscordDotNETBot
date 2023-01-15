using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Models;

namespace Hainz.Commands.Modules.Gamble;

[CommandName("diceroll")]
[Summary("rolls a dice")]
public class DiceRollCommand : GambleCommandBase
{
    [Command("diceroll")]
    public async Task RollDiceAsync([CommandParameter("sides", "number of sides")]int? sides = null)
    {
        if (sides < 2)
        {
            await ReplyAsync("The dice must have at least two sides");
        }
        else
        {
            int side;

            if (sides == null) side = Dice.Roll();
            else side = Dice.Roll(sides.Value);

            await ReplyAsync($"🔮 Dice landed on side {Format.Bold(side.ToString())}");
        }
    }
}