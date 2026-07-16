CHAPTERS := \
	chapters/00-frontmatter.md \
	chapters/01-mike.md \
	chapters/02-energiemissie.md \
	chapters/03-ceo-pieter.md \
	chapters/04-vraagbibliotheek.md \
	chapters/05-projecten-star.md \
	chapters/06-ai-native-cheats.md \
	chapters/07-salaris-last5.md

.PHONY: all book pdf pages clean

all: pdf

book: book.md

book.md: $(CHAPTERS)
	python3 build_book.py

pdf: book.md
	pandoc book.md -o playbook.pdf \
		--pdf-engine=typst \
		--toc \
		--toc-depth=2 \
		-V fontsize=11pt
	@echo "Wrote playbook.pdf"
	@$(MAKE) pages

pages: playbook.pdf
	@file playbook.pdf | sed -n 's/.*PDF document, version [^,]*, //p'

clean:
	rm -f book.md playbook.pdf
