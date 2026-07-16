# Deel VI — AI Native cheats (interview-ready)

Alleen wat deze rol vraagt. Doel: je kunt elk begrip in **één heldere zin** uitleggen en aan Yaya/Echo koppelen.

## Cheatsheet — definities in mensentaal

| Term | Zeg dit |
|------|---------|
| **LLM** | Model dat tokens voorspelt; nuttig met tools, data en policies eromheen |
| **Prompt** | Instructie + context; belangrijk, maar geen vervanging voor architectuur |
| **Tool calling** | Model vraagt een functie aan; jouw runtime valideert en voert uit |
| **Agent** | Doelgericht systeem: model + tools + state + stopcondities/policies |
| **RAG** | Eerst ophalen uit eigen data, dan genereren met bronnen |
| **Embeddings** | Numerieke betekenisvectoren om similarity/search te doen |
| **Vector DB** | Opslag/index voor embeddings (bij jou: o.a. Chroma-ervaring) |
| **MCP** | Standaardprotocol om tools/context aan agents & devtools te koppelen |
| **Evals** | Geautomatiseerde (en soms menselijke) tests op AI-gedrag/kwaliteit |
| **Tracing** | Spoor van prompts, tool calls, latency, errors, kosten |
| **Fallback** | Veilig alternatief als AI faalt of onzeker is |
| **Guardrail** | Harde limiet: authz, allowlists, max steps, output filters |
| **Hallucinatie** | Model verzint; bestrijd met grounding + detectie + weigeren |

---

## Agents — whiteboard in 60 seconden

```text
User / UI
   → Orchestrator (state machine / graph)
        → LLM (plan / choose tool)
        → Tool layer (RBAC, validation, timeouts)
        → Data platform (tenant-scoped)
        → LLM (observe / next step)
   → Result + citations / actions
   → Trace + eval hooks
```

**Jouw bewijs:** LangGraph StateGraph + checkpointing; LiveKit voice loop; Echo intent router.

> **Perfect answer**  
> “De slimheid zit niet alleen in het model, maar in de tool-contracts en state machine.”

---

## RAG — wanneer wel, hoe goed

**Wel:** kennis/antwoorden die in documenten of historische data staan; uitleg met bronnen.  
**Niet als enige:** actuele meterstanden die beter via API/tool komen; writes; compliance-besluiten.

### Kwaliteitshefbomen (noem er twee in een gesprek)

1. Chunking + metadata (gebouw, periode, meetserie)
2. Retrieval eval (kwam het juiste document terug?)
3. Answer eval (was het antwoord grounded?)
4. Verse indexatie / ACL’s per tenant

**Jouw bewijs:** LlamaIndex + Chroma + OpenAI embeddings + Azure Blob ingest.

---

## Evals — minimale productiebar

Voor elke AI-feature vóór prod:

1. **Golden set** van 20–50 echte cases (later groeien)
2. Assertions: “moet tool X aanroepen”, “mag geen tenant Y zien”, “moet bron noemen”
3. Regressie in CI op kritieke intents
4. Online monitoring: thumbs, escalation rate, cost/latency

**Jouw bewijs:** DeepEval/semantic checklist matching; E2E audio generatie om gedrag te valideren.

> **CEO Insight**  
> Evals = hoe je belooft dat AI niet stil kapotgaat na release.

---

## Tracing & observability

Log per request (privacy-aware):

- feature + tenant (id)
- model + token usage + cost
- tool calls + latency + success/fail
- fallback triggered?
- trace id gekoppeld aan App Insights/OTel

**Jouw bewijs:** OpenTelemetry / App Insights op Yaya-platform.

---

## Fallbacks & guardrails (energy SaaS)

| Risico | Guardrail |
|--------|-----------|
| Cross-tenant leak | Authz in tool-laag, tests |
| Foute besparingsclaim | Bronplicht + confidente drempels |
| Runaway agent | Max steps, timeouts, circuit breaker |
| High cost | Budget caps, model routing |
| Write-acties | Human-in-the-loop of sterke allowlist |

> **Don't say this**  
> “We prompten het model om voorzichtig te zijn” als enige controle.

---

## MCP & CLI voor het team

Waarom de vacature dit noemt: **dezelfde tools** voor mensen en agents.

Interview-pitch:

> “Ik wil MCP/CLI-servers die veilige, gedocumenteerde operaties exposen — aansluitingen opvragen, aggregaties draaien, tickets maken — zodat engineers met Claude Code dezelfde contracten gebruiken als productie-agents. Dat versnelt development en verkleint drift tussen ‘wat de agent kan’ en ‘wat wij lokaal testen’.”

Eerlijkheidsregel: je hoeft geen jaren MCP-productie te claimen; wel tool-calling + developer tooling mindset.

---

## Frameworks — hoe je erover praat

