using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
	/// <summary>
	/// Controller for working with 'Zaidejas' entity.
	/// </summary>
	public class ZaidejasController : Controller
	{
		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			var zaidejai = ZaidejasRepo.List();
			return View(zaidejai);
		}

		/// <summary>
		/// This is invoked when creation form is first opened in browser.
		/// </summary>
		/// <returns>Creation form view.</returns>
		public ActionResult Create()
		{
			var zaidejasEvm = new ZaidejasEditVM();
			PopulateSelections(zaidejasEvm);
			return View(zaidejasEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="zaidejasEvm">Entity model filled with latest data.</param>
		/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(ZaidejasEditVM zaidejasEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				ZaidejasRepo.Insert(zaidejasEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(zaidejasEvm);
			return View(zaidejasEvm);
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(int id)
		{
			var zaidejasEvm = ZaidejasRepo.Find(id);
			PopulateSelections(zaidejasEvm);

			return View(zaidejasEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="zaidejasEvm">Entity model filled with latest data.</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(int id, ZaidejasEditVM zaidejasEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				ZaidejasRepo.Update(zaidejasEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(zaidejasEvm);
			return View(zaidejasEvm);
		}

		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(int id)
		{
			var zaidejasLvm = ZaidejasRepo.FindForDeletion(id);
			return View(zaidejasLvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(int id)
		{
			//try deleting, this will fail if foreign key constraint fails
			try
			{
				ZaidejasRepo.Delete(id);

				//deletion success, redired to list form
				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var zaidejasLvm = ZaidejasRepo.FindForDeletion(id);

				return View("Delete", zaidejasLvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="zaidejasEvm">'Automobilis' view model to append to.</param>
		public void PopulateSelections(ZaidejasEditVM zaidejasEvm)
		{
			//load entities for the select lists
			var salys = SalisRepo.List();

			//build select lists
			zaidejasEvm.Lists.Salys =
				salys.Select(it => {
					return
						new SelectListItem() {
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

			List<string> pozicijos = new List<string>{"puolejas", "gynejas", "saugas", "vartininkas"};
			
			zaidejasEvm.Lists.Pozicijos = 
				pozicijos.Select(it => {
					return
						new SelectListItem() {
							Value = it,
							Text = it
						};
				}).ToList();
		}
	}
}
