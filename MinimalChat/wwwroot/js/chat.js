function chatRoom(_roomName, _username) {

    let sendButton = document.getElementById("send-button");
    let sendAll = document.getElementById("send-all");
    let conversazione = document.getElementById("conversazione");
    let messageToSend = document.getElementById("message-to-send")
    let exitButton = document.getElementById("exitRoomButton")
    let connection = new signalR.HubConnectionBuilder().withUrl(`/Enter`).build();


    function SvuotaInput() {
        document.getElementById("message-to-send").value = "";
    }

    // connection.start
    connection.start().then(() => {
        console.log("connessione iniziata")
        let roomName = _roomName;
        let username = _username;
        connection.invoke("JoinRoom", roomName, username).catch((err) => {
            console.error(err.toString());
        });
    });

    //RICEVI MESSAGGI
    connection.on("ReceiveMessage", (message) => {
        let div = document.createElement("div")

        div.innerHTML = `${message}`
        if (div.innerText.match("has joined the room") || div.innerText.match("has left")) {
            div.classList = "message fw-6 border border-light p-2 rounded-3"
        }
        else {
            div.classList = "message fw-6 border border-light p-2 rounded-3"
            div.setAttribute("style", "background-color: white; color: black")
        }

        console.log(div.textContent)
        let conversazione = document.getElementById("conversazione")
        conversazione.appendChild(div)
    })

    //INVIA A STANZA
    sendButton.addEventListener('click', (e) => {
        e.preventDefault();
        console.log("hai cliccato")
        let roomName = _roomName;
        console.log(roomName)

        let message = document.getElementById("message-to-send").value;
        console.log(message)
        let username = _username.toString();

        //append message to conversazione
        // let newMessage = document.createElement("div")
        // newMessage.textContent = message;

        connection.invoke("SendMessageToRoom", roomName, message, username).catch((err) => {
            console.log("entra nella funzione connection.invoke")
            return console.error(err.toString());
        })
        messageToSend.value = "";

    })

    // //INVIA A TUTTI
    // sendAll.addEventListener('click', (e) => {
    //     e.preventDefault();
    //     console.log("hai cliccato")
    //     let user = "pippo"


    //     let message = document.getElementById("message-to-send").value;
    //     console.log(message)

    //     connection.invoke("SendMessage", user, message).catch((err) => {
    //         console.log("entra nella funzione connection.invoke")
    //         return console.error(err.toString());
    //     })

    // })

    //Connessione chiusa
    connection.onclose(() => {
        let roomName = _roomName;
        let username = _username;
        connection.invoke("LeaveRoom", roomName, username).catch((err) => {
            console.error(err.toString());
        });
    });

    //Pulsante ESCI

    exitButton.addEventListener("click", function (event) {
        event.preventDefault();
        let roomName = _roomName;
        let username = _username;
        connection.invoke("LeaveRoom", roomName, username).catch((err) => {
            console.error(err.toString());
        });
        connection.stop(); // interrompo la connessione
        //metto il messagio "tizio è uscito dalla chat"
        window.location.href = "https://localhost:7028"

    });

}