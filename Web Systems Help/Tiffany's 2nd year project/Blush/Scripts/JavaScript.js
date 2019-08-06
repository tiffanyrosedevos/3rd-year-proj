$("#myLink").click(function (e) {

    e.preventDefault();
    $.ajax({

        url: $(this).attr("href"), // comma here instead of semicolon   
        success: function () {
            alert("Added to Cart");  // or any other indication if you want to show
        }

    });

});