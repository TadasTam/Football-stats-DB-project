using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
    /// View model for editing data of 'Rungtynes' entity.
    /// </summary>
	public class RungtynesEditVM
	{
		/// <summary>
        /// Entity data.
        /// </summary>
        public class RungtynesM
        {
			[DisplayName("Faktinė data")]
			[DataType(DataType.Date)]
			[Required]
			public DateTime FaktineData { get; set; }

			[DisplayName("Šeimininkas")]
			[Required]
			public string FkSeimininkas { get; set; }

			[DisplayName("Svečias")]
			[Required]
			public string FkSvecias { get; set; }

			[DisplayName("Numatyta data")]
			[DataType(DataType.Date)]
			public DateTime? NumatytaData { get; set; }

			#nullable enable
			[DisplayName("Numatytas laikas")]
			public string? NumatytasLaikas { get; set; }
			#nullable disable

			[DisplayName("Faktinis laikas")]
			[Required]
			public string FaktinisLaikas { get; set; }

			[DisplayName("Žiūrovų skaičius")]
			[Required]
			public int Ziurovai { get; set; }

			[DisplayName("Žaistas pratęsimas")]
			[Required]
			public bool Pratesimas { get; set; }

			[DisplayName("Žaista baudinių serija")]
			[Required]
			public bool Baudiniai { get; set; }

			[DisplayName("Žaista neutralioje aikštėje")]
			[Required]
			public bool Neutrali { get; set; }

			[DisplayName("Stadionas ir miestas")]
			[Required]
			public string FkStadionasIrMiestas { get; set; }

			[DisplayName("Teisėjas")]
			[Required]
			public int FkTeisejas { get; set; }

			[DisplayName("Turnyras")]
			[Required]
			public string FkTurnyroInfo { get; set; }
        }

		/// <summary>
        /// Representation of 'Ivartis' entity in 'Rungtynes' edit form.
        /// </summary>
		public class IvartisM
		{
			/// <summary>
            /// ID of the record in the form. Is used when adding and removing records.
            /// </summary>
            public int InListId { get; set; }

            [DisplayName("Minutė")]
			[Required]
            public int Minute { get; set; }

			[DisplayName("Ar į savus")]
			[Required]
			public bool Savus { get; set; }

			[DisplayName("Ar baudinys")]
			[Required]
			public bool Baudinys { get; set; }

			[DisplayName("Už kurią komandą")]
			[Required]
			public string KuriKom { get; set; }

			[DisplayName("Žaidėjas")]
			[Required]
			public int FkZaidejas { get; set; }
        }

		/// <summary>
        /// Select lists for making drop downs for choosing values of entity fields.
        /// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Komandos { get; set; }

			public IList<SelectListItem> Stadionai { get; set; }

			public IList<SelectListItem> Teisejai { get; set; }

			public IList<SelectListItem> Turnyrai { get; set; }
			

        	public IList<SelectListItem> Zaidejai {get;set;}
        	public IList<SelectListItem> UzKuria {get;set;}
		}

		/// <summary>
        /// Entity view.
        /// </summary>
        public RungtynesM Rungtynes { get; set; } = new RungtynesM();

		/// <summary>
        /// Views of related 'Ivarciai' records.
        /// </summary>
        public IList<IvartisM> Ivarciai { get; set;  } = new List<IvartisM>();

		/// <summary>
        /// Lists for drop down controls.
        /// </summary>
        public ListsM Lists { get; set; } = new ListsM();
	}
}