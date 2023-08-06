<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-1">
         <button style="display: none;" type="button" id="detail_dlg" data-target="#modal_financedetail" data-toggle="modal"></button>
	    <table class="table_edit" cellspacing=0 cellpadding=0 style="width:100%;">
			<tr>

                <td style="text-align:right">开始日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                    <input type=text name="start_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" id="start_date" readonly/>
			    </td>
                <td style="text-align:right">结束日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                    <input type=text name="end_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" id="end_date"  readonly/>
			    </td>
			    <td style="text-align:right">
                    <b id="error_msg1" style="display:none">日期过错！</b>
                    <button type="submit" class="btn_action" onclick="printFiance();" style="margin-right:0px;">打印</button>

                    <button type="submit" class="btn_action" onclick="searchFinance();" style="margin-right:10px;">查询</button>

			    </td>
			</tr>
		</table>
		<table class="table table-striped  table-advance table-bordered table-hover" border=0 id="tbl_finance">
            <thead>
			<tr>
		    	<th style="width:130px;text-align:center">日期</th>
			    <th style="text-align:center">销售收入</th>
                <th style="text-align:center">其他收入</th>
    			<th style="text-align:center">销售成本</th>
                <th style="text-align:center">其他费用</th>
                <th style="text-align:center">抹零金额</th>
                <th style="text-align:center">利润</th>
                <th style="text-align:center">明细</th>
	    		
		    </tr>
            </thead>

		</table>
		<table class="table_bottom">
			<tr>
                <td style="width:70%;text-align:right">
                    总利润:&nbsp;&nbsp;&nbsp;
                    </td>
		    	<td>
                    <input type="text" id="total_profit" name="Porfit" disabled class='form-control'/>
		    	</td>
			</tr>
		</table>
	
        <div id="modal_financedetail" class="modal fade" tabindex="-1" data-width="800px">
            <form action="#" id="submit_form_detail" class="form-horizontal form-validate">
                <div class="modal-dialog" style="width: 800px">
                    <div class="modal-content">
                        <div class="modal-head">
                             <h4>财务信息名细表</h4>                           
                        </div>
                        <div>
                            <table class="table_edit" cellspacing=0 cellpadding=0 >    
                                    <td style="text-align:right">日期:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input type=text name="date" class="date_input form-control" style="float:left;margin-right: 5px;"  id="detail_date" readonly/>
			                        </td>  
                            </table>
                        </div>
                      
                        <div class="modal-body">
                            <table class="table table-striped  table-advance table-bordered table-hover" border=0 id="tbl_financedetail">
                                <thead>
			                    <tr>
		    	                    <th style="text-align:center">编号</th>
			                        <th style="text-align:center">说明</th>
                                    <th style="text-align:center">金额</th>
    			                    <th style="text-align:center">操作员</th>
                                    <th style="text-align:center">备注</th>                                  
	    		
		                        </tr>
                                </thead>

		                    </table>
                        </div>
                        <div class="modal-footer">
                            <button class="btn_rect" onclick ="printFinanceDetail()">打印</button>
                            <button type="button" data-dismiss="modal" id="cancelDlg" class="btn_rect">关闭</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
	</div>

<div id="print_page_financedetail" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>财务信息名细表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">日期:<span id="print_financedetail_date"></span></span>
    <span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
                <th style="text-align:center">编号</th>
                <th style="text-align:center">说明</th>
                <th style="text-align:center">金额</th>
                <th style="text-align:center">操作员</th>
                <th style="text-align:center">备注</th>  
			</tr>
		</thead>
		<tbody id="print_financedetail_body" style="background: white; border: 0px;">			
		</tbody>
	</table>    
</div>

<div id="print_finance_overview" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>财务报表</b></font></p><br/>
    <span style="float: left;margin-left: 15px;"> 统计日期:
        <span id="print_table_finance_startdate"></span>&nbsp至
        <span id="print_table_finance_enddate"></span>    
    </span>
    <br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
		    	<th style="width:130px;text-align:center">日期</th>
			    <th style="text-align:center">销售收入</th>
                <th style="text-align:center">其他收入</th>
    			<th style="text-align:center">销售成本</th>
                <th style="text-align:center">其他费用</th>
                <th style="text-align:center">抹零金额</th>
                <th style="text-align:center">利润</th>
			</tr>
		</thead>
		<tbody id="print_finance_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
