//选择主题及初始化主题逻辑
//(function () {
//    $(".theme-colors > ul > li").click(function () {
//        var color = $(this).attr("data-style");
//        $.cookie('currentTheme', color, { expires: 7, path: '/' });
//    });
//    var currentTheme = $.cookie('currentTheme');
//    if (currentTheme != null && currentTheme) {
//        $('#style_color').attr("href", "/administration/content/assets/css/themes/" + currentTheme + ".css");
//    }
//})();

//表单验证
(function () {
    $(".form-horizontal .field-validation-error").each(function () {
        if ($(this).html() != "") {
            $(this).parent().parent().parent().addClass("has-error");
        }
    });
})();

////TODO:新菜单根据Url决定逻辑, 目前适配二级菜单
//新菜单根据Url决定逻辑, 目前适配多级菜单
(function () {
    var locationHref = window.location.href.toLocaleLowerCase();
    var strhtml = "";
    var index = 0;
    $(".page-sidebar-menu > li > a").each(function () {
        var parent = $(this);
        //sideMenus(locationHref, parent, strhtml, index);
        
        $(this).next("ul").each(function () {
            $("a", $(this)).each(function () {
                if (locationHref.indexOf($(this).attr("key")) > 0) {
                    var htmlTemplate = new Object();
                    sideMenus($(this).parent(), $(this), 0, htmlTemplate);
                    
                    var breads = htmlTemplate.con.split("<jf>");
                    var breadstr = "";
                    for (var i = breads.length ; i > 0; i--) {
                        breadstr += breads[i - 1];
                    }
                    $("#navigation .breadcrumb").append(breadstr);
                    
                    document.title = $(this).text() + " - " + document.title;

                    return false;
                }
            });
        });
        if (locationHref.indexOf($(this).attr("key")) > 0) {
            $(this).parent().addClass("active");
            $(this).append("<span class='selected'></span>");

            $("#navigation .page-title span").html($(this).text());
            $("#navigation .page-title small").html($(this).attr("title") || "");

            var htmlTemplate = "";            
            htmlTemplate += "<li>";
            htmlTemplate += "<i class='" + $(this).find("i").attr("class") + "'>" + "</i>";
            htmlTemplate += "<a href='" + $(this).attr("href") + "'>" + $(this).attr("title") + "</a>";
            htmlTemplate += "</li>";
            $("#navigation .breadcrumb").append(htmlTemplate);

            document.title = $(this).text() + " - " + document.title;
            return false;
        }
    });
})();

function sideMenus(objli, obja, i, htmlTemplate) {
    objli.addClass("active");
    $(".arrow", objli).addClass("open")    
    //$(".arrow", objli).addClass("open").before("<span class='selected'></span>");
    if (i == 0) {
        htmlTemplate.con = "<li>";
        htmlTemplate.con += "<i class='" + obja.find("i").attr("class") + "'>" + "</i>";
        htmlTemplate.con += "<a href='" + obja.attr("href") + "'>" + obja.attr("title") + "</a>";
        htmlTemplate.con += "</li>";
        htmlTemplate.con += "<jf>";
        $("#navigation .page-title span").html(obja.text());
        $("#navigation .page-title small").html(obja.attr("title") || "");
    } else {
        htmlTemplate.con += "<li>";
        htmlTemplate.con += "<i class='" + $("a", objli).find("i").attr("class") + "'>" + "</i>";
        htmlTemplate.con += "<span>" + $("a", objli).attr("title") + "</span>";
        htmlTemplate.con += "<i class='fa fa-angle-right'></i>";
        htmlTemplate.con += "</li>";
        htmlTemplate.con += "<jf>"
    }
    i++;
    if (objli.parent()[0].className === "page-sidebar-menu") {
        $(".arrow", objli).before("<span class='selected'></span>");
    } else {
        sideMenus(objli.parent().parent(), obja, i, htmlTemplate);
    }    
}

(function () {
    var isIE8Or9 = false;

    if (window.ActiveXObject) {
        var ua = navigator.userAgent.toLowerCase();
        var ie = ua.match(/msie ([\d.]+)/)[1]
        if (ie == 8.0 || ie == 9.0) {
            isIE8Or9 = true;
        }

        if (ie == 6.0) {
            alert("您的浏览器版本是IE6，在本系统中不能达到良好的视觉效果，建议你升级到IE8及以上！")
        }
    }

    if (!isIE8Or9) {
        //alert("您的浏览器版本不是IE8或IE9，在本系统中不能达到良好的视觉效果，建议你升级到IE8以上！")
    }
})();

$("#checkall").click(function () {
    var ischecked = this.checked;
    $("input:checkbox[name='ids']").each(function () {
        this.checked = ischecked;
    });

    $.uniform.update(':checkbox');
});

$("#delete").click(function () {
    var message = "你确定要删除勾选的记录吗?";
    if ($(this).attr("message"))
        message = $(this).attr("message") + "，" + message;
    if (confirm(message))
        $("#mainForm").submit();
});



