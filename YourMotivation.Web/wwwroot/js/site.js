$("li.disabled > a").on("click", function (e) {
  e.preventDefault();
});

$("a.disabled").on("click", function (e) {
  e.preventDefault();
});

$("button.disabled").on("click", function (e) {
  e.preventDefault();
});

$('#NewTransferBlockCollapseBtn').click(function () {
  var htmlContent = $('#NewTransferBlockCollapseBtn').html();
  if ($('#NewTransferBlockCollapseBtn span').hasClass('glyphicon-plus')) {
    htmlContent = htmlContent.replace('glyphicon-plus', 'glyphicon-minus');
  }
  else {
    htmlContent = htmlContent.replace('glyphicon-minus', 'glyphicon-plus');
  };

  $('#NewTransferBlockCollapseBtn').html(htmlContent);
});


$(function () {
  $("[data-autocomplete-source]").each(function () {
    var target = $(this);
    target.autocomplete({
      source: target.attr("data-autocomplete-source"),
      classes: { "ui-autocomplete": "ui-state-highlight" }
    });
  });
});