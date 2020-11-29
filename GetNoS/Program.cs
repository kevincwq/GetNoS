//
//  Program.cs
//
//  Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 All Right Reserved.
//
using System;
using System.IO;

namespace GetNoS
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try {
				int parallel = Share.ParallelTask;
				if (args.Length > 1 && int.TryParse (args [1], out parallel))
					Share.ParallelTask = parallel;
				if (!File.Exists (Share.SqliteName)) {
					if (args.Length > 0 && !string.IsNullOrWhiteSpace (args [0]) && File.Exists (args [0])) {
						Console.WriteLine ("Start to load file db");
						Share.Db.LoadFileToDb (args [0]);
						Console.WriteLine ("Finish loading file");
					} else
						Console.WriteLine ("Invalid input filename");
				}
				Share.Db.UpdateGoogleSearchCount (Share.GetGoogleSearchCountAsync);
			} catch (Exception ex) {
				Console.WriteLine (ex);
				Console.WriteLine ("------Error Occured---------");
				Logger.Log (ex);
			}
		}
	}
}
