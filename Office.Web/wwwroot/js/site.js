// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById("FormFile").onchange = function () {
    var reader = new FileReader();
    if (this.files[0].size > 528385) {
        alert("Image Size should not be greater than 500Kb");
        $("#menu_image").attr("src", "blank");
        $("#menu_image").hide();
        $('#FormFile').wrap('<form>').closest('form').get(0).reset();
        $('#FormFile').unwrap();
        return false;
    }
    if (this.files[0].type.indexOf("image") == -1) {
        alert("Invalid Type");
        $("#menu_image").attr("src", "blank");
        $("#menu_image").hide();
        $('#FormFile').wrap('<form>').closest('form').get(0).reset();
        $('#FormFile').unwrap();
        return false;
    }
    reader.onload = function (e) {
        // get loaded data and render thumbnail.
        document.getElementById("menu_image").src = e.target.result;
        $("#menu_image").show();
    };

    // read the image file as a data URL.
    reader.readAsDataURL(this.files[0]);
};
