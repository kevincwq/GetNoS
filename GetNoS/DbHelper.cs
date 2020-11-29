//
//  DbHelper.cs
//
//  Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 All Right Reserved.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

			CreateTables ();
		}

		private void CreateTables ()
		{
			using (var db = DbFactory.OpenDbConnection ()) {
				if (db.CreateTableIfNotExists<ProUnitKbRecord> ()) {
					Logger.Info ("Table ProUnitKbRecord Created");
				}
				if (db.CreateTableIfNotExists<Keyword> ()) {
					Logger.Info ("Table Keyword Created");
				}
				if (db.CreateTableIfNotExists<CrossKeyword> ()) {
					Logger.Info ("Table CrossKeyword Created");
				}
			}
		}

		private string GetValueWith (string name, Dictionary<string, int> indexDict, string[] values, Dictionary<string, HashSet<string>> keywords, bool keywordEnable)
		{
			if (indexDict.ContainsKey (name)) {
				var line = values [indexDict [name]];
				if (keywordEnable) {
					var keys = line.Split (';');
					if (keys.Length > 0) {
						if (!keywords.ContainsKey (name))
							keywords.Add (name, new HashSet<string> ());
						var hashset = keywords [name];
						foreach (var key in keys) {
							if (!string.IsNullOrWhiteSpace (key))
								hashset.Add (key.Trim ());
						}
					}
				}
				return line;
			} else {
				return null;
			}
		}

		private void AddKeywords (Dictionary<string, HashSet<string>> keywords)
		{
			try {
				using (var db = DbFactory.Open ()) {
					var list = new List<Keyword> ();
					foreach (var category in keywords) {
						foreach (var value in category.Value) {
							if (!db.Exists<Keyword> (k => (k.Value == value && k.Category == category.Key))) {
								list.Add (new Keyword () { 
									Value = value,
									Category = category.Key,
									GoogleSearchCount = -1
								});
							}
						}
					}
					db.InsertAll (list);
					list.Clear ();
				}
			} catch (Exception ex) {
				Logger.Log (ex);
			}
		}

		public void LoadFileToDb (string fileName, char delimiter = '\t')
		{
			using (var reader = new StreamReader (fileName)) {
				var columns = reader.ReadLine ().Split (delimiter);
				var indexDict = new Dictionary<string, int> ();
				var index = 0;
				foreach (var column in columns) {
					indexDict.Add (column, index++);
				}
				var list = new List<ProUnitKbRecord> (1000);
				var keywordsDict = new Dictionary<string, HashSet<string>> ();
				using (var db = DbFactory.Open ()) {
					while (!reader.EndOfStream) {
						var line = reader.ReadLine ();
						var values = line.Split (delimiter);
						if (list.Count > 1000) {
							db.InsertAll (list);
							list.Clear ();
							AddKeywords (keywordsDict);
							keywordsDict.Clear ();
						}
						list.Add (new ProUnitKbRecord () {
							Entry = GetValueWith ("Entry", indexDict, values, keywordsDict, false), 
							Sequence = GetValueWith ("Sequence", indexDict, values, keywordsDict, false), 
							Entry_name = GetValueWith ("Entry name", indexDict, values, keywordsDict, false), 
							Protein_names = GetValueWith ("Protein names", indexDict, values, keywordsDict, true), 
							Organism = GetValueWith ("Organism", indexDict, values, keywordsDict, false), 
							Gene_ontology_GO = GetValueWith ("Gene ontology (GO)", indexDict, values, keywordsDict, false), 
							Gene_ontology_IDs = GetValueWith ("Gene ontology IDs", indexDict, values, keywordsDict, true), 
							Gene_names = GetValueWith ("Gene names", indexDict, values, keywordsDict, false), 
							Proteomes = GetValueWith ("Proteomes", indexDict, values, keywordsDict, true), 
							Virus_hosts = GetValueWith ("Virus hosts", indexDict, values, keywordsDict, false), 
							Metal_binding = GetValueWith ("Metal binding", indexDict, values, keywordsDict, false), 
							Nucleotide_binding = GetValueWith ("Nucleotide binding", indexDict, values, keywordsDict, false), 
							Site = GetValueWith ("Site", indexDict, values, keywordsDict, false), 
							Annotation = GetValueWith ("Annotation", indexDict, values, keywordsDict, false), 
							Features = GetValueWith ("Features", indexDict, values, keywordsDict, false), 
							Keywords = GetValueWith ("Keywords", indexDict, values, keywordsDict, true), 
							Protein_existence = GetValueWith ("Protein existence", indexDict, values, keywordsDict, false), 
							Domain_CC = GetValueWith ("Domain [CC]", indexDict, values, keywordsDict, true), 
							Sequence_similarities = GetValueWith ("Sequence similarities", indexDict, values, keywordsDict, false), 
							Protein_families = GetValueWith ("Protein families", indexDict, values, keywordsDict, true), 
							Coiled_coil = GetValueWith ("Coiled coil", indexDict, values, keywordsDict, false), 
							Compositional_bias = GetValueWith ("Compositional bias", indexDict, values, keywordsDict, false), 
							Domain_FT = GetValueWith ("Domain [FT]", indexDict, values, keywordsDict, false), 
							Motif = GetValueWith ("Motif", indexDict, values, keywordsDict, false), 
							Region = GetValueWith ("Region", indexDict, values, keywordsDict, false), 
							Repeat = GetValueWith ("Repeat", indexDict, values, keywordsDict, false), 
							Zinc_finger = GetValueWith ("Zinc finger", indexDict, values, keywordsDict, false), 
							Cross_reference_PDB = GetValueWith ("Cross-reference (PDB)", indexDict, values, keywordsDict, true), 
							Cross_reference_Pfam = GetValueWith ("Cross-reference (Pfam)", indexDict, values, keywordsDict, false), 
							DNA_binding = GetValueWith ("DNA binding", indexDict, values, keywordsDict, false), 
							// Taxonomic_lineage_all = GetValueWith ("Taxonomic lineage (all)", indexDict, values, keywordsDict, false), 
							Taxonomic_lineage_IDs_all = GetValueWith ("Taxonomic lineage IDs (all)", indexDict, values, keywordsDict, false), 
							// Gene_ontology_biological_process = GetValueWith ("Gene ontology (biological process)", indexDict, values, keywordsDict, false), 
							// Gene_ontology_molecular_function = GetValueWith ("Gene ontology (molecular function)", indexDict, values, keywordsDict, false), 
							// Gene_ontology_cellular_component = GetValueWith ("Gene ontology (cellular component)", indexDict, values, keywordsDict, false), 
							Length = GetValueWith ("Length", indexDict, values, keywordsDict, false), 
							Mass = GetValueWith ("Mass", indexDict, values, keywordsDict, false), 
							Mapped_PubMed_ID = GetValueWith ("Mapped PubMed ID", indexDict, values, keywordsDict, false), 
							D3 = GetValueWith ("3D", indexDict, values, keywordsDict, false), 
						});
					}
					if (list.Count > 0) {
						db.InsertAll (list);
						list.Clear ();
						AddKeywords (keywordsDict);
						keywordsDict.Clear ();
					}
				}
			}
		}

		public void UpdateGoogleSearchCount (Func<string, Task<int>> doSearchAsync)
		{
			var tasklist = new List<Task> ();
			using (var db = DbFactory.Open ()) {
				var keywordCategories = db.ColumnDistinct<string> ("select Category from Keyword");
				// generate cross keywords
				foreach (var cat in keywordCategories) {
					var keywords = db.Where<Keyword> (new {Category = cat});
					for (int i = 0; i < keywords.Count; i++) {
						var idi = i;
						if (keywords [i].GoogleSearchCount == -1) {
							tasklist.Add (doSearchAsync (keywords [i].Value).ContinueWith (t => {
								if (t.IsFaulted)
									// Logger.Log (t.Exception);
									Console.WriteLine (t.Exception.InnerException.Message);
								else {
									if (t.Result != -1) {
										keywords [idi].GoogleSearchCount = t.Result;
										db.Update (keywords [idi]);
									} else {
										Console.WriteLine ("Invalid Result for {0}", keywords [idi].Value);
									}
								}
							}));
						}
						for (int j = i + 1; j < keywords.Count; j++) {
							var idj = j;
							var existCk = db.Single<CrossKeyword> (ck => ck.FisrtKeyword == keywords [idi].Id && ck.SecondKeyword == keywords [idj].Id);
							if (existCk == null || existCk.GoogleSearchCount == -1) {
								Task.Delay (new Random (System.Environment.TickCount).Next (50)).Wait ();
								tasklist.Add (doSearchAsync (keywords [i].Value + " " + keywords [j].Value).ContinueWith (t => {
									if (t.IsFaulted)
										Console.WriteLine (t.Exception.InnerException.Message);
										// Logger.Log (t.Exception.InnerException);
									else {
										if (t.Result != -1) {
											var ck = new CrossKeyword () {
												FisrtKeyword = keywords [idi].Id,
												SecondKeyword = keywords [idj].Id,
												GoogleSearchCount = t.Result
											};
											if (existCk != null)
												ck.Id = existCk.Id;
											db.Save (ck);
										} else {
											Console.WriteLine ("Invalid Result for {0} {1}", keywords [idi].Value, keywords [idj].Value);
										}
									}
								}));
							} else {
								continue;
							}
							try {
								if (tasklist.Count > Share.ParallelTask) {
									tasklist.RemoveAll (t => t.IsCompleted);
									if (tasklist.Count < Share.ParallelTask) {
										Console.WriteLine ("{0} Tasks < {1}, continue", tasklist.Count, Share.ParallelTask);
										continue;
									} else {
										Task.Delay (2000).Wait ();
										Console.WriteLine ("{0} Tasks, too many wait ...", tasklist.Count);
										Task.WhenAny (tasklist).Wait ();
									}
								}
							} catch (Exception ex) {
								Console.WriteLine (ex.Message);
								// Logger.Log (ex);
							}
						}
					}
				}
			}
		}
	}
}

