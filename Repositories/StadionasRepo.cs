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
	public class StadionasRepo
	{
		public static List<MiestasEditVM.StadionasM> List(int miestas)
		{
			var result = new List<MiestasEditVM.StadionasM>();

			var query =
				$@"SELECT *
				FROM `{Config.TblPrefix}stadionai` 
				WHERE fk_miestas=?miestas";

			var dt =
				Sql.Query(query, args => {
                    args.Add("?miestas", MySqlDbType.Int32).Value = miestas;
				});

			var inListId = 0;

			foreach( DataRow item in dt )
			{
				result.Add(new MiestasEditVM.StadionasM
				{
					InListId = inListId,

					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Talpa = Convert.ToInt32(item["talpa"]),
					Adresas = Convert.ToString(item["adresas"]),
					Metai = Convert.ToInt32(item["metai"]),
					Renovuotas = Sql.AllowNull(item["renovuotas"], it => (int?)Convert.ToInt32(it)),
				});

				inListId += 1;
			}

			return result;
		}

		public static List<MiestasEditVM.StadionasM> List()
		{
			var result = new List<MiestasEditVM.StadionasM>();

			var query =
				$@"SELECT *
				FROM `{Config.TblPrefix}stadionai`";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new MiestasEditVM.StadionasM
				{
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Talpa = Convert.ToInt32(item["talpa"]),
					Adresas = Convert.ToString(item["adresas"]),
					Metai = Convert.ToInt32(item["metai"]),
					Renovuotas = Sql.AllowNull(item["renovuotas"], it => (int?)Convert.ToInt32(it)),
				});
			}

			return result;
		}

		public static List<StadionasListVM> ListWithCity()
		{
			var result = new List<StadionasListVM>();

			var query =
				$@"SELECT
					md.pavadinimas,
					md.talpa,
					md.adresas,
					md.metai,
					md.renovuotas,
					mark.pavadinimas AS miestas
				FROM
					`{Config.TblPrefix}stadionai` md
					LEFT JOIN `{Config.TblPrefix}miestai` mark ON mark.id=md.fk_miestas
				ORDER BY mark.pavadinimas ASC, md.pavadinimas ASC";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new StadionasListVM
				{
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Miestas = Convert.ToString(item["miestas"]),
					Talpa = Convert.ToInt32(item["talpa"]),
					Adresas = Convert.ToString(item["adresas"]),
					Metai = Convert.ToInt32(item["metai"]),
					Renovuotas = Sql.AllowNull(item["renovuotas"], it => (int?)Convert.ToInt32(it)),
				});
			}

			return result;
		}

		public static int FindCityId(string miestas)
		{
			int id=0;

			var query = $@"SELECT * FROM `{Config.TblPrefix}miestai` WHERE pavadinimas=?miestas";

			var dt =
				Sql.Query(query, args => {
					args.Add("?miestas", MySqlDbType.VarChar).Value = miestas;
				});

			foreach( DataRow item in dt )
			{
				id = Convert.ToInt32(item["id"]);
			}

			return id;
		}

		public static int FindCountryId(string salis)
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

		public static void Insert(int miestas, MiestasEditVM.StadionasM stadionasEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}stadionai`
				(
					pavadinimas,
					fk_miestas,
					talpa,
					adresas,
					metai,
					renovuotas
				)
				VALUES(
					?pavadinimas,
					?miestas,
					?talpa,
					?adresas,
					?metai,
					?renovuotas
				)";

			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = stadionasEvm.Pavadinimas;
				args.Add("?miestas", MySqlDbType.VarChar).Value = miestas;
				args.Add("?talpa", MySqlDbType.Int32).Value = stadionasEvm.Talpa;
				args.Add("?adresas", MySqlDbType.VarChar).Value = stadionasEvm.Adresas;
				args.Add("?metai", MySqlDbType.Int32).Value = stadionasEvm.Metai;
				args.Add("?renovuotas", MySqlDbType.Int32).Value = stadionasEvm.Renovuotas;
			});
		}

		public static void InsertAndUpdate(int miestas, MiestasEditVM.StadionasM stadionasEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}stadionai`
				(
					pavadinimas,
					fk_miestas,
					talpa,
					adresas,
					metai,
					renovuotas
				)
				VALUES(
					?pavadinimas,
					?miestas,
					?talpa,
					?adresas,
					?metai,
					?renovuotas
				)
				ON DUPLICATE KEY UPDATE
				talpa=?talpa,
				adresas=?adresas,
				metai=?metai,
				renovuotas=?renovuotas";

			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = stadionasEvm.Pavadinimas;
				args.Add("?miestas", MySqlDbType.VarChar).Value = miestas;
				args.Add("?talpa", MySqlDbType.Int32).Value = stadionasEvm.Talpa;
				args.Add("?adresas", MySqlDbType.VarChar).Value = stadionasEvm.Adresas;
				args.Add("?metai", MySqlDbType.Int32).Value = stadionasEvm.Metai;
				args.Add("?renovuotas", MySqlDbType.Int32).Value = stadionasEvm.Renovuotas;
			});
		}
		
		public static void Delete(string pav, int miestas)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}stadionai` WHERE pavadinimas=?pavadinimas AND fk_miestas=?miestas";

			Sql.Delete(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pav;
				args.Add("?miestas", MySqlDbType.VarChar).Value = miestas;
			});
		}

		public static void DeleteForMiestas(int miestas)
		{
			var query = $@"DELETE FROM a
			USING `{Config.TblPrefix}stadionai` as a
			WHERE fk_miestas=?miestas";

			Sql.Delete(query, args => {
				args.Add("?miestas", MySqlDbType.VarChar).Value = miestas;
			});
		}

		public static bool DeleteRemoved(int miestas, MiestasEditVM shouldExist)
		{
			bool deletedAll = true;
			List<MiestasEditVM.StadionasM> InDb = List(miestas);
			List<MiestasEditVM.StadionasM> ShouldDelete = new List<MiestasEditVM.StadionasM>();

			foreach (MiestasEditVM.StadionasM stad in InDb)
				if(!shouldContain(shouldExist, stad))
					ShouldDelete.Add(stad);

			foreach(MiestasEditVM.StadionasM stad in ShouldDelete)
				try
				{
					Delete(stad.Pavadinimas, miestas);
				}
				catch (MySql.Data.MySqlClient.MySqlException)
				{
					deletedAll = false;
				}

			return deletedAll;	
		}

		public static bool shouldContain(MiestasEditVM shouldExist, MiestasEditVM.StadionasM searched)
		{
				foreach( MiestasEditVM.StadionasM existing in shouldExist.Stadionai )
					if (existing.Pavadinimas.Equals(searched.Pavadinimas))
						return true;
				return false;
		}
	}
}