using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fonlow.Testing
{
    public static class DirFunctions
    {
        public static DirectoryInfo GetSlnDir(string dir)
        {
            var d = new DirectoryInfo(dir);

            DirectoryInfo[] ds;
            do
            {
                d = d.Parent;
                if (d == null)
                    break;

                ds = d.EnumerateDirectories(".vs", SearchOption.TopDirectoryOnly).ToArray();
            }
            while (ds.Length == 0);

            return d;
        }

    }
}
