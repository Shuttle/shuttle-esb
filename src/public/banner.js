setInterval(function() {
    if (!document.getElementById("pendel-banner")) {
        var banner = document.createElement("div");

        banner.id = "pendel-banner";
        banner.textContent = "All Shuttle documentation is now available at ";
        banner.style.cssText = "font-size: 0; padding: 0; transition: font-size 0.5s linear, padding 0.5s linear;";

        var a = document.createElement("a");
        a.href = "https://www.pendel.co.za";
        a.textContent = "pendel.co.za";
        a.style.color = "var(--vt-c-orange-dark)";

        banner.appendChild(a);

        var e = document.querySelector(".VPContentPage");

        if (!!e) {
            banner.style.textAlign = "center";
        } else {
            var parent = document.querySelector(".VPContentDoc");

            if (!!parent) {
                e = parent.querySelector(".content");
            }
        }

        if (!!e) {
            e.insertBefore(banner, e.firstChild);

            setTimeout(() => {
                banner.style.fontSize = "1.5rem";
                banner.style.padding= "1rem 0";
            }, 500);
        }
    }
}, 1000);
