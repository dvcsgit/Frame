/* 原型繫結
* ============================================================ */
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

String.prototype.startWith = function (prefix) {
    return (this.substr(0, prefix.length) === prefix);
}

Array.prototype.last = function () { return this[this.length - 1]; }

//判斷字串是否包含指定的字
//回傳: bool
String.prototype.contains = function (txt) {
    return (this.indexOf(txt) >= 0);
}

Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
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

Date.prototype.ToChineseDate = function () {
    var year = parseInt(this.getFullYear()) - 1911;
    var month = parseInt(this.getMonth()) + 1;
    var day = parseInt(this.getDate());
    var monthStr = month;
    var dayStr = day;
    if (month < 10) {
        monthStr = "0" + month;
    }
    if (day < 10) {
        dayStr = "0" + day;
    }
    return year + monthStr + dayStr;
}


//拓展方法
$.extend({
    goBack: function () {
        $("#item-detail").hide().empty();
        $("#item-index").fadeIn();
    },
    Alert: function (msg, callback, title, isError) {
        var isError = isError || false;
        var classIcon = isError ? ' icon-ban-circle red' : 'icon-warning-sign yellow';
        var title_text = title ? title : "系統訊息";
        var title_template = '<h4 class="modal-title"><i class="' + classIcon + '"></i>' + title_text + '</h4>';
        var btn_title = $("<button><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>").addClass("close").attr({ "data-dismiss": "modal" });
        var headerArea = $("<div class='modal-header'></div>").append(btn_title).append(title_template);
        var bodyArea = $("<div class='modal-body'></div>").html(msg);
        var footArea = $("<div class='modal-footer'></div>");
        var btn_OK = $("<button>確定</button>").addClass("btn btn-primary").attr({ "data-dismiss": "modal" }).appendTo(footArea);
        if (callback != undefined) {
            btn_OK.click(function () {
                callback();
            });
        }


        var contentArea = $("<div id='alertDialog'><div class='modal-dialog'><div class='modal-content'></div></div></div>")
            .find(".modal-content").append(headerArea).append(bodyArea).append(footArea).end()
            .attr({ "tabindex": "-1", "role": "dialog", "aria-labelledby": "myModalLabel", "aria-hidden": "true" }).addClass("modal fade");
        contentArea.appendTo("body").modal('show');
        contentArea.bind('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
            console.log('hidden');
            $(this).remove();
        });




    },
    Confirm: function (msg, callback, title) {

        var title_text = title ? title : "系統訊息";
        var title_template = '<h4 class="modal-title">' + title_text + '</h4>';
        var btn_title = $("<button><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>").addClass("close").attr({ "data-dismiss": "modal" });
        var headerArea = $("<div class='modal-header'></div>").append(btn_title).append(title_template);
        var bodyArea = $("<div class='modal-body'></div>").html(msg);
        var footArea = $("<div class='modal-footer'></div>");
        var btn_OK = $("<button>確定</button>").addClass("btn btn-primary").attr({ "data-dismiss": "modal" }).appendTo(footArea);
        var btn_Cancel = $("<button>取消</button>").addClass("btn").attr({ "data-dismiss": "modal" }).appendTo(footArea);
        if (callback != undefined) {
            btn_OK.click(function () {
                callback();
            });
        }


        var contentArea = $("<div id='confirmDialog'><div class='modal-dialog'><div class='modal-content'></div></div></div>")
            .find(".modal-content").append(headerArea).append(bodyArea).append(footArea).end()
            .attr({ "tabindex": "-1", "role": "dialog", "aria-labelledby": "myModalLabel", "aria-hidden": "true" }).addClass("modal fade");

        contentArea.appendTo("body").modal('show');
        contentArea.on('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
            $(this).remove();
        });



    },
    PopDialog: function (msg, title, width) {
        var modalWidth = 650;
        if (width != undefined) modalWidth = width;

        var title_text = title ? title : "系統訊息";
        var title_template = '<h4 class="modal-title">' + title_text + '</h4>';
        var btn_title = $("<button><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>").addClass("close").attr({ "data-dismiss": "modal" });
        var headerArea = $("<div class='modal-header'></div>").append(btn_title).append(title_template);
        var bodyArea = $("<div class='modal-body'></div>").html(msg);
        var footArea = $("<div class='modal-footer'></div>");
        var btn_OK = $("<button>確定</button>").addClass("btn btn-primary").attr({ "data-dismiss": "modal" }).appendTo(footArea);

        var contentArea = $("<div id='popDialog'><div class='modal-dialog' style='width:" + width + "px'><div class='modal-content'></div></div></div>")
            .find(".modal-content").append(headerArea).append(bodyArea).append(footArea).end()
            .attr({ "tabindex": "-1", "role": "dialog", "aria-labelledby": "myModalLabel", "aria-hidden": "true" }).addClass("modal fade");
        contentArea.appendTo("body").modal('show');
        contentArea.bind('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
            console.log('hidden');
            $(this).remove();
        });




    },
    ShowLoading: function () {
        $('#loading').fadeIn();
    },
    HideLoading: function () {
        $('#loading').fadeOut();
    },
    ShowGridLoading: function (divId) {
        var template = $("<div id='grid-loading' style='z-index: 2000;' class='ajax-loading-overlay'><i class='ajax-loading-icon fa fa-spin fa-spinner fa-2x blue'></i> <span class='blue'> 讀取中... </span></div>");
        template.appendTo('#' + divId).fadeIn();
    },
    HideGridLoading: function () {
        //$("#grid-loading").fadeOut();
    },
    SuccessDialog: function (msg) {
        $.msgGrowl({
            type: 'success'
            , title: '@Resources.Resource.SystemMessage'
            , text: msg
            , position: 'top-center'
            , closeTrigger: true
            , lifetime: 5000
        });
    },
    ErrorDialog: function (msg) {
        $.msgGrowl({
            type: 'error'
            , title: '@Resources.Resource.SystemMessage'
            , text: msg
            , position: 'top-center'
            , closeTrigger: true
            , lifetime: 5000
        });
    }
});


