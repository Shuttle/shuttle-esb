$(function(){
	var highlighter = function(type, text) {
			var result;
		
			try	{
				result = hljs.highlight(type, text).value; 
			}
			catch (e) {
				result = Flatdoc.highlighters.generic(text);
			}
			
			return result;
		};

	if (window.Flatdoc && flatdocFileUrl && flatdocFileUrl.length){
		Flatdoc.highlighters.csharp = Flatdoc.highlighters.cs = Flatdoc.highlighters['c#'] = function(text) {
			return highlighter('cs', text);
		}

		Flatdoc.highlighters.css = function(text) {
			return highlighter('css', text);
		}

		Flatdoc.highlighters.javascript = Flatdoc.highlighters.js = function(text) {
			return highlighter('javascript', text);
		}

		Flatdoc.highlighters.json = function(text) {
			return highlighter('json', text);
		}

		Flatdoc.highlighters.markdown = Flatdoc.highlighters.md = function(text) {
			return highlighter('markdown', text);
		}

		Flatdoc.highlighters.sql = function(text) {
			return highlighter('sql', text);
		}

		Flatdoc.highlighters.vbnet = Flatdoc.highlighters.vb = function(text) {
			return highlighter('vbnet', text);
		}

		Flatdoc.highlighters.xml = function(text) {
			return highlighter('xml', text);
		}
	
		Flatdoc.run({
		  fetcher: Flatdoc.file(baseurl + flatdocFileUrl)
		});	
	}
})