using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
using Hainz.Commands.Models;
using Hainz.Commands.Preconditions;
using static Hainz.Commands.Models.RockPaperScissorsSelection;

namespace Hainz.Commands.Modules.Gamble;

[OnlyInGuild]
[CommandName("rockpaperscissors")]
[Summary("Starts a game of rock paper scissors")]
public class RockPaperScissorsCommand : GambleCommandBase
{
    [Command("rockpaperscissors")]
    [Summary("Play rock, paper, scisors against another user")]
    public async Task RockPaperScissorsAsync([NotSelfInvokable, NoBot, CommandParameter("user", "the user to play against")] SocketGuildUser user)
    {
        var componentBuilder = new ComponentBuilder()
            .WithButton("Rock", customId: "rock", style: ButtonStyle.Secondary, emote: new Emoji("✊🏻"))
            .WithButton("Paper", customId: "paper", style: ButtonStyle.Secondary, emote: new Emoji("✋🏻"))
            .WithButton("Scissors", customId: "scissors", style: ButtonStyle.Secondary, emote: new Emoji("✌🏻"));

        await ReplyAsync($"{Format.Bold("Rock Paper Scissors:")} {Context.User.Username} 🆚 {user.Username}. Make your choice!", components : componentBuilder.Build());
    }

    [Command("rockpaperscissors")]
    [Summary("Play rock, paper, scissors against the computer")]
    [Remarks("very difficult to win")]
    public async Task RockPaperScissorsAsync([CommandParameter("selection", "rock, paper or scissors")] RockPaperScissorsSelection selection)
    {
        var winningSelection = RockPaperScissors.GetWinningSelection(selection);
        await ReplyAsync($"Computer selected {winningSelection}. You lost.");
    }
}