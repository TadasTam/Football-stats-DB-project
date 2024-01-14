using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
    /// View model for displaying list oft 'Komanda' entities.
    /// </summary>
	public class KomandaListVM
	{
			[DisplayName("Pavadinimas")]
			public string Pavadinimas { get; set; }
			
			[DisplayName("Miestas")]
			public string Miestas { get; set; }
	}
}