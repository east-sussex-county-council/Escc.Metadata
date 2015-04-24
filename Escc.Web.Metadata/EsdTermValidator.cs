using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.Web.Metadata
{
    /// <summary>
    /// Checks that one or more terms exist in a specified controlled list from the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
    /// </summary>
    /// <remarks>
    /// <example>
    /// <para>The first example shows how to reference this control on an ASP.net web page.</para>
    /// <code>
    ///		&lt;%@ Register TagPrefix=&quot;Egms&quot; Namespace=&quot;eastsussexgovuk.webservices.EgmsWebMetadata&quot; Assembly=&quot;eastsussexgovuk.webservices.EgmsWebMetadata&quot; %&gt;
    ///		
    ///		&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
    ///		&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot; dir=&quot;ltr&quot; xml:lang=&quot;en&quot;&gt;
    ///			&lt;body&gt;
    ///
    ///				&lt;asp:ValidationSummary id="valSummary" runat="server" EnableClientScript="False" ShowSummary="True" DisplayMode="BulletList"&gt;&lt;/asp:validationsummary&gt;
    ///	
    ///				&lt;asp:TextBox id="ipsvTerms" runat="server" /&gt;
    ///
    ///				&lt;Egms:EsdTermValidator id=&quot;termValidator&quot; runat=&quot;server&quot; 
    ///					ControlToValidate=&quot;ipsvTerms&quot; 
    ///					ControlledListName=&quot;IPSV&quot; 
    ///					MultipleTerms=&quot;true&quot; 
    ///					PreferredState=&quot;Preferred&quot;
    ///					TermIdsAllowed=&quot;true&quot;
    ///					/&gt;
    ///					
    ///			&lt;/body&gt;
    ///		&lt;/html&gt;
    /// </code>
    /// </example>
    /// <example>
    /// <para>This validator requires an XML file of any controlled list it validates against. Suitable XML files are all available from the Electronic Service Delivery (ESD) Standards site at 
    /// <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.</para>
    /// <para>The second example shows how to specify the paths to these XML files using the <c>Escc.Web.Metadata/ControlledListXml</c> element in <b>web.config</b>.</para>
    /// <code>
    ///		&lt;configSections&gt;
    ///         &lt;sectionGroup name=&quot;Escc.Web.Metadata&quot;&gt;
    ///			    &lt;section name=&quot;ControlledListXml&quot; type=&quot;System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089&quot;/&gt;
    ///		    &lt;/sectionGroup&gt;
    ///		&lt;/configSections&gt;
    ///		
    ///		&lt;Escc.Web.Metadata&gt;
    ///	    	&lt;ControlledListXml&gt;
    ///		    	&lt;add key=&quot;Ipsv&quot; value=&quot;C:\folder\ipsv.xml&quot; /&gt;
    ///		    	&lt;add key=&quot;Lgcl&quot; value=&quot;C:\folder\lgcl.xml&quot; /&gt;
    ///		    	&lt;add key=&quot;Gcl&quot; value=&quot;C:\folder\gcl.xml&quot; /&gt;
    ///		    	&lt;add key=&quot;Lgsl&quot; value=&quot;C:\folder\lgsltermslist.xml&quot; /&gt;
    ///		    	&lt;add key=&quot;Lgal&quot; value=&quot;C:\folder\lgal.xml&quot; /&gt;
    ///		    	&lt;add key=&quot;Lgtl&quot; value=&quot;C:\folder\lgtl.xml&quot; /&gt;
    ///		    	&lt;add key=&quot;Lgil&quot; value=&quot;C:\folder\lgil.xml&quot; /&gt;
    ///		    &lt;/ControlledListXml&gt;
    ///		&lt;/Escc.Web.Metadata&gt;
    /// </code>
    /// </example>
    /// </remarks>
    public class EsdTermValidator : BaseValidator
    {
        private EsdTermCollection matchedTerms = new EsdTermCollection();
        private EsdTermCollection allowedTerms = new EsdTermCollection();
        private string controlledListName;
        private bool multipleTerms;
        private EsdPreferredState preferredState = EsdPreferredState.Preferred;
        private ArrayList invalidTerms;
        private bool termIdsAllowed;
        private bool? cachedIsValid = null;

        /// <summary>
        /// Gets the terms not matched by the validator when it validated the control
        /// </summary>
        public ArrayList InvalidTerms
        {
            get
            {
                return this.invalidTerms;
            }
        }

        /// <summary>
        /// Gets the preferred terms which are allowed. If the collection is empty, any preferred term is allowed.
        /// </summary>
        /// <value>The allowed terms.</value>
        public EsdTermCollection AllowedTerms
        {
            get { return this.allowedTerms; }
        }

        /// <summary>
        /// Gets or sets whether to validate preferred, non-preferred or any terms
        /// </summary>
        public EsdPreferredState PreferredState
        {
            get
            {
                return this.preferredState;
            }
            set
            {
                this.preferredState = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to allow multiple semi-colon separated terms
        /// </summary>
        public bool MultipleTerms
        {
            get
            {
                return this.multipleTerms;
            }
            set
            {
                this.multipleTerms = value;
            }
        }

        /// <summary>
        /// Gets or sets the abbreviated name of the ESD controlled list to validate against
        /// </summary>
        public string ControlledListName
        {
            get
            {
                return this.controlledListName;
            }
            set
            {
                this.controlledListName = value;
            }
        }

        /// <summary>
        /// Gets the terms matched by the validator when it validated the control
        /// </summary>
        public EsdTermCollection MatchedTerms
        {
            get
            {
                return this.matchedTerms;
            }
        }

        /// <summary>
        /// Gets or sets whether the control can include valid term ids
        /// </summary>
        public bool TermIdsAllowed
        {
            get { return this.termIdsAllowed; }
            set { this.termIdsAllowed = value; }
        }

        /// <summary>
        /// Creators a validtor to check one or more terms exists against an ESD controlled list.
        /// </summary>
        public EsdTermValidator()
        {
        }

        /// <summary>
        /// Creators a validtor to check one or more terms exists against an ESD controlled list.
        /// </summary>
        /// <param name="controlToValidateId">Id of the input control to validate</param>
        public EsdTermValidator(string controlToValidateId)
        {
            this.ControlToValidate = controlToValidateId;
        }

        /// <summary>
        /// Sets standard properties when the control is initiated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;

        }

        /// <summary>
        /// Determines whether the control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        /// <returns>
        /// true if the control specified by <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> is a valid control; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">
        /// No value is specified for the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property.
        /// - or -
        /// The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is not found on the page.
        /// - or -
        /// The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property does not have a <see cref="T:System.Web.UI.ValidationPropertyAttribute"/> attribute associated with it; therefore, it cannot be validated with a validation control.
        /// </exception>
        protected override bool ControlPropertiesValid()
        {
            try
            {
                return base.ControlPropertiesValid();
            }
            catch (HttpException)
            {
                // Code in EvaluateIsValid is written to support *all* ListControl decendants including CheckBoxList,
                // which .NET says is not validatable. This catches that exception and allows a CheckBoxList to be validated.
                if (this.Parent.FindControl(this.ControlToValidate) is ListControl) return true;
                throw;
            }
        }

        /// <summary>
        /// Checks the terms in the control against the specified controlled list
        /// </summary>
        /// <returns>true if all terms are valid; false if any one term is not valid (matching is not case sensitive)</returns>
        protected override bool EvaluateIsValid()
        {
            // If an instance of this validator is run more than once on one request, just return the same result and don't do all the work again.
            // Not that anyone would run it more than once on one request, unless it was really inefficient (SharePoint field controls).
            if (cachedIsValid != null) return (bool)cachedIsValid;

            // check the list was specified
            if (this.controlledListName == null || this.controlledListName.Length == 0) throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The ControlledListName property was not set for {0}", this.GetType().ToString()));

            // get control to validate and get its value
            Control controlToValidate = this.Parent.FindControl(this.ControlToValidate);

            string valueToValidate = String.Empty;

            // If it's a textbox ensure it's in a variable we can use later to put a tidied-up value back into the textbox
            TextBox textboxToValidate = controlToValidate as TextBox;
            if (textboxToValidate != null)
            {
                // Trim off any trailing semi-colon separator
                valueToValidate = textboxToValidate.Text.TrimEnd(' ', ';');
            }
            else
            {
                // Maybe it's a list of some kind? Test with cast rather than .GetType() == typeof(ListControl) 
                // because need to support customised derived types of control.
                ListControl listToValidate = controlToValidate as ListControl;
                if (listToValidate != null)
                {
                    EsdTermCollection selectedTerms = new EsdTermCollection();
                    foreach (ListItem item in listToValidate.Items)
                    {
                        if (item.Selected)
                        {
                            EsdTerm selectedTerm = new EsdTerm();
                            selectedTerm.Id = item.Value;
                            selectedTerm.Text = item.Text;
                            selectedTerms.Add(selectedTerm);
                        }
                    }
                    valueToValidate = selectedTerms.ToString();
                }
                else
                {
                    // No other control type supported yet - throw a useful error for the developer
                    throw new ArgumentException(String.Format("The control type matched by the ControlToValidate property, {0}, is not supported by the EsdTermValidator", controlToValidate.GetType().ToString()));
                }
            }


            // this isn't a required validator
            if (valueToValidate.Length == 0)
            {
                this.cachedIsValid = true;
                return true;
            }

            // get the XML we need
            EsdControlledList esdListXml = EsdControlledList.GetControlledList(this.controlledListName, false);

            // box has semi-colon separated list of preferred terms
            string[] searchTerms;
            if (this.multipleTerms)
            {
                valueToValidate = valueToValidate.Trim(' ', ';');
                searchTerms = valueToValidate.Split(';');
            }
            else
            {
                searchTerms = new string[1];
                searchTerms[0] = valueToValidate.Trim();
            }

            // If we only want preferred terms, search for any term then we can convert it to the preferred term
            EsdPreferredState searchState = (this.preferredState == EsdPreferredState.Preferred ? EsdPreferredState.Any : this.preferredState);

            // Search for terms and gather results
            StringBuilder termBuilder = new StringBuilder();
            this.invalidTerms = new ArrayList();
            this.matchedTerms.Clear();
            List<string> matchedTermIds = new List<string>();
            int totalMatchedTerms = 0;
            string trimmedSearch;
            Regex numeric = new Regex("^[0-9]+$");

            foreach (string searchTerm in searchTerms)
            {
                trimmedSearch = searchTerm.Trim();
                if (trimmedSearch.Length == 0) continue;

                if (this.termIdsAllowed && numeric.IsMatch(trimmedSearch))
                {
                    if (this.allowedTerms.Count > 0)
                    {
                        // Validate the text of the term against just those terms in the AllowedTerms collection
                        int termIndex = this.allowedTerms.IndexOfId(trimmedSearch, this.preferredState);

                        // If that didn't work and we want preferred terms, see if it matches a non-preferred term and the preferred term is allowed
                        if (this.preferredState == EsdPreferredState.Preferred && termIndex == -1)
                        {
                            EsdTerm matchedTerm = esdListXml.GetTerm(trimmedSearch, EsdPreferredState.NonPreferred);
                            if (matchedTerm != null)
                            {
                                matchedTerm = esdListXml.GetPreferredTerm(matchedTerm.ConceptId);
                                if (matchedTerm != null)
                                {
                                    termIndex = this.allowedTerms.IndexOf(matchedTerm.Text, EsdPreferredState.Preferred);
                                }
                            }
                        }

                        if (termIndex > -1)
                        {
                            EsdTerm matchedTerm = this.allowedTerms[termIndex];
                            totalMatchedTerms++;

                            // Add it unless it's a duplicate
                            if (!matchedTermIds.Contains(matchedTerm.Id))
                            {
                                this.matchedTerms.Add(matchedTerm);

                                if (termBuilder.Length > 0) termBuilder.Append("; ");
                                termBuilder.Append(matchedTerm.Id);

                                matchedTermIds.Add(matchedTerm.Id);
                            }
                        }
                        else
                        {
                            this.invalidTerms.Add(trimmedSearch);

                            if (termBuilder.Length > 0) termBuilder.Append("; ");
                            termBuilder.Append(trimmedSearch);
                        }
                    }
                    else
                    {
                        EsdTerm matchedTerm = esdListXml.GetTerm(trimmedSearch, searchState);
                        if (matchedTerm != null)
                        {
                            totalMatchedTerms++;

                            // If we want preferred terms, ensure we have the preferred term
                            if (this.preferredState == EsdPreferredState.Preferred && !matchedTerm.Preferred)
                            {
                                matchedTerm = esdListXml.GetPreferredTerm(matchedTerm.ConceptId);
                            }

                            // Add it unless it's a duplicate
                            if (!matchedTermIds.Contains(matchedTerm.Id))
                            {
                                this.matchedTerms.Add(matchedTerm);

                                if (termBuilder.Length > 0) termBuilder.Append("; ");
                                termBuilder.Append(matchedTerm.Id);

                                matchedTermIds.Add(matchedTerm.Id);
                            }
                        }
                        else
                        {
                            this.invalidTerms.Add(trimmedSearch);

                            if (termBuilder.Length > 0) termBuilder.Append("; ");
                            termBuilder.Append(trimmedSearch);
                        }
                    }
                }
                else
                {
                    if (this.allowedTerms.Count > 0)
                    {
                        // Validate the text of the term against just those terms in the AllowedTerms collection
                        int termIndex = this.allowedTerms.IndexOf(trimmedSearch, this.preferredState);

                        // If that didn't work and we want preferred terms, see if it matches a non-preferred term and the preferred term is allowed
                        if (this.preferredState == EsdPreferredState.Preferred && termIndex == -1)
                        {
                            EsdTermCollection matchedTerms = esdListXml.GetTerms(trimmedSearch, true, EsdPreferredState.NonPreferred);
                            if (matchedTerms.Count == 1)
                            {
                                EsdTerm matchedTerm = esdListXml.GetPreferredTerm(matchedTerms[0].ConceptId);
                                if (matchedTerm != null)
                                {
                                    termIndex = this.allowedTerms.IndexOf(matchedTerm.Text, EsdPreferredState.Preferred);
                                }
                            }
                        }

                        if (termIndex > -1)
                        {
                            EsdTerm matchedTerm = this.allowedTerms[termIndex];
                            totalMatchedTerms++;

                            // Add it unless it's a duplicate
                            if (!matchedTermIds.Contains(matchedTerm.Id))
                            {
                                this.matchedTerms.Add(matchedTerm);

                                if (termBuilder.Length > 0) termBuilder.Append("; ");
                                termBuilder.Append(matchedTerm.Text);

                                matchedTermIds.Add(matchedTerm.Id);
                            }
                        }
                        else
                        {
                            this.invalidTerms.Add(trimmedSearch);

                            if (termBuilder.Length > 0) termBuilder.Append("; ");
                            termBuilder.Append(trimmedSearch);
                        }
                    }
                    else
                    {
                        // Validate the text of the term against the full controlled list
                        EsdTermCollection terms = esdListXml.GetTerms(trimmedSearch, true, searchState);

                        if (terms.Count == 1)
                        {
                            totalMatchedTerms++;

                            // If we want preferred terms, ensure we have the preferred term
                            EsdTerm matchedTerm = terms[0];
                            if (this.preferredState == EsdPreferredState.Preferred && !matchedTerm.Preferred)
                            {
                                matchedTerm = esdListXml.GetPreferredTerm(matchedTerm.ConceptId);
                            }

                            // Add it unless it's a duplicate
                            if (!matchedTermIds.Contains(matchedTerm.Id))
                            {
                                this.matchedTerms.Add(matchedTerm);

                                if (termBuilder.Length > 0) termBuilder.Append("; ");
                                termBuilder.Append(matchedTerm.Text);

                                matchedTermIds.Add(matchedTerm.Id);
                            }
                        }
                        else
                        {
                            this.invalidTerms.Add(trimmedSearch);

                            if (termBuilder.Length > 0) termBuilder.Append("; ");
                            termBuilder.Append(trimmedSearch);
                        }
                    }
                }
            }

            // if control had user-editable (rather than selectable) values, update control with case-corrected terms
            if (textboxToValidate != null)
            {
                textboxToValidate.Text = termBuilder.ToString();
            }

            // return true if terms valid; false if any one term is not valid
            // totalMatchedTerms may not be the same as this.MatchedTerms.Count because totalMatchedTerms is incremented for a duplicate
            if (totalMatchedTerms == searchTerms.Length)
            {
                this.cachedIsValid = true;
                return true; // all terms were matched
            }
            else
            {
                StringBuilder errorBuilder = new StringBuilder();
                string listName = esdListXml.AbbreviatedName;
                string termDelimiter = "'";

                // build error message with singular grammar
                if (this.invalidTerms.Count == 1)
                {
                    if (this.allowedTerms.Count > 0)
                    {
                        errorBuilder.Append(termDelimiter).Append(this.invalidTerms[0]).Append(termDelimiter).Append(" is not one of the ").Append(listName).Append(" terms allowed here");
                    }
                    else
                    {
                        errorBuilder.Append(termDelimiter).Append(this.invalidTerms[0]).Append(termDelimiter).Append(" is not a");
                        if (listName.StartsWith("A") || listName.StartsWith("E") || listName.StartsWith("I") || listName.StartsWith("O") || listName.StartsWith("U")) errorBuilder.Append("n");
                        errorBuilder.Append(" ").Append(listName).Append(" ");
                        if (this.preferredState == EsdPreferredState.Preferred) errorBuilder.Append("preferred");
                        else if (this.preferredState == EsdPreferredState.NonPreferred) errorBuilder.Append("non-preferred");
                        errorBuilder.Append(" term");
                    }
                }
                else
                {
                    // build error message with plural grammar
                    for (short i = 0; i < this.invalidTerms.Count; i++)
                    {
                        if (errorBuilder.Length > 0)
                        {
                            if (i == (this.invalidTerms.Count - 1)) errorBuilder.Append(" and ");
                            else errorBuilder.Append(", ");
                        }
                        errorBuilder.Append(termDelimiter).Append(this.invalidTerms[i]).Append(termDelimiter);
                    }

                    if (this.allowedTerms.Count > 0)
                    {
                        errorBuilder.Append(" are not among the ").Append(listName).Append(" terms allowed here");
                    }
                    else
                    {
                        errorBuilder.Append(" are not ").Append(listName).Append(" ");
                        if (this.preferredState == EsdPreferredState.Preferred) errorBuilder.Append("preferred");
                        else if (this.preferredState == EsdPreferredState.NonPreferred) errorBuilder.Append("non-preferred");
                        errorBuilder.Append(" terms");
                    }
                }

                this.ErrorMessage = errorBuilder.ToString();

                this.cachedIsValid = false;
                return false;
            }
        }

    }
}
