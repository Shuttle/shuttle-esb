$(function(){
	if (flatdocFileUrl && flatdocFileUrl.length){
		Flatdoc.run({
		  fetcher: Flatdoc.file(baseurl + flatdocFileUrl)
		});	
	}
})