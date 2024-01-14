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
	public class SalisRepo
	{
		public static List<Salis> List()
		{
			var salys = new List<Salis>();

			string query = $@"SELECT * FROM `{Config.TblPrefix}salys` ORDER BY id ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				salys.Add(new Salis
				{
					Id = Convert.ToInt32(item["id"]),
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
				});
			}

			return salys;
		}

		public static Salis Find(int id)
		{
			var salis = new Salis();

			var query = $@"SELECT * FROM `{Config.TblPrefix}salys` WHERE id=?id";
			var dt = 
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				salis.Id = Convert.ToInt32(item["id"]);
				salis.Pavadinimas = Convert.ToString(item["pavadinimas"]);
			}

			return salis;
		}

		public static void Update(Salis salis)
		{			
			var query = 
				$@"UPDATE `{Config.TblPrefix}salys` 
				SET 
					pavadinimas=?pavadinimas 
				WHERE 
					id=?id";

			Sql.Update(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = salis.Pavadinimas;
				args.Add("?id", MySqlDbType.VarChar).Value = salis.Id;
			});							
		}

		public static void Insert(Salis salis)
		{			
			var query = $@"INSERT INTO `{Config.TblPrefix}salys` ( pavadinimas, id ) VALUES ( ?pavadinimas, ?id )";
			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = salis.Pavadinimas;
				args.Add("?id", MySqlDbType.Int32).Value = findNextIndex();
			});
		}


		public static int findNextIndex()
		{
			int id = 0;
			var query = $@"SELECT MAX(id) AS MaxId
			FROM `{Config.TblPrefix}salys` 
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
			var query = $@"DELETE FROM `{Config.TblPrefix}salys` where id=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.Int32).Value = id;
			});			
		}
	}
}