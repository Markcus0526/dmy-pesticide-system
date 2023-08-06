<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>


<!DOCTYPE html>
<html>
<head>
    <title></title>
    <%--<link href="<%= ViewData["rootUri"] %>Content/plugins/select2/awesome.css" rel="stylesheet" type="text/css" />--%>
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-datepicker/css/datepicker.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <div id="tabs-1">
        <form action="#" id="supply_table_frm" class="custom-help form-validate">
            <table class="table_edit table_edit_small smallfont" cellspacing="10" cellpadding="0">
                <tr>
                    <td>单号:
                    <td>
                        <input type="text" id="dticketnum" value="<% =ViewData["ticketnum"] %>" class="form-control" disabled />
                        <input type="hidden" name="ticketnum" id="ticketnum" value="<% =ViewData["ticketnum"] %>" class="form-control"/>
                    </td>
                    <td>供应商: 
                    </td>
                    <td>
                        <div class="form-group">
                            <select name="supply" class="form-control" id="supplier'>
                                <%
                                    List<tbl_supply> supplys = (List<tbl_supply>)ViewData["supplys"];
                                    for (int i = 0; i < supplys.Count(); i++)
                                    {
                                        tbl_supply supplyitem = supplys.ElementAt(i);
                                %>
                                <option value="<% =supplyitem.id %>"><% =supplyitem.name %></option>
                                <%
                                }
                                %>
                            </select>
                        </div>
                    </td>
                    <td>货品分类: 
                    </td>
                    <td>
                            <select id="topnongyao_type" class="form-control" onchange="changeTopNongyao();">
                                <option value="0">农药</option>
                                <option value="1">种子</option>
                                <option value="2">化肥</option>
                            </select>
                    </td>
                </tr>
                <tr>
                    <td>管理员 :
                    </td>
                    <td>
                        <input type="text" value="<% =ViewData["username"] %>" disabled class="form-control" />
                    </td>
                    <td>日期: 
                    </td>
                    <td>
                        <input type="text" value="<% =ViewData["curdate"] %>" disabled class="form-control" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>购货方式: 
                    </td>
                    <td>
                        <select name="paytype" class="form-control" id="paytype" onchange="changePaytype();">
                            <option value="0">现金</option>
                            <option value="1">赊购</option>
                            <option value="2">银行存款</option>
                            <option value="3">赠送</option>
                        </select>
                    </td>
                    <td>仓库: 
                    </td>
                    <td>
                        <div class="form-group">
                            <select name="store" class="form-control">
                                <%
                                    List<tbl_store> stores = (List<tbl_store>)ViewData["stores"];
                                    for (int i = 0; i < stores.Count(); i++)
                                    {
                                        tbl_store storeitem = stores.ElementAt(i);
                                %>
                                <option value="<% =storeitem.id %>"><% =storeitem.name %></option>
                                <%
                                }
                                %>
                            </select>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <button type="button" class="btn_actionex" onclick="make_supply_table();">添加</button>
            <%--<table width="100%"><tr><td><button type="button" class="btn_actionex" onclick="make_supply_table();">添加</button></td></tr></table>--%>
            <button type="button" class="btn_actionex" onclick="printSupply()">打印</button>
            <button type="button" class="btn_actionex" onclick="window.location='<%=ViewData["rootUri"] %>Catalog/Index'">农药添加</button>
            <table class="table_view smallfont table_view_bottom" border="0" cellspacing="1">
                <thead>
                    <tr>
                        <th style="text-align:center">顺序</th>
                        <th style="text-align:center">农药登记证号\拼音简码</th>
                        <th style="text-align:center">农药名称</th>
                        <th style="text-align:center">农药登记证号</th>
                        <th style="text-align:center">产地</th>
                        <th style="text-align:center">农药规格</th>
                        <th style="text-align:center">产品批号</th>
                        <th style="text-align:center">生产日期</th>
                        <th style="text-align:center">有效期</th>
                        <th style="text-align:center">单价</th>
                        <th style="text-align:center">数量</th>
                        <th style="text-align:center">金额</th>
                        <th style="text-align:center">操作</th>
                    </tr>
                </thead>
                <tbody id="supply_body1" style="background: white;">
                   
                </tbody>
            </table>
            <button class="btn_submit" onclick="submitform();">保存</button>

            <input type="hidden" name="type" value="0" />

        </form>
    </div>