</div>

    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>

    <script type="text/javascript">
        var oTable, oDetailTable;

        var handleDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            oTable = $('#tbl_finance').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveFinance",

                "aoColumns": [
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
                        aTargets: [7],    // Column number which needs to be modified
                        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            var rhtml = "<button class='btn_rect' onclick=\"detailView_Dlg('" + o.aData[7] + "');\" style=\"margin-right:0px;\">点击查看</button>";
                            //alert(o.aData[6]);
                            //var rhtml = "<img src=\"../../Content/image/main_data.png\" onclick=\"Modify_SupplyDlg(" + o.aData[6] + ")\" style=\"width:20px; height:20px;\" />";
                            return rhtml;
                        },
                        sClass: 'tableCell'    // Optional - class to be applied to this table cell
                    }
                ],
                
            });

            oDetailTable = $('#tbl_financedetail').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveFinanceDetail",

                "aoColumns": [
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

        function detailView_Dlg(cur_date) {
            var value = cur_date;           
            $("#detail_dlg").click();
            $("#detail_date").val(value);

            refreshTicketSaleTable(value);
        }

        function refreshFinanceTable() {
            oSettings = oTable.fnSettings();
            oTable.fnDraw();
            calTotalProfitOnly();
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

        function refreshTicketSaleTable(detail_date) {
            oSettings = oDetailTable.fnSettings();
            oSettings.sAjaxSource = rootUri + "Money/RetrieveFinanceDetail" + "?detaildate=" + detail_date;

            oDetailTable.dataTable().fnDraw();
        }

        function searchFinance() {
            var f_start_date = $("#start_date").val();
            var f_end_date = $("#end_date").val();
            if (f_start_date > f_end_date)  {
                //$("#error_msg1").show();
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
                $("#error_msg1").hide();
                $.ajax({
                    type: "GET",
                    url: "<%= ViewData["rootUri"] %>Money/filterFinance",
                    dataType: 'json',
                    data: {
                        start_date: f_start_date,
                        end_date: f_end_date
                    },
                    success: function (data) {
                        refreshFinanceTable();
                    }
                });
            }
        }
        function getTable_Total() {
            $.ajax({
                type: "GET",
                url: "<%= ViewData["rootUri"] %>Money/financeTotal",
                dataType: 'json',
                data: {
                },
                success: function (data) {
                    handleDataTable();
                    $("#total_profit").val(data+"元");
                }
            });
        }
        function calTotalProfitOnly() {
            $.ajax({
                type: "GET",
                url: "<%= ViewData["rootUri"] %>Money/financeTotal",
                dataType: 'json',
                data: {
                },
                success: function (data) {
                    $("#total_profit").val(data + "元");
                }
            });
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
            
            getTable_Total();            
	    });
    </script>
    <script type="text/javascript">
        function printFinanceDetail() {
            if ($("#tbl_financedetail tbody").find("tr") == null) {
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

            $("#print_financedetail_date").html($("#detail_date").val());
            $("#print_financedetail_body").html($("#tbl_financedetail tbody").html());

            pageprint3();
        }
        function beforePrint3() {
        }
        function afterPrint3() {
            $("#header").css("display", "block");
            $("#tabs-1").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_financedetail").css("display", "none");
            $("#modal_financedetail").css("display", "none");
        }
        function pageprint3() {
            window.onbeforeprint = beforePrint3;
            window.onafterprint = afterPrint3;
            $("#header").css("display", "none");
            $("#tabs-1").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#modal_financedetail").css("display", "none");
            $("#print_page_financedetail").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-1").css("display", "block");
            $(".ui-tabs-nav").css("display", "block"); 
            $("#modal_financedetail").css("display", "block");
            $("#print_page_financedetail").css("display", "none");
        }

</script>

<script type="text/javascript">

    function printFiance() {
        var html = "";
        var print_body = $("#tbl_finance tbody").find("tr");
        for (i = 0; i <= print_body.length; i++) {
            var tdlist = print_body.eq(i).find("td");
            if (tdlist.length != 1) {
                for (i = 0; i <= tdlist.length - 2; i++) {
                    html += tdlist.eq(i).prop('outerHTML');
                }
            }
            else {
                alert("没有数据");
                return;
            }
        }
        $("#print_finance_body").html(html);
        $("#print_table_finance_startdate").html($("#start_date").val());
        $("#print_table_finance_enddate").html($("#end_date").val());
        pageprintFinance();
    }

    function beforePrintFinance() {
    }
    function afterPrintFinance() {
        $("#header").css("display", "block");
        $("#tabs-1").css("display", "block");
        $(".ui-tabs-nav").css("display", "block");
        $("#print_finance_overview").css("display", "none");
    }
    function pageprintFinance() {
        window.onbeforeprint = beforePrintFinance;
        window.onafterprint = afterPrintFinance;
        $("#header").css("display", "none");
        $("#tabs-1").css("display", "none");
        $(".ui-tabs-nav").css("display", "none");
        $("#print_finance_overview").css("display", "block");
        window.print();
        $("#header").css("display", "block");
        $("#tabs-1").css("display", "block");
        $(".ui-tabs-nav").css("display", "block");
        $("#print_finance_overview").css("display", "none");
    }

     </script>
</body>
</html>
