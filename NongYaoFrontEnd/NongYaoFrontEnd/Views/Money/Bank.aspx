<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <script type="text/javascript">

        var oTable, oDetailTable;
        var handleDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            oTable = $('#tbl_moneybank').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveMoneyBank",
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
                    {
                        aTargets: [4],    // Column number which needs to be modified
                        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            var rst = "<button class='btn_rect' data-target='#moneybank_detail_modal' data-toggle='modal' onclick='showDetailTable(\"" + o.aData[0] + "\",\"" + o.aData[1] + "\",\"" + o.aData[2] + "\",\"" + o.aData[3] + "\",\"" + o.aData[5] + "\",\"" + o.aData[6] + "\")'>" +
                                "点击查看</button>";

                            return rst;
                        },
                        sClass: 'center'
                    }
                ]
            });

            oDetailTable = $('#tbl_moneybank_detail').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveMoneyBankDetail",
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
                ]
            });
        }

        function search_Bank() {
            var start_date = $("#moneybank_start_date").val();
            var end_date = $("#moneybank_end_date").val();
            if (start_date > end_date) {
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
                toastr["error"]("", "日期过错！");
            } else if ("" + start_date < "<% =ViewData["startdatelimit"]%>") {
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
                toastr["error"]("开始日期要从当年1月1日", "日期错误！");
            } else if ("" + end_date > "<% =ViewData["enddatelimit"]%>") {
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
                toastr["error"]("结束日期要到今天", "日期错误！");
            } else {
                //$("#error_msg").hide();
                //$.ajax({
                //    url: rootUri + "Money/FilterBank",
                //    data: {
                //        start_date: start_date,
                //        end_date: end_date
                //    },
                //    success: function (data) {
                        refreshBankTable();
                //    }
                //});
            }
        }

        function refreshBankTable() {
            var start_date = $("#moneybank_start_date").val();
            var end_date = $("#moneybank_end_date").val();

            oSettings = oTable.fnSettings();

            oSettings.sAjaxSource = rootUri + "Money/RetrieveMoneyBank" + "?startdate=" + start_date + "&enddate=" + end_date;
            oTable.fnDraw();
        }

        function showDetailTable(detail_date, money, bank, total, inmoney, outmoney) {
            $("#moneybank_detail_type").val(4);
            $("#moneybank_detail_moneytype").val(2);
            $("#moneybank_detail_date").val(detail_date);
            $("#moneybank_detail_money").val(money);
            $("#moneybank_detail_bank").val(bank);
            $("#moneybank_detail_total").val(total);
            $("#moneybank_detail_in").val(inmoney);
            $("#moneybank_detail_out").val(outmoney);

            refreshDetailTable();
        }

        function refreshDetailTable() {
            var detail_date = $("#moneybank_detail_date").val();
            var paytype = $("#moneybank_detail_type").val();
            var moneytype = $("#moneybank_detail_moneytype").val();

            oSettings = oDetailTable.fnSettings();

            oSettings.sAjaxSource = rootUri + "Money/RetrieveMoneyBankDetail" + "?detaildate=" + detail_date + "&paytype=" + paytype + "&moneytype=" + moneytype;
            oDetailTable.fnDraw();
        }        

        $(function () {
            $("#moneybank_start_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
	            buttonImageOnly: true,
	            dateFormat: 'yy-mm-dd',
	            option: $.datepicker.regional['zh-CN'],
                
            });
            $("#moneybank_end_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
	            buttonImageOnly: true,
	            dateFormat: 'yy-mm-dd',
	            option: $.datepicker.regional['zh-CN']
            });
            
            handleDataTable();
        });
    </script>
</head>

