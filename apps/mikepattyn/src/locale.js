/**
 * EN/NL locale for the portfolio — policy matches Kapsalon LocaleService
 * (and the Lumen retrofit): stored `locale` wins, else navigator.language,
 * NL only when it starts with "nl", persisted on switch.
 */
import { en } from "./i18n/en.js";
import { nl } from "./i18n/nl.js";

const I18N = { en, nl };
const STORAGE_KEY = "locale";

function readStored() {
  try {
    const stored = localStorage.getItem(STORAGE_KEY);
    return stored === "en" || stored === "nl" ? stored : null;
  } catch {
    return null;
  }
}

function persist(locale) {
  try {
    localStorage.setItem(STORAGE_KEY, locale);
  } catch {
    /* ignore */
  }
}

function resolveInitial() {
  return readStored() ?? (navigator.language.toLowerCase().startsWith("nl") ? "nl" : "en");
}

function getByPath(obj, path) {
  return path.split(".").reduce((acc, key) => (acc == null ? undefined : acc[key]), obj);
}

let active = resolveInitial();

export function t(key) {
  const value = getByPath(I18N[active], key);
  return value == null ? key : value;
}

export function getLocale() {
  return active;
}

function apply() {
  document.documentElement.lang = active;
  document.title = t("meta.title");

  const meta = document.querySelector('meta[name="description"]');
  if (meta) meta.setAttribute("content", t("meta.description"));

  // Dictionary strings are our own content; some contain markup (<br>, links).
  document.querySelectorAll("[data-i18n]").forEach((el) => {
    el.innerHTML = t(el.getAttribute("data-i18n"));
  });

  document.querySelectorAll("[data-i18n-aria]").forEach((el) => {
    el.setAttribute("aria-label", t(el.getAttribute("data-i18n-aria")));
  });

  document.querySelectorAll("[data-locale]").forEach((btn) => {
    const isActive = btn.getAttribute("data-locale") === active;
    btn.setAttribute("aria-pressed", isActive ? "true" : "false");
    btn.classList.toggle("is-active", isActive);
  });
}

export function setLocale(locale) {
  if (locale !== "en" && locale !== "nl") return;
  if (locale === active) return;
  active = locale;
  persist(locale);
  apply();
}

export function initLocale() {
  apply();
  document.querySelectorAll("[data-locale]").forEach((btn) => {
    btn.addEventListener("click", () => setLocale(btn.getAttribute("data-locale")));
  });
}
