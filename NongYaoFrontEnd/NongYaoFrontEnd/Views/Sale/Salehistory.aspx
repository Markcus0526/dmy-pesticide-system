<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-3">
	    <table class="table_edit table_edit_small smallfont" cellspacing=0 cellpadding=0 style="width:100%;">
			<tr>
                <td style="text-align:right">销售类型:&nbsp;&nbsp;&nbsp;
                </td>
			    <td valign=middle>
                    <select id="history_type" class="sale-shopper input form-control">							
					    <option value="4">全部</option>						
					    <option value="2">销售开单</option>						
					    <option value="3">退货</option>
					</select>
			    </td>
                <td style="text-align:right">开始日期:&nbsp;&nbsp;&nbsp;
                </td>
			    <td valign=middle>
                    <input id="start_date" type=text class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" readonly/>
			    </td>
                <td style="text-align:right">结束日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                    <input id="end_date" type=text class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" readonly/>
			    </td>
			</tr>
            <tr>
                <td style="text-align:right">销售单号\客户名称:&nbsp;&nbsp;&nbsp;
                </td>
			    <td valign=middle colspan="2">
                    <input id="history_search" type=text class="form-control" />
			    </td>
			    <td style="text-align:right">
                    <button type="button" class="btn_action" onclick="searchSalehistory();" style="margin-right:0px;">查询</button>
			    </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
		</table>
		<table class="table table-striped  table-advance table-bordered table-hover smallfont" border=0 id="tbldata">
            <thead>
			<tr>
		    	<th style="text-align:center">销售类型</th>
			    <th style="text-align:center">单号</th>
    			<th style="text-align:center">开单日期</th>
	    		<th style="text-align:center">客户名称</th>
	    		<th style="text-align:center">联系方式</th>
	    		<th style="text-align:center">应收金额</th>
	    		<th style="text-align:center">实收金额</th>
	    		<th style="text-align:center">业务员</th>
	    		<th style="text-align:center">操作</th>
		    </tr>
            </thead>
			<tbody>
            </tbody>
		</table>

        <div style="height:40px;">
		<button type="button" class="btn_actionex" onclick="printSalehistory()" style="float:right;">打印</button>
            </div>
        <div style="width:100%;border:solid;border-width:1px;border-color:#808080"></div>

        <table class="table_edit table_edit_small smallfont" cellspacing=0 cellpadding=0 style="width:100%;">
			<tr>
                <td style="text-align:right">单号详细信息:&nbsp;&nbsp;&nbsp;
                </td>
			    <td valign=middle>
                    <input type=text class="form-control" id="history_ticketnum" readonly />
			    </td>
			    <td valign=middle>
                    <input type=text class="form-control" id="history_customer" readonly />
			    </td>
                <td style="width:40%;"></td></tr>
		</table>

		<table class="table table-striped  table-advance table-bordered table-hover smallfont" border=0 id="tblticketinfo">
            <thead>
			<tr>
		    	<th style="text-align:center">货品编号</th>
			    <th style="text-align:center">仓库</th>
    			<th style="text-align:center">农药名称</th>
	    		<th style="text-align:center">农药登记证号</th>
	    		<th style="text-align:center">产地</th>
	    		<th style="text-align:center">产品批号</th>
	    		<th style="text-align:center">产品规格</th>
	    		<th style="text-align:center">单价</th>
	    		<th style="text-align:center">数量</th>
	    		<th style="text-align:center">总价</th>
		    </tr>
            </thead>
			<tbody>
            </tbody>			
		</table>
        <div style="height:40px;">
		<button type="button" class="btn_actionex" onclick="printTicketinfo()" style="float:right;">打印</button>
            </div>
	</div>

<div id="print_page_salehistory" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>销售统计表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">查询日期:<span id="history_date" class="print_history_date"></span></span>
    <span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
				<th style="text-align:center">销售类型</th>
				<th style="text-align:center">单号</th>
				<th style="text-align:center">开单日期</th>
				<th style="text-align:center">客户名称</th>
				<th style="text-align:center">联系方式</th>
				<th style="text-align:center">应收金额(元)</th>
				<th style="text-align:center">实收金额(元)</th>
				<th style="text-align:center">业务员</th>
			</tr>
		</thead>
		<tbody id="print_salehistory_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
</div>

<div id="print_page_ticketinfo" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>销售详细表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">查询日期:<span id="Span1" class="print_history_date"></span></span><span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
				<th style="text-align:center">货品编号</th>
				<th style="text-align:center">仓库</th>
				<th style="text-align:center">农药名称</th>
				<th style="text-align:center">农药登记证号</th>
				<th style="text-align:center">产地</th>
				<th style="text-align:center">产品批号</th>
				<th style="text-align:center">产品规格</th>
				<th style="text-align:center">单价(元)</th>
				<th style="text-align:center">数量</th>
				<th style="text-align:center">总价(元)</th>
			</tr>
		</thead>
		<tbody id="print_ticketinfo_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
    <span style="float: left; margin-left: 15px;">单号:<span id="print_ticketnum"></span></span>&nbsp;<span>客户名称:<span id="print_customer"></span></span><br/>