$.fn.extend({
    goToNextPage: function (url) {
        var separator = url.indexOf('?') >= 0 ? '&' : '?';
        $.get(url + separator).done(function (content) {
            $("#item-index").hide();
            var detail = $("#item-detail");
            //僅僅保留主體，將其他元素過濾掉，script寫在主體上（Index）
            detail.html(content).filter('div').fadeIn();
            $.validator.unobtrusive.parse(detail.find('form'));
        });
    },
    ChineseDatePicker: function () {
        return $(this).datepicker({
            autoclose: true,
            //format: "yyyy/mm/dd",
            format: {
                /*
                 * Say our UI should display the Chinese Date,
                 * but textbox should store the actual date.
                 * This is useful if we need UI to select local dates,
                 * but store in zh-Tw
                 */
                toDisplay: function (date, format, language) {
                    debugger;
                    var d = new Date(date);
                    var year = d.getFullYear() - 1911;
                    var mouth = d.getMonth() + 1;
                    if (mouth < 10) {
                        mouth = "0" + mouth;
                    }
                    var day = d.getDate();
                    if (day < 10) {
                        day = "0" + day;
                    }
                    return (year + "" + mouth + "" + day);
                },
                toValue: function (date, format, language) {
                    debugger;
                    var year = parseInt(date.substr(0, 3)) + 1911;
                    var month = parseInt(date.substr(3, 2)) - 1;
                    var day = date.substr(5, 2);
                    var d = new Date(year, month, day, 0, 0, 0, 0);
                    return d;
                }
            },
            language: "zh-TW",
            todayHighlight: true
        }).next().on(ace.click_event, function () {
            $(this).prev().focus();
        });
    },
    DefaultDatePicker: function (format) {
        var format = format || "yyyy/mm/dd";
        return $(this).datepicker({
            autoclose: true,
            format: format,
            language: "zh-TW",
            todayHighlight: true
        }).next().on(ace.click_event, function () {
            $(this).prev().focus();
        });
    },
    DefaultMonthPicker: function () {
        return $(this).datetimepicker({
            minView: "year",
            autoclose: true,
            language: "zh-TW",
            format: "yyyy/mm",
            startView: "year",
            todayHighlight: true

        });
    },
    ChineseDateRangePicker: function () {
        return $(this).datepicker({
            autoclose: true,
            //format: "yyyy/mm/dd",
            format: {
                /*
                 * Say our UI should display the Chinese Date,
                 * but textbox should store the actual date.
                 * This is useful if we need UI to select local dates,
                 * but store in zh-Tw
                 */
                toDisplay: function (date, format, language) {
                    debugger;
                    var d = new Date(date);
                    var year = d.getFullYear() - 1911;
                    var mouth = d.getMonth() + 1;
                    if (mouth < 10) {
                        mouth = "0" + mouth;
                    }
                    var day = d.getDate();
                    if (day < 10) {
                        day = "0" + day;
                    }
                    return (year + "" + mouth + "" + day);
                },
                toValue: function (date, format, language) {
                    debugger;
                    var year = parseInt(date.substr(0, 3)) + 1911;
                    var month = date.substr(3, 2) - 1;
                    var day = date.substr(5, 2);
                    var d = new Date(year, month, day, 0, 0, 0, 0);
                    return d;
                    //return new Date(date);
                }
            },
            language: "zh-TW",
            todayHighlight: true
        });
    },
    DefaultDateRangePicker: function (format) {

        var format = format || "yyyy/mm/dd";
        return $(this).datepicker({
            autoclose: true,
            //format: "yyyy/mm/dd",
            format: format,
            language: "zh-TW",
            todayHighlight: true
        });
    },
    SelectDefaultOptions: function (placeholder, width) {
        placeholder = placeholder || "請輸入資料";
        width = width || "200px";

        var options = {
            placeholder: placeholder,
            width: width,
            allowClear: true,
            formatSearching: function () { return "搜尋中，請稍後..."; },
            formatNoMatches: function () { return "沒有可查詢的資料"; },
            formatLoadMore: function () { return "更多..."; },
            minimunInputLength: 0
        };
        return $(this).select2(options);
    },
    DeleteItem: function () {
        var btn = $(this);
        var url = btn.attr("href");
        $.Confirm("確定要刪除所選項目嗎!?", function () {
            $.get(url).done(function (json) {
                var json = json || {};
                debugger;
                if (json.success) {
                    $.Alert("刪除成功!", function () {
                        $("#criteria-form").submit();
                    });
                } else if (json.errors) {
                    $.Alert(json.errors);
                }
            }).fail(function (fail) {

                $.Alert("系統發生錯誤");
            });
        });
    },

    ShowImageDialog: function (width) {
        var url = $(this).attr("href");
        var modalWidth = 650;
        if (width != undefined) modalWidth = width;

        var image = $("<img src=" + url + " title='' width='500'/>");

        var contentArea = $("<div id='showDialog'><div class='modal-dialog' style='width:" + width + "px'><div class='modal-content'></div></div></div>")
            .find(".modal-content").append(image).end()
            .attr({ "tabindex": "-1", "role": "dialog", "aria-labelledby": "myModalLabel", "aria-hidden": "true" }).addClass("modal fade");
        contentArea.appendTo("body").modal('show');



        contentArea.bind('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
            $(this).remove();
        });

    }
});


