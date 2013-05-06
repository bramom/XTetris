using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
	public static class Extensions
	{
        public static bool IsNull(this Element source)
        {
            return (source == null);
        }
	}
}
