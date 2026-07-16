# Deel III — CEO Pieter Sleeboom

## Wie zit er tegenover je?

**Pieter Sleeboom** is sinds 1 september 2024 CEO van Energiemissie|Trenton. Hij is geen “energie-nerd die toevallig CEO werd”. Hij is een **B2B SaaS-groei-executive** die het stokje overnam na een fase van groei en de fusie met Trenton.

Publiek bekende achtergrond:

- 15+ jaar ervaring als executive bij innovatieve B2B SaaS en scale-ups
- Ex-COO bij **Shypple** (digitale freight forwarder) — supply chain inzichtelijk en efficiënter maken voor grote bedrijven + organisatie laten groeien
- Bestuursrol bij AdfluenceHub (B2B platform)
- LinkedIn-positionering: energy transition SaaS, serial founder / exits — gebruik dat als context, niet als gossip

Zijn publieke boodschap bij aantreden draait om:

- duurzaamheid en energiebeheer worden belangrijker voor overheid en bedrijven
- administratie versimpelen naast verbruik optimaliseren
- oplossingen moeten meebewegen met snel veranderende klantbehoeften
- groeien **én** impact op efficiënt energiegebruik

> **CEO Insight**  
> Een CEO met SaaS-achtergrond luistert naar: klariteit, ownership, klantwaarde, tempo met kwaliteit, en of jij risico’s snapt (security, data, cost). Techniek moet vertaalbaar zijn naar business outcome.

---

## Wat meet een CEO in dit gesprek?

Pieter hoeft niet elke LangGraph-node te begrijpen. Hij moet wel voelen:

| Signaal | Sterk | Zwak |
|---------|-------|------|
| Ownership | “Dit heb ik end-to-end gedragen” | “Ik werkte in een team aan tickets” |
| AI-leverage | “Ik stuur agents + evals; zo schaal ik output” | “Ik gebruik Copilot voor autocomplete” |
| Product sense | “Eerste feature zou X zijn omdat klant Y…” | “Ik bouw wat Product vraagt” |
| Schaal | Praat over data, multi-tenant, failure modes | Alleen happy-path demo’s |
| Cultuurfit | Autonomie + AI-first + bijstellen | Process-zwaar of “zeg maar wat ik moet doen” |
| Communicatie | Helder, kort, met bewijs | Jargon-muur of vage claims |

### Wat wil hij horen?

1. **Je snapt het bedrijf** — multi-site energiebeheer, niet “groene tech is cool”.
2. **Je snapt de rol** — agentic features in SaaS, geen chatbot-theater.
3. **Je hebt het al gedaan** — agents/tool use/.NET/fullstack in productie.
4. **Je kunt de 90 dagen invullen** — zonder hun interne roadmap te claimen.
5. **Je maakt anderen sneller** — MCP/CLI, standaarden, “collega’s komen voor hoe-vragen”.

### Welke antwoorden laten alarmbellen afgaan?

> **Don't say this**
>
> - “Ik ben vooral een frontend-dev die AI leuk vindt.”
> - “Ik wil remote-first en maximale vrijheid.” (hybride Diemen staat expliciet)
> - “Energie ken ik nog niet, maar AI is universeel.” (zonder leerplan)
> - “Ik schrijf zelf geen code meer — AI doet alles.” (klinkt als abdication)
> - “Chatbots zijn de toekomst van jullie portal.”
> - Salaris als openingszet zonder fit te tonen

---

## Hoe Pieter waarschijnlijk vragen stelt

CEO-vragen klinken vaak persoonlijk of strategisch, maar meten ownership en oordeel.

### “Waarom moeten wij jou aannemen?”

> **What they're really asking**  
> Wat is jouw unieke combinatie voor *deze* fase van *dit* bedrijf?

**Structuur:**

1. Bewijs dat je 0→1 AI+product hebt gedragen (Yaya)
2. .NET + fullstack diepte voor hun stack
3. Productie-AI discipline (evals/tracing)
4. Cultuur: jij stuurt, AI bouwt — en je wilt de AI-laag hier neerzetten

### “Wat zou je in de eerste 90 dagen doen?”

Hij test of je realistisch bent zonder passief te worden. Gebruik Deel II. Eindig met een meetbaar succes: “één AI-feature in productie met klantwaarde + basis voor herhaalbaar AI-engineeren.”

### “Hoe werk je met AI-tools?”

> **Perfect answer**  
> “Ik behandel agents als junior engineers met supersnel typen: ik geef context, constraints en acceptance tests. Ik review diffs, dwing tracing/evals af, en hou kosten en security in de gaten. Op Yaya betekende dat LangGraph-workflows en voice agents die echt live gingen — niet alleen een prompt in een notebook.”

### “Vertel over een moment waarop iets misging”

CEO’s willen leren zien + verantwoordelijkheid. Kies een Yaya-voorbeeld: AI-gedrag, real-time, of migratie — Situation → wat jij deed → wat je structureel verbeterde (tests, evals, observability).

---

## Mike’s Playbook: gesimuleerde CEO-loop

Gebruik dit patroon bij elke zware vraag:

