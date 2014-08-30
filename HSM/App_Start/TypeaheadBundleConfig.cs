using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(HSM.App_Start.TypeaheadBundleConfig), "RegisterBundles")]

namespace HSM.App_Start
{

	public class TypeaheadBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Scripts.Render("~/bundles/typeahead") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, MVC 5 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/typeahead").Include("~/Scripts/typeahead.bundle*", 
                "~/Scripts/bloodhound*", 
                "~/Scripts/typeahead.jquery*"));
			//BundleTable.Bundles.Add(new ScriptBundle("~/bundles/typeahead-jquery").Include());
		}
	}
}
