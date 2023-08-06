<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div id="tabs">
	<ul>
		<li><a href="<%= ViewData["rootUri"] %>Sale/Sale" id="sale">销售开单</a></li>
		<li><a href="<%= ViewData["rootUri"] %>Sale/Back" id="back">销售退货</a></li>
		<li><a href="<%= ViewData["rootUri"] %>Sale/Salehistory" id="salehistory">销售查询</a></li>
	</ul>
	<div id="tabs-1">
		
	</div>
	<div id="tabs-2">
		
	</div>
	<div id="tabs-3">
		
	</div>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.tabs.js"></script>
	<script type="text/javascript">

	    $(function () {
	        $("#tabs").tabs({
	            ajaxOptions: {
	                error: function (xhr, status, index, anchor) {
	                    $(anchor.hash).html("对不起，内部错误");
	                }
	            }
	        });
	    });

	    jQuery(document).ready(function () {
	        var tabnum = "<% = ViewData["tabnum"]%>";
	        if (tabnum == "1") {
	            $("#Sale").click();
	        } else if(tabnum == "2") {
	            $("#back").click();
	        }
	        $("#menu_tb").removeClass("active");
	        $("#sale_data").addClass("active");
	    });

    </script>
</asp:Content>