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
	public class KomandaRepo
	{
		public static List<KomandaListVM> List()
		{
			var result = new List<KomandaListVM>();

			var query =
				$@"SELECT
					md.pavadinimas,
					mark.pavadinimas AS miestas,
					mark2.pavadinimas AS salis
				FROM `{Config.TblPrefix}komandos` md
					LEFT JOIN `{Config.TblPrefix}miestai` mark ON mark.id=md.fk_miestas
					LEFT JOIN `{Config.TblPrefix}salys` mark2 ON mark2.id=mark.fk_salis
				ORDER BY md.pavadinimas ASC";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new KomandaListVM
				{
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
					Miestas = (Convert.ToString(item["miestas"]) + " (" + Convert.ToString(item["salis"]) + ")"),
				});
			}

			return result;
		}

		public static KomandaEditVM Find(string pavadinimas)
		{
			var mevm = new KomandaEditVM();

			var query = $@"SELECT * FROM `{Config.TblPrefix}komandos` WHERE pavadinimas=?pavadinimas";

			var dt =
				Sql.Query(query, args => {
					args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pavadinimas;
				});

			foreach( DataRow item in dt )
			{
				mevm.Model.Pavadinimas = Convert.ToString(item["pavadinimas"]);
				#nullable enable
				mevm.Model.Sutrumpinimas = Sql.AllowNull(item["trumpas_pavadinimas"], it => (string?)Convert.ToString(it));
				#nullable disable
				mevm.Model.Treneris = Convert.ToString(item["treneris"]);
				mevm.Model.FKs = Convert.ToString(item["fk_stadionas"]) + ";" + Convert.ToString(item["fk_miestas"]);
			}

			return mevm;
		}

		public static KomandaListVM FindForDeletion(string pavadinimas)
		{
			var mlvm = new KomandaListVM();

			var query =
				$@"SELECT
					md.pavadinimas,
					mark.pavadinimas AS miestas
				FROM `{Config.TblPrefix}komandos` md
					LEFT JOIN `{Config.TblPrefix}miestai` mark ON mark.id=md.fk_miestas
				WHERE md.pavadinimas=?pavadinimas";

			var dt =
				Sql.Query(query, args => {
					args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pavadinimas;
				});

			foreach( DataRow item in dt )
			{
				mlvm.Pavadinimas = Convert.ToString(item["pavadinimas"]);
				mlvm.Miestas = Convert.ToString(item["miestas"]);
			}

			return mlvm;
		}

		public static void Update(KomandaEditVM komandaEvm)
		{
			var query =
				$@"UPDATE `{Config.TblPrefix}komandos`
				SET
					trumpas_pavadinimas=?trumpas_pavadinimas,
					treneris=?treneris,
					fk_stadionas=?stadionas,
					fk_miestas=?miestas
				WHERE
					pavadinimas=?pavadinimas";

			string[] parts = komandaEvm.Model.FKs.Split(';');

			Sql.Update(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = komandaEvm.Model.Pavadinimas;
				
				args.Add("?trumpas_pavadinimas", MySqlDbType.VarChar).Value = komandaEvm.Model.Sutrumpinimas;
				args.Add("?treneris", MySqlDbType.VarChar).Value = komandaEvm.Model.Treneris;
				args.Add("?stadionas", MySqlDbType.VarChar).Value = parts[0];
				args.Add("?miestas", MySqlDbType.Int32).Value = Convert.ToInt32(parts[1]);
			});
		}

		public static void Insert(KomandaEditVM komandaEvm)
		{
			var query =
				$@"INSERT INTO `{Config.TblPrefix}komandos`
				(
					pavadinimas,
					trumpas_pavadinimas,
					treneris,
					fk_stadionas,
					fk_miestas
				)
				VALUES(
					?pavadinimas,
					?trumpas,
					?treneris,
					?stadionas,
					?miestas
				)";

			string[] parts = komandaEvm.Model.FKs.Split(';');

			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = komandaEvm.Model.Pavadinimas;
				args.Add("?trumpas", MySqlDbType.VarChar).Value = komandaEvm.Model.Sutrumpinimas;
				args.Add("?treneris", MySqlDbType.VarChar).Value = komandaEvm.Model.Treneris;
				args.Add("?stadionas", MySqlDbType.VarChar).Value = parts[0];
				args.Add("?miestas", MySqlDbType.Int32).Value = Convert.ToInt32(parts[1]);
			});
		}

		public static void Delete(string pavadinimas)
		{
			var query = $@"DELETE FROM `{Config.TblPrefix}komandos` WHERE pavadinimas=?pavadinimas";

			Sql.Delete(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = pavadinimas;
			});
		}
	}
}