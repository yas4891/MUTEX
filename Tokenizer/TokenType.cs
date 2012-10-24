using System;

namespace Tokenizer
{
    /// <summary>
    /// wrapper to hide implementation type of token
    /// </summary>
    public class TokenType
    {
        /// <summary>
        /// the underlying type 
        /// </summary>
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
            //Console.WriteLine("TokenType: got called for {0}", Type);
            return Type.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("TokenType:{0}", Type);
        }
    }
}
