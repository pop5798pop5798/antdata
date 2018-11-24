
$(document).ready(function () {
    var timedatatype = false;
    var dtats = 0;

    $(".allgamelist").hide();

    GameListType();
    $("#title02 h2").click(function () {
        if ($("#triangleattr").attr("class") == "triangle") {
            $("#triangleattr").attr('class', 'triangleopen');
            $(".allgamelist").slideDown(600);
        } else {
            $("#triangleattr").attr('class', 'triangle');
            $(".allgamelist").slideUp(600);
        }

    });

    jQuery(window).resize(function () {
        GameListType();

    });





    $(".allgamelist li").on("click", function (even) {
        var s = $(this).val();
        UpdateGameList(s);



    });






    




    /*賽局動態旋轉 */
                /*var setover = true;

                $("#gamelistst").mouseover(function () {
                    setTimeout(function () {
                        $(".rankallbody").stop().attr('id', 'allrank');
                    }, 680);
                    setover = false;

                });

                $("#allrank").mouseover(function () {

                    setover = true;

                });

                if (setover)
                {
                    setInterval(Hovering, 180000);

                }*/



    });




function Dtime() {
    $("[name=sportsmagazine-countdown]").each(
        function (i, obj) {
            var d = new Date($(obj).siblings("[name=gametime]").val());
            var dt = new Date();
            var dtuptime = dt.getTime() + 1800000;
            var dtvalue = new Date(dtuptime);

            $(obj).countdown({
                until: d,
                compact: true,
                layout: '剩 <b>{dn} 天 {hnn} 小時 {mnn} 分 {desc}'
            });


            $(obj).css("color", "#ebdec2");


            if (Date.parse(d).valueOf() < Date.parse(dt).valueOf()) {
                $(obj).html("比賽進行中...");
                $(obj).css("color", "#FF5722");
            } else if (Date.parse(d).valueOf() < Date.parse(dtvalue).valueOf()) {
                $(obj).css("color", "rgb(231, 177, 68)");
            }


        });


    var font = $("#tableview").val();
    if (font) {
        $("#tablegamenull").css("display", "none");
    } else {

        $("#tablegamenull").css("display", "black");
    }


    var that = $(this);
    var mSearch = $("#m-search");
    $("#search-input").bind("change paste keyup", function () {
        var value = $(this).val();
        if (!value) {
            mSearch.html("");
            return;
        };
        mSearch.html('.gamebody:not([data-index*="' + value.toLowerCase() + '"]) {display: none;}');
    });

}


function GameListType() {
    var wdth = $(window).width();
    if (wdth <= 974) {
        $(".sportsmagazine-wishlist").show();
        $(".allgamelist").hide();
        $("#triangleattr").attr('class', 'triangle');
        $("#title02 h2").html("全部遊戲");
    } else {
        $(".allgamelist").slideDown(600);
        $("#title02 h2").html("遊戲選單");
        $("#triangleattr").attr('class', 'triangleopen');
    }

}




function canvasmovie() {

    $("tr canvas").each(function (index, obj) {
        var newid = $(obj).attr('id');


        const noise = () => {
            let canvas, ctx;

            let wWidth, wHeight;

            let noiseData = [];
            let frame = 0;

            let loopTimeout;


            // Create Noise
            const createNoise = () => {
                const idata = ctx.createImageData(wWidth, wHeight);
                const buffer32 = new Uint32Array(idata.data.buffer);
                const len = buffer32.length;

                for (let i = 0; i < len; i++) {
                    if (Math.random() < 0.5) {
                        buffer32[i] = 0xff000000;
                    }
                }

                noiseData.push(idata);
            };


            // Play Noise
            const paintNoise = () => {
                if (frame === 9) {
                    frame = 0;
                } else {
                    frame++;
                }

                ctx.putImageData(noiseData[frame], 0, 0);
            };


            // Loop
            const loop = () => {
                paintNoise(frame);

                loopTimeout = window.setTimeout(() => {
                    window.requestAnimationFrame(loop);
                }, (1000 / 25));
            };


            // Setup
            const setup = () => {
                wWidth = window.innerWidth;
                wHeight = window.innerHeight;

                canvas.width = wWidth;
                canvas.height = wHeight;

                for (let i = 0; i < 10; i++) {
                    createNoise();
                }

                loop();
            };


            // Reset
            let resizeThrottle;
            const reset = () => {
                window.addEventListener('resize', () => {
                    window.clearTimeout(resizeThrottle);

                    resizeThrottle = window.setTimeout(() => {
                        window.clearTimeout(loopTimeout);
                        setup();
                    }, 200);
                }, false);
            };


            // Init
            const init = (() => {
                canvas = document.getElementById(newid);
                ctx = canvas.getContext('2d');

                setup();
            })();
        };

        noise();


    });





}

function MovieClick() {
    $("tr #movie_iframe").each(function (index, obj) {
        $(obj).hide();


        $(obj).siblings("span").on('click', function () {
            $(obj).slideToggle(500);
            var vedival = $(obj).find("#vedioval").val();
            var ifvalue = $(obj).find("iframe").attr("src");

            if (ifvalue != "") {
                setTimeout(function () {
                    $(obj).find("iframe").attr("src", "");

                }, 200);

            } else if (ifvalue == "") {

                setTimeout(function () {
                    $(obj).find("iframe").attr("src", vedival);

                }, 500);
            }

            $("tr #movie_iframe").find("iframe").not(obj).attr("src", "");
            $("tr #movie_iframe").not(obj).slideUp();


        });


    });


}


