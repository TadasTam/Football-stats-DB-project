using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
    /// View model for displaying list oft 'Teisejas' entities.
    /// </summary>
	public class TeisejasListVM
	{
		
            [DisplayName("Id")]
            public int Id { get; set; }

            [DisplayName("Vardas")]
            public string Vardas { get; set; }

            [DisplayName("Pavardė")]
            public string Pavarde { get; set; }

            [DisplayName("Šalis")]
            public string Salis { get; set; }
	}
}