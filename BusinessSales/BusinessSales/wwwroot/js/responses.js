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

const purchasesHistoryList = document.getElementById("purchasesHistoryList");
const salesHistoryList = document.getElementById("salesHistoryList");
const storeList = document.getElementById("storeList");

const indexPageResponseMessageSpan = document.getElementById("indexPageResponseMessageSpan");
const signUpPageResponseMessageSpan = document.getElementById("signUpPageResponseMessageSpan");
const mainPageResponseMessageSpan = document.getElementById("mainPageResponseMessageSpan");
const settingsPageResponseMessageSpan = document.getElementById("settingsPageResponseMessageSpan");

// ******************* REQUESTS *********************

// ******************* SIGNING IN / UP *********************

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
    else {
        signUpPageResponseMessageSpan.innerText = result;
        console.log(result);
    }
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
    else {
        indexPageResponseMessageSpan.innerText = result;
        console.log(result);
    }
}

// ******************* MAIN PAGE *********************

async function loadMainPage(){
    const response = await fetch("/loadMainPage", {
        method: "loadMainPage",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();

    mainPageH1.innerText = result.name;
    mainPageTotalIncomeSpan.innerText = result.totalIncome;
    mainPageTotalCountSpan.innerText = result.totalCount;
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

    if(result == "success") {
        console.log("Purchase successfully saved");

        purchaseDateSpan.value = "";
        purchaseNameSpan.value = "";
        purchasePriceSpan.value = "";
        purchaseCountSpan.value = "";
        purchaseCommentSpan.value = "";

        mainPageResponseMessageSpan.innerText = "Purchase successfully saved";

        loadMainPage();
    }
    else {
        mainPageResponseMessageSpan.innerText = result;
        console.log(result);
    }   
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

    if(result == "success") {
        console.log("Sale successfully saved");
        
        saleDateSpan.value = "";
        saleNameSpan.value = "";
        salePriceSpan.value = "";
        saleCountSpan.value = "";
        saleCommentSpan.value = "";

        mainPageResponseMessageSpan.innerText = "Sale successfully saved";

        loadMainPage();
    }
    else {
        mainPageResponseMessageSpan.innerText = result;
        console.log(result);
    }
}

// ******************* SALES / PURCHASES HISTORY & STORE *********************

async function loadPurchasesHistory(){
    const response = await fetch("/purchasesHistory", {
        method: "purchasesHistory",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();
    
    result.forEach(fieldJson => {
        let fieldDiv = document.createElement("div");

        fieldDiv.innerText = 
        `Date: ${fieldJson.date}, ` +
        `Name of products: ${fieldJson.nameOfProducts}, ` +
        `Price of each product: ${fieldJson.priceOfProduct}, ` +
        `Count of products: ${fieldJson.countOfProducts}, ` +
        `Comment to purchase: ${fieldJson.comment}, ` +
        `Price of purchase: ${fieldJson.priceOfPurchase}\n\n`;

        purchasesHistoryList.append(fieldDiv);
    });
}

async function loadSalesHistory(){
    const response = await fetch("/salesHistory", {
        method: "salesHistory",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();
    
    result.forEach(fieldJson => {
        let fieldDiv = document.createElement("div");

        fieldDiv.innerText = 
        `Date: ${fieldJson.date}, ` +
        `Name of products: ${fieldJson.nameOfProducts}, ` +
        `Price of each product: ${fieldJson.priceOfProduct}, ` +
        `Count of products: ${fieldJson.countOfProducts}, ` +
        `Comment to purchase: ${fieldJson.comment}, ` +
        `Price of purchase: ${fieldJson.priceOfSale}\n\n`;

        salesHistoryList.append(fieldDiv);
    });
}

async function loadStore(){
    const response = await fetch("/store", {
        method: "store",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();
    
    result.forEach(fieldJson => {
        let fieldDiv = document.createElement("div");

        fieldDiv.innerText = 
        `Name of products: ${fieldJson.nameOfProducts}, ` +
        `Count of products: ${fieldJson.countOfProducts}, ` +
        `Price of products batch: ${fieldJson.purchasePrice}\n\n`;

        storeList.append(fieldDiv);
    });
}

// ******************* SETTINGS PAGE *********************

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
    else {
        settingsPageResponseMessageSpan.innerText = result;
        console.log(result);
    }
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
    else {
        settingsPageResponseMessageSpan.innerText = result;
        console.log(result);
    }
}

async function deleteAccount(){
    const response = await fetch("/deleteAccount", {
        method: "deleteAccount",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify()
    });

    const result = await response.json();

    if(result == "success") location.href = "index.html";
    else {
        settingsPageResponseMessageSpan.innerText = result;
        console.log(result);
    }
}
