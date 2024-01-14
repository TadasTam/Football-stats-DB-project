using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
    /// View model for displaying list oft 'Turnyras' entities.
    /// </summary>
	public class TurnyrasListVM
	{
			[DisplayName("Pavadinimas")]
			public string Pavadinimas { get; set; }
			
			[DisplayName("Sezonas")]
			public string Sezonas { get; set; }

			[DisplayName("Šalis")]
			public string Salis { get; set; }
	}
}