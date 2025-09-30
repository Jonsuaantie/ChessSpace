document.addEventListener('DOMContentLoaded', () => {
    const screen = document.querySelector('.screen');
    const button = document.querySelector('.onbutton');
    const container = document.querySelector('.container');

    if (screen) {
        screen.addEventListener('click', () => {
            if (container.classList.contains('on')) {
                screen.classList.add('zoom');
                const targetUrl = screen.dataset.url;
                setTimeout(() => {
                    window.location.href = targetUrl;
                }, 800);
            }
        });
    }

    if (button && container) {
        button.addEventListener('click', (e) => {
            e.preventDefault();
            container.classList.toggle('on');
        });
    }
});
