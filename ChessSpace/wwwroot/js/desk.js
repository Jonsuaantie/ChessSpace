document.addEventListener('DOMContentLoaded', () => {
    const screen = document.querySelector('.screen');
    const button = document.querySelector('.onbutton');
    const container = document.querySelector('.container');
    const bgWrapper = document.querySelector('.bg-wrapper');
    const img = container ? container.querySelector('.bg-img') : null;

    if (screen) {
        screen.addEventListener('click', () => {
            if (container.classList.contains('on')) {
                window.location.href = screen.dataset.url;
            }
        });
    }

    if (button && container && bgWrapper && img) {
        button.addEventListener('click', (e) => {
            e.preventDefault();

            // voorkom dubbel klikken tijdens animatie
            if (container.dataset.animating === 'true') return;

            const isOn = container.classList.contains('on');
            const targetSrc = isOn ? "../images/gamingsetupoff.jpg" : "../images/gamingsetupon.jpg";

            // maak overlay
            const overlay = document.createElement('div');
            overlay.className = 'crossfade-overlay';
            overlay.style.backgroundImage = `url("${targetSrc}")`;
            overlay.style.opacity = '0';

            bgWrapper.appendChild(overlay);

            // force reflow zodat transition werkt
            void overlay.offsetWidth;

            // start animatie: fade overlay in
            container.dataset.animating = 'true';
            overlay.style.opacity = '1';

            // als transition klaar is: swap src en cleanup
            const finish = () => {
                // guard (in geval van dubbele calls)
                if (container.dataset.animating !== 'true') return;
                img.src = targetSrc; // vervang de onderliggende img
                overlay.remove();
                container.dataset.animating = 'false';
                container.classList.toggle('on');
            };

            const onTransitionEnd = (ev) => {
                if (ev.propertyName === 'opacity') {
                    overlay.removeEventListener('transitionend', onTransitionEnd);
                    finish();
                }
            };
            overlay.addEventListener('transitionend', onTransitionEnd);

            // fallback (in geval transitionend niet vuurt)
            setTimeout(() => {
                if (container.dataset.animating === 'true') finish();
            }, 1000);
        });
    }
});
