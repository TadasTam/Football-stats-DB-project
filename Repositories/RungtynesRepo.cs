using MySql.Data.MySqlClient;
using System.Data;

using Org.Ktu.Isk.P175B602.Autonuoma.ViewModels;

namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories
{
	/// <summary>
    /// Database operations related to 'Rungtynes' entity.
    /// </summary>
	public class RungtynesRepo
	{
		public static List<RungtynesListVM> List()
		{
			var result = new List<RungtynesListVM>();
			
			var query = 
				$@"SELECT 
					*
				FROM 
					`{Config.TblPrefix}rungtynes`";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new RungtynesListVM {
					FaktineData = Convert.ToDateTime(item["faktine_data"]),
					Seimininkai = Convert.ToString(item["fk_seimininkai"]),
					Sveciai = Convert.ToString(item["fk_sveciai"]),
				});
			}

			return result;
		}

		public static RungtynesEditVM Find(DateTime data, string seim, string svec)
		{
			var result = new RungtynesEditVM();
			
			string qlquery = $@"SELECT * FROM `{Config.TblPrefix}rungtynes` WHERE faktine_data=?data AND fk_seimininkai=?seim AND fk_sveciai=?svec";

			var dt = 
				Sql.Query(qlquery, args => {
                    args.Add("?data", MySqlDbType.Date).Value = data;
                    args.Add("?seim", MySqlDbType.VarChar).Value = seim;
                    args.Add("?svec", MySqlDbType.VarChar).Value = svec;
                });

			var sut = result.Rungtynes;

			foreach( DataRow item in dt )
			{
				sut.FaktineData = Convert.ToDateTime(item["faktine_data"]);
				sut.FkSeimininkas = Convert.ToString(item["fk_seimininkai"]);
				sut.FkSvecias = Convert.ToString(item["fk_sveciai"]);
				sut.NumatytaData = Sql.AllowNull(item["numatyta_data"], it => (DateTime?)Convert.ToDateTime(it));
				#nullable enable
				sut.NumatytasLaikas = Sql.AllowNull(item["numatytas_laikas"], it => (string?)Convert.ToString(it));
				#nullable disable
				sut.FaktinisLaikas = Convert.ToString(item["faktinis_laikas"]);
				sut.Ziurovai = Convert.ToInt32(item["ziurovu_skaicius"]);
				sut.Pratesimas = Convert.ToBoolean(item["zaistas_pratesimas"]);
				sut.Baudiniai = Convert.ToBoolean(item["zaista_baudiniu_serija"]);
				sut.Neutrali = Convert.ToBoolean(item["zaista_neutralioje_aiksteje"]);
				sut.FkStadionasIrMiestas = Convert.ToString(item["fk_stadionas"]) + ";" + Convert.ToInt32(item["fk_miestas"]);
				sut.FkTeisejas = Convert.ToInt32(item["fk_teisejas"]);
				sut.FkTurnyroInfo = Convert.ToInt32(item["fk_salis"]) + ";" + Convert.ToString(item["fk_sezonas"]) + ";" + Convert.ToString(item["fk_turnyras"]);
			}

			return result;
		}

		public static void Update(RungtynesEditVM evm)
		{
            var query = 
				$@"UPDATE `{Config.TblPrefix}rungtynes`
				SET
					`ivarciai_seimininku` = ?ivarciaiSeim,
					`ivarciai_sveciu` = ?ivarciaiSvec,
					`numatyta_data` = ?numatytaData,
					`numatytas_laikas` = ?numatytasLaikas,
					`faktinis_laikas` = ?faktinisLaikas,
					`ziurovu_skaicius` = ?ziurovai,
					`zaistas_pratesimas` = ?pratesimas,
					`zaista_baudiniu_serija` = ?baudiniai,
					`zaista_neutralioje_aiksteje` = ?neutrali,
					`fk_stadionas` = ?stadionas,
					`fk_miestas` = ?miestas,
					`fk_teisejas` = ?teisejas,
					`fk_salis` = ?salis,
					`fk_sezonas` = ?sezonas,
					`fk_turnyras` = ?turnyras
				WHERE faktine_data=?faktine_data AND fk_seimininkai=?seimininkai AND fk_sveciai=?sveciai";

			string[] stad = evm.Rungtynes.FkStadionasIrMiestas.Split(';');
			string[] turn = evm.Rungtynes.FkTurnyroInfo.Split(';');

            Sql.Update(query, args => {
				args.Add("?faktine_data", MySqlDbType.Date).Value = evm.Rungtynes.FaktineData;
				args.Add("?seimininkai", MySqlDbType.VarChar).Value = evm.Rungtynes.FkSeimininkas;
				args.Add("?sveciai", MySqlDbType.VarChar).Value = evm.Rungtynes.FkSvecias;
				
				args.Add("?numatytaData", MySqlDbType.Date).Value = evm.Rungtynes.NumatytaData;
				args.Add("?numatytasLaikas", MySqlDbType.VarChar).Value = evm.Rungtynes.NumatytasLaikas;
				args.Add("?faktinisLaikas", MySqlDbType.VarChar).Value = evm.Rungtynes.FaktinisLaikas;
				args.Add("?ziurovai", MySqlDbType.Int32).Value = evm.Rungtynes.Ziurovai;
				args.Add("?pratesimas", MySqlDbType.Int32).Value = (evm.Rungtynes.Pratesimas ? 1 : 0);
				args.Add("?baudiniai", MySqlDbType.Int32).Value = (evm.Rungtynes.Baudiniai ? 1 : 0);
				args.Add("?neutrali", MySqlDbType.Int32).Value = (evm.Rungtynes.Neutrali ? 1 : 0);

				args.Add("?stadionas", MySqlDbType.VarChar).Value = stad[0];
				args.Add("?miestas", MySqlDbType.Int32).Value = Convert.ToInt32(stad[1]);

				args.Add("?teisejas", MySqlDbType.Int32).Value = evm.Rungtynes.FkTeisejas;

				args.Add("?salis", MySqlDbType.Int32).Value = Convert.ToInt32(turn[0]);
				args.Add("?sezonas", MySqlDbType.VarChar).Value = turn[1];
				args.Add("?turnyras", MySqlDbType.VarChar).Value = turn[2];
            });
		}

		public static int Insert(RungtynesEditVM evm)
		{			
			var query = 
				$@"INSERT INTO `{Config.TblPrefix}rungtynes` 
				(
					`faktine_data`,
					`fk_seimininkai`,
					`fk_sveciai`,
					`ivarciai_seimininku`,
					`ivarciai_sveciu`,
					`numatyta_data`,
					`numatytas_laikas`,
					`faktinis_laikas`,
					`ziurovu_skaicius`,
					`zaistas_pratesimas`,
					`zaista_baudiniu_serija`,
					`zaista_neutralioje_aiksteje`,
					`fk_stadionas`,
					`fk_miestas`,
					`fk_teisejas`,
					`fk_salis`,
					`fk_sezonas`,
					`fk_turnyras`
				)
				VALUES(
					?faktine_data,
					?fk_seimininkai,
					?fk_sveciai,
					?ivarciai_seimininku,
					?ivarciai_sveciu,
					?numatyta_data,
					?numatytas_laikas,
					?faktinis_laikas,
					?ziurovu_skaicius,
					?zaistas_pratesimas,
					?zaista_baudiniu_serija,
					?zaista_neutralioje_aiksteje,
					?fk_stadionas,
					?fk_miestas,
					?fk_teisejas,
					?fk_salis,
					?fk_sezonas,
					?fk_turnyras
				)";
				
			string[] stad = evm.Rungtynes.FkStadionasIrMiestas.Split(';');
			string[] turn = evm.Rungtynes.FkTurnyroInfo.Split(';');

			var nr = 
				Sql.Insert(query, args => {
					args.Add("?faktine_data", MySqlDbType.Date).Value = evm.Rungtynes.FaktineData;
					args.Add("?fk_seimininkai", MySqlDbType.VarChar).Value = evm.Rungtynes.FkSeimininkas;
					args.Add("?fk_sveciai", MySqlDbType.VarChar).Value = evm.Rungtynes.FkSvecias;
					args.Add("?numatyta_data", MySqlDbType.Date).Value = evm.Rungtynes.NumatytaData;
					args.Add("?numatytas_laikas", MySqlDbType.VarChar).Value = evm.Rungtynes.NumatytasLaikas;
					args.Add("?faktinis_laikas", MySqlDbType.VarChar).Value = evm.Rungtynes.FaktinisLaikas;
					args.Add("?ziurovu_skaicius", MySqlDbType.Int32).Value = evm.Rungtynes.Ziurovai;
					args.Add("?zaistas_pratesimas", MySqlDbType.Int32).Value = (evm.Rungtynes.Pratesimas ? 1 : 0);
					args.Add("?zaista_baudiniu_serija", MySqlDbType.Int32).Value = (evm.Rungtynes.Baudiniai ? 1 : 0);
					args.Add("?zaista_neutralioje_aiksteje", MySqlDbType.Int32).Value = (evm.Rungtynes.Neutrali ? 1 : 0);

					args.Add("?fk_stadionas", MySqlDbType.VarChar).Value = stad[0];
					args.Add("?fk_miestas", MySqlDbType.Int32).Value =  Convert.ToInt32(stad[1]);
					args.Add("?fk_teisejas", MySqlDbType.Int32).Value = evm.Rungtynes.FkTeisejas;
					args.Add("?fk_salis", MySqlDbType.Int32).Value = Convert.ToInt32(turn[0]);
					args.Add("?fk_sezonas", MySqlDbType.VarChar).Value = turn[1];
					args.Add("?fk_turnyras", MySqlDbType.VarChar).Value = turn[2];
				});

			return (int)nr;
		}

		public static void Delete(DateTime data, string seim, string svec)
		{			
			var query = $@"DELETE FROM `{Config.TblPrefix}rungtynes` WHERE faktine_data=?faktine_data AND fk_seimininkai=?seimininkai AND fk_sveciai=?sveciai";
			Sql.Delete(query, args => {
				args.Add("?faktine_data", MySqlDbType.Date).Value = data;
				args.Add("?seimininkai", MySqlDbType.VarChar).Value = seim;
				args.Add("?sveciai", MySqlDbType.VarChar).Value = svec;
			});
		}
	}
}