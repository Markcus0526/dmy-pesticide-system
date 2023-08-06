<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>

<link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
<link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
<link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
<link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />
<link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />

<div id="tabs-1">
    <form action="#" id="sale_table_frm" class="custom-help form-validate">
        <table class="table_edit smallfont" cellspacing="10" cellpadding="0">
            <tr>
                <td>单号: 
                </td>
                <td>
                    <input type="text" id="dticketnum" value="<% =ViewData["ticketnum"] %>" class='form-control' disabled />
                    <input type="hidden" name="ticketnum" id="ticketnum" value="<% =ViewData["ticketnum"] %>" class='form-control' />
                </td>
                <td>客户: 
                </td>
                <td>
                    <input type="radio" name="customer_type" value="0" class="s_input" checked onclick="changeCustomer()" />
                    个人
				<input type="radio" name="customer_type" value="1" class="s_input" onclick="changeCustomer()" />
                    经销商
                </td>
            </tr>
            <tr>
                <td>经销商: 
                </td>
                <td>
                    <input type="text" value="<% =ViewData["shoppername"]%>" disabled class='form-control' />
                </td>
                <td>
                    <label>客户名称: </label>
                </td>
                <td>
                    <div class="form-group">
                        <input name="customer_name" class="sale-customer input form-control" type="text" value="" onchange="changeCustomerNameOrPhone(false)" />
                        <select name="othershop" class="sale-shopper input form-control" onchange="ShowShopPhone();">
                            <%
                                List<tbl_shop> shoppers = (List<tbl_shop>)ViewData["othershoppers"];
                                for (int i = 0; i < shoppers.Count(); i++)
                                {
                                    tbl_shop shopperitem = shoppers.ElementAt(i);
                            %>
                            <option value="<% =shopperitem.id %>"><% =shopperitem.name %></option>
                            <%
                                }
                            %>
                        </select>
                    </div>
                </td>

            </tr>
            <tr>
                <td>日期: 
                </td>
                <td>
                    <input type="text" value="<% =ViewData["curdate"] %>" disabled class='form-control' />
                </td>
                <td>
                    <label>手机号码: </label>
                </td>
                <td>
                    <div class="form-group">
                        <input name="customer_phone" class="input form-control" type="text" value="" onchange="changeCustomerNameOrPhone(true)" onkeyup="this.value=this.value.replace(/[^\d]/,'')" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>业务员: 
                </td>
                <td>
                    <input type="text" value="<% =ViewData["username"] %>" disabled class='form-control' />
                </td>
                <td>购货方式 :
                </td>
                <td>
                    <select name="paytype" class='form-control' id="paytype">
                        <option value="0">现金</option>
                        <option value="1">赊购</option>
                        <option value="2">银行存款</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>选择仓库:
                </td>
                <td>
                        <div class="form-group">
                    <select name="store" class="form-control" onchange="changeStore();">
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
                <td></td>
                <td></td>
            </tr>
        </table>
        <button type="button" class="btn_actionex" onclick="make_sale_table()">添加</button>
        <button type="button" class="btn_actionex" onclick="window.location = '<% =ViewData["rootUri"] %>Sale/Index?tabnum=1';">开单</button>
        <table class="table_view smallfont table_view_bottom" border="0" cellspacing="1">
            <thead>
                <tr>
                    <th style="text-align: center">货品编号</th>
                    <th style="text-align: center">农药名称</th>
                    <th style="text-align: center">农药规格</th>
                    <th style="text-align: center">库存</th>
                    <th style="text-align: center">单价</th>
                    <th style="text-align: center">数量</th>
                    <th style="text-align: center">金额</th>
                    <th style="text-align: center">生产厂家</th>
                    <th style="text-align: center">生产批号</th>
                    <th style="text-align: center">生产日期</th>
                    <th style="text-align: center">有效期</th>
                    <th style="text-align: center">操作</th>
                </tr>
            </thead>
            <tbody id="sale_body1" style="background: white;">
            </tbody>
        </table>
        <button class="btn_submit" onclick="submitform();">结算</button>
        <button data-target="#CalcChange" data-toggle="modal" style="display: none;" id="submitButton">结算</button>
        <!--<button type="button" class="btn_actionex" onclick="printToDoc()">打印</button>-->

        <input type="hidden" name="type" value="2" />

        <input type="hidden" name="sellmoney" id="sellmoney" value="0" />
        <input type="hidden" name="sellchange" id="sellchange" value="0" />

    </form>
    <div id="CalcChange" class="modal fade" tabindex="-1" data-width="300">
        <form action="#" id="submit_form_price" class="form-horizontal form-validate">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <table class="table_dialog" border="0" style="width: 80%">
                            <tr>
                                <td style="vertical-align: top">应收金额：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align: top">
                                    <div class="form-group">
                                            <input type="text" name="TotalMoney" id="TotalMoney" data-required="1" class="form-control"
                                                value="" style="width: 300px;" disabled />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">实收金额：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align: top">
                                    <div class="form-group">
                                            <input type="text" name="PayMoney" id="PayMoney" data-required="1" class="form-control"
                                                value="" style="width: 300px;" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">抹零：&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="vertical-align: top">
                                    <div class="form-group">
                                            <input type="text" name="Change" id="Change" data-required="1" class="form-control"
                                                value="" style="width: 300px;" disabled />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn_rect" onclick="submitform_real(0);">确定</button>

                        <%--<button type="button" class="btn_rect" onclick="SubmitStatistics()">抹零</button>--%>
                        <button type="button" data-dismiss="modal" class="btn_rect" id="cancelButton" style="display: none"></button>
                    </div>
                </div>
            </div>
        </form>
    </div>
