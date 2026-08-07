/* Lumen — anonymous usage events for dashboard.mikepattyn.nl (fire-and-forget). */

const ANALYTICS_KEYS = {
  vid: "lumen.vid",
  completedSent: "lumen.analytics.completed"
};

const SESSION_KEYS = {
  visitSent: "lumen.analytics.visit"
};

function getVisitorId() {
  try {
    let vid = localStorage.getItem(ANALYTICS_KEYS.vid);
    if (!vid) {
      vid = crypto.randomUUID();
      localStorage.setItem(ANALYTICS_KEYS.vid, vid);
    }
    return vid;
  } catch {
    return "anonymous";
  }
}

function sendEvent(type, data = {}) {
  try {
    const payload = JSON.stringify({ type, vid: getVisitorId(), ...data });
    const url = "/api/events";
    if (navigator.sendBeacon) {
      navigator.sendBeacon(url, new Blob([payload], { type: "application/json" }));
    } else {
      fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: payload,
        keepalive: true
      });
    }
  } catch {
    /* analytics must never affect the game */
  }
}

const analytics = {
  trackVisit() {
    try {
      if (sessionStorage.getItem(SESSION_KEYS.visitSent)) return;
      sessionStorage.setItem(SESSION_KEYS.visitSent, "1");
      sendEvent("visit");
    } catch {
      /* ignore */
    }
  },

  trackLessonView(lessonId) {
    sendEvent("lesson_view", { lessonId });
  },

  trackLessonWalked(lessonId) {
    sendEvent("lesson_walked", { lessonId });
  },

  trackLessonUnwalked(lessonId) {
    sendEvent("lesson_unwalked", { lessonId });
  },

  trackCompleted() {
    try {
      if (localStorage.getItem(ANALYTICS_KEYS.completedSent)) return;
      localStorage.setItem(ANALYTICS_KEYS.completedSent, "1");
      sendEvent("completed");
    } catch {
      /* ignore */
    }
  },

  maybeTrackCompleted(getProgressFn, getLessonsFn) {
    const progress = getProgressFn();
    const lessons = getLessonsFn();
    const done = Object.values(progress).filter(Boolean).length;
    if (lessons.length > 0 && done >= lessons.length) {
      this.trackCompleted();
    }
  }
};

window.analytics = analytics;
