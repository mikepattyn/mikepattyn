# Deel IV — Vraagbibliotheek (~35)

Formaat per vraag:

1. **Waarom** stellen ze dit?
2. **Wat zoeken ze?**
3. **Mike-antwoord** (kern, spreektaal)
4. **Don't say**
5. **Doorvraag** om op voorbereid te zijn

---

## A. Persoonlijk & motivatie

### V1. Vertel eens iets over jezelf

**Waarom:** IJsbreker; ze horen wat jij zelf belangrijk vindt.  
**Wat zoeken ze?** Relevantie, focus, geen levensverhaal.  
**Mike-antwoord:**  
“Ik ben full stack engineer met ongeveer negen jaar ervaring, nu lead op een AI-communicatieplatform dat ik als primary engineer van nul naar productie heb gebracht. Wat mij typeert: ownership over de hele keten — Angular, .NET, Azure en Python-agents — en productdenken dicht op de gebruiker. Ik zoek een volgende stap waar AI geen side-project is maar de intelligence layer van een SaaS met echte schaal — precies wat jullie met de AI-native rebuild doen.”  
**Don't say:** Chronologisch CV vanaf 2010.  
**Doorvraag:** “Wat was jouw rol precies ten opzichte van het team?”

### V2. Waarom wil je bij Energiemissie werken?

**Waarom:** Motivatie-test.  
**Wat zoeken ze?** Specifiek bedrijf, niet “ik zoek een baan”.  
**Mike-antwoord:**  
“Drie dingen. Eén: jullie zitten al op energiedata voor overheid en zorg — AI hier raakt euro’s en CO₂, niet alleen demo’s. Twee: de agentic energy-intelligence layer op 250.000+ aansluitingen is precies het soort probleem waar mijn Yaya-ervaring met agents, RAG en productie-evals op aansluit. Drie: autonomie en AI-first zonder corporate rem — dat is hoe ik al werk.”  
**Don't say:** “Ik vind duurzaamheid belangrijk” zonder productbrug.  
**Doorvraag:** “Wat weet je over onze producten?”

### V3. Waarom moet ik jou aannemen?

**Waarom:** CEO-compressietest.  
**Wat zoeken ze?** Unieke combinatie + bewijs.  
**Mike-antwoord:**  
“Omdat jullie iemand nodig hebben die .NET-SaaS wél serieus neemt én agents in productie heeft gebracht. Ik heb laten zien dat ik 0→1 kan dragen, AI kan evalueren en monitoren, en features afmaak over de hele stack. In negentig dagen wil ik een eerste AI-feature live hebben en de standaard zetten hoe jullie AI-native bouwen.”  
**Don't say:** “Ik ben een hard worker en teamplayer.”  
**Doorvraag:** “Waar ben je minder sterk?”

### V4. Waar wil je over drie jaar staan?

**Waarom:** Ambitie vs. flight risk.  
**Wat zoeken ze?** Groei die hen helpt.  
**Mike-antwoord:**  
“Ik wil de engineer zijn waar het team naartoe komt voor AI-systemen in productie — architectuur, evals, tool-ecosysteem — en intussen diep genoeg in jullie energiedomein zitten om zelf kansen te zien. Niet per se een klassieke manager; wel technical leadership met productimpact.”  
**Don't say:** “Ik wil CTO worden / eigen bedrijf.” (tenzij gevraagd en genuanceerd)  
**Doorvraag:** “Hoe ga je het energiedomein leren?”

### V5. Wat motiveert je het meest?

**Mike-antwoord:**  
“Brug tussen product en engineering: iets bouwen dat gebruikers echt voelen, snel itereren op feedback, en techniek hoog houden. AI is voor mij een multiplier op dat pad — niet het doel op zich.”  
**Don't say:** Alleen “nieuwe tech”.

---

## B. Ownership & werkwijze

### V6. Hoe ziet ownership eruit in jouw werk?

**Mike-antwoord:**  
“Op Yaya betekende ownership: ik was accountable voor architectuurkeuzes, oplevering en kwaliteit over frontend, API en AI-services. Ik wachtte niet tot iemand de AI-stukjes ‘oppakte’ — ik maakte ze onderdeel van het productpad, inclusief tests en telemetry.”  
**Don't say:** “Ik pak tickets van de board.”

### V7. Hoe ga je om met onduidelijke requirements?

**Mike-antwoord:**  
“Ik maak de kleinste scherpe hypothese: welk gebruikersprobleem, welk succescriterium, welke data hebben we nodig. Dan korte spike of vertical slice, feedback van stakeholder, bijstellen. Op Yaya was de client-loop continuous build–measure–learn.”  
**Don't say:** “Dan wacht ik tot Product het uitschrijft.”

### V8. Vertel over een conflict of meningsverschil

