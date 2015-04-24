using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.Web.Metadata
{
    /// <summary>
    /// Support file for autocomplete facility on fields expecting ESD metadata terms. 
    /// Autocomplete can be added using JQuery UI plugin from <a href="http://jqueryui.com/">http://jqueryui.com/</a>
    /// </summary>
    public class Autocomplete : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            // If there is no query, just return no results
            string searchTerm = String.Empty;
            bool formatResultsAsJSON = true;
            if (!String.IsNullOrEmpty(context.Request.QueryString["term"]))
            {
                searchTerm = context.Request.QueryString["term"]; // JQueryUI plugin
            }
            else if (!String.IsNullOrEmpty(context.Request.QueryString["q"]))
            {
                searchTerm = context.Request.QueryString["q"]; // old autocomplete plugin
                formatResultsAsJSON = false;
            }
            if (String.IsNullOrEmpty(searchTerm)) return;


            // Check we know which list to query
            if (String.IsNullOrEmpty(context.Request.QueryString["list"]))
            {
                throw new ArgumentException("The controlled list must be specified in the querystring as list=<abbreviated name of list>");
            }

            // Load the list
            EsdControlledList controlledList = EsdControlledList.GetControlledList(context.Request.QueryString["list"]);
            List<string> termsFound = new List<string>();

            // Are the valid terms limited to a specific list of allowed terms?
            if (String.IsNullOrEmpty(context.Request.QueryString["allowed"]))
            {
                // If not limited, do a standard search for the search term. Get non-preferred terms too because 
                // they're there to help the user find the appropriate preferred term.
                EsdTermCollection results = controlledList.GetTerms(searchTerm, false, EsdPreferredState.Any);

                // Return results as one per line, with non-preferred terms suffixed by a separator and the appropriate preferred term
                foreach (EsdTerm term in results)
                {
                    if (term.Text.ToUpperInvariant().Contains(searchTerm.ToUpperInvariant()))
                    {
                        if (term.Preferred)
                        {
                            termsFound.Add(term.Text);

                            // Add non-preferred terms suffixing them with a separator and the appropriate preferred term
                            EsdTermCollection nonPreferredTerms = controlledList.GetNonPreferredTerms(term.Id);
                            foreach (EsdTerm nonPreferredTerm in nonPreferredTerms)
                            {
                                if (nonPreferredTerm.Text.Contains(searchTerm)) termsFound.Add(nonPreferredTerm.Text + "|" + term.Text);
                            }
                        }
                        else
                        {
                            // Non-preferred terms will only be directly returned like this from an XML file published before June 2010, which uses the pre-SKOS schema
                            termsFound.Add(term.Text + "|" + controlledList.GetPreferredTerm(term.ConceptId).Text);
                        }
                    }
                }
            }
            else
            {
                // Only a restricted set of terms are allowed, passed as semi-colon separated list of ids in the querystring.
                // Use IDs rather than terms to limit length of querystring.
                EsdTermCollection allowed = new EsdTermCollection();
                allowed.ReadString(context.Request.QueryString["allowed"]);

                // Look up each allowed term (we knew those) and its non-preferred terms (those are new)
                foreach (EsdTerm term in allowed)
                {
                    // ReadString has put the ids into the Text property, so use that for search to get the 
                    // text of the preferred term.
                    EsdTerm preferredTerm = controlledList.GetTerm(term.Text, EsdPreferredState.Preferred);
                    if (preferredTerm.Text.Contains(searchTerm)) termsFound.Add(preferredTerm.Text);

                    // Add non-preferred terms suffixing them with a separator and the appropriate preferred term
                    EsdTermCollection nonPreferredTerms = controlledList.GetNonPreferredTerms(preferredTerm.Id);
                    foreach (EsdTerm nonPreferredTerm in nonPreferredTerms)
                    {
                        if (nonPreferredTerm.Text.Contains(searchTerm)) termsFound.Add(nonPreferredTerm.Text + "|" + preferredTerm.Text);
                    }
                }

            }


            // Sort and return results
            termsFound.Sort();

            if (formatResultsAsJSON)
            {
                context.Response.Write("[");

                int len = termsFound.Count;
                for (int i = 0; i < len; i++)
                {
                    int dividerPos = termsFound[i].IndexOf("|");
                    if (dividerPos == -1)
                    {
                        // preferred term needs only a value property
                        context.Response.Write("{ \"label\": \"" + termsFound[i].Replace(searchTerm, "<em>" + searchTerm + "</em>") + "\", \"value\": \"" + termsFound[i] + "\" }");
                    }
                    else
                    {
                        string[] parts = termsFound[i].Split(new char[] { '|' }, 2, StringSplitOptions.RemoveEmptyEntries);

                        // non-preferred term needs label and value
                        context.Response.Write("{ \"label\": \"" + parts[0].Replace(searchTerm, "<em>" + searchTerm + "</em>") + " &#8211; use " + parts[1] + "\", \"value\": \"" + parts[1] + "\" }");
                    }

                    if (i < len - 1) context.Response.Write(",");
                }

                context.Response.Write("]");
            }
            else
            {
                // one-per-line format for old autocomplete plugin
                foreach (string result in termsFound)
                {
                    context.Response.Write(result + Environment.NewLine);
                }
            }
        }

        #endregion
    }
}
