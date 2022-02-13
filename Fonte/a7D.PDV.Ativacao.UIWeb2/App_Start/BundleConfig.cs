using AspNetBundling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace a7D.PDV.Ativacao.UIWeb2
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap")
                .Include("~/Content/bootstrap.min.css")
                .Include("~/Content/ng-table.min.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/Bootstrap")
                    .Include("~/Scripts/underscore.min.js")
                    .Include("~/Scripts/jquery-2.2.4.min.js")
                    .Include("~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/AngularJS")
                .Include("~/Scripts/angular.min.js")
                .Include("~/Scripts/angular-route.min.js")
                .Include("~/Scripts/angular-cookies.min.js")
                .Include("~/Scripts/angular-animate.min.js")
                .Include("~/Scripts/angular-resource.min.js")
                .Include("~/Scripts/angular-touch.min.js")
                .Include("~/Scripts/angular-translate.min.js")
                .Include("~/Scripts/ui-bootstrap.min.js")
                .Include("~/Scripts/ng-table.min.js")
                .Include("~/Scripts/angular-jwt.min.js")
                .Include("~/Scripts/ui-mask.min.js"));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/moment")
                .Include("~/Scripts/moment.min.js")
                .Include("~/Scripts/moment-with-locales.min.js"));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/Ativacao")
                .Include("~/Scripts/ng/AtivacaoApp.js")
                .Include("~/Scripts/ng/filters.js")
                .IncludeDirectory("~/Scripts/ng/Filters", "*.js")
                .IncludeDirectory("~/Scripts/ng/Services", "*.js")
                .IncludeDirectory("~/Scripts/ng/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/ng/Directives", "*.js")
                );
        }
    }
}