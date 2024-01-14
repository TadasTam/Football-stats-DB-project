using System.Data;
using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories
{
	/// <summary>
	/// Database operations related to 'Ivartis' entity.
	/// </summary>
	public class IvartisRepo
	{
		public static List<RungtynesEditVM.IvartisM> List(DateTime data, string seim, string svec)
		{
			var ivarciai = new List<RungtynesEditVM.IvartisM>();

			var query =
				$@"SELECT *
				FROM `{Config.TblPrefix}ivarciai`
				WHERE fk_seimininkai=?seim AND fk_sveciai=?svec AND fk_data=?data";

			var dt =
				Sql.Query(query, args => {
                    args.Add("?data", MySqlDbType.Date).Value = data;
                    args.Add("?seim", MySqlDbType.VarChar).Value = seim;
                    args.Add("?svec", MySqlDbType.VarChar).Value = svec;
				});

			var inListId = 0;

			foreach( DataRow item in dt )
			{
				ivarciai.Add(new RungtynesEditVM.IvartisM
				{
					InListId = inListId,

					Minute = Convert.ToInt32(item["minute"]),
					Savus = Convert.ToBoolean(item["ar_i_savus_vartus"]),
					Baudinys = Convert.ToBoolean(item["ar_baudinys"]),
					KuriKom = Convert.ToString(item["uz_kuria_komanda"]),
					FkZaidejas = Convert.ToInt32(item["fk_zaidejas"])
				});

				//advance in list ID for next entry
				inListId += 1;
			}

			return ivarciai;
		}

		public static void Insert(DateTime data, string seim, string svec, RungtynesEditVM.IvartisM up)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}ivarciai`
					(
						minute,
						ar_i_savus_vartus,
						ar_baudinys,
						uz_kuria_komanda,
						fk_seimininkai,
						fk_sveciai,
						fk_data,
						fk_zaidejas
					)
					VALUES(
						?minute,
						?ar_i_savus_vartus,
						?ar_baudinys,
						?uz_kuria_komanda,
						?fk_seimininkai,
						?fk_sveciai,
						?fk_data,
						?fk_zaidejas
					)";

			Sql.Insert(query, args => {
				args.Add("?minute", MySqlDbType.Int32).Value = up.Minute;
				args.Add("?ar_i_savus_vartus", MySqlDbType.Int32).Value = (up.Savus ? 1 : 0);
				args.Add("?ar_baudinys", MySqlDbType.Int32).Value = (up.Baudinys ? 1 : 0);
				args.Add("?uz_kuria_komanda", MySqlDbType.VarChar).Value = up.KuriKom;
				args.Add("?fk_seimininkai", MySqlDbType.VarChar).Value = seim;
				args.Add("?fk_sveciai", MySqlDbType.VarChar).Value = svec;
				args.Add("?fk_data", MySqlDbType.Date).Value = data;
				args.Add("?fk_zaidejas", MySqlDbType.Int32).Value = up.FkZaidejas;
			});
		}

		public static void DeleteForRungtynes(DateTime data, string seim, string svec)
		{
			var query =
				$@"DELETE FROM a
				USING `{Config.TblPrefix}ivarciai` as a
				WHERE fk_seimininkai=?seim AND fk_sveciai=?svec AND fk_data=?data";

			Sql.Delete(query, args => {
                    args.Add("?data", MySqlDbType.Date).Value = data;
                    args.Add("?seim", MySqlDbType.VarChar).Value = seim;
                    args.Add("?svec", MySqlDbType.VarChar).Value = svec;
			});
		}
	}
}