function toggleDropdown(id, event) {
  if (event) event.stopPropagation();

  const menu = document.getElementById(id);
  if (!menu) return;

  document.querySelectorAll(".dropdown-list").forEach((item) => {
    if (item !== menu) item.classList.remove("show");
  });

  menu.classList.toggle("show");
}

function toggleMobileMenu(event) {
  if (event) event.stopPropagation();

  const nav = document.getElementById("navContent");
  if (nav) nav.classList.toggle("show");
}

document.addEventListener("click", (event) => {
  if (!event.target.closest(".custom-dropdown")) {
    document.querySelectorAll(".dropdown-list").forEach((item) => {
      item.classList.remove("show");
    });
  }

  const link = event.target.closest("[data-close-menu]");
  const nav = document.getElementById("navContent");
  if (link && nav && window.innerWidth <= 1400) nav.classList.remove("show");
});

window.addEventListener("resize", () => {
  const nav = document.getElementById("navContent");
  if (nav && window.innerWidth > 1400) nav.classList.remove("show");
});

document.addEventListener("DOMContentLoaded", () => {
  const nav = document.getElementById("navContent");
  if (nav) nav.classList.remove("show");
});
