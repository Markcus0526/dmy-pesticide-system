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
        <table class="table_edit" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 180px;"></td>
                <td style="width: 80px;">
                    <label class="control-label title" style="margin-top:5px">仓库名称:</label>
                </td>
                <td style="width: 380px;">
                    <select class="form-control" name="StoreNameList" id="StoreNameList" onchange="InventCatalog()">
                        <option value="全部">全部</option>
                        <%foreach (tbl_store strItem in (List<tbl_store>)ViewData["StoreNameList"])
                          {%><option value="<%=strItem.id %>"><%=strItem.name %></option>
                        <%} %>
                    </select>
                </td>
                <td style="text-align:right">
                    <button type="submit" class="btn_action" onclick="printTable();" style="margin-right:0px;">打印</button>                    
                </td>
                <!--<td>
                    <button type="submit" class="btn_action" onclick="InventCatalog()">搜索</button>
                </td>-->
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td valign="top" width="15%" style="text-align: left;">
                    <div id="treeview"></div>
                </td>
                <td valign="top">
                    <div id="portlet">
                        <div class="portlet-body" style="display: block;">
                            <div id="table-responsive">
                                <table class="table table-striped  table-advance table-bordered table-hover tinyfont" id="tblinventory">
                                    <thead>
                                        <tr style="height:60px;">
                                            <th style="text-align: center; vertical-align:middle; width:85px; font-size:13px">农药登记证号</th>
                                            <th style="text-align: center; vertical-align:middle; width:70px; font-size:13px">农药名称</th>
                                            <th style="text-align: center; vertical-align:middle; width:75px; font-size:13px">农药通用名</th>
                                            <th style="text-align: center; vertical-align:middle; width:70px; font-size:13px">产品规格</th>
                                            <th style="text-align: center; vertical-align:middle; width:30px; font-size:13px">單位</th>
                                            <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">供应商</th>
                                            <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">生产日期</th>
                                            <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">有效期</th>
                                            <th style="text-align: center; vertical-align:middle; width:65px; font-size:13px">产品批号</th>
                                            <th style="text-align: center; vertical-align:middle; width:50px; font-size:13px">单价</th>
                                            <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">库存量</th>
                                            <th style="text-align: center; vertical-align:middle; width:40px; font-size:13px">金额</th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size:12px">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
		<table class="table_bottom">
			<tr>
                <td style="text-align:right">
                    总金额:&nbsp;&nbsp;&nbsp;
                </td>
		    	<td style="width:20%">
                    <input type="text" id="total_price" disabled class='form-control' value="<% =ViewData["total_price"] %>"  />
		    	</td>
                <td>
                    <span style="float:left;">元</span>
                </td>
                <td style="text-align:right">
                    库存总量:&nbsp;&nbsp;&nbsp;
                </td>
		    	<td style="width:20%">
                    <input type="text" id="total_remain" disabled class='form-control' value="<% =ViewData["total_remain"] %>"  />
		    	</td>
                <td>
                    <span style="float:left;">千克</span>
                </td>
			</tr>
		</table>
    </div>

<div id="print_page_inventory" style="display: none; font-size: 16px;">
	<p style="text-align:center;"><font style="font-size: 24px;"><b>库存查询表</b></font></p><br/>
	<table class="table_view" id="print_tblinventory" border="1">
          <thead>
            <tr>
                <th style="text-align: center; vertical-align:middle; width:85px; font-size:13px">农药登记证号</th>
                <th style="text-align: center; vertical-align:middle; width:70px; font-size:13px">农药名称</th>
                <th style="text-align: center; vertical-align:middle; width:75px; font-size:13px">农药通用名</th>
                <th style="text-align: center; vertical-align:middle; width:70px; font-size:13px">产品规格</th>
                <th style="text-align: center; vertical-align:middle; width:30px; font-size:13px">單位</th>
                <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">供应商</th>
                <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">生产日期</th>
                <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">有效期</th>
                <th style="text-align: center; vertical-align:middle; width:65px; font-size:13px">产品批号</th>
                <th style="text-align: center; vertical-align:middle; width:50px; font-size:13px">单价</th>
                <th style="text-align: center; vertical-align:middle; width:60px; font-size:13px">库存量</th>
                <th style="text-align: center; vertical-align:middle; width:40px; font-size:13px">金额</th>
            </tr>
        </thead>
		<tbody id="print_tbody_inventory" style="background: white; border: 0px;">
		</tbody>
	</table>

    <span style="float: left; margin-left: 15px;">仓库名称:
            <span id="print_storename"></span>
    </span>
	<span style="margin-left: 15px;">库存总量:
        <span id="print_total_remain"><% =ViewData["total_remain"]%></span><span>千克</span>
    </span>
    <span style="float: right; margin-right: 15px;">总金额:
        <span id="print_total_price"><% =ViewData["total_price"]%></span><span>元</span>
    </span>
