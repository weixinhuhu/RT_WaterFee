using System.Web.Mvc;

namespace WHC.MVCWebMis.Common
{
    public static class HtmlHelpers
    {
        public static bool HasFunction(this HtmlHelper helper, string functionId)
        {
            return Permission.HasFunction(functionId);
        }

        public static bool IsAdmin()
        {
            return Permission.IsAdmin();
        }
    }
}