$(function(){
	var i;
	var item;
	var hasTOC = $('#toc').length > 0;
	var tocGuide = $('#toc-guide');
	var hasTOCGuide = tocGuide.length > 0;
	
	var setTOC = function() {
		var toc = $('#toc');
		
		if (!hasTOC){
			return;
		}
	
		$('#toc').width($('#toc-container').width())
	};
	
    var move = function() {
		var st = $(window).scrollTop();
		var ot;
		var s;
	
		//$('#footer').css('position', (st + $(window).height()) < ($(document).height() - $(window).height()) ? 'inherit' : 'fixed');

		if (!hasTOC) {
			return;
		}
		
        ot = $('#toc-anchor').offset().top;
        s = $('#toc');
		
        if(st > ot) {
            s.css({
                position: 'fixed',
                top: '15px'
            });
        } else {
            if(st <= ot) {
                s.css({
                    position: 'relative',
                    top: ''
                });
            }
        }
    };
	
	setTOC();

	$(window).resize(setTOC);

	if (hasTOC) {
		$('#toc')
			.tocify({
				theme: 'bootstrap',
				showAndHide: false,
				showAndHideOnScroll: false
			})
			.data('toc-tocify');

		if (hasTOCGuide && shuttle.guideData && shuttle.guideData.items && $.isArray(shuttle.guideData.items)) {
			for (i = 0; i < shuttle.guideData.items.length; i++){
				item = shuttle.guideData.items[i];

				if (item.name == (shuttle.guideData.selectedItemName || '')) {
					continue;
				}
				
				$('#toc-guide-list').append('<li><a href="' + shuttle.baseurl + '/' + item.name + '/index.html">' + item.text + '</a></li>');
			}

			tocGuide.remove();
			$('#toc').append(tocGuide);
			
			if (shuttle.guideData.title) {
				$('#toc-guide-title').text(shuttle.guideData.title);
			}
		} else {
			if (hasTOCGuide) {
				tocGuide.remove();
			}
		};
	}
	
	$('table').addClass('table table-hover table-condensed table-responsive');
	
    $(window).scroll(move);
    move();
	
	shuttle.notify(shuttle.notifyOptions);
})

shuttle.notify = function(options) {
	var notification = $('#notification');
	var o = options || {};
	
	if (o.type == undefined || !notification) {
		return;
	}
	
	switch (o.type.toLowerCase()) {
		case 'construction': {
			notification.text('This documentation is currently being developed and may not yet be complete.');
			notification.addClass('alert alert-warning');
			
			break;
		}
	}
	
	notification.show();
}