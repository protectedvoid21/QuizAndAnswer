const trueButtons = document.querySelectorAll("input[class='true']")
const falseButtons = document.querySelectorAll("input[class='false']")

const trueBackgroundColor = trueButtons[0].style.backgroundColor
const trueTextColor = trueButtons[0].style.color
const falseBackgroundColor = falseButtons[0].style.backgroundColor
const falseTextColor = falseButtons[0].style.color

let checkedOptions = [];

for (let i = 0; i < trueButtons.length; i++) {
    checkedOptions.push(false)
}

for (let i = 0; i < trueButtons.length; i++) {
    trueButtons[i].addEventListener("click", () => {
        trueButtons[i].style.backgroundColor = trueBackgroundColor
        trueButtons[i].style.color = trueTextColor
        falseButtons[i].style.backgroundColor = "#999999"
        falseButtons[i].style.color = "#333333"
        checkedOptions[i] = true
    })
}

for (let i = 0; i < falseButtons.length; i++) {
    falseButtons[i].addEventListener("click", () => {
        falseButtons[i].style.backgroundColor = falseBackgroundColor
        falseButtons[i].style.color = falseTextColor
        trueButtons[i].style.backgroundColor = "#999999"
        trueButtons[i].style.color = "#333333"
        checkedOptions[i] = false
    })
}

const submitButton = document.querySelector("button[class='submit-button']")

submitButton.addEventListener("click", () => {
    console.log("check")
    for (let i = 0; i < checkedOptions.length; i++) {
        console.log(checkedOptions[i])
    }
})

