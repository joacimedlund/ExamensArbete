// AI genererad kod av ChatGPT

(function () {
    // Öppna/stäng meny
    function toggleMenu(btn, menu, open) {
        const willOpen = (open !== undefined) ? open : !menu.classList.contains('open');
        document.querySelectorAll('.menu.open').forEach(m => { // stäng andra
            if (m !== menu) {
                m.classList.remove('open');
                const b = document.querySelector(`[data-menu-id="${m.id}"]`);
                if (b) b.setAttribute('aria-expanded', 'false');
            }
        });
        menu.classList.toggle('open', willOpen);
        btn.setAttribute('aria-expanded', String(willOpen));
        menu.setAttribute('aria-hidden', String(!willOpen));
        if (willOpen) {
            // fokus på första item
            const first = menu.querySelector('.menu-item');
            first && first.focus();
        }
    }

    // Klick på actions-knapp
    document.addEventListener('click', (e) => {
        const btn = e.target.closest('.project-actions');
        if (btn) {
            const id = btn.getAttribute('data-menu-id');
            const menu = document.getElementById(id);
            if (menu) toggleMenu(btn, menu);
            return;
        }

        // Klick utanför -> stäng alla
        if (!e.target.closest('.menu')) {
            document.querySelectorAll('.menu.open').forEach(m => {
                m.classList.remove('open');
                const b = document.querySelector(`[data-menu-id="${m.id}"]`);
                if (b) b.setAttribute('aria-expanded', 'false');
            });
        }
    });

    // Esc för att stänga
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            document.querySelectorAll('.menu.open').forEach(m => {
                m.classList.remove('open');
                const b = document.querySelector(`[data-menu-id="${m.id}"]`);
                if (b) b.setAttribute('aria-expanded', 'false');
                b && b.focus();
            });
        }
    });

    // Tangentnavigering i menyn (pil upp/ner + Enter)
    document.addEventListener('keydown', (e) => {
        const menu = e.target.closest('.menu');
        if (!menu) return;
        const items = [...menu.querySelectorAll('.menu-item')];
        const idx = items.indexOf(document.activeElement);
        if (e.key === 'ArrowDown') {
            e.preventDefault();
            (items[idx + 1] || items[0]).focus();
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            (items[idx - 1] || items[items.length - 1]).focus();
        } else if (e.key === 'Enter' || e.key === ' ') {
            // här kan du göra vad menyalternativet ska göra
            document.activeElement.click();
        }
    });
})();
