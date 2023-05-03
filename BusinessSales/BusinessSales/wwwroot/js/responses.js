const nameInputSignUp = document.getElementById("nameInputSignUp");
const passInputSignUp = document.getElementById("passInputSignUp");

const nameInputSignIn = document.getElementById("nameInputSignIn");
const passInputSignIn = document.getElementById("passInputSignIn");

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
    console.log(result);

    if(result == "success") location.href = "main.html";
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
    console.log(result);

    if(result == "success") location.href = "main.html";
}