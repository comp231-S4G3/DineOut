#pragma checksum "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bbad93674b76011533d5a72f57d94456b0626ada"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_QrGenerator), @"mvc.1.0.view", @"/Views/Home/QrGenerator.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/QrGenerator.cshtml", typeof(AspNetCore.Views_Home_QrGenerator))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\_ViewImports.cshtml"
using DineOut;

#line default
#line hidden
#line 2 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\_ViewImports.cshtml"
using DineOut.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bbad93674b76011533d5a72f57d94456b0626ada", @"/Views/Home/QrGenerator.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4c2cebbd3840c3b18d6a0efb5833584bb8a9f925", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_QrGenerator : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml"
  
    ViewBag.Title = "Qr Generator";
    Layout = "_Layout";

#line default
#line hidden
            BeginContext(69, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 7 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml"
 using (Html.BeginForm("QrGenerator", "Home", FormMethod.Post))
{

#line default
#line hidden
            BeginContext(141, 90, true);
            WriteLiteral("    <input type=\"text\" name=\"inputText\" />\r\n    <input type=\"submit\" value=\"Generate\" />\r\n");
            EndContext();
#line 11 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml"

}

#line default
#line hidden
            BeginContext(236, 14, true);
            WriteLiteral("\r\n\r\n<hr />\r\n\r\n");
            EndContext();
#line 17 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml"
 if (ViewBag.QRCode != null)
{

#line default
#line hidden
            BeginContext(283, 8, true);
            WriteLiteral("    <img");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 291, "\"", 312, 1);
#line 19 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml"
WriteAttributeValue("", 297, ViewBag.QRCode, 297, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(313, 38, true);
            WriteLiteral(" style=\"width:200px;height:200px\" />\r\n");
            EndContext();
#line 20 "C:\Users\luizs\Desktop\Project\DineOut\DineOut\Views\Home\QrGenerator.cshtml"

}

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
