using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(HSM.App_Start.DatePickerHelperBundleConfig), "RegisterBundles")]

namespace HSM.App_Start
{
	public class DatePickerHelperBundleConfig
	{
		public static void RegisterBundles()
		{
            //BundleTable.Bundles.Add(new ScriptBundle("~/bundles/datepicker").Include("~/Scripts/bootstrap-datepicker.js","~/Scripts/locales/bootstrap-datepicker.*"));
            //BundleTable.Bundles.Add(new StyleBundle("~/Content/datepicker").Include(
            //"~/Content/bootstrap-datepicker.css"));
		}
	}
}