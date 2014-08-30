using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Web.Helpers;
using System.Web.WebPages;
using System.Globalization;
using HsmBI;
using System.Text.RegularExpressions;
using System.Reflection;

namespace HSM
{
    public class MyHelpers
    {
    }


    public class FilteredTextBox : IHtmlString
    {
        #region Filter types

        public enum FilterType
        {
            /// <summary>
            /// Check the name entered, Names will not contain number and will start with capital letter
            /// </summary>
            Fullname,
            /// <summary>
            /// Allows only alphabetic characters from a to z and A to Z and space characters.
            /// </summary>
            AlphabeticCharacter,

            /// <summary>
            /// Allows only numbers.
            /// </summary>
            Numbers,

            /// <summary>
            /// Allows numbers and the decimal separator as defined by the current culture info.
            /// </summary>
            DecimalNumbers,

            /// <summary>
            /// Allows numbers, the decimal separator and the currency group separator as defined by the current culture info.
            /// </summary>
            DecimalNumbersWithGroupSeparator,
            /// <summary>
            /// Allows email format.
            /// </summary>
            Email,
            /// <summary>
            /// Allows Web address.
            /// </summary>
            WebAddress,
            /// <summary>
            /// Allows all the characters specified in the ValidCharacters field.
            /// </summary>
            CustomValidCharacters,

            /// <summary>
            /// Allows all the characters except the ones specified in the InvalidCharacters field.
            /// </summary>
            CustomInvalidCharacters
        }

        #endregion

        #region Properties

        public HtmlHelper HtmlHelper { get; set; }

        public FilterType Type { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }

        public string ValidCharacters { get; set; }

        public string InvalidCharacters { get; set; }

        public IDictionary<string, object> HtmlAttributes { get; set; }

        #endregion

        #region Implementation of IHtmlString

        /// <summary>
        /// Returns an HTML-encoded string.
        /// </summary>
        /// <returns>
        /// An HTML-encoded string.
        /// </returns>
        public string ToHtmlString()
        {
            return Render();
        }

        #endregion

        #region Constructors

        internal FilteredTextBox(HtmlHelper htmlHelper, string name, FilterType type, string validCharacters, string invalidCharacters)
        {
            HtmlHelper = htmlHelper;
            Name = name;
            Type = type;
            ValidCharacters = validCharacters;
            InvalidCharacters = invalidCharacters;
        }

        #endregion

        #region Fluent configuration

        /// <summary>
        /// Set the value of the text box.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FilteredTextBox SetValue(object value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Set the dictionary that contains the HTML attributes to set for the element.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public FilteredTextBox SetHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            HtmlAttributes = htmlAttributes;
            return this;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return ToHtmlString();
        }

