<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>

<body>
    <div id="tabs-1">
        <div class="table-toolbar">
            <form action="#" id="submit_form_updateshop" class="form-horizontal form-validate">
                <input type="hidden" name="shop_id" id="shop_id" value="<%=((Shop)ViewData["shopInfo"]).id%>" />
                <table class="receive_table" cellspacing="20" border="0">
                    <tr>
                        <td valign="top">
                            <label class="control-label col-md-3" for="name">经销商名称:<span class="required">*</span></label>
                        </td>
                        <td style="vertical-align:top;" class="sub_table">
                            <table cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="vertical-align:top;">
                                        <div class="modal-body">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <input type="text" name="shopname" id="shopname" value="<%=((Shop)ViewData["shopInfo"]).name%>" style="width: 275px" class="form-control" readonly />
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="vertical-align:top;">
                                        <label class="control-label col-md-3" for="name">名称拼音简码:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align:top;">
                                        <div class="modal-body">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <input type="text" name="nickname" id="nickname" placeholder="请输入名称拼音简码" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).nickname%>" style="width: 189px" readonly/>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <label class="control-label col-md-3" for="name">地区:<span class="required">*</span></label>
                        </td>
                        <td style="vertical-align:top;" class="sub_table">
                            <table cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td style="vertical-align:top; width:300px">
                                        <div class="modal-body">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <select class="form-control adjust-left" name="cityregion" id="cityregion" style="width: 100px; float:left; margin-right:10px;" onchange="changecity();">
                                                        <% foreach (var item in (List<Region>)ViewData["cityList"]) { %>
                                                        <option value="<%= item.id %>" <% if (((long)ViewData["cityid"]) == item.id) { %> selected <% } %> ><%= item.name %></option>
                                                        <% } %>
                                                    </select>
                                                    <select class="form-control" name="districtregion" id="districtregion" style="width: 100px; margin-left:10px;">
                                                        <% foreach (var item in (List<Region>)ViewData["districtList"]) { %>
                                                        <option value="<%= item.id %>" <% if (((Shop)ViewData["shopInfo"]).region == item.id) { %> selected <% } %> ><%= item.name %></option>
                                                        <% } %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="vertical-align:top;">
                                        <label class="control-label col-md-3" for="name">地址:<span class="required">*</span></label>
                                    </td>
                                    <td style="vertical-align:top; align:right;">
                                        <div class="adjust-right" style="width:400px">
                                            <div class="form-group">
                                                <div class="col-md-3">
                                                    <input type="text" name="addr" id="addr" placeholder="请输入地址" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).addr%>" style="width: 260px" />
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <label class="control-label col-md-3" for="name">联系人:</label>
                        </td>
                        <td style="vertical-align:top;">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type="text" name="username" id="username" placeholder="请输入联系人" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).username%>" style="width: 300px" />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <label class="control-label col-md-3" for="name">手机号码:</label>
                        </td>
                        <td style="vertical-align:top;">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type="text" name="mobile_phone" id="mobile_phone" placeholder="请输入手机号码" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).mobile_phone%>" style="width: 300px"  onkeyup="this.value=this.value.replace(/[^\d]/,'')"/>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <label class="control-label col-md-3" for="name">座机号码:<span class="required">*</span></label>
                        </td>
                        <td style="vertical-align:top;">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type="text" name="phone" id="phone" placeholder="请输入座机号码" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).phone%>" style="width: 300px"  onkeyup="this.value=this.value.replace(/[^\d]/,'')"/>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <label class="control-label col-md-3" for="name">电子邮箱:<span class="required">*</span></label>
                        </td>
                        <td style="vertical-align:top;">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type="text" name="mailaddr" id="mailaddr" placeholder="请输入电子邮箱" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).mailaddr%>" style="width: 300px" />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <label class="control-label col-md-3" for="name">QQ号码:</label>
                        </td>
                        <td style="vertical-align:top;">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type="text" name="qqnum" id="qqnum" placeholder="请输入QQ号码" data-required="1" class="form-control" value="<%=((Shop)ViewData["shopInfo"]).qqnum%>" style="width: 300px" />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <button class="receive_table_button" type="button" onclick="Submit_UpdateShop()">保 存</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <button class="receive_table_button" type="button" onclick="window.location = rootUri + 'First/Index';">
                                    返 回
                                </button>
                        </td>
                    </tr>
                </table>
            </form>
        </div>

    </div>


    <script type="text/javascript">

        $(function () {

            var form_shop = $('#submit_form_updateshop');
            error = $('.alert-danger', form_shop);
            success = $('.alert-success', form_shop);

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

            form_shop.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    shopname: {
                        required: true,
                        minlength: 1
                    },
                    nickname: {
                        required: true,
                        minlength: 1,
                        maxlength: 10                        
                    },
                    addr: {
                        required: true
                    },
                   /* username: {
                        required: true
                    },*/
                    mobile_phone: {
                      //  required: true,
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


        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                // format 
                $(".has-success").removeClass("has-success");
                $(".has-error").removeClass("has-error");
            }
        }


        function Submit_UpdateShop() {
            if ($('#submit_form_updateshop').valid()) {
                $('.alert-danger').hide();

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "<%= ViewData["rootUri"] %>Main/SubmitMain",
                    dataType: "json",
                    data: $('#submit_form_updateshop').serialize(),
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


    </script>


</body>
</html>
