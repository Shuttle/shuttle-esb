decode = function(input){
    var e = document.createElement('div');
    e.innerHTML = decodeURIComponent(input);
    return e.childNodes[0].nodeValue.replace(/\<\s*br\s*\/\>/g, '\n');
}

performSearch = function (version) {
    var idx;
    var headerIndex = 0;

    $.ajax({
        cache: true,
        url: baseurl + '/content.json?' + version,
        dataType: 'json'
    })
        .done(function(data){
            window.searchData = data;

            idx = lunr(function () {
                var self = this;

                this.ref('id');
                this.field('title', {boost: 10});
                this.field('url');
                this.field('content');

                $.each(data, function (index, entry) {
                    self.add($.extend({"id": index}, entry))
                });
            });

            results = idx.search(new URLSearchParams(window.location.search).get('match'));

            $('#search-results-container').html('<h2>Search Results</h2><br/>');
            $('#search-results-container').append('<div id="search-results"></div>');

            $.each(results, function (index, result) {
                var summaryId = 'search-result-summary-' + headerIndex;
                var headerId = 'search-result-header-' + headerIndex;

                entry = window.searchData[result.ref];

                if (!entry.title){
                    return;
                }

                $('#search-results').append('<div class="card"><div><h5 class="card-header mr-auto collapsed" data-toggle="collapse" data-target="#' + summaryId + '" aria-expanded="false" aria-controls="' + summaryId + '" id="' + headerId + '" class="d-block">' + entry.title + '<a class="p-2" href="' + entry.url + '"><i class="fas fa-external-link-square-alt"></i></a><i class="fas fa-chevron-down float-right"></i></h5></div><div id="' + summaryId + '" class="card-body collapse" aria-labelledby="' + headerId + '"><p class="card-text">' + marked(decode(entry.summary)) + '</p></div></div><br/>');

                headerIndex += 1;
            });

            if (!results.length){
                $('#search-results').append('<div class="alert alert-info" role="alert">That\'s a miss :(</div>');            
            }

            $('#search-results-container .card-header a').on('click', function(e) { e.stopPropagation(); });
        })
        .fail(function( jqXHR, textStatus, errorThrown){
            $('#search-results-container').html('<p class="alert-danger">' + errorThrown + '</p>');
        });
}

$(function () {
    $('table').addClass('table table-striped');

    $('#search-button').on('click', function (e) {
        e.preventDefault()

        if (!$('#search').val()) {
            return;
        }

        window.location.replace(window.location.origin + baseurl + '/search?match=' + $('#search').val());
    })
});