        private string Render()
        {
            var tagBuilder = new TagBuilder("input");

            if (HtmlAttributes != null)
                tagBuilder.MergeAttributes(HtmlAttributes);

            tagBuilder.MergeAttribute("type", "text");

            if (!string.IsNullOrEmpty(Name))
            {
                tagBuilder.MergeAttribute("name", Name, true);
                tagBuilder.MergeAttribute("id", Name, true);
            }

            var value = Value == null ? string.Empty : Value.ToString();
            var regex = string.Empty;
            switch (Type)
            {
                case FilterType.AlphabeticCharacter:
                    regex = "[^a-zA-Z ]";
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                case FilterType.Numbers:
                    regex = "[^0-9]";
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                case FilterType.DecimalNumbers:
                    regex = string.Format("[^0-9{0}]", Regex.Escape(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                case FilterType.DecimalNumbersWithGroupSeparator:
                    regex = string.Format("[^0-9{0}{1}]", Regex.Escape(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), Regex.Escape(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator));
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                case FilterType.Email:
                    regex = "^(([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)(\\s*;\\s*|\\s*$))*";
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                case FilterType.Fullname:
                    regex = "^[a-zA-Z]+(([\\'\\,\\.\\-][a-zA-Z])?[a-zA-Z]*)*$";
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                case FilterType.CustomValidCharacters:
                    regex = string.Format("[^{0}]", Regex.Escape(ValidCharacters));
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
                //case FilterType.CustomInvalidCharacters:
                //    regex = string.Join("|", InvalidCharacters.ToCharArray().Select(c => Regex.Escape(c.ToString(CultureInfo.InvariantCulture))));
                //    value = Regex.Replace(value, regex, string.Empty);
                //    break;
                case FilterType.CustomInvalidCharacters:
                    /*
                    regex = string.Join("|", InvalidCharacters.ToCharArray().Select(c => Regex.Escape(c.ToString(CultureInfo.InvariantCulture))));
                    replaced with the next line thanks to Siderite - http://siderite.blogspot.com/
                    */
                    regex = string.Format("[{0}]", Regex.Escape(InvalidCharacters));
                    value = Regex.Replace(value, regex, string.Empty);
                    break;
            }

            tagBuilder.MergeAttribute("class", "filtered-text form-control");
            tagBuilder.MergeAttribute("value", value, true);
            tagBuilder.MergeAttribute("data-regex", regex, true);

            if (!string.IsNullOrEmpty(Name))
            {
                // If there are any errors for a named field, we add the css attribute.
                ModelState modelState;
                if (HtmlHelper.ViewData.ModelState.TryGetValue(Name, out modelState))
                {
                    if (modelState.Errors.Count > 0)
                    {
                        tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    }
                }
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        #endregion
    }

    public static class ExtenssionMethods
    {
        /// <summary>
        /// Split Options
        /// </summary>
        /// <param name="html"></param>
        /// <param name="currentSplit"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString SplitOption(this HtmlHelper html, string currentSplit, Func<string, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            defaultcon db = new defaultcon();
          
            var choir = (from c in db.ChoirSplits 
                        select c).ToList();

            foreach (var c in choir)
            {
                // Construct an <a> tag
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(c.SplitId.ToString()));
                tag.InnerHtml = c.Description;
                tag.AddCssClass("btn btn-default btn-large");

                int nsplit = Convert.ToInt32(currentSplit);
                if (c.SplitId  == nsplit )
                {
                    tag.AddCssClass("btn btn-active btn-large");
                }
                result.AppendLine(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString PartOption(this HtmlHelper html, string currentPart, Func<string, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            defaultcon db = new defaultcon();

            var choir = (from c in db.ChoirParts 
                         select c).ToList();

            foreach (var c in choir)
            {
                // Construct an <a> tag
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(c.sn.ToString()));
                tag.InnerHtml = c.Part ;
                tag.AddCssClass("btn btn-default btn-large");

                //int npart = Convert.ToInt32(currentPart);
                //if (c.sn == npart)
                //{
                //    tag.AddCssClass("btn btn-active btn-large");
                //}
                result.AppendLine(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
        /// <summary>
        /// AlphaNumberic Pager..
        /// </summary>
        /// <param name="html"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageUrl"></param>
        /// <param name="showAllLink"></param>
        /// <returns></returns>
        public static MvcHtmlString AlphabeticalPager(this HtmlHelper html, string currentPage, Func<string, string> pageUrl, bool showAllLink = false)
        {
            StringBuilder result = new StringBuilder();


            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


            foreach (char letter in letters)
            {
                // Construct an <a> tag
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(letter.ToString().ToLower()));
                tag.InnerHtml = letter.ToString();
                tag.AddCssClass("btn btn-default btn-large alphalist");


                if (letter.ToString().ToLower() == currentPage.ToLower())
                {
                    tag.AddCssClass("btn btn-active btn-large alphalist");
                }
                result.AppendLine(tag.ToString());
            }


            if (showAllLink)
            {
                // Construct an All tag
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(string.Empty));
                tag.InnerHtml = "All";
                tag.AddCssClass("btn btn-success btn-large  alphalist");


                if (string.IsNullOrEmpty(currentPage))
                {
                    tag.AddCssClass("btn btn-active btn-large alphalist");


                }
                result.AppendLine(tag.ToString());
            }


            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// Pager - List
        /// </summary>
        /// <param name="webGrid"></param>
        /// <param name="mode"></param>
        /// <param name="firstText"></param>
        /// <param name="previousText"></param>
        /// <param name="nextText"></param>
        /// <param name="lastText"></param>
        /// <param name="numericLinksCount"></param>
        /// <param name="paginationStyle"></param>
        /// <returns></returns>
        public static HelperResult PagerList(
        this WebGrid webGrid,
        WebGridPagerModes mode = WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric,
        string firstText = null,
        string previousText = null,
        string nextText = null,
        string lastText = null,
        int numericLinksCount = 5,
        string paginationStyle = null)
        {
            return PagerList(webGrid, mode, firstText, previousText, nextText, lastText, numericLinksCount, paginationStyle, explicitlyCalled: true);
        }

        private static HelperResult PagerList(
            WebGrid webGrid,
            WebGridPagerModes mode,
            string firstText,
            string previousText,
            string nextText,
            string lastText,
            int numericLinksCount,
            string paginationStyle,
            bool explicitlyCalled)
        {

            int currentPage = webGrid.PageIndex;
            int totalPages = webGrid.PageCount;
            int lastPage = totalPages - 1;

            var ul = new TagBuilder("ul");
            ul.AddCssClass(paginationStyle);

            var li = new List<TagBuilder>();

            if (webGrid.TotalRowCount <= webGrid.PageCount)
            {
                return new HelperResult(writer =>
                {
                    writer.Write(string.Empty);
                });
            }

            if (ModeEnabled(mode, WebGridPagerModes.FirstLast))
            {
                if (String.IsNullOrEmpty(firstText))
                {
                    firstText = "First";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(0), firstText)
                };

                if (currentPage == 0)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious))
            {
                if (String.IsNullOrEmpty(previousText))
                {
                    previousText = "Prev";
                }

                int page = currentPage == 0 ? 0 : currentPage - 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(page), previousText)
                };

                if (currentPage == 0)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }


            if (ModeEnabled(mode, WebGridPagerModes.Numeric) && (totalPages > 1))
            {
                int last = currentPage + (numericLinksCount / 2);
                int first = last - numericLinksCount + 1;
                if (last > lastPage)
                {
                    first -= last - lastPage;
                    last = lastPage;
                }
                if (first < 0)
                {
                    last = Math.Min(last + (0 - first), lastPage);
                    first = 0;
                }
                for (int i = first; i <= last; i++)
                {

                    var pageText = (i + 1).ToString(CultureInfo.InvariantCulture);
                    var part = new TagBuilder("li")
                    {
                        InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(i), pageText)
                    };

                    if (i == currentPage)
                    {
                        part.MergeAttribute("class", "active");
                    }

                    li.Add(part);

                }
            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious))
            {
                if (String.IsNullOrEmpty(nextText))
                {
                    nextText = "Next";
                }

                int page = currentPage == lastPage ? lastPage : currentPage + 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(page), nextText)
                };

