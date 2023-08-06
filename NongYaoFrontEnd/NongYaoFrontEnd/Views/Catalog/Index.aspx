<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="NongYaoFrontEnd.Models" %>
<%@ Import Namespace="NongYaoFrontEnd.Models.Library" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
<form action="#" id="submit_form_catalog" class="form-horizontal form-validate">
     <div id="ctlg_dialog_ok" class="modal fade" data-width="600">              
        <div class="modal-content">
            <div class="modal-head">
                <h4>提示</h4>
            </div>
            <div class="modal-body">
                <h4>当前农药其他商家尚未提交申请，您可以提交申请！</h4>
            </div>
            <div class="modal-footer">  
                <center>                           
                     <button type="button" data-dismiss="modal" class="btn_rect">继续</button>
                </center>
            </div>
        </div>                      
    </div>
   
     <div id="ctlg_dialog_exist" class="modal fade" data-width="600">              
        <div class="modal-content">
            <div class="modal-head">
                <h4>提示</h4>
            </div>
            <div class="modal-body">
                <h4>当前农药其他商家已提交申请，请等待申请通过！</h4>
            </div>
            <div class="modal-footer">  
                <center>                           
                     <button type="button" data-dismiss="modal" class="btn_rect">返回</button>
                </center>
            </div>
        </div>                      
    </div>
    
     <div id="ctlg_dialog_pass" class="modal fade" data-width="600">              
        <div class="modal-content">
            <div class="modal-head">
                <h4>提示</h4>
            </div>
            <div class="modal-body">
                <h4>当前农药已通过申请，需要采购可以直接选择此农药进货！</h4>
            </div>
            <div class="modal-footer">  
                <center>                           
                    <button type="button" data-dismiss="modal" class="btn_rect">返回</button>
                </center>
            </div>
        </div>                      
    </div>

    <table class="receive_table" cellspacing="20" border="0">        
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药登记证号:<span class="required">*</span></label>
            </td>
            <td valign="top" class="sub_table">
                <table cellspacing="0" cellpadding="0">
                    <tr>
                         <td>
                           <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                       <input type=text name="ctlg_register" id="ctlg_register" placeholder="请输入农药登记证号" value="" class="form-control">                                      
                                     </div>
                               </div>
                           </div> 
                          </td>
                        <td>
                           <div class="modal-body">
                                <div class="form-group">                                                                        
                                    <button type="submit" id = "ctlg_check"  class="btn_tabright"  data-toggle="modal" onclick = "check_nongyao()">当前申请查询</button> 
                                    <button type="button"  style ="display:none"  id = "ctlg_check1" data-target = "#ctlg_dialog_pass"  class ="btn_tabright" data-toggle="modal" ></button>
                                    <button type="button" style ="display:none" id = "ctlg_check2" data-target = "#ctlg_dialog_exist" class ="btn_tabright"  data-toggle="modal" ></button> 
                                    <button type="button" style ="display:none" id = "ctlg_check0" data-target = "#ctlg_dialog_ok" class ="btn_tabright"  data-toggle="modal" ></button>                                    
                               </div>
                           </div> 
                          </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药生产许可证号:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_permit" id="ctlg_permit" value="" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药产品标准号\文件:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_sample" id="Text2" value="" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药名称:<span class="required">*</span></label>
            </td>
            <td valign="top" class="sub_table">
                <table cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type=text name="ctlg_name" id="ctlg_name" value="" style="width: 245px" class="form-control" onkeyup="changeNongyaoName();">
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td valign="top">
                            <label class="control-label col-md-3 title" for="name">名称拼音简码:<span class="required">*</span></label>
                        </td>
                        <td valign="top">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type=text id="ctlg_nickname" value="" style="width: 151px" class="form-control" disabled />
                                        <input type="hidden" name="ctlg_nickname" id="real_ctlg_nickname" />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    
        <!--
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药通用名称:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_usingname" id="ctlg_usingname" value="" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药批号:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_pihao" id="ctlg_pihao" value="" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        -->
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">厂商名:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_product" id="ctlg_product" value="" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">剂型:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_shape" id="Text3" value="" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">有效成分:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_material" id="Text4" value="" placeholder="有效成分" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">含量:<span class="required">*</span></label>
            </td>
            <td valign="top"  class="sub_table">
                <table cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <input type=text name="ctlg_content" id="Text5" value="" placeholder="含量" class="form-control" style="width: 245px">                                                                    
                                    </div>
                                </div>                    
                            </div>
                        </td>
                        <td>%</td>
                    </tr>
                 </table>
            </td>
        </tr>
        <!--
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">单位:<span class="required">*</span></label>
            </td>
            <td valign="top" class="sub_table">
                <table cellspacing="0" cellpadding="0" style="width:98%;">
                    <tr>
                        <td valign="top">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                    <select name="ctlg_unit" id="ctlg_unit" class="form-control" style="width: 245px" onchange="changeUnit();">
                                        <%foreach (UnitInfo item in (List<UnitInfo>)ViewData["unitList"]) 
                                          {%> <option value="<%=item.id%>"><%=item.name %></option>
                                        <%} %>
		                            </select>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td valign="top">
                            <label class="control-label col-md-3 title" for="name">规格:<span class="required">*</span></label>
                        </td>
                        <td valign="top">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div>
                                        <input type=text name="ctlg_standard" id="ctlg_standard" value="" class="form-control"  style="width: 130px" />
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td valign="top">
                            <div class="modal-body">
                                <div class="form-group">
                                    <div >
                                        <input type=text id="standard_unit" value="" class="form-control"  style="width: 70px" disabled/>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    -->

        <!--  <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">农药价格:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_price" id="ctlg_price" value="" placeholder="7.8元" class="form-control">
                        </div>
                    </div>
                </div>
            </td>
        </tr>-->
      
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">是否高毒:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <%--<input class="input_radio" type="radio" name="ctlg_highlevel" id="ctlg_highlevel" value="1">
                            是&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input class="input_radio" type="radio" name="ctlg_rowlevel" id="ctlg_rowlevel" value="0">
                            否--%>

                            <div id="mark_div" class="make-switch" data-on-label="&nbsp;是&nbsp;" data-off-label="&nbsp;否&nbsp;" style="width:110px;">
						                        <input type="checkbox" name="ctlg_level" id="ctlg_level" class="toggle"/>
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">产地:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_productarea" id="ctlg_productarea" value="" class="form-control"/>
                        </div>
                    </div>
                </div>
            </td>
        </tr> 
         
        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">创建人:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_username" id="ctlg_username" value="<%=ViewData["userName"] %>" disabled class="form-control"/>
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">创建时间:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <input type=text name="ctlg_regtime" id="ctlg_regtime" value="<%=ViewData["curdate"]%>" disabled class="form-control"/>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
           

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title" for="name">使用说明:<span class="required">*</span></label>
            </td>
            <td>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="col-md-4">
                            <textarea class="form-control col-md-4" name="ctlg_description" id="ctlg_description" style="max-width:600px;min-width:600px;"></textarea>
                        </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <label class="control-label col-md-3 title">农药图片:<span class="required">*</span></label>
            </td>
            <td>
                <div class="form-group">
                    <div class="col-md-4">
                        <%--<input type="hidden" id="imgpath" name="imgpath" />
                        <div id="img1" style="padding:5px 5px 5px 0px;">
                        <img src="" style="width:50px; height:50px;">  
                        </div>
                        <input type=button class="btn btn-small btn-primary" id='upload_btn' value="选择头像图片">--%>
                        <input type="hidden" id="imgpath" name="imgpath" value="<% if (ViewData["imgpath"] != null) { %><%= ViewData["imgpath"] %><% } %>" />
                    <input type="button" class="btn btn-upload" id='upload_btn' value="农药图片" style="width:150px">
                    <img src="<%= ViewData["rootUri"] %>Content/img/ajax_loader.gif" style="display:none;" id="loading_photo">

                    <div id="img1" style=" padding:5px;">
                        <% if (ViewData["imgpath"] != null && !String.IsNullOrEmpty(ViewData["imgpath"].ToString()))
                            { %>
                            <img src="<%= ViewData["rootUri"] %> <%= ViewData["imgpath"] %>" 
                                style="width:100px; height:100px;">                         
                        <% } %>
                    </div>
                    </div>
                </div>
            </td>
        </tr>

        <tr>
            <td colspan=2 align="center">
                <button class="receive_table_button" type="button" onclick="Submit_Catalog()">保存</button>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <button class="receive_table_button" type="button" onclick="Clear_FormInputs()">清空重填</button>
            </td>
        </tr>
    </table>
