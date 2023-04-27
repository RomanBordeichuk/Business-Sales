const nameInput = document.getElementById("nameInput");

async function signIn(){
    const response = await fetch("/signIn", {
        method: "signIn",
        headers: { "Accept":"application/json", "Content-type":"application/json" },
        body: JSON.stringify({
            name: nameInput.value
        })
    });
}