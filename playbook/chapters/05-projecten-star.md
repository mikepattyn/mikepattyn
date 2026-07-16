# Deel V — Projecten en STAR-bibliotheek

Herschrijf projecten als **bewijs**, niet als CV-opsomming. In het gesprek noem je maximaal één project per antwoord, met één scherpe les.

## Projectkaart

| Project | Bewijs-type | Wanneer inzetten |
|---------|-------------|------------------|
| **Yaya** | Ownership + AI engineering + fullstack .NET/Angular | Primaire story voor bijna alles |
| **Echo** | Side-project AI agent + RAG + product judgment | Nice-to-have, MCP/agents, “bouw je zelf ook?” |
| **Flyingdarts** | Cloud/serverless + 0→1 productdenken | AWS, schaalbaarheid, eigenaarschap buiten werk |
| **Donatieplatform (2018–2023)** | Schaal, betrouwbaarheid, payments, multi-stack | “100k users”, verantwoordelijkheid, .NET historie |

---

## Yaya — bewijs van ownership én AI engineering

**Eén zin:** Ik bouwde als primary engineer een AI-powered real-time communicatieplatform van idee naar productie in ongeveer twaalf maanden.

### Architectuur (interview-niveau)

```text
Angular (4 apps, monorepo)
    ↕ SignalR / HTTP
ASP.NET Core 9 API (CQRS/MediatR, EF Core, SQL Server)
    ↕
Python AI services (LiveKit agents, LangGraph, RAG)
    ↕
Azure (Blob, Key Vault, App Config, Notification Hubs, App Insights)
```

**Wat jij deed dat telt voor Energiemissie:**

- Einde-tot-einde ownership in startupfase (architectuur + productrichting)
- Agents met tool/workflow-orkestratie (LangGraph) live
- Voice pipelines (STT/TTS/VAD) — bewijst real-time + multimodal thinking
- RAG met embeddings + bronnen (LlamaIndex/ChromaDB)
- Kwaliteit: DeepEval / semantic evaluation, E2E inclusief generated audio
- .NET backbone die “saaie” productie-eisen serieus neemt (auth, migrations, validation, telemetry)

> **CEO Insight**  
> Noem niet alle tech. Noem: probleem → wat jij eigende → resultaat voor gebruiker → hoe je kwaliteit afdwong.

### STAR — Ownership 0→1

- **S:** Startupfase; platform moest van PoC naar stabiele MVP voor echte gebruikers.
- **T:** Als primary engineer architectuur en levering dragen zonder het product te fragmenteren over frontend/backend/AI.
- **A:** CQRS API + Angular monorepo + AI-services; strakke client feedback loop; kwaliteitstooling (Jest/Playwright/xUnit/evals) vroeg ingebouwd.
- **R:** Productie-ready MVP binnen ~12 maanden; één ownership-lijn over de stack.
- **Les:** AI-features overleven alleen als het platform eromheen (auth, data, observatie) volwassen is.
- **Competenties:** ownership, fullstack, productdenken, tempo met kwaliteit
- **Gebruik bij:** “Vertel over jezelf”, “grootste prestatie”, “waarom jij”

### STAR — Agents in productie

- **S:** Real-time conversatie/AI-workflows moesten betrouwbaar genoeg zijn voor echte sessies, niet alleen demo.
- **T:** Voice + agent orchestration leveren met duidelijke failure modes.
- **A:** LiveKit Agents + LangGraph state/checkpointing + streaming; monitoring via OpenTelemetry/App Insights; evaluatieflows met embeddings/DeepEval.
- **R:** AI-gedrag testbaar en observeerbaar; regressies eerder zichtbaar.
- **Les:** “Agent werkt” is betekenisloos zonder evals, tracing en fallback.
- **Competenties:** LLM product engineering, tool/workflow design, productie-discipline
- **Gebruik bij:** vacature-eis agents/tool use, evals/tracing

### STAR — RAG met bronnen

