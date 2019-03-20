//const var
var PX100 = "100px"; var PX150 = "150px";
var PX200 = "200px"; var PX250 = "250px";
var PX300 = "300px"; var PX350 = "350px";
var PX400 = "400px"; var PX450 = "450px";
var PX500 = "500px"; var PX550 = "550px";
var PX600 = "600px"; var PX650 = "650px";
var PX700 = "700px"; var PX750 = "750px";
var PX800 = "800px"; var PX750 = "850px";
var PX800 = "900px"; var PX750 = "950px";
var PX800 = "1000px";

var PER10 = "10%"; var PER20 = "20%";
var PER30 = "30%"; var PER40 = "40%";
var PER50 = "50%"; var PER60 = "60%";
var PER70 = "70%"; var PER80 = "80%";
var PER90 = "90%"; var PER100 = "100%";

var $nn = (function () {
    var prototype = {
        refresh: function () { window.location.href = window.location.href; },
        refreshAll: function () { window.parent.location.href = window.parent.location.href; },
        setTitle: function () { window.document.title = title; },
        setParentTitle: function () { window.parent.document.title = title; },
        isSuccess: function (str) { return str.indexOf("成功") > -1; }
    };

    return prototype;
})();

/*
创建数据共享接口——简化框架之间相互传值
http://www.planeart.cn/?p=1554
*/
var share = {
    /**
    * 跨框架数据共享接口
    * @param	{String}	存储的数据名
    * @param	{Any}		将要存储的任意数据(无此项则返回被查询的数据)
    */
    data: function (name, value) {
        var top = window.top,
			cache = top['_CACHE'] || {};
        top['_CACHE'] = cache;

        return value !== undefined ? cache[name] = value : cache[name];
    },

    /**
    * 数据共享删除接口
    * @param	{String}	删除的数据名
    */
    removeData: function (name) {
        var cache = window.top['_CACHE'];
        if (cache && cache[name]) delete cache[name];
    }

};

/*
artDialog扩展
*/
var artDialogExt = {
    tipsAuto: function (msg) {
        msg = msg || "";
        artDialog.tips(msg);
        if (msg.indexOf('成功') > -1) {
            return true;
        }
        else {
            return false;
        }
    },
    //提示并刷新
    tipsAndReflesh: function (msg) {
        msg = msg || "";
        artDialog.tips(msg);
        if (msg.indexOf('成功') > -1) {
            window.location.href = window.location.href;
        }
    },
    //提示并跳转页面
    tipsToUrl: function (msg, url) {
        art.dialog.tips(msg || "");
        window.location.href = url;
    },
    alertAuto: function (msg) {
        msg = msg || "";
        artDialog.alert(msg);
        if (msg.indexOf('成功') > -1) {
            return true;
        }
        else {
            return false;
        }
    },
    //关闭并刷新
    closeAndReload: function () {
        if (art.dialog.data("isok")) {
            window.location.href = window.location.href;
            art.dialog.removeData("isok");
        }
    },
    //提示并跳转页面
    alertToUrl: function (msg, url) {
        art.dialog.alert(msg, function () {
            window.location.href = url;
        });
    }
}
var artExt = artDialogExt;


function newGuid() {
    function guid_S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    // then to call it, plus stitch in '4' in the third group
    return (guid_S4() + guid_S4() + "-" + guid_S4() + "-4" + guid_S4().substr(0, 3) + "-" + guid_S4() + "-" + guid_S4() + guid_S4() + guid_S4()).toLowerCase();
}


//======================Date 扩展================================
// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
//调用：  
//var time1 = new Date().Format("yyyy-MM-dd");
//var time2 = new Date().Format("yyyy-MM-dd HH:mm:ss");

Date.prototype.Format = function (fmt) { //author: meizz 
    if (fmt == null || fmt == '' || fmt == undefined) fmt = "yyyy-MM-dd";
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日    
        "h+": this.getHours() == 0 ? 12 : this.getHours(), //小时       
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.toStr = function (fmt) {
    return this.Format(fmt);
}

Date.prototype.addDays = function (days) {
    return new Date(this.setDate(this.getDate() + days));
}

Date.prototype.addMonths = function (v) {
    return new Date(this.setMonth(this.getMonth() + v));
}
Date.prototype.addYears = function (v) {
    return new Date(this.setFullYear(this.getFullYear() + v));
}

//计算时间相差天数
function TotalDays(date2, date1) {
    return DiffDays(date2, date1);
}
//计算时间相差天数
function DiffDays(date2, date1) {
    var d1 = new Date(date1);
    var d2 = new Date(date2);
    var date3 = d2.getTime() - d1.getTime()  //时间差的毫秒数 

    //计算出相差天数
    var days = Math.floor(date3 / (24 * 3600 * 1000))
    return days;
}

/*
是否为int
*/
String.prototype.isInt = function () {
    return !isNaN(parseInt(this));
}
String.prototype.isFloat = function () {
    return !isNaN(parseFloat(this));
}

//easy ui 很忧伤的地方
; var easyui;
(function (window, undefined) {
    if (jQuery) {
        easyui = function (selector, context) {
            return {
                val: function (context) {
                    if (jQuery(selector).hasClass('easyui-datebox')) {
                        if (context == undefined)
                            return jQuery(selector).datebox('getValue');
                        else jQuery(selector).datebox('setValue', context);
                    }
                        //numberbox
                    else if (jQuery(selector).hasClass('easyui-numberbox')) {
                        if (context == undefined)
                            return jQuery(selector).numberbox('getValue');
                        else jQuery(selector).numberbox('setValue', context);
                    }

                    return jQuery(selector).val(context);
                }
            }
        }
    }
    else console.log("dbjs.js need jQuery.")
    //easyui = window.easyui;
})(window, undefined);