**Structuur:** Situation → jouw belang → hoe je het oploste → resultaat.  
**Mike-antwoord (template):**  
Kies een technisch/product conflict (scope vs. kwaliteit, of framework-keuze). Toon dat je data/gebruikersrisico gebruikte, niet ego. Eindig met wat je anders deed in het proces (ADRs, spike, eval).  
**Don't say:** Collega’s zwartmaken.

### V9. Wanneer heb je iets te vroeg of te laat geshipped?

**Mike-antwoord:**  
Wees eerlijk: noem een moment waarop je scope sneed om te leren, of juist een kwaliteitshek (tests/evals) afdwong vóór release. Koppel aan AI: “bij agents is ‘werkt op mijn machine’ gevaarlijk — ik ship liever met fallback en eval-set.”

### V10. Hoe werk je in een klein team naast een CTO die bouwt?

**Mike-antwoord:**  
“Ik kom met voorstellen en trade-offs, niet alleen problemen. Ik spar op code/architectuur-niveau, maar ik sleep niet alles naar de CTO — ik maak zoveel mogelijk zelf af en escaller bij echte platformkeuzes.”  
**Don't say:** “Ik heb veel mentoring nodig.”

---

## C. .NET, architectuur, fullstack

### V11. Hoe diep is jouw .NET-ervaring?

**Mike-antwoord:**  
“Recent: ASP.NET Core 9 API greenfield met CQRS/MediatR, pipeline behaviors, EF Core 9 met tientallen migrations, SignalR hubs, JWT-flows, FluentValidation, integratie met Azure-services, xUnit met echte SQL-testdatabase. Dat is mijn primaire backend-spier. Daarvóór jaren .NET in productie op grotere user bases.”  
**Don't say:** “Ik doe vooral frontend, .NET ken ik een beetje.”

### V12. CQRS — waarom wel/niet?

**Mike-antwoord:**  
“Op Yaya paste CQRS omdat we duidelijke commands/queries, validatie en behaviors wilden zonder fat controllers. Het is geen religie: bij eenvoudige CRUD zou ik het niet forceren. Op een AI-laag zou ik commands voor tool-side-effects en queries voor retrieval/analytics strikt scheiden.”  
**Don't say:** “CQRS is always best practice.”

### V13. Hoe ontwerp je APIs voor AI-tool use?

**Mike-antwoord:**  
“Tools moeten smal, idempotent waar kan, goed geautoriseerd per tenant, en traceerbaar zijn. Liever tien scherpe tools dan één god-endpoint. Contract-first: schema’s, errors, timeouts. De LLM is onbetrouwbaar; de tool-laag moet saai en hard zijn.”  
**Doorvraag:** “Hoe doe je authz per aansluiting/organisatie?”

### V14. Frontend — hoe sterk ben je?

**Mike-antwoord:**  
“Sterk genoeg om features end-to-end te leveren. Op Yaya vier Angular-apps in een monorepo, shared libraries, NgRx + SignalR, Playwright/Storybook. Ik ben backend-zwaarder, maar ik lever geen ‘API over de schutting’.”  
**Don't say:** “Frontend boeit me niet.”

### V15. Hoe denk je over microservices vs. modular monolith?

**Mike-antwoord:**  
“Start coherent; split op team/scale/failure-boundaries. AI-services (Python) naast .NET kan logisch zijn vanwege ecosystem — maar data contracts en observability moeten eerst kloppen. Voor een rebuild: modulariteit > microservices-theater.”

---

## D. AI, LLM, agents, RAG

### V16. Wat is een agent — in jouw woorden?

**Mike-antwoord:**  
“Een systeem waarbij een model niet alleen tekst geeft, maar een doel nastreeft via tools, state en policies — met menselijke of automatische stops. Op Yaya: LangGraph-workflows met tools en streaming; op Echo: routing naar RAG of search.”  
**Don't say:** “Alles met een LLM is een agent.”

### V17. Wanneer géén agent gebruiken?

**Mike-antwoord:**  
“Als een deterministische pipeline of ruleset beter, goedkoper en uitlegbaarder is. Agents waar oordeel + tool-keuze nodig is; klassieke software waar het pad vastligt. Op energiedata zou ik compliance-gevoelige writes nooit ‘vrij’ aan een agent geven zonder harde guards.”

### V18. Hoe heb je tool use gebouwd?

**Mike-antwoord:**  
“Tools als expliciete functies met schema’s; het model kiest/vult arguments; runtime valideert en executeert; resultaten terug in de loop. State/checkpointing via LangGraph zodat lange flows hervatbaar zijn. Logging van tool calls is non-negotiable.”

### V19. RAG — hoe leg je het uit aan een niet-tech CEO?

**Mike-antwoord:**  
“Eerst zoeken we in jullie eigen kennis en data naar relevante stukken, dan laat ik het model daarop antwoorden met bronnen. Zo verklein je hallucinaties en kun je aantonen wáárom iets gezegd wordt — cruciaal bij energieadvies.”  
**Don't say:** Alleen “vector database”.

