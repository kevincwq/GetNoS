//
//  Pocos.cs
//
//  Chen Weiqing <kevincwq@gmail.com>
//
//  Copyright (c) 2015 All Right Reserved.
//
using System;
using ServiceStack.DataAnnotations;

namespace GetNoS
{
	public class ProUnitKbRecord
	{
		[AutoIncrement]
		public int Id { get; set; }

		public string Entry { get; set; }

		public string Sequence { get; set; }

		public string Entry_name { get; set; }

		public string Protein_names { get; set; }

		public string Organism { get; set; }

		public string Gene_ontology_GO { get; set; }

		public string Gene_ontology_IDs { get; set; }

		public string Gene_names { get; set; }

		public string Proteomes { get; set; }

		public string Virus_hosts { get; set; }

		public string Metal_binding { get; set; }

		public string Nucleotide_binding { get; set; }

		public string Site { get; set; }

		public string Annotation { get; set; }

		public string Features { get; set; }

		public string Keywords { get; set; }

		public string Protein_existence { get; set; }

		public string Domain_CC { get; set; }

		public string Sequence_similarities { get; set; }

		public string Protein_families { get; set; }

		public string Coiled_coil { get; set; }

		public string Compositional_bias { get; set; }

		public string Domain_FT { get; set; }

		public string Motif { get; set; }

		public string Region { get; set; }

		public string Repeat { get; set; }

		public string Zinc_finger { get; set; }

		public string Cross_reference_PDB { get; set; }

		public string Cross_reference_Pfam { get; set; }

		public string DNA_binding { get; set; }

		public string Taxonomic_lineage_all { get; set; }

		public string Taxonomic_lineage_IDs_all { get; set; }

		public string Gene_ontology_biological_process { get; set; }

		public string Gene_ontology_molecular_function { get; set; }

		public string Gene_ontology_cellular_component { get; set; }

		public string Length { get; set; }

		public string Mass { get; set; }

		public string Mapped_PubMed_ID { get; set; }

		public string D3 { get; set; }
	}
}

