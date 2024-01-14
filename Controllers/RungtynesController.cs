using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
	/// <summary>
	/// Controller for working with 'Rungtynes' entity.
	/// </summary>
	public class RungtynesController : Controller
	{
		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			return View(RungtynesRepo.List());
		}

		/// <summary>
		/// This is invoked when creation form is first opened in a browser.
		/// </summary>
		/// <returns>Entity creation form.</returns>
		public ActionResult Create()
		{
			var rungtynesEvm = new RungtynesEditVM();
			PopulateLists(rungtynesEvm);

			return View(rungtynesEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="rungtynesEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns creation from view or redirets back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(int? save, int? add, int? remove, RungtynesEditVM rungtynesEvm)
		{
			//addition of new 'Ivartis' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new RungtynesEditVM.IvartisM {
						InListId =
							rungtynesEvm.Ivarciai.Count > 0 ?
							rungtynesEvm.Ivarciai.Max(it => it.InListId) + 1 :
							0,

						FkZaidejas = 0,
						Minute = 0,
						Savus = false,
						Baudinys = false,
						KuriKom = "seimininku"
					};

				

				rungtynesEvm.Ivarciai.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(rungtynesEvm);
				return View(rungtynesEvm);
			}

			//removal of existing 'Ivartis' record was requested?
			if( remove != null )
			{
				//filter out 'Ivartis' record having in-list-id the same as the given one
				rungtynesEvm.Ivarciai =
					rungtynesEvm
						.Ivarciai
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(rungtynesEvm);
				return View(rungtynesEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				var match = RungtynesRepo.Find(rungtynesEvm.Rungtynes.FaktineData, rungtynesEvm.Rungtynes.FkSeimininkas, rungtynesEvm.Rungtynes.FkSvecias);
				if (match!=null)
				{
					ModelState.AddModelError("faktine_data", "Field value already exists in database.");
					ModelState.AddModelError("fk_seimininkai", "Field value already exists in database.");
					ModelState.AddModelError("fk_sveciai", "Field value already exists in database.");
				}

				//form field validation passed?
				if( ModelState.IsValid )
				{
					//create new 'Sutartis'
					RungtynesRepo.Insert(rungtynesEvm);

					//create new 'Ivartis' records
					foreach( var upVm in rungtynesEvm.Ivarciai )
						IvartisRepo.Insert(rungtynesEvm.Rungtynes.FaktineData, rungtynesEvm.Rungtynes.FkSeimininkas, rungtynesEvm.Rungtynes.FkSvecias, upVm);

					//save success, go back to the entity list
					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateLists(rungtynesEvm);
					return View(rungtynesEvm);
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
		public ActionResult Edit(DateTime data, string seim, string svec)
		{
			var rungtynesEvm = RungtynesRepo.Find(data, seim, svec);
			
			rungtynesEvm.Ivarciai = IvartisRepo.List(data, seim, svec);			
			PopulateLists(rungtynesEvm);

			return View(rungtynesEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="rungtynesEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns editing from view or redired back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(int id, int? save, int? add, int? remove, RungtynesEditVM rungtynesEvm)
		{
			//addition of new 'Ivartis' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new RungtynesEditVM.IvartisM {
						InListId =
							rungtynesEvm.Ivarciai.Count > 0 ?
							rungtynesEvm.Ivarciai.Max(it => it.InListId) + 1 :
							0,

						FkZaidejas = 0,
						Minute = 0,
						Savus = false,
						Baudinys = false,
						KuriKom = "seimininku"
					};
				rungtynesEvm.Ivarciai.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(rungtynesEvm);
				return View(rungtynesEvm);
			}

			//removal of existing 'Ivartis' record was requested?
			if( remove != null )
			{
				//filter out 'Ivartis' record having in-list-id the same as the given one
				rungtynesEvm.Ivarciai =
					rungtynesEvm
						.Ivarciai
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(rungtynesEvm);
				return View(rungtynesEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{
					//update 'Sutartis'
					RungtynesRepo.Update(rungtynesEvm);

					//delete all old 'Ivartis' records
					IvartisRepo.DeleteForRungtynes(rungtynesEvm.Rungtynes.FaktineData, rungtynesEvm.Rungtynes.FkSeimininkas, rungtynesEvm.Rungtynes.FkSvecias);

					//create new 'Ivartis' records
					foreach( var upVm in rungtynesEvm.Ivarciai )
					{
						try
						{
							IvartisRepo.Insert(rungtynesEvm.Rungtynes.FaktineData, rungtynesEvm.Rungtynes.FkSeimininkas, rungtynesEvm.Rungtynes.FkSvecias, upVm);
						}
						catch (MySql.Data.MySqlClient.MySqlException)
						{

						}
						
					}						

					//save success, go back to the entity list
					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateLists(rungtynesEvm);
					return View(rungtynesEvm);
				}
			}

			//should not reach here
			throw new Exception("Should not reach here.");
		}

		/// <summary>
		/// This is invoked when deletion form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(DateTime data, string seim, string svec)
		{
			var rungtynesEvm = RungtynesRepo.Find(data, seim, svec);
			return View(rungtynesEvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(DateTime data, string seim, string svec)
		{
			//load 'Rungtynes'
			var rungtynesEvm = RungtynesRepo.Find(data, seim, svec);
			
			IvartisRepo.DeleteForRungtynes(data, seim, svec);
			RungtynesRepo.Delete(data, seim, svec);

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="rungtynesEvm">'Rungtynes' view model to append to.</param>
		private void PopulateLists(RungtynesEditVM rungtynesEvm)
		{
			rungtynesEvm.Lists.Stadionai =
				StadionasRepo.ListWithCity().Select(it => {
					string miestas = it.Miestas;
					return new SelectListItem() {
							Value = it.Pavadinimas + ";" + StadionasRepo.FindCityId(it.Miestas),
							Text = Convert.ToString(it.Pavadinimas) + " (" + miestas + ")"
					};
				})
				.ToList();

				rungtynesEvm.Lists.Teisejai =
				TeisejasRepo.List().Select(it => {
					return new SelectListItem() {
							Value = Convert.ToString(it.Id),
							Text = it.Vardas + " " + it.Pavarde
					};
				})
				.ToList();

				rungtynesEvm.Lists.Komandos =
				KomandaRepo.List().Select(it => {
					return new SelectListItem() {
							Value = it.Pavadinimas,
							Text = it.Pavadinimas
					};
				})
				.ToList();
				
				rungtynesEvm.Lists.Turnyrai =
				TurnyrasRepo.List().Select(it => {
					string salis = it.Salis;
					return new SelectListItem() {
							Value = StadionasRepo.FindCountryId(it.Salis) + ";" + it.Sezonas + ";" + it.Pavadinimas,
							Text = it.Pavadinimas + " (" + salis + ") " + it.Sezonas
					};
				})
				.ToList();

			//build select list for 'Ivartis'
			{
				rungtynesEvm.Lists.Zaidejai =
				ZaidejasRepo.List().Select(it => {
					return new SelectListItem() {
							Value = Convert.ToString(it.Id),
							Text = it.Vardas
					};
				})
				.ToList();
				
			List<string> kuria = new List<string>{"seimininku", "sveciu"};
			
			rungtynesEvm.Lists.UzKuria = 
				kuria.Select(it => {
					return
						new SelectListItem() {
							Value = it,
							Text = it
						};
				}).ToList();
			}


		}
	}
}