//名稱空間
var commonjs = {};

/* 取得摘要式的錯誤 
 * ============================================================
 * Useage : commonjs.getValidationSummaryErrors(errors);
 * Param $form -> from selector
 * Return html string
 * ============================================================ */
commonjs.getValidationSummaryErrors = function ($form) {
    // We verify if we created it beforehand
    var errorSummary = $form.find('.validation-summary-errors, .validation-summary-valid');
    if (!errorSummary.length) {
        errorSummary = $('<div class="alert alert-danger alert-message">' +
            '<button type="button" class="close" data-dismiss="alert">' +
            '<i class="icon-remove"></i></button><strong><i class="icon-warning-sign red bigger-130"></i> 儲存失敗. 請修正以下的錯誤後，再試一次.</strong><ul></ul></div>')
            .prependTo($form);
    }

    return errorSummary;
};

/* Display Json Error
 * ============================================================
 * Useage : commonjs.displayJSONErrors(errors);
 * Return html string
 * ============================================================ */
commonjs.displayJSONErrors = function (errors) {
    var errorSummary = $("<div class='json-error alert alert-danger'><h4><i class='icon-warning-sign'></i> 發生錯誤</h4><ul></ul></div>")
    var items = $.map(errors, function (error) {
        return '<li>' + error + '</li>';
    }).join('');
    var ul = errorSummary
        .find('ul')
        .append(items);
    return errorSummary.wrap('<p/>').parent().html();
};

