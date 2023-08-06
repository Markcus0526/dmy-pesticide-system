<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div id="chg_pwd_div">
        <!--<form id="chg_pwd_form" action="#" class="form-horizontal form-validate">
	<table class="chg_pwd_table">
        
	<tr><div class="form-group"><th>原密码 : </th><td width=210px><input class="chg_pwd_input" type="password" name="old_pass" id="old_pass" value=""></td><div class="form-group"></tr>
	<tr><th>密码 : </th><td><div class="form-group"><input class="chg_pwd_input" type="password" name="new_pass" value=""></div></td></tr>
	<tr><th>确认密码 : </th><td><div class="form-group"><input class="chg_pwd_input" type="password" name="retype_pass" id="retype_pass" value=""></td></div></tr>
	<tr><td colspan=2 align="center"><button type="submit">保存</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button type="reset">取消</button></td></tr>
	</table>
	</form>-->
        <form id="chg_pwd_form" action="#" class="form-horizontal form-validate" style="margin-left: -100px">
            <!--   <div class="alert alert-danger" style="display: none;">
                                <button class="close" data-close="alert"></button>
                                <table>
                                    <tr>
                                        <td width="100%">您还未完成填写信息，请确认下面的内容。</td>
                                    </tr>
                                </table>
                            </div>

                            <div class="alert alert-success" style="display: none;">
                                <button class="close" data-dismiss="alert"></button>
                                <table>
                                    <tr>
                                        <td width="100%">提交中，请稍等...</td>
                                    </tr>
                                </table>
                            </div>-->
            <table class="receive_table" cellspacing="20" border="0" style="margin-left:50px; margin-top:30px;">
                <tr>
                     <td>
                            <label class="control-label col-md-5 title">旧密码 :<span class="required">*</span></label>
                        </td>
                        <td>
                            <div class="form-group">
                            <div class="col-md-4">
                                <input class="form-control" type="password" autocomplete="off" placeholder="旧密码" name="old_pass" id="old_pass" style="width: 320px;" value="<%=ViewData["password"] %>"/>
                            </div>
                                </div>
                        </td>
                    </tr>
                <tr>
                   <td width="30%">
                            <label class="control-label col-md-5 title">密码 :<span class="required">*</span> </label>
                            </td>
                        <td>
                            <div class="form-group">
                            <div class="col-md-4">
                                <input class="form-control" type="password" autocomplete="off" placeholder="" name="new_pass" id="new_pass" style="width: 320px;" />
                            </div>
                                </div>
                        </td>
                    </tr>
                <tr>
                       <td width="30%">
                            <label class="control-label col-md-5 title">确认密码 :<span class="required">*</span> </label>
                            </td>
                        <td>
                            <div class="form-group">
                            <div class="col-md-4">
                                <input class="form-control" type="password" autocomplete="off" placeholder="" name="retype_pass" id="Password1" style="width: 320px;" />
                            </div>
                                </div>
                        </td>
                    </tr>
            </table>
           <!-- <div style="margin-top: 40px;">
                <table>
                    <tr>
                       
                    </tr>
                </table>
            </div>-->
          <!--  <div>
                <table>
                    <tr>
                        <td width="30%">
                            <label class="control-label col-md-5">密码 : </label>
                            </td>
                        <td>
                            <div class="form-group">
                            <div class="col-md-4">
                                <input class="form-control" type="password" autocomplete="off" placeholder="密码" name="new_pass" id="new_pass" style="width: 320px;" />
                            </div>
                                </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr>
                        <td width="30%">
                            <label class="control-label col-md-5">确认密码 : </label>
                            </td>
                        <td>
                            <div class="form-group">
                            <div class="col-md-4">
                                <input class="form-control" type="password" autocomplete="off" placeholder="确认密码" name="retype_pass" id="retype_pass" style="width: 320px;" />
                            </div>
                                </div>
                        </td>
                    </tr>
                </table>
            </div>-->
            <div class="form-group" style="margin-top: 30px; margin-left: 100px;">

                <button type="button" onclick="doSubmit();" data-loading-text="提交中..." class="loading-btn btn btn-primary" style="margin-right: 20px;">
                    保存
                </button>
                <button type="button" class="btn" onclick="window.history.go(-1);">返回</button>

            </div>

        </form>

    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">
    <link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/style1.css" type="text/css" />
    <link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/style.css" type="text/css" />
    <link type="text/css" href="<%= ViewData["rootUri"] %>Content/css/themes/base/jquery.ui.all.css" rel="stylesheet" />

    <link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style-responsive.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/plugins.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/pages/login-soft.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.widget.js"></script>

    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/select2/select2.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootbox/bootbox.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-switch/static/js/bootstrap-switch.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.tabs.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js"></script>
    <script type="text/javascript">

        jQuery(document).ready(function () {
            //App.init();

            // initiate layout and plugins
            var form1 = $('#chg_pwd_form');
            error = $('.alert-danger', form1);
            success = $('.alert-success', form1);

            $.validator.messages.required = "必须要填写";
            $.validator.messages.remote = "请输入正确的"
            $.validator.messages.email = "请输入Email格式"
            $.validator.messages.password = "确认密码不同"
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


            form1.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    old_pass: {
                        required: true,
                        minlength: 1
                    },
                    new_pass: {
                        required: true,
                        minlength: 1
                    },
                    retype_pass: {
                        required: true,
                        minlength: 1,
                        equalTo: "#new_pass"
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

        function doSubmit() {
            if ($('#chg_pwd_form').valid()) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "<%= ViewData["rootUri"] %>Password/ChangePassword",
                    dataType: "json",
                    data: $('#chg_pwd_form').serialize(),
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
                            toastr["success"]("操作成功!", "恭喜您");
                            window.location.href = "<%= ViewData["rootUri"] %>Account/LogOff";
                            
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
                        $('.loading-btn').button('reset');
                    }
                                });
            }
        }
    </script>
</asp:Content>
