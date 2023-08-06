<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <script type="text/javascript">

        var oTable;
        var handleDataTable = function (store_id) {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            oTable = $('#tbl_manageavail').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Store/RetrieveManageAvail" + "?store=" + store_id,
                "oLanguage": {
                    "sProcessing": "处理中...",
                    "sLengthMenu": "_MENU_ 记录",
                    "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
                    "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
                    "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
                    "sZeroRecords": "没有搜索结果",
                    "oPaginate": {
                        "sFirst": "首页",
                        "sPrevious": "上页",
                        "sNext": "下页",
                        "sLast": "末页"
                    },
                    "sSearch": "搜索:"
                },
                "aoColumns": [
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false }
                  
                ],
                "aLengthMenu": [
                    [1, 10, 20, 50, -1],
                    [1, 10, 20, 50, "All"] // change per page values here
                ],
                // set the initial value
                "iDisplayLength": 10,
                "bFilter": false,
                "bLengthChange": false,
                "sPaginationType": "bootstrap",
                "aoColumnDefs": [
                    //{
                    //    aTargets: [4],    // Column number which needs to be modified
                    //    fnRender: function (o, v) {   // o, v contains the object and value for the column
                    //        var rst = "<button class='btn default' data-target='#moneybank_detail_modal' data-toggle='modal' onclick='showDetailTable(\"" + o.aData[0] + "\",\"" + o.aData[1] + "\",\"" + o.aData[2] + "\",\"" + o.aData[3] + "\")'>" +
                    //            "点击查看</button>";

                    //        return rst;
                    //    },
                    //    sClass: 'center'
                    //}
                ]
            });
        }

        function search_manageavail() {
            if ($("#manageavail_store").val() == null) {
                //$("#error_msg").show();
                toastr.options = {
                    "closeButton": false,
                    "debug": true,
                    "positionClass": "toast-top-center",
                    "onclick": null,
                    "showDuration": "3",
                    "hideDuration": "3",
                    "timeOut": "3500",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["error"]("", "请选择仓库！");
            }
            else {
                var store_id = $("#manageavail_store").val();
                $("#manageavail_storename").val($("#manageavail_store option[value='" + store_id + "']").html());

                refreshManageavailTable();
            }
        }

        function refreshManageavailTable() {
            var store_id = $("#manageavail_store").val();
            var status = $("#manageavail_status").val();
            var remaindays = $("#manageavail_remaindays").val();

            oSettings = oTable.fnSettings();

            oSettings.sAjaxSource = rootUri + "Store/RetrieveManageAvail" + "?store=" + store_id + "&status=" + status + "&remaindays=" + remaindays;
            oTable.fnDraw();
        }             

        $(function () {
            var store_id = $("#manageavail_store").val();
            $("#manageavail_storename").val($("#manageavail_store option[value='" + store_id + "']").html());

            handleDataTable(store_id);
        });
    </script>
</head>

<body>
    <div id="tabs-4">
		<table class="table_edit table_edit_small" cellspacing=0 cellpadding=0 style="width:100%;">
	    	<tr>
		    	<td style="text-align:right">仓库:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                     <select id="manageavail_store" class="form-control">
                        <%
                            List<tbl_store> stores = (List<tbl_store>)ViewData["stores"];
                            for (int i = 0; i < stores.Count(); i++)
                            {
                                tbl_store storeitem = stores.ElementAt(i);
                        %>
                        <option value="<% =storeitem.id %>"><% =storeitem.name %></option>
                        <%
                            }
                        %>
                    </select>
			    </td>
			    <td style="text-align:right">有效期状态:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>                     
                     <select id="manageavail_status" class="form-control">
                        <option value="2" selected>全部</option>
                        <option value="0">已过期</option>
                        <option value="1">未过期</option>
                    </select>
			    </td>
			    <td style="text-align:right">农药有效期剩余天数:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>                  
                     <select id="manageavail_remaindays" class="form-control">
                        <option value="7">7</option>
                        <option value="30">30</option>
                        <option value="60">60</option>
                    </select>
			    </td>
                <td style="text-align:right">
                    <b id="error_msg" style="display:none">日期过错！</b>
                    <button type="submit" class="btn_action" onclick="search_manageavail();" style="margin-right:0px;">搜索</button>
                </td>
			</tr>
		</table>

        <div style="width:100%;border:solid;border-width:1px;border-color:#808080"></div>

        <table class="table_edit" cellspacing=0 cellpadding=0>
	    	<tr>
		    	<td style="text-align:right">仓库名称:&nbsp;&nbsp;&nbsp;
                    </td>			    
			    <td valign=middle>
                     <input id="manageavail_storename" class="form-control" readonly/>
			    </td>
			</tr>
		</table>
		<table class="table table-striped  table-advance table-bordered table-hover tinyfont" id="tbl_manageavail">
            <thead>
			    <tr>
                    <th style="text-align:center">有效期状态</th>
		    	    <th style="text-align:center">农药名称</th>
    			    <th style="text-align:center">农药登记证号</th>
	    		    <th style="text-align:center">生产批号</th>
	    		    <th style="text-align:center">过期日期</th>
                    <th style="text-align:center">库存数量</th>
		    	    <th style="text-align:center">进货单号</th>
    			    <th style="width:100px;text-align:center">农药规格</th>
	    		    <th style="width:100px;text-align:center">生产日期</th>
	    		    <th style="text-align:center">有效期</th>
                    <th style="text-align:center">生产厂家</th>
		    	    <th style="text-align:center">联系方式</th>
			    </tr>
            </thead>
			<tbody>

			</tbody>
		</table>

        <div style="height:40px;">
            <button type="button" class="btn_rect" onclick="printManageavail()" style="float:right;">打印</button>	
        </div>	
	</div>
      

<div id="print_page_manageavail" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>农药效期信息表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">仓库名称:<span id="print_manageavail_store"></span></span>
	<span style="float: left; margin-left: 15px;">有效期状态:<span id="print_manageavail_status"></span></span>
    <span style="margin-right: 15px;">农药有效期剩余天数:<span id="print_manageavail_remaindays"></span>天</span><br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
                    <th style="text-align:center">有效期状态</th>
		    	    <th style="text-align:center">农药名称</th>
    			    <th style="text-align:center">农药登记证号</th>
	    		    <th style="text-align:center">生产批号</th>
	    		    <th style="text-align:center">过期日期</th>
                    <th style="text-align:center">库存数量</th>
		    	    <th style="text-align:center">进货单号</th>
    			    <th style="text-align:center">农药规格</th>
	    		    <th style="text-align:center">生产日期</th>
	    		    <th style="text-align:center">有效期</th>
                    <th style="text-align:center">生产厂家</th>
		    	    <th style="text-align:center">联系方式</th>
			</tr>
		</thead>
		<tbody id="print_manageavail_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
    <span style="float:left;margin-left:15px;">打印日期:<% =ViewData["curdate"] %></span><br/>
</div>

    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>

    <script type="text/javascript">

        function printManageavail() {
            if ($("#tbl_manageavail tbody").find("tr") == null) {
                toastr.options = {
                    "closeButton": false,
                    "debug": true,
                    "positionClass": "toast-top-center",
                    "onclick": null,
                    "showDuration": "3",
                    "hideDuration": "3",
                    "timeOut": "1500",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["error"]("", "没有资料！");

                return;
            }
            
            $("#print_manageavail_store").html($("#manageavail_storename").val());
            var status = $("#manageavail_status").val();
            $("#print_manageavail_status").html($("#manageavail_status option[value='" + status + "']").html());
            $("#print_manageavail_remaindays").html($("#manageavail_remaindays").val());

            $("#print_manageavail_body").html($("#tbl_manageavail tbody").html());

            pageprint();
        }

        function beforePrint() {
        }
        function afterPrint() {
            $("#header").css("display", "block");
            $("#tabs-4").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_manageavail").css("display", "none");
        }
        function pageprint() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint;
            $("#header").css("display", "none");
            $("#tabs-4").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_manageavail").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-4").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_manageavail").css("display", "none");
        }

</script>

</body>
</html>
