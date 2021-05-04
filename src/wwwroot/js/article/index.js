let index = 1;
let pageCount = 0;
let loadingState = isEnd = false;

function getArticles() {

    loadingState = true;
    let url = "/Article/GetArticles?index=" + index;

    $.ajax({
        url: url,
        type: 'get',
        dataType: 'text',
        success: function (data) {
            let articles = JSON.parse(data.toString());
            if(articles.length != 0){
                
                document.querySelector("#loading").setAttribute("hidden","hidden");

                for(let i=0;i<articles.length;i++){
                    if(i%2 == 0) {
                        createArticle(articles[i],"active");
                    }
                    else{
                        createArticle(articles[i],"");
                    }
                }

                loadingState = false;
            }
            else {
                document.querySelector("#nothing").removeAttribute("hidden");
            }
        },
        error: error => console.log(error)
    });
}

function createArticle(article, active) {

    let html = `
        <div class="col-md-4 d-flex animate__animated animate__jackInTheBox">
            <div class="blog-entry ${active}">
                <a href="/Article/Detail/${article.ArticleCode}" class="img" style="background-image: url(${article.CoverUrl});"></a>
                <div class="text p-4">
                    <h3 class="mb-2"><a href="/Article/Detail/${article.ArticleCode}" >${article.Title}</a></h3>
                    <div class="meta-wrap">
                        <p class="meta">
                            <span>${article.Date}</span>
                            <span>|</span>
                            <span>${article.Comments.length} Comment</span>
                        </p>
                    </div>
                    <p class="mb-4">${article.Overview}</p>
                    <p><a href="/Article/Detail/${article.ArticleCode}" class="btn-custom">Read More</span></a></p>
                </div>
            </div>
        </div>
    `;

    document.querySelector("#post > div.row.no-gutters").innerHTML += html;
}

function showEnd() {
    document.querySelector("#endWarning").removeAttribute("hidden");
    isEnd = true;
}

function init() {
    pageCount = Number(document.querySelector("#pageCount").value);
    getArticles();

    window.onscroll = () => {
        // check is top
        if (window.scrollY > 200) {
            document.querySelector(".js-top").classList.add("active");
        }
        else {
            document.querySelector(".js-top").classList.remove("active");
        }
        
        // check buttom
        if(window.innerHeight + window.scrollY >= document.body.scrollHeight-1){
            if(index < pageCount) {
                if(!loadingState) {
                    getArticles(++index);
                }
            }
            else{
                if(!isEnd) { showEnd(); }
            }
        }
    }
}