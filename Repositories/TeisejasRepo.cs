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
	public class TeisejasRepo
	{
		public static List<TeisejasListVM> List()
		{
			var result = new List<TeisejasListVM>();

			var query =
				$@"SELECT
					md.vardas,
					md.pavarde,
					md.id,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}teisejai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				ORDER BY md.vardas ASC";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new TeisejasListVM
				{
					Vardas = Convert.ToString(item["vardas"]),
					Pavarde = Convert.ToString(item["pavarde"]),
					Id = Convert.ToInt32(item["id"]),
					Salis = Convert.ToString(item["salis"]),
				});
			}

			return result;
		}

		public static TeisejasEditVM Find(int id)
		{
			var mevm = new TeisejasEditVM();

			var query = $@"SELECT * FROM `{Config.TblPrefix}teisejai` WHERE id=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mevm.Model.Vardas = Convert.ToString(item["vardas"]);
				mevm.Model.FkSalis = Convert.ToInt32(item["fk_salis"]);
				mevm.Model.Pavarde = Convert.ToString(item["pavarde"]);
				mevm.Model.Id = Convert.ToInt32(item["id"]);
			}

			return mevm;
		}

		public static TeisejasListVM FindForDeletion(int id)
		{
			var mlvm = new TeisejasListVM();

			var query =
				$@"SELECT
					md.vardas,
					md.pavarde,
					md.id,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}teisejai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				WHERE
					md.id=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mlvm.Vardas = Convert.ToString(item["vardas"]);
				mlvm.Salis = Convert.ToString(item["salis"]);
				mlvm.Pavarde = Convert.ToString(item["pavarde"]);
				mlvm.Id = Convert.ToInt32(item["id"]);
			}

			return mlvm;
		}

		public static void Update(TeisejasEditVM TeisejasEvm)
		{

			var query =
				$@"UPDATE `{Config.TblPrefix}teisejai`
				SET
					vardas=?vardas,
					pavarde=?pavarde,
					fk_salis=?salis
				WHERE
					id=?id";

			Sql.Update(query, args => {
				args.Add("?vardas", MySqlDbType.VarChar).Value = TeisejasEvm.Model.Vardas;
				args.Add("?pavarde", MySqlDbType.VarChar).Value = TeisejasEvm.Model.Pavarde;
				args.Add("?id", MySqlDbType.Int32).Value = TeisejasEvm.Model.Id;
				args.Add("?salis", MySqlDbType.VarChar).Value = TeisejasEvm.Model.FkSalis;
			});
		}

		public static void Insert(TeisejasEditVM TeisejasEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}teisejai`
				(
					id,
					vardas,
					pavarde,
					fk_salis
				)
				VALUES(
					?id,
					?vardas,
					?pavarde,
					?salis
				)";

			Sql.Insert(query, args => {
				args.Add("?vardas", MySqlDbType.VarChar).Value = TeisejasEvm.Model.Vardas;
				args.Add("?pavarde", MySqlDbType.VarChar).Value = TeisejasEvm.Model.Pavarde;
				args.Add("?id", MySqlDbType.Int32).Value = findNextIndex();
				args.Add("?salis", MySqlDbType.VarChar).Value = TeisejasEvm.Model.FkSalis;
			});
		}
		
		public static int findNextIndex()
		{
			int id = 0;
			var query = $@"SELECT MAX(id) AS MaxId
			FROM `{Config.TblPrefix}teisejai` 
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
			var query = $@"DELETE FROM `{Config.TblPrefix}teisejai` WHERE id=?id";

			Sql.Delete(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
			});
		}
	}
}