using System;
using System.Runtime.Serialization;
namespace DemoCoreWeb
{
	[DataContract]
	public class WeatherForecast
	{
		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public int TemperatureC { get; set; }

		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

		[DataMember]
		public string Summary { get; set; }
	}
}