</div> 
    <!-- END PAGE LEVEL SCRIPTS -->
    <script>

        var inventoryTable;
        var print_inventoryTable;
        
        function handleInventoryDataTable() {
            if (inventoryTable!=null) {
                return;
            }

            // begin first table
            inventoryTable = $('#tblinventory').dataTable({
                "bServerSide": true,
                "bProcessing": true,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Store/RetrieveCatalogList",
	            "aoColumns": [
                  { "bSortable": false },
                  { "bSortable": false },
                  { "bSortable": false }, 
                  { "bSortable": false },
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
                        
                    }
	            ],
//                "fnDrawCallback":function(oSettings) {
//                    if (print_inventoryTable==null)
//                    {
//                        make_print_supply_table();
//                    }
//                    else{
//                        print_inventoryTable();

//                    }

//			    },
                "fnInitComplete":function(oSettings) {
                    if (print_inventoryTable==null)
                    {
                        make_print_supply_table();
                    }
                    else{
                        print_inventoryTable();

                    }

			    }
	        });
	    }
        function  make_print_supply_table(){
            if (print_inventoryTable!=null) {
                return;
            }
            print_inventoryTable=$('#print_tblinventory').dataTable({
                "bServerSide": true,
                "bProcessing": false,
                "bPaginate" :false,
                "bInfo":false,
                "sAjaxSource": "<%= ViewData["rootUri"] %>Store/RetrievePrintCatalogList",
	            "aoColumns": [
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false }
	            ],
	            // set the initial value
	            // "iDisplayLength": 10,
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
                        
                    }
	            ]
	        });
	    }
        var treeview;
        var treeDataSource = [];
        function initTree() {

            $.ajax({
                type: "GET",
                url: "<%= ViewData["rootUri"] %>/Store/RetrieveAgrochemicalType",
	            dataType: 'json',
	            data: {},
	            success: function (data) {
	                treeDataSource = [
                    {
                        text: "农药类别", expanded: true, items: []
                    }
	                ];

	                var middleTypes = [];
	                var lastTypes = [[]];

	                middleTypes = data.middle;
	                lastTypes = data.last;

	                for (var i = 0; i < middleTypes.length; i++) {
	                    var lastItems = [];
	                    for (var j = 0; j < lastTypes[i].length; j++) {
	                        lastItems = lastItems.concat([{ text: lastTypes[i][j] }]);
	                    }

	                    treeDataSource[0].items = treeDataSource[0].items.concat([{ text: middleTypes[i], expanded: true, items: lastItems }]);
	                }

	                treeview = $("#treeview").kendoTreeView({
	                    dataSource: treeDataSource,
	                    select: treeSelect
	                }).data("kendoTreeView"),
                    append = function (e) {
                        if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode) {
                            var selectedNode = treeview.select();

                            // passing a falsy value as the second append() parameter
                            // will append the new node to the root group
                            if (selectedNode.length == 0) {
                                selectedNode = null;
                            }

                            treeview.append({
                                text: $("#text").val()
                            }, selectedNode);
                        }
                    };
	            }
	        });
        }
        function treeSelect(e) {            
            InventCatalogbyName(treeview.text(e.node));
            //alert("tree selected" + treeview.text(e.node));
        }
        function refreshTree() {
            treeDataSource = [
                    {
                        text: "农药类别", expanded: true, items: []
                    }
            ];

            $.ajax({
                type: "GET",
                url: "<%= ViewData["rootUri"] %>/Store/RetrieveAgrochemicalType",
	            dataType: 'json',
	            data: {},
	            success: function (data) {
	                var middleTypes = [];
	                var lastTypes = [[]];

	                middleTypes = data.middle;
	                lastTypes = data.last;

	                for (var i = 0; i < middleTypes.length; i++) {
	                    var lastItems = [];
	                    for (var j = 0; j < lastTypes[i].length; j++) {
	                        lastItems = lastItems.concat([{ text: lastTypes[i][j] }]);
	                    }

	                    treeDataSource[0].items = treeDataSource[0].items.concat([{ text: middleTypes[i], expanded: true, items: lastItems }]);
	                }
	            }
	        });
        }
        function refresh_double_table(count){
            refreshInventoryTable();            
            setTimeout(  function(){$.ajax({
                    url: rootUri + "Store/RefreshTotal_remain",
                    async:false,
                    success: function (data) {
                       if (data!=null){
                            $("#total_remain").val(data);
                            count=4;
                       }
                       else{
                            count=4;
                       }
                    }
                });
            

                $.ajax({
                    url: rootUri + "Store/RefreshTotal_price",
                    async:false,
                    success: function (data) {
                       if (data!=null){
                            $("#total_price").val(data);
                       }
                    }
                });},1000);
     //               refreshPrintInventoryTable();
        }

        function refreshInventoryTable() {
            oSettings = inventoryTable.fnSettings();
            inventoryTable.fnDraw();
        }

        function refreshPrintInventoryTable() {
            oSettings = print_inventoryTable.fnSettings();
            print_inventoryTable.fnDraw();
        }

        function InventCatalog() {
            var selectedNode = treeview.select();

            var name = $(selectedNode.find(".k-in")[0]).text();

            $.ajax({
                url: rootUri + "Store/InventByStore",
                data: {
                    storeId: $("#StoreNameList").val(),
                    nongyaoName: name
                },
                success: function (data) {
                    refresh_double_table(1);
                }
            });
        }
        function InventCatalogbyName(name) {
            $.ajax({
                url: rootUri + "Store/InventByStore",
                data: {
                    storeId: $("#StoreNameList").val(),
                    nongyaoName: name
                },
                success: function (data) {
                    refresh_double_table(1);
                //    refreshInventoryTable();
                //    refreshPrintInventoryTable();
                }
            });
        }
        jQuery(document).ready(function () {
            initTree();
            handleInventoryDataTable();
            //InventCatalogbyName("农药类别");
        });
    </script>

        <script type="text/javascript">

            function printTable() {
                if ($("#print_page_inventory tbody").find("tr") == null) {
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
                //make_print_supply_table();
                refreshPrintInventoryTable();
                $("#print_storename").html($("#StoreNameList").find("option:selected").text());
                $("#print_total_remain").html($("#total_remain").val());
                $("#print_total_price").html($("#total_price").val());
                setTimeout(function () {
                    pageprint();
                }, 1000);
            }

            function beforePrint() {
            }
            function afterPrint() {
                $("#header").css("display", "block");
                $("#tabs-1").css("display", "block");
                $(".ui-tabs-nav").css("display", "block");
                $("#print_page_inventory").css("display", "none");
            }
            function pageprint() {
                window.onbeforeprint = beforePrint;
                window.onafterprint = afterPrint;
                $("#header").css("display", "none");
                $("#tabs-1").css("display", "none");
                $(".ui-tabs-nav").css("display", "none");
                $("#print_page_inventory").css("display", "block");
                window.print();
                $("#header").css("display", "block");
                $("#tabs-1").css("display", "block");
                $(".ui-tabs-nav").css("display", "block");
                $("#print_page_inventory").css("display", "none");
            }

        </script>
</body>
</html>