<div id="print_page_supply" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>采购进货表</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">日期:<span><% =ViewData["curdateymd"]%></span></span>
	<table class="table_view" border="1">
		<thead>
			<tr>
         <%--       <th style="text-align:center">顺序</th>--%>
               <%-- <th style="text-align:center">农药登记证号\拼音简码</th>--%>
                <th style="text-align:center">农药名称</th>
                <th style="text-align:center">农药登记证号</th>
                <th style="text-align:center">产地</th>
                <th style="text-align:center">农药规格</th>
                <th style="text-align:center">产品批号</th>
                <th style="text-align:center">生产日期</th>
                <th style="text-align:center">有效期</th>
                <th style="text-align:center">单价</th>
                <th style="text-align:center">数量</th>
                <th style="text-align:center">金额</th>
                <%--<th style="text-align:center">操作</th>--%>
			</tr>
		</thead>
		<tbody id="print_supply_body1" style="background: white; border: 0px;">			
		</tbody>

    </table>
        <span style="float: left; margin-left: 15px;">单号:
            <span><% =ViewData["ticketnum"] %></span>
        </span>
	    <span style="margin-left: 15px;">供货商名称:
            <span id="print_supplier"></span>
        </span>
        <span style="float: right; margin-right: 15px;">管理员:
            <span><% =ViewData["username"] %></span>
        </span>
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
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });

        var cur_count = 0, next_num = 0;
        jQuery(document).ready(function () {
            App.init();
            handleValidate();
            make_supply_table();
        });

        function make_supply_table() {
            var curbody = $("#supply_body1").html();
            var body = "";//curbody;

            cur_count++; next_num++;
            body += "<tr id='supply_tr_1_" + cur_count + "'>" +
                    "<td style='width:40px'>" + next_num + "</td>" +
                    "<td style='width:80px'><input class='form-control' onchange='searchCatalog(" + cur_count + ")' /></td>" +
                    "<td><div class='form-group'><select name='catalog_id' class='form-control' value='' onchange='changeCatalog(" + cur_count + ")' style='width:100px;'>";
            <% 
            List<tbl_catalog> catalogs = (List<tbl_catalog>)ViewData["catalogs"];
            for (int i = 0; i < catalogs.Count(); i++)
            {
                tbl_catalog catalogitem = catalogs.ElementAt(i);
                %>
                body += "<option value='<% =catalogitem.id%>'><% =catalogitem.name%></option>";
                <%
            }
            %>

            body += "</select></div></td>" +
                    "<td><input value='<% =ViewData["register_id0"]%>' disabled class='form-control' /></td>" +
                    "<td><input value='<% =ViewData["product_area0"]%>' disabled class='form-control' /></td>" +
                    "<td><div style='width:170px;'><div class='form-group' style='float:left;'><input name='standard' value='' class='form-control' style='width:50px;' /></div>" +
                        "<select name='mass' class='form-control' style='width:60px;float:left;' >" +
                            "<option value='0' selected>克</option>" +
                            "<option value='1'>毫升</option>";

            body += "</select><select name='unit_id' class='form-control' style='width:60px;float:left;'>";
                <% 
                List<tbl_unit> units = (List<tbl_unit>)ViewData["units"];
                for (int i = 0; i < units.Count(); i++)
                {
                    tbl_unit unititem = units.ElementAt(i);
                    %>
                    body += "<option value='<% =unititem.id%>' <% if(i == 0) { %>selected<% } %>><% =unititem.name%></option>";
            <%
                }
                %>
            if ($("#paytype").val() != 3) {
                body += "</select></div></td>" +
                    "<td><div class='form-group'><input name='largenumber' value='' class='form-control' /></div></td>" +
                    "<td style='width:100px;'><div class='form-group'><input name='product_date' value='' class='form-control date-picker' data-date-format='yyyy-mm-dd' placeholder='yyyy-mm-dd' readonly style='cursor:pointer;' /></div></td>" +
                    "<td><input value='<% =ViewData["avail_date0"]%>' disabled class='form-control' /></td>" +
                    "<td><div class='form-group'><input name='price' value='' onchange='changeAutoPrice(" + cur_count + ")' class='form-control' /></div></td>" +
                    "<td><div class='form-group'><input name='count' value='' onchange='changeAutoPrice(" + cur_count + ")' class='form-control' /></div></td>" +
                    "<td><input id='autoprice_" + cur_count + "' value='自动结算' disabled class='form-control' /></td>" +
                    "<td style='width:50px;text-align:center;'> <img src='<%= ViewData["rootUri"]%>Content/image/delete_img.png' width=20px height=20px onclick='remove_supply_table(" + cur_count + ");' />&nbsp;</td></tr>";
            }
            else {
                body += "</select></div></td>" +
                    "<td><div class='form-group'><input name='largenumber' value='' class='form-control' /></div></td>" +
                    "<td style='width:100px;'><div class='form-group'><input name='product_date' value='' class='form-control date-picker' data-date-format='yyyy-mm-dd' placeholder='yyyy-mm-dd' readonly style='cursor:pointer;' /></div></td>" +
                    "<td><input value='<% =ViewData["avail_date0"]%>' disabled class='form-control' /></td>" +
                    "<td><div class='form-group'><input name='price' value='0' onchange='changeAutoPrice(" + cur_count + ")' class='form-control' /></div></td>" +
                    "<td><div class='form-group'><input name='count' value='' onchange='changeAutoPrice(" + cur_count + ")' class='form-control' /></div></td>" +
                    "<td><input id='autoprice_" + cur_count + "' value='自动结算' disabled class='form-control' /></td>" +
                    "<td style='width:50px;text-align:center;'> <img src='<%= ViewData["rootUri"]%>Content/image/delete_img.png' width=20px height=20px onclick='remove_supply_table(" + cur_count + ");' />&nbsp;</td></tr>";
            }
                
            //$("#supply_body1").html(body);
            $("#supply_body1").append(body);

            $('.date-picker').datepicker({
                //option: $.fn.datepicker.dates['zh-CN']
                //locale: 'zh-CN'
            });
        }

        function make_print_supply_table() {
            var curpbody = $("#print_supply_body1").html();
            var body = "";//curbody;
            for(i=1; i<=next_num;i++){
                body += "<tr><td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td style='width:100px;'></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td style='width:50px;text-align:center;'></td></tr>";
            }
            //$("#supply_body1").html(body);
            $("#print_supply_body1").html(body);
        }

        function handel_print_table() {
            var html_body=$("#supply_body1").find("tr"); 
            if (html_body == null) {

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
            else{        
                for(i=0;i<html_body.length;i++){
                var print_body = $("#print_supply_body1").find("tr"); 
                print_body.eq(i).children().eq(0).html(html_body.eq(i).find("select").eq(0).find("option:selected").text());
                print_body.eq(i).children().eq(1).html(html_body.eq(i).find("td").eq(3).find("input").val());
                print_body.eq(i).children().eq(2).html(html_body.eq(i).find("td").eq(4).find("input").val());
                
                //规格
                var guige=html_body.eq(i).find("td").eq(5).find("input").eq(0).val()+html_body.eq(i).find("td").eq(5).find("select").eq(0).find("option:selected").text()+ "/" +html_body.eq(i).find("td").eq(5).find("select").eq(1).find("option:selected").text();
                print_body.eq(i).children().eq(3).html(guige);
                
                print_body.eq(i).children().eq(4).html(html_body.eq(i).find("td").eq(6).find("input").val());

                print_body.eq(i).children().eq(5).html(html_body.eq(i).find("td").eq(7).find("input").val());
                print_body.eq(i).children().eq(6).html(html_body.eq(i).find("td").eq(8).find("input").val());
                print_body.eq(i).children().eq(7).html(html_body.eq(i).find("td").eq(9).find("input").val());
                print_body.eq(i).children().eq(8).html(html_body.eq(i).find("td").eq(10).find("input").val());
                print_body.eq(i).children().eq(9).html(html_body.eq(i).find("td").eq(11).find("input").val());

                } 
                $("#print_supplier").html($("#supplier").find("option:selected").text());
            }
        }

        

        
        function remove_supply_table(id) {
            if (next_num > 1) {
                $("#supply_tr_1_" + id).remove();
                next_num--;

                var tableRows = $("#supply_body1 tr");
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

        function renumber_table() {
            var tableRows = $("#supply_body1 tr");
            alert(tableRows.length);
        }

        function changeCatalog(num) {
            var catalog_id = $("#supply_tr_1_" + num).find("select").eq(0).val();

            if (catalog_id == null)
                return;

            $.ajax({
                async: false,
                type: "GET",
                url: rootUri + "Supply/GetCatalogInfo",
                dataType: "json",
                data: {
                    "id": catalog_id,
                    "num": num
                },
                success: function (data) {
                    if (data != null && data != "") {
                        var info = data.data;                        
                        var parenttr = $("#supply_table_frm #supply_tr_1_" + data.num);
                        parenttr.find("td").eq(3).find("input").val(info.register_id);
                        parenttr.find("td").eq(4).find("input").val(info.product_area);
                        parenttr.find("td").eq(8).find("input").val(info.avail_date);
                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }

        function searchCatalog(num) {
            var search = $("#supply_tr_1_" + num).find("input").eq(0).val();

            //if (search == null || search.length == 0)
            //    return null;

            $.ajax({
                async: false,
                type: "GET",
                url: rootUri + "Supply/SearchCatalogList",
                dataType: "json",
                data: {
                    "search": search,
                    "num": num
                },
                success: function (data) {
                    if (data != null && data != "") {
                        catalogs = [];
                        catalogs = catalogs.concat(data.data);
                        var catalogselect = $("#supply_table_frm #supply_tr_1_" + data.num + " select[name='catalog_id']")
                        var options = "";
                        for (var i = 0; i < catalogs.length; i++) {
                            options += "<option value='" + catalogs[i].id + "'>" + catalogs[i].name + "</option>";
                        }
                        catalogselect.html(options);

                        var parenttr = $("#supply_table_frm #supply_tr_1_" + data.num);
                        if (catalogs.length > 0) {
                            parenttr.find("td").eq(3).find("input").val(catalogs[0].register_id);
                            parenttr.find("td").eq(4).find("input").val(catalogs[0].product_area);
                            parenttr.find("td").eq(8).find("input").val(catalogs[0].avail_date);
                        } else {
                            parenttr.find("td").eq(3).find("input").val("");
                            parenttr.find("td").eq(4).find("input").val("");
                            parenttr.find("td").eq(8).find("input").val("");
                        }
                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }

        function changeAutoPrice(num) {
            var price = $("#supply_tr_1_" + num).find("td").eq(9).find("input").val();
            var count = $("#supply_tr_1_" + num).find("td").eq(10).find("input").val();

            if (!isNaN(price) && !isNaN(count) && price.length > 0 && count.length > 0) {
                var autoprice = price * count;
                $("#supply_tr_1_" + num).find("td").eq(11).find("input").val(autoprice);
            } else {
                $("#supply_tr_1_" + num).find("td").eq(11).find("input").val("自动结算");
            }
        }

        function handleValidate() {
            var form_shop = $('#supply_table_frm');
            error = $('.alert-danger', form_shop);
            success = $('.alert-success', form_shop);

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
                    ticketnum: {
                        required: true
                    },
                    supply: {
                        required: true
                    },
                    store: {
                        required: true
                    },
                    paytype: {
                        required: true
                    },
                    catalog_id: {
                        required: true
                    },
                    standard: {
                        required: true,
                        number: true
                    },
                    largenumber: {
                        required: true
                    },
                    product_date: {
                        required: true,
                        date: true
                    },
                    price: {
                        required: true,
                        number: true
                    },
                    count: {
                        required: true,
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
        }

        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                window.location = rootUri + "Supply/Index?tabnum=1";
            }
        }

        function submitform() {
            $.each($("#supply_table_frm input[name='price']"), function () {
                if (/^[1-9][0-9]*\.?[0-9]*|0\.[0-9]*$/.test($(this).val()))
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#supply_table_frm input[name='count']"), function () {
                if (/^[1-9][0-9]*$/.test($(this).val()))
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#supply_table_frm select[name='catalog_id']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#supply_table_frm input[name='standard']"), function () {
                if (/^[1-9][0-9]*\.?[0-9]*|0\.[0-9]*$/.test($(this).val()))
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#supply_table_frm input[name='largenumber']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#supply_table_frm input[name='product_date']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });
            $.each($("#supply_table_frm input[name='unit_id']"), function () {
                if ($(this).val() != null && $(this).val().length > 0)
                    $(this).parent().removeClass('has-error').addClass('has-succes');
                else
                    $(this).parent().removeClass('has-success').addClass('has-error');
            });


            if ($("#supply_body1").find(".has-error").length > 0)
                return;

            if ($("#supply_table_frm").valid()) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Supply/SubmitTicket",
                    dataType: "json",
                    data: $('#supply_table_frm').serialize(),
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
                                "timeOut": "3500",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            };
                            toastr["error"](data, "温馨敬告");

                            if (data.error == "重复") {
                                $("#supply_table_frm #ticketnum").val(data.ticketnum);
                                $("#supply_table_frm #dticketnum").val(data.ticketnum);
                                bootbox.dialog({
                                    message: "采购单号重复了，单号已经修改了，再试一试一边",
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
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }

        function changeTopNongyao() {
            if ($("#topnongyao_type").val() != 0) {

                $("#topnongyao_type").attr("value", 0);

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
                toastr["error"]("您所在的用户组无此操作权限!", "温馨敬告");
            }
        }

        function changePaytype() {
            if ($("#paytype").val() == 3) {
                for (var i = 1; i <= cur_count; i++) {
                    $("#supply_tr_1_" + i).find("td").eq(9).find("input").attr("value", 0);
                    var count = $("#supply_tr_1_" + i).find("td").eq(10).find("input").val();
                    if (!isNaN(count) && count.length > 0)
                        $("#supply_tr_1_" + i).find("td").eq(11).find("input").attr("value", 0);
                    else
                        $("#supply_tr_1_" + i).find("td").eq(11).find("input").val("自动结算");
                }
                
            }
        }

    </script>

    <script type="text/javascript">

        function printSupply() {
            if ($("#supply_body1 tbody").find("tr") == null) {
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

         //   $("#print_supply_body").html($("#supply_body1").html());
            make_print_supply_table();
            handel_print_table();
            pageprint();
        }

        function beforePrint() {
        }
        function afterPrint() {
            $("#header").css("display", "block");
            $("#tabs-1").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_supply").css("display", "none");
        }
        function pageprint() {
            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint;
            $("#header").css("display", "none");
            $("#tabs-1").css("display", "none");
            $(".ui-tabs-nav").css("display", "none");
            $("#print_page_supply").css("display", "block");
            window.print();
            $("#header").css("display", "block");
            $("#tabs-1").css("display", "block");
            $(".ui-tabs-nav").css("display", "block");
            $("#print_page_supply").css("display", "none");
        }

</script>
</body>
</html>
