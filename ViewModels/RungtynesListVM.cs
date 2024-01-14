using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
    /// View model for displaying list oft 'Rungtynes' entities.
    /// </summary>
	public class RungtynesListVM
	{
		[DisplayName("Faktinė data")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime FaktineData { get; set; }

		[DisplayName("Šeimininkai")]
		public string Seimininkai { get; set; }

		[DisplayName("Svečiai")]
		public string Sveciai { get; set; }
	}
}