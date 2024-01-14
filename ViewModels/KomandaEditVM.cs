using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
	/// Model of 'Komanda' entity used in creation and editing forms.
	/// </summary>
	public class KomandaEditVM
	{
		/// <summary>
		/// Entity data
		/// </summary>
		public class ModelM
		{
			[DisplayName("Pavadinimas")]
			[Required]	
			public string Pavadinimas { get; set; }
			
			#nullable enable
			[DisplayName("Trumpas pavadinimas")]
			public string? Sutrumpinimas { get; set; }
			#nullable disable

			[DisplayName("Treneris")]
			[Required]	
			public string Treneris { get; set; }

			[DisplayName("Stadionas ir miestas")]
			public string FKs { get; set; }
		}

		/// <summary>
		/// Select lists for making drop downs for choosing values of entity fields.
		/// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Stadionai { get; set; }
		}

		/// <summary>
		/// Entity view.
		/// </summary>
		public ModelM Model { get; set; } = new ModelM();

		/// <summary>
		/// Lists for drop down controls.
		/// </summary>
		public ListsM Lists { get; set; } = new ListsM();
	}
}