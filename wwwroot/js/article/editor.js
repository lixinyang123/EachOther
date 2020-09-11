function upload() {
    document.querySelector("#articleContent").value = CKEDITOR.instances.htmlEditor.getData();
    document.querySelector("form").submit();
}

CKEDITOR.replace('htmlEditor');