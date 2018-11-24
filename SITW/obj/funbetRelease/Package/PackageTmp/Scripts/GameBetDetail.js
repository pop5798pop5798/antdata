
$(document).ready(function () {
    setInterval(checkTopicTimeOut, 3000);

});


function betcheck(arr, $form, options) {
    var msg = "";
    $form.find("[name=money]").each(function (key, value) {
        if ($(value).val() != "") {
            var m = $(value).val();
            if (!$.isNumeric(m)) {
                alert("請輸入數字");
                return false;
            }
            if (m <= 0) {
                alert("請輸入正確下注金額")
                return false;
            }
        } 
    });
    $form.find(".betButton").attr("disabled", true);
};

function betresult(responseText, statusText, xhr, $form) {
    if (responseText.isTrue) {
        $form.find("input[name$=money]").val("");
        updateAll();
    }
    else {
        alert(responseText.ErrorMsg);
    }
    $form.find(".betButton").removeAttr("disabled");

};

function TopicHtmlGenerator(value)
{
    var datego = value.edate;
    var newDate = datego.replace(/T/g, ' ');
    var htmlstr = "";
    htmlstr += ' \
            <div id='+ value.md5TopicSn + ' class="item col-xs-12 col-sm-6 col-md-6 menucolor"> \
        <div id="internal">\
                    <div class="sportsmagazine-fancy-title4">\
                        <h2>\
                            ' + value.title + ' \
                        </h2>\
                        \
                    </div>\
                    <div class="progres-title">\
                        <div class=info>\
                            <div class="progress">'
    $.each(value.choiceList, function (ckey, cvalue) {
        htmlstr += '\
                                <div name="p' + cvalue.md5ChoiceSn + '" class="progress-bar progress-bar-striped active" role="progressbar"\
                                        aria-valuenow="'+ cvalue.betMoneyRate + '" aria-valuemin="0" aria-valuemax="100"\
                                        style="width: '+ cvalue.betMoneyRate + '%;">\
                                </div>';
    });
    htmlstr += '\
                            </div>\
                            <span>'
    $.each(value.choiceList, function (ckey, cvalue) {
        htmlstr += '\
                                <p name="p' + cvalue.md5ChoiceSn + '">' + cvalue.choiceStr + '： ' + cvalue.betMoney + ' (' + cvalue.betMoneyRate + '%)</p>'
    });
    htmlstr += '\
                            </span>\
                        </div>\
                    </div>\
                    <h4 class="card-title" style="">\
                        ' + (value.comment == null ? '' : '題目說明：' +value.comment) + ' \
                    </h4>\
                    <div class="card-body"> \
                        <div class="betin"> \
                            <form name="betForm" action="/bet/Create" method="post"> \
                            <input type="hidden" name="edate" class="canbet" value="'+ newDate +'"> \
';
    ;

    var canbet = value.canbet;

    $.each(value.choiceList, function (ckey, cvalue) {
        htmlstr += ' \
                                <div class="row choiceRow">\
                                    <div class="col-xs-8 col-md-9 col-lg-9 detailscolor">\
                                        <p>'+ cvalue.choiceStr + '</p>\
                                    </div>\
                                    <div class="col-xs-4 col-md-3 col-lg-3">\
'
        if (canbet) {
            var OddsFunction = false;
            if (cvalue.Odds != null)
                OddsFunction = true;
            htmlstr += ' \
                                        <input id="EchoiceSn" name="betList['+ ckey + '].EchoiceSn" type="hidden" value="' + cvalue.EchoiceSn + '"> \
                                        <input name="Odds" type="hidden" value="' + cvalue.Odds + '"> \
                                        <input class="form-control canbet" id="money" name="betList['+ ckey + '].money" type="tel" pattern="^\+?[1-9][0-9]*0$|^$|^0$" \
                                            '+ (OddsFunction ? 'placeholder="賠率 ' + cvalue.Odds + '" onfocus="showOddsPanel(this)" onblur="hideOddsPanel(this)" onkeyup="showOddsDetail(this)"' : "")
                                        +' inputmode="numeric" value=""> \
                ';
        }
        else {
            htmlstr += ' \
                '
        }
        htmlstr += ' \
                                    </div> \
                                </div> \
            ';
    });
    htmlstr += '\
                                <div class="row">\
                                    <div class="col-md-3"> \
                                    </div> \
                                    <div class="col-md-6">'
    if (canbet) {
        htmlstr +='\
                                        <a class="betButton canbet" style="cursor:pointer" ><i class="fa fa-diamond" style="margin:0px 10px 0px 5px"></i>點 擊 下 注</a>\ '
    }
    htmlstr +='\
                                    </div> \
                                </div>\
                            </form>\
                        </div>\
                        <p></p>\
                    </div>\
                 </div>\
            </div>\
    ';
    return htmlstr;
}