```text
Pieter vraagt X
  → Wat meet hij écht?
  → Welk bewijs van Mike past?
  → Antwoord in 60–90 seconden
  → Eén concrete detail (stack/impact)
  → Stop. Laat hem doorvragen.
```

### Voorbeeld: “Waarom Energiemissie?”

**Wat meet hij:** motivatie die langer meegaat dan “leuke vacature”.

**Mike-antwoord (kern):**

> Ik wil AI bouwen waar data en klanten al bestaan — niet alleen greenfield speeltuinen. Jullie zitten op honderdduizenden aansluitingen voor overheid en zorg. De AI-native rebuild is precies het moment waarop een engineer met mijn profiel — .NET, agents in productie, ownership — het verschil kan maken. En jullie cultuur van autonomie en AI-first matcht hoe ik al werk.

**Waarschijnlijke doorvraag:** “Oké, maar wat bouw je dan als eerste?”  
→ Terug naar 90-dagen feature-hypotheses + eval/fallback discipline.

---

## Sparring met een CTO die bouwt

De vacature noemt expliciet een CTO die zelf codeert. Implicatie voor het CEO-gesprek:

- Te vaag technisch = Pieter (of later de CTO) prikt erdoorheen
- Te diep zonder businessbrug = “slimme specialist, geen multiplier”
- Ideaal: **één laag dieper dan de vraag**, dan vertalen naar klant/risico/tempo

Als Pieter zegt “we willen agentic features”, antwoord niet alleen “LangGraph”. Zeg:

> Agent = LLM + tools + state + policies. Op jullie schaal zou ik eerst de tool-laag op het data platform hard maken — wat mag een agent lezen per tenant — en dan één vertical feature shippen met evals. Framework is secundair; contracten en observatie eerst.

---

## Mentale checklist vlak voor je binnenloopt

- [ ] Ik kan Energiemissie in 30 seconden uitleggen zonder website na te praten
- [ ] Ik kan de rol in één zin: “agentic energy intelligence in de SaaS, geen chatbot”
- [ ] Ik heb 3 bewijzen paraat: Yaya AI, .NET ownership, schaal/productie
- [ ] Ik heb een 90-dagenverhaal
- [ ] Ik ken de drie cultuurpijlers
- [ ] Ik weet wie Pieter is (SaaS CEO, niet alleen “de baas”)
- [ ] Ik praat over “jij stuurt, AI bouwt” zonder arrogant of lui te klinken

---

## Meer CEO-simulaties

### “Hoe meet je succes van AI?”

> **What they're really asking**  
> Ben jij metrics-gedreven of demo-gedreven?

**Mike-antwoord:**  
“Op drie lagen. Product: gebruiken klanten de feature en levert het tijd/geld/CO₂-inzicht op? Kwaliteit: eval-score, escalation/fallback-rate, complaint-rate. Economie: kosten per succesvolle actie. Als alleen ‘het model antwoordt’ groen is, hebben we de verkeerde metric.”

### “Wat als Product een chatbot wil?”

**Mike-antwoord:**  
“Dan vraag ik welk job-to-be-done de chat oplost. Vaak is het echte probleem triage, scenario’s of uitleg — dat kan een guided agent-flow zijn zonder ‘ChatGPT-sidebar’. Ik push terug met een snellere path-to-value en lagere risico’s. Als chat toch de schil wordt, dan als UI over tools met grounding — niet als vrij model.”

### “Hoe overtuig je sceptische domeinexperts?”

**Mike-antwoord:**  
“Niet met modelbenchmarks. Met shadow mode: AI stelt voor, expert beslist, we meten agreement. Daarna steeds meer autonomie waar evals het toelaten. Domeinexperts moeten de tool-laag vertrouwen — authz, bronnen, audit.”

### “Waarom jij en niet een pure data scientist?”

**Mike-antwoord:**  
“Omdat deze rol SaaS-engineering is: .NET, integratie, UI, multi-tenant security, shippen. Ik kan model-orkestratie, maar ik optimaliseer voor productoutcome in jullie stack — niet voor papers.”

---

## Signalen tijdens het gesprek (lees de kamer)

| Signaal van Pieter | Wat je doet |
|--------------------|-------------|
| Hij onderbreekt met “maar wat is de business value?” | Stop tech; geef euro/tijd/risico |
| Hij vraagt door op ownership | Geef “ik was accountable voor X” + resultaat |
| Hij noemt klanten/overheid | Koppel ISO, uitlegbaarheid, geen hallucinatie-risico |
| Hij glimlacht om AI-tools | Toon discipline (review, evals), geen speelsheid alleen |
| Hij is stil | Eindig je antwoord; vul stilte niet met waffle |

---

## Afsluiting die sterk voelt

> “Ik wil deze rol omdat ik hier meetbare AI kan shippen op echte energiedata — met ownership. Als jullie twijfelen over één ding, laat het mijn energiedomein-diepte zijn: die haal ik bewust in. Waar ik niet aan twijfel is of ik agents, .NET en productiekwaliteit kan dragen — dat doe ik al.”
