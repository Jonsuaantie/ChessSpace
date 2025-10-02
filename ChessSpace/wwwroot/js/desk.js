document.addEventListener('DOMContentLoaded', () => {
    const screen = document.querySelector('.screen');
    const button = document.querySelector('.onbutton');
    const container = document.querySelector('.container');

    if (screen) {
        screen.addEventListener('click', () => {
            if (container.classList.contains('on')) {
                window.location.href = screen.dataset.url;
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
