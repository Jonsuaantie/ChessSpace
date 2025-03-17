const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ChessHub")
    .build();

connection.start().then(() => {
    console.log("Connected to SignalR!");
}).catch(err => console.error(err));

connection.on("ReceiveMessage", (message) => {
    console.log("Bericht ontvangen:", message);
    document.getElementById("messages").innerHTML += `<p>${message}</p>`;
});
let selectedpiece = null;
function sendMessage() {
    let msg = document.getElementById("msgInput").value;
    connection.invoke("SendMessage", msg);
}

document.addEventListener("DOMContentLoaded", function () {
    let selectedButton = null;

    document.querySelectorAll(".chessbutton").forEach(button => {
        button.addEventListener("click", function () {
            let currentImage = this.style.backgroundImage;

            if (selectedButton === null) {
                // Eerste klik: Selecteer pion als er een afbeelding op staat
                if (currentImage && currentImage !== "none") {
                    selectedButton = this;
                    this.style.border = "2px solid red"; // Markeer geselecteerde pion
                }
            } else {
                // Tweede klik: Verplaats de pion
                if (!this.style.backgroundImage || this.style.backgroundImage === "none") {
                    this.style.backgroundImage = selectedButton.style.backgroundImage;
                    selectedButton.style.backgroundImage = ""; // Verwijder afbeelding van de oude plek
                    selectedButton.style.border = "none"; // Reset de border
                    selectedButton = null; // Reset selectie
                } else {
                    // Klikt op een andere pion, dan wordt de selectie gewijzigd
                    selectedButton.style.border = "none"; // Haal de rode rand weg
                    selectedButton = this;
                    this.style.border = "2px solid red"; // Nieuwe selectie
                }
            }
        });
    });
});
