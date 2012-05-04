using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tokenizer
{
    /// <summary>
    /// wrapper to hide implementation type of token
    /// </summary>
    public class TokenType
    {
        public Int32 Type { get; set; }


        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

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
