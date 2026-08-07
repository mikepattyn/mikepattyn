/**
 * Contact form → FormSubmit AJAX endpoint → contact@mikepattyn.nl.
 * The plain <form action> stays as a no-JS fallback; with JS we submit
 * inline and show localized success/error feedback. Spam protection is
 * FormSubmit's honeypot field (`_honey`) in the markup.
 */
import { t } from "./locale.js";

const ENDPOINT = "https://formsubmit.co/ajax/contact@mikepattyn.nl";

// Keys are stored as data-i18n so a locale switch re-translates live text.
function setText(el, key) {
  if (key) {
    el.setAttribute("data-i18n", key);
    el.innerHTML = t(key);
  } else {
    el.removeAttribute("data-i18n");
    el.innerHTML = "";
  }
}

export function initContactForm() {
  const form = document.getElementById("contact-form");
  if (!form) return;

  const button = form.querySelector('button[type="submit"]');
  const status = form.querySelector(".contact-form__status");

  form.addEventListener("submit", async (event) => {
    event.preventDefault();
    if (button.disabled) return;

    button.disabled = true;
    setText(button, "contact.form.sending");
    setText(status, null);
    status.classList.remove("is-success", "is-error");

    try {
      const response = await fetch(ENDPOINT, {
        method: "POST",
        headers: { Accept: "application/json" },
        body: new FormData(form),
      });
      if (!response.ok) throw new Error(`FormSubmit responded ${response.status}`);
      form.reset();
      setText(status, "contact.form.success");
      status.classList.add("is-success");
    } catch {
      setText(status, "contact.form.error");
      status.classList.add("is-error");
    } finally {
      button.disabled = false;
      setText(button, "contact.form.send");
    }
  });
}
