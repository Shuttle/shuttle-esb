$(function(){
	var setTOC = function() {
		$("#toc").width($('#toc-container').width())
	};
    var move = function() {
        var st = $(window).scrollTop();
        var ot = $("#toc-anchor").offset().top;
        var s = $("#toc");
        if(st > ot) {
            s.css({
                position: "fixed",
                top: "15px"
            });
        } else {
            if(st <= ot) {
                s.css({
                    position: "relative",
                    top: ""
                });
            }
        }
    };
	
	setTOC();

	$(window).resize(setTOC);

	$("#toc")
		.tocify({
			theme: 'bootstrap'
		})
		.data('toc-tocify');
	
	$('table').addClass('table table-hover table-condensed table-responsive');
	
    $(window).scroll(move);
    move();	
})