class ChessboardImage extends HTMLElement {
    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });

        const style = document.createElement('style');
        style.textContent = `
            .chessboard {
                width: 480px;
                height: 480px;
                background-image: url('../images/ChessBoardImg.png'); 
                background-size: cover;
                border-radius: 17px;
                box-shadow: 0 0 10px 18px rgba(0, 0, 0, 0.65);
                margin-left: 40px;
            }
        `;

        const div = document.createElement('div');
        div.classList.add('chessboard');

        shadow.appendChild(style);
        shadow.appendChild(div);
    }
}

customElements.define('chessboard-image', ChessboardImage);