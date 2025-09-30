document.addEventListener('DOMContentLoaded', () => {
    const screen = document.querySelector('.screen');
    const button = document.querySelector('.onbutton');
    const container = document.querySelector('.container');

    if (screen) {
        // Screen zoom animatie en redirect
        screen.addEventListener('click', () => {
            screen.classList.add('zoom');
            setTimeout(() => {
                window.location.href = "projects.html"; // Nieuwe pagina
            }, 800);
        });
    }

    if (button && container) {
        // Toggle aan/uit
        button.addEventListener('click', (e) => {
            console.log("Button clicked!");
            container.classList.toggle('on');
        });
    }
});
