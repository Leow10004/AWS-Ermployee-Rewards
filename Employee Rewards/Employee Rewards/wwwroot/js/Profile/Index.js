document.addEventListener("DOMContentLoaded", (event) => {
    var cropper;
    var imageToCrop = document.getElementById('imageToCrop');
    console.log("Loaded");

    document.getElementById('fileInput').addEventListener('change', function (e) {
        var files = e.target.files;
        var done = function (url) {
            imageToCrop.src = url;
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

    document.getElementById('cancelButton').addEventListener('click', function () {
        $('#cropperModal').modal('hide');
    });

    document.getElementById('cropButton').addEventListener('click', function () {
        const canvas = cropper.getCroppedCanvas({
            width: 150,
            height: 150,
        });
        canvas.toBlob(function (blob) {
            const formData = new FormData();
            formData.append('croppedImage', blob, 'profile-picture.png');
            formData.append('employeeID', document.getElementById('EmployeeID').value);

            fetch('/Profile/UpdateProfilePicture', {
                method: 'POST',
                body: formData,
            }).then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            }).then(data => {
                if (data.success) {
                    console.log(data.imageUrl);
                    $('#cropperModal').modal('hide');
                    document.getElementById('Image').src = data.imageUrl;
                    // Force image reload
                    document.getElementById('Image').src += '?' + new Date().getTime();
                } else {
                    alert("Error updating profile picture: " + data.message);
                }
            }).catch(error => {
                console.error("Unexpected error: ", error);
                alert("Unexpected error occurred. Please try again.");
            });
        });
    });

    document.getElementById('profileForm').addEventListener('submit', function (e) {
        e.preventDefault(); // Prevent the default form submission
        fetch('/Profile/UpdateProfile', {
            method: 'POST',
            body: new FormData(this),
        }).then(response => {
            if (response.redirected) {
                window.location.href = response.url;
            }
        }).catch(error => {
            console.error("Unexpected error: ", error);
            alert("Unexpected error occurred. Please try again.");
        });
    });
});
