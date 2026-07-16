#!/usr/bin/env python3
"""Assemble chapter markdown into book.md for Pandoc."""

from pathlib import Path

ROOT = Path(__file__).resolve().parent
CHAPTERS = [
    "chapters/00-frontmatter.md",
    "chapters/01-mike.md",
    "chapters/02-energiemissie.md",
    "chapters/03-ceo-pieter.md",
    "chapters/04-vraagbibliotheek.md",
    "chapters/05-projecten-star.md",
    "chapters/06-ai-native-cheats.md",
    "chapters/07-salaris-last5.md",
]

PAGEBREAK = "\n\n```{=typst}\n#pagebreak()\n```\n\n"


def strip_leading_yaml(text: str) -> str:
    if text.startswith("---"):
        end = text.find("\n---", 3)
        if end != -1:
            return text[end + 4 :].lstrip("\n")
    return text


def main() -> None:
    parts = [
        "---",
        'title: "The AI Engineer Interview Playbook"',
        'subtitle: "v0.1 — Energiemissie Interview Edition"',
        'author: "Mike Pattyn"',
        "lang: nl",
        "---",
        "",
    ]
    bodies = []
    for rel in CHAPTERS:
        raw = (ROOT / rel).read_text(encoding="utf-8")
        bodies.append(strip_leading_yaml(raw).rstrip() + "\n")
    parts.append(PAGEBREAK.join(bodies))
    out = ROOT / "book.md"
    out.write_text("\n".join(parts) + "\n", encoding="utf-8")
    print(f"Wrote {out}")


if __name__ == "__main__":
    main()
