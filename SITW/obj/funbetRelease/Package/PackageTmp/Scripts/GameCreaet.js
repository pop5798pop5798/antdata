var currentIndex = 0;
var stardtime;
var enddtime;
var timeindex;

function indexIterate() {
    var newHref = $("#addTopic").attr("href");
    currentIndex = $("div[name=topicDiv]").length;
    timeindex = currentIndex;
    var newerHref = newHref.replace(/(?:index=)[0-9]+/i, "index=" + currentIndex);
    $("#addTopic").attr("href", newerHref);
    $(".form_datetime").datetimepicker(
        {
            sideBySide: true,
            format: 'YYYY/M/D HH:mm'
        }
    );
    if (currentIndex == 1) {
        $("#addChoice" + 0).attr("href", "/game/_choiceCreate?index=" + 0 + "&topicIndex=" + 0 + "&allnumber=" + 2);
        $("#addChoice" + 0).click();

    }


    $("#game_topicList_" + (currentIndex - 1) + "__sdate").val($("#game_sdate").val());
    $("#game_topicList_" + (currentIndex - 1) + "__edate").val($("#game_edate").val());

    var cuindex = currentIndex - 1;

    choiceCreate(cuindex, 0);
    $("#gamepost_TeamASn,#gamepost_TeamBSn").change(function () { syncTeamChoice() });

};
function indexIterateChoice(i, index) {
    var newHref = $("#addChoice" + i).attr("href");
    var choiceIndex = newHref.match('(?:index=)([0-9]+)')[1];
    choiceIndex = $("input[name^='game.topicList[" + i + "].choiceList'][name$='hashSn']").length - 1;
    var newerHref = newHref.replace(/(?:index=)[0-9]+/i, "index=" + ++choiceIndex);
    $("#addChoice" + i).attr("href", newerHref);

    var bM = $("input[name$=betModel]:checked").val();
    if (bM == 1) {
        $("div[name=Oddsgroup]").show();
    }
    else if (bM == 2) {
        $("div[name=Oddsgroup]").hide();
    }
    changeUnit();
    choiceCreate(i, choiceIndex);
    if (i == 0) {
        syncTeamChoice();
        //$("div[name=topicDiv]").eq(i).find("[name=delChoice],[name=delTopic]").hide();
        // $("#addChoice" + i).hide();
    }
    for (var j = 0; j < 30; j++) {
        $('#game_topicList_' + i + '__choiceList_' + j + '__Odds').change(function () {
            if ($(this).val() >= 10) {
                myoddscheck($(this).val(), $(this));
            }

        });

        $('#game_topicList_' + i + '__choiceList_' + j + '__bearSn').val(String(i) + String(j));

    }

};

function myoddscheck(odds, topicname) {
    var oddscheck = prompt("賠率異常,是否繼續:", odds);
    if (oddscheck == null || oddscheck == "") {
        topicname.val(0);
    } else {
        topicname.val(oddscheck);
    }

}

function syncTeamChoice() {
    $("input[name='game.topicList[0].choiceList[0].choiceStr']").val($("#gamepost_TeamASn option:selected").text());
    $("input[name='game.topicList[0].choiceList[1].choiceStr']").val($("#gamepost_TeamBSn option:selected").text());
}

function syncTeamTopic() {
    $("input[name='game.title']").val($("#gamepost_TeamASn option:selected").text() + " " + "Vs" + " " + $("#gamepost_TeamBSn option:selected").text());

}

function changeUnit() {
    $("input[name$=betUnitArray]:checked").each(function (i, e) {
        $("div[name=choiceUnitDiv][unit=" + $(e).val() + "]").show();
    });
    $("input[name$=betUnitArray]:not(:checked)").each(function (i, e) {
        $("div[name=choiceUnitDiv][unit=" + $(e).val() + "]").hide();
    });

}

function betModelChange() {
    var bM = $("input[name$=betModel]:checked").val();
    if (bM == 1) {
        $("#rakegroup").hide();
        $("div[name=Oddsgroup]").show();
    }
    else if (bM == 2) {
        $("#rakegroup").show();
        $("div[name=Oddsgroup]").hide();
    }

}

function choiceCreate(i, index) {
    if ($("#allnumber-" + i).val() == 1) {
        $("#addChoice" + i).attr("href", "/game/_choiceCreate?index=" + index + "&topicIndex=" + i + "&allnumber=" + 1);
    }
    $("#allnumber-" + i).change(function () {
        var allch = $("#allnumber-" + i).val();
        if (allch == 0) {
            alert("error");
            $("#allnumber-" + i).val(1);
        }
        $("#addChoice" + i).attr("href", "/game/_choiceCreate?index=" + index + "&topicIndex=" + i + "&allnumber=" + allch);


    });


}

