<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-2">
        <table class="table_subtab">
            <tr>
                <td class="search_title">客户名称:</td>
                <td style="width: 55%">
                    <input type="text" id="search_customer" name="search_customer" placeholder="请输入客户名称" style="width: 90%" class='form-control'></td>
                <td>
                    <button type="submit" class="btn_rect" onclick="Filter_CustomerList()" style="float:right;">搜索</button></td>
                <td>
                    <button type="button" class="btn_rect" onclick="FilterCustomerText_Clear()" style="float:right;">条件清空</button></td>
            </tr>
        </table>
        <%--<div class ="modal-hidden" style="display:none">
                <a id="init_customerdlg" class="btn white" data-target="#CustomerDlg" data-toggle="modal"></a>
            </div>--%>
        <button style="display: none;" type="button" id="init_customerdlg" data-target="#CustomerDlg" data-toggle="modal"></button>
        <div id="portlet">
            <div class="portlet-body" style="display: block;">
                <div id="table-responsive">
                    <table class="table table-striped  table-advance table-bordered table-hover" id="tblcustomer">
                        <thead>
                            <tr>
                                <th style="width: 80px;text-align:center">编号</th>
                                <th style="text-align:center">客户名称</th>
                                <th style="text-align:center">手机号码</th>
                                <th style="width: 80px;text-align:center">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div id="CustomerDlg" class="modal fade" tabindex="-1" data-width="600px">
            <form action="#" id="submit_form_customer" class="form-horizontal form-validate">
                <div class="modal-dialog" style="width: 600px">
                    <div class="modal-content">
                        <div class="modal-body">
                            <table class="receive_table" cellspacing="20" border="0">
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" style="width: 120px;" for="name">客户名称 ：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">

                                            <div class="col-md-4">
                                                <input type="text" name="customer_name" id="customer_name" data-required="1" class="form-control"
                                                    style="width: 275px" value="" />
                                                <input type="text" name="customer_id" id="customer_id" value="" style="display: none" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" style="width: 120px;" for="name">手机号码 ：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">

                                            <div class="col-md-4">
                                                <input type="text" name="customer_phone" id="customer_phone" data-required="1" class="form-control"
                                                    style="width: 275px" value="" onkeyup="this.value=this.value.replace(/[^\d]/,'')"/>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </div>
                        <div class="modal-footer">
                            <button class="btn_rect" onclick="SubmitCustomer()">保存</button>
                            <button class="btn_rect" data-dismiss="modal" id="cancelCustomer" aria-hidden="true">取消</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>


    <script type="text/javascript">

        $(function () {

            var form_customer = $('#submit_form_customer');
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
                    customer_name: {
                        required: true
                    },
                    customer_phone: {
                        number: true,
                        maxlength: 11,
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


        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                // format 

                $(".has-success").removeClass("has-success");
                $("input").attr("value", "");
                $("#area").children().eq(0).attr("selected", "selected");
            }
        }


        function Modify_CustomerDlg(id) {
            $.ajax({
                url: rootUri + "Main/GetSelectedCustomer",
                data: {
                    "id": id
                },
                success: function (data) {
                    var result = data.custom;
                    $(".has-error").removeClass("has-error");
                    $(".has-success").removeClass("has-success");
                    $(".help-block").remove();

                    $("#customer_id").attr('value', result.id);
                    $("#customer_name").attr('value', result.name);
                    $("#customer_phone").attr("value", result.phone);
                    $("#init_customerdlg").click();
                }
            });
        }

        function Filter_CustomerList() {
            $.ajax({
                url: rootUri + "Main/FilterCustomer",
                data: {
                    searchWord: $("#search_customer").val()
                },
                success: function (data) {
                    refreshCustomerTable();
                }
            });
        }

        function FilterCustomerText_Clear() {
            $("#search_customer").val("");
            Filter_CustomerList();
        }

        var customerTable;
        var handleCustomerDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }

            // begin first table
            customerTable = $('#tblcustomer').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Main/RetrieveCustomerList",
                "aoColumns": [
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false }
                ],
                "aLengthMenu": [
                    [10, 20, 50, -1],
                    [10, 20, 50, "All"] // change per page values here
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
                "aoColumnDefs": [{
                    aTargets: [3],    // Column number which needs to be modified
                    fnRender: function (o, v) {   // o, v contains the object and value for the column
                        //var rhtml = '<a href="' + rootUri + 'AppPatch/Edit/' + o.aData[3] + '" class="btn default btn-xs default"><i class="fa fa-edit"></i> 编辑</a>&nbsp;' +
                        //  '<a href="javascript:void(0);" class="btn default btn-xs default" onclick="return deleteApp(' + o.aData[3] + ');"><i class="fa fa-trash-o"></i> 删除</a>';

                        var rhtml = "<div align=\"center\"> <img src=\"../../Content/img/pen.png\" onclick=\"Modify_CustomerDlg(" + o.aData[3] + ")\" style=\"width:20px; height:20px; \" />&nbsp;&nbsp<img src=\"../../Content/img/thresh.png\" onclick=\"DeleteCustomer(" + o.aData[3] + ")\" style=\"width:20px; height:20px;\"/> </div>";
                        return rhtml;
                    },
                    sClass: 'tableCell'    // Optional - class to be applied to this table cell
                }
                ]
            });
        }

        function refreshCustomerTable() {
            oSettings = customerTable.fnSettings();
            customerTable.fnDraw();
            //Retrieve the new data with $.getJSON. You could use it ajax too
            /*$.getJSON(oSettings.sAjaxSource, null, function (json) {
                //customerTable.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    customerTable.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                customerTable.fnDraw();
            });*/
        }

        function DeleteCustomer(id) {
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
                                url: rootUri + "Main/DeleteCustomer",
                                data: {
                                    "id": id
                                },
                                type: "POST",
                                success: function (message) {
                                    if (message == true) {
                                        flag = false;
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

                                    }
                                }
                            });
                        }
                    }
                }
            });
        }

        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                refreshCustomerTable();
            }
        }

        function SubmitCustomer() {
            if ($('#submit_form_customer').valid()) {
                $("#cancelCustomer").trigger("click");
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Main/SubmitEditCustomer",
                    dataType: "json",
                    data: $('#submit_form_customer').serialize(),
                    success: function (data) {
                        if (data == "") {
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
                            toastr["success"]("操作成功!", "恭喜您");
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

        jQuery(document).ready(function () {
            // initiate layout and plugins
            App.init();
            handleCustomerDataTable();
        });

    </script>

</body>
</html>
