//Vissa funktioner har jag byggt upp med hjälp av ChatGpt


document.addEventListener('DOMContentLoaded', () => {

    initOpenModals()
    initCloseButtons()
    initForms();
    initValidateOnlyForms();

})

function clearFormErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}

function addFormErrorMessages(errors, form) {
    Object.keys(errors).forEach(key => {
        const input = form.querySelector(`[name="${key}"]`);
        if (input) {
            input.classList.add('input-validation-error')
        }

        const span = form.querySelector(`[data-valmsg-for="${key}"]`);
        if (span) {
            span.innerText = errors[key].join(' ')
            span.classList.add('field-validation-error')
        }
    })
}

function parseDecimalLocaleAware(str) {
    if (str == null) return NaN;
    const s = String(str).trim().replace(/\s+/g, '').replace(',', '.');
    if (s === '') return NaN;
    const n = Number(s);
    return Number.isFinite(n) ? n : NaN;
}
function parseDateInput(yyyyMmDd) {
    if (!yyyyMmDd) return null;
    const [y, m, d] = yyyyMmDd.split('-').map(Number);
    if (!y || !m || !d) return null;
    return new Date(y, m - 1, d);
}
function validateProjectForm(form) {
    const errors = {};
    const get = (name) => form.querySelector(`[name="${name}"]`);
    const val = (name) => (get(name)?.value ?? '').trim();

    const projectName = val('ProjectName');
    const clientName = val('ClientName');
    const description = val('Description');
    const budgetStr = val('Budget');
    const startStr = val('StartDate');
    const endStr = val('EndDate');
    const statusVal = val('Status');

    if (!projectName) errors.ProjectName = ['Required'];
    else if (projectName.length > 200) errors.ProjectName = ['Max 200 characters'];

    if (!clientName) errors.ClientName = ['Required'];
    else if (clientName.length > 200) errors.ClientName = ['Max 200 characters'];

    if (description && description.length > 4000)
        errors.Description = ['Max 4000 characters'];

    if (budgetStr) {
        const n = parseDecimalLocaleAware(budgetStr);
        if (Number.isNaN(n)) errors.Budget = ['Must be a number'];
        else if (n < 0 || n > 1_000_000_000) errors.Budget = ['Must be between 0 and 1 000 000 000'];
    }

    const start = parseDateInput(startStr);
    const end = parseDateInput(endStr);
    if (startStr && !start) errors.StartDate = ['Invalid date'];
    if (endStr && !end) errors.EndDate = ['Invalid date'];
    if (start && end && end < start)
        errors.EndDate = ['End Date cannot be earlier than Start Date'];


    return errors;
}

function validateAuthLoginForm(form) {
    const errors = {};
    const get = (name) => form.querySelector(`[name="${name}"]`);
    const val = (name) => (get(name)?.value ?? '').trim();

    const email = val('Email');
    const password = val('Password');

    if (!email) errors.Email = ['Required'];
    else {
        const emailValid = /^[\w.-]+@([\w-]+\.)+[\w-]{2,}$/;
        if (!emailValid.test(email)) errors.Email = ['Invalid email'];
    }

    if (!password) errors.Password = ['Required'];

    return errors;
}

function validateAuthRegisterForm(form) {
    const errors = {};
    const get = (name) => form.querySelector(`[name="${name}"]`);
    const val = (name) => (get(name)?.value ?? '').trim();

    const firstName = val('FirstName');
    const lastName = val('LastName');
    const email = val('Email');
    const password = val('Password');
    const confirm = val('ConfirmPassword');
    const terms = get('TermsandConditions')?.checked ?? false;

    if (!firstName) errors.FirstName = ['Required'];
    else if (firstName.length > 100) errors.FirstName = ['Max 100 characters'];

    if (!lastName) errors.LastName = ['Required'];
    else if (lastName.length > 100) errors.LastName = ['Max 100 characters'];

    if (!email) errors.Email = ['Required'];
    else {
        const emailValid = /^[\w.-]+@([\w-]+\.)+[\w-]{2,}$/;
        if (!emailValid.test(email)) errors.Email = ['Invalid email'];
    }

    if (!password) errors.Password = ['Required'];
    else if (password.length < 6) errors.Password = ['Min 6 characters'];

    if (!confirm) errors.ConfirmPassword = ['Required'];
    else if (confirm !== password) errors.ConfirmPassword = ['Not matching'];

    if (!terms) errors.TermsandConditions = ['You must accept the terms'];

    return errors;
}


