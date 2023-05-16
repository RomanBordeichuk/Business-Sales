// ******************* buttons & inputs *********************

const nameInputSignUp = document.getElementById("nameInputSignUp");
const passInputSignUp = document.getElementById("passInputSignUp");

const nameInputSignIn = document.getElementById("nameInputSignIn");
const passInputSignIn = document.getElementById("passInputSignIn");

const nameInputChange = document.getElementById("nameInputChange");
const passInputChange = document.getElementById("passInputChange");

const purchaseDateInput = document.getElementById("purchaseDateInput");
const purchaseNameInput = document.getElementById("purchaseNameInput");
const purchasePriceInput = document.getElementById("purchasePriceInput");
const purchaseCountInput = document.getElementById("purchaseCountInput");
const purchaseCommentInput = document.getElementById("purchaseCommentInput");

const saleDateInput = document.getElementById("saleDateInput");
const saleNameInput = document.getElementById("saleNameInput");
const salePriceInput = document.getElementById("salePriceInput");
const saleCountInput = document.getElementById("saleCountInput");
const saleCommentInput = document.getElementById("saleCommentInput");

// ******************* DATA ELEMENTS *********************

const mainPageH1 = document.getElementById("mainPageH1");
const mainPageTotalIncomeSpan = 
    document.getElementById("mainPageTotalIncomeSpan");
const mainPageTotalCountSpan = 
    document.getElementById("mainPageTotalCountSpan");
const mainPageAvPercentSpan = 
    document.getElementById("mainPageAvPercentSpan");
const mainPageCountStoreSpan =
    document.getElementById("mainPageCountStoreSpan");
const mainPageCostStoreSpan =
    document.getElementById("mainPageCostStoreSpan");

let loadImages = document.querySelectorAll(".loadImg");

const purchasesHistoryList = document.getElementById("purchasesHistoryList");
const salesHistoryList = document.getElementById("salesHistoryList");
const storeList = document.getElementById("storeList");

const indexPageResponseMessageSpan = document.getElementById("indexPageResponseMessageSpan");
const signUpPageResponseMessageSpan = document.getElementById("signUpPageResponseMessageSpan");
const mainPagePurchaseResponseSpan = document.getElementById("mainPagePurchaseResponseSpan");
const mainPageSaleResponseSpan = document.getElementById("mainPageSaleResponseSpan");
const settingsPageResponseMessageSpan = document.getElementById("settingsPageResponseMessageSpan");

// ******************* PROLOADER *********************

let angle = 0;

function loading(){
    loadImages.forEach(img => {
        img.style = "transform: rotate(" + 
        angle + "deg)";
    });

    angle += 2;
}

let loadDuration;

function startLoading(page){
    loadImages.forEach(img => {
        img.style = "display: block";
    });

    switch(page){
        case "index":
            indexPageResponseMessageSpan.style = "background: transparent;";
            indexPageResponseMessageSpan.innerHTML = "";
            break;
        case "signUp":
            signUpPageResponseMessageSpan.style = "background: transparent;";
            signUpPageResponseMessageSpan.innerHTML = "";
            break;
    }

    loadDuration = setInterval(loading, 30);
}
function cancelLoading(){
    clearInterval(loadDuration);

    loadImages.forEach(img => {
        img.style = "display: none";
    });
}

// ******************* REQUESTS *********************

// ******************* SIGNING IN / UP *********************

async function signUp(){
    startLoading("signUp");

    const response = await fetch("/signUp", {
        method: "signUp",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            name: nameInputSignUp.value,
            password: passInputSignUp.value
        })
    });

    const result = await response.json();

    if(result == "success") {
        location.href = "main.html";
        signUpPageResponseMessageSpan.innerHTML = "Account succesfully created";
        signUpPageResponseMessageSpan.style = "background: #97B981;";
    }
    else {
        signUpPageResponseMessageSpan.innerHTML = result;
        signUpPageResponseMessageSpan.style = "background: #cf6655;";
        console.log(result);
    }

    cancelLoading();
}

async function signIn(){
    startLoading("index");

    const response = await fetch("/signIn", {
        method: "signIn",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            name: nameInputSignIn.value,
            password: passInputSignIn.value
        })
    });

    const result = await response.json();

    if(result == "success") {
        location.href = "main.html";
        indexPageResponseMessageSpan.innerHTML = "Account succesfully found";
        indexPageResponseMessageSpan.style = "background: #97B981;";
    }
    else {
        indexPageResponseMessageSpan.innerHTML = result;
        indexPageResponseMessageSpan.style = "background: #cf6655;";
        console.log(result);
    }

    cancelLoading();
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
    mainPageTotalIncomeSpan.innerText = result.totalIncome + "$";
    mainPageTotalCountSpan.innerText = result.totalCount;
    mainPageAvPercentSpan.innerText = result.avPercent + "%";
    mainPageCountStoreSpan.innerText = result.countStore;
    mainPageCostStoreSpan.innerText = result.costStore + "$";

    purchaseDateInput.value = result.currentDate;
    saleDateInput.value = result.currentDate;
}

async function pushPurchase(){
    const response = await fetch("/pushPurchase", {
        method: "pushPurchase",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            date: purchaseDateInput.value,
            nameOfProducts: purchaseNameInput.value,
            priceOfProduct: purchasePriceInput.value,
            countOfProducts: purchaseCountInput.value,
            comment: purchaseCommentInput.value
        })
    });

    const result = await response.json();

    if(result == "success") {
        console.log("Purchase successfully saved");

        purchaseDateInput.value = "";
        purchaseNameInput.value = "";
        purchasePriceInput.value = "";
        purchaseCountInput.value = "";
        purchaseCommentInput.value = "";

        mainPagePurchaseResponseSpan.innerText = "Purchase successfully saved";
        mainPagePurchaseResponseSpan.style = "background: #97B981;";

        loadMainPage();
    }
    else {
        mainPagePurchaseResponseSpan.innerText = result;
        mainPagePurchaseResponseSpan.style = "background: #cf6655;";
        console.log(result);
    }   
}

async function pushSale(){
    const response = await fetch("/pushSale", {
        method: "pushSale",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            date: saleDateInput.value,
            nameOfProducts: saleNameInput.value,
            priceOfProduct: salePriceInput.value,
            countOfProducts: saleCountInput.value,
            comment: saleCommentInput.value
        })
    });

    const result = await response.json();

    if(result == "success") {
        console.log("Sale successfully saved");
        
        saleDateInput.value = "";
        saleNameInput.value = "";
        salePriceInput.value = "";
        saleCountInput.value = "";
        saleCommentInput.value = "";

        mainPageSaleResponseSpan.innerText = "Sale successfully saved";
        mainPageSaleResponseSpan.style = "background: #97B981;";

        loadMainPage();
    }
    else {
        mainPageSaleResponseSpan.innerText = result;
        mainPageSaleResponseSpan.style = "background: #cf6655;";
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
        `Price of purchase: ${fieldJson.priceOfSale}` +
        `Cost of purchase: ${fieldJson.costOfSale}\n\n`;

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
