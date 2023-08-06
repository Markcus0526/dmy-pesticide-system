<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>  
</head>

<body>
    <div id="tabs-3">
        <table class="table_edit" cellspacing=0 cellpadding=0 style="width:100%;">
			<tr>
                <td style="text-align:right">
                    收支方式:&nbsp;&nbsp;&nbsp; 
                    </td>
				<td align=left class="search_title">
                    <select id="search_stat_type" name="" style="width:110px" onchange="search_stat();" class="form-control">
                        <option value="2">全部</option>
                        <option value="0">支出</option>
                        <option value="1">收入</option>
                    </select>
                </td>
                <td style="text-align:right">开始日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                    <input type=text name="start_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" id="stat_start_date" readonly/>
			    </td>
                <td style="text-align:right">结束日期:&nbsp;&nbsp;&nbsp;
                    </td>
			    <td valign=middle>
                    <input type=text name="end_date" class="date_input form-control" style="float:left;margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" id="stat_end_date"  readonly/>
			    </td>
				<td>
                    <button type="submit" class="btn_action" onclick="search_stat();" style="margin-right:0px;">查询</button>
				</td>	
			</tr>
            <tr>
                <td></td>
				<td></td>
                <td></td>
			    <td></td>
                <td></td>
			    <td></td>
				<td>
                    <button type="button" class="btn_action" data-target="#dialog2" data-toggle="modal" onclick="clear_stat_dlg();" style="margin-right:0px;">添加</button>
				</td>
                </tr>
		</table>
		<table class="table table-striped  table-advance table-bordered table-hover" id="tbl_stat">
            <thead>
			    <tr>
		    	    <th style="width:130px;text-align:center">日期</th>
    			    <th style="width:95px;text-align:center">收支方式</th>
	    		    <th style="width:160px;text-align:center">金额</th>
		    	    <th style="text-align:center">摘要</th>
			        <th style="width:90px;text-align:center">操作</th>
			    </tr>
            </thead>
			<tbody>
			</tbody>
		</table>
        <table class="table_bottom">
			<tr>
                <td style="width:30%;">
                </td>
                <td style="width:120px;text-align:right">
                    支出合计:&nbsp;&nbsp;&nbsp;
                </td>
		    	<td>
                    <input type="text" id="total_zhichu" value="<% = ViewData["total_zhichu"] %>元" disabled class='form-control'/>
		    	</td>
                <td style="width:120px;text-align:right">
                    收入合计:&nbsp;&nbsp;&nbsp;
                </td>
		    	<td>
                    <input type="text" id="total_shouru" value="<% = ViewData["total_shouru"] %>元" disabled class='form-control'/>
		    	</td>
			</tr>
             <tr>
                <td></td>
                <td></td>
                <td></td>
			    <td></td>
				<td>
                    <button type="button" class="btn_actionex" onclick ="printStatisticsDetail()" style="margin-right:0px;">打印</button>
				</td>
                </tr>
		</table>
        
        <div id="dialog2" class="modal fade" tabindex="-1" data-width="600px">
            <form action="#" id="submit_form" class="form-horizontal form-validate">
            <div class="modal-dialog" style="width:600px">
				<div class="modal-content">
					<div class="modal-body">
                        <table class="table_dialog" border=0 style="width:80%">
                            <tr>
			                    <td style="vertical-align:top">
                                   收支类型 ：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align:top">   
                                    <div class="form-group">
				                        <input type="radio" name="stat_type" value="1" id="stat_type1" class="s_input" checked />
					                    收入&nbsp;&nbsp;&nbsp;&nbsp;
                                        <input type="radio" name="stat_type" value="0" id="stat_type0" class="s_input" />
					                    支出
                                    </div>                                 
                                     
			                    </td>
			                </tr>
                            <tr>
			                    <td style="vertical-align:top">
                                   收支方式 ：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align:top">   
                                    <div class="form-group">
				                        <input type="radio" name="stat_paytype" value="2" id="stat_paytype2" class="s_input" checked />
					                    银行存款&nbsp;&nbsp;&nbsp;&nbsp;
                                        <input type="radio" name="stat_paytype" value="0" id="stat_paytype0" class="s_input" />
					                    现金
                                    </div>                                 
                                     
			                    </td>
			                </tr>
                            <tr>
			                    <td style="vertical-align:top">
                                    金额：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align:top">
                                    <div class="form-group">
                                    <input type=text name="statprice" id="statprice" value="" class="form-control" style="width:300px;"/>
                                        </div>
			                    </td>
			                </tr>
                            <tr>
			                    <td style="vertical-align:top">
                                    理由：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align:top">
                                    <div class="form-group">
                                    <input type=text name="statreason" id="statreason" value="" class="form-control" style="width:300px;"/>
                                        </div>
			                    </td>
			                </tr>
                        </table>
                    </div>
                    <div class ="modal-hidden" style="display:none">
                        <input type=text name="stat_id" id="stat_id" value="" />
                        <a id="real_Stat" data-target="#dialog2" data-toggle="modal"></a>
                    </div>
                    
					<div class="modal-footer">
                        <button type="button" onclick="SubmitStatistics();" class="btn_rect">保存</button>
						<button type="button" id="cancelSubmit" data-dismiss="modal" class="btn_rect">取消</button>
					</div>
                </div>
            </div>
                
            </form>
         </div>
	</div>

    
		<div id="print_page_statisticshistory" style="display: none; font-size: 16px;">
	        <p style="text-align:center;"><font style="font-size: 24px;"><b>收支统计表</b></font></p><br/>
	        <span style="float: left; margin-left: 15px;">查询日期:<span id="history_date1" class="print_history_date"></span></span><span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br/>
	        <table class="table_view" border="1">
		        <thead>
			        <tr>				       
				        <th style="text-align:center">日期</th>				      
				        <th style="text-align:center">收支方式</th>
				        <th style="text-align:center">金额(元)</th>
				        <th style="text-align:center">理由</th>				       
			        </tr>
		        </thead>
		        <tbody id="print_statisticshistory_body" style="background: white; border: 0px;">			
		        </tbody>
	        </table>
        </div>


    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>
    <script type="text/javascript">

        var oTable;      
        var handleDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            oTable = $('#tbl_stat').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveStatistics",

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
                    {
                        aTargets: [4],    // Column number which needs to be modified
                        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            var rhtml = "<a onclick=\"Edit_Statistics(\'" + o.aData[4] + "\');\"><img src='" + "../../Content/img/pen.png" + "' style='width:20px; height:20px;'/></a>"; /*&nbsp;&nbsp;"; +
                                    "<a href='javascript:void(0);'  onclick=\"return remove_row(\'" + o.aData[4] + "\');\"><img src='" + "../../Content/img/thresh.png" + "' style='width:20px; height:20px;'/></a>";*/
                            return rhtml;
                        },
                        sClass: 'tableCell'    // Optional - class to be applied to this table cell
                    }
                ]
            });
        }
        $(function () {
            $("#stat_start_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            $("#stat_end_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            
            handleDataTable();

            var form_customer = $("#submit_form");
            error = $('.alert-danger', form_customer);
            success = $('.alert-success', form_customer);

            $.validator.messages.required = "必须要填写";
            $.validator.messages.remote = "请输入正确的"
            $.validator.messages.email = "请输入Email格式"
            $.validator.messages.url = "请输入URL格式"
            $.validator.messages.date = "请输入日期"
            $.validator.messages.dateISO = "请输入日期ISO"
            $.validator.messages.number = "请输入数字"
            $.validator.messages.digits = "请输入数字"
            $.validator.messages.creditcard = "请输入信用卡格式"
            $.validator.messages.equalTo = "确认密码不同"
            $.validator.messages.maxlength = $.validator.format("输入的文字数量超过了");
            $.validator.messages.minlength = $.validator.format("输入的文字数量太少");
            $.validator.messages.rangelength = $.validator.format("输入的文字数量没在范围");
            $.validator.messages.range = $.validator.format("输入的数值没在范围");
            $.validator.messages.max = $.validator.format("输入的数值超过了");
            $.validator.messages.min = $.validator.format("输入的数值太少");


            form_customer.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    statprice: {
                        required: true,
                        number: true,
                        minlength: 1,
                        maxlength: 8
                    },
                    statreason: {
                        required: true
                    }
                },

                errorPlacement: function (error, element) { // render error placement for each input type
                    if (element.attr("name") == "gender") { // for uniform radio buttons, insert the after the given container
                        error.insertAfter("#form_gender_error");
                    } else if (element.attr("name") == "payment[]") { // for uniform radio buttons, insert the after the given container
                        error.insertAfter("#form_payment_error");
                    } else {
                        error.insertAfter(element); // for other inputs, just perform default behavior
                    }
                },

                invalidHandler: function (event, validator) { //display error alert on form submit   
                    success.hide();
                    error.show();
                    App.scrollTo(error, -200);
                },

                highlight: function (element) { // hightlight error inputs

                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
                },

                unhighlight: function (element) { // revert the change done by hightlight

                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },

                success: function (label) {
                    if (label.attr("for") == "gender" || label.attr("for") == "payment[]") { // for checkboxes and radio buttons, no need to show OK icon
                        label
                            .closest('.form-group').removeClass('has-error').addClass('has-success');
                        label.remove(); // remove error label here
                    } else { // display success icon for other inputs
                        label
                            .addClass('valid') // mark the current input as valid and display OK icon
                        .closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                    }
                },

                submitHandler: function (form) {
                    success.show();
                    error.hide();
                    //add here some ajax code to submit your form or just call form.submit() if you want to submit the form without ajax
                }

            });
        });
        function Edit_Statistics(id) {
            $.ajax({
                type: "GET",
                url: "<%= ViewData["rootUri"] %>Money/RetrieveEditStatistics",
                dataType: 'json',
                data: {
                    id: id,
                },
                success: function (data) {
                    $(".has-success").removeClass("has-success");
                    $(".has-error").removeClass("has-error");
                    $(".help-block").remove();

                    var result = data.statInfo;
                    $("#stat_id").val(result.id);
                    //$("#stat_type").val(result.type);
                    if (result.type == 0)
                        $("#stat_type0").attr("checked", "checked");
                    else
                        $("#stat_type1").attr("checked", "checked");
                    
                    if (result.paytype == 0)
                        $("#stat_paytype0").attr("checked", "checked");
                    else
                        $("#stat_paytype2").attr("checked", "checked");
                    $("#statprice").val(result.price);
                    $("#statreason").val(result.reason);
                    $("#real_Stat").trigger("click");
                }
            });
        }
        function clear_stat_dlg() {
            $(".has-success").removeClass("has-success");
            $(".has-error").removeClass("has-error");
            $(".help-block").remove();

            $("stat_id").val("");
            //$("#stat_type").val(1);
            $("#stat_type1").attr("checked", "checked");
            $("#stat_paytype0").attr("checked", "checked");
            $("#statprice").val("");
            $("#statreason").val("");
        }

        function SubmitStatistics() {
            if ($('#submit_form').valid()) {
                $("#cancelSubmit").trigger("click");
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "<%= ViewData["rootUri"] %>Money/SubmitStatistics",
                    dataType: "json",
                    data: $('#submit_form').serialize(),
                    success: function (data) {
                        $(".has-success").removeClass("has-success");
                        $(".has-error").removeClass("has-error");
                        $(".help-block").remove();

                        if (data == true) {
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
                            toastr["success"]("操作成功！", "恭喜您");
                            refreshStatisticsTable();
                        } else {
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
                            toastr["error"](data.error, "客户没有");
                        }
                    },
                    error: function (data) {
                        $(".has-success").removeClass("has-success");
                        $(".has-error").removeClass("has-error");
                        $(".help-block").remove();

                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }
        function remove_row(id) {
            bootbox.dialog({
                message: "您确定要删除吗？",
                buttons: {
                    danger: {
                        label: "取消",
                        className: "btn-dialog",
                        callback: function () {
                            return true;
                        }
                    },
                    main: {
                        label: "确定",
                        className: "btn-dialog",
                        callback: function () {
                            $.ajax({
                                async: false,
                                type: "POST",
                                url: rootUri + "Money/DeleteStatistics",
                                dataType: "json",
                                data: {
                                    id: id,
                                },
                                success: function (data) {
                                    if (data == true) {
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
                                        toastr["success"]("删除成功！", "恭喜您");
                                        refreshStatisticsTable();
                                    } else {
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
                                        toastr["error"](data.error, "温馨敬告");
                                    }
                                },
                                error: function (data) {
                                    alert("Error: " + data.status);
                                    $('.loading-btn').button('reset');
                                }
                            });
                        }
                    }
                }
            });


        }
        function refreshStatisticsTable() {
            oSettings = oTable.fnSettings();
            oTable.fnDraw();
            getTotalShouZhi();
            //Retrieve the new data with $.getJSON. You could use it ajax too
            /*$.getJSON(oSettings.sAjaxSource, null, function (json) {
               // oTable.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    oTable.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                oTable.fnDraw();
            });*/
        }
        function search_stat() {
            var f_start_date = $("#stat_start_date").val();
            var f_end_date = $("#stat_end_date").val();
            if (f_start_date > f_end_date) {
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
                var s_type = $("#search_stat_type").val();
                $.ajax({
                    url: rootUri + "Money/FilterStat",
                    data: {
                        search_stat_type: s_type,
                        start_date: f_start_date,
                        end_date: f_end_date
                    },
                    success: function (data) {
                        $("#total_zhichu").val(data.zhichu + "元");
                        $("#total_shouru").val(data.shouru + "元");

                        refreshStatisticsTable();
                    }
                });
            }
        }

        function getTotalShouZhi() {
            $.ajax({
                type: "GET",
                url: "<%= ViewData["rootUri"] %>Money/GetTotalShouZhi",
                dataType: 'json',
                data: {
                },
                success: function (data) {
                    $("#total_zhichu").val(data.zhichu + "元");
                    $("#total_shouru").val(data.shouru + "元");
                }
            });
        }

        function printStatisticsDetail() {
            $(".print_history_date").html($("#start_date").val() + "至" + $("#end_date").val());

            var tableRows = $("#tbl_stat tbody tr");

            var htmlStr = "";
            for (var i = 0; i < tableRows.length; i++) {
                var row = $(tableRows[i]);
                htmlStr += "<tr>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(0).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(1).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(2).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(3).html() + "</td>" +
                    "</tr>";
            }
            $("#print_statisticshistory_body").html(htmlStr);
            
            pageprint_statisticshistory();
        }

    </script>
    
     <script type="text/javascript">
           var initBody;
          function beforePrint() {
            }
            function afterPrint_statisticshistory() {
                $("#header").css("display", "block");
                $("#tabs-3").css("display", "block");
                $(".ui-tabs-nav").css("display", "block");
                $("#print_page_statisticshistory").css("display", "none");
            }
            function pageprint_statisticshistory() {
                window.onbeforeprint = beforePrint;
                window.onafterprint = afterPrint_statisticshistory;
                $("#header").css("display", "none");
                $("#tabs-3").css("display", "none");
                $(".ui-tabs-nav").css("display", "none");
                $("#print_page_statisticshistory").css("display", "block");
                window.print();
                $("#header").css("display", "block");
                $("#tabs-3").css("display", "block");
                $(".ui-tabs-nav").css("display", "block");
                $("#print_page_statisticshistory").css("display", "none");
            }
       </script>
</body>
</html>
