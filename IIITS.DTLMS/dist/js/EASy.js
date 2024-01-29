$("a").hover(
  function () {
      var title = $(this).attr("data-title");
      $('<div/>', {
          text: title,
          class: 'box'
      }).appendTo(this);
  }, function () {
      $(document).find("div.box").remove();
  }
);

function alertwithredirect(EasyTitle, EasyText, EasyType, EasyRedirectPath) {
    swal({ title: EasyTitle,
        text: EasyText,
        type: EasyType,
        showCancelButton: false
    },
        function () {
            window.location = EasyRedirectPath;
        });
}

function alertBox(EasyTitle, EasyText, EasyType) {
    swal({ title: EasyTitle,
        text: EasyText,
        type: EasyType
    });
}