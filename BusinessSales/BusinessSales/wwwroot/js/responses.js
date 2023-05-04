// ******************* buttons & inputs *********************

const nameInputSignUp = document.getElementById("nameInputSignUp");
const passInputSignUp = document.getElementById("passInputSignUp");

const nameInputSignIn = document.getElementById("nameInputSignIn");
const passInputSignIn = document.getElementById("passInputSignIn");

const nameInputChange = document.getElementById("nameInputChange");
const passInputChange = document.getElementById("passInputChange");

// ******************* DATA ELEMENTS *********************

const mainPageH1 = document.getElementById("mainPageH1");
const mainPageTotalIncomeSpan = 
    document.getElementById("mainPageTotalIncomeSpan");
const mainPageTotalCountSpan = 
    document.getElementById("mainPageTotalCountSpan");

const purchaseDateSpan = document.getElementById("purchaseDateSpan");
const purchaseNameSpan = document.getElementById("purchaseNameSpan");
const purchasePriceSpan = document.getElementById("purchasePriceSpan");
const purchaseCountSpan = document.getElementById("purchaseCountSpan");
const purchaseCommentSpan = document.getElementById("purchaseCommentSpan");

const saleDateSpan = document.getElementById("saleDateSpan");
const saleNameSpan = document.getElementById("saleNameSpan");
const salePriceSpan = document.getElementById("salePriceSpan");
const saleCountSpan = document.getElementById("saleCountSpan");
const saleCommentSpan = document.getElementById("saleCommentSpan");

// ******************* REQUESTS *********************

async function signUp(){
    const response = await fetch("/signUp", {
        method: "signUp",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            name: nameInputSignUp.value,
            password: passInputSignUp.value
        })
    });

    const result = await response.json();

    if(result == "success") location.href = "main.html";
    else console.log(result);
}

async function signIn(){
    const response = await fetch("/signIn", {
        method: "signIn",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            name: nameInputSignIn.value,
            password: passInputSignIn.value
        })
    });

    const result = await response.json();

    if(result == "success") location.href = "main.html";
    else console.log(result);
}

async function loadMainPage(){
    const response = await fetch("/loadMainPage", {
        method: "loadMainPage",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();

    mainPageH1.innerText = result["name"];
    mainPageTotalIncomeSpan.innerText = result["totalIncome"];
    mainPageTotalCountSpan.innerText = result["totalCount"];
}

async function pushPurchase(){
    const response = await fetch("/pushPurchase", {
        method: "pushPurchase",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            date: purchaseDateSpan.value,
            nameOfProducts: purchaseNameSpan.value,
            priceOfProduct: purchasePriceSpan.value,
            countOfProducts: purchaseCountSpan.value,
            comment: purchaseCommentSpan.value
        })
    });

    const result = await response.json();

    if(result == "success") console.log("Purchase successfully saved");
    else console.log(result);   
}

async function pushSale(){
    const response = await fetch("/pushSale", {
        method: "pushSale",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            date: saleDateSpan.value,
            nameOfProducts: saleNameSpan.value,
            priceOfProduct: salePriceSpan.value,
            countOfProducts: saleCountSpan.value,
            comment: saleCommentSpan.value
        })
    });

    const result = await response.json();

    if(result == "success") console.log("Sale successfully saved");
    else console.log(result);
}

async function changeAccountName(){
    const response = await fetch("/changeName", {
        method: "changeName",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            name: nameInputChange.value
        })
    });

    const result = await response.json();

    if(result == "success") location.href = "index.html";
    else console.log(result);
}

async function changeAccountPassword(){
    const response = await fetch("/changePassword", {
        method: "changePassword",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            password: passInputChange.value
        })
    });

    const result = await response.json();

    if(result == "success") location.href = "index.html";
    else console.log(result);
}

async function deleteAccount(){
    const response = await fetch("/deleteAccount", {
        method: "deleteAccount",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();

    if(result == "success") location.href = "index.html";
    else console.log(result);
}
