using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
	/// <summary>
	/// Controller for working with 'Komanda' entity.
	/// </summary>
	public class KomandaController : Controller
	{

		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			return View(KomandaRepo.List());
		}

		/// <summary>
		/// This is invoked when creation form is first opened in browser.
		/// </summary>
		/// <returns>Creation form view.</returns>
		public ActionResult Create()
		{
			var komandaEvm = new KomandaEditVM();
			PopulateSelections(komandaEvm);
			return View(komandaEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="komandaEvm">Entity model filled with latest data.</param>
		/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(KomandaEditVM komandaEvm)
		{
				var match = KomandaRepo.Find(komandaEvm.Model.Pavadinimas);
				if (match!=null)
				{
					ModelState.AddModelError("pavadinimas", "Field value already exists in database.");
				}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				KomandaRepo.Insert(komandaEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(komandaEvm);
			return View(komandaEvm);
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(string pavadinimas)
		{
			var komandaEvm = KomandaRepo.Find(pavadinimas);
			PopulateSelections(komandaEvm);

			return View(komandaEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="komandaEvm">Entity model filled with latest data.</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(string pavadinimas, KomandaEditVM komandaEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				KomandaRepo.Update(komandaEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(komandaEvm);
			return View(komandaEvm);
		}

		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(string pavadinimas)
		{
			var komandaLvm = KomandaRepo.FindForDeletion(pavadinimas);
			return View(komandaLvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(string pavadinimas)
		{
			//try deleting, this will fail if foreign key constraint fails
			try
			{
				KomandaRepo.Delete(pavadinimas);

				//deletion success, redired to list form
				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var komandaLvm = KomandaRepo.FindForDeletion(pavadinimas);
				return View("Delete", komandaLvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="komandaEvm">'Automobilis' view model to append to.</param>
		public void PopulateSelections(KomandaEditVM komandaEvm)
		{
			//build select lists
			komandaEvm.Lists.Stadionai =
				StadionasRepo.ListWithCity().Select(it => {
					string miestas = it.Miestas;
					return new SelectListItem() {
							Value = it.Pavadinimas + ";" + StadionasRepo.FindCityId(it.Miestas),
							Text = Convert.ToString(it.Pavadinimas) + " (" + miestas + ")"
					};
				})
				.ToList();
		}
	}
}
