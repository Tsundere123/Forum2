$(document).ready(function () {
    // Ensure that post content images are responsive
    $(".post-content img").addClass("img-fluid");

    // Remove paragraph margin on post content
    $(".post-content p:last-child").addClass("m-0");

    $(".post-content table").addClass("table table-sm table-striped table-bordered");

    // Add blockquote class to blockquotes
    $("blockquote").addClass("post-content-quote");
});

// Tooltips
// From https://getbootstrap.com/docs/5.1/components/tooltips/
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})

function parseQuoteIn(string) {
    const parsed = new DOMParser().parseFromString(string, "text/html").documentElement.textContent;
    return handleQuoteIn(parsed);
}

function handleQuoteIn(string) {
    // Remove newlines
    string = string.replace(/\n/g, " ");

    // Remove tags
    string = string.replace(/<\/?[^>]+(>|$)/g, "");

    // Add markdown blockquote 
    string = string.replace(/^/gm, "> ");
    return string;
}

function handleQuoteNewPost(string) {
    if (string.length > 0)
    {
        string = string + "\n\n";
    }
    
    return string;
}