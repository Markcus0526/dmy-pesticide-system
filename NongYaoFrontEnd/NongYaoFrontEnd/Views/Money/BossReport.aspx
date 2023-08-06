<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <script type="text/javascript">
        
        function search_bossreport() {
            var start_date = $("#bossreport_start_date").val();
            var end_date = $("#bossreport_end_date").val();
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
            }
            else {
                //$("#error_msg").hide();
                $("#bossreport_table_startdate").html(start_date);
                $("#bossreport_table_enddate").html(end_date);
                $("#print_bossreport_table_startdate").html(start_date);
                $("#print_bossreport_table_enddate").html(end_date);

                $.ajax({
                    type: "GET",
                    url: rootUri + "Money/RetrieveBossReport",
                    data: {
                        startdate: start_date,
                        enddate: end_date
                    },
                    success: function (data) {
                        //alert(data);
                        $("#bossreport_buying_count").val(data.buying_count);
                        $("#print_bossreport_buying_count").html(data.buying_count);
                        
                        $("#bossreport_buying_price").val(Math.round((data.buying_price) * 10) / 10);
                        $("#print_bossreport_buying_price").html(Math.round((data.buying_price) * 10) / 10);

                        $("#bossreport_buying_back_count").val(data.buying_back_count);
                        $("#print_bossreport_buying_back_count").html(data.buying_back_count);
                        
                        $("#bossreport_buying_back_price").val(Math.round((data.buying_back_price) * 10) / 10);
                        $("#print_bossreport_buying_back_price").html(Math.round((data.buying_back_price) * 10) / 10);
                        
                        $("#bossreport_buying_totalcount").val(data.buying_count - data.buying_back_count);
                        $("#print_bossreport_buying_totalcount").html(data.buying_count - data.buying_back_count);
                        
                        $("#bossreport_buying_totalprice").val(Math.round((data.buying_price - data.buying_back_price) * 10) / 10);
                        $("#print_bossreport_buying_totalprice").html(Math.round((data.buying_price - data.buying_back_price) * 10) / 10);
                        

                        $("#bossreport_sell_count").val(data.sell_count);
                        $("#print_bossreport_sell_count").html(data.sell_count);

                        $("#bossreport_sell_price").val(Math.round((data.sell_price) * 10) / 10);
                        $("#print_bossreport_sell_price").html(Math.round((data.sell_price) * 10) / 10);
  
                        $("#bossreport_sell_back_count").val(data.sell_back_count);
                        $("#print_bossreport_sell_back_count").html(data.sell_back_count);
  
                        $("#bossreport_sell_back_price").val(Math.round((data.sell_back_price) * 10) / 10);
                        $("#print_bossreport_sell_back_price").html(Math.round((data.sell_back_price) * 10) / 10);
  
                        $("#bossreport_sell_totalcount").val(data.sell_count - data.sell_back_count);
                        $("#print_bossreport_sell_totalcount").html(data.sell_count - data.sell_back_count);
  
                        $("#bossreport_sell_totalprice").val(Math.round((data.sell_price - data.sell_back_price) * 10) / 10);
                        $("#print_bossreport_sell_totalprice").html(Math.round((data.sell_price - data.sell_back_price) * 10) / 10);
  
                        
                        $("#bossreport_moving_out_count").val(data.moving_out_count);
                        $("#print_bossreport_moving_out_count").html(data.moving_out_count);
  
                        $("#bossreport_moving_in_count").val(data.moving_in_count);
                        $("#print_bossreport_moving_in_count").html(data.moving_in_count);


                        $("#bossreport_spending_loss_count").val(data.spending_loss_count);
                        $("#print_bossreport_spending_loss_count").html(data.spending_loss_count);
                        
                        $("#bossreport_spending_loss_price").val(Math.round((data.spending_loss_price) * 10) / 10);
                        $("#print_bossreport_spending_loss_price").html(Math.round((data.spending_loss_price) * 10) / 10);
                        
                        $("#bossreport_spending_more_count").val(data.spending_more_count);
                        $("#print_bossreport_spending_more_count").html(data.spending_more_count);
                        
                        $("#bossreport_spending_more_price").val(Math.round((data.spending_more_price) * 10) / 10);
                        $("#print_bossreport_spending_more_price").html(Math.round((data.spending_more_price) * 10) / 10);


                        $("#bossreport_remain_count").val(data.remain_count);
                        $("#print_bossreport_remain_count").html(data.remain_count);
                        
                        $("#bossreport_remain_price").val(Math.round((data.remain_price) * 10) / 10);
                        $("#print_bossreport_remain_price").html(Math.round((data.remain_price) * 10) / 10);


                        $("#bossreport_paying_in").val(Math.round((data.paying_in) * 10) / 10);
                        $("#print_bossreport_paying_in").html(Math.round((data.paying_in) * 10) / 10);
                        
                        $("#bossreport_paying_out").val(Math.round((data.paying_out) * 10) / 10);
                        $("#print_bossreport_paying_out").html(Math.round((data.paying_out) * 10) / 10);


                        $("#bossreport_money_sell").val(Math.round((data.money_sell) * 10) / 10);
                        $("#print_bossreport_money_sell").html(Math.round((data.money_sell) * 10) / 10);
                        
                        $("#bossreport_money_buying").val(Math.round((data.money_buying) * 10) / 10);
                        $("#print_bossreport_money_buying").html(Math.round((data.money_buying) * 10) / 10);
                        
                        $("#bossreport_money_profit").val(Math.round((data.money_sell - data.money_buying - data.money_smallchange)*10)/10);
                        $("#print_bossreport_money_profit").html(Math.round((data.money_sell - data.money_buying - data.money_smallchange)*10)/10);
                    }
                });
            }
        }            

        $(function () {
            $("#bossreport_start_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
	            buttonImageOnly: true,
	            dateFormat: 'yy-mm-dd',
	            option: $.datepicker.regional['zh-CN'],
                
            });
            $("#bossreport_end_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
	            buttonImageOnly: true,
	            dateFormat: 'yy-mm-dd',
	            option: $.datepicker.regional['zh-CN']
            });
            
            search_bossreport();
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
                     <input type=text id="bossreport_start_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" +String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" readonly/>
			    </td>
			    <td style="text-align:right">结束日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                     <input type=text id="bossreport_end_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" readonly/>
			    </td>
                <td style="text-align:right">
                    <b id="error_msg" style="display:none">日期过错！</b>
                    <button type="submit" class="btn_action" onclick="search_bossreport();" style="margin-right:0px;">统计</button>
                </td>
			</tr>
            <tr>
		    	<td style="text-align:right">统计开始日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                     <span id="bossreport_table_startdate"></span>
			    </td>
			    <td style="text-align:right">统计结束日期:&nbsp;&nbsp;&nbsp;
                </td>
			    <td valign=middle>
                     <span id="bossreport_table_enddate"></span>                     
			    </td>
           <%--     <td>
                </td>--%>
                <td style="text-align:right">
                    <button type="button" class="btn_actionex" onclick="printBossReport()" style="margin-right:0px;">打印</button>
                </td>
			</tr>
		</table>
		<table class="table table-striped table-advance table-bordered  table_view_bottom" id="tbl_bossreport">
			<tbody>
                <tr>
                    <td><h4><b>采购情况</b></h4></td>
                    <td>采购数量:</td>
                    <td><input id="bossreport_buying_count" class="form-control" readonly /></td>
                    <td>采购金额:</td>
                    <td><input id="bossreport_buying_price" class="form-control" readonly /></td>
                    <td>退货数量:</td>
                    <td><input id="bossreport_buying_back_count" class="form-control" readonly /></td>
                    <td>退货金额:</td>
                    <td><input id="bossreport_buying_back_price" class="form-control" readonly /></td>
                </tr>
                <tr>
                    <td></td>
                    <td>总数量:</td>
                    <td><input id="bossreport_buying_totalcount" class="form-control" readonly /></td>
                    <td>总金额:</td>
                    <td><input id="bossreport_buying_totalprice" class="form-control" readonly /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><h4><b>销售情况</b></h4></td>
                    <td>销售数量:</td>
                    <td><input id="bossreport_sell_count" class="form-control" readonly /></td>
                    <td>销售金额:</td>
                    <td><input id="bossreport_sell_price" class="form-control" readonly /></td>
                    <td>退货数量:</td>
                    <td><input id="bossreport_sell_back_count" class="form-control" readonly /></td>
                    <td>退货金额:</td>
                    <td><input id="bossreport_sell_back_price" class="form-control" readonly /></td>
                </tr>
                <tr>
                    <td></td>
                    <td>总数量:</td>
                    <td><input id="bossreport_sell_totalcount" class="form-control" readonly /></td>
                    <td>总金额:</td>
                    <td><input id="bossreport_sell_totalprice" class="form-control" readonly /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><h4><b>库存调拨情况</b></h4></td>
                    <td>调出数量:</td>
                    <td><input id="bossreport_moving_in_count" class="form-control" readonly /></td>
                    <td>调入数量:</td>
                    <td><input id="bossreport_moving_out_count" class="form-control" readonly /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><h4><b>报损报溢情况</b></h4></td>
                    <td>报损数量:</td>
                    <td><input id="bossreport_spending_loss_count" class="form-control" readonly /></td>
                    <td>报损金额:</td>
                    <td><input id="bossreport_spending_loss_price" class="form-control" readonly /></td>
                    <td>报溢数量:</td>
                    <td><input id="bossreport_spending_more_count" class="form-control" readonly /></td>
                    <td>报溢金额:</td>
                    <td><input id="bossreport_spending_more_price" class="form-control" readonly /></td>
                </tr>
                <tr>
                    <td><h4><b>库存状况</b></h4></td>
                    <td>库存总量:</td>
                    <td><input id="bossreport_remain_count" class="form-control" readonly /></td>
                    <td>库存总价:</td>
                    <td><input id="bossreport_remain_price" class="form-control" readonly /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><h4><b>付款收款情况</b></h4></td>
                    <td>应收合计:</td>
                    <td><input id="bossreport_paying_in" class="form-control" readonly /></td>
                    <td>应付合计:</td>
                    <td><input id="bossreport_paying_out" class="form-control" readonly /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><h4><b>财务分析</b></h4></td>
                    <td>销售金额:</td>
                    <td><input id="bossreport_money_sell" class="form-control" readonly /></td>
                    <td>销售成本:</td>
                    <td><input id="bossreport_money_buying" class="form-control" readonly /></td>
                    <td>销售利润:</td>
                    <td><input id="bossreport_money_profit" class="form-control" readonly /></td>
                    <td></td>
                    <td></td>
                </tr>
			</tbody>
		</table>
		
	</div>  
    
    