/* Display Json Error to form
 * ============================================================
 * Useage : commonjs.displayJSONErrors(errors);
 * Return append html string to form
 * ============================================================ */
commonjs.displayErrors = function (form, errors, isfade) {
    var errorSummary = commonjs.getValidationSummaryErrors(form)
        .removeClass('validation-summary-valid  alert alert-danger')
        .addClass('validation-summary-errors  alert alert-danger');
    debugger;
    var items = $.map(errors, function (error) {
        return '<li>' + error + '</li>';
    }).join('');

    var ul = errorSummary
        .find('ul')
        .empty()
        .append(items);



};


/* 將表單回到初始狀態
 * ============================================================
 * Usage : commonjs.resetForm(selector);
 * Return void
 * ============================================================ */
commonjs.resetForm = function ($form) {
    $form[0].reset();

    if ($form.find(".validation-summary-valid").length > 0) {
        // We reset the form so we make sure unobtrusive errors get cleared out.
        commonjs.getValidationSummaryErrors($form)
            .removeClass('validation-summary-errors  alert alert-danger')
            .addClass('validation-summary-valid  alert alert-danger');
    }


};

/* 表單提交 Handler
 * ============================================================
 * Useage : $("#add-form").submit(callback, commonjs.formSubmitHandler);
 *          OR $("#add-form").submit(commonjs.formSubmitHandler);
 * Return void
 * ============================================================ */
commonjs.formSubmitHandler = function (e) {
    var $form = $(this);
    var callback = null;
    if (arguments.length != 0) callback = arguments[0].data;

    if (!$form.valid || $form.valid()) {
        $.ajax({
            type: "POST",
            url: $form.attr('action'),
            data: $form.serializeArray(),
            contentType: "application/x-www-form-urlencoded;charset=utf-8"
        }).done(function (json) {
            json = json || {};
            // In case of success, we redirect to the provided URL or the same page.
            if (json.success) {
                if (callback != null) { callback.call(json); }
                else {
                    location = json.redirect || location.href;
                }
            } else if (json.errors) {
                debugger;
                commonjs.displayErrors($form, json.errors);
            }
        })
            .error(function () {
                commonjs.displayErrors($form, ['伺服器發生錯誤']);
            });
    }
    // Prevent the normal behavior since we opened the dialog
    e.preventDefault();
};


/* 表單成功通用動作 彈出訊息 觸發criteriaForm
 * ============================================================
 * Useage :
 * Return void
 * ============================================================ */
commonjs.success = function () {
    $.Alert('儲存成功', function () {
        commonjs.goBack();
        $("#criteria-form").submit();
    });
};


/* 刪除動作 彈出訊息 觸發criteriaForm
 * ============================================================
 * Useage :
 * Return void
 * ============================================================ */
commonjs.deleteHandler = function () {
    var btn = $(this);
    var url = btn.attr("href");
    $.Confirm("確定要刪除所選項目嗎!?", function () {
        $.get(url).done(function (json) {
            var json = json || {};
            if (json.success) {
                $.Alert("刪除成功!", function () {
                    $("#criteria-form").submit();
                });
            } else if (json.errors) {
                $.Alert(json.errors);
            }
        }).fail(function (fail) {

            $.Alert("系統發生錯誤");
        });
    });
};

