<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div id="main_title">


	<h2>首页</h2>
	</div>
	<table class="table_menu">
	    <tr>
             <td class="image">
                 <a href="#" onclick="haveRole(4);">
                     <img src="<%= ViewData["rootUri"]%>Content/image/main.png" width=289 height=107>
                 </a>
             </td>
             
             <td class="image">
                 <a href="#" onclick="haveRole(0);">
                     <img src="<%= ViewData["rootUri"]%>Content/image/supply.png" width=289 height=107>
                 </a>
             </td>
            
	    </tr>
 	    <tr>
             <td class="image">
                 <a href="#" onclick="haveRole(1);">
                     <img src="<%= ViewData["rootUri"]%>Content/image/sale.png" width=289 height=107>
                 </a>
             </td>            
             <td class="image">
                 <a href="#" onclick="haveRole(2);">
                     <img src="<%= ViewData["rootUri"]%>Content/image/store.png" width=289 height=107>
                 </a>
             </td>             
 	    </tr>
	    <tr>
            <td class="image">
                <a href="#" onclick="haveRole(3);">
                    <img src="<%= ViewData["rootUri"]%>Content/image/money.png" width=289 height=107>
                </a>
            </td>          
            <td class="image">
                <a href="<%= ViewData["rootUri"]%>Catalog/Index">
                    <img src="<%= ViewData["rootUri"]%>Content/image/nongyao.png" width=289 height=107>
                </a>
            </td> 
	    </tr>
	</table>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">


    <script type="text/javascript">

        $(document).ready(function () {
            $("#menu_tb").removeClass("active");
            $("#first").addClass("active");

            var passeditems = "<% =ViewData["passedCatalogs"]%>";
            //alert(passeditems);
            if (passeditems.length > 0) {
                toastr.options = {
                    "closeButton": false,
                    "debug": true,
                    "positionClass": "toast-top-center",
                    "onclick": null,
                    "showDuration": "3",
                    "hideDuration": "3",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["success"]("通过审核了", passeditems);
            }

            <%
        List<UnpassedCatalogInfo> unpasseditems = (List<UnpassedCatalogInfo>)ViewData["unpassedCatalogs"];
        for(int i = 0; i < unpasseditems.Count; i ++) {
            UnpassedCatalogInfo item = unpasseditems.ElementAt(i);
        %>

            toastr.options = {
                "closeButton": false,
                "debug": true,
                "positionClass": "toast-top-center",
                "onclick": null,
                "showDuration": "3",
                "hideDuration": "3",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr["error"]("<% =item.reason%>", "<% =item.name%> 未通过了");
            <%
    }
        %>
        });

    </script>
</asp:Content>