### V20. Hoe ga je om met hallucinaties?

**Mike-antwoord:**  
“Preventie: retrieval, grounded prompts, tool results als source of truth. Detectie: evals, human review op riskante flows. Mitigatie: weigeren/escaleren, fallback UI, duidelijke onzekerheid. Nooit stille verzinsels in factuur- of compliance-paden.”

### V21. Evals — wat heb je concreet gedaan?

**Mike-antwoord:**  
“Semantic evaluation en DeepEval-achtige flows op Yaya: checklists/embedding-matching, tracing, plus E2E met generated audio om AI-gedrag te valideren. Mijn standaard: geen productie zonder minimale testset en regressie op kritieke intents.”  
**Doorvraag:** “Hoe groot was je testset?” — Wees eerlijk over orde van grootte en dat je die hier wilt professionaliseren.

### V22. Tracing en monitoring van AI in productie

**Mike-antwoord:**  
“OpenTelemetry/App Insights op het platform; voor AI: log prompts/tool calls/latency/token cost/error rates (met privacy-redaction). Alerts op drift: opeens meer fallbacks of duurdere traces. Zonder tracing debugg je magie.”

### V23. Fallback-logica

**Mike-antwoord:**  
“Als retrieval zwak is of het model faalt: degradeer naar bewezen UI/rules, toon ‘kan dit niet betrouwbaar’, of queue voor menselijke review. Liever een saai correct pad dan een charmante foute AI.”

### V24. Kostencontrole

**Mike-antwoord:**  
“Budgets per feature/tenant, caching van retrieval, kleinere modellen voor routing, grotere alleen waar nodig, batch waar kan, max tokens, en productkeuzes: niet alles hoeft realtime LLM.”  
**CEO Insight:** Kosten = productfeature, geen nasleep.

### V25. MCP — ken je dat?

**Mike-antwoord:**  
“MCP is een standaard om tools/context aan agents en devtools te hangen. Ik heb agents met tool calling gebouwd; MCP is de portable laag daaroverheen. In de eerste weken zou ik team-CLI/MCP-servers willen die dezelfde veilige data-tools exposen als productie-agents — zodat ‘jij stuurt, AI bouwt’ ook voor het engineering team geldt.”  
**Don't say:** “Nooit van gehoord.” (zonder direct brug te slaan)

### V26. Claude Code / Copilot — hoe werk je dagelijks?

**Mike-antwoord:**  
“Ik geef agents repo-context, acceptance criteria en constraints; zij genereren; ik review architectuur, security en tests. Ik gebruik ze om sneller te verkennen en te implementeren, niet om verantwoordelijkheid te outsourcen. Past exact bij jullie ‘jij stuurt, AI bouwt’.”  
**Don't say:** “Ik plak wat Copilot geeft.”

### V27. Hoe zou een eerste energy-intelligence feature eruitzien?

**Mike-antwoord:**  
“Ik zou met Product één vertical kiezen met duidelijke euro/CO₂-waarde — bijvoorbeeld uitlegbare besparingssignalen of scenario’s op een subset aansluitingen. Technisch: read-only tools op het data platform, grounded answers, eval-set met echte cases, fallback naar bestaande monitor-UI. Geen chatbot als primaire UX.”  
**Don't say:** Concrete interne features claimen alsof je hun backlog kent.

---

## E. Data, schaal, security

### V28. 250.000 aansluitingen — waar let je op?

**Mike-antwoord:**  
“Tenant isolatie, query-kosten, indexing, async jobs voor zware analyses, rate limits op AI, en dat agents nooit ‘alles ophalen’ als default hebben. AI maakt slechte data access patterns duurder en gevaarlijker.”

### V29. Security / ISO — hoe raakt dat AI?

**Mike-antwoord:**  
“Jullie zijn ISO 27001 — AI mag dat niet ondermijnen. Geen secrets in prompts, strikte RBAC op tools, audit logs, dataminimalisatie, en leverancierskeuzes voor models met duidelijk data policy. Ik behandel tool calls als privileged API calls.”  
**Don't say:** “Security regelt iemand anders.”

### V30. Multi-tenant fout: agent lekt data tussen organisaties

**Mike-antwoord:**  
“Dat is seve­re. Design: authz in de tool-laag, nooit alleen in de prompt. Tests voor cross-tenant. Bij incident: kill switch, audit, klantcommunicatie met Product/CTO. Preventie > sorry.”

---

## F. Cultuur & AI-first

### V31. Wat betekent AI-first voor jou?

**Mike-antwoord:**  
“Voordat we iets bouwen of automatiseren, vraag ik: welk deel kan een model+tools beter, sneller of goedkoper — en welk deel moet deterministisch blijven? AI-first is een ontwerpreflex, geen hype-checkbox.”

