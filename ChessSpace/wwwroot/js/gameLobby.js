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

let selectedPiece = null;
let selectedpiececell = null;

function movePawn(event) {
    const clickedButton = event.target;
    const clickedCell = clickedButton.parentNode;
    const pieceClasses = [
        'bluepawn', 'orangepawn', 'blueking', 'orangeking',
        'bluequeen', 'orangequeen', 'bluerook', 'orangerook',
        'blueknight', 'orangeknight', 'bluebishop', 'orangebishop'
    ];

    if (selectedPiece === null) {
        if (pieceClasses.some(cls => clickedButton.classList.contains(cls))) {
            selectedPiece = clickedButton;
            selectedpiececell = clickedCell;
            selectedPiece.style.outline = '2px solid red';
            console.log('Pion geselecteerd:', selectedPiece.id);
        }
    } else {
        if (clickedButton === selectedPiece) {
            selectedPiece.style.outline = '';
            selectedPiece = null;
            selectedpiececell = null;
            console.log('Pion gedeselecteerd');
            return;
        }

        const targetPiece = clickedCell.querySelector('button');

        if (targetPiece && pieceClasses.some(cls => targetPiece.classList.contains(cls))) {
            const selectedIsBlue = selectedPiece.classList.contains('bluepawn') ||
                selectedPiece.classList.contains('blueking') ||
                selectedPiece.classList.contains('bluequeen') ||
                selectedPiece.classList.contains('bluerook') ||
                selectedPiece.classList.contains('blueknight') ||
                selectedPiece.classList.contains('bluebishop');

            const targetIsBlue = targetPiece.classList.contains('bluepawn') ||
                targetPiece.classList.contains('blueking') ||
                targetPiece.classList.contains('bluequeen') ||
                targetPiece.classList.contains('bluerook') ||
                targetPiece.classList.contains('blueknight') ||
                targetPiece.classList.contains('bluebishop');

            if ((selectedIsBlue && targetIsBlue) || (!selectedIsBlue && !targetIsBlue)) {
                console.log('Illegale zet: je kunt niet op een stuk van je eigen kleur staan.');
                return; // Stop de functie als de zet illegaal is
            }
        }
        clickedCell.innerHTML = '';
        let temppiece = selectedPiece;
        temppiece.style.outline = '';
        clickedCell.appendChild(temppiece);
        selectedpiececell.innerHTML = `<button class="chessbutton" onclick="movePawn(event)"></button>`;

        console.log('Piece moved to:', clickedCell.id);

        let updatedBoardHtml = document.getElementById('chessboard').innerHTML;
        connection.invoke("MovePiece", updatedBoardHtml, gamecode);
        selectedPiece = null;
        selectedpiececell = null;
    }
}

function joinGame(gamecode) {
    connection.invoke("JoinGame", gamecode);
}


