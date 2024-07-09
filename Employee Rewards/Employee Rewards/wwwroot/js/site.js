document.addEventListener("DOMContentLoaded", function () {
    // Select the image inside the link with class .nav-button-active
    var imgElement = document.querySelector('.nav-button-active img');

    // Change the src attribute of the image
    if (imgElement) {
        imgElement.src = imgElement.src.split('.')[0]+"-Active.png";
    }
});

document.addEventListener("DOMContentLoaded", function () {
    var navButtonsImg = document.querySelectorAll('.nav-button img');

    navButtonsImg.forEach(function (img) {
        console.log(img.src);
        var originalSrc = img.getAttribute('data-original');
        var hoverSrc = img.getAttribute('data-hover');

        img.parentElement.addEventListener('mouseover', function () {
            if (!(img.parentElement.classList.contains(".nav-button-active"))) {
                img.src = hoverSrc;
            }
        });

        img.parentElement.addEventListener('mouseout', function () {
            if (!(img.parentElement.classList.contains("nav-button-active"))) {
                img.src = originalSrc;
            }
        });
    });
});