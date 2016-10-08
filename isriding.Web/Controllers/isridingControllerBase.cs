using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Web.Mvc.Controllers;

namespace isriding.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class isridingControllerBase : AbpController
    {
        protected isridingControllerBase()
        {
            LocalizationSourceName = isridingConsts.LocalizationSourceName;
        }

        /// <summary>
        /// Retuan DataTable Result
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iTotalRecords"></param>
        /// <param name="iTotalDisplayRecords"></param>
        /// <param name="aaData"></param>
        /// <returns></returns>
        protected ActionResult DataTableJsonResult(string sEcho, int iDisplayStart,
            int iTotalRecords, int iTotalDisplayRecords, IEnumerable<string[]> aaData)
        {
            return Json(new
            {
                sEcho = sEcho,
                iDisplayStart = iDisplayStart,
                iTotalRecords = iTotalRecords,
                iTotalDisplayRecords = iTotalDisplayRecords,
                aaData = aaData
            }, JsonRequestBehavior.AllowGet);
        }
    }
}