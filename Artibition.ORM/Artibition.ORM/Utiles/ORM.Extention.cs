using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM
{
    public static class ORMExtention
    {
        public static StringBuilder TrimComma(this StringBuilder sb)
        {
            if (sb.Length > 0)
                return sb.Remove(sb.Length - 1, 1);
            return sb;
        }
    }
}
