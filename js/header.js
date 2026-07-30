(function () {
    const menuBtn = document.querySelector(".menu-btn");
    const navLinks = document.querySelector(".nav-links");

    if (!menuBtn || !navLinks) return;

    function closeMenu() {
        navLinks.classList.remove("open");
        menuBtn.setAttribute("aria-expanded", "false");
    }

    menuBtn.addEventListener("click", function (e) {
        e.stopPropagation();
        const isOpen = navLinks.classList.toggle("open");
        menuBtn.setAttribute("aria-expanded", isOpen ? "true" : "false");
    });

    document.addEventListener("click", function (e) {
        if (
            navLinks.classList.contains("open") &&
            !navLinks.contains(e.target) &&
            !menuBtn.contains(e.target)
        ) {
            closeMenu();
        }
    });

    // close when a link is tapped
    navLinks.querySelectorAll("a").forEach(function (link) {
        link.addEventListener("click", closeMenu);
    });

    window.addEventListener("resize", function () {
        if (window.innerWidth > 768) closeMenu();
    });
})();
