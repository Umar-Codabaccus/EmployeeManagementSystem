// emailValidation.js
const emailRegex = /^[a-zA-Z0-9]+@[a-zA-Z]+\.(gmail|yahoo|outlook)\.com$/;

function validateEmail(input) {
    const emailError = document.getElementById("emailError");
    if (!emailRegex.test(input.value)) {
        emailError.style.display = "block";
        emailError.textContent = "Email must follow the format: example@gmail.com, example@yahoo.com, or example@outlook.com";
    } else {
        emailError.style.display = "none";
    }
}

// Attach the event listener
document.addEventListener("DOMContentLoaded", function () {
    const emailInput = document.getElementById("email");
    if (emailInput) {
        emailInput.addEventListener("input", function () {
            validateEmail(this);
        });
    }
});