### V32. “Jij stuurt, AI bouwt” — ben je het daarmee eens?

**Mike-antwoord:**  
“Ja, als sturen betekent: probleemkadering, architectuur, kwaliteitspoorten, productie-accountability. Nee, als het betekent: blind genereren zonder te snappen wat er live gaat. Ik wil juist sneller shippen *omdat* ik strenger stuur.”

### V33. Hoe leer je een nieuw domein (energie) snel?

**Mike-antwoord:**  
“Shadow support/product, glossary bouwen, één klantjourney end-to-end tekenen, en meteen een kleine feature gebruiken als leervoertuig. Domeinkennis plakt beter aan concrete shipping dan aan alleen documentatie.”

---

## G. Gedrag & lastige vragen

### V34. Wat is je grootste zwakte voor deze rol?

**Mike-antwoord:**  
“Ik heb geen jaren energiedomein-diepte — dat ga ik bewust inhalen in week 1–2. Technisch compenseer ik met ervaring in data-heavy SaaS en productie-AI. Waar ik scherp op blijf: niet te snel ‘slim’ AI’en vóór de data contracts kloppen.”  
**Don't say:** “Ik werk te hard” / “Ik ben perfectionist.”

### V35. Salarisverwachting?

Zie Deel VII. Kort: range + dat fit en scope eerst komen; hybride/Diemen en AI-budget meewegen.  
**Don't say:** Eerste zin van het gesprek.

### V36. Heb je nog vragen aan ons? (stel er 3)

1. “Hoe ziet succes van de AI-native rebuild eruit over zes maanden — productmetrics, niet alleen tech?”  
2. “Wat is vandaag de grootste frictie tussen data platform en feature teams?”  
3. “Hoe beslissen Product en engineering welke AI-features betaald/packaged worden?”  

Bonus aan Pieter: “Waar wil jij dat deze hire jou persoonlijk ontzorgt in het eerste halfjaar?”

---

## Top 12 — als je maar één uur hebt

1. V1 Vertel over jezelf  
2. V2 Waarom Energiemissie  
3. V3 Waarom jij  
4. V6 Ownership  
5. V11 .NET diepte  
6. V16 Wat is een agent  
7. V18 Tool use  
8. V21 Evals  
9. V25 MCP  
10. V26 Claude Code-workflow  
11. V27 Eerste feature  
12. V32 Jij stuurt, AI bouwt  

---

## H. Whiteboard & ontwerpvragen

### V37. Schets hoe je een agent op ons data platform zou zetten

**Waarom:** Architectuurhoofd.  
**Mike-antwoord (spreek terwijl je tekent):**  
“UI in bestaande SaaS → orchestration service → LLM → tool layer → tenant-scoped data APIs. Naast de flow: tracing, eval harness, kill switch. Writes alleen via allowlisted tools. Ik begin read-heavy.”  
**Don't say:** Alleen een cloud-vendor logo-tekening zonder authz.

### V38. Hoe test je non-deterministische systemen?

**Mike-antwoord:**  
“Splits deterministische tool-logic (gewone tests) van modelgedrag (evals met toleranties, snapshot van tool-keuzes, human review op riskante classes). Flaky tests aanpakken met vastgelegde fixtures en seed waar mogelijk.”

### V39. Wat is het verschil tussen LangGraph en ‘gewoon’ een for-loop met prompts?

**Mike-antwoord:**  
“Een graph/state machine maakt branches, retries, human-in-the-loop en checkpointing expliciet. Een for-loop kan werken tot je productiecomplexiteit krijgt — toen heb ik op Yaya juist state/tool orchestration nodig gehad.”

### V40. Hoe voorkom je dat AI-features een second system worden?

**Mike-antwoord:**  
“Zelfde auth,zelfde design system,zelfde release-train,zelfde observatie. AI is een capability inside the product — geen schaduw-IT. MCP/CLI helpt het team dezelfde capabilities te delen.”

---

## Antwoord-timing

| Type vraag | Doeltijd |
|------------|----------|
| Vertel over jezelf | 60–90s |
| Waarom wij / waarom jij | 45–60s |
| STAR | 75–120s |
| Tech deep-dive | 90s + “ik kan dieper” |
| Mening/strategie | 45s, dan stoppen |

Als je merkt dat je >2 minuten praat zonder adem: afronden met resultaat + brug naar Energiemissie.

---

## Recruiter-oefening (morgen)

Draai minimaal deze vijf hardop, met timer:

1. V1 + V2 aan elkaar (2 min totaal)  
2. V3 Waarom jij  
3. V11 .NET  
4. V21 Evals  
5. V32 Jij stuurt, AI bouwt  

Laat de recruiter expres doorvragen: “Waarom niet chatbot?” en “Ken je MCP?”  
