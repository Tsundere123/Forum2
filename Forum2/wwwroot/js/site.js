$(document).ready(function () {
    // Ensure that post content images are responsive
    $(".post-content img").addClass("img-fluid");

    // Remove paragraph margin on post content
    $(".post-content p:last-child").addClass("m-0");

    $(".post-content table").addClass("table table-sm table-striped table-bordered");

    // Add blockquote class to blockquotes
    $("blockquote").addClass("post-content-quote");
});