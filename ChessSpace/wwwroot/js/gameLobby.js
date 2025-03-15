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

function sendMessage() {
    let msg = document.getElementById("msgInput").value;
    connection.invoke("SendMessage", msg);
}
