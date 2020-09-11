function getArticles(index) {

    let url = "/Article/GetArticles?index=" + index;

    $.ajax({
        url: url,
        type: 'get',
        dataType: 'text',
        success: function (data) {
            let articles = JSON.parse(data.toString());
            articles.forEach(element => {
                createArticle(element);
            });
        },
        error: error => console.log(error)
    });
}

function createArticle(article) {
    var html = `
        <div class="col-md-4 d-flex">
            <div class="blog-entry">
                <a asp-controller="Article" asp-action="Detail" class="img" style="background-image: url(${article.CoverUrl});"></a>
                <div class="text p-4">
                    <h3 class="mb-2"><a asp-controller="Article" asp-action="Detail">${article.Title}</a></h3>
                    <div class="meta-wrap">
                        <p class="meta">
                            <span>${article.Date}</span>
                            <span>0 Comment</span>
                        </p>
                    </div>
                    <p class="mb-4">${article.Overview}</p>
                    <p><a asp-controller="Article" asp-action="Detail" class="btn-custom">Read More</span></a></p>
                </div>
            </div>
        </div>
    `;

    document.querySelector("#post > div").innerHTML += html;
}