                if (currentPage == lastPage)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.FirstLast))
            {
                if (String.IsNullOrEmpty(lastText))
                {
                    lastText = "Last";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(lastPage), lastText)
                };

                if (currentPage == lastPage)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            ul.InnerHtml = string.Join("", li);

            var html = "";
            if (explicitlyCalled && webGrid.IsAjaxEnabled)
            {
                var span = new TagBuilder("span");
                span.MergeAttribute("data-swhgajax", "true");
                span.MergeAttribute("data-swhgcontainer", webGrid.AjaxUpdateContainerId);
                span.MergeAttribute("data-swhgcallback", webGrid.AjaxUpdateCallback);

                span.InnerHtml = ul.ToString();
                html = span.ToString();

            }
            else
            {
                html = ul.ToString();
            }

            return new HelperResult(writer =>
            {
                writer.Write(html);
            });
        }

        private static String GridLink(WebGrid webGrid, string url, string text)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.MergeAttribute("href", url  );
            if (webGrid.IsAjaxEnabled)
            {
                builder.MergeAttribute("data-swhglnk", "true");
            }
            return builder.ToString(TagRenderMode.Normal);
        }


        private static bool ModeEnabled(WebGridPagerModes mode, WebGridPagerModes modeCheck)
        {
            return (mode & modeCheck) == modeCheck;
        }

        #region HtmlHelperForFilters
        /// <summary>
        /// Email - Email format textbox
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FilteredTextBox Email(this HtmlHelper htmlHelper, string name)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.Email, string.Empty, string.Empty);
        }

        /// <summary>
        /// Fullname with some checks
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FilteredTextBox Fullname(this HtmlHelper htmlHelper, string name)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.Fullname, string.Empty, string.Empty);
        }

        /// <summary>
        /// Alphabetic - Character textbox
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FilteredTextBox AlphabeticCharactersTextBox(this HtmlHelper htmlHelper, string name)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.AlphabeticCharacter, string.Empty, string.Empty);
        }

        public static FilteredTextBox NumbersTextBox(this HtmlHelper htmlHelper, string name)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.Numbers, string.Empty, string.Empty);
        }

        public static FilteredTextBox DecimalNumbersTextBox(this HtmlHelper htmlHelper, string name)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.DecimalNumbers, string.Empty, string.Empty);
        }

        public static FilteredTextBox DecimalNumbersWithGroupSeparatorTextBox(this HtmlHelper htmlHelper, string name)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.DecimalNumbersWithGroupSeparator, string.Empty, string.Empty);
        }

        public static FilteredTextBox ValidCharactersFilteredTextBox(this HtmlHelper htmlHelper, string name, string validCharacters)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.CustomValidCharacters, validCharacters, string.Empty);
        }

        public static FilteredTextBox InvalidCharactersFilteredTextBox(this HtmlHelper htmlHelper, string name, string invalidCharacters)
        {
            return new FilteredTextBox(htmlHelper, name, FilteredTextBox.FilterType.CustomInvalidCharacters, string.Empty, invalidCharacters);
        }
        #endregion

        #region Autocomplete
        public static string AutoCompleteTextBox(this HtmlHelper html, string textBoxName, string fieldName, string selectedText, object selectedValue, object htmlAttributes)
        {

            return string.Format("{0} {1}", html.TextBox(textBoxName, selectedText, htmlAttributes), html.Hidden(fieldName, selectedValue));
        }

        public static string AutoCompleteTextBox(this HtmlHelper html, string textBoxName, string fieldName, object htmlAttributes)
        {

            return string.Format("{0} {1}", html.TextBox(textBoxName, null, htmlAttributes), html.Hidden(fieldName));
        }
        //public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] arguments)
        //{

        //    string value = String.Format(format, arguments);

        //    builder.AppendLine(value);

        //    return builder;
        //}

        public static void AppendLineFormat(this StringBuilder stringBuilder, string source, params object[] args)
        {
            stringBuilder.AppendFormat(source + "\r\n", args);
        }
        public static string InitializeAutoComplete(this HtmlHelper html, string textBoxName, string fieldName, string url, object options, bool wrapInReady)
        {

            StringBuilder sb = new StringBuilder();
            if (wrapInReady) sb.AppendLineFormat("<script language='javascript'>");

            if (wrapInReady) sb.AppendLineFormat("$().ready(function() {{");
            sb.AppendLine();
            sb.AppendLineFormat("   $('#{0}').autocomplete('{1}', {{", textBoxName.Replace(".", "\\\\."), url);

            PropertyInfo[] properties = options.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                sb.AppendLineFormat("   {0} : {1}{2}",
                                        properties[i].Name,
                                        properties[i].GetValue(options, null),
                                        i != properties.Length - 1 ? "," : "");
            }
            sb.AppendLineFormat("   }});");
            sb.AppendLine();
            sb.AppendLineFormat("   $('#{0}').result(function(e, d, f) {{", textBoxName.Replace(".", "\\\\."));
            sb.AppendLineFormat("       $('#{0}').val(d[1]);", fieldName);
            sb.AppendLineFormat("    }});");
            sb.AppendLine();
            if (wrapInReady) sb.AppendLineFormat("}});");
            if (wrapInReady) sb.AppendLineFormat("</script>");
            return sb.ToString();

        }

        public static string InitializeAutoComplete(this HtmlHelper html, string textBoxName, string fieldName, string url, object options)
        {
            return InitializeAutoComplete(html, textBoxName, fieldName, url, options, false);
        }
        #endregion
    }
    

}