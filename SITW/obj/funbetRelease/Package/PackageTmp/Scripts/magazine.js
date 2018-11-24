
jQuery(function($) {
	
	var $container = $(".masonry");

	$container.imagesLoaded( function() {
		$container.masonry();
	});
	
	$(".go-top").on("click",function(event) {
		event.preventDefault();
		$("html, body").animate({scrollTop: 0}, 600);
	});

	// Append Masonry elements
	var pagenum  = 1;
	var pagetotal = $(".loadmore").data("total");

	$(".loadmore").on("click",function(event) {
		event.preventDefault();

		pagenum++;

		if (pagenum <= pagetotal) {

			var url = document.URL + "/page:" + pagenum;

			$.get(url, function( result ) {

				var items = $(result).find(".item");

				// append items to grid
				$(".masonry").append( items )
				// add and lay out newly appended items
				.masonry( "appended", items );

				$container.imagesLoaded( function() {
					$container.masonry();
				});
			});

		}

		if (pagenum == pagetotal) {
			$(this).hide();
		}

	});

});
