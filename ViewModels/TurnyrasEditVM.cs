using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
	/// Model of 'Turnyras' entity used in creation and editing forms.
	/// </summary>
	public class TurnyrasEditVM
	{
		/// <summary>
		/// Entity data
		/// </summary>
		public class ModelM
		{
			[DisplayName("Pavadinimas")]
			[Required]	
			public string Pavadinimas { get; set; }
			
			[DisplayName("Sezonas")]
			[Required]	
			public string Sezonas { get; set; }

			[DisplayName("Šalis")]
			[Required]	
			public int FkSalis { get; set; }

			[DisplayName("Nacionalinių rinktinių turnyras")]
			[Required]	
			public bool Rinktines { get; set; }

			[DisplayName("Klubinis turnyras")]
			[Required]	
			public bool Klubai { get; set; }

			[DisplayName("Tipas")]
			[Required]	
			public string Tipas { get; set; }

			[DisplayName("Galimų keitimų skaičius")]
			[Required]	
			public string Keitimai { get; set; }
		}

		/// <summary>
		/// Select lists for making drop downs for choosing values of entity fields.
		/// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Salys { get; set; }
			public IList<SelectListItem> Tipai { get; set; }
			public IList<SelectListItem> KeitimaiL { get; set; }
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