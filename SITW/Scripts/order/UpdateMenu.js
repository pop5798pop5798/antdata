
function updateAll()
{
    updateAssets();
    updateMissionNote();
}

function updateAssets()
{
    $.get("/Assets/_getAssetsString", function (data) {
        $("#spanAssets").html(data);
    });
    
}

function updateMissionNote()
{

    $.getJSON("/Mission/getMissionCountString", function (data) {
        $("#spanMissionNumber").html(data.MissionNumber);
        $("#spanMissionFinshNumber").html(data.MissionFinshNumber);
    });
    
}

