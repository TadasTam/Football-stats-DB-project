using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
	/// Model of 'Miestas' entity used in creation and editing forms.
	/// </summary>
	public class MiestasEditVM
	{
		/// <summary>
		/// Entity data
		/// </summary>
		public class MiestasM
		{
			[DisplayName("Id")]
			public int Id { get; set; }

			[DisplayName("Pavadinimas")]
			[MaxLength(20)]
			[Required]
			public string Pavadinimas { get; set; }

			[DisplayName("Šalis")]
			[Required]
			public int FkSalis { get; set; }
		}


		/// <summary>
        /// Representation of 'Stadionas' entity in 'Miestas' edit form.
        /// </summary>
		public class StadionasM
		{
			/// <summary>
            /// ID of the record in the form. Is used when adding and removing records.
            /// </summary>
            public int InListId { get; set; }

            [DisplayName("Pavadinimas")]
			[Required]	
            public string Pavadinimas { get; set; }

            [DisplayName("Talpa")]
			[Required]	
            public int Talpa { get; set; }

            [DisplayName("Adresas")]
			[Required]	
            public string Adresas { get; set; }

            [DisplayName("Metai")]
			[Required]	
            public int Metai { get; set; }

            [DisplayName("Renovuotas")]
            public int? Renovuotas { get; set; }
        }

		/// <summary>
		/// Select lists for making drop downs for choosing values of entity fields.
		/// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Salys { get; set; }
		}

		/// <summary>
		/// Entity view.
		/// </summary>
		public MiestasM Miestai { get; set; } = new MiestasM();

		/// <summary>
        /// Views of related 'Stadionai' records.
        /// </summary>
        public IList<StadionasM> Stadionai { get; set;  } = new List<StadionasM>();

		/// <summary>
		/// Lists for drop down controls.
		/// </summary>
		public ListsM Lists { get; set; } = new ListsM();
	}
}