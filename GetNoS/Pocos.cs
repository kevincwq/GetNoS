//
//  Pocos.cs
//
//  Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 All Right Reserved.
//
using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace GetNoS
{
	public class ProUnitKbRecord: IHasId<int>
	{
		[AutoIncrement]
		public int Id { get; set; }

		[Index (true)]
		public string Entry { get; set; }

		[Index]
		public string Sequence { get; set; }

		[Index]
		public string Entry_name { get; set; }

		[Index]
		public string Protein_names { get; set; }

		[Index]
		public string Organism { get; set; }

		[Index]
		public string Gene_ontology_GO { get; set; }

		[Index]
		public string Gene_ontology_IDs { get; set; }

		[Index]
		public string Gene_names { get; set; }

		[Index]
		public string Proteomes { get; set; }

		[Index]
		public string Virus_hosts { get; set; }

		[Index]
		public string Metal_binding { get; set; }

		[Index]
		public string Nucleotide_binding { get; set; }

		[Index]
		public string Site { get; set; }

		[Index]
		public string Annotation { get; set; }

		[Index]
		public string Features { get; set; }

		[Index]
		public string Keywords { get; set; }

		[Index]
		public string Protein_existence { get; set; }

		[Index]
		public string Domain_CC { get; set; }

		[Index]
		public string Sequence_similarities { get; set; }

		[Index]
		public string Protein_families { get; set; }

		[Index]
		public string Coiled_coil { get; set; }

		[Index]
		public string Compositional_bias { get; set; }

		[Index]
		public string Domain_FT { get; set; }

		[Index]
		public string Motif { get; set; }

		[Index]
		public string Region { get; set; }

		[Index]
		public string Repeat { get; set; }

		[Index]
		public string Zinc_finger { get; set; }

		[Index]
		public string Cross_reference_PDB { get; set; }

		[Index]
		public string Cross_reference_Pfam { get; set; }

		[Index]
		public string DNA_binding { get; set; }

		[Index]
		public string Taxonomic_lineage_all { get; set; }

		[Index]
		public string Taxonomic_lineage_IDs_all { get; set; }

		[Index]
		public string Gene_ontology_biological_process { get; set; }

		[Index]
		public string Gene_ontology_molecular_function { get; set; }

		[Index]
		public string Gene_ontology_cellular_component { get; set; }

		[Index]
		public string Length { get; set; }

		[Index]
		public string Mass { get; set; }

		[Index]
		public string Mapped_PubMed_ID { get; set; }

		[Index]
		public string D3 { get; set; }
	}

	//	public enum KeywordCategory
	//	{
	//		// Entry = 1,
	//		// Sequence = 2,
	//		// Entryname = 3,
	//		Proteinnames = 4,
	//		Organism = 5,
	//		GeneontologyGO = 6,
	//		GeneontologyIDs = 7,
	//		Genenames = 8,
	//		Proteomes = 9,
	//		Virushosts = 10,
	//		Metalbinding = 11,
	//		Nucleotidebinding = 12,
	//		Site = 13,
	//		Annotation = 14,
	//		Features = 15,
	//		Keywords = 16,
	//		Proteinexistence = 17,
	//		DomainCC = 18,
	//		Sequencesimilarities = 19,
	//		Proteinfamilies = 20,
	//		Coiledcoil = 21,
	//		Compositionalbias = 22,
	//		DomainFT = 23,
	//		Motif = 24,
	//		Region = 25,
	//		Repeat = 26,
	//		Zincfinger = 27,
	//		CrossreferencePDB = 28,
	//		CrossreferencePfam = 29,
	//		DNAbinding = 30,
	//		Taxonomiclineageall = 31,
	//		TaxonomiclineageIDsall = 32,
	//		Geneontologybiologicalprocess = 33,
	//		Geneontologymolecularfunction = 34,
	//		Geneontologycellularcomponent = 35,
	//		Length = 36,
	//		Mass = 37,
	//		MappedPubMedID = 38,
	//		D3 = 39
	//	}

	public class Keyword: IHasId<int>
	{
		[AutoIncrement]
		public int Id { get; set; }

		[Index (true)]
		public string Value{ get; set; }

		public string Extra { get; set; }

		[Index]
		public string Category { get; set; }

		[Default (typeof(int), "-1")]
		public int GoogleSearchCount{ get; set; }
	}

	public class CrossKeyword:IHasId<int>
	{
		[AutoIncrement]
		public int Id { get; set; }

		public int FisrtKeyword{ get; set; }

		public int SecondKeyword{ get; set; }

		//		[Index]
		//		public string Category { get; set; }

		[Default (typeof(int), "-1")]
		public int GoogleSearchCount{ get; set; }
	}
}

