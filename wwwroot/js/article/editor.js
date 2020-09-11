function uploadingState(flag, text) {
    var uploading = document.querySelector("#uploading");
    if (flag) {
        uploading.innerText = "Uploading...";
        uploading.setAttribute("disabled", "disabled");
    }
    else {
        uploading.innerText = text;
        uploading.removeAttribute("disabled");
    }
}

function showSelector() {
    document.querySelector("#fileSelector").click();
}

function uploadCover() {
    uploadingState(true, "Upload Cover");

    var formData = new FormData();
    formData.append("upload", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: "/Article/UploadCover",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            document.querySelector("#coverUrl").value = data.toString();
            alert("Upload successful");
            uploadingState(false, "Upload Cover");
        },
        error: function (response) {
            alert("Upload failed " + response.toString());
            uploadingState(false, "Upload Cover");
        }
    });
}

function upload() {
    document.querySelector("#articleContent").value = CKEDITOR.instances.htmlEditor.getData();
    document.querySelector("form").submit();
}

CKEDITOR.replace('htmlEditor');
CKEDITOR.instances.htmlEditor.setData(document.querySelector("#articleContent").value);