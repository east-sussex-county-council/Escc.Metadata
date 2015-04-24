# Escc.Web.Metadata

A library to work with website metadata.

ESD controlled lists 
--------------------
This library was originally based on the E-Government Metadata Standard 3.0, and most of the classes here are for working with the ESD controlled lists lists from http://standards.esd.org.uk/ in their Taxonomy XML format. This includes the following features:

* Autocomplete (using JQuery UI)
* Validation of terms against lists
* Querying lists for terms
* Following links between preferred terms and non-preferred terms
* Following links between lists using mappings

Common metadata control (E-GMS compliant)
-----------------------------------------
The `MetadataControl` class can be used to write common metadata elements to a WebForms page, with values specified in `web.config`, set declaratively in ASPX files or set in code.

Where appropriate metadata is written in the format expected by the E-Government Metadata Standard 3.0, though we will most likely update this to a more modern format such as RDFa.  
