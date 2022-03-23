using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hainz.Models;

namespace Hainz.Commands.Modules
{
    public class RandomModule : ModuleBase<SocketCommandContext> 
    {
        [Command("coinflip", RunMode = RunMode.Async)]
        public async Task CoinFlipAsync() 
        {
            var result = Coin.Flip();
            string message = result switch 
            {
                CoinResult.Head => "Kopf",
                _ => "Zahl"
            };
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("dice", RunMode = RunMode.Async)]
        public async Task RollDiceAsync(int sides = Dice.DEFAULT_SIDES) 
        {
            if (sides < 1)
            {
                await Context.Channel.SendMessageAsync($"Hast du schonmal nen Würfel mit {sides} Seiten gesehen?");
                return;
            }

            var dice = new Dice(sides);
            var result = dice.RollOnce();
            await Context.Channel.SendMessageAsync($"Es wurde eine {result} gewürfelt.");
        }

        [Command("random", RunMode = RunMode.Async)]
        public async Task RandomAsync(int lower, int upper) 
        {
            if (upper <= lower) 
            {
                await Context.Channel.SendMessageAsync($"{nameof(upper)} muss größer als {nameof(lower)} sein.");
                return;
            }

            var random = new Random();
            var result = random.Next(lower, upper);
            await Context.Channel.SendMessageAsync($"Die Zufallszahl ist {result}");
        }
    }
}