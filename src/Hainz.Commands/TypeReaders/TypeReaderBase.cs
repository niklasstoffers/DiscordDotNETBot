using Discord.Commands;

namespace Hainz.Commands.TypeReaders;

public abstract class TypeReaderBase : TypeReader 
{
    public abstract Type ForType { get; }
}