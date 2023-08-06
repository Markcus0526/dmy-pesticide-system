<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-3">
        <table class="table_subtab">
            <tr>
                <td class="search_title">供应商名称:
                </td>
                <td style="width: 55%">
                    <input type="text" name="search_supply" id="search_supply" placeholder="请输入供应商名称" style="width: 90%" class='form-control'></td>
                <td>
                    <button type="button" class="btn_rect" onclick="Filter_SupplyList()" style="float:right;">搜索</button></td>
                <td>
                    <button type="button" class="btn_rect" onclick="FilterSupplyText_Clear()" style="float:right;">条件清空</button>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <button type="button" class="btn_rect" onclick="Insert_SupplyDlg()" style="float:right;">添加</button>
                </td>
            </tr>
        </table>
        <button style="display: none;" type="button" id="init_supplydlg" data-target="#SupplyDlg" data-toggle="modal"></button>
        <div class="portlet-body" style="display: block;">

            <table class="table table-striped  table-advance table-bordered table-hover" id="tblsupply">
                <thead>
                    <tr>
                        <th style="width: 80px !important; text-align: center">编号</th>
                        <th style="width: 120px; text-align: center">供应商名称</th>
                        <th style="text-align: center">地区</th>
                        <th style="text-align: center">供应商地址</th>
                        <th style="text-align: center">联系人</th>
                        <th style="text-align: center">手机号码</th>
                        <th style="text-align: center">座机号码</th>
                        <th style="width: 80px !important; text-align: center">操作</th>
                    </tr>
                </thead>

                <tbody>
                </tbody>
                <%--<img src="<%= ViewData["rootUri"] %>Content/image/edit_img.png" data-target="#dialog1" data-toggle="modal" width=20px height=20px>
			                    <b> &nbsp</b>
			                    <img src="<%= ViewData["rootUri"] %>Content/image/delete_img.png" width=20px height=20px>--%>
            </table>

        </div>

        <div id="SupplyDlg" class="modal fade" tabindex="-1" data-width="800px">
            <form action="#" id="submit_form_supply" class="form-horizontal form-validate">
                <div class="modal-dialog" style="width: 800px">
                    <div class="modal-content">
                        <div class="modal-body">
                            <table class="receive_table" cellspacing="20" border="0">
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" for="name">供应商名称:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;" class="sub_table">
                                        <table cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="vertical-align: top;">
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <input type="text" name="supply_name" id="supply_name" value="" style="width: 150px;" class="form-control" onkeyup="changeSupplyName();"/>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <label class="control-label col-md-3" for="name">字母缩写:<span class="required">*</span></label>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <input type="text" name="supply_nickname" id="supply_nickname" value="" style="width: 150px;" class="form-control" />
                                                            <input type="text" name="supply_id" id="supply_id" value="" style="display: none" />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" for="name">地区:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="supply_region" id="supply_region" value="" style="width: 150px;" class="form-control" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" for="name">地址:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="supply_addr" id="supply_addr" value="" style="width: 150px;" class="form-control" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" for="name">联系人:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="supply_contactname" id="supply_contactname" value="" style="width: 150px;" class="form-control" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" for="name">手机号码:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;" class="sub_table">
                                        <table cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="vertical-align: top;">
                                                    <div class="form-group">
                                                        <div class="col-md-4">

                                                            <input type="text" name="supply_contactmobile" id="supply_contactmobile" value="" style="width: 150px;" class="form-control" onkeyup="this.value=this.value.replace(/[^\d]/,'')" />
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <label class="control-label col-md-3" for="name">座机号码:<span class="required">*</span></label>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <input type="text" name="supply_phone" id="supply_phone" value="" style="width: 150px;" class="form-control" onkeyup="this.value=this.value.replace(/[^\d]/,'')" />

                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-3" for="name">QQ账户:</label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="supply_qqnum" id="supply_qqnum" value="" style="width: 150px;" class="form-control" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button class="btn_rect" onclick="SubmitSupply()">保存</button>
                            <button type="button" data-dismiss="modal" id="cancelSupply" class="btn_rect">关闭</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>




    <script type="text/javascript">

        $(function () {

            var form_supply = $('#submit_form_supply');
            error = $('.alert-danger', form_supply);
            success = $('.alert-success', form_supply);

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

            function is_alphabetics(value) {
                return /^[a-zA-Z]+$/.test(value);
            }
            $.validator.addMethod("alphabetics", is_alphabetics, "请输入英文字");

            form_supply.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    supply_name: {
                        required: true
                    },
                    supply_nickname: {
                        required: true,
                        alphabetics: true
                    },
                    supply_addr: {
                        required: true
                    },
                    supply_region: {
                        required: true
                    },
                    supply_contactname: {
                        required: true
                    },
                    supply_contactmobile: {
                        number: true,
                        maxlength: 11,
                        required: true
                    },
                    supply_phone: {
                        number: true,
                        maxlength: 11,
                        required: true
                    },
                    supply_qqnum: {
                        digits: true
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

        function Modify_SupplyDlg(id) {
            $.ajax({
                url: rootUri + "Main/GetSelectedSupply",
                data: {
                    "id": id
                },
                success: function (data) {
                    var result = data.custom;
                    $(".has-error").removeClass("has-error");
                    $(".has-success").removeClass("has-success");
                    $(".help-block").remove();
                    $("#supply_id").attr('value', result.id);
                    $("#supply_name").attr('value', result.name);
                    $("#supply_nickname").attr("value", result.nickname);
                    $("#supply_region").attr("value", result.region);
                    $("#supply_addr").attr("value", result.addr);
                    $("#supply_contactname").attr("value", result.contact_name);
                    $("#supply_contactmobile").attr("value", result.contact_mobilephone);
                    $("#supply_phone").attr("value", result.contact_phone);
                    $("#supply_qqnum").attr("value", result.qqnum);
                    $("#init_supplydlg").click();
                }
            });
        }

        function Insert_SupplyDlg() {

            $(".has-error").removeClass("has-error");
            $(".has-success").removeClass("has-success");
            $(".help-block").remove();
            $("#supply_id").attr('value', "");
            $("#supply_name").attr('value', "");
            $("#supply_nickname").attr("value", "");
            $("#supply_addr").attr("value", "");
            $("#supply_contactname").attr("value", "");
            $("#supply_contactmobile").attr("value", "");
            $("#supply_phone").attr("value", "");
            $("#supply_qqnum").attr("value", "");

            $("#supply_region").attr("value", "<%=ViewData["RegionFirst"]%>");
            $("#init_supplydlg").click();

        }

        function Filter_SupplyList() {
            $.ajax({
                url: rootUri + "Main/FilterSupply",
                data: {
                    searchWord: $("#search_supply").val()
                },
                success: function (data) {
                    refreshSupplyTable();
                }
            });
        }

        function FilterSupplyText_Clear() {
            $("#search_supply").val("");
            Filter_SupplyList();
        }

        var supplyTable;
        var handleSupplyDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }

            // begin first table
            supplyTable = $('#tblsupply').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Main/RetrieveSupplyList",
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
                "aoColumnDefs": [
                        {
                            aTargets: [1],
                            fnRender: function (o, v) {
                                var s = "";
                                if (o.aData[1].length <= 6)
                                    return o.aData[1];
                                else
                                    return o.aData[1].substring(0, 6) + "...";
                            },
                            sClass: 'tableCell'
                        },
                        {
                            aTargets: [2],
                            fnRender: function (o, v) {
                                var s = "";
                                if (o.aData[2].length <= 4)
                                    return o.aData[2];
                                else
                                    return o.aData[2].substring(0, 4) + "...";
                            },
                            sClass: 'tableCell'
                        },
                        {
                            aTargets: [3],
                            fnRender: function (o, v) {
                                var s = "";
                                if (o.aData[3].length <= 9)
                                    return o.aData[3];
                                else
                                    return o.aData[3].substring(0, 9) + "...";
                            },
                            sClass: 'tableCell'
                        },
                        {
                            aTargets: [4],
                            fnRender: function (o, v) {
                                var s = "";
                                if (o.aData[4].length <= 6)
                                    return o.aData[4];
                                else
                                    return o.aData[4].substring(0, 6) + "...";
                            },
                            sClass: 'tableCell'
                        },
                        {
                            aTargets: [7],    // Column number which needs to be modified
                            fnRender: function (o, v) {   // o, v contains the object and value for the column
                                var rhtml = "<div align=\"center\" ><img src=\"../../Content/img/pen.png\" onclick=\"Modify_SupplyDlg(" + o.aData[7] + ")\" style=\"width:20px; height:20px; \" />&nbsp;&nbsp<img src=\"../../Content/img/thresh.png\" onclick=\"DeleteSupply(" + o.aData[7] + ")\" style=\"width:20px; height:20px;\"/></div>";
                                return rhtml;
                            },
                            sClass: 'tableCell'    // Optional - class to be applied to this table cell
                        }
                ]
            });
        }

        function refreshSupplyTable() {
            oSettings = supplyTable.fnSettings();
            supplyTable.fnDraw();
            //Retrieve the new data with $.getJSON. You could use it ajax too
            /*$.getJSON(oSettings.sAjaxSource, null, function (json) {
               // supplyTable.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    supplyTable.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                supplyTable.fnDraw();
            });*/
        }

        function DeleteSupply(id) {
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
                                url: rootUri + "Main/DeleteSupply",
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
                refreshSupplyTable();
            }
        }

        function SubmitSupply() {
            if ($('#submit_form_supply').valid()) {
                $("#cancelSupply").trigger("click");
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Main/SubmitEditSupply",
                    dataType: "json",
                    data: $('#submit_form_supply').serialize(),
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

        function changeSupplyName() {
            var shopname = $("#supply_name").val();
            $.ajax({
                url: "<%= ViewData["rootUri"] %>Account/GetPinyinCode",
                data: {
                    originStr: shopname
                },
                success: function (data) {
                    $("#supply_nickname").val(data.pinyincode);
                }
            });
        }

        jQuery(document).ready(function () {
            App.init();
            handleSupplyDataTable();
        });

    </script>
</body>
</html>
