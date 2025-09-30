function updateClock() {
    const now = new Date();
    const timeEl = document.getElementById('time');
    const dateEl = document.getElementById('date');


    const hours = now.getHours().toString().padStart(2, '0');
    const mins = now.getMinutes().toString().padStart(2, '0');
    timeEl.textContent = hours + ':' + mins;


    const opts = { weekday: 'short', day: 'numeric', month: 'short' };
    dateEl.textContent = now.toLocaleDateString('nl-NL', opts);
}
updateClock();
setInterval(updateClock, 1000 * 30);


const desktopIcons = document.querySelectorAll('.desktop-icon');
desktopIcons.forEach(el => {
    el.addEventListener('dblclick', (e) => {
        e.preventDefault();
        el.animate([
            { transform: 'scale(1)' },
            { transform: 'scale(0.96)' },
            { transform: 'scale(1)' }
        ], { duration: 260 });
    });
});