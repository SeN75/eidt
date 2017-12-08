$(document).ready(function () {
	var WR1 = $('#W-r-1');
	
	WR1.css('opacity', 0);
	$('#W-r-1').waypoint(function () {
		WR1.animate({
			opacity: 1
		}, 1800), WR1.addClass("slideInRight");
	}, {
		offset: '50%'
	});
	
});
$(document).ready(function () {
	var WL1 = $('#W-l-1');
	WL1.css('opacity', 0);
	$('#W-l-1').waypoint(function () {
		WL1.animate({
			opacity: 1
		}, 1800), WL1.addClass("slideInRight");
	}, {
		offset: '50%'
	});
	
	
});

$(document).ready(function () {
	var WR2 = $('#W-r-2');
	WR2.css('opacity', 0);
	$('#W-r-2').waypoint(function () {
		WR2.animate({
			opacity: 1
		}, 1800), WR2.addClass("slideInRight");
	}, {
		offset: '50%'
	});
	
});

$(document).ready(function () {
	var WT = $('.W-tt');
	WT.css('opacity', 0);
	$('.W-t').waypoint(function () {
		WT.animate({
			opacity: 1
		}, 1800), WT.addClass("slideInUp");
	}, {
		offset: '50%'
	});
	
});
$(document).ready(function () {
	var WP = $('#W-p');
	WP.css('opacity', 0);
	$('#W-p').waypoint(function () {
		WP.animate({
			opacity: 1
		}, 1800), WP.addClass("bounceIn");
	}, {
		offset: '50%'
	});
	
});
$(document).ready(function () {
	var WP = $('#W-t');
	WP.css('opacity', 0);
	$('#W-t').waypoint(function () {
		WP.animate({
			opacity: 1
		}, 3000), WP.addClass("slideInDown");
	}, {
		offset: '50%'
	});
	
});
$(document).ready(function () {
	var WP = $('#W-sh');
	WP.css('opacity', 0);
	$('#W-sh').waypoint(function () {
		WP.animate({
			opacity: 1
		}, 1800), WP.addClass("slideInRight");
	}, {
		offset: '50%'
	});
	
});
$(document).ready(function () {
	var WP = $('#W-a');
	WP.css('opacity', 0);
	$('#W-a').waypoint(function () {
		WP.animate({
			opacity: 1
		}, 1800), WP.addClass("slideInRight");
	}, {
		offset: '50%'
	});
	
});
$(document).ready(function () {
	var WP = $('.W-ll');
	WP.css('opacity', 0);
	$('.W-l').waypoint(function () {
		WP.animate({
			opacity: 1
		}, 1800), WP.addClass("slideInUp");
	}, {
		offset: '50%'
	});
	
});$(document).ready(function () {
	var WP = $('#W-m');
	WP.css('opacity', 0);
	$('.W-l').waypoint(function () {
		WP.animate({
			opacity: 1
		}, 3000), WP.addClass("fadeInDownBig");
	}, {
		offset: '50%'
	});
	
});

$(document).ready(function () {
        $('map').imageMapResize();
    });
