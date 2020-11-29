//
//  Shareables.cs
//
//  Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 All Right Reserved.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace GetNoS
{
	class Share
	{
		public const string GoogleSearchUrl = "https://www.google.com/search?q=";
		public	const string CountPattern = @"About\s([\d,]+)\sresults";
		public const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
		public const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
		public const int ReqTimeout = 60000;
		public const int ReqReadWriteTimeout = 6000;
		public static int ParallelTask = 100;

		public	static string SqliteName = "uniprot_db.sqlite";
		//		public static readonly IReadOnlyList<string> ColumnNames = new List<string> {
		//			"Entry",
		//			"Sequence",
		//			"Entry name",
		//			"Protein names",
		//			"Organism",
		//			"Gene ontology (GO)",
		//			"Gene ontology IDs",
		//			"Gene names",
		//			"Proteomes",
		//			"Taxonomic lineage (ALL)",
		//			"Virus hosts",
		//			"Metal binding",
		//			"Nucleotide binding",
		//			"Site",
		//			"Annotation",
		//			"Features",
		//			"Keywords",
		//			"Protein existence",
		//			"Domain [CC]",
		//			"Sequence similarities",
		//			"Protein families",
		//			"Coiled coil",
		//			"Compositional bias",
		//			"Domain [FT]",
		//			"Motif",
		//			"Region",
		//			"Repeat",
		//			"Zinc finger",
		//			"Cross-reference (PDB)",
		//			"Cross-reference (Pfam)",
		//			"DNA binding",
		//			"Taxonomic lineage (all)",
		//			"Taxonomic lineage IDs (all)",
		//			"Gene ontology (biological process)",
		//			"Gene ontology (molecular function)",
		//			"Gene ontology (cellular component)",
		//			"Length",
		//			"Mass",
		//			"Mapped PubMed ID",
		//			"3D"
		//		};

		//		public static class ColumnNames
		//		{
		//			public const string Entry = "Entry";
		//			public const string Sequence = "Sequence";
		//			public const string Entry_name = "Entry name";
		//			public const string Protein_names = "Protein names";
		//			public const string Organism = "Organism";
		//			public const string Gene_ontology_GO = "Gene ontology (GO)";
		//			public const string Gene_ontology_IDs = "Gene ontology IDs";
		//			public const string Gene_names = "Gene names";
		//			public const string Proteomes = "Proteomes";
		//			public const string Virus_hosts = "Virus hosts";
		//			public const string Metal_binding = "Metal binding";
		//			public const string Nucleotide_binding = "Nucleotide binding";
		//			public const string Site = "Site";
		//			public const string Annotation = "Annotation";
		//			public const string Features = "Features";
		//			public const string Keywords = "Keywords";
		//			public const string Protein_existence = "Protein existence";
		//			public const string Domain_CC = "Domain [CC]";
		//			public const string Sequence_similarities = "Sequence similarities";
		//			public const string Protein_families = "Protein families";
		//			public const string Coiled_coil = "Coiled coil";
		//			public const string Compositional_bias = "Compositional bias";
		//			public const string Domain_FT = "Domain [FT]";
		//			public const string Motif = "Motif";
		//			public const string Region = "Region";
		//			public const string Repeat = "Repeat";
		//			public const string Zinc_finger = "Zinc finger";
		//			public const string Cross_reference_PDB = "Cross-reference (PDB)";
		//			public const string Cross_reference_Pfam = "Cross-reference (Pfam)";
		//			public const string DNA_binding = "DNA binding";
		//			public const string Taxonomic_lineage_all = "Taxonomic lineage (all)";
		//			public const string Taxonomic_lineage_IDs_all = "Taxonomic lineage IDs (all)";
		//			public const string Gene_ontology_biological_process = "Gene ontology (biological process)";
		//			public const string Gene_ontology_molecular_function = "Gene ontology (molecular function)";
		//			public const string Gene_ontology_cellular_component = "Gene ontology (cellular component)";
		//			public const string Length = "Length";
		//			public const string Mass = "Mass";
		//			public const string Mapped_PubMed_ID = "Mapped PubMed ID";
		//			public const string D3 = "3D";
		//		}

		public	static readonly DbHelper Db;

		private static readonly  HttpClient MyHttpClient;

		static Share ()
		{
			ServicePointManager.DefaultConnectionLimit = 200;
			ServicePointManager.UseNagleAlgorithm = true;
			ServicePointManager.Expect100Continue = true;

			Db = new DbHelper (SqliteName);
			MyHttpClient = CreateHttpClient ();
		}

		//		public static int GetGoogleSearchCount (string query)
		//		{
		//			return DoSearch (query);
		//		}

		public static async Task<int> GetGoogleSearchCountAsync (string query)
		{
			return await DoSearchAsync1 (MyHttpClient, query);
		}

		#region PRIVATE HELPER

		private static HttpClient CreateHttpClient ()
		{
			// Declare an HttpClient object, and increase the buffer size. The
			// default buffer size is 65,536.
			HttpClient client = new HttpClient () {
				MaxResponseContentBufferSize = 1000000, 
				Timeout = TimeSpan.FromSeconds (60)
			};
			return client;
		}

		private static HttpRequestMessage CreateRequest (string query)
		{
			// Add a new Request Message
			HttpRequestMessage requestMessage = new HttpRequestMessage (HttpMethod.Get, Share.GoogleSearchUrl + query);

			// Add our custom headers
			requestMessage.Headers.Add ("User-Agent", Share.UserAgent); // "Mozilla/5.0"
			// requestMessage.Headers.Add ("Content-Type", "text/html");
			requestMessage.Headers.Add ("Accept", Share.Accept);
			return requestMessage;
		}

		private static async Task<int> DoSearchAsync (string query)
		{
			// Instantiate the regular expression object.
			Regex r = new Regex (Share.CountPattern, RegexOptions.IgnoreCase);
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create (Share.GoogleSearchUrl + query);
			req.UserAgent = "Google Chrome/45";//Share.UserAgent;// "Google Chrome/36"; // "Mozilla/5.0"
			//			req.SendChunked = true;
			req.Timeout = Share.ReqTimeout;
			req.ReadWriteTimeout = Share.ReqReadWriteTimeout;
			//			req.TransferEncoding = "UTF-8";
			var resp = await req.GetResponseAsync ();
			var stream = resp.GetResponseStream ();
			int count = -1;
			using (var textReader = new StreamReader (stream)) {
				Match m = r.Match (textReader.ReadToEnd ());
				if (m.Success) {
					var g = m.Groups [1];
					int.TryParse (g.Value.Replace (",", ""), out count);
				}
			}
			resp.Close ();
			return count;
		}

		private static async Task<int> DoSearchAsync1 (HttpClient client, string query)
		{
			var text = await client.GetStringAsync (Share.GoogleSearchUrl + query);
			int count = -1;
			var r = new Regex (Share.CountPattern, RegexOptions.IgnoreCase);
			var m = r.Match (text);
			if (m.Success) {
				var g = m.Groups [1];
				int.TryParse (g.Value.Replace (",", ""), out count);
			}
			return count;
		}

		private static async Task<int> DoSearchAsync2 (HttpClient client, HttpRequestMessage requestMessage)
		{
			int count = -1;

			// Send the request to the server
			using (HttpResponseMessage response = await client.SendAsync (requestMessage)) {

				// Just as an example I'm turning the response into a string here
				string text = await response.Content.ReadAsStringAsync ();
				var r = new Regex (Share.CountPattern, RegexOptions.IgnoreCase);
				var m = r.Match (text);
				if (m.Success) {
					var g = m.Groups [1];
					int.TryParse (g.Value.Replace (",", ""), out count);
				}
				requestMessage.Dispose ();
				return count;
			}
		}

		#endregion
	}
}

