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