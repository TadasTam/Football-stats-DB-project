using System.Data;
using MySql.Data.MySqlClient;

using RungtynesReport = Org.Ktu.Isk.P175B602.Autonuoma.ViewModels.RungtynesReport;


namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories
{
	/// <summary>
	/// Database operations related to reports.
	/// </summary>
	public class AtaskaitaRepo
	{

		public static List<RungtynesReport.Rungtynes> GetRungtynes(DateTime? dateFrom, DateTime? dateTo, int? salis, string? komanda)
		{
			var result = new List<RungtynesReport.Rungtynes>();
			
			var query =
				$@"SELECT
					rungtynes.faktine_data, rungtynes.fk_seimininkai, rungtynes.fk_sveciai,
					bs1.teisejas,
					turnyrai.pavadinimas, turnyrai.sezonas, turnyrai.fk_salis,
					salys.pavadinimas AS salis,
					COUNT(IF(ivarciai.uz_kuria_komanda = 'seimininku', 1, NULL)) AS ivarciai_seim,
					COUNT(IF(ivarciai.uz_kuria_komanda = 'sveciu', 1, NULL)) AS ivarciai_svec,
					bs2.ivarciu_suma,
					bs2.rungtyniu_suma,
					bs2.ivarciu_suma / bs2.rungtyniu_suma AS ivarciu_vidurkis
				FROM
					`{Config.TblPrefix}rungtynes` rungtynes
					INNER JOIN `{Config.TblPrefix}turnyrai` turnyrai ON turnyrai.pavadinimas = rungtynes.fk_turnyras AND turnyrai.sezonas = rungtynes.fk_sezonas AND turnyrai.fk_salis = rungtynes.fk_salis
					INNER JOIN `{Config.TblPrefix}salys` salys ON salys.id = turnyrai.fk_salis
					INNER JOIN (SELECT CONCAT(vardas, ' ', pavarde) AS teisejas, id FROM `{Config.TblPrefix}teisejai`) AS bs1 ON bs1.id = rungtynes.fk_teisejas
					LEFT JOIN `{Config.TblPrefix}ivarciai` ivarciai ON ivarciai.fk_seimininkai = rungtynes.fk_seimininkai AND ivarciai.fk_sveciai = rungtynes.fk_sveciai AND ivarciai.fk_data = rungtynes.faktine_data
					LEFT JOIN
						( 	SELECT
								turnyrai2.pavadinimas, turnyrai2.sezonas, turnyrai2.fk_salis,
								COUNT(ivarciai2.minute) AS ivarciu_suma,
								COUNT(DISTINCT rungtynes2.fk_seimininkai, rungtynes2.fk_sveciai, rungtynes2.faktine_data) AS rungtyniu_suma
							FROM 
								`{Config.TblPrefix}turnyrai` turnyrai2
								INNER JOIN `{Config.TblPrefix}rungtynes` rungtynes2 ON turnyrai2.pavadinimas = rungtynes2.fk_turnyras AND turnyrai2.sezonas = rungtynes2.fk_sezonas AND turnyrai2.fk_salis = rungtynes2.fk_salis
								LEFT JOIN `{Config.TblPrefix}ivarciai` ivarciai2 ON ivarciai2.fk_seimininkai = rungtynes2.fk_seimininkai AND ivarciai2.fk_sveciai = rungtynes2.fk_sveciai AND ivarciai2.fk_data = rungtynes2.faktine_data
							WHERE
								turnyrai2.fk_salis = IFNULL(?salis, turnyrai2.fk_salis)
								AND rungtynes2.faktine_data >= IFNULL(?nuo, rungtynes2.faktine_data)
								AND rungtynes2.faktine_data <= IFNULL(?iki, rungtynes2.faktine_data)
								AND (rungtynes2.fk_seimininkai = IFNULL(?komanda, rungtynes2.fk_seimininkai) OR rungtynes2.fk_sveciai = IFNULL(?komanda, rungtynes2.fk_sveciai))
							GROUP BY turnyrai2.pavadinimas, turnyrai2.sezonas, turnyrai2.fk_salis
						) AS bs2 ON bs2.pavadinimas = turnyrai.pavadinimas AND bs2.sezonas = turnyrai.sezonas AND bs2.fk_salis = turnyrai.fk_salis
				WHERE
					turnyrai.fk_salis = IFNULL(?salis, turnyrai.fk_salis)
					AND rungtynes.faktine_data >= IFNULL(?nuo, rungtynes.faktine_data)
					AND rungtynes.faktine_data <= IFNULL(?iki, rungtynes.faktine_data)
					AND (rungtynes.fk_seimininkai = IFNULL(?komanda, rungtynes.fk_seimininkai) OR rungtynes.fk_sveciai = IFNULL(?komanda, rungtynes.fk_sveciai))
				GROUP BY
					rungtynes.faktine_data, rungtynes.fk_seimininkai, rungtynes.fk_sveciai
				ORDER BY 
					turnyrai.pavadinimas, salys.pavadinimas, turnyrai.sezonas ASC, rungtynes.faktine_data DESC";
			
			var dt =
				Sql.Query(query, args => {
					args.Add("?nuo", MySqlDbType.DateTime).Value = dateFrom;
					args.Add("?iki", MySqlDbType.DateTime).Value = dateTo;
					args.Add("?salis", MySqlDbType.VarChar).Value = salis;
					args.Add("?komanda", MySqlDbType.VarChar).Value = komanda;
				});

			foreach( DataRow item in dt )
			{
				result.Add(new RungtynesReport.Rungtynes
				{
					FaktineData = Convert.ToDateTime(item["faktine_data"]),
					Seimininkai = Convert.ToString(item["fk_seimininkai"]),
					Sveciai = Convert.ToString(item["fk_sveciai"]),
					Teisejas = Convert.ToString(item["teisejas"]),

					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Sezonas = Convert.ToString(item["sezonas"]),
					Salis = Convert.ToString(item["salis"]),
					FkSalis = Convert.ToInt32(item["fk_salis"]),

					IvSeimininku = Convert.ToInt32(item["ivarciai_seim"]),
					IvSveciu = Convert.ToInt32(item["ivarciai_svec"]),
					RungtyniuSuma = Convert.ToInt32(item["rungtyniu_suma"]),
					IvarciuSuma = Convert.ToInt32(item["ivarciu_suma"]),
					IvVidurkis = Convert.ToInt32(item["ivarciu_vidurkis"])
				});
			}

			return result;

			
		}
	}
}