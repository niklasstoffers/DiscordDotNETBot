using Discord.Commands;

namespace Hainz.Commands.Helpers;

public static class PreconditionResultBuilder
{
    public static PreconditionResult SuccessIf(bool condition, string errorReason) =>
        condition ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(errorReason);
    
    public static PreconditionResult SuccessIfNot(bool condition, string errorReason) =>
        SuccessIf(!condition, errorReason);
}