<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />
</head>
<%--<tr>
    <th>编号</th>
    <th>仓库名称</th>
    <th>农药名称</th>
    <th>数量</th>
    <th>消耗理由</th>
    <th>操作</th>
</tr>--%>
<body>
    <div id="tabs-3">
        <form action="#" id="spend_table_frm" class="custom-help form-validate">
            <div style="height: 1px;">
                <button type="button" class="btn_actionex" onclick="make_spending_table(1)">添加</button>
            </div>
            <table class="table_view" border="0" cellspacing="1">
                <thead>
                    <tr style="height: 60px;">
                        <th style="text-align: center; width: 30px">编号</th>
                        <th style="text-align: center; width: 130px">仓库名称</th>
                        <th style="text-align: center; width: 130px">产品名称</th>
                        <th style="text-align: center; width: 130px">产品规格</th>
                        <th style="text-align: center; width: 130px">产品批号</th>
                        <th style="text-align: center; width: 50px">数量</th>
                        <th style="text-align: center; width: 130px">损耗理由</th>
                        <th style="text-align: center; width: 50px">操作</th>
                    </tr>
                </thead>
                <tbody id="mspdbody" style="background: white;">
                </tbody>
            </table>
            <button type="submit" class="btn_submit" onclick="submitform();">保存</button>
        </form>
    </div>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/select2/select2.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootbox/bootbox.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.tabs.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/localization/messages_zh.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js"></script>
    <script>
        var cur_count = 0, next_num = 0;
        jQuery(document).ready(function () {
            handleValidate();
            make_spending_table(0);
            changeStore(1);
        });

        function make_spending_table(flag) {
            var body = "";
            if (flag == 0) {
                cur_count = 0;
                next_num = 0;
            }
            cur_count++; next_num++;

            body += "<tr id='spend_tr_" + cur_count + "'>" + "<td style=\"width:60px\">" + next_num + "</td>";

            body += "<td><div class=\"form-group\"><select id=\"start_store\" name=\"start_store\" class='form-control'  onchange='changeStore(" + cur_count + ")'>";
            <% 
            int i;
            List<tbl_store> stores = (List<tbl_store>)ViewData["stores"];
            for (i = 0; i < stores.Count(); i++)
            {
                tbl_store storeitem = stores.ElementAt(i);
            %>
            body += "<option value='<% =storeitem.id%>'><% =storeitem.name%></option>";
            <%
            }
            %>
            body += "</select></div></td>";

            body += "<td><div class=\"form-group\"><select name=\"catalog_id\" class='form-control' onchange='changeCatalog(" + cur_count + ")'>";
            <% 
            List<tbl_catalog> catalogs = (List<tbl_catalog>)ViewData["catalogs"];
            if (catalogs != null) {
            for (i = 0; i < catalogs.Count(); i++)
            {
                tbl_catalog catalogitem = catalogs.ElementAt(i);
		    %>
            body += "<option value='<% =catalogitem.id%>'><% =catalogitem.name%></option>";
			<%
            }
            }
		    %>
            body += "</select></div></td>";

            body += "<td><div class=\"form-group\"><select name=\"standard_id\" class='form-control' onchange='changeStandard(" + cur_count + ")'>";
            <% 
            List<StandardInfo> standards = (List<StandardInfo>)ViewData["standards0"];
            if (standards != null)
            {
                for (i = 0; i < standards.Count(); i++)
                {
                    StandardInfo standarditem = standards.ElementAt(i);
                    %>
            body += "<option value='<% =standarditem.id%>'><% =standarditem.standard%></option>";
                    <%
                }
            }
            %>
            body += "</select></div></td>";

            body += "<td><div class=\"form-group\"><select name=\"largenumber\" class='form-control'>";
            <% 
            List<string> largenumbers = (List<string>)ViewData["largenumbers0"];
            if (largenumbers != null)
            {
                for (i = 0; i < largenumbers.Count(); i++)
                {
                    string largenumber = largenumbers.ElementAt(i);
            %>
            body += "<option value='<% =largenumber%>'><% =largenumber%></option>";
            <%
                }
            }
            %>
            body += "</select></div></td>";

            body += "<td><div class=\"form-group\"><input name=\"count\" class='form-control' value=\"\" style=\"width: 100%\"  /></div></td>" +
                    "<td><div class=\"form-group\"><input name=\"spendreason\" class='form-control' value=\"\" style=\"width: 100%\"  /></div></td>" +
                    "<td style=\"width:50px\" align=\"center\"> <img src=\"<%= ViewData["rootUri"]%>Content/image/delete_img.png\" width=20px height=20px onclick='remove_move_table(" + cur_count + ");'/>&nbsp;</td></tr>";
            if (flag == 1)
                $("#mspdbody").append(body);
            else
                $("#mspdbody").html(body);
            changeStore(cur_count);
        }

        var catalogs;
        function changeStore(num) {
            var store_id = $("#spend_tr_" + num).find("select").eq(0).val();

            if (store_id == null)
                return;

            $.ajax({
                async: false,
                type: "POST",
                url: rootUri + "Supply/GetCatalogsInStore_1",
                dataType: "json",
                data: {
                    "store": store_id,
                    "num": num
                },
                success: function (data) {
                    var catalogs = [];
                    catalogs = catalogs.concat(data.data);
                    var catalogselect = $("#spend_table_frm #spend_tr_" + data.num + " select[name='catalog_id']")
                    var options = "";
                    for (var i = 0; i < catalogs.length; i++) {
                        options += "<option value='" + catalogs[i].id + "'>" + catalogs[i].name + "</option>";
                    }
                    catalogselect.html(options);

                    changeCatalog(data.num);
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }

        function changeCatalog(num) {
            var store_id = $("#spend_tr_" + num).find("select").eq(0).val();
            var catalog_id = $("#spend_tr_" + num).find("select").eq(1).val();

            if (store_id == null || catalog_id == null) {
                $("#spend_tr_" + num).find("td").eq(3).find("select").html("");
                $("#spend_tr_" + num).find("td").eq(4).find("select").html("");

                return;
            }

            $.ajax({
                async: false,
                type: "GET",
                url: rootUri + "Sale/GetCatalogInfoAndStandardList",
                dataType: "json",
                data: {
                    "store_id": store_id,
                    "catalog_id": catalog_id,
                    "num": num
                },
                success: function (data) {
                    if (data != null && data != "") {
                        var info = data.info
                        var parenttr = $("#spend_table_frm #spend_tr_" + data.num);

                        var standards = [];
                        standards = standards.concat(data.data);
                        var standardselect = $("#spend_table_frm #spend_tr_" + data.num + " select[name='standard_id']")
                        var options = "";
                        for (var i = 0; i < standards.length; i++) {
                            options += "<option value='" + standards[i].id + "'>" + standards[i].standard + "</option>";
                        }
                        standardselect.html(options);

                        changeStandard(num);
                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }

        function changeStandard(num) {
            var store_id = $("#spend_tr_" + num).find("select").eq(0).val();
            var catalog_id = $("#spend_tr_" + num).find("select").eq(1).val();
            var standard_id = $("#spend_tr_" + num).find("select").eq(2).val();

            if (store_id == null || catalog_id == null || standard_id == null) {
                $("#spend_tr_" + num).find("td").eq(4).find("select").html("");

                return;
            }

            $.ajax({
                async: false,
                type: "GET",
                url: rootUri + "Sale/GetLargenumberList",
                dataType: "json",
                data: {
                    "store_id": store_id,
                    "catalog_id": catalog_id,
                    "standard_id": standard_id,
                    "num": num
                },
                success: function (data) {
                    if (data != null && data != "") {
                        var largenumbers = [];
                        largenumbers = largenumbers.concat(data.data);
                        var largenumberselect = $("#spend_table_frm #spend_tr_" + data.num + " select[name='largenumber']")
                        var options = "";
                        for (var i = 0; i < largenumbers.length; i++) {
                            options += "<option value='" + largenumbers[i] + "'>" + largenumbers[i] + "</option>";
                        }
                        largenumberselect.html(options);
                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }

        function remove_move_table(id) {
            if (next_num > 1) {
                $("#spend_tr_" + id).remove();
                next_num--;

                var tableRows = $("#mspdbody tr");
                for (var i = 1; i <= tableRows.length; i++) {
                    var item = $(tableRows[i - 1]).find("td").eq(0);
                    item.html(i);
                }
            } else {
                bootbox.dialog({
                    message: "不能删除，现在没有任何产品",
                    buttons: {
                        main: {
                            label: "确定",
                            className: "btn-dialog",
                            callback: function () {
                                return true;
                            }
                        }
                    }
                });
            }
        }

        function handleValidate() {
            var form_move = $('#spend_table_frm');
            error = $('.alert-danger', form_move);
            success = $('.alert-success', form_move);

            function is_alphabetics(value) {
                return /^[a-zA-Z]+$/.test(value);
            }
            $.validator.addMethod("alphabetics", is_alphabetics, "请输入英文字");

            form_move.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {

                    count: {
                        required: true,
                        digits: true
                    },

                    spendreason: {
                        required: true
                    },
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
        }

        function submitform() {
            var store1_val = 0, store2_val = 0;
            $.each($("#spend_table_frm select[name='start_store']"), function () {
                store1_val = $(this).val();
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#spend_table_frm input[name='spendreason']"), function () {
                store2_val = $(this).val();
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
                if (store1_val == store2_val) {
                    $(this).parent().removeClass('has-success').addClass('has-error');
                }
            });
            $.each($("#spend_table_frm select[name='catalog_id']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#spend_table_frm select[name='standard_id']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#spend_table_frm select[name='largenumber']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#spend_table_frm input[name='count']"), function () {
                if (/^[1-9][0-9]*$/.test($(this).val()))
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else {
                    $(this).parent().removeClass('has-success').addClass('has-error');
                }
            });

            if ($("#mspdbody").find(".has-error").length > 0)
                return;

            if ($("#spend_table_frm").valid()) {
                $("#cancelButton").trigger("click");

                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Store/SubmitSpendInStore",
                    dataType: "json",
                    data: $('#spend_table_frm').serialize(),
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
                            toastr["error"](data.error, data);
                        }
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }

        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                //window.location = rootUri + "Supply/Index?tabnum=1";
                make_spending_table(0);
                changeStore(1);
            }
        }
    </script>
</body>
</html>
