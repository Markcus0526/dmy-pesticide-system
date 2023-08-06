<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        
            <div id="set_dialog" class="modal fade" tabindex="-1" data-width="400">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <h4>请用户注意将清空所有信息， 相当于软件初始化功能</h4>
                        </div>
                        <div class="modal-footer">
                            <button type="button" data-dismiss="modal" class="btn_rect" onclick="window.location='<% =ViewData["rootUri"]%>Main/Index?init=1'">通过</button>
                            <button type="button" data-dismiss="modal" class="btn_rect">取消</button>
                        </div>
                    </div>
                </div>
            </div>

    <div id="tabs">
        <ul>
            <li><a href="<%= ViewData["rootUri"] %>Main/ShopInfo">经销商信息</a></li>
            <li><a href="<%= ViewData["rootUri"] %>Main/CustomerInfo">客户资料</a></li>
            <li><a href="<%= ViewData["rootUri"] %>Main/SupplyInfo">供应商资料</a></li>
            <li><a href="<%= ViewData["rootUri"] %>Main/UserInfo">权限配置</a></li>
            <li><a href="<%= ViewData["rootUri"] %>Main/StoreInfo">仓库资料</a></li>
            <button type="submit" class="btn_tabright" data-target="#set_dialog" data-toggle="modal">数据清空</button>
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

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server"> 
    
   

    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/select2/select2.js" type="text/javascript"></script>
    <link href="<%= ViewData["rootUri"] %>Content/plugins/select2/select2_locale_zh-CN.js" rel="stylesheet" type="text/css" />
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
    <link href="<%= ViewData["rootUri"] %>Content/plugins/select2/select2_metro.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/jquery-multi-select/css/multi-select-metro.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />
    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js"></script>

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

        $(document).ready(function () {          
            $("#menu_tb").removeClass("active");
            $("#main_data").addClass("active");

        });

    </script>
</asp:Content>
