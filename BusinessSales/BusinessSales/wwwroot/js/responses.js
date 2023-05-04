// ******************* buttons & inputs *********************

const nameInputSignUp = document.getElementById("nameInputSignUp");
const passInputSignUp = document.getElementById("passInputSignUp");

const nameInputSignIn = document.getElementById("nameInputSignIn");
const passInputSignIn = document.getElementById("passInputSignIn");

const nameInputChange = document.getElementById("nameInputChange");
const passInputChange = document.getElementById("passInputChange");

// ******************* requests *********************

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
