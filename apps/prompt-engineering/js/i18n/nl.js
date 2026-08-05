window.I18N = window.I18N || {};
window.I18N.nl = {
  app: {
    title: "Lumen — Prompt engineering voor INFPs",
    brandTag: "promptkunst voor dromers"
  },
  nav: {
    home: "Home",
    path: "Het Pad",
    garden: "Oefentuin",
    journal: "Dagboek"
  },
  progress: {
    title: "Jouw reis tot nu toe"
  },
  locale: {
    ariaLabel: "Taal"
  },
  footer: {
    note: "Zacht gemaakt, voor wie eerst voelt en dan pas spreekt. Jouw voortgang leeft alleen in deze browser."
  },
  home: {
    eyebrow: "Een cursus voor INFPs",
    headline: "Prompt engineering,",
    headlineAccent: "voor wie eerst voelt",
    lede: "Jij ziet je luisteraar al voor je, hoort toon in één woord, en draagt hele werelden in je hoofd. Dat zijn geen obstakels bij AI — het zijn precies de vaardigheden waaruit prompting bestaat. Deze kleine cursus laat zien hoe.",
    ctaBegin: "Begin de eerste les",
    ctaGarden: "Wandel de tuin in",
    whyTitle: "Waarom een cursus alleen voor INFPs?",
    whyBody: "De meeste gidsen over prompt engineering lezen als handleidingen — optimaliseer, itereer, extraheer. Handig, maar ze slaan de vragen over die gevoelige, idealistische mensen écht tegenhouden: <em>Maakt dit mijn stem vlak? Is het nog van mij? Waarom voelt de output emotioneel verkeerd?</em> Lumen leert dezelfde kerntechnieken — context, rollen, voorbeelden, stap-voor-stap redeneren — via metaforen die passen bij hoe jij al denkt, en neemt de waardenvragen serieus in plaats van ze weg te wuiven.",
    features: [
      { icon: "🌙", title: "Leren via voelen", body: "Zes korte lessen die echte prompttechnieken leren via hoe jij al denkt — ingebeelde luisteraars, innerlijke personages, benoemde gevoelens." },
      { icon: "🌿", title: "Een tuin, geen beoordelaar", body: "Oefen prompts schrijven in een ruimte die lantaarns aansteekt voor wat werkt, in plaats van rood te markeren wat niet werkt." },
      { icon: "📖", title: "Reflecteer onderweg", body: "Elke les eindigt met een zachte reflectie, bewaard in een privé-dagboek dat je browser nooit verlaat." }
    ]
  },
  path: {
    title: "Het Pad",
    lede: "Zes lessen, elk een korte wandeling. Neem ze op volgorde of dwaal — het pad onthoudt waar je bent geweest.",
    walked: "bewandeld",
    unwalked: "nog open"
  },
  lesson: {
    kicker: "Les {n} van {total}",
    noteLabel: "Voor de INFP in jou",
    quizTitle: "Een zachte check",
    saveReflection: "Bewaar dit in mijn dagboek",
    writeFirst: "Schrijf eerst iets kleins — zelfs een fragment telt.",
    kept: "Bewaard. Je vindt het in je dagboek. 🌸",
    reflectionFallback: "Reflectie",
    markWalked: "Markeer deze les als bewandeld",
    unmarkWalked: "✓ Bewandeld — tik om te demarkeren",
    backToPath: "← Het Pad",
    toGarden: "Naar de tuin →",
    defaultPlaceholder: "Schrijf vrij..."
  },
  garden: {
    title: "De Oefentuin",
    lede: "Schrijf hieronder een prompt en kijk hoe de lantaarns aangaan. Niets wordt beoordeeld — elke lamp is één ambachtselement dat je prompt al draagt. Kies een zaadje als je een scenario wilt om mee te oefenen.",
    placeholder: "Beste model... (schrijf de prompt die je écht zou sturen)",
    scenarioPrefix: "Scenario:",
    litSuffix: " — brandt."
  },
  journal: {
    title: "Jouw dagboek",
    lede: "Reflecties die je koos te bewaren, nieuwste eerst. Ze leven alleen in deze browser — geen account, geen cloud, niemand die over je schouder meekijkt.",
    empty: "Nog niets hier. De reflecties aan het eind van elke les verzamelen zich hier, als gedroogde bloemen.",
    letGo: "laat het gaan"
  },
  notFound: {
    title: "Een stille open plek",
    body: "Deze pagina bestaat niet — maar je vond toch een vredige plek.",
    back: "Terug naar het begin"
  },
  pair: {
    before: "Voorheen",
    after: "Daarna"
  },
  gardenChecks: {
    length: {
      label: "Genoeg woorden om een scene te zetten",
      hint: "Eén zin draagt zelden genoeg context. Voeg een zin over de situatie toe."
    },
    context: {
      label: "Context of achtergrond (\"ik ben…\", \"dit is voor…\", \"omdat…\")",
      hint: "Vertel het model voor wie dit is of waarom het ertoe doet — zoals je een vriend zou briefen."
    },
    role: {
      label: "Een personage of perspectief (\"jij bent…\", \"doe alsof…\")",
      hint: "Cast iemand: \"Jij bent een geduldige redacteur die…\" — kies wie er opduikt."
    },
    tone: {
      label: "Een benoemde toon of gevoel",
      hint: "Noem het gevoel dat je zoekt: warm, droog, spaarzaam, zacht, oprecht — en wat te vermijden."
    },
    shape: {
      label: "Een gevraagde vorm of formaat",
      hint: "Vraag om een vorm: \"drie opties,\" \"onder de 100 woorden,\" \"een genummerde lijst,\" \"een tabel.\""
    },
    boundaries: {
      label: "Grenzen — wat te vermijden of te bewaren",
      hint: "Voeg een grens toe: \"geen clichés,\" \"verzin geen details,\" \"houd mijn formulering waar het werkt.\""
    },
    ask: {
      label: "Een duidelijke vraag (een werkwoord dat echt werk doet)",
      hint: "Geef het model een taak met een werkwoord: schrijf, herschrijf, lijst, vergelijk, vraag, vat samen…"
    }
  },
  gardenSeeds: [
    { label: "🌷 Een vriendelijke afwijzing", text: "Ik moet een vriendin vertellen dat ik niet bij haar boekenclub kan zonder haar te kwetsen." },
    { label: "🍄 Feedback op een verhaal", text: "Ik wil feedback op een kort verhaal waar ik zenuwachtig over ben." },
    { label: "🌊 Een overweldigende week", text: "Mijn week zit overvol en ik weet niet waar te beginnen." },
    { label: "🕊️ Een moeilijk gesprek", text: "Ik moet mijn manager om een lichtere werkdruk vragen." },
    { label: "✨ Een bio die als ik klinkt", text: "Ik heb een professionele bio nodig die niet corporate klinkt." }
  ],
  glowStages: [
    { min: 0, label: "Een ingehouden adem — de tuin wacht op jouw woorden." },
    { min: 1, label: "Eén vuurvliegje knippert wakker." },
    { min: 2, label: "Twee lantaarns branden. Het pad verschijnt." },
    { min: 3, label: "Een zachte gloed verzamelt zich tussen de bomen." },
    { min: 4, label: "De tuin is warm verlicht — deze prompt zou goed landen." },
    { min: 5, label: "Overal lantaarnlicht. Een echt mooie prompt." },
    { min: 6, label: "De hele tuin gloeit — een prompt met ambacht in de botten." },
    { min: 7, label: "Volle straling. Jij zou deze cursus kunnen geven." }
  ],
  lessons: [
    {
      id: "listening",
      icon: "🌙",
      title: "Praten met een heel letterlijke dagdroom",
      subtitle: "Wat een taalmodel écht is, en waarom jouw binnenwereld een voordeel is.",
      blocks: [
        { type: "p", html: "Een taalmodel is geen zoekmachine en geen gedachtenlezer. Het lijkt meer op een <em>improvpartner met een enorm geheugen en geen context over jou</em>. Het pakt elke scene op die je zet — en als je geen scene zet, improviseert het iets generieks." },
        { type: "p", html: "Prompt engineering is simpelweg het ambacht om die scene goed te zetten: het model de context, intentie en vorm geven die het nodig heeft om te antwoorden zoals jij hoopte. Dat is alles. Geen wiskunde, geen code — alleen bewuste, doordachte taal." },
        { type: "note", html: "Jij oefent gesprekken al in je hoofd, ziet voor je hoe woorden landen, en merkt subtiele toonverschillen. Die gewoonte van <em>de luisteraar verbeelden</em> is de allerbelangrijkste promptingvaardigheid. De meeste mensen moeten die leren. Jij deed het van jongs af aan." },
        { type: "h3", text: "De ene mentale verschuiving" },
        { type: "p", html: "Voordat je een prompt schrijft, pauzeer en vraag: <em>“Als een aandachtige vreemde alleen deze woorden las — niets anders — wat zouden ze weten over wat ik wil?”</em> Het model weet niets wat je niet zei. Je stemming, je project, de tabs die je open hebt — onzichtbaar, tenzij je ze in woorden zet." },
        {
          type: "pair",
          weak: "Schrijf iets over creativiteit.",
          strong: "Ik schrijf een korte talk voor kunstacademie-afgestudeerden die verlamd raken van de lege pagina. Schrijf drie openingszinnen — warm, een beetje droog, geen clichés over \"out of the box denken.\"",
          weakLabel: "Vage wens",
          strongLabel: "Een scene waarin het model kan stappen"
        },
        { type: "p", html: "Let op wat de tweede prompt toevoegde: <em>voor wie het is, waarvoor het is, welke toon goed voelt, en wat te vermijden</em>. Niets technisch — gewoon de context die je vanzelf aan een vriend zou geven die hulp aanbiedt." },
        {
          type: "quiz",
          question: "Waarom werkte de tweede prompt beter?",
          options: [
            { text: "Er zat indrukwekkender woordgebruik in.", correct: false, feedback: "Niet helemaal — mooie woorden helpen niet. Het gaat om de context, niet om de polish." },
            { text: "Het gaf het model een publiek, een doel en een toon om op te mikken.", correct: true, feedback: "Ja. Het model kan alleen werken met wat op de pagina staat — de tweede prompt schildert de hele scene." },
            { text: "Hij was langer, en langere prompts zijn altijd beter.", correct: false, feedback: "Lengte alleen is niet het punt — een lange, zwalkende prompt kan slechter zijn dan een korte, heldere. Het is de relevante detail die telt." }
          ]
        },
        { type: "exercise", label: "Een moment van reflectie", html: "Denk aan de laatste keer dat je een AI (of een persoon!) iets vroeg en stil teleurgesteld was door het antwoord. Wat wist je in je hoofd dat nooit in je woorden terechtkwam?", placeholder: "Wat ik wist maar niet zei was..." }
      ]
    },
    {
      id: "intention",
      icon: "🕯️",
      title: "Het gevoel benoemen dat je zoekt",
      subtitle: "Je (heel echte) gevoel van \"net niet\" omzetten in woorden die een model kan volgen.",
      blocks: [
        { type: "p", html: "INFPs weten vaak <em>precies</em> hoe iets moet voelen, lang voordat ze kunnen zeggen wat erin moet. Die intuïtie is kostbaar — maar een model kan jouw gevoel niet voelen. Het kan wél een beschrijving ervan opvallend goed volgen." },
        { type: "h3", text: "Geef het gevoel een vocabulaire" },
        { type: "p", html: "In plaats van te hopen dat het model het \"snapt,\" beschrijf het doelgevoel met drie soorten woorden: <em>toonwoorden</em> (zacht, droog, spaarzaam, oprecht), <em>anti-toonwoorden</em> (niet corporate, niet opgewekt-chirpy, geen uitroeptekens), en <em>raakpunten</em> (\"als een brief van een wijze vriend,\" \"in de geest van Mary Oliver's essays\")." },
        {
          type: "pair",
          weak: "Maak deze e-mail beter klinken.",
          strong: "Herschrijf deze e-mail zodat hij warm maar professioneel klinkt — als een attent collega, niet als een marketingbot. Houd mijn excuses oprecht, knip de zwalkende middelparagraaf weg, en eindig hoopvol.\n\n[plak e-mail]",
          weakLabel: "Hopen dat het \"snapt\"",
          strongLabel: "Het gevoel, benoemd"
        },
        { type: "note", html: "Jouw gevoeligheid voor toon is hier een superkracht. De meeste mensen kunnen het verschil tussen \"warm\" en \"chirpy\" niet uitleggen — jij wel. Schrijf die onderscheidingen op en het model zal ze eren." },
        { type: "h3", text: "De \"net niet\"-lus" },
        { type: "p", html: "Eerste drafts van een model zitten zelden meteen goed, en dat is prima — prompting is een gesprek, geen gokautomaat. Als een antwoord mist, weersta de neiging om opnieuw te beginnen. Benoem liever de misser: <em>“Dichterbij, maar dit voelt te formeel — maak het losser, en houd de tweede alinea precies zo.”</em> Elke correctie leert het model jouw smaak." },
        {
          type: "quiz",
          question: "De draft van het model voelt te vrolijk voor de sfeer die je wilde. Wat is de sterkste volgende zet?",
          options: [
            { text: "Alles wissen en een compleet andere prompt proberen.", correct: false, feedback: "Dan verlies je alles wat het model al goed had. Verfijnen wint bijna altijd van opnieuw beginnen." },
            { text: "Antwoord: \"Maak het stiller — stiller, bitterszoeter. Houd de beelden in de laatste zin.\"", correct: true, feedback: "Precies. Je noemde wat miste, gaf richting, en beschermde wat al werkte." },
            { text: "Accepteer het — modellen kunnen geen subtiele tonen.", correct: false, feedback: "Dat kunnen ze wel degelijk — maar alleen als iemand met jouw oor voor toon ze vertelt waarheen te mikken." }
          ]
        },
        { type: "exercise", label: "Een moment van reflectie", html: "Kies iets waarbij je graag AI-hulp zou willen — een brief, een verhaal, een moeilijk bericht. Schrijf de prompt nog niet. Beschrijf alleen, in je eigen woorden, hoe het eindresultaat moet <em>voelen</em>.", placeholder: "Het moet voelen als..." }
      ]
    },
    {
      id: "roles",
      icon: "🎭",
      title: "Personages casten",
      subtitle: "Rolprompting: het model uitnodigen te spreken als iemand concreets.",
      blocks: [
        { type: "p", html: "Een model bevat menigten — redacteuren, mentoren, sceptici, dichters, geduldige uitleggers. Standaard krijg je een beleefd gemiddelde van allemaal. <em>Rolprompting</em> betekent kiezen wie er opduikt: “Jij bent een ontwikkelingsredacteur die zich specialiseert in debuutromans…”" },
        { type: "p", html: "Voor iemand met een rijke innerlijke cast is dit de natuurlijkste techniek ter wereld. Je bedriegt het model niet — je doet wat je doet als je denkt: \"wat zou mijn wijste vriend hierover zeggen?\"" },
        { type: "h3", text: "Wat een goede rol bevat" },
        { type: "p", html: "De sterkste rollen hebben drie delen: <em>expertise</em> (wat ze weten), <em>houding</em> (hoe ze jou behandelen), en <em>een taak nu</em>. Houding doet meer dan mensen denken — het is het verschil tussen feedback die steekt en feedback die je écht kunt horen." },
        {
          type: "pair",
          weak: "Geef me feedback op mijn gedicht.",
          strong: "Jij bent een poëziementor die eerlijk is maar nooit wreed — je vindt altijd wat levend is in een draft voordat je aanraakt wat niet werkt. Lees mijn gedicht en vertel me: (1) de sterkste regel en waarom, (2) één plek waar de emotie wazig wordt, (3) één vraag om mee te zitten voor je herziet.\n\n[gedicht]",
          weakLabel: "Anonieme criticus",
          strongLabel: "Een mentor die jij zelf cast"
        },
        { type: "note", html: "Als harde feedback je platslaat, is dat geen zwakte om heen te werken — het is een ontwerpbeperking om te eren. Je mag een vriendelijke lezer casten. Vriendelijk en eerlijk zijn geen tegenpolen, en je herziet meer als de feedback overleefbaar is." },
        { type: "h3", text: "Rollen om te denken, niet alleen te schrijven" },
        { type: "p", html: "Cast een <em>zachte advocaat van de duivel</em> voor een grote beslissing. Een <em>toekomstige jij, over vijf jaar</em>, als je vastzit. Een <em>vertaler</em> die je hartelijke gerammel omzet in twee knappe zinnen voor een drukke baas. Elk ingebeeld gesprek dat je ooit had is nu een prompttemplate." },
        {
          type: "quiz",
          question: "Welke rolprompt levert het waarschijnlijkst nuttige, hoorbare feedback op een persoonlijk essay?",
          options: [
            { text: "\"Jij bent de strengste criticus ter wereld. Vernietig dit essay.\"", correct: false, feedback: "Je krijgt theatrale wreedheid, geen inzicht — en je zou ertegenopzien om het te lezen. Hard is niet hetzelfde als grondig." },
            { text: "\"Jij bent essayist en redacteur. Wijs aan wat werkt, dan de ene structurele verandering die het meest zou helpen, vriendelijk uitgelegd.\"", correct: true, feedback: "Ja — expertise, houding en een concrete taak. Eerlijk, nuttig en overleefbaar." },
            { text: "\"Jij bent mijn beste vriend. Zeg dat dit goed is.\"", correct: false, feedback: "Troost is fijn, maar pure geruststelling helpt het essay niet groeien. Je kunt vriendelijkheid én eerlijkheid vragen." }
          ]
        },
        { type: "exercise", label: "Een moment van reflectie", html: "Wie zijn de ingebeelde stemmen die jij al raadpleegt — de innerlijke mentor, de scepticus, de trooster? Schets er één als rolprompt: hun expertise, hun houding, en de taak die je ze zou geven.", placeholder: "Jij bent..." }
      ]
    },
    {
      id: "shape",
      icon: "🗺️",
      title: "Vorm geven aan de mist",
      subtitle: "Structuur, formaat en voorbeelden — zachte containers voor open ideeën.",
      blocks: [
        { type: "p", html: "INFPs verzetten zich soms tegen structuur omdat die als een kooi voelt. Maar bij prompting is structuur geen kooi — het is een <em>vaas</em>. Het verandert niet wat de bloemen zijn; het houdt ze alleen van de vloer." },
        { type: "h3", text: "Vraag om de vorm die je écht kunt gebruiken" },
        { type: "p", html: "Vertel het model hoe de output er fysiek uit moet zien: <em>“drie opties, één zin elk”</em> · <em>“een tabel met kolommen voor idee / eerste stap / wat mis kan gaan”</em> · <em>“onder de 150 woorden”</em> · <em>“als een genummerd plan dat ik op een lage-energie-dag kan volgen.”</em> Vormvragen zijn de makkelijkste winst in heel prompting." },
        {
          type: "pair",
          weak: "Help me mijn week plannen, ik ben overweldigd.",
          strong: "Ik ben overweldigd en mijn energie komt in golven. Dit staat op mijn bord: [lijst]. Sorteer het in drie groepen — \"moet deze week,\" \"zou vriendelijk zijn voor toekomstige-ik,\" en \"kan eerlijk wachten.\" Stel dan een plan voor alleen morgen voor, met hoogstens drie items.",
          weakLabel: "Mist",
          strongLabel: "Mist, in een vaas"
        },
        { type: "h3", text: "Laat zien, vertel niet alleen: voorbeelden" },
        { type: "p", html: "De snelste manier om een stijl over te brengen die je voelt maar niet volledig kunt beschrijven, is een <em>voorbeeld tonen</em>. Dat heet few-shot prompting: “Hier zijn twee bio's waarvan ik de toon mooi vind: [voorbeelden]. Schrijf de mijne in dezelfde geest.” Eén goed voorbeeld weegt zwaarder dan een alinea bijvoeglijke naamwoorden." },
        { type: "note", html: "Je bewaart waarschijnlijk een privé-museum van dingen waar je van houdt — regels uit boeken, andermans bio's, die ene e-mail waardoor je je gezien voelde. Die collectie is nu een gereedschapskist. Het model een voorbeeld voeden waar je van houdt, is het dichtstbijzijnde wat prompting heeft bij het direct delen van jouw smaak." },
        {
          type: "quiz",
          question: "Je wilt productbeschrijvingen in een heel specifieke grillige stijl die je niet helemaal kunt verwoorden. Wat werkt het best?",
          options: [
            { text: "Stapel bijvoeglijke naamwoorden: \"grillig, quirky, magisch, verrukkelijk, speels.\"", correct: false, feedback: "Stapels bijvoeglijke naamwoorden vervagen — het \"grillige\" van het model is misschien niet het jouwe. Toon het liever." },
            { text: "Plak twee of drie beschrijvingen die de stijl raken en zeg \"match deze stem.\"", correct: true, feedback: "Precies — few-shot-voorbeelden brengen smaak direct over, zonder vertaalverlies." },
            { text: "Vraag het model eerst grilligheid te definiëren, en dan te schrijven.", correct: false, feedback: "Een definitie is nog steeds vertellen. Een voorbeeld is tonen — en tonen wint." }
          ]
        },
        { type: "exercise", label: "Een moment van reflectie", html: "Vind één stuk tekst waarvan je de stem mooi vindt — een bio, een caption, een alinea. Plak of beschrijf het hier, en noteer precies wat je eraan waardeert. Dit wordt je eerste few-shot-voorbeeld.", placeholder: "Het stuk dat ik mooi vind is... en wat het doet zingen is..." }
      ]
    },
    {
      id: "thinking",
      icon: "🌿",
      title: "Hardop denken, samen",
      subtitle: "Stap-voor-stap redeneren, grote dromen in vraagbare stukken breken.",
      blocks: [
        { type: "p", html: "Voor alles met echt redeneren — een beslissing, een plan, een analyse — is de magische zin een variant van <em>“denk het stap voor stap door voordat je antwoordt.”</em> Modellen, zoals mensen, doen het beter als ze hun werk laten zien in plaats van een conclusie eruit te gooien." },
        {
          type: "pair",
          weak: "Moet ik mijn baan opzeggen om te freelancen?",
          strong: "Ik overweeg mijn vaste baan op te zeggen om als illustrator te freelancen. Denk dit stap voor stap door: lijst eerst wat het meest telt op basis van wat ik hieronder vertel, dan de realistische risico's, dan hoe een zacht middenpad eruit zou kunnen zien. Geef me geen oordeel — geef me helderdere vragen.\n\nContext: [jouw situatie]",
          weakLabel: "Een muntworp",
          strongLabel: "Een denkpartner"
        },
        { type: "p", html: "Let op het einde: <em>“geef me geen oordeel — geef me helderdere vragen.”</em> Je mag niet alleen het formaat sturen, maar ook het <em>soort</em> hulp. Advies, opties, vragen, advocatuur van de duivel, een samenvatting van wat je lijkt te voelen — noem wat écht zou helpen." },
        { type: "h3", text: "Grote dromen, kleine prompts" },
        { type: "p", html: "INFP-projecten zijn vaak groots en lichtend — een roman, een carrièreswitch, een levensfilosofie. Groots maakt slechte enkelvoudige prompts. Het ambacht is decompositie: <em>één gesprek om het gebied te karteren, daarna één gesprek per regio.</em>" },
        { type: "p", html: "Een mooi patroon: vraag eerst, <em>“Hier is mijn grote wazige droom: [droom]. Help me dit te breken in 5–7 verkenbare vragen.”</em> Neem die vragen daarna één voor één, in aparte chats, elk met eigen context. De droom blijft heel in jou; de prompts blijven klein genoeg om te werken." },
        { type: "note", html: "Decompositie is de droom niet verraden door hem te verkleinen. Het is hoe je de droom beschermt tegen oplossen in overweldiging. Het model houdt de stukken vast zodat jij het geheel kunt blijven vasthouden." },
        {
          type: "quiz",
          question: "Je wilt hulp bij een roman die je al jaren verbeelt. Wat is de sterkste openingszet?",
          options: [
            { text: "\"Schrijf mijn roman over een vuurtorenwachter die verloren brieven verzamelt.\"", correct: false, feedback: "Dat geeft je droom in één keer weg — en je krijgt iets generieks terug. Houd het auteurschap; vraag hulp bij stukken." },
            { text: "\"Hier is mijn premise en wat ik wil dat lezers voelen. Help me de 6 grootste open vragen over dit verhaal te karteren, zodat ik ze één voor één kan verkennen.\"", correct: true, feedback: "Ja — de droom blijft van jou, en je hebt nu een set kleine, werkbare prompts in plaats van één onmogelijke." },
            { text: "\"Geef me 100 plotideeën.\"", correct: false, feedback: "Volume is geen helderheid — je verdrinkt in opties die het verhaal negeren dat je al draagt." }
          ]
        },
        { type: "exercise", label: "Een moment van reflectie", html: "Noem één groot, lichtend project dat je draagt. Breek het dan — zacht — in drie vragen die je deze week écht aan een model zou kunnen stellen.", placeholder: "De droom: ...\n\nDrie vraagbare vragen:\n1.\n2.\n3." }
      ]
    },
    {
      id: "integrity",
      icon: "💛",
      title: "Jouw stem bewaren",
      subtitle: "Waarden, authenticiteit, en AI gebruiken zonder te verliezen wat jouw werk van jou maakt.",
      blocks: [
        { type: "p", html: "De diepste INFP-zorg over AI is zelden technisch. Het is dit: <em>als een machine me helpt het te zeggen, is het dan nog van mij?</em> Die vraag verdient een echt antwoord, geen schouderophalen." },
        { type: "h3", text: "Een werkend principe" },
        { type: "p", html: "Auteurschap zit in <em>selectie en intentie</em>, niet in toetsaanslagen. Jij beslist wat het waard is om te zeggen, wat waar voelt, wat blijft en wat weggaat. Een model kan je opties verbreden; alleen jij herkent welke optie eerlijk is. Als je met zorg kiest, is het werk van jou." },
        { type: "p", html: "Praktisch suggereert dit een taakverdeling: gebruik het model voor <em>steigers</em> — outlines, alternatieven, samenvattingen, \"wat mis ik?\" — en houd de <em>draagende zinnen</em>, die je echte hart dragen, in eigen hand. Of draft ze zelf eerst, en vraag het model alleen om aan te scherpen, nooit te vervangen." },
        {
          type: "pair",
          weak: "Schrijf een hartelijke bruiloftstoespraak voor mijn zus.",
          strong: "Ik schrijf een toast voor de bruiloft van mijn zus. Hier zijn mijn rauwe aantekeningen — echte herinneringen, in mijn eigen woorden: [aantekeningen]. Voeg geen verzonnen details of geleende sentimentaliteit toe. Help me een volgorde te vinden die opbouwt, en scherp mijn zinnen aan terwijl je mijn formulering houdt waar die werkt.",
          weakLabel: "Uitbesteed hart",
          strongLabel: "Jouw hart, ondersteund"
        },
        { type: "note", html: "Let op de instructie <em>“voeg geen verzonnen details toe.”</em> Je kunt je waarden direct in prompts coderen: eerlijkheid (\"als je het niet zeker weet, zeg dat\"), credit (\"markeer alles wat op bestaand werk lijkt\"), vriendelijkheid (\"help me dit stevig te zeggen zonder iemand te vernederen\"). Prompting is een plek waar je idealen operationeel mogen zijn, niet alleen gevoeld." },
        { type: "h3", text: "Wanneer niet te prompten" },
        { type: "p", html: "Soms ís schrijven het denken. Dagboekstukken, rouw, de brief die je nooit stuurt — soms ís het stuntelen het punt, en zou gladstrijken wissen waarvoor het was. Een vaardige prompter kent de tool goed genoeg om te weten wanneer die in de la blijft. Dat onderscheidingsvermogen hoort bij het ambacht, het is geen falen ervan." },
        {
          type: "quiz",
          question: "Welke praktijk beschermt jouw stem het best terwijl je AI nog zinvol gebruikt?",
          options: [
            { text: "Gebruik AI alleen voor dingen waar je niet om geeft.", correct: false, feedback: "Dat werkt, maar laat de echte waarde van de tool liggen. Je kunt samenwerken aan wat je dierbaar is — met de juiste taakverdeling." },
            { text: "Draft de emotionele kern zelf, en vraag het model om te structureren, inkorten en vragen te stellen — nooit om je gevoelens te verzinnen.", correct: true, feedback: "Ja. Steigers van het model, draagende zinnen van jou. Selectie en intentie blijven van jou." },
            { text: "Laat het model alles draften, en verander dan een paar woorden zodat het als jou voelt.", correct: false, feedback: "Dat is de uncanny-valley-zone — het leest als bijna-jij, wat vaak erger voelt dan beide uitersten." }
          ]
        },
        { type: "exercise", label: "Een afsluitende reflectie", html: "Schrijf jezelf korte \"samenwerkingsvoorwaarden\" — twee of drie regels over wat je wel en niet aan een model overhandigt, zodat toekomstige-jij een kompas heeft.", placeholder: "Ik laat AI me helpen met...\nIk houd voor mezelf...\nIk zal altijd..." }
      ]
    }
  ]
};
