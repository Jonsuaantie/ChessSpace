const screen = document.querySelector('.screen');

// Screen zoom animatie en redirect
screen.addEventListener('click', () => {
    screen.classList.add('zoom');
    setTimeout(() => {
        window.location.href = "projects.html"; // Nieuwe pagina
    }, 800);
});
