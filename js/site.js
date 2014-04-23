$(function(){
	var setTOC = function() {
		$("#toc").width($('#toc-container').width())
	};
	
	setTOC();

	$("#toc")
		.tocify({
			theme: 'bootstrap'
		})
		.data('toc-tocify');
		
	$( window ).resize(setTOC);
	
	$('table').addClass('table');
})