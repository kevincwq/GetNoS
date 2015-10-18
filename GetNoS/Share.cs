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

		public	const string SqliteName = "uniprot_db.sqlite";
		public static readonly IReadOnlyDictionary<string, int> ColumnNames = new Dictionary<string, int> () {
			{ "Entry",1 },
			{ "Sequence",2 },
			{ "Entry name",3 },
			{ "Protein names",4 },
			{ "Organism",5 },
			{ "Gene ontology (GO)",6 },
			{ "Gene ontology IDs",7 },
			{ "Gene names",8 },
			{ "Proteomes",9 },
			{ "Taxonomic lineage (ALL)",10 },
			{ "Virus hosts",11 },
			{ "Metal binding",12 },
			{ "Nucleotide binding",13 },
			{ "Site",14 },
			{ "Annotation",15 },
			{ "Features",16 },
			{ "Keywords",17 },
			{ "Protein existence",18 },
			{ "Domain [CC]",19 },
			{ "Sequence similarities",20 },
			{ "Protein families",21 },
			{ "Coiled coil",22 },
			{ "Compositional bias",23 },
			{ "Domain [FT]",24 },
			{ "Motif",25 },
			{ "Region",26 },
			{ "Repeat",27 },
			{ "Zinc finger",28 },
			{ "Cross-reference (PDB)",29 },
			{ "Cross-reference (Pfam)",30 },
			{ "DNA binding",31 },
			{ "Taxonomic lineage (all)",32 },
			{ "Taxonomic lineage IDs (all)",33 },
			{ "Gene ontology (biological process)",34 },
			{ "Gene ontology (molecular function)",35 },
			{ "Gene ontology (cellular component)",36 },
			{ "Length",37 },
			{ "Mass",38 },
			{ "Mapped PubMed ID",39 },
			{ "3D",40 }
		};

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

		public static int GetGoogleSearchCount (string query)
		{
			return DoSearch (query);
		}

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

		private static int DoSearch (string query)
		{
			// Instantiate the regular expression object.
			Regex r = new Regex (Share.CountPattern, RegexOptions.IgnoreCase);
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create (Share.GoogleSearchUrl + query);
			req.UserAgent = Share.UserAgent;// "Google Chrome/36"; // "Mozilla/5.0"
			//			req.SendChunked = true;
			req.Timeout = Share.ReqTimeout;
			req.ReadWriteTimeout = Share.ReqReadWriteTimeout;
			//			req.TransferEncoding = "UTF-8";
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse ();
			var stream = resp.GetResponseStream ();
			int count = 0;
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
			int count = 0;
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
			// Send the request to the server
			HttpResponseMessage response = await client.SendAsync (requestMessage);

			// Just as an example I'm turning the response into a string here
			string text = await response.Content.ReadAsStringAsync ();
			int count = 0;
			var r = new Regex (Share.CountPattern, RegexOptions.IgnoreCase);
			var m = r.Match (text);
			if (m.Success) {
				var g = m.Groups [1];
				int.TryParse (g.Value.Replace (",", ""), out count);
			}
			return count;
		}

		#endregion
	}
}