- **S:** Antwoorden moesten steunen op bronmateriaal, niet op modelgokwerk.
- **T:** Retrieval pipeline bouwen die documenten uit blob-opslag bruikbaar maakt voor agents.
- **A:** Ingest → embeddings → ChromaDB → LlamaIndex retrieval → LLM met source-backed antwoorden.
- **R:** Traceerbare antwoorden richting gebruiker/use-case.
- **Les:** RAG-kwaliteit = data-kwaliteit + chunking + eval van retrieval, niet alleen “vector DB erbij”.
- **Gebruik bij:** energy intelligence / analyses met uitleg

### STAR — Fullstack kwaliteit

- **S:** Vier Angular apps + gedeelde libs dreigden inconsistent te worden.
- **T:** Monorepo-discipline en shared libraries voor UI, auth, logging, Sentry.
- **A:** pnpm/Turbo, standalone components, NgRx + SignalR patterns, CI lint/test.
- **R:** Snellere feature-levering zonder vier keer hetzelfde wiel.
- **Gebruik bij:** fullstack, frontend-affiniteit, schaal van codebase

---

## Echo — bewijs van AI side-project met oordeel

**Eén zin:** Ik bouw een Discord-agent die harm-reduction kennis veilig routeert via RAG, chat of web search — met disclaimers en privacy-scoped memory.

Waarom dit telt voor Energiemissie:

- Intent classification + tool/route keuze (agentic patroon)
- RAG op gecureerde kennis
- Product judgment: wanneer wél/niet antwoorden, tone, risk
- Eigen initiative buiten werktijd

> **Don't say this**  
> Maak Echo niet groter dan Yaya. Echo is versterking; Yaya is het hoofdargument.

### STAR — Intent routing

- **S:** Gebruikersvragen variëren van feitelijk tot gevoelig; één generieke chat is onveilig/onbruikbaar.
- **T:** Elke query naar het beste pad routeren.
- **A:** Classifier → RAG collectie / conversatie / web search; antwoorden als embeds + disclaimers.
- **R:** Consistenter, veiliger antwoordgedrag.
- **Les:** Agent design = policy design.
- **Gebruik bij:** “Hoe ontwerp je agents?”, guardrails

---

## Flyingdarts — bewijs van cloud en makersmentaliteit

**Eén zin:** Ik bouwde een real-time online dartsplatform op AWS serverless (API Gateway, Lambda, DynamoDB single-table), met een langetermijnvisie op ML dart-detectie.

### STAR — Serverless product

- **S:** Real-time multiplayer + social features vroegen om schaalbare, betaalbare backend.
- **T:** Architectuur kiezen die pieken aankan zonder zware ops.
- **A:** Serverless API + DynamoDB single-table design; productfocus op social/real-time UX.
- **R:** Werkend productpad + leerlijn cloud data modeling.
- **Gebruik bij:** AWS, eigen projecten, schaaldenken

---

## Donatieplatform — bewijs van schaal en verantwoordelijkheid

**Eén zin:** Ik werkte jaren aan een stack met 100.000+ users en meer dan een miljoen euro donaties per maand — fullstack, cloud, payments.

### STAR — Geld en betrouwbaarheid

- **S:** Payment-integraties en hoge gebruikersvolumes; fouten kosten echt geld en vertrouwen.
- **T:** Features en onderhoud leveren over Angular/React/mobile/.NET met Azure/AWS.
- **A:** Integraties met Slimpay/EazyCollect/Stripe; multi-stack delivery; ook Scrum/PO verantwoordelijkheden.
- **R:** Langdurige productie-ervaring op bedrijfskritische flows.
- **Les:** “Shipt snel” betekent niets zonder respect voor money-paths en auditability — relevant voor energiefacturen/compliance-denken.
- **Gebruik bij:** schaal, .NET historie, ownership in bestaande producten

---

## Hoe je projecten “verkoopt” in één adem

### Template

> Op **[project]** had ik **[ownership-scope]**. Het probleem was **[gebruikersprobleem]**. Ik koos **[1–2 technische keuzes]** omdat **[constraint]**. Resultaat: **[concrete uitkomst]**. Wat ik meeneem naar Energiemissie: **[brug naar 250k aansluitingen / agents / .NET / evals]**.

### Voorbeeld (60 seconden)

