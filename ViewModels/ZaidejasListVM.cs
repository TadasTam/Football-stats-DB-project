using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.Autonuoma.ViewModels
{
	/// <summary>
	/// Model of 'Zaidejas' entity used in lists.
	/// </summary>
	public class ZaidejasListVM
	{
		[DisplayName("Id")]
		public int Id { get; set; }

		[DisplayName("Vardas")]
		public string Vardas { get; set; }		

		[DisplayName("Šalis")]
		public string Salis { get; set; }
	}
}