document.addEventListener('DOMContentLoaded', () => {
    initAccountDropdown();
});

function initAccountDropdown() {
    const btn = document.querySelector('[data-dropdown-target="#accountMenu"]');
    const menu = document.querySelector('#accountMenu');
    if (!btn || !menu) return;

    const open = () => {
        menu.classList.add('open');
        menu.setAttribute('aria-hidden', 'false');
        btn.setAttribute('aria-expanded', 'true');
        document.addEventListener('click', onDocClick, { capture: true });
        document.addEventListener('keydown', onEsc);
    };

    const close = () => {
        menu.classList.remove('open');
        menu.setAttribute('aria-hidden', 'true');
        btn.setAttribute('aria-expanded', 'false');
        document.removeEventListener('click', onDocClick, { capture: true });
        document.removeEventListener('keydown', onEsc);
    };

    const toggle = () => menu.classList.contains('open') ? close() : open();

    const onDocClick = (e) => {
        if (!menu.contains(e.target) && !btn.contains(e.target)) close();
    };

    const onEsc = (e) => {
        if (e.key === 'Escape') close();
    };

    btn.addEventListener('click', (e) => {
        e.preventDefault();
        toggle();
    });
}