</div>


<%--<div id="print_page" name="print_page" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>批发销售单</b></font></p><br/>
	<span style="float: left; margin-left: 15px;">销售日期:<% =ViewData["curdate"] %></span><span style="float: right; margin-right: 15px;">单号:<% =ViewData["ticketnum"] %></span><br/>
	<span style="float: left; margin-left: 15px;">公司名称:<% =ViewData["shoppername"]%></span><span style="float: right; margin-right: 15px;">开发人员:<% =ViewData["username"] %></span>
	<table class="table_view" border="1">
		<thead>
			<tr>
				<th style="text-align:center">货品编号</th>
				<th style="width:200px;text-align:center">农药名称</th>
				<th style="text-align:center">单位</th>
				<th style="text-align:center">单价</th>
				<th style="text-align:center">数量</th>
				<th style="text-align:center">总价</th>
			</tr>
		</thead>
		<tbody id="print_body" style="background: white; border: 0px;">			
		</tbody>
	</table>
	<span style="float:center;"><input type="text" id="print_total" name="print_total" style="border: none; border-color: transparent;" value /></span>
    <span style="float: right; margin-right: 15px;"><input type="text" id="print_user" name="print_user" style="border: none; border-color: transparent;" value /></span><br/>
	<span style="float: right; margin-right: 15px;"><input type="text" id="print_phone" name="print_phone" style="border: none; border-color: transparent;" value /></span><br/>
