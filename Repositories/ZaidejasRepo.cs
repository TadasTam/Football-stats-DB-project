using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories
{
	public class ZaidejasRepo
	{
		public static List<ZaidejasListVM> List()
		{
			var result = new List<ZaidejasListVM>();

			var query =
				$@"SELECT
					md.id,
					md.vardas,
					md.pavarde,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}zaidejai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				ORDER BY md.pavarde ASC, md.vardas ASC";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new ZaidejasListVM
				{
					Id = Convert.ToInt32(item["id"]),
					Vardas = Convert.ToString(item["vardas"]) + " " + Convert.ToString(item["pavarde"]),
					Salis = Convert.ToString(item["salis"])
				});
			}

			return result;
		}

		public static ZaidejasEditVM Find(int id)
		{
			var mevm = new ZaidejasEditVM();

			var query = $@"SELECT * FROM `{Config.TblPrefix}zaidejai` WHERE id=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mevm.Model.Id = Convert.ToInt32(item["id"]);
				mevm.Model.Vardas = Convert.ToString(item["vardas"]);
				mevm.Model.Pavarde = Convert.ToString(item["pavarde"]);
				mevm.Model.Zinomas = Convert.ToString(item["zinomas_kaip"]);
				mevm.Model.Data = Convert.ToDateTime(item["gimimo_data"]);
				mevm.Model.Pozicija = Convert.ToString(item["pozicija"]);
				mevm.Model.FkSalis = Convert.ToInt32(item["fk_salis"]);
			}

			return mevm;
		}

		public static ZaidejasListVM FindForDeletion(int id)
		{
			var mlvm = new ZaidejasListVM();

			var query =
				$@"SELECT
					md.id,
					md.vardas,
					md.pavarde,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}zaidejai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				WHERE
					md.id = ?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mlvm.Id = Convert.ToInt32(item["id"]);
				mlvm.Vardas = Convert.ToString(item["vardas"]) + " " + Convert.ToString(item["pavarde"]);
				mlvm.Salis = Convert.ToString(item["salis"]);
			}

			return mlvm;
		}

		public static void Update(ZaidejasEditVM ZaidejasEvm)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}zaidejai`
				SET
					vardas=?vardas,
					pavarde=?pavarde,
					zinomas_kaip=?zinomas,
					gimimo_data=?data,
					pozicija=?pozicija,
					fk_salis=?salis
				WHERE
					id=?id";

			Sql.Update(query, args => {
				args.Add("?vardas", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Vardas;
				args.Add("?pavarde", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Pavarde;
				args.Add("?zinomas", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Zinomas;
				args.Add("?data", MySqlDbType.DateTime).Value = ZaidejasEvm.Model.Data;
				args.Add("?pozicija", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Pozicija;

				args.Add("?salis", MySqlDbType.Int32).Value = ZaidejasEvm.Model.FkSalis;


				args.Add("?id", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Id;
			});
		}

		public static void Insert(ZaidejasEditVM ZaidejasEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}zaidejai`
				(
					vardas,
					pavarde,
					zinomas_kaip,
					gimimo_data,
					pozicija,
					fk_salis,
					id
				)
				VALUES(
					?vardas,
					?pavarde,
					?zinomas,
					?data,
					?pozicija,
					?salis,
					?id
				)";

			Sql.Insert(query, args => {
				args.Add("?vardas", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Vardas;
				args.Add("?pavarde", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Pavarde;
				args.Add("?zinomas", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Zinomas;
				args.Add("?data", MySqlDbType.DateTime).Value = ZaidejasEvm.Model.Data;
				args.Add("?pozicija", MySqlDbType.VarChar).Value = ZaidejasEvm.Model.Pozicija;
				args.Add("?salis", MySqlDbType.Int32).Value = ZaidejasEvm.Model.FkSalis;
				args.Add("?id", MySqlDbType.Int32).Value = findNextIndex();
			});
		}
		
		public static int findNextIndex()
		{
			int id = 0;
			var query = $@"SELECT MAX(id) AS MaxId
			FROM `{Config.TblPrefix}zaidejai` 
			";

			var dt = Sql.Query(query, args => {
			});

			foreach( DataRow item in dt )
			{
				id = Convert.ToInt32(item["MaxId"]) + 1;
			}

			return id;
		}

		public static void Delete(int id)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}zaidejai` WHERE id=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.Int32).Value = id;
			});
		}
	}
}