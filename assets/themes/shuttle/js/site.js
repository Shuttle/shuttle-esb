$(function(){
	var hasTOC = $("#toc").length > 0;
	
	var setTOC = function() {
		var toc = $("#toc");
		
		if (!hasTOC){
			return;
		}
	
		$("#toc").width($('#toc-container').width())
	};
	
    var move = function() {
		var st = $(window).scrollTop();
		var ot;
		var s;
	
		$('#footer').css('position', (st + $(window).height()) < ($(document).height() - $(window).height()) ? 'inherit' : 'fixed');

		if (!hasTOC) {
			return;
		}
		
        ot = $("#toc-anchor").offset().top;
        s = $("#toc");
		
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

	if (hasTOC) {
		$("#toc")
			.tocify({
				theme: 'bootstrap'
			})
			.data('toc-tocify');
	}
	
	$('table').addClass('table table-hover table-condensed table-responsive');
	
    $(window).scroll(move);
    move();	
})