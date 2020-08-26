function upload() {
    document.querySelector("#content").value = CKEDITOR.instances.htmlEditor.getData();
    document.querySelector("form").submit();
}