using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
	/// <summary>
	/// Controller for working with 'Turnyras' entity.
	/// </summary>
	public class TurnyrasController : Controller
	{

		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			var turnyrai = TurnyrasRepo.List();
			return View(turnyrai);
		}

		/// <summary>
		/// This is invoked when creation form is first opened in browser.
		/// </summary>
		/// <returns>Creation form view.</returns>
		public ActionResult Create()
		{
			var turnyrasEvm = new TurnyrasEditVM();
			PopulateSelections(turnyrasEvm);
			return View(turnyrasEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="turnyrasEvm">Entity model filled with latest data.</param>
		/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(TurnyrasEditVM turnyrasEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				TurnyrasRepo.Insert(turnyrasEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(turnyrasEvm);
			return View(turnyrasEvm);
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(string pavadinimas, string sezonas, string salis)
		{
			var turnyrasEvm = TurnyrasRepo.Find(pavadinimas, sezonas, salis);
			PopulateSelections(turnyrasEvm);

			return View(turnyrasEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="autoEvm">Entity model filled with latest data.</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(string pavadinimas, string sezonas, string salis, TurnyrasEditVM TurnyrasEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				TurnyrasRepo.Update(TurnyrasEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(TurnyrasEvm);
			return View(TurnyrasEvm);
		}

		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(string pavadinimas, string sezonas, string salis)
		{
			var TurnyrasLvm = TurnyrasRepo.FindForDeletion(pavadinimas, sezonas, salis);
			return View(TurnyrasLvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(string pavadinimas, string sezonas, string salis)
		{
			//try deleting, this will fail if foreign key constraint fails
			try
			{
				TurnyrasRepo.Delete(pavadinimas, sezonas, salis);

				//deletion success, redired to list form
				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var TurnyrasLvm = TurnyrasRepo.FindForDeletion(pavadinimas, sezonas, salis);

				return View("Delete", TurnyrasLvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="TurnyrasEvm">'Automobilis' view model to append to.</param>
		public void PopulateSelections(TurnyrasEditVM TurnyrasEvm)
		{
			//load entities for the select lists
			var salys = SalisRepo.List();

			//build select lists
			TurnyrasEvm.Lists.Salys =
				salys.Select(it => {
					return
						new SelectListItem() {
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			List<string> tipai = new List<string>{"taure", "lyga", "draugiskos rungtynes", "cempionatas"};
			TurnyrasEvm.Lists.Tipai = 
				tipai.Select(it => {
					return
						new SelectListItem() {
							Value = it,
							Text = it
						};
				}).ToList();

			List<string> keitimai = new List<string>{"3 is 5", "3 is 7", "5 is 7", "3 is 12", "5 is 12"};
			TurnyrasEvm.Lists.KeitimaiL = 
				keitimai.Select(it => {
					return
						new SelectListItem() {
							Value = it,
							Text = it
						};
				}).ToList();
		}
	}
}
