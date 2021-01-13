using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace Banorte
{
    public class BundleConfig
    {
        // Para obtener más información sobre la unión, visite http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms",
                            "~/Scripts/WebForms/WebUIValidation",
                            "~/Scripts/WebForms/MenuStandards",
                            "~/Scripts/WebForms/Focus",
                            "~/Scripts/WebForms/GridView",
                            "~/Scripts/WebForms/DetailsView",
                            "~/Scripts/WebForms/TreeView",
                            "~/Scripts/WebForms/WebParts"));

            // El orden es muy importante para el funcionamiento de estos archivos ya que tienen dependencias explícitas
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use la versión de desarrollo de Modernizr para desarrollar y aprender. Luego, cuando esté listo
            // para la producción, use la herramienta de creación en http://modernizr.com para elegir solo las pruebas que necesite
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //                "~/Scripts/modernizr-*"));
            bundles.Add(new Bundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));


            // Establezca EnableOptimizations en false para la depuración. Para obtener más información,
            // visite http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "html5shiv",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/html5shiv.min.js",
                    DebugPath = "~/Scripts/html5shiv.js",
                });

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "bootstrap-select",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/bootstrap-select.min.js",
                    DebugPath = "~/Scripts/bootstrap-select.js",
                });

          ScriptManager.ScriptResourceMapping.AddDefinition(
                "moment-with-locales",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/moment-with-locales.min.js",
                    DebugPath = "~/Scripts/moment-with-locales.js",
                });

          ScriptManager.ScriptResourceMapping.AddDefinition(
                "bootstrap-datetimepicker",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/bootstrap-datetimepicker.min.js",
                    DebugPath = "~/Scripts/bootstrap-datetimepicker.js",
                });

          ScriptManager.ScriptResourceMapping.AddDefinition(
              "custom-functions",
              new ScriptResourceDefinition
              {
                  Path = "~/Scripts/custom-functions.js",
                  DebugPath = "~/Scripts/custom-functions.js",
              });

          ScriptManager.ScriptResourceMapping.AddDefinition(
              "block_click",
              new ScriptResourceDefinition
              {
                  Path = "~/Scripts/block_click.js",
                  DebugPath = "~/Scripts/block_click.js",
              });


        }
    }
}