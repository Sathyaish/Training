document.addEventListener("DOMContentLoaded", setup);

function setup() {
    document.getElementById("generateSuccess").addEventListener("click", generateSuccessMessageClickEventHandler);
    document.getElementById("generateError").addEventListener("click", generateErrorMessageClickEventHandler);
}

function generateSuccessMessageClickEventHandler(event) {
    let element = document.getElementById("message");
    element.innerText = "The operation was successful.";
    element.addClass("success").removeClass("error");
}

function generateErrorMessageClickEventHandler(event) {
    let element = document.getElementById("message");
    element.innerText = "There was an error.";
    element.addClass("error").removeClass("success");
}

HTMLElement.prototype.hasClass = function hasClass(className) {
    // return this.classList.contains(className);

    // or

    return (` ${this.className} `.replace(/[\n\t\r]/g, " ").indexOf(className) > -1);
};

HTMLElement.prototype.addClass = function addClass(className) {
    if (!this.hasClass(className)) {
        this.className += ` ${className} `;
    }

    return this;
};

HTMLElement.prototype.removeClass = function removeClass(className) {
    if (this.classList.contains(className)) {
        this.classList.remove(className);
    }

    return this;
};