function initForms() {
    const forms = document.querySelectorAll('form.ajax');
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            clearFormErrorMessages(form);

            const formData = new FormData(form);
            const failMsg = form.getAttribute('data-failmsg') || 'Unable to submit form';

            try {
                const res = await fetch(form.action, {
                    method: form.method || 'post',
                    body: formData,
                    headers: { 'X-Requested-With': 'XMLHttpRequest' },
                    credentials: 'same-origin' 
                });

                if (res.ok) {
                    const modal = form.closest('.modal');
                    if (modal) closeModal(modal);
                    window.location.reload();
                    return;
                }

                if (res.status === 400) {
                    const data = await res.json().catch(() => ({}));
                    if (data && data.errors) addFormErrorMessages(data.errors, form);
                    return;
                }

                if (res.status === 404) { alert('Not found'); return; }
                if (res.status === 409) { alert('Already exists'); return; }

                alert(failMsg);
            } catch {
                alert(failMsg);
            }
        });
    });
}

function initValidateOnlyForms() {
    const forms = document.querySelectorAll('form[data-validate]:not(.ajax)');
    forms.forEach(form => {
        form.addEventListener('submit', (e) => {
            clearFormErrorMessages(form);

            const kind = form.getAttribute('data-validate');
            let errors = {};

            if (kind === 'project') {
                errors = validateProjectForm(form);
            } else if (kind === 'auth') {
                errors = validateAuthRegisterForm(form);
            } else if (kind === 'login') {
                errors = validateAuthLoginForm(form);
            } 

            if (Object.keys(errors).length > 0) {
                e.preventDefault();
                addFormErrorMessages(errors, form);

                const firstKey = Object.keys(errors)[0];
                const firstInput = form.querySelector(`[name="${firstKey}"]`);
                if (firstInput) firstInput.focus();
            }
        });
    });
}



//function initForms() {
//    const forms = document.querySelectorAll('form')
//    forms.forEach(form => {
//        form.addEventListener('submit', async (e) => {
//            e.preventDefault()

//            clearFormErrorMessages(form)

//            const formData = new FormData(form)

//            try {
//                const res = await fetch(form.action, {
//                    method: 'post',
//                    body: formData
//                })

//                if (res.ok) {
//                    const modal = form.closest('.modal')
//                    if (modal)
//                        closeModal(modal)

//                    window.location.reload()
//                }
//                else if (res.status === 400) {
//                    const data = await res.json()
//                    if (data.errors) {
//                        addFormErrorMessages(data.errors, form)
//                    }
//                }
//                else if (res.status === 409) {
//                    alert('Project already exists')
//                }
//                else {
//                    alert('Unable to create new Project')
//                }
               
//            }

//            catch {

//            }
//        })
//    })
//}

function initOpenModals() {
    const modalButtons = document.querySelectorAll('[data-modal="true"')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const target = button.getAttribute('data-target')
            const modal = document.querySelector(target)

            if (modal) {
                modal.classList.add('flex')
            }
        })
    })
}

function initCloseButtons(){
    const closeButtons = document.querySelectorAll('[data-close="true"')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const target = button.getAttribute('data-target')
            const targetElement = document.querySelector(target)

            if (targetElement) {
                if (targetElement.classList.contains('modal')) {
                    closeModal(targetElement)
                }                             
            }
        } )
    })
    
}

function closeModal(modal) {
    if (modal) {
        modal.classList.remove('flex')

        modal.querySelectorAll('form').forEach(form => {
            form.reset()
        })
    }
}

