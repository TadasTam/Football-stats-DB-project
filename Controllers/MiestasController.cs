using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
	/// <summary>
	/// Controller for working with 'Miestas' entity.
	/// </summary>
	public class MiestasController : Controller
	{

		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			return View(MiestasRepo.List());
		}

		/// <summary>
		/// This is invoked when creation form is first opened in browser.
		/// </summary>
		/// <returns>Creation form view.</returns>
		public ActionResult Create()
		{
			var miestasEvm = new MiestasEditVM();
			PopulateSelections(miestasEvm);
			return View(miestasEvm);
		}
		
		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="miestasEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns creation from view or redirets back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(int? save, int? add, int? remove, MiestasEditVM miestasEvm)
		{
			//addition of new 'Stadionas' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new MiestasEditVM.StadionasM {
						InListId =
							miestasEvm.Stadionai.Count > 0 ?
							miestasEvm.Stadionai.Max(it => it.InListId) + 1 :
							0,

						Pavadinimas = "",
						Talpa = 0,
						Adresas = "",
						Metai = 0,
						Renovuotas = null
					};
				miestasEvm.Stadionai.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateSelections(miestasEvm);
				return View(miestasEvm);
			}

			//removal of existing 'Stadionas' record was requested?
			if( remove != null )
			{
				//filter out 'Stadionas' record having in-list-id the same as the given one
				miestasEvm.Stadionai =
					miestasEvm
						.Stadionai
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateSelections(miestasEvm);
				return View(miestasEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{
					int id = MiestasRepo.Insert(miestasEvm);
					foreach( var upVm in miestasEvm.Stadionai )
						StadionasRepo.InsertAndUpdate(id, upVm);

					//save success, go back to the entity list
					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateSelections(miestasEvm);
					return View(miestasEvm);
				}
			}

			//should not reach here
			throw new Exception("Should not reach here.");
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(int id)
		{
			var miestasEvm = MiestasRepo.Find(id);
			miestasEvm.Stadionai = StadionasRepo.List(id);		
			PopulateSelections(miestasEvm);

			return View(miestasEvm);
		}
		
		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="miestasEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns editing from view or redired back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(int id, int? save, int? add, int? remove, MiestasEditVM miestasEvm)
		{
			//addition of new 'Stadionas' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new MiestasEditVM.StadionasM {
						InListId =
							miestasEvm.Stadionai.Count > 0 ?
							miestasEvm.Stadionai.Max(it => it.InListId) + 1 :
							0,

						Pavadinimas = "",
						Talpa = 0,
						Adresas = "",
						Metai = 0,
						Renovuotas = null
					};
				miestasEvm.Stadionai.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateSelections(miestasEvm);
				return View(miestasEvm);
			}

			//removal of existing 'Stadionas' record was requested?
			if( remove != null )
			{
				//filter out 'Stadionas' record having in-list-id the same as the given one
				miestasEvm.Stadionai =
					miestasEvm
						.Stadionai
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateSelections(miestasEvm);
				return View(miestasEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{
					MiestasRepo.Update(miestasEvm);

					foreach( var upVm in miestasEvm.Stadionai )
						StadionasRepo.InsertAndUpdate(miestasEvm.Miestai.Id, upVm);
					
					bool deletedAll = StadionasRepo.DeleteRemoved(miestasEvm.Miestai.Id, miestasEvm);	
					
					if (deletedAll == true)
					{
						return RedirectToAction("Index");
					}
					else
					{
						ViewData["StadionasDeletionNotPermitted"] = true;
						return View("Delete", MiestasRepo.FindForDeletion(id));
					}
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateSelections(miestasEvm);
					return View(miestasEvm);
				}
			}

			//should not reach here
			throw new Exception("Should not reach here.");
		}

		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(int id)
		{
			var miestasLvm = MiestasRepo.FindForDeletion(id);
			return View(miestasLvm);
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
				var miestasEvm = MiestasRepo.Find(id);

				StadionasRepo.DeleteForMiestas(id);
				MiestasRepo.Delete(id);

				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var miestasLvm = MiestasRepo.FindForDeletion(id);

				return View("Delete", miestasLvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="miestasEvm">'Automobilis' view model to append to.</param>
		public void PopulateSelections(MiestasEditVM miestasEvm)
		{
			//load entities for the select lists
			var salys = SalisRepo.List();

			//build select lists
			miestasEvm.Lists.Salys =
				salys.Select(it => {
					return
						new SelectListItem() {
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();
		}
	}
}