</form>
    <input type="hidden" id="crop_x" name="x" />
	<input type="hidden" id="crop_y" name="y" />
	<input type="hidden" id="crop_w" name="w" />
	<input type="hidden" id="crop_h" name="h" />
    <div id="ajax-modal" class="modal fade" tabindex="-1">
    </div>
</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="PageStyle" runat="server">    
    <link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/style.css" type="text/css" />
    <link rel="stylesheet" href="<%= ViewData["rootUri"] %>Content/css/style1.css" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/select2/select2_metro.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />


    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="<%= ViewData["rootUri"] %>Content/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />

	<link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-switch/static/stylesheets/bootstrap-switch-metro.css" rel="stylesheet" type="text/css"/>
    <link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css" rel="stylesheet" type="text/css"/>
	<link href="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-modal/css/bootstrap-modal.css" rel="stylesheet" type="text/css"/>
	<link href="<%= ViewData["rootUri"] %>Content/plugins/jcrop/css/jquery.Jcrop.min.css" rel="stylesheet" type="text/css"/>
	<link href="<%= ViewData["rootUri"] %>Content/css/pages/image-crop.css" rel="stylesheet" type="text/css"/>
    <link href="<%= ViewData["rootUri"] %>Content/css/style-metronic.css" rel="stylesheet" type="text/css" />


    
	
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageScripts" runat="server">
    <%--<script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/select2/select2.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootbox/bootbox.min.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/js/ui/jquery.ui.tabs.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-modal/js/bootstrap-modal.js"></script>
	<script type="text/javascript" src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-modal/js/bootstrap-modalmanager.js"></script>


    <script src="<%= ViewData["rootUri"] %>Content/scripts/ajaxupload.js"></script>
    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js"></script>--%>

    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.blockui.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/uniform/jquery.uniform.min.js")" type="text/javascript"></script>    

    <script src="<%= ViewData["rootUri"] %>Content/plugins/select2/select2.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootbox/bootbox.min.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/data-tables/DT_bootstrap.js" type="text/javascript"></script>
    <script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-toastr/toastr.js" type="text/javascript"></script>
    
	<script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/jquery-validation/dist/additional-methods.min.js" type="text/javascript"></script> 
        
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-switch/static/js/bootstrap-switch.min.js" type="text/javascript" ></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>
                
	<script src="<%= ViewData["rootUri"] %>Content/plugins/chosen-bootstrap/chosen/chosen.jquery.min.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-wysihtml5/wysihtml5-0.3.0.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-modal/js/bootstrap-modal.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/bootstrap-modal/js/bootstrap-modalmanager.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/jcrop/js/jquery.color.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/plugins/jcrop/js/jquery.Jcrop.min.js" type="text/javascript"></script>
	<script src="<%= ViewData["rootUri"] %>Content/scripts/ajaxupload.js" type="text/javascript"></script>

    <script src="<%= ViewData["rootUri"] %>Content/scripts/app.js")" type="text/javascript"></script>
    <script>
        var $modal = $('#ajax-modal');
        jQuery(document).ready(function () {
            // initiate layout and plugins
            $("#menu_tb").removeClass("active");
            $("#catalog_data").addClass("active");
            App.init();
           // changeUnit();

            $modal = $('#ajax-modal');
            handleCropModal();
            var form_catalog = $('#submit_form_catalog');
            error = $('.alert-danger', form_catalog);
            success = $('.alert-success', form_catalog);

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

            function is_permitid(value) {
                return /^[a-zA-Z0-9]+[\-a-zA-Z0-9]*[a-zA-Z0-9]+$/.test(value) || /^[a-zA-Z0-9]+$/.test(value);
            }

            function is_alphanumber(value) {
                return /^[a-zA-Z0-9]+$/.test(value);
            }

            $.validator.addMethod("is_permitid", is_permitid, "请输入英文字,数字");
            $.validator.addMethod("is_alphanumber", is_alphanumber, "请输入英文字,数字");

            form_catalog.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    ctlg_permit: {
                        required: true,                        
                        is_permitid: true
                    },
                    ctlg_register: {
                        required: true,                        
                    },
                    ctlg_sample: {
                        required: true,
                    },
                    ctlg_material: {
                        required: true,
                    },
                    ctlg_content: {
                        required: true,
                        number: true
                    },
                    ctlg_shape: {
                        required: true,
                    },
                    ctlg_name: {
                        required: true,
                    },                  
                    ctlg_nickname: {
                        required: true,
                        maxlength: 10
                    },               
                    ctlg_product: {
                        required: true,
                    },                  
                    ctlg_productarea: {
                        required: true
                    },
                    ctlg_description: {
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

            /*---------- Ajax Image Upload setup ---------*/
            
        });

        var $modal = $('#ajax-modal');
        function handleCropModal(){
            new AjaxUpload('#upload_btn', {
                action: rootUri + 'Upload/UploadImage',
                onSubmit: function (file, ext) {
                    $('#loading_photo').show();
                    if (!(ext && /^(JPG|PNG|JPEG|GIF)$/.test(ext.toUpperCase()))) {
                        // extensiones permitidas
                        alert('错误: 只能上传图片', '');
                        $('#loading_photo').hide();
                        return false;
                    }
                },
                onComplete: function (file, response) {
                    //alert(file + " | " + " | " + response);
                    var f_name = response;
                    $('#loading_photo').hide();
                    showCropDialog(f_name);
                }
            });

            /*---------- Image Crop Dialog setup ---------*/
            $.fn.modalmanager.defaults.resize = true;
            $.fn.modalmanager.defaults.spinner = '<div class="loading-spinner fade" style="width: 200px; margin-left: -100px;"><img src="' + rootUri + 'Content/img/ajax-modal-loading.gif" align="middle">&nbsp;<span style="font-weight:300; color: #eee; font-size: 18px; font-family:Open Sans;">&nbsp;Loading...</span></div>';
        }

        function showCropDialog(fname) {
            // create the backdrop and wait for next modal to be triggered
            $('body').modalmanager('loading');
            setTimeout(function () {
                $modal.load(rootUri + "Upload/RetrieveCropDialogHtml?cropfile=" + fname, '', function () {
                    cropimage();
                    $modal.modal({
                        backdrop: 'static',
                        keyboard: true,
                        width: parseInt($("#imgcrop").css("width"), 10) + 400
                    })

                        .on("hidden", function () {
                            $modal.empty();
                        });
                });
            }, 100);
        }

        var cropimage = function () {
            // Create variables (in this scope) to hold the API and image size
            //alert(parseInt($("#imgcrop").css("width"), 10) + 300);
            //$("#ajax-modal").css("width", parseInt($("#imgcrop").css("width"), 10) + 300);
            var jcrop_api,
                boundx,
                boundy,
                // Grab some information about the preview pane
                $preview = $('#preview-pane'),
                $pcnt = $('#preview-pane .preview-container'),
                $pimg = $('#preview-pane .preview-container img');

            $pcnt.width(150);
            $pcnt.height(150);

            var xsize = $pcnt.width(),
                ysize = $pcnt.height();

            //console.log('init', [xsize, ysize]);

            $('#imgcrop').Jcrop({
                onChange: updatePreview,
                onSelect: updatePreview,
                aspectRatio: Math.round(150 / 150),
                setSelect: [0, 0, 150, 150]
            }, function () {
                // Use the API to get the real image size
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];
                // Store the API in the jcrop_api variable
                jcrop_api = this;
                // Move the preview into the jcrop container for css positioning
                $preview.appendTo(jcrop_api.ui.holder);
            });

            function updatePreview(c) {
                $('#crop_x').val(c.x);
                $('#crop_y').val(c.y);
                $('#crop_w').val(c.w);
                $('#crop_h').val(c.h);

                if (parseInt(c.w) > 0) {
                    var rx = xsize / c.w;
                    var ry = ysize / c.h;

                    $pimg.css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });
                }
            };
        }

        var submitCrop = function (cropfile) {
            $modal.modal('loading');
            $.ajax({
                url: rootUri + "Upload/ResizeImage",
                data: {
                    x: $('#crop_x').val(),
                    y: $('#crop_y').val(),
                    w: $('#crop_w').val(),
                    h: $('#crop_h').val(),
                    imgpath: cropfile,
                    kind: "<%= UpImageCategory.NEWS %>",
                    size: "<%= CropImageSizes.NEWS %>"
                },
                type: "POST",
                success: function (rst) {
                    $modal.modal('loading');
                    if (rst == "") {
                        $modal.find('.modal-body')
		                    .prepend('<div class="alert alert-error fade in">' +
		                    '操作失败：原图不存在！<button type="button" class="close" data-dismiss="alert"></button>' +
		                    '</div>');
                    } else {
                        var str_html = "<img src='" + rootUri + rst + "' style='width:100px; height:100px;'>";
                        $('#img1').html(str_html);
                        $('#imgpath').val(rst);
                        $modal.modal('hide');
                    }
                }
            });
        }

        function redirectToListPage(status) {
            if (status.indexOf("error") != -1) {
            } else {
                window.location = rootUri + "Catalog/Index";
            }
        }

        function Clear_FormInputs()
        {
            window.location = rootUri + "Catalog/Index";
        }

        function Submit_Catalog()
        {
            if ($('#submit_form_catalog').valid()) {
                $('.alert-danger').hide();

                $("#real_ctlg_nickname").val($("#ctlg_nickname").val());

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "<%= ViewData["rootUri"] %>Catalog/SubmitCatalog",
                    dataType: "json",
                    data: $('#submit_form_catalog').serialize(),
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

                        }
                    },
                    error: function (data) {
                        alert("Error: " + data.status);
                        $('.loading-btn').button('reset');
                    }
                });
                
            }

        }

        /*function changeUnit() {
            var val = $("#ctlg_unit").val();
            if (val == 1)
                $("#standard_unit").val("毫升");
            else
                $("#standard_unit").val("克");
        }*/

        function check_nongyao() {
            var html = "";

            var registerid = $("#ctlg_register").val();
            if (registerid.length > 0) {
                $.ajax({
                    url: "<%= ViewData["rootUri"] %>Catalog/CheckCatalogPass",
                    data: {
                        ctl_register: registerid
                    },
                    success: function (data) {
                        var pass = data.pass;
                        if (pass == 1) {
                            $("#ctlg_check1").trigger("click");
                        } else if (pass == 2 || pass == 0) {
                            $("#ctlg_check2").trigger("click");
                        } else {
                            $("#ctlg_check0").trigger("click");
                        }

                    }
                });
            }
            else {
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
                toastr["error"](0, "必须要填写");
            }

           
        }

        function changeNongyaoName() {
            var nongyaoname = $("#ctlg_name").val();
            $.ajax({
                url: "<%= ViewData["rootUri"] %>Account/GetPinyinCode",
                data: {
                    originStr: nongyaoname
                },
                success: function (data) {
                    $("#ctlg_nickname").val(data.pinyincode);
                }
            });
        }
        
    </script>
</asp:Content>
