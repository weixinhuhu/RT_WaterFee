/*
根据key和value查询json
*/
function QueryJson(objJson, key, value) {
    for (var i = 0; i < objJson.length; i++)
        if (objJson[i][key] == value)
            return objJson[i];
    return null;
}
function QueryJson2(objJson, key1, value1, key2, value2) {
    for (var i = 0; i < objJson.length; i++)
        if (objJson[i][key1] == value1)
            if (objJson[i][key2] == value2)
                return objJson[i];
    return null;
}
//名称:加载显示等待画面,loading等待...
//作者:来自深蓝QQ群257018781
//版本:v0.1 (2017.06.26)
var isInitDivloading = false;
var loading = {
    show: function (msg) {
        if (!isInitDivloading) initDivloading();
        $("#divLoadingElement").show();
        msg = msg || "正在处理，请稍待。。。";//自定义消息
        $("#divLoadingElementMsg").html(msg);
        $("#divLoadingElementMsg").show();
    },
    hide: function () {
        $("#divLoadingElement").hide();
        $("#divLoadingElementMsg").hide();
    }
};
function initDivloading() {
    var nodeLoading = document.createElement('style');
    var style = "#divLoadingElement{z-index:9999;display:none;position:fixed;width:100%;height:100%;background:black;filter:alpha(opacity=50);opacity:0.5;}";
    style += "#divLoadingElementMsg{border-color: #95B8E7;background: #ffffff url('data:image/gif;base64,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') no-repeat scroll 5px center; position: absolute;top: 50%;left:45%;margin-top: -20px; padding: 10px 5px 10px 30px; width: auto; height: 16px;border-width: 2px;border-style: solid; z-index: 99999;display:none;}";

    nodeLoading.type = 'text/css';
    if (nodeLoading.styleSheet) { //IE
        nodeLoading.styleSheet.cssText = style;
    } else {
        nodeLoading.innerHTML = style; //或者写成 nodeLoading.appendChild(document.createTextNode(style))  
    }
    document.getElementsByTagName('head')[0].appendChild(nodeLoading);
    //div
    $("body").prepend('<div id="divLoadingElement"></div><div id="divLoadingElementMsg">正在处理，请稍待。。。</div>');
    isInitDivloading = true;
}
//本文件定义一些在各个视图里面经常用到的一些Javascript脚本函数
//datagrid宽度高度自动调整的函数
$.fn.extend({
    resizeDataGrid: function (heightMargin, widthMargin, minHeight, minWidth) {
        var height = $(document.body).height() - heightMargin;
        var width = $(document.body).width() - widthMargin;
        height = height < minHeight ? minHeight : height;
        width = width < minWidth ? minWidth : width;
        $(this).datagrid('resize', {
            height: height,
            width: width
        });
    }
});
//对象居中的函数，调用例子：$("#loading").center();
$.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", Math.max(0, (($(window).height() - this.outerHeight()) / 2) +
                                        $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - this.outerWidth()) / 2) +
                                        $(window).scrollLeft()) + "px");
    return this;
}
//在页面中生成GUID的值
function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
}
//打包下载所有附件
function DownloadAttach(guid) {
    window.open('/FileUpload/DownloadAttach?guid=' + guid);
}
//在新窗口中查看附件
function ShowAttach(id, ext) {
    var showWindow = true;//标识是否使用窗口查看。office文档+图片文档窗口查看，其他的直接下载
    var viewUrl = '/FileUpload/ViewAttach';
    var returnUrl;
    //var hostname = window.location.host;
    //var hostname = 'http://www.iqidi.com'

    var postData = { id: id };
    $.ajaxSettings.async = false;
    $.get("/FileUpload/GetAttachViewUrl", postData, function (url) {
        if (ext == 'xls' || ext == 'xlsx' || ext == 'doc' || ext == 'docx' || ext == 'ppt' || ext == 'pptx') {
            viewUrl = url;
        }
        else if (ext == "png" || ext == "jpg" || ext == "jpeg" || ext == "gif" || ext == "bmp" || ext == "tif") {
            viewUrl = "/" + url;
        }
        else {
            viewUrl = "/" + url;
            showWindow = false;
        }

        returnUrl = url;
    });

    if (showWindow) {
        $.showWindow({
            title: '查看附件',
            useiframe: true,
            width: 900,
            height: 700,
            content: 'url:' + viewUrl,
            data: { url: returnUrl, ext: ext },
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function (win) {
                    win.close();
                }
            }],
            onLoad: function (win, content) {
                //window打开时调用，初始化form内容
                if (content) {
                    //content.doInit(win);
                }
            }
        });
    }
    else {
        //附件直接下载，不用打开窗体
        window.open(viewUrl);
    }
}
//绑定附件列表
function ShowUpFiles(guid, show_div) {
    $.ajax({
        type: 'GET',
        url: '/FileUpload/GetAttachmentHtml?guid=' + guid,
        //async: false, //同步
        //dataType: 'json',
        success: function (json) {
            $("#" + show_div + "").html(json);
        },
        error: function (xhr, status, error) {
            $.messager.alert("提示", "操作失败" + xhr.responseText); //xhr.responseText
        }
    });
}
//绑定附件列表（查看状态）
function ViewUpFiles(guid, show_div) {
    $.ajax({
        type: 'GET',
        url: '/FileUpload/GetViewAttachmentHtml?guid=' + guid,
        success: function (json) {
            $("#" + show_div + "").html(json);
        },
        error: function (xhr, status, error) {
            $.messager.alert("提示", "操作失败" + xhr.responseText); //xhr.responseText
        }
    });
}
//删除指定的附件后，对附件组进行更新
// id 删除附件id, attachguid 附件组ID, show_div 显示附件的Div
function DeleteAndRefreshAttach(id, attachguid, show_div) {
    $.messager.confirm("删除确认", "您确定要删除该附件吗？", function (action) {
        if (action) {
            $.ajax({
                type: 'POST',
                url: '/FileUpload/Delete?id=' + id,
                async: false,
                success: function (msg) {
                    ShowUpFiles(attachguid, show_div);//更新列表
                },
                error: function (xhr, status, error) {
                    $.messager.alert("提示", "操作失败"); //xhr.responseText
                }
            });
        }
    });
}
//绑定字典内容到指定的控件
function BindDictItem(control, dictTypeName) {
    $('#' + control).combobox({
        url: '/DictData/GetDictJson?dictTypeName=' + encodeURI(dictTypeName),
        valueField: 'Value',
        textField: 'Text'
    });
}
//绑定回车键操作到指定的控件
function BindReturnEvent(control) {
    $('#' + control).bind("enterKey", function (e) {
        ConfirmScanData();
    });
    $('#' + control).keyup(function (e) {
        if (e.keyCode == 13) {
            $(this).trigger("enterKey");
        }
    });
}
//获取日期获取日期+时间的字符串
function GetCurrentDate(hasTime) {
    var curr_time = new Date();
    var strDate = curr_time.getFullYear() + "-";
    strDate += curr_time.getMonth() + 1 + "-";
    strDate += curr_time.getDate();

    if (hasTime) {
        strDate += " " + curr_time.getHours() + ":";
        strDate += curr_time.getMinutes() + ":";
        strDate += curr_time.getSeconds();
    }
    return strDate;
}
//EasyUI树控件的相关操作
function expandAll(treeName) {
    var node = $('#' + treeName).tree('getSelected');
    if (node) {
        $('#' + treeName).tree('expandAll', node.target);
    }
    else {
        $('#' + treeName).tree('expandAll');
    }
}
function collapseAll(treeName) {
    var node = $('#' + treeName).tree('getSelected');
    if (node) {
        $('#' + treeName).tree('collapseAll', node.target);
    }
    else {
        $('#' + treeName).tree('collapseAll');
    }
}
function unCheckTree(tree) {
    var nodes = $('#' + tree).tree('getChecked');
    if (nodes) {
        for (var i = 0; i < nodes.length; i++) {
            $('#' + tree).tree('uncheck', nodes[i].target);
        }
    }
}
function checkAllTree(tree, checked) {
    var children = $('#' + tree).tree('getChildren');
    for (var i = 0; i < children.length; i++) {
        if (checked) {
            $('#' + tree).tree('check', children[i].target);
        } else {
            $('#' + tree).tree('uncheck', children[i].target);
        }
    }
}
//显示错误或提示信息（需要引用jNotify相关文件）
function showError(tips, TimeShown, autoHide) {
    jError(
      tips,
      {
          autoHide: autoHide || true, // added in v2.0
          TimeShown: TimeShown || 1500,
          HorizontalPosition: 'center',
          VerticalPosition: 'top',
          ShowOverlay: true,
          ColorOverlay: '#000',
          onCompleted: function () { // added in v2.0
          //alert('jNofity is completed !');
          }
      }
    );
}
//显示错误或提示信息（需要引用jNotify相关文件）
function showErrorTop(tips, TimeShown, autoHide) {
    parent.jError(
       tips,
       {
           autoHide: autoHide || true, // added in v2.0
           TimeShown: TimeShown || 1500,
           HorizontalPosition: 'center',
           VerticalPosition: 'top',
           ShowOverlay: true,
           ColorOverlay: '#000',
           onCompleted: function () { // added in v2.0
           //alert('jNofity is completed !');
           }
       }
     );
}
function showTips(tips, TimeShown, autoHide) {
    jSuccess(
      tips,
      {
          autoHide: autoHide || true, // added in v2.0
          TimeShown: TimeShown || 1500,
          HorizontalPosition: 'center',
          VerticalPosition: 'top',
          ShowOverlay: true,
          ColorOverlay: '#000',
          onCompleted: function () { // added in v2.0
          //alert('jNofity is completed !');
          }
      }
    );
}
function showTipsTop(tips, TimeShown, autoHide) {
    parent.jSuccess(
       tips,
       {
           autoHide: autoHide || true, // added in v2.0
           TimeShown: TimeShown || 1500,
           HorizontalPosition: 'center',
           VerticalPosition: 'top',
           ShowOverlay: true,
           ColorOverlay: '#000',
           onCompleted: function () { // added in v2.0
           //alert('jNofity is completed !');
           }
       }
     );
}