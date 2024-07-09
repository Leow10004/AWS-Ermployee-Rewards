document.addEventListener("DOMContentLoaded", (event) => {
    var cropper;
    var imageToCrop = document.getElementById('imageToCrop');
    console.log("Loaded");

    document.getElementById('fileInput').addEventListener('change', function (e) {
        var files = e.target.files;
        var done = function (url) {
            imageToCrop.src = url;
            console.log("YES");
            $('#cropperModal').modal('show');
        };
        var reader;
        var file;
        var url;

        if (files && files.length > 0) {
            file = files[0];

            if (URL) {
                done(URL.createObjectURL(file));
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result);
                };
                reader.readAsDataURL(file);
            }
        }
    });

    $('#cropperModal').on('shown.bs.modal', function () {
        cropper = new Cropper(imageToCrop, {
            aspectRatio: 1,
            viewMode: 2,
            autoCrop: true,
            autoCropArea: 1,
            movable: true,
            zoomable: true,
            rotatable: true,
            scalable: true
        });
    }).on('hidden.bs.modal', function () {
        cropper.destroy();
        cropper = null;
    });

    document.getElementById('cropButton').addEventListener('click', function () {
        var canvas;
        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                width: 160,
                height: 160,
            });
            canvas.toBlob(function (blob) {
                var url = URL.createObjectURL(blob);
                var reader = new FileReader();
                reader.readAsDataURL(blob);
                reader.onloadend = function () {
                    var base64data = reader.result;
                    document.getElementById('Image').src = base64data;
                    $('#cropperModal').modal('hide');
                };
            });
        }
    });
});