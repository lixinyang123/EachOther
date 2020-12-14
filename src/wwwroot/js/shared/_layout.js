function MoveTop()
{
	$("html,body").animate({ scrollTop: 0 }, 500);
}

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