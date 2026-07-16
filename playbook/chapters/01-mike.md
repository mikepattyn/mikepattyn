# Deel I — Mike Pattyn

## Wie ben jij als engineer?

Niet je CV. Je **professionele identiteit** — zodat je in het gesprek klinkt als iemand met een kompas, niet als iemand die vacatureteksten napraten.

### Kerndefinitie (leer dit)

> Ik ben een product-minded full stack engineer die ownership neemt over de hele keten — van UI tot API tot AI-agents in productie. Ik werk het best in kleine teams waar tempo, kwaliteit en gebruikersfeedback samenkomen. AI is voor mij geen speeltje: het is een multiplier die ik ontwerp, evalueer en operationaliseer.

Drie ankers:

1. **Ownership** — primary engineer 0→1 (Yaya)
2. **Fullstack met backend-zwaarte** — .NET/C# + Angular + cloud
3. **AI als productdiscipline** — agents, RAG, evals, tracing — live

### Hoe jij denkt

- Begin bij het gebruikersprobleem, niet bij het framework
- Snijd scope tot een vertical slice die leert
- Maak failure modes expliciet (zeker bij AI)
- Verwerf feedback vroeg (client loop op Yaya)
- Automatiseer kwaliteit (tests, evals, CI) zodat tempo houdbaar blijft

### Hoe jij leert

- Bouwen > alleen consumeren (Echo, Flyingdarts)
- Nieuwe stack leren door een echt pad naar productie
- Tools zoals Cursor/Claude Code gebruiken om *sneller* te leren én te leveren — niet om na te denken te vermijden

> **What they're really asking** wanneer ze “vertel over jezelf” zeggen:  
> Ben jij de persoon die de AI-laag kan dragen zonder te verdwalen in hype?

---

## Carrière-arc (als verhaal)

### Hoofdstuk A — Fundament (.NET + multi-stack)

VDAB .NET/C#, daarna jaren op een donatieplatform met **100.000+ users** en **>€1M/maand** flows. Angular, React, mobile, .NET, Azure/AWS, payment providers. Rollen naast development: Scrum Master, Product Owner.

**Les die je meeneemt:** software raakt geld en vertrouwen. Discipline rond integraties en stabiliteit is geen “enterprise overhead”.

### Hoofdstuk B — Maker buiten werktijd

**Flyingdarts:** real-time product + AWS serverless. Bewijst dat je producten wilt bestaan, niet alleen tickets sluiten.

### Hoofdstuk C — Lead + AI-native (nu)

Sinds 2023: lead developer op Yaya — AI-powered real-time platform. Van idee naar productie in ~12 maanden. Vier Angular-apps, ASP.NET Core CQRS, Python agents (LangGraph, LiveKit, RAG), Azure.

**Les:** AI in productie eist dezelfde engineering rigor als de rest van het platform — plus evals en cost control.

### Hoofdstuk D — Echo (nu)

Discord-agent met intent routing en RAG. Side-project dat oordeel + agent patterns scherpt.

### De boog in één zin

> Van betrouwbare fullstack op schaal → naar ownership van AI-producten end-to-end → klaar om een energy-intelligence layer in een serieuze SaaS mede neer te zetten.

---

## Superpowers (gemapt op de vacature)

### 1. Ownership zonder permission theater

**Waarom belangrijk bij Energiemissie:** autonomie is een cultuurpijler; 90-dagenplan eist dat jij trekt.  
**Bewijs:** primary engineer Yaya; architectuur + productrichting.  
**Interviewregel:** praat in “ik was accountable voor…”, niet “wij als team deden…”, behalve waar credit delen eerlijk is.

### 2. .NET/C# als ruggengraat

**Waarom:** expliciete must-have.  
**Bewijs:** ASP.NET Core 9, CQRS/MediatR, EF Core, SignalR, Azure integrations, testbaarheid.  
**Interviewregel:** één concrete design choice noemen (bijv. waarom CQRS daar paste).

### 3. Agents & tool use in productie

**Waarom:** “LLMs als product… agents en tool use… live ging”.  
**Bewijs:** LangGraph workflows, LiveKit agents, Echo routing.  
**Interviewregel:** definieer agent scherp; noem policies/fallbacks.

### 4. Evals, tracing, fallbacks

**Waarom:** expliciet in vacature + ISO-context.  
**Bewijs:** DeepEval/semantic evaluation, OpenTelemetry/App Insights, E2E audio tests.  
**Interviewregel:** “geen magie zonder metriek”.