</div>--%>

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
<script type="text/javascript">
    $(function () {
        $("#tabs").tabs();
    });

    var cur_count = 0, next_num = 0;
    jQuery(document).ready(function () {
        App.init();

        switchCustomerOrShopper(true);

        handleValidate();
        make_sale_table();
    });

    function CalcChangeFunc() {
       /* var totalmoney = $("#TotalMoney").val();
        var paymoney = $("#PayMoney").val();
        var change = paymoney - totalmoney;
        if (change != undefined && !isNaN(change))
            $("#Change").val(change);*/
    }

    function SubmitStatistics() {
        /*if ($("#submit_form_price").valid()) {
			$("#cancelButton").trigger("click");

			$.ajax({
				type: "POST",
				url: "<%= ViewData["rootUri"] %>/*Money/SubmitStatistics",
				dataType: 'json',
				data: {
					stat_id: "0",
					stat_type: "1",
					statprice: $("#Change").val(),
					statreason: "销售的找零"
				},
				success: function (data) {
					if (data == true)*/
	    submitform_real(1);
	    /*}
    });
}*/
	}

	function changeCustomer() {
	    $(".has-error").removeClass("has-error");
	    $(".has-success").removeClass("has-success");
	    $(".help-block").remove();

	    if ($("#sale_table_frm input[name='customer_type']").eq(0).attr("checked") == undefined)
	        switchCustomerOrShopper(false);
	    else
	        switchCustomerOrShopper(true);
	}

	function switchCustomerOrShopper(isCustomer) {
	    if (isCustomer) {
	        $("#sale_table_frm input[name='customer_phone']").attr("disabled", false);
	        $("#sale_table_frm input[name='customer_phone']").attr("value", "");
	        $.each($(".sale-customer"), function () {
	            $(this).show();
	        });
	        $.each($(".sale-shopper"), function () {
	            $(this).hide();
	        });
	    } else {
	        $("#sale_table_frm input[name='customer_phone']").attr("disabled", "disabled");
	        $.each($(".sale-customer"), function () {
	            $(this).hide();
	        });
	        $.each($(".sale-shopper"), function () {
	            $(this).show();
	        });
	        ShowShopPhone();
	    }
	}

	function make_sale_table() {
	    var body = "";

	    cur_count++; next_num++;
	    body += "<tr id='sale_tr_1_" + cur_count + "'>" +
				"<td><% = ViewData["catalog_num0"]%></td>" +
				"<td><div class='form-group'><select name='catalog_id' class='form-control' onchange='changeCatalog(" + cur_count + ")'>";
			<% 
    List<tbl_catalog> catalogs = (List<tbl_catalog>)ViewData["catalogs"];
    if (catalogs != null) { 
    for (int i = 0; i < catalogs.Count(); i++)
    {
        tbl_catalog catalogitem = catalogs.ElementAt(i);
		%>
	    body += "<option value='<% =catalogitem.id%>'><% =catalogitem.name%></option>";
			<%
    }
    }
		%>

	    body += "</select></div></td>" +
                "<td style='width:100px;'><div class='form-group'><select name='standard_id' class='form-control' onchange='changeStandard(" + cur_count + ")' style='width:100px;'>";
           <% 
    List<StandardInfo> standards = (List<StandardInfo>)ViewData["standards0"];
    if (standards != null)
    {
        for (int i = 0; i < standards.Count(); i++)
        {
            StandardInfo standarditem = standards.ElementAt(i);
                %>
	    body += "<option value='<% =standarditem.id%>'><% =standarditem.standard%></option>";
                <%
            }
            }
            %>

	    body += "</select></div></td>" +
				"<td><input value='<% =ViewData["remain0"]%>' style = 'width:100%' disabled class='form-control' /></td>" +
				"<td><div class='form-group'><input name='price' value='' class='form-control' onchange='changeAutoPrice(" + cur_count + ")' /></div></td>" +
				"<td><div class='form-group'><input name='count' value='' class='form-control' onchange='changeAutoPrice(" + cur_count + ")' /></div></td>" +
				"<td><input value='自动结算' disabled class='form-control' /></td>" +
				"<td><input value='<% =ViewData["product_area0"]%>' disabled class='form-control' /></td>" +
                "<td><div class='form-group'><select name='largenumber' class='form-control' onchange='changeLargenumber(" + cur_count + ")' style='width:100px;'>";
            <% 
    List<string> largenumbers = (List<string>)ViewData["largenumbers0"];
    if (largenumbers != null)
    {
        for (int i = 0; i < largenumbers.Count(); i++)
        {
            string largenumber = largenumbers.ElementAt(i);
                %>
	    body += "<option value='<% =largenumber%>'><% =largenumber%></option>";
                <%
            }
            }
            %>

	    body += "</select></div></td>" +
				"<td style='width:100px;'><input value='<% =ViewData["product_date0"]%>' disabled class='form-control' /></td>" +
				"<td><input value='<% =ViewData["avail_date0"]%>' disabled class='form-control' /></td>" +
				"<td style='width:50px;text-align:center;'> <img src='<%= ViewData["rootUri"]%>Content/image/delete_img.png' width=20px height=20px onclick='remove_sale_table(" + cur_count + ");'/>&nbsp;</td></tr>";

        $("#sale_body1").append(body);

	    //changeStore();
    }

    function remove_sale_table(id) {
        if (next_num > 1) {
            $("#sale_tr_1_" + id).remove();
            next_num--;

            /*var tableRows = $("#sale_body1 tr");
			for (var i = 1; i <= tableRows.length; i++) {
				var item = $(tableRows[i - 1]).find("td").eq(0);
				item.html(i);
			}*/
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

    var catalogs;
    function changeStore() {
        //$.each($("#sale_body1 tr"), function () {
        //    var idstr = $(this).attr("id");
        //    changeCatalog(idstr.substring(10, idstr.length));
        //});

        var store_id = $("#sale_table_frm select[name='store']").val();

        if (store_id == null)
            return;

        $.ajax({
            async: false,
            type: "POST",
            url: rootUri + "Supply/GetCatalogsInStore",
            dataType: "json",
            data: {
                "store": store_id
            },
            success: function (data) {
                catalogs = [];
                catalogs = catalogs.concat(data);
                $.each($("#sale_body1 tr"), function () {
                    var catalogselect = $(this).find("select[name='catalog_id']");
                    var options = "";
                    for (var i = 0; i < catalogs.length; i++) {
                        options += "<option value='" + catalogs[i].id + "'>" + catalogs[i].name + "</option>";
                    }
                    catalogselect.html(options);

                    var idstr = $(this).attr("id");
                    changeCatalog(idstr.substring(10, idstr.length));
                });
            },
            error: function (data) {
                alert("Error: " + data.status);
                $('.loading-btn').button('reset');
            }
        });
    }

    function changeCatalog(num) {
        var store_id = $("#sale_table_frm select[name='store']").val();
        var catalog_id = $("#sale_tr_1_" + num).find("select").eq(0).val();

        if (store_id == null || catalog_id == null) {
            $("#sale_tr_1_" + num).find("td").eq(0).html("");
            $("#sale_tr_1_" + num).find("td").eq(2).find("select").html("");
            $("#sale_tr_1_" + num).find("td").eq(3).find("input").val("");
            $("#sale_tr_1_" + num).find("td").eq(7).find("input").val("");
            $("#sale_tr_1_" + num).find("td").eq(8).find("select").html("");
            $("#sale_tr_1_" + num).find("td").eq(9).find("input").val("");
            $("#sale_tr_1_" + num).find("td").eq(10).find("input").val("");

            return;
        }
        //alert(num + " | " + store_id + " | " + catalog_id );

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
                    var parenttr = $("#sale_table_frm #sale_tr_1_" + data.num);
                    parenttr.find("td").eq(0).html(info.catalog_num);
                    parenttr.find("td").eq(7).find("input").val(info.product_area);
                    parenttr.find("td").eq(10).find("input").val(info.avail_date);

                    var standards = [];
                    standards = standards.concat(data.data);
                    var standardselect = $("#sale_table_frm #sale_tr_1_" + data.num + " select[name='standard_id']")
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
        var store_id = $("#sale_table_frm select[name='store']").val();
        var catalog_id = $("#sale_tr_1_" + num).find("select").eq(0).val();
        var standard_id = $("#sale_tr_1_" + num).find("select").eq(1).val();

        if (store_id == null || catalog_id == null || standard_id == null) {
            $("#sale_tr_1_" + num).find("td").eq(3).find("input").val("");
            $("#sale_tr_1_" + num).find("td").eq(8).find("select").html("");
            $("#sale_tr_1_" + num).find("td").eq(9).find("input").val("");

            return;
        }

        //alert(num+ " | " + store_id + " | " + catalog_id + " | " + standard_id);

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
                    //var info = data.info
                    //var parenttr = $("#sale_table_frm #sale_tr_1_" + data.num);
                    //parenttr.find("td").eq(9).find("input").val(info.product_date);

                    var largenumbers = [];
                    largenumbers = largenumbers.concat(data.data);
                    var largenumberselect = $("#sale_table_frm #sale_tr_1_" + data.num + " select[name='largenumber']")
                    var options = "";
                    for (var i = 0; i < largenumbers.length; i++) {
                        options += "<option value='" + largenumbers[i] + "'>" + largenumbers[i] + "</option>";
                    }
                    largenumberselect.html(options);

                    changeLargenumber(num);
                }
            },
            error: function (data) {
                alert("Error: " + data.status);
                $('.loading-btn').button('reset');
            }
        });
    }

    function changeLargenumber(num) {
        var store_id = $("#sale_table_frm select[name='store']").val();
        var catalog_id = $("#sale_tr_1_" + num).find("select").eq(0).val();
        var standard_id = $("#sale_tr_1_" + num).find("select").eq(1).val();
        var largenumber = $("#sale_tr_1_" + num).find("select").eq(2).val();

        if (store_id == null || catalog_id == null || standard_id == null || largenumber == null) {
            $("#sale_tr_1_" + num).find("td").eq(8).find("select").html("");

            return;
        }
        //alert(num+ " | " + store_id + " | " + catalog_id + " | " + standard_id + " | " + largenumber);

        $.ajax({
            async: false,
            type: "GET",
            url: rootUri + "Sale/GetCatalogRemainInfo",
            dataType: "json",
            data: {
                "store_id": store_id,
                "catalog_id": catalog_id,
                "standard_id": standard_id,
                "largenumber": largenumber,
                "num": num
            },
            success: function (data) {
                if (data != null && data != "") {
                    var info = data.info
                    var parenttr = $("#sale_table_frm #sale_tr_1_" + data.num);
                    parenttr.find("td").eq(3).find("input").val(info.quantity);
                    parenttr.find("td").eq(9).find("input").val(info.product_date);
                }
            },
            error: function (data) {
                alert("Error: " + data.status);
                $('.loading-btn').button('reset');
            }
        });
    }

    function changeAutoPrice(num) {
        var price = $("#sale_tr_1_" + num).find("td").eq(4).find("input").val();
        var count = $("#sale_tr_1_" + num).find("td").eq(5).find("input").val();

        if (!isNaN(price) && !isNaN(count) && price.length > 0 && count.length > 0) {
            var autoprice = price * count;
            $("#sale_tr_1_" + num).find("td").eq(6).find("input").val(autoprice);
        } else {
            $("#sale_tr_1_" + num).find("td").eq(6).find("input").val("自动结算");
        }
    }

    function changeCustomerNameOrPhone(isPhone) {
        var search = "";
        if (isPhone) {
            search = $("#sale_table_frm input[name='customer_phone']").val();

            if (isNaN(search) == true) {
                $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-success').addClass('has-error');
                return;
            }

            if (search.length == 0 || search.length > 11) {
                $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-success').addClass('has-error');
                return;
            } else {
                $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-error').addClass('has-success');
            }
        } else {
            search = $("#sale_table_frm input[name='customer_name']").val();
            if (search.length > 0)
                $("#sale_table_frm input[name='customer_name']").parent().removeClass('has-success').removeClass('has-error');
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
                            $("#sale_table_frm input[name='customer_name']").val(data.result);
                        else
                            $("#sale_table_frm input[name='customer_phone']").val(data.result);
                    }
                },
                error: function (data) {
                    alert("Error: " + data.status);
                    $('.loading-btn').button('reset');
                }
            });
        }
    }

    function handleValidate() {
        var form_shop = $('#sale_table_frm');
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
                paytype: {
                    required: true
                },
                store: {
                    required: true
                },
                catalog_id: {
                    required: true
                },
                largenumber: {
                    required: true
                },
                standard_id: {
                    required: true
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
                if ($(element).attr("name") != "customer_phone")
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

        var form_price = $('#submit_form_price');
        error = $('.alert-danger', form_price);
        success = $('.alert-success', form_price);

        function compare_price(value) {
            var paymoney = parseFloat($("#PayMoney").val(), 10);
            var change = parseFloat($("#Change").val(), 10);
            var totalmoney = parseFloat($("#TotalMoney").val(), 10);
            //if ($("#PayMoney").val() + $("#Change").val() > $("#TotalMoney").val())
            if (paymoney + change > totalmoney)
                return false;
            else
                return true;
            //return /^[a-zA-Z]+$/.test(value);
        }
        $.validator.addMethod("compare_price", compare_price, "实收金额加找零要少比应收金额");

        form_price.validate({
            doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                PayMoney: {
                    required: true,
                    number: true,
                    compare_price: true
                },
                Change: {
                    required: true,
                    number: true,
                    compare_price: true
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

    function redirectToListPage(status) {
        if (status.indexOf("error") != -1) {
        } else {
            window.location = rootUri + "Sale/Index?tabnum=1";
        }
    }

    function submitform() {
        if ($("#sale_table_frm input[name='customer_type']").eq(0).attr("checked") == undefined) {
            if ($("#sale_table_frm select[name='othershop']").val() != null && $("#sale_table_frm select[name='othershop']").val().length > 0)
                $("#sale_table_frm select[name='othershop']").parent().removeClass('has-error').addClass('has-succes');
            else
                $("#sale_table_frm select[name='othershop']").parent().removeClass('has-success').addClass('has-error');
        } else {
            if ($("#sale_table_frm input[name='customer_name']").val() != null && $("#sale_table_frm input[name='customer_name']").val().length > 0)
                $("#sale_table_frm input[name='customer_name']").parent().removeClass('has-error').addClass('has-succes');
            else {
                $("#sale_table_frm input[name='customer_name']").parent().removeClass('has-success').addClass('has-error');
            }
            if ($("#sale_table_frm input[name='customer_phone']").val() != null && $("#sale_table_frm input[name='customer_phone']").val().length > 0) {
                if (isNaN($("#sale_table_frm input[name='customer_phone']").val()) == true || $("#sale_table_frm input[name='customer_phone']").val().length > 11)
                    $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-success').addClass('has-error');
                else
                    $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-error').addClass('has-succes');
            } else
                $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-success').addClass('has-error');
        }


        $.each($("#sale_table_frm input[name='price']"), function () {
            if (/^[1-9][0-9]*\.?[0-9]*|0\.[0-9]*$/.test($(this).val()))
                $(this).parent().removeClass('has-error').addClass('has-succes');
            else
                $(this).parent().removeClass('has-success').addClass('has-error');
        });
        $.each($("#sale_table_frm input[name='count']"), function () {
            if (/^[1-9][0-9]*$/.test($(this).val()))
                $(this).parent().removeClass('has-error').addClass('has-succes');
            else
                $(this).parent().removeClass('has-success').addClass('has-error');
        });
        $.each($("#sale_table_frm select[name='store']"), function () {
            if ($(this).val() != null && $(this).val().length > 0)
                $(this).parent().removeClass('has-error').addClass('has-succes');
            else
                $(this).parent().removeClass('has-success').addClass('has-error');
        });
        $.each($("#sale_table_frm select[name='catalog_id']"), function () {
            if ($(this).val() != null && $(this).val().length > 0)
                $(this).parent().removeClass('has-error').addClass('has-succes');
            else
                $(this).parent().removeClass('has-success').addClass('has-error');
        });
        $.each($("#sale_table_frm select[name='standard_id']"), function () {
            if ($(this).val() != null && $(this).val().length > 0)
                $(this).parent().removeClass('has-error').addClass('has-succes');
            else
                $(this).parent().removeClass('has-success').addClass('has-error');
        });
        $.each($("#sale_table_frm select[name='largenumber']"), function () {
            if ($(this).val() != null && $(this).val().length > 0)
                $(this).parent().removeClass('has-error').addClass('has-succes');
            else
                $(this).parent().removeClass('has-success').addClass('has-error');
        });


        if ($("#sale_table_frm").find(".has-error").length > 0)
            return;

        $("#CalcChange .has-error").removeClass("has-error");
        $("#CalcChange .has-success").removeClass("has-success");
        $("#CalcChange .help-block").remove();


        if ($("#sale_table_frm").valid()) {
            setPriceDialog();

            if ($("#sale_table_frm input[name='customer_type']").eq(0).attr("checked") == undefined)
                $("#sale_table_frm #submitButton").trigger("click");
            else {
                var name = $("#sale_table_frm input[name='customer_name']").val();
                var phone = $("#sale_table_frm input[name='customer_phone']").val();

                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Sale/SubmitUser",
                    dataType: "json",
                    data: {
                        "customer_name": name,
                        "customer_phone": phone,
                        "isinsert": 0
                    },
                    success: function (data) {
                        if (data != "")
                            submit_user();
                        else
                            $("#sale_table_frm #submitButton").trigger("click");
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
            }
        }
    }

    function submit_user() {
        var name = $("#sale_table_frm input[name='customer_name']").val();
        var phone = $("#sale_table_frm input[name='customer_phone']").val();

        bootbox.dialog({
            message: "客户没有在数据，您要生成客户吗？",
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
                            url: rootUri + "Sale/SubmitUser",
                            dataType: "json",
                            data: {
                                "customer_name": name,
                                "customer_phone": phone,
                                "isinsert": 1
                            },
                            success: function (data) {
                                if (data == "") {
                                    $("#sale_table_frm #submitButton").trigger("click");
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
                                    toastr["error"]("删除失败", "温馨敬告");
                                }
                            }
                        });
                    }
                }
            }
        });
    }

    function submitform_real(flag) {
        if ($("#submit_form_price").valid()) {
            var change = $("#Change").val();

            if (flag == 1 || (flag == 0 && parseInt(change) >= 0)) {
                $("#cancelButton").trigger("click");

                $("#sellmoney").val($("#PayMoney").val());
                $("#sellchange").val(change);
                
                $.ajax({
                    async: false,
                    type: "POST",
                    url: rootUri + "Sale/SubmitTicket",
                    dataType: "json",
                    data: $('#sale_table_frm').serialize(),
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
                            toastr["error"](data, "温馨敬告");

                            if (data.error == "重复") {
                                $("#sale_table_frm #ticketnum").val(data.ticketnum);
                                $("#sale_table_frm #dticketnum").val(data.ticketnum);
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
            } else {
                bootbox.dialog({
                    message: "实收金额少比应收金额，找零要大比0",
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
    }

    function setPriceDialog() {
        $("#CalcChange input").val("");
        $("#CalcChange #PayMoney").attr("disabled", false);
        $("#Change").attr("disabled", false);
        var total = 0.0;
        $.each($("#sale_table_frm #sale_body1 tr"), function () {
            price = $(this).find("input[name='price']").val();
            count = $(this).find("input[name='count']").val();
            total += price * count;
        });

        $("#TotalMoney").val(total);
        
        if ($("#paytype").val() == 1)
        {          
            $("#PayMoney").val(0);
            $("#Change").val(0);
            $("#PayMoney").attr("disabled", "disabled");
            $("#Change").attr("disabled", "disabled");
        }
            
    }

    function ShowShopPhone() {
        
        <%List<tbl_shop> shoppers1 = (List<tbl_shop>)ViewData["othershoppers"];
          for (var i = 0;i < shoppers1.Count(); i++)
          {
              tbl_shop shop = shoppers1.ElementAt(i);
          %>
        if ($("#sale_table_frm select[name='othershop']").val() == '<%=shop.id%>')
            $("#sale_table_frm input[name='customer_phone']").val('<%=shop.mobile_phone%>');
        <%}%>
    }

    //function convert_shuzi(value) {
    //    //去掉空格
    //    value = value.replace(/(^\s+)|(\s+$)/gi, '');

    //    var pattern = /^[0-9]+((\.)?[0-9]+)?$/;
    //    if (!pattern.test(value)) {
    //        alert('请输入数字');
    //        return false;
    //    }
    //    //数据检查
    //    if (value.length > 12) {
    //        alert('数据太大，请合理输入');
    //        return false;
    //    }

    //    var dian = value.indexOf('.');
    //    var arr_shuzi = ['零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖', '拾'];
    //    var daxie_shuzi = '';

    //    var val = Number(value);
    //    var yi = Math.floor(val / 100000000);//亿
    //    var wan = Math.floor(val % 100000000 / 10000);//万
    //    var qian = Math.floor(val % 10000 / 1000);//千
    //    var bai = Math.floor(val % 1000 / 100); //百
    //    var shi = Math.floor(val % 100 / 10);//十
    //    var ge = Math.floor(val % 10);//个
    //    //亿，万要特殊处理下
    //    daxie_shuzi += yi == 0 ? '' : convert_int(yi) + '亿';

    //    if ((yi > 0 && wan < 1000) && (yi % 10 == 0)) {
    //        daxie_shuzi += "零";
    //    }

    //    var wan_conv = convert_int(wan);
    //    daxie_shuzi += wan == 0 && yi == 0 ? '' : wan_conv;
    //    daxie_shuzi += wan == 0 ? '' : '万';

    //    if (wan % 10 == 0 && wan > 0) {
    //        daxie_shuzi += "零";
    //    }

    //    daxie_shuzi += qian == 0 && wan == 0 ? '' : arr_shuzi[qian];
    //    daxie_shuzi += qian == 0 ? '' : '仟';

    //    daxie_shuzi += bai == 0 && qian == 0 ? '' : arr_shuzi[bai];
    //    daxie_shuzi += bai == 0 ? '' : '佰';

    //    daxie_shuzi += shi == 0 && bai == 0 ? '' : arr_shuzi[shi];
    //    daxie_shuzi += shi == 0 ? '' : '拾';

    //    daxie_shuzi += ge == 0 ? '' : arr_shuzi[ge];
    //    //去掉最后一个零	
    //    daxie_shuzi = daxie_shuzi.replace(/零$/gi, '');

    //    daxie_shuzi += '圆';
    //    //无小数点
    //    if (dian == -1) {
    //        daxie_shuzi += '整';
    //        //小数点
    //    } else {
    //        //如果个位不是零，则不需加零
    //        if (ge == 0) {
    //            daxie_shuzi += '零';
    //        }

    //        //只取两位小数点 
    //        var float_shuzhi = value.substr(dian + 1);

    //        var jiao = value.substr(dian + 1, 1); //角
    //        daxie_shuzi += jiao == 0 ? '' : arr_shuzi[jiao];
    //        daxie_shuzi += jiao == 0 ? '' : '角';

    //        if (float_shuzhi.length > 1) {
    //            daxie_shuzi += jiao == 0 ? '零' : '';
    //            var fen = value.substr(dian + 2, 1); //分
    //            daxie_shuzi += fen == 0 ? '' : arr_shuzi[fen] + '分';
    //        }

    //    }
    //    //去掉重复零
    //    daxie_shuzi = daxie_shuzi.replace(/零{2,}/g, '零');

    //    return daxie_shuzi;
    //}

    //function convert_int(value) {
    //    var arr_shuzi = ['零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖', '拾'];
    //    var daxie_shuzi = '';
    //    var val = Number(value);
    //    var yi = Math.floor(val / 100000000);//亿
    //    var wan = Math.floor(val % 100000000 / 10000);//万
    //    var qian = Math.floor(val % 10000 / 1000);//千
    //    var bai = Math.floor(val % 1000 / 100); //百
    //    var shi = Math.floor(val % 100 / 10);//十
    //    var ge = Math.floor(val % 10);//个

    //    daxie_shuzi += yi == 0 ? '' : arr_shuzi[yi] + '亿';

    //    daxie_shuzi += wan == 0 && yi == 0 ? '' : arr_shuzi[wan];
    //    daxie_shuzi += wan == 0 ? '' : '万';

    //    daxie_shuzi += qian == 0 && wan == 0 ? '' : arr_shuzi[qian];
    //    daxie_shuzi += qian == 0 ? '' : '仟';

    //    daxie_shuzi += bai == 0 && qian == 0 ? '' : arr_shuzi[bai];
    //    daxie_shuzi += bai == 0 ? '' : '佰';

    //    daxie_shuzi += shi == 0 && bai == 0 ? '' : arr_shuzi[shi];
    //    daxie_shuzi += shi == 0 ? '' : '拾';

    //    daxie_shuzi += ge == 0 ? '' : arr_shuzi[ge];

    //    //去掉最后一个零	
    //    daxie_shuzi = daxie_shuzi.replace(/零$/gi, '');

    //    return daxie_shuzi;
    //}

    ////复制功能
    //function copy_cont() {
    //    var value = document.getElementById('output_shuzi').innerHTML;
    //    //去掉空格
    //    value = value.replace(/(^\s+)|(\s+$)/gi, '');
    //    if (window.clipboardData) {
    //        window.clipboardData.setData('text', value);
    //        alert('复制成功');
    //    } else {
    //        alert('只支持IE内核的浏览器，可使用ctrl+c进行复制');
    //    }

    //}

    ////输入框
    //function input_focus(obj) {
    //    if (obj.className == 'ipt_style') {
    //        obj.value = '';
    //        obj.className = 'ipt_style ipt_style_ing';
    //    }
    //}
    //function input_blur(obj) {
    //    var val = obj.value.replace(/(^\s+)|(\s+$)/gi, '');
    //    if (val == '') {
    //        obj.className = 'ipt_style';
    //        obj.value = '请输入阿拉伯数字，小数点后最多2位数字';
    //    }
    //}

    //function printToDoc()
    //{
    //    if ($("#sale_table_frm input[name='customer_type']").eq(0).attr("checked") == undefined) {
    //        if ($("#sale_table_frm select[name='othershop']").val().length > 0)
    //            $("#sale_table_frm select[name='othershop']").parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $("#sale_table_frm select[name='othershop']").parent().removeClass('has-success').addClass('has-error');
    //    } else {
    //        if ($("#sale_table_frm input[name='customer_name']").val().length > 0)
    //            $("#sale_table_frm input[name='customer_name']").parent().removeClass('has-error').addClass('has-succes');
    //        else {
    //            $("#sale_table_frm input[name='customer_name']").parent().removeClass('has-success').addClass('has-error');
    //        }
    //        if ($("#sale_table_frm input[name='customer_phone']").val().length > 0) {
    //            if (isNaN($("#sale_table_frm input[name='customer_phone']").val()) == true) 
    //                $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-success').addClass('has-error');
    //            else 
    //                $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-error').addClass('has-succes');
    //        } else
    //            $("#sale_table_frm input[name='customer_phone']").parent().removeClass('has-success').addClass('has-error');
    //    }


    //    $.each($("#sale_table_frm input[name='price']"), function () {
    //        if (/^[1-9][0-9]*\.?[0-9]*|0\.[0-9]*$/.test($(this).val()))
    //            $(this).parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $(this).parent().removeClass('has-success').addClass('has-error');
    //    });
    //    $.each($("#sale_table_frm input[name='count']"), function () {
    //        if (/^[1-9][0-9]*$/.test($(this).val()))
    //            $(this).parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $(this).parent().removeClass('has-success').addClass('has-error');
    //    });
    //    $.each($("#sale_table_frm select[name='store']"), function () {
    //        if ($(this).val() != null && $(this).val().length > 0)
    //            $(this).parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $(this).parent().removeClass('has-success').addClass('has-error');
    //    });
    //    $.each($("#sale_table_frm select[name='catalog_id']"), function () {
    //        if ($(this).val() != null && $(this).val().length > 0)
    //            $(this).parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $(this).parent().removeClass('has-success').addClass('has-error');
    //    });
    //    $.each($("#sale_table_frm select[name='largenumber']"), function () {
    //        if ($(this).val() != null && $(this).val().length > 0)
    //            $(this).parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $(this).parent().removeClass('has-success').addClass('has-error');
    //    });
    //    $.each($("#sale_table_frm select[name='standard_id']"), function () {
    //        if ($(this).val() != null && $(this).val().length > 0)
    //            $(this).parent().removeClass('has-error').addClass('has-succes');
    //        else
    //            $(this).parent().removeClass('has-success').addClass('has-error');
    //    });


    //    if ($("#sale_table_frm").find(".has-error").length > 0)
    //        return;

    //    $("#CalcChange .has-error").removeClass("has-error");
    //    $("#CalcChange .has-success").removeClass("has-success");
    //    $("#CalcChange .help-block").remove();

    //    $('#print_user').val("购买人 : " + $("#sale_table_frm input[name='customer_name']").val());
    //    $('#print_phone').val("手 机 : " + $("#sale_table_frm input[name='customer_phone']").val());

    //    var tableRows = $("#sale_body1 tr");
    //    var total = 0, price, count;
    //    var totalCount = 0;
    //    for (var i = 1; i <= tableRows.length; i++)
    //    {
    //        //$("#CalcChange input").val("");
    //        /*
    //        var total = 0;
    //        $.each($("#sale_table_frm #sale_body1 tr"), function () {
    //            price = parseInt($(this).find("input[name='price']").val(), 10);
    //            count = parseInt($(this).find("input[name='count']").val(), 10);
    //            total += price * count;
    //        });
    //        */

    //        //var id = $("#sale_tr_1_" + num).find("select").eq(1).val();

    //        price = parseInt($(tableRows[i - 1]).find("input[name='price']").val(), 10);
    //        count = parseInt($(tableRows[i - 1]).find("input[name='count']").val(), 10);
    //        var subTotal = price * count;
    //        var appendBody = "<tr>"
    //            + "<td align='center'>" + $(tableRows[i - 1]).find("td").eq(0).html() + "</td>"
    //            + "<td align='center'>" + $(tableRows[i - 1]).find("select").eq(1).find(":selected").html() + "</td>"
    //            + "<td align='center'>" + $(tableRows[i - 1]).find("select").eq(2).find(":selected").html() + "</td>"
    //            + "<td align='center'>" + $(tableRows[i - 1]).find("input[name='price']").val() + "</td>"
    //            + "<td align='center'>" + $(tableRows[i - 1]).find("input[name='count']").val() + "</td>"
    //            + "<td align='center'>" + subTotal + "</td>"
    //            + "</tr>";

    //        $("#print_body").append(appendBody);

    //        total += subTotal;
    //        totalCount += count;
    //    }

    //    $("#print_body").append("<tr'><td style='border-width: 0px;' rowspan='2' align='center'><b>合计 : </b></td><td style='border-width: 0px;' rowspan='2' colspan='3'></td><td align='center' style='border-width: 0px;' rowspan='2'>"
    //        + totalCount + "</td><td align='center' style='border-width: 0px;' rowspan='2'>" + total + "</td></tr>");

    //    $('#print_total').val("合计金额 : " + convert_shuzi(total.toString()));

    //	pageprint();
    //}

</script>
<%--<script type="text/javascript">
	var initBody;
	function beforePrint()
	{		
	} 
	function afterPrint()
	{
		$("#header").css("display", "block");
		$("#tabs-1").css("display", "block");
		$(".ui-tabs-nav").css("display", "block");
		$("#print_page").css("display", "none");
	} 
	function pageprint()
	{		
		window.onbeforeprint = beforePrint; 
		window.onafterprint = afterPrint; 
		$("#header").css("display", "none");
		$("#tabs-1").css("display", "none");
		$(".ui-tabs-nav").css("display", "none");
		$("#print_page").css("display", "block");
		window.print();
		$("#header").css("display", "block");
		$("#tabs-1").css("display", "block");
		$(".ui-tabs-nav").css("display", "block");
		$("#print_page").css("display", "none");
	} 
</script>--%>

