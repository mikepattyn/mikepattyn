# Energiemissie Interview Playbook (v0.1)

Persoonlijk Nederlands handboek (40–60 pagina’s) voor Mike Pattyn’s interview bij Energiemissie|Trenton.

## Build PDF

```bash
cd playbook
make pdf
```

Requires: `pandoc`, `typst` (PDF engine).

Output: `playbook.pdf`

## Structure

- `CONTEXT.md` — sourced facts
- `STYLE.md` — call-out conventions
- `chapters/` — manuscript
- `book.md` — generated assemble
- `playbook.pdf` — deliverable