> Op Yaya was ik primary engineer: van idee naar productie in ongeveer een jaar. We hadden real-time communicatie plus AI-agents nodig die echt live moesten. Ik zette een ASP.NET Core CQRS-API neer met Angular-frontends, en Python-services voor LangGraph-agents, voice en RAG. Belangrijker dan de stack: we dwongen evaluatie en telemetry af zodat AI-gedrag testbaar werd. Dat is precies het spiergeheugen dat je nodig hebt voor agentic features op een data platform met honderdduizenden aansluitingen.

---

## Mini-STAR cheat (8 kaarten)

Leer deze acht “triggers” — niet uit het hoofd als toneelstuk, wel als spiergeheugen:

1. **Ownership** → Yaya 0→1  
2. **Agents live** → LangGraph + LiveKit  
3. **Evals/tracing** → DeepEval + App Insights  
4. **RAG** → LlamaIndex/Chroma/Blob  
5. **.NET diepte** → CQRS, EF migrations, SignalR  
6. **Frontend op schaal** → Angular monorepo  
7. **Side-project oordeel** → Echo routing/policies  
8. **Bedrijfskritische schaal** → donatieplatform 100k users / payments  

Als een vraag komt: kies **één** kaart, vertel 45–75 seconden, stop.

---

## Whiteboard: Yaya in 3 minuten

Als ze vragen “schets je architectuur”:

1. Teken drie banden: **Clients (Angular)** → **.NET API** → **AI services**  
2. Zet erbij: SignalR/real-time, SQL/EF, Azure blob/config/insights  
3. Zoom in op AI: **LangGraph loop** met tools + streaming; voice via LiveKit  
4. Zeg hardop waar kwaliteit zit: tests, evals, telemetry  
5. Brug: “Bij Energiemissie zou de middelste band jullie SaaS/.NET zijn; AI-band praat via strikte tools met tenant-scope naar het data platform.”

### Wat je níet tekent

- Elke NuGet-package  
- Interne klassen  
- Een LLM als database  

---

## STAR — Client feedback loop

- **S:** Startupfase; requirements bewegelijk; risico op bouwen wat niemand nodig heeft.  
- **T:** Snel leren zonder de codebase te laten rotten.  
- **A:** Korte cycles met client; PoCs bewust omzetten naar stabiele paden; kwaliteitstooling vroeg.  
- **R:** MVP die in echte omgevingen gebruikt werd; minder “verrassing bij oplevering”.  
- **Brug:** Energy features moeten met Product/klantpaden getest — niet alleen met synthetic prompts.  

---

## STAR — Parallel leadership (Scrum/coach)

- **S:** Naast Yaya ook faciliteren op ander product (Flutter modernisering, UI-test automation).  
- **T:** Delivery helpen zonder zelf de enige builder te zijn.  
- **A:** Backlog refinement met PO; test automation ownership; structure/performance verbeteren met team.  
- **R:** Betrouwbaarder releases; duidelijkere stories.  
- **Gebruik bij:** “Hoe werk je met Product?”, leadership zonder titel.  
- **Don't:** Dit groter maken dan je AI/.NET hoofdverhaal.

---

## Frasering: van techniek naar Energiemissie

| Yaya-detail | Brugzin |
|-------------|---------|
| 4 Angular apps | “Ik lever features tot in de UI — belangrijk als AI in de SaaS-flow moet landen.” |
| CQRS/MediatR | “Side-effects en queries scheiden — handig als agents writes mogen doen.” |
| LangGraph | “State + tools — het skelet van agentic features.” |
| RAG + embeddings | “Grounding — onmisbaar bij energieadvies.” |
| DeepEval / audio E2E | “AI test je als gedrag, niet als string-equals.” |
| App Insights/OTel | “Zonder traces is een agent niet te runnen in prod.” |

---

## Wat je zwijgt (bewust)

- Interne klantnamen van Methylium/Yaya tenzij publiek  
- Credentials, private Azure details, ongepubliceerde metrics  
- Negatieve framing van werkgever  

Blijf feitelijk en professioneel; het interview is geen therapie over je huidige job.
