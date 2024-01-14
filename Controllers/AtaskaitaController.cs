using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using RungtynesReport = Org.Ktu.Isk.P175B602.Autonuoma.ViewModels.RungtynesReport;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
	/// <summary>
	/// Controller for producing reports.
	/// </summary>
	public class AtaskaitaController : Controller
	{

		/// <summary>
		/// Produces rungtynes report.
		/// </summary>
		/// <param name="dateFrom">Starting date. Can be null.</param>
		/// <param name="dateTo">Ending date. Can be null.</param>
		/// <param name="salis">Country of tournament. </param>
		/// <returns>Report view.</returns>
		public ActionResult Rungtynes(DateTime? dateFrom, DateTime? dateTo, int? salis, string? komanda)
		{
			var report = new RungtynesReport.Report();
			PopulateSelections(report);
			report.DateFrom = dateFrom;
			report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); //move time of end date to end of day
			report.salis = salis;
			report.komanda = komanda;

			report.RRungtynes = AtaskaitaRepo.GetRungtynes(report.DateFrom, report.DateTo, report.salis, report.komanda);

			foreach (var item in report.RRungtynes)
			{
				report.VisoRungtyniu += 1;
				report.VisoIvarciu += item.IvSeimininku + item.IvSveciu;
			}

			if ((decimal)report.VisoRungtyniu == 0)
				report.VisoVidurkis = 0;
			else
				report.VisoVidurkis = (decimal)report.VisoIvarciu / (decimal)report.VisoRungtyniu;

			return View(report);
		}

		public void PopulateSelections(RungtynesReport.Report report)
		{
			//build select lists
			report.Lists.Salys =
				SalisRepo.List().Select(it => {
					return
						new SelectListItem() {
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			report.Lists.Komandos =
				KomandaRepo.List().Select(it => {
					return new SelectListItem() {
							Value = it.Pavadinimas,
							Text = it.Pavadinimas
					};
				})
				.ToList();
		}
	}
}
