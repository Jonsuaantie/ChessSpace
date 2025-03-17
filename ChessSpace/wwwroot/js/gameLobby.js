const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ChessHub")
    .build();

connection.start().then(() => {
    console.log("Connected to SignalR!");
    joinGame(gamecode);
}).catch(err => console.error(err));

window.onload = function () {
    let gamecodeElement = document.getElementById("gamecode");
    if (gamecodeElement) {
        gamecode = gamecodeElement.innerText.trim();
        joinGame(gamecode);
    } else {
        console.error("Gamecode-element niet gevonden!");
    }
};

connection.on("ReceiveMessage", (message) => {
    console.log("Bericht ontvangen:", message);
    document.getElementById("messages").innerHTML += `<p>${message}</p>`;
});
connection.on("UpdateBoard", (updatedBoardHtml) => {
    document.getElementById("chessboard").innerHTML = updatedBoardHtml;
});
let gamecode = document.getElementById('gamecode').innerText;

function sendMessage() {
    let msg = document.getElementById("msgInput").value;
    connection.invoke("SendMessage", msg, gamecode);
}

let selectedPawn = null;
let selectedpawncell = null;

function movePawn(event) {
    const clickedButton = event.target;
    const clickedCell = clickedButton.parentNode;

    if (selectedPawn === null) {
        if (clickedButton.classList.contains('orangepawn') || clickedButton.classList.contains('bluepawn')) {
            selectedPawn = clickedButton;
            selectedpawncell = clickedCell;

            clickedButton.style.border = '2px solid red';
            console.log('Pion geselecteerd:', clickedButton.id);
        }
    } else {
        if (clickedCell && !(clickedButton.classList.contains('orangepawn') || clickedButton.classList.contains('bluepawn'))) {

            clickedCell.innerHTML = '';
            let temppawn = selectedPawn;
            temppawn.style.border = '';
            clickedCell.appendChild(temppawn);
            selectedpawncell.innerHTML = `<button class="chessbutton" onclick="movePawn(event)"></button>`


            
            console.log('Pion verplaatst naar:', clickedCell.id);

            let updatedBoardHtml = document.getElementById('chessboard').innerHTML;

            connection.invoke("MovePiece", updatedBoardHtml, gamecode);

            selectedPawn = null;
            selectedpawncell = null;
        }
    }
}
function joinGame(gamecode) {
    connection.invoke("JoinGame", gamecode);
}


