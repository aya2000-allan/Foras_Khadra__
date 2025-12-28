// دالة لتحميل الملفات الجزئية (Header, Navbar, Footer)
function loadPartial(id, file) {
    fetch(file)
        .then(response => response.text())
        .then(data => document.getElementById(id).innerHTML = data);
}

// تحميل الملفات الجزئية تلقائيًا
document.addEventListener("DOMContentLoaded", function () {
    loadPartial('header', 'partials/header.html');
    loadPartial('navbar', 'partials/navbar.html');
    loadPartial('footer', 'partials/footer.html');
});
