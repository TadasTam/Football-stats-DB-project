using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels.RungtynesReport
{
	/// <summary>
	/// View model for single rungtynes in a report.
	/// </summary>
	public class Rungtynes
	{
		//Rungtyniu fields
		[DisplayName("Data")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime FaktineData { get; set; }

		[DisplayName("Šeimininkai")]
		public string Seimininkai { get; set; }

		[DisplayName("Svečiai")]
		public string Sveciai { get; set; }

		
		[DisplayName("Teisėjas")]
		public string Teisejas { get; set; }

		//Turnyro fields
		public string Pavadinimas { get; set; }
			
		public string Sezonas { get; set; }

		public string Salis { get; set; }
		public int FkSalis { get; set; }

		//Rungtynes aggregates
		[DisplayName("Įvarčiai šeimininkų")]
		public int IvSeimininku { get; set; }

		[DisplayName("Įvarčiai svečių")]
		public int IvSveciu { get; set; }

		//Turnyras aggregates
		public int RungtyniuSuma { get; set; }
		public int IvarciuSuma { get; set; }

		public decimal IvVidurkis { get; set; }
	}

	/// <summary>
	/// View model for whole report.
	/// </summary>
	public class Report
	{
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? DateFrom { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? DateTo { get; set; }

		public int? salis { get; set; }
		public string? komanda {get; set; }
		public ListsM Lists { get; set; } = new ListsM();

		public List<Rungtynes> RRungtynes { get; set; }

		public int VisoRungtyniu { get; set; }
		public int VisoIvarciu { get; set; }

		public decimal VisoVidurkis { get; set; }
	}

	public class ListsM
	{
		public IList<SelectListItem> Salys { get; set; }
		public IList<SelectListItem> Komandos { get; set; }
	}

}