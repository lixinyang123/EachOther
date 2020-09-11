function getArticles(index) {

    var url = "/Article/GetArticles?index=" + index;

    $.ajax({
        url: url,
        type: 'get',
        dataType: 'text',
        success: function (data) {
            console.log(JSON.parse(data.toString()));
        },
        error: error => console.log(error)
    });
}

function createArticle(article) {
    var html = `
        <div class="col-md-4 d-flex">
            <div class="blog-entry active">
                <a asp-controller="Article" asp-action="Detail" class="img" style="background-image: url(/images/f.jpg);"></a>
                <div class="text p-4">
                    <h3 class="mb-2"><a asp-controller="Article" asp-action="Detail">宝贝，七夕快乐~</a></h3>
                    <div class="meta-wrap">
                        <p class="meta">
                            <span>Aug. 24, 2020</span>
                            <span>0 Comment</span>
                        </p>
                    </div>
                    <p class="mb-4">宝贝，我欠你的500字情书</p>
                    <p class="mb-4">希望你能喜欢😘</p>
                    <p><a asp-controller="Article" asp-action="Detail" class="btn-custom">Read More</span></a></p>
                </div>
            </div>
        </div>
    `;
}