| Tech | Jouw relatie | Zeg |
|------|--------------|-----|
| LangGraph | Hands-on Yaya | State/tool loops gebouwd |
| LangChain | Gebruikt in stack | Messages/tools/streaming |
| LlamaIndex | RAG path | Ingest/retrieve |
| Semantic Kernel / OpenAI Agents SDK | Niet claimen als expert | Ken het landschap; kies op team+.NET-fit |
| Cursor / Claude Code / Copilot | Dagelijks | Stuur & review |

> **Perfect answer**  
> “Frameworks zijn replaceable. Contracts, evals en data access niet.”

---

## .NET + AI — brugzin

Energiemissie vraagt sterke .NET. Jij hebt Python AI-services naast .NET gebouwd. Framing:

> “Ik hou domain/API’s graag in .NET — saai, typed, testbaar. Model-orkestratie kan .NET of Python zijn afhankelijk van ecosystem en team. De grens trek ik bij duidelijke service contracts, niet bij hype.”

---

## Cost & latency — één minuut

- Router model (klein) beslist; groot model alleen voor zware redenering
- Cache retrieval en stabiele tool results
- Prefetch data in job; LLM over samenvatting i.p.v. raw dumps
- Stream naar UI voor perceived performance
- Meet €/feature en p95 latency als productmetrics

---

## Security one-liner

> “Alles wat een agent mag, moet een normale API-user met dezelfde rol ook mogen — niet meer.”

---

## 10 flitsvragen (antwoord ≤20 seconden)

1. **Agent vs. chatbot?** → tools + state + goal vs. alleen dialoog  
2. **RAG vs. fine-tuning?** → feiten/documenten wijzigen vaak: RAG; gedrag/stijl: soms tune  
3. **Temperature?** → lager voor feitelijke/tool flows  
4. **Embeddings model wisselen?** → hermeten + herindexeren  
5. **Idempotency?** → tool retries veilig maken  
6. **Eval vs. unit test?** → gedrag/kwaliteit vs. deterministische logica  
7. **Prompt injection?** → tool-laag vertrouwt modeloutput niet blind  
8. **PII?** → redaction, dataminimalisatie, retention  
9. **Multi-agent?** → alleen bij duidelijke rol-splits; complexiteit kost  
10. **Succesmetric AI-feature?** → klantactie/besparing/tijd, niet “tokens used”

---

## Uitlegpatronen (CEO → CTO)

### Aan de CEO (Pieter)

Gebruik: probleem → aanpak → risico → metric.  
Vermijd: framework-namen zonder vertaling.

**Voorbeeld:**  
“We laten een agent alleen gegevens *lezen* die de gebruiker mag zien, en we meten of experts het eens zijn met de suggesties voordat we meer autonomie geven.”

### Aan een CTO die bouwt

Gebruik: contracts, boundaries, failure modes, operability.  
Frameworks oké, maar secundair.

**Voorbeeld:**  
“Tool layer is de security boundary. Orchestration mag LangGraph of SK zijn; ik wil eerst idempotente, geautoriseerde capabilities en trace propagation vanuit .NET.”

---

## Mini-ontwerp: “Besparingssignaal”-feature

Gebruik dit als concreet ontwerpgesprek (hypothese).

1. **Trigger:** schedule of user opent monitor  
2. **Retrieve:** relevante meetreeksen + metadata object  
3. **Analyze:** rules + LLM voor uitleg (niet voor raw math waar rules beter zijn)  
4. **Tools:** `GetUsage`, `GetBenchmarks`, `CreateTask` (optioneel, guarded)  
5. **Guardrails:** geen cross-tenant; max cost; weigeren bij lage confidence  
6. **UI:** kaart in bestaande portal met bronlinks + “waarom dit signaal”  
7. **Evals:** 30 golden cases; “juiste gebouw”, “geen verzonnen €”  
8. **Fallback:** klassieke threshold alert zonder narratief  

> **Perfect answer**  
> “Ik laat het model vertellen en routeren; ik laat het niet stilletjes euro’s verzinnen.”

---

## Begrippen die je mag durven corrigeren

Als iemand zegt “we doen AI” en bedoelt alleen een wrapper:

- Vraag naar tools, evals, authz, cost  
- Bied aan hoe jij “AI-native” definieert: agents/capabilities geïntegreerd in product loops  

Blijf respectvol — zij hebben het platform gebouwd; jij brengt een lens mee.

---

## Week-1 leerplan AI×energie (zodat je concreet klinkt)

1. Glossary: aansluiting, EAN, meetverantwoordelijke, allocatie, etc. (uit interne docs/support)  
2. Één tenant happy-path in de UI naspelen  
3. Data access patterns: wie mag wat zien?  
4. Bestaande jobs/rapportages die AI zou kunnen versnellen  
5. Risicoregister: waar hallucinatie het duurst is (facturen, compliance)  

Dit toont nederigheid + plan — sterker dan “ik zoom me in”.
