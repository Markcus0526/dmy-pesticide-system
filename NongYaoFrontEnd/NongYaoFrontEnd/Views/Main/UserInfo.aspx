<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-4">
            <div class="search_frm">
                <form>
                    <button type="button" class="btn_actionex" onclick="Insert_UserDlg()">添加</button>
                </form>
            </div>
            <div class="portlet-body" style="display: block;">
                <table class="table table-striped  table-advance table-bordered table-hover" id="tbluser">
                    <thead>
                        <tr>
                            <th style="width:80px;text-align:center">编号</th>
                            <th style="text-align:center">用户姓名</th>
                            <th style="text-align:center">用户名称</th>
                            <th style="text-align:center">手机号码</th>
                            <th style="text-align:center">用户权限</th>
                            <th style="width:80px;text-align:center">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <button style="display: none;" type="button" id="init_userdlg" data-target="#UserDlg" data-toggle="modal"></button>
            <div id="UserDlg" class="modal fade" tabindex="-1" data-width="600px">
                <form action="#" id="submit_form_user" class="form-horizontal form-validate">
                    <div class="modal-dialog" style="width:600px;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <table class="receive_table" cellspacing="20" border="0">
                                    <tr>
                                        <td style="vertical-align:top">
                                            <label class="control-label col-md-3" for="name">用户姓名:<span class="required">*</span></label>
                                        </td>
                                        <td style="vertical-align:top">
                                            <div class="form-group">                                    
                                    <div class="col-md-4">
                                        <input type="text" name="user_name" id="user_name" value="" style="width: 200px;" class="form-control"/>
                                            <input type="text" name="uid" id="uid" value="" style="display: none" />
                                    </div>
                                </div>
                                        </td>
                                        </tr>
                                    <tr>
                                        <td style="vertical-align:top">
                                            <label class="control-label col-md-3" for="name">用户名称:<span class="required">*</span></label>
                                        </td>
                                        <td style="vertical-align:top">
                                            <div class="form-group">                                    
                                    <div class="col-md-4">
                                        <input type="text" name="user_id" id="user_id" value="" style="width: 200px;" class="form-control" />
                                    </div>
                                </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top">
                                            <label class="control-label col-md-3" for="name">用户密码:<span class="required">*</span></label>
                                        </td>
                                        <td style="vertical-align:top">
                                            <div class="form-group">
                                    
                                    <div class="col-md-4">
                                        <input type="password" name="user_password" id="user_password" value="" style="width: 200px;" class="form-control"/>
                                    </div>
                                </div>
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td style="vertical-align:top">
                                            <label class="control-label col-md-3" for="name">确认密码:<span class="required">*</span></label>
                                        </td>
                                        <td style="vertical-align:top">
                                            <div class="form-group">                                    
                                                <div class="col-md-4">
                                                    <input type="password" name="user_confirm" id="user_confirm" value="" style="width: 200px;" class="form-control"/>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>           
                                    <tr>
                                        <td style="vertical-align:top">
                                            <label class="control-label col-md-3" for="name">手机号码:<span class="required">*</span></label>
                                        </td>
                                        <td style="vertical-align:top">
                                            <div class="form-group">                                    
                                                <div class="col-md-4">
                                                    <input type="text" name="user_phone" id="user_phone" value="" style="width: 200px;" class="form-control" onkeyup="this.value=this.value.replace(/[^\d]/,'')"/>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="vertical-align:top">
                                            <label class="control-label col-md-3" for="name">权限:<span class="required">*</span></label>
                                        </td>
                                        <td style="vertical-align:top">
                                            <div class="form-group">
                                    
                                    <div class="col-md-4">
                                        <div id="user_role" style="width: 200px; background-color: white; border: solid; border-width: 1px; text-align: center;">
                                                <input type="checkbox" name="buying" id="buying" style="width:30px" />&nbsp;进货<br />
                                                <input type="checkbox" name="sale" id="sale" style="width:30px" />&nbsp;销售<br />
                                                <input type="checkbox" name="store" id="store" style="width:30px" />&nbsp;仓库<br />
                                                <input type="checkbox" name="account" id="account"  style="width:30px" checked/>&nbsp;财务<br />
                                            </div>
                                    </div>
                                </div>
                                        </td>
                                    </tr> 
                                    </table>    
                            <div class="modal-footer">
                                <button type="button" onclick="SubmitUser()" class="btn_rect">保存</button>
                                <button type="button" data-dismiss="modal" id="cancelUser" class="btn_rect">关闭</button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>

    
    <script type="text/javascript">

        $(function () {

            var form_user = $('#submit_form_user');
            error = $('.alert-danger', form_user);
            success = $('.alert-success', form_user);

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


            form_user.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    user_name: {
                        required: true
                    },
                    user_id: {
                        required: true
                    },
                    user_password: {
                        required: true
                    },
                    user_confirm:{
                        required: true,
                        equalTo: "#user_password"
                    },
                    user_phone: {
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

        function Modify_UserDlg(id) {
            $.ajax({
                url: rootUri + "Main/GetSelectedUser",
                data: {
                    "id": id
                },
                success: function (data) {
                    var result = data.custom;
                    $(".has-error").removeClass("has-error");
                    $(".has-success").removeClass("has-success");
                    $(".help-block").remove();

                    $("#uid").attr('value', result.id);
                    $("#user_name").attr('value', result.name);
                    $("#user_id").attr("value", result.userid);
                    $("#user_password").attr("value", result.password);
                    $("#user_confirm").attr("value", result.password);
                    $("#user_phone").attr("value", result.phone);
                    $("#buying").attr("checked", null);
                    $("#sale").attr("checked", null);
                    $("#store").attr("checked", null);
                    $("#account").attr("checked", null);

                    var role = result.roleSet;
                    for (var i = 0; i < role.length; i++)
                        $("#" + role[i]).attr("checked", 1);

                    $("#init_userdlg").click();
                }
            });
        }

        function Insert_UserDlg(id) {
            $(".has-error").removeClass("has-error");
            $(".has-success").removeClass("has-success");
            $(".help-block").remove();

            $("#uid").attr('value', "");
            $("#user_name").attr('value', "");
            $("#user_id").attr("value", "");
            $("#user_password").attr("value", "");
            $("#user_confirm").attr("value", "");
            $("#user_phone").attr("value", "");
            $("#buying").attr("checked", null);
            $("#sale").attr("checked", null);
            $("#store").attr("checked", null);
            $("#account").attr("checked", 1);
            $("#init_userdlg").click();

        }
        var userTable;
        var handleUserDataTable = function () {
            if (!jQuery().dataTable) {
                return;
            }

            // begin first table
            userTable = $('#tbluser').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Main/RetrieveUserList",
                "aoColumns": [
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
                                if (o.aData[1].length <= 10)
                                    return o.aData[1];
                                else
                                    return o.aData[1].substring(0, 10) + "...";
                            },
                            sClass: 'tableCell'
                        },
                        {
                            aTargets: [2],
                            fnRender: function (o, v) {
                                var s = "";
                                if (o.aData[2].length <= 10)
                                    return o.aData[2];
                                else
                                    return o.aData[2].substring(0, 10) + "...";
                            },
                            sClass: 'tableCell'
                        },
                    {
                    aTargets: [5],    // Column number which needs to be modified
                    fnRender: function (o, v) {   // o, v contains the object and value for the column
                        var rhtml = "<div align=\"center\"> <img src=\"../../Content/img/pen.png\" onclick=\"Modify_UserDlg(" + o.aData[5] + ")\" style=\"width:20px; height:20px; \" />&nbsp;&nbsp<img src=\"../../Content/img/thresh.png\" onclick=\"DeleteUser(" + o.aData[5] + ")\" style=\"width:20px; height:20px;\"/></div>";
                        return rhtml;
                    },
                    
                    sClass: 'tableCell'    // Optional - class to be applied to this table cell
                }
                ]
            });
        }

        function refreshUserTable() {
            oSettings = userTable.fnSettings();
            userTable.fnDraw();
            //Retrieve the new data with $.getJSON. You could use it ajax too
            /*$.getJSON(oSettings.sAjaxSource, null, function (json) {
               // userTable.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    userTable.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                userTable.fnDraw();
            });*/
        }

        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                refreshUserTable();
            }
        }

        function DeleteUser(id) {
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
                                url: rootUri + "Main/DeleteUser",
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

        function SubmitUser() {
            if ($('#submit_form_user').valid()) {
                $("#cancelUser").trigger("click");
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Main/SubmitEditUser",
                    dataType: "json",
                    data: $('#submit_form_user').serialize(),
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
            handleUserDataTable();
        });
    </script>
</body>
</html>
