using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
    /// View model for displaying list oft 'Stadionas' entities.
    /// </summary>
	public class StadionasListVM
	{
		[DisplayName("Pavadinimas")]
		public string Pavadinimas { get; set; }

		[DisplayName("Miestas")]
		public string Miestas { get; set; }

        [DisplayName("Talpa")]
		public int Talpa { get; set; }

        [DisplayName("Adresas")]
		public string Adresas { get; set; }

        [DisplayName("Metai")]
		public int Metai { get; set; }

        [DisplayName("Renovuotas")]
		public int? Renovuotas { get; set; }
	}
}