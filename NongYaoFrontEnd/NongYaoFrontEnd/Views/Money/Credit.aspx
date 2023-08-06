<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-2">
        <button style="display: none;" type="button" id="detail_dlg2" data-target="#viewCreditDlg" data-toggle="modal"></button>
        <table class="table_edit" cellspacing="10" cellpadding="0" style="width: 100%;">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: right; width:100px;">客户名称:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width:250px;">
                                <input type="text" name="" id="search_input" placeholder="请输入客户名称或关键字" class="small_input_text form-control" style="width: 230px;">
                            </td>
                            <td style="text-align: right; width:200px;">类型:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width:100px;">
                                <select id="search_type" name="" style="width: 110px" class="form-control">
                                    <option value="2">全部</option>
                                    <option value="1">应收</option>
                                    <option value="0">应付</option>
                                </select>
                            </td>
                            <td style="width:300px;" align="center">
                                <button type="submit" class="btn_actionex" onclick="search_Credit()" style="margin-right: 0px; margin-top:10px;float: right;">搜索</button>
                            </td>
                            <!--
                            <td>
                                <button type="submit" class="btn_rect" onclick="search_Credit();" style="float: right;">搜索</button>
                            </td>
                            <td>
                                <button type="submit" class="btn_rect" onclick="clear_Search();" style="float: right;">条件清空</button>
                            </td>
                            -->
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: right">开始日期:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="middle">
                                <input type="text" name="start_date1" class="date_input form-control" style="float: left; margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" id="start_date1" readonly />
                            </td>
                            <td style="text-align: right">结束日期:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="middle">
                                <input type="text" name="end_date1" class="date_input form-control" style="float: left; margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" id="end_date1" readonly />
                            </td>

                            <td>
                                <button type="submit" class="btn_actionex" onclick="clear_Search()" style="margin-right: 0px; margin-top:10px; float: right;">条件清空</button>
                            </td>
                            <td>
                                <button type="submit" class="btn_rect" data-target="#dialog1" data-toggle="modal" onclick="clear_dlg();" style="">添加</button>
                            
                            </td>
                            <td>
                                <button type="submit" class="btn_actionex" onclick="printCreditPage()" style="margin-right: 0px; margin-top:10px;float: right;">打印</button>
                            </td>
                            <td>
                                <button type="submit" class="btn_actionex" onclick="showAddHistory()" style="margin-right: 0px; margin-top:10px;float: right;">历史信息</button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="table table-striped  table-advance table-bordered table-hover" id="tbllist_credit">
            <thead>
                <tr>
                    <th style="width: 150px; text-align: center">日期</th>
                    <th style="text-align: center; width:150px;">客户名称</th>
                    <th style="text-align: center">金额</th>
                    <%--<th style="text-align: center">应收金额</th>
                    <th style="text-align: center">应付金额</th>--%>
                    <th style="width: 110px; text-align: center">明细</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>

        <div id="viewCreditDlg" class="modal fade" tabindex="-1" data-width="1000px" style="top:200px;">
            <form action="#" id="submit_form_detail" class="form-horizontal form-validate">
                <div class="modal-dialog" style="width: 1000px">
                    <div class="modal-content">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <h2>应收应付明细表</h2>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table style="width:95%">
                                <tr>
                                    <td style="text-align:right;">
                                        <button class="btn_rect" onclick="printCreditDetail()">打印</button>
                                        <button type="button" data-dismiss="modal" id="Button1" class="btn_rect">关闭</button>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div class="modal-body">
                            <table class="table table-striped  table-advance table-bordered table-hover" border="0" id="DetailCreditTable">
                                <thead>
                                    <tr>
                                        <th style="width: 130px; text-align: center">日期</th>
                                        <th style="text-align: center">单号</th>
                                        <th style="text-align: center">内容摘要</th>
                                        <th style="text-align: center">应收/应付金额</th>
                                        <th style="text-align: center">实收/实付金额</th>
                                        <th style="text-align: center">经办人</th>
                                        <th style="text-align: center">备注</th>

                                    </tr>
                                </thead>

                            </table>
                        </div>

                    </div>
                </div>
            </form>
        </div>
        <div id="dialog1" class="modal fade" tabindex="-1" data-width="600px">
            <form action="#" id="submit_form" class="form-horizontal form-validate">
                <div class="modal-dialog" style="width: 600px">
                    <div class="modal-content">
                        <div class="modal-body">
                            <table class="table_dialog" border="0" style="width: 80%">
                                <tr>
                                    <td style="vertical-align: top">客户名称 ：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top">
                                        <div class="form-group">
                                            <input type="text" id="cName" name="cName" class="form-control" value="" data-required="1" style="width: 300px;" onchange="changeCustomerNameOrPhone1(false)" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">手机号码 ：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top">
                                        <div class="form-group">
                                            <input type="text" id="cPhone" name="cPhone" class="form-control" value="" style="width: 300px;" onchange="changeCustomerNameOrPhone1(true)" onkeyup="this.value=this.value.replace(/[^\d]/,'')" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">实收/实付：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top">
                                        <div class="form-group">
                                            <select name="type" id="type" class="form-control" style="width: 300px">
                                                <option selected value="1">实收</option>
                                                <option value="0">实付</option>
                                            </select>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">收款/付款方式：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top">
                                        <div class="form-group">
                                            <input type="radio" name="paytype" value="2" id="paytype2" class="s_input" />
                                            银行存款&nbsp;&nbsp;&nbsp;&nbsp;
                                        <input type="radio" name="paytype" value="0" id="paytype0" class="s_input" checked />
                                            现金
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">金额：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top; padding: 0;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <div class="form-group">
                                                        <input type="text" name="price" id="price" value="" style="width: 120px;" class="form-control"/>
                                                    </div>
                                                </td>
                                                <td style="vertical-align: top">&nbsp;&nbsp;抹零：&nbsp;&nbsp;
                                                </td>
                                                <td style="vertical-align: top">
                                                    <div class="form-group">
                                                        <input type="text" name="smallchange" id="smallchange" value="" style="width: 120px;" class="form-control"/>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">备注 ：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top">
                                        <div class="form-group">
                                            <input type="text" id="reason" name="reason" value="" style="width: 300px;" class="form-control"/>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">操作员 ：&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="vertical-align: top">
                                        <div class="form-group">
                                            <input type="text" value="<% =ViewData["username"] %>" style="width: 300px;" disabled />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-hidden" style="display: none">
                            <input type="text" name="date" id="date" value="" />
                            <input type="text" name="id" id="id" value="" />
                            <a id="real_act" data-target="#dialog1" data-toggle="modal"></a>
                        </div>

                        <div class="modal-footer">
                            <button type="button" onclick="submitRealPayment();" class="btn_rect">保存</button>
                            <button type="button" id="cancel" data-dismiss="modal" class="btn_rect">取消</button>
                        </div>
                    </div>
                </div>


            </form>
        </div>
    </div>
    <div id="add_History" style="display:none;">
        <table class="table_edit" cellspacing="10" cellpadding="0" style="width: 100%;">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: right; width:100px;">客户名称:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width:250px;">
                                <input type="text" name="" id="a_search_input" placeholder="请输入客户名称" class="small_input_text form-control" style="width: 230px;">
                            </td>
                            <td style="text-align: right; width:200px;">实收/实付:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width:100px;">
                                <select id="a_search_type" name="" style="width: 110px" class="form-control">
                                    <option value="2">全部</option>
                                    <option value="1">实收</option>
                                    <option value="0">实付</option>
                                </select>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: right">开始日期:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="middle">
                                <input type="text" name="a_start_date" class="date_input form-control" style="float: left; margin-right: 5px;" value="<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>" id="a_start_date" readonly />
                            </td>
                            <td style="text-align: right">结束日期:&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="middle">
                                <input type="text" name="a_end_date" class="date_input form-control" style="float: left; margin-right: 5px;" value="<%=String.Format("{0:yyyy-MM-dd}",DateTime.Now) %>" id="a_end_date" readonly />
                            </td>
                            <td>
                                <button type="submit" class="btn_actionex" onclick="clear_searchAddHistory()" style="margin-right: 0px; float: right;">条件清空</button>
                            </td>
                            <td>
                                <button type="submit" class="btn_actionex" onclick="searchAddHistory()" style="margin-right: 0px; float: right;">搜索</button>
                            </td>
                            <td>
                                <button type="submit" class="btn_actionex" onclick="printAddHistory()" style="margin-right: 0px; float: right;">打印</button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="table table-striped  table-advance table-bordered table-hover" id="addhistoryTable">
            <thead>
                <tr>
                    <th style="width: 150px; text-align: center">日期</th>
                    <th style="text-align: center; width:150px;">客户</th>
                    <th style="text-align: center">金额</th>
                    <th style="text-align: center">抹零金额</th>
                    <th style="width: 110px; text-align: center">备注</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <table class="table table-striped  table-advance table-hover" style="text-align:right;">
            <tr>
                <td>
                    <button type="submit" class="btn_actionex" onclick="backOriginPage()" style="margin-right: 0px; float: right;">关闭</button>
                </td>
            </tr>
        </table>

    </div>
    <div id="print_page_credithistory" style="display: none; font-size: 16px;">
        <p style="text-align: center;"><font style="font-size: 24px;"><b>应收应付表</b></font></p>
        <br />
        <span style="float: left; margin-left: 15px;">查询日期:<span id="history_date2" class="print_history_date"></span></span><span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br />
        <table class="table_view" border="1">
            <thead>
                <tr>
                    <th style="text-align: center">日期</th>
                    <th style="text-align: center">客户姓名</th>
                    <th style="text-align: center">金额</th>
                </tr>
            </thead>
            <tbody id="print_credithistory_body" style="background: white; border: 0px;">
            </tbody>
        </table>
    </div>
    <div id="print_page_creditdetail" style="display: none; font-size: 16px;">
        <p style="text-align: center;"><font style="font-size: 24px;"><b>应收应付明细表</b></font></p>
        <br />
        <span style="float: left; margin-left: 15px;">查询日期:<span id="detail_date2" class="print_detail_date"></span></span><span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br />
        <table class="table_view" border="1">
            <thead>
                <tr>
                    <th style="text-align: center">日期</th>
                    <th style="text-align: center">单号</th>
                    <th style="text-align: center">内容摘要</th>
                    <th style="text-align: center">应收/应付金额</th>
                    <th style="text-align: center">实收/实付金额</th>
                    <th style="text-align: center">经办人</th>
                    <th style="text-align: center">备注</th>
                </tr>
            </thead>
            <tbody id="print_creditdetail_body" style="background: white; border: 0px;">
            </tbody>
        </table>
    </div>
    <div id="print_page_addhistory" style="display: none; font-size: 16px;">
        <p style="text-align: center;"><font style="font-size: 24px;"><b>实收/实付表</b></font></p>
        <br />
        <span style="float: left; margin-left: 15px;">查询日期:<span id="addhistory_date" class="print_add_history_date"></span></span><span style="float: right; margin-right: 15px;">打印日期:<% =ViewData["curdate"] %></span><br />
        <table class="table_view" border="1">
            <thead>
                <tr>
                    <th style="text-align: center">日期</th>
                    <th style="text-align: center">客户</th>
                    <th style="text-align: center">金额</th>
                    <th style="text-align: center">抹零金额</th>
                    <th style="text-align: center">备注</th>
                </tr>
            </thead>
            <tbody id="print_addhistory_body" style="background: white; border: 0px;">
            </tbody>
        </table>
    </div>

    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/i18n/jquery.ui.datepicker-zh-CN.js"></script>

    <script type="text/javascript">
        var oTable;
        var detailTable;
        var addhistoryTable;
        var handleAddhistoryTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            addhistoryTable = $('#addhistoryTable').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveAddHistory",

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
                ]
            });
        }
        var handleDetailTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            detailTable = $('#DetailCreditTable').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveDetailCredit",

                "aoColumns": [
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
                    //{
                    //    aTargets: [3],    // Column number which needs to be modified
                    //    fnRender: function (o, v) {   // o, v contains the object and value for the column
                    //        var rhtml = "<a  onclick=\"Edit_Credit(\'" + o.aData[4] + "\');\"><img src='" + "../../Content/img/pen.png" + "' style='width:20px; height:20px;'/></a>&nbsp;&nbsp;" +
                    //                "<a href='javascript:void(0);'  onclick=\"return remove_row(\'" + o.aData[4] + "\');\"><img src='" + "../../Content/img/thresh.png" + "' style='width:20px; height:20px;'/></a>";
                    //        var rhtml = "<button class='btn default' onclick=\"detailView_Credit(\'" + o.aData[3] + "\');\" style=\"margin-right:0px;\">点击查看</button>";
                    //        return rhtml;
                    //    },
                    //    sClass: 'tableCell'    // Optional - class to be applied to this table cell
                    //}
                ]
            });
        }
        var handleDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }
            // begin first table
            oTable = $('#tbllist_credit').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Money/RetrieveCredit",

                "aoColumns": [
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
                        aTargets: [3],    // Column number which needs to be modified
                        fnRender: function (o, v) {   // o, v contains the object and value for the column
                            //var rhtml = "<a  onclick=\"Edit_Credit(\'" + o.aData[4] + "\');\"><img src='" + "../../Content/img/pen.png" + "' style='width:20px; height:20px;'/></a>&nbsp;&nbsp;" +
                            //        "<a href='javascript:void(0);'  onclick=\"return remove_row(\'" + o.aData[4] + "\');\"><img src='" + "../../Content/img/thresh.png" + "' style='width:20px; height:20px;'/></a>";
                            var rhtml = "<button class='btn_rect' onclick=\"detailView_Credit(\'" + o.aData[3] + "\');\" style=\"margin-right:0px;\">点击查看</button>";
                            return rhtml;
                        },
                        sClass: 'tableCell'    // Optional - class to be applied to this table cell
                    }
                ]
            });
        }
        $(function () {
            $("#start_date1").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            $("#end_date1").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            clean_search_Credit();
            handleDataTable();
            handleDetailTable();
            handleAddhistoryTable();
            //clear_Search();
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
                    cName: {
                        required: true
                    },
                    cPhone: {
                        number: true,
                        maxlength: 11,
                        required: true
                    },
                    price: {
                        required: true,
                        number: true
                    },
                    smallchange: {
                        required: true,
                        number: true
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
                                url: rootUri + "Money/DeleteCredit",
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
                                        refreshCreditTable();
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
                
        

        function detailView_Credit(id) {
            $.ajax({
                type: "POST",
                url: "<%= ViewData["rootUri"] %>Money/changeDetailId",
                dataType: 'json',
                data: {
                    id: id
                },
                success: function (data) {
                    refreshDetailCreditTable();
                    $("#detail_dlg2").click();
                }
            });
            
        }

        function changeCustomerNameOrPhone1(isPhone) {
            var search = "";
            if (isPhone) {
                search = $("#submit_form input[name='cPhone']").val();

                if (isNaN(search) == true) {
                    $("#submit_form input[name='cPhone']").parent().removeClass('has-success').addClass('has-error');
                    return;
                }

                if (search.length == 0 || search.length > 11) {
                    $("#submit_form input[name='cPhone']").parent().removeClass('has-success').addClass('has-error');
                    return;
                } else {
                    $("#submit_form input[name='cPhone']").parent().removeClass('has-error').addClass('has-success');
                }
            } else {
                search = $("#submit_form input[name='cName']").val();
                if (search.length > 0)
                    $("#submit_form input[name='cName']").parent().removeClass('has-success').removeClass('has-error');
            }
            //alert(search + " | " + isPhone);
            if (search != null && search.length > 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Sale/GetUsernameOrPhone",
                    dataType: "json",
                    data: {
                        "search": search,
                        "isphone": isPhone
                    },
                    success: function (data) {
                        if (data.result.length > 0) {
                            if (data.isphone == "true")
                                $("#submit_form input[name='cName']").val(data.result);
                            else
                                $("#submit_form input[name='cPhone']").val(data.result);
                        }
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }

        function clear_dlg() {
            $(".has-success").removeClass("has-success");
            $(".has-error").removeClass("has-error");
            $(".help-block").remove();

            $("id").val("");
            $("date").val("");
            $("#cName").val("");
            $("#cPhone").val("");
            $("#type").val(1);
            $("#price").val("");
            $("#smallchange").val("");
            $("#reason").val("");
        }
        function submitRealPayment() {
            if ($('#submit_form').valid()) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Money/SubmitRealPay",
                    dataType: "json",
                    data: $("#submit_form").serialize(),
                    success: function (data) {
                        if (data == true) {
                            $("#cancel").trigger("click");
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
                            refreshCreditTable();
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
                            toastr["error"](data.error, "客户信息不对");
                        }
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }

        function showAddHistory() {
            $("#a_start_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            $("#a_end_date").datepicker({
                showOn: 'button',
                buttonImage: '<%= ViewData["rootUri"] %>Content/image/calendar.png',
                buttonImageOnly: true,
                dateFormat: 'yy-mm-dd',
                option: $.datepicker.regional['zh-CN']
            });
            refreshAddHistoryTable();
            $("#tabs-2").hide();
            $("#add_History").show();
        }
        function backOriginPage() {
            clear_searchAddHistory();
            $("#tabs-2").show();
            $("#add_History").hide();
        }

        function refreshCreditTable() {
            oSettings = oTable.fnSettings();
            oTable.fnDraw();
        }
        function refreshDetailCreditTable() {
            oSettings = detailTable.fnSettings();
            detailTable.fnDraw();
        }
        function refreshAddHistoryTable() {
            oSettings = addhistoryTable.fnSettings();
            addhistoryTable.fnDraw();
        }
        function search_Credit() {
            var c_customer_name = $("#search_input").val();
            var c_paytype = $("#search_type").val();
            var f_start_date = $("#start_date1").val();
            var f_end_date = $("#end_date1").val();
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
                return;
            }
            $.ajax({
                url: rootUri + "Money/FilterCredit",
                data: {
                    start_date: f_start_date,
                    end_date: f_end_date,
                    customer_name: c_customer_name,
                    paytype: c_paytype
                },
                success: function (data) {
                    refreshCreditTable();
                }
            });
        }
        function clean_search_Credit() {
            $.ajax({
                url: rootUri + "Money/CleanFilterCredit",
                data: {
                },
                success: function (data) {
                    //refreshCreditTable();
                }
            });
        }
        function clear_Search() {
            var f_start_date = "<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>";
            var f_end_date = $("#end_date1").val();
            var c_customer_name = "";
            var c_paytype = 2;
            $("#search_input").val("");
            $("#search_type").val(2);
            $("#start_date1").val(f_start_date);
            $.ajax({
                url: rootUri + "Money/FilterCredit",
                data: {
                    start_date: f_start_date,
                    end_date: f_end_date,
                    customer_name: c_customer_name,
                    paytype: c_paytype
                },
                success: function (data) {
                    refreshCreditTable();
                }
            });
        }
        function searchAddHistory() {
            var a_customer_name = $("#a_search_input").val();
            var a_paytype = $("#a_search_type").val();
            var a_start_date = $("#a_start_date").val();
            var a_end_date = $("#a_end_date").val();
            if (a_start_date > a_end_date) {
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
                return;
            }
            $.ajax({
                url: rootUri + "Money/FilterAddHistory",
                data: {
                    start_date: a_start_date,
                    end_date: a_end_date,
                    customer_name: a_customer_name,
                    paytype: a_paytype
                },
                success: function (data) {
                    refreshAddHistoryTable();
                }
            });
        }
        function clear_searchAddHistory() {
            var a_start_date = "<%=Convert.ToString(DateTime.Now.Year) + "-" + String.Format("{0:00}", DateTime.Now.Month-1) + "-" + String.Format("{0:00}", DateTime.Now.Day)%>";
            var a_end_date = $("#a_end_date").val();
            var a_customer_name = "";
            var a_paytype = 2;
            $("#a_search_input").val("");
            $("#a_search_type").val(2);
            $("#a_start_date").val(a_start_date);
            $("#a_end_date").val(a_end_date);
            $.ajax({
                url: rootUri + "Money/FilterAddHistory",
                data: {
                    start_date: a_start_date,
                    end_date: a_end_date,
                    customer_name: a_customer_name,
                    paytype: a_paytype
                },
                success: function (data) {
                    refreshAddHistoryTable();
                }
            });
        }
        function changeCustomerNameOrPhone(isPhone) {
            var search = "";
            if (isPhone) {
                search = $("#submit_form input[name='cPhone']").val();
                //if (search.length > 0)
                //     $("#submit_form input[name='cPhone']").parent().removeClass('has-success').removeClass('has-error');
            } else {
                search = $("#submit_form input[name='cName']").val();
                //if (search.length > 0)
                //    $("#submit_form input[name='cName']").parent().removeClass('has-success').removeClass('has-error');
            }
            //alert(search + " | " + isPhone);
            if (search != null && search.length > 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Sale/GetUsernameOrPhone",
                    dataType: "json",
                    data: {
                        "search": search,
                        "isphone": isPhone
                    },
                    success: function (data) {
                        if (data.result.length > 0) {
                            if (data.isphone == "true")
                                $("#submit_form input[name='cName']").val(data.result);
                            else
                                $("#submit_form input[name='cPhone']").val(data.result);
                        }
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }


        function printCreditPage() {
            $(".print_history_date").html($("#start_date").val() + "至" + $("#end_date").val());

            var tableRows = $("#tbllist_credit tbody tr");

            var htmlStr = "";
            for (var i = 0; i < tableRows.length; i++) {
                var row = $(tableRows[i]);
                htmlStr += "<tr>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(0).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(1).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(2).html() + "</td>" +
                    "</tr>";
            }

            $("#print_credithistory_body").html(htmlStr);

            pageprint_credithistory();
        }
        function printCreditDetail() {

            var tableRows = $("#DetailCreditTable tbody tr");

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
                    "</tr>";
            }

            $("#print_creditdetail_body").html(htmlStr);

            pageprint_creditdetail();
        }
        function printAddHistory() {
            $(".print_add_history_date").html($("#a_start_date").val() + "至" + $("#a_end_date").val());

            var tableRows = $("#addhistoryTable tbody tr");

            var htmlStr = "";
            for (var i = 0; i < tableRows.length; i++) {
                var row = $(tableRows[i]);
                htmlStr += "<tr>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(0).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(1).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(2).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(3).html() + "</td>" +
                    "<td style='text-align:center;'>" + row.find("td").eq(4).html() + "</td>" +
                    "</tr>";
            }

            $("#print_addhistory_body").html(htmlStr);

            pageprint_addhistory();
        }
    </script>
    <script type="text/javascript">
        var initBody;
        function beforePrint() {
        }
        function beforePrint_addhistory() {
        }
        function afterPrint_credithistory() {
            $("#header").css("display", "block");
            $("#tabs-2").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_credithistory").css("display", "none");
        }

        
        function pageprint_credithistory() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint_credithistory;
            $("#header").css("display", "none");
            $("#tabs-2").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_credithistory").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-2").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_credithistory").css("display", "none");
        }
        
        function afterPrint_creditdetail() {
            $("#header").css("display", "block");
            $("#tabs-2").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_credithistory").css("display", "none");
        }
        function pageprint_creditdetail() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint_creditdetail;
            $("#header").css("display", "none");
            $("#tabs-2").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_creditdetail").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-2").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_creditdetail").css("display", "none");
        }
        function afterPrint_addhistory() {
            $("#header").css("display", "block");
            $("#tabs-2").css("display", "block");
            $("#add_History").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_addhistory").css("display", "none");

        }
        function pageprint_addhistory() {
            window.onbeforeprint = beforePrint_addhistory;
            window.onafterprint = afterPrint_addhistory;
            $("#header").css("display", "none");
            $("#tabs-2").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#add_History").css("display", "none");
            $("#print_page_addhistory").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-2").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_addhistory").css("display", "none");
        }
    </script>
</body>
</html>