<body>
    <div id="tabs-4">
		<table class="table_edit" cellspacing=0 cellpadding=0 style="width:100%;">
	    	<tr>
		    	<td style="text-align:right">开始日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                     <input type=text id="moneybank_start_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" +String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" readonly/>
			    </td>
			    <td style="text-align:right">结束日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                     <input type=text id="moneybank_end_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" readonly/>
			    </td>
                <td style="text-align:right">
                    <b id="error_msg" style="display:none">日期过错！</b>
                    <button class="btn_action" onclick="printTableoverview()" style="margin-right:0px;">打印</button>
                    <button type="submit" class="btn_action" onclick="search_Bank();" style="margin-right:10px;">搜索</button>
                </td>
			</tr>
		</table>
		<table class="table table-striped  table-advance table-bordered table-hover" id="tbl_moneybank">
            <thead>
			    <tr>
                    <th style="width:130px;text-align:center">日期</th>
		    	    <th style="text-align:center">现金</th>
    			    <th style="text-align:center">银行存款</th>
	    		    <th style="text-align:center">合计</th>
	    		    <th style="width:100px;text-align:center">明细</th>
			    </tr>
            </thead>
			<tbody>

			</tbody>
		</table>
		
	</div>

    <div id="moneybank_detail_modal" class="modal fade" tabindex="-1">
            <form action="#" id="submit_form_detail" class="form-horizontal form-validate">
                <div class="modal-dialog" style="width: 1024px">
                    <div class="modal-content">
                        <div class="modal-head">
                             <h4>支出\收入明细表</h4>                           
                        </div>
                        <div>
                            <table class="table_edit table_edit_small" cellspacing=0 cellpadding=0 > 
                                <tr>   
                                    <td style="text-align:right" colspan="1">明细类型:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle colspan="2">
                                          <select id="moneybank_detail_type" class="form-control" onchange="refreshDetailTable()">
                                              <option value="4">全部</option>
                                              <option value="0">现金</option>
                                              <option value="2">银行存款</option>
                                            </select>
			                        </td>  
                                    <td style="text-align:right" colspan="1">收支类型:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle colspan="2">
                                          <select id="moneybank_detail_moneytype" class="form-control" onchange="refreshDetailTable()">
                                              <option value="2">全部</option>
                                              <option value="1">收入</option>
                                              <option value="0">支出</option>
                                            </select>
			                        </td> 
                                </tr>
                                <tr>
                                    <td style="text-align:right">日期:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input id="moneybank_detail_date" class="form-control" style="width:120px;" readonly/>
			                        </td>  
                                    <td style="text-align:right">现金收入支出合计:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input id="moneybank_detail_money" class="form-control" style="width:120px;" readonly/>
			                        </td>  
                                    <td style="text-align:right">银行转账合计:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input id="moneybank_detail_bank" class="form-control" style="width:120px;" readonly/>
			                        </td>  
                                    <td style="text-align:right">资金总计:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input id="moneybank_detail_total" class="form-control" style="width:120px;" readonly/>
			                        </td>  
                                 </tr>
                                <tr>
                                    <td></td>
			                        <td></td>  
                                    <td></td>
			                        <td></td>  
                                    <td style="text-align:right">收入合计:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input id="moneybank_detail_in" class="form-control" style="width:120px;" readonly/>
			                        </td>  
                                    <td style="text-align:right">支出总计:&nbsp;&nbsp;&nbsp;</td>
			                        <td valign=middle>
                                          <input id="moneybank_detail_out" class="form-control" style="width:120px;" readonly/>
			                        </td>  
                                 </tr>
                            </table>
                        </div>
                      
                        <div class="modal-body">
                            <table class="table table-striped  table-advance table-bordered table-hover" border=0 id="tbl_moneybank_detail">
                                <thead>
			                    <tr>
		    	                    <th style="text-align:center; width:150px;">单号</th>
			                        <th style="text-align:center; width:100px;">明细类型</th>
                                    <th style="text-align:center; width:100px;">收入\支出</th>
    			                    <th style="text-align:center; width:100px;">金额</th>
                                    <th style="text-align:center; width:200px;">摘要</th>
		                        </tr>
                                </thead>

		                    </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn_rect" onclick="printMoneybankDetail()">打印</button>
                            <button type="button" data-dismiss="modal" class="btn_rect">关闭</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>

