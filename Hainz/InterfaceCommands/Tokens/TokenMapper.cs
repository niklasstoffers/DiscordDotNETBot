using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.Tokens
{
    public class TokenMapper
    {
        public T? Map<T>(string token) where T : struct
        {
            token = token.Insert(0, char.ToUpper(token[0]).ToString());
            token = token.Remove(1, 1);

            if (Enum.IsDefined(typeof(T), token))
                return (T)Enum.Parse(typeof(T), token);
            return null;
        }
    }
}
