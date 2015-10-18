//
//  DbHelper.cs
//
//  Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 All Right Reserved.
//
using System;
using System.Diagnostics;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace GetNoS
{
	class DbHelper
	{
		private readonly IDbConnectionFactory DbFactory;

		public DbHelper (string dbName)
		{
			DbFactory = new OrmLiteConnectionFactory (
				dbName,  
				SqliteDialect.Provider);
		}

		public void CreateTables ()
		{
			using (var db = DbFactory.OpenDbConnection ()) {
				if (db.CreateTableIfNotExists<ProUnitKbRecord> ()) {
					Logger.Info ("Table ProUnitKbRecord Created");
				}
			}
		}
	}
}

