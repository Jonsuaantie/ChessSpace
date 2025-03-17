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

let selectedPawn = null;  // Houdt de geselecteerde pion bij

function movePawn(event) {
    const clickedButton = event.target;  // Het aangeklikte element (de button)
    const clickedCell = clickedButton.parentNode;  // De cel (td) van de aangeklikte knop

    if (selectedPawn === null) {
        // Als er geen pion is geselecteerd, selecteer dan de aangeklikte pion
        if (clickedButton.classList.contains('orangepawn') || clickedButton.classList.contains('bluepawn')) {
            selectedPawn = clickedButton;
            clickedButton.style.border = '2px solid red';  // Markeer geselecteerde pion
            console.log('Pion geselecteerd:', clickedButton.id);
        }
    } else {
        // Als er al een pion geselecteerd is, verplaats de pion naar de nieuwe plek
        if (clickedCell && (clickedCell.classList.contains('white') || clickedCell.classList.contains('black'))) {
            // Verwijder de pion uit de oorspronkelijke cel
            selectedPawn.parentNode.innerHTML = '';

            // Voeg de pion toe aan de nieuwe cel
            clickedCell.innerHTML = '';  // Zorg ervoor dat de cel leeg is
            clickedCell.appendChild(selectedPawn);

            selectedPawn.style.border = '';  // Verwijder de selectie markering
            console.log('Pion verplaatst naar:', clickedCell.id);
            selectedPawn = null;  // Reset de geselecteerde pion
        }
    }
}