<div id="print_page_moneybank" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>支出\收入名细表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">日期:<span id="print_moneybank_date"></span></span><span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
				<th style="text-align:center">单号</th>
				<th style="text-align:center">明细类型</th>
				<th style="text-align:center">收入\支出</th>
				<th style="text-align:center">金额(元)</th>
				<th style="text-align:center">理由</th>
			</tr>
		</thead>
		<tbody id="print_moneybank_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
    <span style="float: left; margin-left: 15px;">现金收入支出合计:<span id="print_moneybank_money"></span></span>&nbsp;
    <span>银行转账合计:<span id="print_moneybank_bank"></span></span>&nbsp;
    <span>资金合计:<span id="print_moneybank_total"></span></span><br/>
</div>

<div id="print_overview" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>货币资金总览</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">打印日期:<% =ViewData["curdate"] %></span>
    <span style="float: right;margin-right: 15px;"> 统计日期:
        <span id="print_table_startdate"></span>&nbsp至
        <span id="print_table_enddate"></span>    
    </span><br/>
    <br/>
	<table class="table_view" border="1">
		<thead>
			<tr>
                <th style="width:130px;text-align:center">日期</th>
		    	<th style="text-align:center">现金</th>
    			<th style="text-align:center">银行存款</th>
	    		<th style="text-align:center">合计</th>
			</tr>
		</thead>
		<tbody id="print_overview_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
</div>

    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>

    <script type="text/javascript">

        function printMoneybankDetail() {
            if ($("#tbl_moneybank_detail tbody").find("tr") == null) {
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
            
            $("#print_moneybank_date").html($("#moneybank_detail_date").val());
            $("#print_moneybank_money").html($("#moneybank_detail_money").val());
            $("#print_moneybank_bank").html($("#moneybank_detail_bank").val());
            $("#print_moneybank_total").html($("#moneybank_detail_total").val());

            $("#print_moneybank_body").html($("#tbl_moneybank_detail tbody").html());

            pageprintbank();
        }

        function beforePrintbank() {
        }
        function afterPrintbank() {
            $("#header").css("display", "block");
            $("#tabs-4").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#moneybank_detail_modal").css("display", "block");
            $("#print_page_moneybank").css("display", "none");
        }
        function pageprintbank() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint;
            $("#header").css("display", "none");
            $("#tabs-4").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#moneybank_detail_modal").css("display", "none");
            $("#print_page_moneybank").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-4").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#moneybank_detail_modal").css("display", "block");
            $("#print_page_moneybank").css("display", "none");
        }



</script>

     <script type="text/javascript">

         function printTableoverview() {
             var html = "";
             var print_body = $("#tbl_moneybank tbody").find("tr");
             for (i = 0; i <= print_body.length; i++) {
                 var tdlist = print_body.eq(i).find("td");
                 if (tdlist.length!=1) {
                     for (i = 0; i <= tdlist.length - 2; i++) {
                         html += tdlist.eq(i).prop('outerHTML');
                     }
                 }
                 else 
                 {
                     alert("没有数据");
                     return;
                 }
             }
             $("#print_overview_body").html(html);
             $("#print_table_startdate").html($("#moneybank_start_date").val());
             $("#print_table_enddate").html($("#moneybank_end_date").val());
            pageprint2();
         }

         function beforePrint2() {
         }
         function afterPrint2() {
             $("#header").css("display", "block");
             $("#tabs-4").css("display", "block");
             $(".ui-tabs-nav").css("display", "block");
             $("#print_overview").css("display", "none");
         }
         function pageprint2() {
             window.onbeforeprint = beforePrint2;
             window.onafterprint = afterPrint2;
             $("#header").css("display", "none");
             $("#tabs-4").css("display", "none");
             $(".ui-tabs-nav").css("display", "none");
             $("#print_overview").css("display", "block");
             window.print();
             $("#header").css("display", "block");
             $("#tabs-4").css("display", "block");
             $(".ui-tabs-nav").css("display", "block");
             $("#print_overview").css("display", "none");
         }

     </script>
</body>
</html>
