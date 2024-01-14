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
	public class MiestasRepo
	{
		public static List<MiestasListVM> List()
		{
			var result = new List<MiestasListVM>();

			var query =
				$@"SELECT
					md.id,
					md.pavadinimas,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}miestai` md
				LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new MiestasListVM
				{
					Id = Convert.ToInt32(item["id"]),
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Salis = Convert.ToString(item["salis"])
				});
			}

			return result;
		}

		public static MiestasEditVM Find(int id)
		{
			var result = new MiestasEditVM();
			var mi = result.Miestai;

			var query = $@"SELECT * FROM `{Config.TblPrefix}miestai` WHERE id=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mi.Id = Convert.ToInt32(item["id"]);
				mi.Pavadinimas = Convert.ToString(item["pavadinimas"]);
				mi.FkSalis = Convert.ToInt32(item["fk_salis"]);
			}

			return result;
		}

		public static MiestasListVM FindForDeletion(int id)
		{
			var mlvm = new MiestasListVM();

			var query =
				$@"SELECT
					md.id,
					md.pavadinimas,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}miestai` md
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
				mlvm.Pavadinimas = Convert.ToString(item["pavadinimas"]);
				mlvm.Salis = Convert.ToString(item["salis"]);
			}

			return mlvm;
		}

		public static void Update(MiestasEditVM miestasEvm)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}miestai`
				SET
					pavadinimas=?pavadinimas,
					fk_salis=?salis
				WHERE
					id=?id";

			Sql.Update(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = miestasEvm.Miestai.Pavadinimas;
				args.Add("?salis", MySqlDbType.VarChar).Value = miestasEvm.Miestai.FkSalis;
				args.Add("?id", MySqlDbType.VarChar).Value = miestasEvm.Miestai.Id;
			});
		}

		public static int Insert(MiestasEditVM miestasEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}miestai`
				(
					pavadinimas,
					fk_salis,
					id
				)
				VALUES(
					?pavadinimas,
					?salis,
					?id
				)";

			int nextId = findNextIndex();

			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = miestasEvm.Miestai.Pavadinimas;
				args.Add("?salis", MySqlDbType.VarChar).Value = miestasEvm.Miestai.FkSalis;
				args.Add("?id", MySqlDbType.VarChar).Value = nextId;
			});

			return nextId;
		}

		public static int findNextIndex()
		{
			int id = 0;
			var query = $@"SELECT MAX(id) AS MaxId
			FROM `{Config.TblPrefix}miestai` 
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
			var query = $@"DELETE FROM `{Config.TblPrefix}miestai` WHERE id=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.Int32).Value = id;
			});
		}
	}
}