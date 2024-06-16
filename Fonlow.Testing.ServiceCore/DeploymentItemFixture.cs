using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Fonlow.Testing
{
	/// <summary>
	/// Copy files from source to destination. If file exists, copy only newer.
	/// </summary>
	[Obsolete("In favour of PowerShell scripts")]
	public class DeploymentItemFixture
	{
		public DeploymentItemFixture()
		{
			IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			IConfigurationSection section = config.GetSection("DeploymentItem");
			if (section != null)
			{
				var source = section["Source"];
				var dest = section["Destination"];
				if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(dest))
				{
					throw new ArgumentException("appsettings.json needs to define Source and Destination in section DeploymentItem");
				}

				CopyDirectory(source, dest, true);
			}
		}

		/// <summary>
		/// Copy files. If files exists, copy only newer.
		/// https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
		/// 
		/// </summary>
		/// <param name="sourceDir"></param>
		/// <param name="destinationDir"></param>
		/// <param name="recursive"></param>
		/// <exception cref="DirectoryNotFoundException"></exception>
		static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
		{
			var dir = new DirectoryInfo(sourceDir);
			if (!dir.Exists)
				throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

			DirectoryInfo[] dirs = dir.GetDirectories();
			Directory.CreateDirectory(destinationDir);

			foreach (FileInfo file in dir.GetFiles())
			{
				string targetFilePath = Path.Combine(destinationDir, file.Name);
				if (!File.Exists(targetFilePath) || file.CreationTime > File.GetCreationTime(targetFilePath))
				{
					file.CopyTo(targetFilePath);
				}
			}

			if (recursive)
			{
				foreach (DirectoryInfo subDir in dirs)
				{
					string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
					CopyDirectory(subDir.FullName, newDestinationDir, true);
				}
			}
		}
	}
}
