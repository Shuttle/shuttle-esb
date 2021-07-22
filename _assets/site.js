import marked from 'marked';

const site = {
    decode(input) {
        var e = document.createElement('div');
        e.innerHTML = decodeURIComponent(input);
        return e.childNodes[0].nodeValue.replace(/\<\s*br\s*\/\>/g, '\n');
    },

    performSearch() {
        var idx;
        var headerIndex = 0;

        $.ajax({
            cache: true,
            url: baseurl + '/content.json?' + version,
            dataType: 'json'
        })
            .done(function (data) {
                window.searchData = data;

                idx = lunr(function () {
                    var self = this;

                    this.ref('id');
                    this.field('title', { boost: 10 });
                    this.field('url');
                    this.field('content');

                    $.each(data, function (index, entry) {
                        self.add($.extend({ "id": index }, entry))
                    });
                });

                var results = idx.search(new URLSearchParams(window.location.search).get('match'));

                $.each(results, function (index, result) {
                    var summaryId = 'search-result-summary-' + headerIndex;
                    var headerId = 'search-result-header-' + headerIndex;

                    var entry = window.searchData[result.ref];

                    if (!entry.title) {
                        return;
                    }

                    $('#search-results').append('<div class="card"><div><h5 class="card-header mr-auto collapsed" data-toggle="collapse" data-target="#' + summaryId + '" aria-expanded="false" aria-controls="' + summaryId + '" id="' + headerId + '" class="d-block">' + entry.title + '<a class="p-2" href="' + entry.url + '"><i class="fas fa-ellipsis-h"></i></a><i class="fas fa-chevron-down float-right"></i></h5></div><div id="' + summaryId + '" class="card-body collapse" aria-labelledby="' + headerId + '"><p class="card-text">' + marked(site.decode(entry.summary)) + '</p></div></div><br/>');

                    headerIndex += 1;
                });

                if (!results.length) {
                    $('#search-results').append('<div class="alert alert-info" role="alert">That\'s a miss :(</div>');
                }

                $('#search-results-container .card-header a').on('click', function (e) { e.stopPropagation(); });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $('#search-results-container').html('<p class="alert-danger">' + errorThrown + '</p>');
            });
    },
    copyToClipboard(text) {
        if (!navigator.clipboard) {
            const el = document.createElement("textarea");
            el.value = text;
            el.setAttribute("readonly", "");
            el.style.position = "absolute";
            el.style.left = "-9999px";
            document.body.appendChild(el);
            const selected =
                document.getSelection().rangeCount > 0
                    ? document.getSelection().getRangeAt(0)
                    : false;
            el.focus();
            el.select();

            try {
                var successful = document.execCommand('copy');
                var msg = successful ? 'successful' : 'unsuccessful';
                console.log('Fallback: Copying text command was ' + msg);
            } catch (err) {
                console.error('Fallback: Oops, unable to copy', err);
            }

            document.body.removeChild(el);
            if (selected) {
                document.getSelection().removeAllRanges();
                document.getSelection().addRange(selected);
            }

            return;
        }

        navigator.clipboard.writeText(text).then(function () {
            console.log('Async: Copying to clipboard was successful!');
        }, function (err) {
            console.error('Async: Could not copy text: ', err);
        });
    },
    handleCopyClick(evt) {
        const divs = $(evt.target).parents(".highlight");

        if (divs.length > 0) {
            site.copyToClipboard(divs[0].innerText);
        } else {
            console.log("Could not copy text.");
        }
    },
    applyTheme() {
        let theme = localStorage.getItem("theme");
        var removeClass = "fa-moon";
        var addClass = "fa-sun";

        if (!theme) {
            theme = "dark"
            localStorage.setItem("theme", "dark");
            document.documentElement.setAttribute("theme", "dark");
        }

        $("head link#theme-stylesheet").attr('href', baseurl + '/packed/' + theme + '.css?' + version);

        switch (theme) {
            case "light": {
                removeClass = "fa-sun";
                addClass = "fa-moon";
            }
        }

        $("#theme-switcher").removeClass(removeClass);
        $("#theme-switcher").addClass(addClass);
    },
    toggleTheme() {
        let theme = localStorage.getItem("theme");
        let htmlElement = document.documentElement;

        if (theme == "dark") {
            localStorage.setItem("theme", "light");
            htmlElement.setAttribute("theme", "light");
        } else {
            localStorage.setItem("theme", "dark");
            htmlElement.setAttribute("theme", "dark");
        }

        applyTheme();
    }
}

export default site;

$(function () {
    $('table').addClass('table table-striped');

    $('#search-button').on('click', function (e) {
        e.preventDefault()

        if (!$('#search').val()) {
            return;
        }

        window.location.replace(window.location.origin + baseurl + '/search?match=' + $('#search').val());
    })

    // get the list of all highlight code blocks
    const highlights = document.querySelectorAll("div.highlight")

    highlights.forEach(div => {
        const copy = document.createElement("button");
        copy.innerHTML = `<i class="far fa-copy fa-2x"></i>`;
        copy.setAttribute("data-toggle", "tooltip");
        copy.setAttribute("data-placement", "top");
        copy.setAttribute("title", "Copied!");
        copy.addEventListener("click", site.handleCopyClick);
        div.append(copy);
    });

    $('[data-toggle="tooltip"]').tooltip({
        trigger: "click",
        delay: { "show": 100, "hide": 500 }
    });

    $(document).on('shown.bs.tooltip', function (e) {
        setTimeout(function () {
            $(e.target).tooltip('hide');
        }, 500);
    });

    site.applyTheme();
});