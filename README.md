# Escc.Metadata

A library to work with website metadata which describes the website and the individual web page. 

## Use Escc.Metadata for current projects

The `Escc.Metadata` package targets .NET Standard 2.0 with a modern set of metadata based on standards such as [schema.org](https://schema.org/), the [Open Graph Protocol](http://ogp.me/) used by Facebook and others, [Twitter Cards](https://developer.twitter.com/en/docs/tweets/optimize-with-cards/overview/abouts-cards.html) and [ESD Standards](https://standards.esd.org.uk/) expressed in [JSON-LD](https://json-ld.org/).

This data can be represented by an instance of `Escc.Metadata.Metadata`.

It does not include metadata which relates to technical features such as [Open Search](http://www.opensearch.org/Home), [RSS](https://en.wikipedia.org/wiki/RSS) or [favicons and Apple touch icons](https://realfavicongenerator.net/). Where appropriate these should be implemented by the templates for individual sites.

### Displaying metadata in ASP.NET Core MVC

The package includes two Razor partial views for metadata which should appear in the `<head>` and the `<body>` of the page respectively. These are embedded resources, so in .NET Core you need to register an `EmbeddedFileProvider` in your `Startup` class to use them. This requires the `Microsoft.Extensions.FileProviders.Embedded` NuGet package.

	using Microsoft.Extensions.FileProviders;

	public void ConfigureServices(IServiceCollection services)
    {
		...
		services.Configure<RazorViewEngineOptions>(options => 
        {
            options.FileProviders.Add(new EmbeddedFileProvider(typeof(Metadata.Metadata).Assembly, "Escc.Metadata.Views"));
        });
		...
	}

These partial views can be referenced as normal within your views (where `Model.Metadata` is an instance of `Escc.Metadata.Metadata`):

	<partial name="~/_Metadata_Head.cshtml" for="Metadata" />
	<partial name="~/_Metadata_Body.cshtml" for="Metadata" />

### Loading metadata from configuration in ASP.NET Core

You can set metadata in code, or load it from configuration:

	public void ConfigureServices(IServiceCollection services)
    {
		...
        services.AddOptions();
        services.Configure<Metadata.Metadata>(options => configuration.GetSection("Escc.Metadata").Bind(options));
		...
	} 


Metadata in `appsettings.json` reflects the structure of `Escc.Metadata.Metadata` (this example is not a complete list):

	{
	  "Escc.Metadata": {
	    "SiteName": "East Sussex County Council",
	    "PageImage": {
	      "ImageUrl": "https://www.example.org/open-graph-image.png",
	      "AlternativeText": "East Sussex County Council"
	    },
	    "Facebook": {
	      "OpenGraphType": "article"
	    },
	    "Twitter": {
	      "TwitterCardType": "summary"
	    }
	  }
	}

## Use Escc.Web.Metadata for older projects

This library was originally based on the E-Government Metadata Standard 3.0 using controlled lists published in the now obsolete Taxonomy XML format, and used on WebForms pages. The current version sits alongside this legacy code rather than replacing it because it is problematic to replace the original code due to its strong name and inherited configuration settings. 

### ESD controlled lists 

Most of the classes here are for working with the ESD controlled lists from <http://standards.esd.org.uk/> in their Taxonomy XML format. This includes the following features:

*  Autocomplete (using JQuery UI)
*  Validation of terms against lists
*  Querying lists for terms
*  Following links between preferred terms and non-preferred terms
*  Following links between lists using mappings

### Common metadata control (E-GMS compliant)

The `MetadataControl` class can be used to write common metadata elements to a WebForms page, with values specified in `web.config`, set declaratively in ASPX files or set in code.

Where appropriate metadata is written in the format expected by the E-Government Metadata Standard 3.0.  
