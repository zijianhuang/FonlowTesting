using System.IO;
using System.Linq;

namespace Fonlow.Testing
{
	public static class DirFunctions
	{
		/// <summary>
		/// Get Visual Studio Sln dir.
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
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
