// Care Clinic Manager — shared client-side behaviour.
(function () {
    'use strict';

    // Collapse the mobile nav after a link is tapped.
    document.addEventListener('click', function (e) {
        var link = e.target.closest('.navbar-nav a, .navbar-nav button');
        if (!link) return;
        var collapse = document.getElementById('mainNav');
        if (collapse && collapse.classList.contains('show')) {
            var toggle = document.querySelector('.navbar-toggler');
            if (toggle) toggle.click();
        }
    });
})();
