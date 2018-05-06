$("li.disabled > a").on("click", function (e) {
  e.preventDefault();
});

$("a.disabled").on("click", function (e) {
  e.preventDefault();
});

$("button.disabled").on("click", function (e) {
  e.preventDefault();
});