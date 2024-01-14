using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
	/// Model of 'Zaidejas' entity used in creation and editing forms.
	/// </summary>
	public class ZaidejasEditVM
	{
		/// <summary>
		/// Entity data
		/// </summary>
		public class ModelM
		{
			[DisplayName("Id")]
			[Required]
			public int Id { get; set; }

			[DisplayName("Vardas")]
			[Required]
			public string Vardas { get; set; }

			[DisplayName("Pavardė")]
			[Required]
			public string Pavarde { get; set; }

			[DisplayName("Žinomas kaip")]
			[Required]
			public string Zinomas { get; set; }

			[DisplayName("Gimimo data")]
			[DataType(DataType.Date)]
			[Required]
			public DateTime Data { get; set; }

			[DisplayName("Pozicija")]
			[Required]
			public string Pozicija { get; set; }

			[DisplayName("Šalis")]
			[Required]
			public int FkSalis { get; set; }
		}

		/// <summary>
		/// Select lists for making drop downs for choosing values of entity fields.
		/// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Salys { get; set; }
			public IList<SelectListItem> Pozicijos { get; set; }
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