function getArticles(index) {

    let url = "/Article/GetArticles?index=" + index;

    $.ajax({
        url: url,
        type: 'get',
        dataType: 'text',
        success: function (data) {
            let articles = JSON.parse(data.toString());
            for(let i=0;i<articles.length;i++){
                if(i%2 == 0) {
                    createArticle(articles[i],"active");
                }
                else{
                    createArticle(articles[i],"");
                }
            }
        },
        error: error => console.log(error)
    });
}

function createArticle(article, active) {

    var html = `
        <div class="col-md-4 d-flex">
            <div class="blog-entry ${active}">
                <a href="/Article/Detail/${article.ArticleCode}" class="img" style="background-image: url(${article.CoverUrl});"></a>
                <div class="text p-4">
                    <h3 class="mb-2"><a href="/Article/Detail/${article.ArticleCode}" >${article.Title}</a></h3>
                    <div class="meta-wrap">
                        <p class="meta">
                            <span>${article.Date}</span>
                            <span>|</span>
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