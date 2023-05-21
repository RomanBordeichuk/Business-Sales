const purchasesHiddenBlock = document.getElementById("purchases_hidden_block");
const salesHiddenBlock = document.getElementById("sales_hidden_block");

let hasShownPurchasesInputs = false;
function showPurchasesInputs(){
    if(hasShownPurchasesInputs){
        purchasesHiddenBlock.style = "display: none;";
        hasShownPurchasesInputs = false;
    }
    else{
        purchasesHiddenBlock.style = "display: grid;";
        hasShownPurchasesInputs = true;
    }
}

let hasShownSalesInputs = false;
function showSalesInputs(){
    if(hasShownSalesInputs){
        salesHiddenBlock.style = "display: none;";
        hasShownSalesInputs = false;
    }
    else{
        salesHiddenBlock.style = "display: grid;";
        hasShownSalesInputs = true;
    }
}
