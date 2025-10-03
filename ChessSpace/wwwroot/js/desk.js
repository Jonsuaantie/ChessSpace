document.addEventListener('DOMContentLoaded', () => {
    const screen = document.querySelector('.screen');
    const button = document.querySelector('.onbutton');
    const container = document.querySelector('.container');
    const bgWrapper = document.querySelector('.bg-wrapper');
    const img = container ? container.querySelector('.bg-img') : null;

    if (screen) {
        screen.addEventListener('click', () => {
            if (container.classList.contains('on')) {
                const bgImg = document.querySelector('.bg-img');

                bgImg.classList.add('animate');

                setTimeout(() => {
                    window.location.href = screen.dataset.url;
                }, 1000);
            }
        });
    }


    if (button && container && bgWrapper && img) {
        button.addEventListener('click', (e) => {
            e.preventDefault();

            if (container.dataset.animating === 'true') return;

            const isOn = container.classList.contains('on');
            const targetSrc = isOn ? "../images/gamingsetupoff.jpg" : "../images/gamingsetupon.jpg";

            const overlay = document.createElement('div');
            overlay.className = 'crossfade-overlay';
            overlay.style.backgroundImage = `url("${targetSrc}")`;
            overlay.style.opacity = '0';

            bgWrapper.appendChild(overlay);

            void overlay.offsetWidth;

            container.dataset.animating = 'true';
            container.classList.add('animating'); // <<< animatie staat aan
            overlay.style.opacity = '1';

            const finish = () => {
                if (container.dataset.animating !== 'true') return;
                img.src = targetSrc;
                overlay.remove();
                container.dataset.animating = 'false';
                container.classList.remove('animating'); // <<< animatie klaar
                container.classList.toggle('on');
            };

            const onTransitionEnd = (ev) => {
                if (ev.propertyName === 'opacity') {
                    overlay.removeEventListener('transitionend', onTransitionEnd);
                    finish();
                }
            };
            overlay.addEventListener('transitionend', onTransitionEnd);

            setTimeout(() => {
                if (container.dataset.animating === 'true') finish();
            }, 1000);
        });
    }
});