### 5. AI-first execution (“jij stuurt, AI bouwt”)

**Waarom:** culturele litmus test van de vacature.  
**Bewijs:** dagelijkse agentic tooling; snelle levering mét reviewdiscipline.  
**Interviewregel:** sturen = criteria + architectuur + review; bouwen = accelerated implementation.

### 6. Product sense & UX-brug

**Waarom:** samenwerking met Product; features moeten klantwaarde raken.  
**Bewijs:** client feedback loop; design/engineering brug op je site.  
**Interviewregel:** koppel features aan euro’s/CO₂/tijdswinst voor multi-site klanten — niet aan modelnamen.

### 7. Schaal-instinct

**Waarom:** 250k aansluitingen; eerdere 100k users.  
**Bewijs:** high-traffic/payment verleden; monorepo discipline; cloud patterns (Azure/AWS).  
**Interviewregel:** praat over tenant isolation, query/AI cost, async work.

> **Don't say this**  
> “Mijn superpower is dat ik alles kan.”  
> Kies er drie en maak ze belachelijk concreet.

---

## Positionering t.o.v. de rolstitel

De pagina zegt Full Stack Engineer; de header AI-Native Full Stack Engineer. Positioneer je als:

> Full stack engineer met AI-native productervaring — sterk op .NET en agentic systems — die de energy intelligence layer kan mede-architecten en shippen.

Niet:

> “Pure ML engineer” of “alleen Angular dev die AI leuk vindt”.

---

## Elevator pitches

### 30 seconden

“Ik ben Mike — full stack, .NET-zwaar, met productie-AI. Ik bracht als primary engineer een AI-communicatieplatform van nul naar live in een jaar. Ik wil diezelfde ownership gebruiken om jullie agentic energy-intelligence features te bouwen op jullie data platform.”

### 90 seconden

Voeg toe: Angular + ASP.NET CQRS + LangGraph/RAG/evals; cultuurmatch AI-first; 90 dagen eerste feature + standaard neerzetten.

### 5 seconden (handdruk)

“AI-native fullstack met .NET en agents in productie — klaar om jullie intelligence layer te shippen.”

---

## Engineering filosofie (zodat je consistent klinkt)

### 1. Outcome > activity

Tickets afvinken is geen ownership. Ownership is: de gebruiker kan iets dat gisteren niet kon — veilig, observeerbaar, houdbaar.

### 2. Vertical slices

Liever één smalle flow die end-to-end werkt (UI → API → data → agent → eval) dan horizontale lagen die “bijna klaar” zijn.

### 3. Saai waar het moet, slim waar het mag

Auth, tenant isolation, factuurpaden, migrations: saai en streng.  
Planning, samenvatten, scenariodenken: hier mag AI schitteren — mét guards.

### 4. Feedback is een feature

Op Yaya was de client-loop geen bijzaak. Bij Energiemissie: early exposure aan energiecoördinatoren/support-cases maakt AI-features relevanter.

### 5. Tools vermenigvuldigen jou — ze vervangen je oordeel niet

“Jij stuurt, AI bouwt” betekent: jij blijft de architect van kwaliteit. Als je dat niet wilt dragen, past deze cultuur niet — en jij past niet bij hen.

---

## Anti-patterns in jouw antwoorden

| Anti-pattern | Vervang door |
|--------------|--------------|
| “Wij als team…” (altijd) | “Ik was accountable voor…; het team droeg bij aan…” |
| Tech dump | Eén keuze + waarom + resultaat |
| “Ik leer snel” zonder plan | “Week 1–2 doe ik X, Y, Z” |
| Bescheidenheid die bewijs wist | Feiten noemen zonder opscheppen |
| Overclaim MCP/energie | Eerlijk + brug + hoe je het inhaalt |

---

## Jouw “anders dan gemiddelde senior”

Niet omdat je “beter” bent — omdat je **combinatie** zeldzaam is in NL-vacatures:

- Echte .NET-diepte  
- Echte agentic productie (niet alleen wrappers)  
- Frontend genoeg om af te maken  
- 0→1 ownership  
- Side-projects die oordeel tonen (Echo)  
- Schaal-ervaring met money-paths  

Dat is de zin die je mag verdienen in het gesprek — mits elk deel bewijsbaar blijft.
