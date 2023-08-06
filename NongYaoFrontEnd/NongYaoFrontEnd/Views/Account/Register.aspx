<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>

<!DOCTYPE html>
<html>
<head>
    <title>农药溯源管理云平台</title>
    <link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/style.css" type="text/css" />
    <link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/style1.css" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/plugins.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-switch/static/stylesheets/bootstrap-switch-metro.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=B9c5b113ec6dcbccb4540870728af498"></script>
</head>

<body>

    <table align="center">
        <tr>
            <td align="center" width="700px">

                <div class="row" style="padding-top: 50px;">
                    <div class="col-md-12">
                        <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                        <h3 class="page-title">经销商注册
                        </h3>
                        <hr />
                    </div>
                </div>

                <div class="portlet-body form">
                    <form action="#" id="form_agent" class="form-horizontal form-validate">
                        <div class="form-body">

                            <div class="alert alert-danger" style="display: none;">
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
                            </div>


                            <table class="receive_table" cellspacing="20" border="0">
                                <tr>
                                    <td colspan="2" align="center">经销商账号信息
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title" style="width:200px">用户名：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <table>
                                                    <tr>
                                                        <td style="padding-left: 0px; vertical-align: top">
                                                            <input type="text" name="userid" id="userid" data-required="1" maxlength="20" style="width: 210px;" class="form-control" value="" />
                                                        </td>
                                                        <td style="padding-left: 6px; vertical-align: top">
                                                            <button type="button" class="btn_action" onclick="doUniqueCheck();">唯一性检测</button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">密码：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="password" class="form-control" name="password" id="submit_form_password" style="width: 350px;" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">确认密码：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="password" class="form-control" name="rpassword" id="rpassword" style="width: 350px;" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">姓名：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="manager_name" id="manager_name" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">经销商详细信息
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">农药经营许可证号：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="permitid" id="permitid" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">经销商名称：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="shopname" id="shopname" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" onkeyup="changeShopName();" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">名称拼音简码：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" id="nickname" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" disabled />
                                                <input type="hidden" name="nickname" id="realnickname" />
                                            </div>
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title" style="margin-top: -5px">地区选择：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <table>
                                            <tr>
                                                <td style="padding-left:0;">
                                        <div class="form-group">
                                            <div class="col-md-4">

                                                <select class="form-control adjust-left" name="cityregion" id="cityregion" style="width: 150px; float:left; margin-top:5px; margin-bottom:5px; margin-right:10px" onchange="changecity();">
                                                    <% foreach (var item in (List<Region>)ViewData["cityList"]) { %>
                                                    <option value="<%= item.id %>"><%= item.name %></option>
                                                    <% } %>
                                                </select>
                                            </div>
                                        </div>        
                                                </td>
                                                <td style="padding-left:0;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <select class="form-control" name="districtregion" id="districtregion" style="width: 150px;  margin-top:5px; margin-left:10px;">
                                                    <% foreach (var item in (List<Region>)ViewData["districtList"]) { %>
                                                    <option value="<%= item.id %>"><%= item.name %></option>
                                                    <% } %>
                                                </select>

                                            </div>
                                        </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">地址：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="addr" id="addr" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">联系人：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="username" id="username" data-required="0" maxlength="25" style="width: 350px;" class="form-control" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">手机：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4" width="400px;">
                                                <input type="text" name="mobile_phone" id="mobile_phone" data-required="0" maxlength="25" style="width: 350px;" class="form-control" value="" onkeyup="this.value=this.value.replace(/[^\d]/,'')" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">座机号码：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="phone" id="phone" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" onkeyup="this.value=this.value.replace(/[^\d]/,'')" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">电子邮箱：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-5">
                                                <input type="text" name="mailaddr" id="mailaddr" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">QQ号码：</label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <input type="text" name="qqnum" id="qqnum" data-required="1" maxlength="25" style="width: 350px;" class="form-control" value="" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="vertical-align: top;">
                                        <label class="control-label col-md-5 title">是否高毒：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4" style="margin-top: 5px;">
                                                <!-- <input class="input_radio" type="radio" name="level" value="1" checked>&nbsp;是&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class="input_radio" type="radio" name="level" value="0">&nbsp;否-->
                                                <div id="mark_div" class="make-switch" data-on-label="&nbsp;是&nbsp;" data-off-label="&nbsp;否&nbsp;" style="width: 110px;">
                                                    <input type="checkbox" name="level" id="level" class="toggle" />
                                                </div>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: middle;">
                                        <label class="control-label col-md-5 title">定位我的店铺：<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div class="form-group">
                                            <div class="col-md-4" id="allmap" style="height:250px; width:500px; overflow: hidden; margin: 0;">                                                                                                
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr >
                                    <td>&nbsp;</td>
                                    <td>
                                        <div class="form-group" style="float:left; width:180px">
                                            经度：<input type="text" name="lon" id="lon" style="width:100px;" class="form-control" readonly/>&nbsp;&nbsp;
                                        </div>
                                        <div class="form-group" style="float:left; width:180px">
                                            纬度：<input type="text" name="lat" id="lat" style="width:100px;" class="form-control" readonly/>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="2" align="center">

                                        <button type="button" onclick="doSubmit();" data-loading-text="提交中..." class="btn_action1">
                                            注册
                                        </button>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <button type="button" class="btn_action1" onclick="window.history.go(-1);">返回</button>
                                    </td>


                                </tr>
                            </table>
                    </form>
                </div>
            </td>
        </tr>
    </table>

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
            var form1 = $('#form_agent');
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

            function is_alphabetics(value) {
                return /^[a-zA-Z]+$/.test(value);
            }
            $.validator.addMethod("alphabetics", is_alphabetics, "请输入英文字");

            function is_alphanumber(value) {
                return /^[a-zA-Z0-9]+$/.test(value);
            }
            $.validator.addMethod("alphanumber", is_alphanumber, "请输入英文字,数字");

            function is_permitid(value) {
                return /^[a-zA-Z0-9-]+$/.test(value);
            }
            $.validator.addMethod("validpermitid", is_permitid, "请输入英文字,数字,‘-’符号");

            function is_lonlat(value) {
                if (value.length == 0)
                    return false;
                else {
                    if (value == "")
                        return false;
                }

                return true;
            }
            $.validator.addMethod("lonlat", is_lonlat, "请选择店铺坐标");


            form1.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    permitid: {
                        required: true,
                        validpermitid: true
                    },
                    shopname: {
                        required: true
                    },
                    addr: {
                        required: true
                    },
                    username: {
                        required: true
                    },
                    mobile_phone: {
                        required: true,
                        digits: true,
                        maxlength: 11
                    },
                    phone: {
                        required: true,
                        digits: true,
                        maxlength: 11
                    },
                    mailaddr: {
                        required: true,
                        email: true
                    },
                    qqnum: {
                        digits: true
                    },
                    manager_name: {
                        required: true
                    },
                    userid: {
                        required: true
                    },
                    password: {
                        required: true,
                        minlength: 1
                    },
                    rpassword: {
                        required: true,
                        minlength: 1,
                        equalTo: "#submit_form_password"
                    },
                    cityregion: {
                        required: true
                    },
                    districtregion: {
                        required: true
                    },
                    lon: {
                        lonlat: true
                    },
                    lat: {
                        lonlat: true
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

        function changecity() {
            var html = "";
            
            var cityid = $("#cityregion").val();
            $.ajax({
                url: "<%= ViewData["rootUri"] %>Account/GetDistrictList",
                data: {
                cityid: cityid
                },
                success: function (data) {
                    var districtdata = data.districtdata;
                    for (var i = 0; i < districtdata.length; i++) {
                        html += "<option value=\"" + districtdata[i].id + " \">" + districtdata[i].name + "</option>";
                        $("#districtregion").html(html);
                    }
                    if (districtdata.length == 0) {
                        $("#districtregion").html("");
                    }
                }
            });
        }

        function changeShopName() {
            var shopname = $("#shopname").val();
            $.ajax({
                url: "<%= ViewData["rootUri"] %>Account/GetPinyinCode",
                data: {
                    originStr: shopname
                },
                success: function (data) {
                    $("#nickname").val(data.pinyincode);
                }
            });
        }
        var whichSuccess = 0;

        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
                $('.alert-success').hide();
                $('.loading-btn').button('reset');
            } else {
                if (whichSuccess == 0) {
                }
                else
                    window.history.go(-1);
            }
        }

        function doUniqueCheck() {
            if ($("#userid").val() == "") {
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
                toastr["error"](0, "请输入正确的");

                return;
            }
            $.ajax({
                async: false,
                type: "POST",
                url: "<%= ViewData["rootUri"] %>Account/UserIdUniqueCheck",
                dataType: "json",
                data: {
                    userid: $("#userid").val()
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
                        toastr["success"]("你可以使用这个用户名称");
                        whichSuccess = 0;
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
                        toastr["error"](data.error, "使用别的用户名称");

                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }

        function doSubmit() {
            if ($('#form_agent').valid()) {
                $('.alert-danger').hide();

                $("#realnickname").val($("#nickname").val());

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "<%= ViewData["rootUri"] %>Account/RegisterShop",
                    dataType: "json",
                    data: $('#form_agent').serialize(),
                    success: function (data) {
                        if (data == false) {
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
                            toastr["error"](data, "温馨敬告");
                        }
                        else {
                            if (data == true || data == "") {
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
                            whichSuccess = 1;
                        } 
                        }
                    },
                    error: function (data) {
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }


    </script>

</body>
</html>
<script type="text/javascript">

    var map = new BMap.Map("allmap");
    var point = new BMap.Point(<%= ViewData["fixlng"].ToString() %>, <%= ViewData["fixlat"].ToString() %>);
    map.centerAndZoom(point, 15);
    map.enableScrollWheelZoom();
    map.addEventListener("click", function (e) {
        $("#lon").attr("value", e.point.lng);
        $("#lat").attr("value", e.point.lat);
        map.clearOverlays();
        map.addOverlay(new BMap.Marker(e.point));        
    });

</script>