function createTopic(topicData) {
    $.each(JSON.parse(topicData), function (key, value) {
        var htmlstr = "";
        htmlstr = TopicHtmlGenerator(value);
        $("div[name=ddTopic]").append(htmlstr);
    });
    $('.betButton').click(function () {
        var attr = $(this).attr("disabled");
        if (typeof attr == typeof undefined) {          
            $(this).parents('form').submit();
        }
    });
    $('form[name=betForm]').ajaxForm({
        beforeSubmit: betcheck,
        success: betresult
    });
    var $container = $(".masonry");
    $container.masonry({
        
    });
}

function updateTopic(topicData) {
    $.each(JSON.parse(topicData), function (key, value) {
        var htmlstr = "";
        htmlstr = TopicHtmlGenerator(value);
        if ($("#" + value.md5TopicSn).length >= 1) {
            $("#" + value.md5TopicSn).html($(htmlstr).html());
        }
        else {
            var $container = $("div[name=ddTopic]");
            var $content = $(htmlstr);
            $container.append($content).masonry('appended', $content);
        }
    });
    $('.betButton').click(function () {
        var attr = $(this).attr("disabled");
        if (typeof attr == typeof undefined) {           
            $(this).parents('form').submit();
        }
    });
    $('form[name=betForm]').ajaxForm({
        beforeSubmit: betcheck,
        success: betresult
    });
}


function updateBetMoney(topicData) {
    $.each(JSON.parse(topicData), function (key, value) {
        
        $.each(value.choiceList, function (ckey, cvalue) {
            $("div[name=p" + cvalue.md5ChoiceSn + "]").css("width", cvalue.betMoneyRate + "%");
            $("p[name=p" + cvalue.md5ChoiceSn + "]").html(cvalue.choiceStr + '： ' + cvalue.betMoney + ' (' + cvalue.betMoneyRate + '%)');

        });
    });
    
    /*
    chartstring = "[" + chartstring + "]";

    暫時移除highcharts
    Highcharts.setOptions({
        colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4']
    });

    Highcharts.chart('container', {
        chart: {
            renderTo: 'chart',
            margin: 0,
            defaultSeriesType: 'areaspline',
            backgroundColor: 'rgba(255, 255, 255, 0.0)',
            type: 'bar'
        },
        yAxis: {
            gridLineWidth: 0,
            visible: false
        },
        xAxis: {
            gridLineWidth: 0,
            visible: false
        },
        title: {
            text: ""
        },
        exporting: { enabled: false },
        credits: {
            enabled: false
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                stacking: 'percent'
            },
            bar: {
                borderRadius: 5
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            headerFormat: "",
            shared: true
        },
        series: JSON.parse(chartstring)
    });
    */


}

function showOddsPanel(obj) {
    if ($(obj).parents(".choiceRow").next().attr("name") != "OddsPanel") {
        var oddspanelhtml = '<div name="OddsPanel" class="row OddsPanel detailscolor"> \
            <div class="col-xs-12 col-md-12 col-lg-12 oddsDetail">\
                \
            </div>\
        </div>';

        $(obj).parents(".choiceRow").after(oddspanelhtml);
        showOddsDetail(obj);

        var $container = $(".masonry");
        $container.masonry({

        });
    }
}
function hideOddsPanel(obj) {
    if ($(obj).val() == "" || $(obj).val() == "0") {
        if ($(obj).parents(".choiceRow").next().attr("name") == "OddsPanel") {
            $(obj).parents(".choiceRow").next().remove();
            var $container = $(".masonry");
            $container.masonry({

            });
        }
    }
}

function showOddsDetail(obj) {
    var odds = $(obj).siblings("input[name=Odds]").val();
    var money = $(obj).val();
    var divname = $(obj).siblings("[name$=EchoiceSn]").val();
    $(obj).parents(".choiceRow").next().find(".oddsDetail").html('<p style="color:#d6d6d6;font-size:14px;font-weight:bold;">猜中獲利：' + '<span style="color:#ea1300">' + money + '</span>' + ' × ' + odds + ' = ' + ' × ' + odds + ' = ' + '<span style="color:#ea1300">' + Math.round(money * odds) + '</span>' + '</p>');
}

///檢查題目的時間是不是已經過了，自動關閉
function checkTopicTimeOut() {
    $("input[name=edate]").each(function (i,obj) {
        if (Date.parse($(obj).val()) < Date.now())
        {
            $(obj).parents("form").find(".canbet").remove();
        }
    });
}