<div id="print_page_bossreport" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>老板一张表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">查询日期:
        <span id="print_bossreport_start_date"></span>&nbsp至
        <span id="print_bossreport_end_date"></span>
    </span>
    <span style="margin-right: 15px;"> 统计日期:
        <span id="print_bossreport_table_startdate"></span>&nbsp至
        <span id="print_bossreport_table_enddate"></span>    
    </span><br/>
	<table class="table_view" border="1">
		<tbody id="print_bossreport" style="background: white; border: 0px;">
              <tr>
                    <td rowspan="2" style="width:100px">采购情况</td>
                    <td style="width:100px">采购数量</td>
                    <td id="print_bossreport_buying_count" ></td>
                    <td style="width:100px">采购金额</td>
                    <td id="print_bossreport_buying_price"></td>
                    <td style="width:100px">退货数量</td>
                    <td id="print_bossreport_buying_back_count"></td>
                    <td style="width:100px">退货金额</td>
                    <td id="print_bossreport_buying_back_price"></td>
                </tr>
                <tr>
                    <td>总数量</td>
                    <td id="print_bossreport_buying_totalcount"></td>
                    <td id="print_bossreport_buying_totalprice">总金额</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td rowspan="2">销售情况</td>
                    <td>销售数量</td>
                    <td id="print_bossreport_sell_count"></td>
                    <td>销售金额</td>
                    <td id="priint_bossreport_sell_price"></td>
                    <td>退货数量</td>
                    <td id="print_bossreport_sell_back_count"></td>
                    <td>退货金额</td>
                    <td id="print_bossreport_sell_back_price"></td>
                </tr>
                <tr>
                    <td>总数量</td>
                    <td id="print_bossreport_sell_totalcount"></td>
                    <td>总金额</td>
                    <td id="print_bossreport_sell_totalprice"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>库存调拨情况</td>
                    <td>调出数量</td>
                    <td id="print_bossreport_moving_in_count"></td>
                    <td>调入数量</td>
                    <td id="print_bossreport_moving_out_count"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>报损报溢情况</td>
                    <td>报损数量</td>
                    <td id="print_bossreport_spending_loss_count"></td>
                    <td>报损金额</td>
                    <td id="print_bossreport_spending_loss_price"></td>
                    <td>报溢数量</td>
                    <td id="print_bossreport_spending_more_count"></td>
                    <td>报溢金额</td>
                    <td id="print_bossreport_spending_more_price"></td>
                </tr>
                <tr>
                    <td>库存状况</td>
                    <td>库存总量</td>
                    <td id="print_bossreport_remain_count"></td>
                    <td>库存总价</td>
                    <td id="print_bossreport_remain_price"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>付款收款情况</td>
                    <td>应收合计</td>
                    <td id="print_bossreport_paying_in"></td>
                    <td>应付合计</td>
                    <td id="print_bossreport_paying_out"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>财务分析</td>
                    <td>销售金额</td>
                    <td id="print_bossreport_money_sell"></td>
                    <td>销售成本</td>
                    <td id="print_bossreport_money_buying"></td>
                    <td>销售利润</td>
                    <td id="print_bossreport_money_profit"></td>
                    <td></td>
                    <td></td>
                </tr>			
		</tbody>
	</table>
</div> 
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>
    <script type="text/javascript">

        function printBossReport() {
            if ($("#tbl_bossreport tbody").find("tr") == null) {
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

            $("#print_bossreport_start_date").html($("#bossreport_start_date").val());
            $("#print_bossreport_end_date").html($("#bossreport_end_date").val());
            
           // $("#print_bossreport").html($("#tbl_bossreport tbody").html());

            pageprintboss();
        }
        function beforePrintboss() {
        }
        function afterPrintboss() {
            $("#header").css("display", "block");
            $("#tabs-4").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_bossreport").css("display", "none");
        }

        function pageprintboss() {
            window.onbeforeprint = beforePrintboss;
            window.onafterprint = afterPrintboss;
            $("#header").css("display", "none");
            $("#tabs-4").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_bossreport").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-4").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_bossreport").css("display", "none");
        }

    </script>    
</body>
</html>
