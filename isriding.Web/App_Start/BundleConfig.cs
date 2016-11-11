using System.Web.Optimization;

namespace isriding.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = false;
            //VENDOR RESOURCES

            //~/Bundles/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/vendor/css")
                    .Include("~/Content/themes/base/all.css", new CssRewriteUrlTransform())
                    .Include("~/Content/bootstrap-cosmo.min.css", new CssRewriteUrlTransform())
                    .Include("~/Content/toastr.min.css")
                    .Include("~/Scripts/sweetalert/sweet-alert.css")
                    .Include("~/Content/flags/famfamfam-flags.css", new CssRewriteUrlTransform())
                    .Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform())
                );

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/assets/plugins/jquery-migrate-1.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //~/Bundles/vendor/js/top (These scripts should be included in the head of the page)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/top")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        "~/Scripts/modernizr-2.8.3.js"
                    )
                );

            //~/Bundles/vendor/bottom (Included in the bottom for fast page load)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/bottom")
                    .Include(
                        "~/Scripts/json2.min.js",

                        "~/Scripts/jquery-2.1.4.min.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js",

                        "~/Scripts/bootstrap.min.js",

                        //"~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/sweetalert/sweet-alert.min.js",
                        "~/Scripts/others/spinjs/spin.js",
                        "~/Scripts/others/spinjs/jquery.spin.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js"
                    )
                );

            //APPLICATION RESOURCES

            //~/Bundles/css
            bundles.Add(
                new StyleBundle("~/Bundles/css")
                    .Include("~/css/main.css")
                );

            //~/Bundles/js
            bundles.Add(
                new ScriptBundle("~/Bundles/js")
                    .Include("~/js/main.js")
                );

            //CSS 样式
            bundles.Add(new StyleBundle("~/bundles/admin/css").Include(
                      //全局样式
                      "~/Content/assets/plugins/font-awesome/css/font-awesome.min.css",
                      "~/Content/assets/plugins/bootstrap/css/bootstrap.min.css",
                      "~/Content/datatables.min.css",
                      //表单美化
                      "~/Content/assets/plugins/uniform/css/uniform.default.css",
                      //主题样式
                      "~/Content/assets/css/style-metronic.css",
                      "~/Content/assets/css/style.css",
                      "~/Content/assets/css/style-responsive.css",
                      "~/Content/assets/css/admin.main.css",
                      "~/Content/assets/css/admin.main-responsive.css",
                      "~/Content/assets/css/plugins.css",
                      "~/Content/assets/css/pages/tasks.css",
                      "~/Content/assets/css/custom.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/admin/grid/css").Include(
                      "~/Content/assets/plugins/select2/select2_metro.css",
                      "~/Content/assets/plugins/data-tables/DT_bootstrap.css"
                      ));

            //JS 脚本
            bundles.Add(new ScriptBundle("~/bundles/admin/js").Include(
                        //IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
                        "~/Content/assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js",
                        "~/Content/assets/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Content/assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js",
                        "~/Content/assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/assets/plugins/jquery.blockui.min.js",
                        "~/Content/assets/plugins/jquery.cokie.min.js",
                        "~/Content/assets/plugins/uniform/jquery.uniform.min.js",
                        "~/Content/assets/scripts/app.js",
                        "~/Content/assets/scripts/admin.main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/admin/grid/js").Include(
                       "~/Content/assets/plugins/select2/select2.min.js",
                       "~/Content/assets/plugins/data-tables/jquery.dataTables.js",
                       "~/Content/assets/plugins/data-tables/jquery.dataTables.AjaxSource.min.js",
                       "~/Content/assets/plugins/data-tables/DT_bootstrap.js",
                       "~/Content/assets/scripts/table-managed.js"
                       ));
        }
    }
}