//JQuery DataTable
var TableManaged = {
    oTable: '',
    iDisplayLength : 15,

    init: function (dataTableObj, actionUrl, aoColumns, pageSize) {
        
        if (!jQuery().dataTable) {
            return;
        }

        if (typeof (pageSize) != 'undefined') {
            this.iDisplayLength = pageSize;
        }

        this.oTable = dataTableObj.dataTable({
            "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>",//定义DataTable布局的一个强大属性
            "bInfo": true,
            "bProcessing": true,
            "bAutoWidth": false,
            "bJQueryUI": false,
            'bLengthChange': true,
            "bFilter": false,
            "bSort": false,
            "bPaginate": true,
            "bServerSide": true,
            "bStateSave": false,
            "iDisplayLength": 15,
            "sPaginationType": "bootstrap",
            "aLengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
            "oLanguage": {
                "sLengthMenu": "每页显示 _MENU_ 条记录",
                "sZeroRecords": "对不起，查询不到任何相关数据",
                "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
                "sInfoEmtpy": "找不到相关数据",
                "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
                "sProcessing": "正在加载中...",
                "sSearch": "搜索",
                "oPaginate": {
                    "sFirst": "第一页",
                    "sPrevious": " 上一页 ",
                    "sNext": " 下一页 ",
                    "sLast": " 最后一页 "
                }
            },
            "sAjaxSource": actionUrl,
            "aoColumns": aoColumns,
            "aoColumnDefs": [{
                'bSortable': false,
                'aTargets': [0]
            }]
        });
        return this.oTable;        
    },

    search: function (searchUrl) {
        var filterdata = $("#search_form").serialize();
        var actionUrl = searchUrl + "?rand=" + Math.random() + "&" + filterdata;
        this.oTable.fnReloadAjax(actionUrl);
    },
    //清除表单数据
    reset:function(obj){
        obj.find(':input').not(':button, :submit, :reset').val('').removeAttr('checked').removeAttr('selected');
        var slist = obj.find('select');
        for (var i = 0; i < slist.length; i++) {
            if (slist[i].options.length > 0) {
                slist[i].selectedIndex = 0;
            }
        }
    },
    //编辑
    btnEdit: function (actionUrl, id) {
        return "<a href=\"javascript:void(0);\" class=\"btn btn-xs purple\" onclick=\"Edit('"+actionUrl+"','"+id+"')\"><i class=\"fa fa-edit\"></i> 编辑</a>";
    },
    //删除
    btnDelete: function (actionUrl, id) {
        return "<a href=\"javascript:void(0)\" class=\"btn btn-xs red\" onclick=\"TableManaged.confirmDelete(this, '" + actionUrl + "', '"+ id +"')\"><i class=\"fa fa-times\"></i> 删除</a>";
    },

    confirmDelete: function (target, actionUrl, id) {
        if (!confirm("你确认要删除此记录？")) {
            return;
        }

        $.ajax({
            type: "POST",
            url: actionUrl,
            data: {id : id},
            success: function(response) {
                if (response.success) {
                    var nRow = $(target).parents('tr')[0];
                    TableManaged.oTable.fnDeleteRow(nRow);
                } else {
                    alert(response.message);
                }

            }
        });

    }
};

$('.datatable .group-checkable').change(function () {
    var set = jQuery(this).attr("data-set");
    var checked = jQuery(this).is(":checked");
    jQuery(set).each(function () {
        if (checked) {
            $(this).attr("checked", true);
        } else {
            $(this).attr("checked", false);
        }
    });
    jQuery.uniform.update(set);
});

/*******弹出表单*********/
function ShowModal(actionUrl, param, title) {
    $("#modal-form").draggable({
        cursor: "move",
        handle: ".modal-header"
    });
    //表单初始化
    $(".modal-title").html(title);
    $("#modal-content").attr("action", actionUrl);

    $.ajax({
        type: "GET",
        url: actionUrl,
        data: param,
        beforeSend: function () {
            //
        },
        success: function (result) {
            $("#modal-content").html(result);
            $('#modal-form').modal('show');
            RegisterForm();
        },
        error: function () {
            //
        },
        complete: function () {
            //
        }
    });
}

/*******保存表单*********/
function SaveModal(oTable) {
    var actionUrl = $("#modal-content").attr("action");
    var $form = $("#modal-content");

    if (!$form.valid()) {
        return;
    }

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: $form.serialize(),
        success: function (result) {
            //判断返回值，若为Object类型，即操作成功
            //var result = ((typeof data == 'object') && (data.constructor == Object));
            if (result != null) {
                //bootbox.alert(data.Message);
                $('#modal-form').modal('hide');
                oTable.fnDraw();
            }
            else {
                $("#modal-content").html(data);
            }
        }
    });
}

/*******注册验证脚本*********/
function RegisterForm() {
    $('#modal-content').removeData('validator');
    $('#modal-content').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('#modal-content');
}