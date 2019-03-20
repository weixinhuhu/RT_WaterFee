using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace WHC.MVCWebMis
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //为了减少太多的Bundles命名，定义的CSS的Bundle为："~/Content/css"、"~/Content/jquerytools"
            //定义的Script的Bundles为："~/bundles/jquery"、"~/bundles/jquerytools"

            //Jquery必备的StyleBundle和ScriptBundle
            StyleBundle css = new StyleBundle("~/Content/css");
            ScriptBundle jquery = new ScriptBundle("~/bundles/jquery");
            jquery.Orderer = new AsIsBundleOrderer();

            #region EasyUI样式和JS文件引用
            bool useNewVersion = true;

            if (useNewVersion)
            {
                //添加Jquery EasyUI的样式
                css.Include("~/Content/JqueryEasyUI-New/themes/default/easyui.css",
                            "~/Content/JqueryEasyUI-New/themes/icon.css");

                //添加Jquery，EasyUI和easyUI的语言包的JS文件，
                jquery.Include("~/Content/JqueryEasyUI-New/jquery.min.js",
                            "~/Content/jquery.serializejson.min.js",
                            "~/Content/JqueryEasyUI-New/jquery.easyui.min.js",
                            "~/Content/JqueryEasyUI-New/locale/easyui-lang-zh_CN.js");
            }
            else
            {
                //添加Jquery EasyUI的样式
                css.Include("~/Content/JqueryEasyUI/themes/default/easyui.css",
                            "~/Content/JqueryEasyUI/themes/icon.css");

                //添加Jquery，EasyUI和easyUI的语言包的JS文件，
                jquery.Include("~/Content/JqueryEasyUI/jquery.min.js",
                            "~/Content/jquery.serializejson.min.js",
                            "~/Content/JqueryEasyUI/jquery.easyui.min.js",
                            "~/Content/JqueryEasyUI/locale/easyui-lang-zh_CN.js");
            }
            #endregion
            //执行增加的样式
            css.Include("~/Content/icons-customed/16/icon.css",
                "~/Content/icons-customed/24/icon.css",
                "~/Content/icons-customed/32/icon.css",
                "~/Content/themes/Default/style.css",
                "~/Content/themes/Default/default.css");
            //日期格式的引用
            jquery.Include("~/Content/datapattern.js");

            //扩展的StyleBundle和ScriptBundle
            StyleBundle cssExtend = new StyleBundle("~/Content/jquerytools");
            ScriptBundle jqueryExtend = new ScriptBundle("~/bundles/jquerytools");
            jqueryExtend.Orderer = new AsIsBundleOrderer();

            //引用EasyUI扩展
            cssExtend.Include("~/Content/JQueryTools/jQuery.easyui-extend/extend/themes/easyui.extend.css",
                "~/Content/JQueryTools/jQuery.easyui-extend/extend/themes/icon.css");
            jqueryExtend.Include("~/Content/JQueryTools/jQuery.easyui-extend/jquery.easyui.extend.min.js");

            //引用消息提示控件jNotify
            cssExtend.Include("~/Content/JQueryTools/jNotify/jquery/jNotify.jquery.css");
            jqueryExtend.Include("~/Content/JQueryTools/jNotify/jquery/jNotify.jquery.js");

            //Tag标签的控件应用
            cssExtend.Include("~/Content/JQueryTools/Tags-Input/jquery.tagsinput.css");
            jqueryExtend.Include("~/Content/JQueryTools/Tags-Input/jquery.tagsinput.js");

            //添加对uploadify控件的支持
            cssExtend.Include("~/Content/JQueryTools/uploadify/uploadify.css");
            jqueryExtend.Include("~/Content/JQueryTools/uploadify/jquery.uploadify.js");

            //添加LODOP控件支持
            jqueryExtend.Include("~/Content/JQueryTools/LODOP/CheckActivX.js");

            //添加对ckeditor的支持
            //jqueryExtend.Include("~/Content/JQueryTools/ckeditor/ckeditor.js", 
            //    "~/Content/JQueryTools/ckeditor/adapters/jquery.js");
                            

            //其他一些辅助脚本和样式
            //1、图表JS文件应用
            bundles.Add(new ScriptBundle("~/bundles/jquerytools/highcharts").Include(
                "~/Content/JQueryTools/Highcharts/js/highcharts.js",
                "~/Content/JQueryTools/Highcharts/js/modules/exporting.js",
                "~/Content/JQueryTools/Highcharts/js/themes/grid.js",
                "~/Content/JQueryTools/Highcharts/js/highcharts-more.js",
                "~/Content/JQueryTools/Highcharts/js/highcharts-3d.js"));



            //全部增加到集合里面去
            bundles.Add(css);
            bundles.Add(jquery);
            bundles.Add(cssExtend);
            bundles.Add(jqueryExtend);
            //BundleTable.EnableOptimizations = true;
        }
    }

    /// <summary>
    /// 自定义Bundles排序
    /// </summary>
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
    internal static class BundleExtensions
    {
        public static Bundle ForceOrdered(this Bundle sb)
        {
            sb.Orderer = new AsIsBundleOrderer();
            return sb;
        }
    }
}