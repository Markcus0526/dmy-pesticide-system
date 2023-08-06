<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div id="tabs">
    <ul>
		<li><a href="<%= ViewData["rootUri"] %>Money/Finance">财务报表</a></li>
		<li><a href="<%= ViewData["rootUri"] %>Money/Credit">应收应付</a></li>
		<li><a href="<%= ViewData["rootUri"] %>Money/Statistics">收支统计</a></li>
		<li><a href="<%= ViewData["rootUri"] %>Money/Bank">货币资金</a></li>
		<li><a href="<%= ViewData["rootUri"] %>Money/BossReport">老板一张表</a></li>
	</ul>
    <div id="tabs-1">
	    
	</div>
	<div id="tabs-2">
	    
	</div>
	<div id="tabs-3">
		
	</div>
	<div id="tabs-4">
		
	</div>
	<div id="tabs-5">
		
	</div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/select2/select2.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootbox/bootbox.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.tabs.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>

    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />


    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />
    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js"></script>
<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        
	        $("#tabs").tabs({
	            ajaxOptions: {
	                error: function (xhr, status, index, anchor) {
	                    $(anchor.hash).html("对不起，内部错误.");
	                }
	            }
	        });
	        
	    });
	    jQuery(document).ready(function () {
	      
             $("#menu_tb").removeClass("active");
             $("#money_data").addClass("active");
         });
    </script>
</asp:Content>
