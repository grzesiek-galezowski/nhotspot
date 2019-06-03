﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ResultRendering
{
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class HtmlTemplate : HtmlTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"doctype html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <style>
        p {
            padding: 0;
            margin: 0;
        }

        ol {
            -ms-columns: 2;
            -webkit-columns: 2;
            -moz-columns: 2;
            columns: 2;
            font-weight: normal
        }

        body {
            font-family: Arial !important;
        }

		
        .unfolder { display: none; }

        .toggle-label {
            display: inline-block;
            cursor: pointer;
            font-size: 11px;
            border-radius: 5px;
            padding: 5px;
        }
        .unfold-icon, .fold-icon {
            color: #999;
            width: 10px;
            display: inline-block;
        }
        .unfolder ~ .fold {
            display: none;
        }
        .unfolder ~ label .fold-icon {
            display: none;
        }

        .unfolder:checked ~ .fold {
            display: block;
        }
        .unfolder:checked ~ label .fold-icon {
            display: inline-block;
        }
        .unfolder:checked ~ label .unfold-icon {
            display: none;
        }
    </style>
    <title>Line Chart Test</title>
    <script src=""https://cdn.jsdelivr.net/npm/chart.js@2.8.0""></script>
    <meta charset=""UTF-8"">
</head>
<body>
<h1>Analysis of ");
            
            #line 61 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_viewModel.RepoName));
            
            #line default
            #line hidden
            this.Write("</h1>\r\n    \r\n<h1>Rankings</h1>\r\n\r\n");
            
            #line 65 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
 for(int i = 0 ; i < _viewModel.Rankings.Count ; ++i) {
            
            #line default
            #line hidden
            this.Write("\t\r\n");
            
            #line 66 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
 var ranking = _viewModel.Rankings[i]; 
            
            #line default
            #line hidden
            this.Write("<div>\r\n    <input type=\"checkbox\" id=\"toggle");
            
            #line 68 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i));
            
            #line default
            #line hidden
            this.Write("\" class=\"unfolder\"/> \r\n    <label for=\"toggle");
            
            #line 69 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i));
            
            #line default
            #line hidden
            this.Write("\" class=\"toggle-label\"><h2>");
            
            #line 69 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ranking.Title));
            
            #line default
            #line hidden
            this.Write("</h2><span class=\"unfold-icon\">&#9654;</span><span class=\"fold-icon\">&#9660;</spa" +
                    "n></label>\r\n    \t\t\r\n    <div class=\"fold\">\r\n\t\t<ol>\r\n\t    ");
            
            #line 73 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
  foreach(var entry in ranking.Entries) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t<li>");
            
            #line 74 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entry.Name));
            
            #line default
            #line hidden
            this.Write(" (");
            
            #line 74 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entry.Value));
            
            #line default
            #line hidden
            this.Write(")</li>\r\n\t\t");
            
            #line 75 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
	} 
            
            #line default
            #line hidden
            this.Write("\t    </ol>\r\n    </div>\r\n");
            
            #line 78 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
 }
            
            #line default
            #line hidden
            this.Write("</div>\r\n\r\n\r\n\r\n<h1>Hot Spots</h1>\r\n\r\n");
            
            #line 85 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
  foreach(var hotSpot in _viewModel.HotSpots) {
            
            #line default
            #line hidden
            this.Write("\r\n\t<h2>");
            
            #line 87 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Rank));
            
            #line default
            #line hidden
            this.Write(". ");
            
            #line 87 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Path));
            
            #line default
            #line hidden
            this.Write("</h2>\r\n\r\n\t<p>COMPLEXITY: ");
            
            #line 89 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Complexity));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>CHANGES: ");
            
            #line 90 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.ChangesCount));
            
            #line default
            #line hidden
            this.Write("</p>\r\n\t<p>CREATED: ");
            
            #line 91 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Age));
            
            #line default
            #line hidden
            this.Write(" ago</p>\r\n\t<p>LAST CHANGED: ");
            
            #line 92 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.TimeSinceLastChanged));
            
            #line default
            #line hidden
            this.Write(" ago</p>\r\n\t<p>ACTIVE FOR: ");
            
            #line 93 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.ActivePeriod));
            
            #line default
            #line hidden
            this.Write(" (First commit: ");
            
            #line 93 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.CreationDate));
            
            #line default
            #line hidden
            this.Write(", Last: ");
            
            #line 93 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.LastChangedDate));
            
            #line default
            #line hidden
            this.Write(")</p>\r\n\r\n\r\n\t<div class=\"container\">\r\n\t    <canvas id=\"myChart");
            
            #line 97 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Rank));
            
            #line default
            #line hidden
            this.Write("\" height=\"40\"></canvas>\r\n\t</div>\r\n\t<script>\r\n\t    var ctx = document.getElementBy" +
                    "Id(\'myChart");
            
            #line 100 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Rank));
            
            #line default
            #line hidden
            this.Write(@"').getContext('2d');
	    var chart = new Chart(ctx, {
	        // The type of chart we want to create
	        type: 'line',
	        options: {
	            elements: {
	                line: {
	                    tension: 0 // disables bezier curves
	                }
	            }
	        },
	        // The data for our dataset
	        data: {
	            labels: [");
            
            #line 113 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Labels));
            
            #line default
            #line hidden
            this.Write("], //example \'1\', \'2\', \'3\'\r\n\t            datasets: [{\r\n\t                label: \'");
            
            #line 115 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.ChartValueDescription));
            
            #line default
            #line hidden
            this.Write("\',\r\n\t                fill: false,\r\n\t                borderColor: \'rgb(255, 99, 13" +
                    "2)\',\r\n\t                data: [");
            
            #line 118 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(hotSpot.Data));
            
            #line default
            #line hidden
            this.Write("]\r\n\t            }]\r\n\t        },\r\n\r\n\t    });\r\n\t</script>\r\n    \r\n");
            
            #line 125 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
  } 
            
            #line default
            #line hidden
            this.Write("\r\n\r\n</body>\r\n</html>\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 130 "C:\Users\grzes\Documents\GitHub\nhotspot\src\GitAttempt2\ResultRendering\HtmlTemplate.tt"
private ViewModel _viewModel; 
        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class HtmlTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
