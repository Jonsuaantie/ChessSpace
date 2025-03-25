// Open modal
document.getElementById("playMatchButton").addEventListener("click", function () {
    document.getElementById("gameModal1").style.display = "flex";
    document.getElementById("gameModal2").style.display = "flex";
});

// Close modal
function closeModal() {
    document.getElementById("gameModal1").style.display = "none";
    document.getElementById("gameModal2").style.display = "none";
}

// Stuur de gebruiker naar de CreateGame controller methode
function createGame() {
    window.location.href = "/Game/CreateGame";  // Aanpassen aan je route
}

// Stuur de gebruiker naar de JoinGame controller methode
function joinGame() {
    window.location.href = "/Game/JoinGame";  // Aanpassen aan je route
}