</div>

    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>

    <script type="text/javascript">
        
        var oTable, oTicketInfoTable;
        var handleDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            oTable = $('#tbldata').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Sale/RetrieveSalehistoryList",

                "aoColumns": [
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
                "iDisplayLength": 5,
                "bFilter": false,
                "bLengthChange": false,
                "sPaginationType": "bootstrap",
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
                "aoColumnDefs": [
                    {
                        aTargets: [8],    // Column number which needs to be modified
                        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            var rst = "<button class='btn_rect' onclick='refreshTicketSaleTable(" + o.aData[8] + ",\"" + o.aData[1] + "\",\"" + o.aData[3] + "\")'>" +
                                "点击查看</button>";

                            return rst;
                        },
                        sClass: 'center'
                    }
                ]                
            });

            oTicketInfoTable = $('#tblticketinfo').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Sale/RetrieveTicketSaleList",

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
                  { "bSortable": false }
                ],
                "aLengthMenu": [
                    [1, 10, 20, 50, -1],
                    [1, 10, 20, 50, "All"] // change per page values here
                ],
                // set the initial value
                "iDisplayLength": 5,
                "bFilter": false,
                "bLengthChange": false,
                "sPaginationType": "bootstrap",
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
                "aoColumnDefs": [
                ],

            });
        }

        function refreshTable() {
            var type = $("#history_type").val();
            var start_date = $("#start_date").val();
            var end_date = $("#end_date").val();
            var search = $("#history_search").val();

            oSettings = oTable.fnSettings();
            oSettings.sAjaxSource = rootUri + "Sale/RetrieveSalehistoryList" + "?type=" + type + "&startdate=" + start_date + "&enddate=" + end_date + "&search=" + search;

            oTable.dataTable().fnDraw();

            refreshTicketSaleTable(0, "", "");

            //oSettings = oTable.fnSettings();
            //oTable.fnDraw();

            //Retrieve the new data with $.getJSON. You could use it ajax too
            /*$.getJSON(oSettings.sAjaxSource, null, function (json) {
                //oTable.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    oTable.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                oTable.fnDraw();
            });*/
        }

        function refreshTicketSaleTable(ticket_id, ticketnum, customer) {
            $("#history_ticketnum").val("" + ticketnum);
            $("#history_customer").val("" + customer);

            oSettings = oTicketInfoTable.fnSettings();
            oSettings.sAjaxSource = rootUri + "Sale/RetrieveTicketSaleList" + "?ticketid=" + ticket_id;

            oTicketInfoTable.dataTable().fnDraw();

            //oSettings = oTable.fnSettings();
            //oTable.fnDraw();

            //Retrieve the new data with $.getJSON. You could use it ajax too
            /*$.getJSON(oSettings.sAjaxSource, null, function (json) {
                //oTable.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    oTable.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                oTable.fnDraw();
            });*/
        }

        function searchSalehistory() {
            var start_date = $("#start_date").val();
            var end_date = $("#end_date").val();
            if (start_date > end_date)  {
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
                toastr["error"]("", "日期过错！");
            }
            else {
                refreshTable();
            }
        }

        $(function () {
            $("#start_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            $("#end_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });

            handleDataTable();
        });

        function printSalehistory() {
            $(".print_history_date").html($("#start_date").val() + "至" + $("#end_date").val());

            var tableRows = $("#tbldata tbody tr");
            var htmlStr = "";
            for (var i = 0; i < tableRows.length; i++) {
                var row = $(tableRows[i]);
                htmlStr += "<tr>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(0).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(1).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(2).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(3).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(4).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(5).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(6).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(7).html() + "</td>" +
                    "</tr>";
            }

            $("#print_salehistory_body").html(htmlStr);

            pageprint_salehistory();
        }

        function printTicketinfo() {
            if ($("#history_ticketnum").val() == null || $("#history_ticketnum").val().length == 0) {
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

            $(".print_history_date").html($("#start_date").val() + "至" + $("#end_date").val());
            $("#print_ticketnum").html($("#history_ticketnum").val());
            $("#print_customer").html($("#history_customer").val());

            $("#print_ticketinfo_body").html($("#tblticketinfo tbody").html());

            pageprint_ticketinfo();
        }

    </script>
    <script type="text/javascript">
        var initBody;
        function beforePrint() {
        }
        function afterPrint_salehistory() {
            $("#header").css("display", "block");
            $("#tabs-3").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_salehistory").css("display", "none");
        }
        function pageprint_salehistory() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint_salehistory;
            $("#header").css("display", "none");
            $("#tabs-3").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_salehistory").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-3").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_salehistory").css("display", "none");
        }

        function afterPrint_ticketinfo() {
            $("#header").css("display", "block");
            $("#tabs-3").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_ticketinfo").css("display", "none");
        }
        function pageprint_ticketinfo() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint_ticketinfo;
            $("#header").css("display", "none");
            $("#tabs-3").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_ticketinfo").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-3").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_ticketinfo").css("display", "none");
        }

</script>

</body>
</html>
