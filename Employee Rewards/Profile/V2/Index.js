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
            $('.modal').style.display = 'flex';
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
        const canvas = cropper.getCroppedCanvas({
            width: 150,
            height: 150,
        });
        canvas.toBlob(function (blob) {
            const formData = new FormData(document.getElementById('profileForm'));
            formData.append('croppedImage', blob, 'profile-picture.png');

            fetch('/Profile/UpdateProfile', {
                method: 'POST',
                body: formData,
            }).then(response => {
                return response.json();
            }).then(data => {
                if (data.success) {
                    document.getElementById('Image').src = data.imageUrl;
                    $('#cropperModal').modal('hide');
                    alert("Profile updated successfully!");
                } else {
                    alert("Error updating profile: " + data.message);
                }
            }).catch(error => {
                console.error("Unexpected error: ", error);
                alert("Unexpected error occurred. Please try again.");
            });
        });
    });
});