function timeEdit() {
    $("#game_sdate").datetimepicker().on('dp.change', function (e) {
        stardtime = $(this).val();
        $("div[name=topicDiv] ").find("input[name$='.sdate']").val(stardtime);


    });
    $("#game_edate").datetimepicker().on('dp.change', function (e) {
        enddtime = $(this).val();
        $("#game_gamedate").val(enddtime);
        $("div[name=topicDiv] ").find("input[name$='.edate']").val(enddtime);


    });

}


$(document).ready(function () {


    $("#betModels label").click(function () {
        betModelChange();
    });
    $(".form_datetime").datetimepicker(
        {
            sideBySide: true,
            format: 'YYYY/M/D HH:mm'
        }
    );

    $("#gamepost_TeamASn,#gamepost_TeamBSn").change(function () { syncTeamChoice() });

    /*$("#game_sdate").datetimepicker().on('dp.change', function (e) {
        stardtime = $(this).val();
        for (var i = 0; i < timeindex; i++) {
            if ($("#game_topicList_" + i + "__sdate").val()) {
                $("#game_topicList_" + i + "__sdate").val(stardtime);
            }
        }

    });

    $("#game_edate").datetimepicker().on('dp.change', function (e) {
        enddtime = $(this).val();
        $("#game_gamedate").val(enddtime);
        for (var i = 0; i < timeindex; i++) {
            if ($("#game_topicList_" + i + "__edate").val()) {
                $("#game_topicList_" + i + "__edate").val(enddtime);
            }
        }
    });*/
    timeEdit();
    



    $("input:submit").click(function () {


        //$("[data-valmsg-for]").html("");
        var boolAllTrue = true;
        if ($("input[name^='game.topicList'][name$='title']").length == 0) {
            $("[data-valmsg-for='game.topicList']").html("需新增題目");
            return false;
        }
        if ($("input[name^='game.topicList'][name$='choiceStr']").length == 0) {
            $("[data-valmsg-for='game.topicList']").html("需新增選項");
            return false;
        }

        return boolAllTrue;
    });

    $(document).on("click", "span[name=delTopic]", function () {
        $(this).parents("div[name=topicDiv]").hide();
        $(this).parents("div[name=topicDiv]").find("input[name$='.valid']").attr("value", 0);

        $(this).parents("div[name=topicDiv]").find("input[name$='.title']").attr("value", "刪除題目");
        $(this).parents("div[name=topicDiv]").find("input[name$='.choiceStr']").attr("value", "刪除選項");
        $(this).parents("div[name=topicDiv]").find("input[name$='.Odds']").attr("value", 0);
    });

    $(document).on("click", "span[name=delChoice]", function () {
        $(this).parents("div[name=choiceDiv]").hide();
        $(this).parents("div[name=choiceDiv]").find("input[name$='.valid']").attr("value", 0);
        $(this).parents("div[name=choiceDiv]").find("input[name$='.choiceStr']").attr("value", "刪除選項");
        $(this).parents("div[name=choiceDiv]").find("input[name$='.Odds']").attr("value", 0);
    });


    changeUnit();
    betModelChange();
    $("#dota2dialog").dialog(

        { autoOpen: false }

    );
    $("select[name$='.PlayGameSn']").change(function () {
        if ($(this).val() == 4) {
            $("#dota2dialog").dialog({ position: { my: 'right top', at: 'right top', of: window }, width: "40%", maxHeight: 800 });
            $("#dota2dialog").dialog("open");

        } else if ($(this).val() == 1) {
            $("#loldialog").dialog({ position: { my: 'right top', at: 'right top', of: window }, width: "40%", maxHeight: 800 });
            $("#loldialog").dialog("open");

        }

    });
    

    $(document).on("click", "tr #dota2data", function () {
        //alert($("input[name$ ='game.edate']']").val());
        var dota2begin_at = $(this).find("input[name$='dota2begin_at']").val();
        var team1 = $(this).find("input[name$='team1']").val();
        var team2 = $(this).find("input[name$='team2']").val();
        var league = $(this).find("input[name$='league']").val();
        $("#game_edate,#game_gamedate").val(dota2begin_at);

        $("#game_title").val(team1 + " VS " + team2);
        $("#game_comment").val(league);
        //$("#game_edate").val(dota2begin_at);
        $("#gamepost_TeamASn optgroup option").filter(function () {
            //may want to use $.trim in here
            return $(this).text() == team1;
        }).prop('selected', true);
        $("#gamepost_TeamBSn optgroup option").filter(function () {
            //may want to use $.trim in here
            return $(this).text() == team2;
        }).prop('selected', true);



    });


});