commonjs.goToNextPage = function (link, url) {
    var separator = url.indexOf('?') >= 0 ? '&' : '?';
    $.get(url + separator)
        .done(function (content) {
            $("#item-index").hide();
            var detail = $("#item-detail");
            detail.html(content).filter('div').fadeIn(); // Filter for the div tag only, script tags could surface
            $.validator.unobtrusive.parse(detail.find('form'));
        });

}

commonjs.goBack = function () {
    $("#item-detail").hide().empty();
    $("#item-index").fadeIn();
}

/*pop window but ..*/
commonjs.loadAndShowPop = function (url, width) {
    var separator = url.indexOf('?') >= 0 ? '&' : '?';
    var modalWidth = 650;
    if (width != undefined) modalWidth = width;
    $.get(url + separator)
        .done(function (content) {
            var innerContent = $("#popover-inner");
            innerContent.empty();
            innerContent.html(content);
            $.validator.unobtrusive.parse(innerContent.find('form'));
        });
};

/* Ajax 彈出視窗 使用 modal
 * ============================================================
 * Useage :
 * Return void
 * id => string ex: myDialog
 * link => jquery seletor ex: %(".class")
 * url => 直接給url
 * width => 指定寬度
 * ============================================================ */
commonjs.loadAndShowDialog = function (link, url, width) {
    var modalWidth = 650;
    if (width != undefined) modalWidth = width;

    $.get(url).done(function (content) {

        var contentArea = $("<div id='showDialog'><div class='modal-dialog' style='width:" + width + "px'><div class='modal-content'></div></div></div>")
            .find(".modal-content").append(content).end()
            .attr({ "tabindex": "-1", "role": "dialog", "aria-labelledby": "myModalLabel", "aria-hidden": "true" }).addClass("modal fade");
        contentArea.appendTo("body").modal('show');
        contentArea.bind('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
            $(this).remove();
        });

        $.validator.unobtrusive.parse(contentArea.find('form'));
    });
};


commonjs.exportHandler = function (form, url, parameters) {
    //var para = new Array();
    var par = form.serialize();
    if (parameters) {
        $.each(parameters, function (idx, value) {
            par += "&" + value;
        });
    } else {
        par += "&isExport=true";
    }
    $.post(url, par)
        .done(function (json) {
            json = json || {};
            if (json.success) {
                $("body").append("<iframe src='" + json.url + "' style='display:none;'></iframe>");
            } else if (json.errors) {
                $.Alert(json.errors, undefined, "伺服器錯誤", true);
            }
        })
        .error(function () {
            $.Alert("伺服器發生錯誤", undefined, "伺服器錯誤", true);
        });
};

commonjs.exportUrlHandler = function (url, param) {
    debugger;
    $('#loading').fadeIn();
    $.post(url, param)
        .done(function (json) {
            $('#loading').fadeOut();
            json = json || {};
            if (json.success) {
                $("body").append("<iframe src='" + json.url + "' style='display: none;' ></iframe>");
            } else if (json.errors) {
                $.Alert(commonjs.displayJSONErrors(json.errors), function () { });
            }
        })
        .error(function () {
            $('#loading').fadeOut();
            $.Alert("與報表伺服器主機連線失敗。");
        });
};

commonjs.successForDialog = function () {
    $('#showDialog').modal('hide');
    $.Alert("儲存成功", function () {
        $("#criteria-form").submit();
    }, "提示");
};




commonjs.select2 = function (placeholder, width, url, over_ajax_data) {
    placeholder = placeholder || "請輸入資料";
    width = width || "200px";

    var options = {
        placeholder: placeholder,
        width: width,
        ajax: {
            url: url,
            dataType: 'json',
            quietMillis: 100,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    term: term, //search term
                    pageSize: 10, // page size
                    pageNo: page // page number
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total;
                return { results: data.data, more: more };
            }
        },
        allowClear: true,
        formatSearching: function () { return "搜尋中，請稍後..."; },
        formatNoMatches: function () { return "沒有可查詢的資料"; },
        formatLoadMore: function () { return "更多..."; },
        minimunInputLength: 0,
    };

    if (over_ajax_data != undefined) {
        options.ajax.data = over_ajax_data;
    }
    return options;
};