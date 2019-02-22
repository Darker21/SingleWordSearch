// JavaScript source code

let bMouseDown;
const letters = document.querySelectorAll('#ContentMain_tableWordSearch > tbody > tr > td');

window.onload = function (e) {
    //letters.forEach(cell => cell.addEventListener("mouseover", appendClass));
    //letters.forEach(cell => cell.addEventListener("mouseout", appendClass));
    for (var i = 0; i < letters.length; i++) {
        letters[i].addEventListener('mousedown', appendClass);
        letters[i].addEventListener('mouseenter', appendClass);
    }

    var c = checkCompleted();

    if (c) {
        for (var x = 0; x < letters.length; x++) {
            var sId = "";
            sId = letters[x].id.toUpperCase();
            if (sId !== "") {
                if (sId.includes("TRUE")) {
                    letters[x].click();
                };
            };
        };
    };

};

document.addEventListener("mousedown", function () {
    bMouseDown = true;
});

document.addEventListener("mouseup", function () {
    bMouseDown = false;
    let bCheckExist = true;
    var buildWord = "";
    var checkBox;
    var selectedCells = document.querySelectorAll(".WordSearchTableCell_Hover, .WordSearchTableCell:hover");


    for (var i = 0; i < selectedCells.length; i++) {
        selectedCells[i].classList.remove("WordSearchTableCell_Hover");
    };


    for (var i = 0; i < selectedCells.length; i++) {
        if (selectedCells[i].id === "") continue;
        buildWord += selectedCells[i].id.split("_")[1];
    }
    var element = document.getElementById("ContentMain_clue_" + buildWord);
    if (element === null) {
        buildWord = reverseString(buildWord);
        element = document.getElementById("ContentMain_clue_" + buildWord);
    }
    checkBox = element;

    if (checkBox === null) return;

    sendChecked(checkBox.id, true, checkBox.parentElement.getAttribute("data-id"));

    checkBox.checked = true;

    for (var i = 0; i < selectedCells.length; i++) {
        selectedCells[i].classList.remove("WordSearchTableCell_Hover");
        selectedCells[i].classList.add("WordSearchTableCell_Found");
    };

    // selectedCells.forEach(cell => cell.classList.remove("WordSearchTableCell_Hover"));

    // if (bActualWord) {
    //     selectedCells.forEach(cell => function () {
    //         cell.classList.add("WordSearchTableCell_Found");
    //     });
    // }
});

function appendClass() {
    if (!bMouseDown && event.type !== "mousedown") return;
    var target = event.target || event.srcElement;
    target.classList.toggle("WordSearchTableCell_Hover");
};

function removeClasses(obj) {
    while (obj.classList.length > 0) {
        obj.classList.remove(obj.classList[0]);
    };
};

function checkCompleted() {
    var bCompleted = true;
    var anClues = document.querySelectorAll("input[type='checkbox']");
    for (var i = 0; i < anClues.length; i++) {
        if (anClues[i].checked === false) {
            bCompleted = false;
            break;
        }
    };

    if (bCompleted) {
        document.removeEventListener("mouseup");
        document.removeEventListener("mousedown");
    };
    return bCompleted;
};

function reverseString(str) {
    var splitStr = str.split("");

    var reverseArray = splitStr.reverse();

    var joinArray = reverseArray.join("");

    return joinArray;
};

function sendChecked(cId, checked, sWordId) {
    var bool = checked;
    var sId = cId;
    console.log(bool);
    console.log(cId);
};

function ReceiveServerData(arg, context) {
    //alert(arg);
};

function GetUserSignedIn() {
    var signedInText = document.getElementById("labelSignInWelcome");

    if (signedInText.innerText.split(' ')[1] === 'Visitor') return false;
    else return true;
};