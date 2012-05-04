using System;

namespace Tokenizer
{
    /// <summary>
    /// wrapper to hide implementation type of token
    /// </summary>
    public class TokenType
    {
        public Int32 Type { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Type == ((TokenType)obj).Type;   
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override string ToString()
        {
            return "TokenType:" + Type;
        }
    }
}
