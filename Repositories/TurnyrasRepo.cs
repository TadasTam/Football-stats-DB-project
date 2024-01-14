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
	public class TurnyrasRepo
	{
		public static List<TurnyrasListVM> List()
		{
			var result = new List<TurnyrasListVM>();

			var query =
				$@"SELECT
					md.pavadinimas,
					md.sezonas,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}turnyrai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				ORDER BY md.pavadinimas ASC, mark.pavadinimas ASC";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new TurnyrasListVM
				{
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Sezonas = Convert.ToString(item["sezonas"]),
					Salis = Convert.ToString(item["salis"]),
				});
			}

			return result;
		}

		public static TurnyrasEditVM Find(string pavadinimas, string sezonas, string salis)
		{
			var mevm = new TurnyrasEditVM();

			var query = $@"SELECT
					md.pavadinimas,
					md.sezonas,
					md.fk_salis,
					md.nacionalines_rinktines,
					md.klubai,
					md.tipas,
					md.galimu_keitimu_skaicius,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}turnyrai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				WHERE 
				md.pavadinimas=?pavadinimas AND md.sezonas=?sezonas AND mark.pavadinimas=?salis";

			var dt =
				Sql.Query(query, args => {
					args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pavadinimas;
					args.Add("?sezonas", MySqlDbType.VarChar).Value = sezonas;
					args.Add("?salis", MySqlDbType.VarChar).Value = salis;
				});

			foreach( DataRow item in dt )
			{
				mevm.Model.Pavadinimas = Convert.ToString(item["pavadinimas"]);
				mevm.Model.Sezonas = Convert.ToString(item["sezonas"]);
				mevm.Model.FkSalis = Convert.ToInt32(item["fk_salis"]);
				mevm.Model.Rinktines = Convert.ToBoolean(item["nacionalines_rinktines"]);
				mevm.Model.Klubai = Convert.ToBoolean(item["klubai"]);
				mevm.Model.Tipas = Convert.ToString(item["tipas"]);
				mevm.Model.Keitimai = Convert.ToString(item["galimu_keitimu_skaicius"]);
			}

			return mevm;
		}

		public static TurnyrasListVM FindForDeletion(string pavadinimas, string sezonas, string salis)
		{
			var mlvm = new TurnyrasListVM();

			var query =
				$@"SELECT
					md.pavadinimas,
					md.sezonas,
					mark.pavadinimas AS salis
				FROM
					`{Config.TblPrefix}turnyrai` md
					LEFT JOIN `{Config.TblPrefix}salys` mark ON mark.id=md.fk_salis
				WHERE 
				md.pavadinimas=?pavadinimas AND md.sezonas=?sezonas AND mark.pavadinimas=?salis";

			var dt =
				Sql.Query(query, args => {
					args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pavadinimas;
					args.Add("?sezonas", MySqlDbType.VarChar).Value = sezonas;
					args.Add("?salis", MySqlDbType.VarChar).Value = salis;
				});

			foreach( DataRow item in dt )
			{
				mlvm.Pavadinimas = Convert.ToString(item["pavadinimas"]);
				mlvm.Sezonas = Convert.ToString(item["sezonas"]);
				mlvm.Salis = Convert.ToString(item["salis"]);
			}

			return mlvm;
		}

		public static void Update(TurnyrasEditVM turnyrasEvm)
		{

			var query =
				$@"UPDATE `{Config.TblPrefix}turnyrai`
				SET
					nacionalines_rinktines=?nacionalines_rinktines,
					klubai=?klubai,
					tipas=?tipas,
					galimu_keitimu_skaicius=?galimu_keitimu_skaicius
				WHERE
					fk_salis=?salis AND pavadinimas=?pavadinimas AND sezonas=?sezonas";

			Sql.Update(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Pavadinimas;
				args.Add("?sezonas", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Sezonas;
				args.Add("?salis", MySqlDbType.VarChar).Value = turnyrasEvm.Model.FkSalis;

				args.Add("?nacionalines_rinktines", MySqlDbType.Int32).Value = (turnyrasEvm.Model.Rinktines ? 1 : 0);
				args.Add("?klubai", MySqlDbType.Int32).Value = (turnyrasEvm.Model.Klubai ? 1 : 0);
				args.Add("?tipas", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Tipas;
				args.Add("?galimu_keitimu_skaicius", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Keitimai;
			});
		}

		public static void Insert(TurnyrasEditVM turnyrasEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}turnyrai`
				(
					pavadinimas,
					sezonas,
					fk_salis,
					nacionalines_rinktines,
					klubai,
					tipas,
					galimu_keitimu_skaicius
				)
				VALUES(
					?pavadinimas,
					?sezonas,
					?salis,
					?nacionalines_rinktines,
					?klubai,
					?tipas,
					?galimu_keitimu_skaicius
				)";

			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Pavadinimas;
				args.Add("?sezonas", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Sezonas;
				args.Add("?salis", MySqlDbType.VarChar).Value = turnyrasEvm.Model.FkSalis;
				args.Add("?nacionalines_rinktines", MySqlDbType.Int32).Value = (turnyrasEvm.Model.Rinktines ? 1 : 0);
				args.Add("?klubai", MySqlDbType.Int32).Value = (turnyrasEvm.Model.Klubai ? 1 : 0);
				args.Add("?tipas", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Tipas;
				args.Add("?galimu_keitimu_skaicius", MySqlDbType.VarChar).Value = turnyrasEvm.Model.Keitimai;
			});
		}

		public static void Delete(string pavadinimas, string sezonas, string salis)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}turnyrai` WHERE pavadinimas=?pavadinimas AND sezonas=?sezonas AND fk_salis=?salis";

			Sql.Delete(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pavadinimas;
				args.Add("?sezonas", MySqlDbType.VarChar).Value = sezonas;
				args.Add("?salis", MySqlDbType.Int32).Value = FindSalisId(salis);
			});
		}

		public static int FindSalisId(string salis)
		{
			int id=0;

			var query = $@"SELECT * FROM `{Config.TblPrefix}salys` WHERE pavadinimas=?salis";

			var dt =
				Sql.Query(query, args => {
					args.Add("?salis", MySqlDbType.VarChar).Value = salis;
				});

			foreach( DataRow item in dt )
			{
				id = Convert.ToInt32(item["id"]);
			}

			return id;
		}
	}
}