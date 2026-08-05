window.I18N = window.I18N || {};
window.I18N.en = {
  app: {
    title: "Lumen — Prompt Engineering for INFPs",
    brandTag: "prompt craft for dreamers"
  },
  nav: {
    home: "Home",
    path: "The Path",
    garden: "Practice Garden",
    journal: "Journal"
  },
  progress: {
    title: "Your journey so far"
  },
  locale: {
    ariaLabel: "Language"
  },
  footer: {
    note: "Made softly, for the ones who feel first and speak second. Your progress lives only in this browser."
  },
  home: {
    eyebrow: "A course for INFPs",
    headline: "Prompt engineering,",
    headlineAccent: "for the ones who feel first",
    lede: "You already imagine your listener, hear tone in a single word, and carry whole worlds in your head. Those aren't obstacles to working with AI — they're the exact skills prompting is made of. This little course shows you how.",
    ctaBegin: "Begin the first lesson",
    ctaGarden: "Wander into the garden",
    whyTitle: "Why a course just for INFPs?",
    whyBody: "Most prompt-engineering guides are written like engineering manuals — optimize, iterate, extract. Useful, but they skip the questions that actually stop sensitive, idealistic people: <em>Will this flatten my voice? Is it still mine? Why does the output feel emotionally wrong?</em> Lumen teaches the same core techniques — context, roles, examples, step-by-step reasoning — through metaphors that fit how you already think, and takes the values questions seriously instead of waving them away.",
    features: [
      { icon: "🌙", title: "Learn by feeling", body: "Six short lessons that teach real prompt-engineering techniques through the way you already think — imagined listeners, inner characters, named feelings." },
      { icon: "🌿", title: "A garden, not a grader", body: "Practice writing prompts in a space that lights lanterns for what's working instead of red-marking what isn't." },
      { icon: "📖", title: "Reflect as you go", body: "Every lesson ends with a gentle reflection, saved to a private journal that never leaves your browser." }
    ]
  },
  path: {
    title: "The Path",
    lede: "Six lessons, each a short walk. Take them in order or wander — the path will remember where you've been.",
    walked: "walked",
    unwalked: "unwalked"
  },
  lesson: {
    kicker: "Lesson {n} of {total}",
    noteLabel: "For the INFP in you",
    quizTitle: "A gentle check",
    saveReflection: "Keep this in my journal",
    writeFirst: "Write a little something first — even a fragment counts.",
    kept: "Kept. You'll find it in your journal. 🌸",
    reflectionFallback: "Reflection",
    markWalked: "Mark this lesson walked",
    unmarkWalked: "✓ Walked — tap to unmark",
    backToPath: "← The Path",
    toGarden: "To the garden →",
    defaultPlaceholder: "Write freely..."
  },
  garden: {
    title: "The Practice Garden",
    lede: "Write a prompt below and watch the lanterns light. Nothing is graded here — each lamp is just one craft element your prompt already carries. Pick a seed if you'd like a scenario to practice with.",
    placeholder: "Dear model... (write the prompt you would actually send)",
    scenarioPrefix: "Scenario:",
    litSuffix: " — lit."
  },
  journal: {
    title: "Your Journal",
    lede: "Reflections you chose to keep, newest first. They live only in this browser — no account, no cloud, no one reading over your shoulder.",
    empty: "Nothing here yet. The reflections at the end of each lesson will gather here, like pressed flowers.",
    letGo: "let it go"
  },
  notFound: {
    title: "A quiet clearing",
    body: "This page doesn't exist — but you found a peaceful spot anyway.",
    back: "Back to the beginning"
  },
  pair: {
    before: "Before",
    after: "After"
  },
  gardenChecks: {
    length: {
      label: "Enough words to set a scene",
      hint: "A single line rarely carries enough context. Add a sentence about the situation."
    },
    context: {
      label: "Context or backstory (\"I'm…\", \"this is for…\", \"because…\")",
      hint: "Tell the model who this is for or why it matters — the way you'd brief a friend."
    },
    role: {
      label: "A cast character or perspective (\"you are…\", \"act as…\")",
      hint: "Try casting someone: \"You are a patient editor who…\" — choose who shows up."
    },
    tone: {
      label: "A named tone or feeling",
      hint: "Name the feeling you're after: warm, wry, spare, gentle, earnest — and what to avoid."
    },
    shape: {
      label: "A requested shape or format",
      hint: "Ask for a shape: \"three options,\" \"under 100 words,\" \"a numbered list,\" \"a table.\""
    },
    boundaries: {
      label: "Boundaries — what to avoid or keep",
      hint: "Add a boundary: \"no clichés,\" \"don't invent details,\" \"keep my phrasing where it works.\""
    },
    ask: {
      label: "A clear ask (a verb doing real work)",
      hint: "Give the model a job with a verb: write, rewrite, list, compare, question, summarize…"
    }
  },
  gardenSeeds: [
    { label: "🌷 A kind rejection letter", text: "I need to tell a friend I can't join her book club without hurting her feelings." },
    { label: "🍄 Feedback on a story", text: "I want feedback on a short story I'm nervous about." },
    { label: "🌊 An overwhelming week", text: "My week is overloaded and I don't know where to start." },
    { label: "🕊️ A difficult conversation", text: "I need to ask my manager for a lighter workload." },
    { label: "✨ A bio that sounds like me", text: "I need a professional bio that doesn't sound corporate." }
  ],
  glowStages: [
    { min: 0, label: "A held breath — the garden waits for your words." },
    { min: 1, label: "A single firefly blinks awake." },
    { min: 2, label: "Two lanterns lit. The path is appearing." },
    { min: 3, label: "A soft glow gathers between the trees." },
    { min: 4, label: "The garden is warmly lit — this prompt would land well." },
    { min: 5, label: "Lantern light everywhere. A genuinely lovely prompt." },
    { min: 6, label: "The whole garden glows — a prompt with craft in its bones." },
    { min: 7, label: "Full radiance. You could teach this course." }
  ],
  lessons: [
    {
      id: "listening",
      icon: "🌙",
      title: "Talking to a Very Literal Daydream",
      subtitle: "What a language model actually is, and why your inner world is an advantage.",
      blocks: [
        { type: "p", html: "A language model is not a search engine and not a mind reader. It is closer to an <em>improv partner with an enormous memory and no context about you</em>. It will pick up whatever scene you set — and if you set no scene, it will improvise something generic." },
        { type: "p", html: "Prompt engineering is simply the craft of setting that scene well: giving the model the context, intention, and shape it needs to respond the way you hoped. That's it. No math, no code — just deliberate, thoughtful language." },
        { type: "note", html: "You already rehearse conversations in your head, imagine how words will land, and notice subtle differences in tone. That habit of <em>imagining the listener</em> is the single most important prompting skill. Most people have to learn it. You were born doing it." },
        { type: "h3", text: "The one mental shift" },
        { type: "p", html: "Before you write a prompt, pause and ask: <em>“If a thoughtful stranger read only these words — nothing else — what would they know about what I want?”</em> The model knows nothing you didn't say. Your mood, your project, the tabs you have open — invisible, unless you put them into words." },
        {
          type: "pair",
          weak: "Write something about creativity.",
          strong: "I'm writing a short talk for art-school graduates who feel paralyzed by the blank page. Write three opening lines for the talk — warm, a little wry, no clichés about \"thinking outside the box.\"",
          weakLabel: "Vague wish",
          strongLabel: "A scene the model can step into"
        },
        { type: "p", html: "Notice what the second prompt added: <em>who it's for, what it's for, what tone feels right, and what to avoid</em>. Nothing technical — just the kind of context you'd naturally give a friend who offered to help." },
        {
          type: "quiz",
          question: "Why did the second prompt work better?",
          options: [
            { text: "It used more impressive vocabulary.", correct: false, feedback: "Not quite — fancy words don't help. It's the context that matters, not the polish." },
            { text: "It gave the model an audience, a purpose, and a tone to aim for.", correct: true, feedback: "Yes. The model can only work with what's on the page — the second prompt paints the whole scene." },
            { text: "It was longer, and longer prompts are always better.", correct: false, feedback: "Length alone isn't the point — a long, rambling prompt can be worse than a short, clear one. It's the relevant detail that counts." }
          ]
        },
        { type: "exercise", label: "A moment of reflection", html: "Think of the last time you asked an AI (or a person!) for something and felt quietly disappointed by the answer. What did you know in your head that never made it into your words?", placeholder: "What I knew but didn't say was..." }
      ]
    },
    {
      id: "intention",
      icon: "🕯️",
      title: "Naming the Feeling You're After",
      subtitle: "Turning your (very real) sense of \"not quite right\" into words a model can follow.",
      blocks: [
        { type: "p", html: "INFPs often know <em>exactly</em> how they want something to feel long before they can say what it should contain. That intuition is precious — but a model can't feel your feeling. It can, however, follow a description of it remarkably well." },
        { type: "h3", text: "Give the feeling a vocabulary" },
        { type: "p", html: "Instead of hoping the model \"gets it,\" describe the target feeling with three kinds of words: <em>tone words</em> (gentle, wry, spare, earnest), <em>anti-tone words</em> (not corporate, not chirpy, no exclamation marks), and <em>touchstones</em> (\"like a letter from a wise friend,\" \"in the spirit of Mary Oliver's essays\")." },
        {
          type: "pair",
          weak: "Make this email sound better.",
          strong: "Rewrite this email so it sounds warm but professional — like a considerate colleague, not a marketing bot. Keep my apology sincere, cut the rambling middle paragraph, and end on a hopeful note.\n\n[paste email]",
          weakLabel: "Hoping it \"gets it\"",
          strongLabel: "The feeling, named"
        },
        { type: "note", html: "Your sensitivity to tone is a superpower here. Most people can't tell you the difference between \"warm\" and \"chirpy\" — you can. Write those distinctions down and the model will honor them." },
        { type: "h3", text: "The \"not quite right\" loop" },
        { type: "p", html: "First drafts from a model are rarely right, and that's fine — prompting is a conversation, not a slot machine. When a response misses, resist the urge to start over. Instead, name the miss: <em>“Closer, but this feels too formal — loosen it, and keep the second paragraph exactly as is.”</em> Each correction teaches the model your taste." },
        {
          type: "quiz",
          question: "The model's draft feels too cheerful for the mood you wanted. What's the strongest next move?",
          options: [
            { text: "Delete everything and try a completely different prompt.", correct: false, feedback: "You'd lose everything the model already got right. Refining beats restarting almost every time." },
            { text: "Reply: \"Tone it down — quieter, more bittersweet. Keep the imagery in the last line.\"", correct: true, feedback: "Exactly. You named what missed, gave a direction, and protected what was already working." },
            { text: "Accept it — models can't really do subtle tones.", correct: false, feedback: "They genuinely can — but only when someone with your ear for tone tells them where to aim." }
          ]
        },
        { type: "exercise", label: "A moment of reflection", html: "Pick something you'd love an AI's help with — a letter, a story, a difficult message. Don't write the prompt yet. Just describe, in your own words, how the finished thing should <em>feel</em>.", placeholder: "It should feel like..." }
      ]
    },
    {
      id: "roles",
      icon: "🎭",
      title: "Casting Characters",
      subtitle: "Role prompting: inviting the model to speak as someone in particular.",
      blocks: [
        { type: "p", html: "A model contains multitudes — editors, mentors, skeptics, poets, patient explainers. By default you get a polite average of all of them. <em>Role prompting</em> means choosing who shows up: “You are a developmental editor who specializes in first novels…”" },
        { type: "p", html: "For someone with a rich inner cast of characters, this is the most natural technique in the world. You're not tricking the model — you're doing what you do when you imagine \"what would my wisest friend say about this?\"" },
        { type: "h3", text: "What a good role includes" },
        { type: "p", html: "The strongest roles have three parts: <em>expertise</em> (what they know), <em>disposition</em> (how they treat you), and <em>a job to do right now</em>. Disposition matters more than people think — it's the difference between feedback that stings and feedback you can actually hear." },
        {
          type: "pair",
          weak: "Give me feedback on my poem.",
          strong: "You are a poetry mentor who is honest but never cruel — you always find what's alive in a draft before touching what isn't working. Read my poem and tell me: (1) the strongest line and why, (2) one place where the emotion goes fuzzy, (3) one question to sit with before revising.\n\n[poem]",
          weakLabel: "Anonymous critic",
          strongLabel: "A mentor you cast yourself"
        },
        { type: "note", html: "If harsh feedback shuts you down, this isn't a weakness to work around — it's a design constraint to honor. You are allowed to cast a kind reader. Kind and honest are not opposites, and you'll revise more when the feedback is survivable." },
        { type: "h3", text: "Roles for thinking, not just writing" },
        { type: "p", html: "Cast a <em>gentle devil's advocate</em> before a big decision. A <em>future you, five years on</em>, when you're stuck. A <em>translator</em> who turns your heartfelt rambling into two crisp sentences for a busy boss. Every imagined conversation you've ever had is now a prompt template." },
        {
          type: "quiz",
          question: "Which role prompt is most likely to get useful, hearable feedback on a personal essay?",
          options: [
            { text: "\"You are the world's harshest critic. Destroy this essay.\"", correct: false, feedback: "You'd get theatrical cruelty, not insight — and you'd dread reading it. Harsh isn't the same as rigorous." },
            { text: "\"You are an essayist and editor. Point out what's working, then the one structural change that would help most, explained kindly.\"", correct: true, feedback: "Yes — expertise, disposition, and a specific job. Honest, useful, and survivable." },
            { text: "\"You are my best friend. Tell me this is good.\"", correct: false, feedback: "Comfort is lovely, but pure reassurance won't help the essay grow. You can ask for kindness and honesty." }
          ]
        },
        { type: "exercise", label: "A moment of reflection", html: "Who are the imagined voices you already consult — the inner mentor, the skeptic, the comforter? Sketch one of them as a role prompt: their expertise, their disposition, and the job you'd give them.", placeholder: "You are..." }
      ]
    },
    {
      id: "shape",
      icon: "🗺️",
      title: "Giving Shape to the Mist",
      subtitle: "Structure, format, and examples — gentle containers for open-ended ideas.",
      blocks: [
        { type: "p", html: "INFPs sometimes resist structure because it feels like a cage. But in prompting, structure isn't a cage — it's a <em>vase</em>. It doesn't change what the flowers are; it just keeps them from ending up all over the floor." },
        { type: "h3", text: "Ask for the shape you can actually use" },
        { type: "p", html: "Tell the model what the output should physically look like: <em>“three options, one sentence each”</em> · <em>“a table with columns for idea / first step / what could go wrong”</em> · <em>“under 150 words”</em> · <em>“as a numbered plan I can follow on a low-energy day.”</em> Shape requests are the easiest wins in all of prompting." },
        {
          type: "pair",
          weak: "Help me plan my week, I'm overwhelmed.",
          strong: "I'm overwhelmed and my energy comes in waves. Here's everything on my plate: [list]. Sort it into three groups — \"must happen this week,\" \"would be kind to future me,\" and \"honestly can wait.\" Then suggest a plan for tomorrow only, with no more than three items.",
          weakLabel: "Mist",
          strongLabel: "Mist, given a vase"
        },
        { type: "h3", text: "Show, don't only tell: examples" },
        { type: "p", html: "The fastest way to convey a style you can feel but can't fully describe is to <em>show a sample</em>. This is called few-shot prompting: “Here are two bios I love the tone of: [examples]. Write mine in the same spirit.” One good example outweighs a paragraph of adjectives." },
        { type: "note", html: "You probably keep a private museum of things you love — lines from books, other people's bios, that one email that made you feel seen. That collection is now a toolkit. Feeding the model an example you love is the closest thing prompting has to sharing your taste directly." },
        {
          type: "quiz",
          question: "You want product descriptions in a very specific whimsical style you can't quite articulate. What works best?",
          options: [
            { text: "Stack up adjectives: \"whimsical, quirky, magical, delightful, playful.\"", correct: false, feedback: "Adjective piles blur together — the model's \"whimsical\" may not be yours. Show it instead." },
            { text: "Paste two or three descriptions that nail the style and say \"match this voice.\"", correct: true, feedback: "Exactly — few-shot examples transmit taste directly, no translation loss." },
            { text: "Ask the model to define whimsy first, then write.", correct: false, feedback: "A definition is still telling. An example is showing — and showing wins." }
          ]
        },
        { type: "exercise", label: "A moment of reflection", html: "Find one piece of writing you love the voice of — a bio, a caption, a paragraph. Paste or describe it here, and note what precisely you love about it. This becomes your first few-shot example.", placeholder: "The piece I love is... and what makes it sing is..." }
      ]
    },
    {
      id: "thinking",
      icon: "🌿",
      title: "Thinking Out Loud, Together",
      subtitle: "Step-by-step reasoning, breaking big dreams into askable pieces.",
      blocks: [
        { type: "p", html: "For anything with real reasoning in it — a decision, a plan, an analysis — the magic phrase is some form of <em>“think it through step by step before answering.”</em> Models, like people, do better when they show their work instead of blurting a conclusion." },
        {
          type: "pair",
          weak: "Should I quit my job to freelance?",
          strong: "I'm weighing quitting my stable job to freelance as an illustrator. Think this through step by step: first list what matters most based on what I tell you below, then the realistic risks, then what a gentle middle path might look like. Don't give me a verdict — give me clearer questions.\n\nContext: [your situation]",
          weakLabel: "A coin flip",
          strongLabel: "A thinking partner"
        },
        { type: "p", html: "Notice the ending: <em>“don't give me a verdict — give me clearer questions.”</em> You're allowed to control not just the format but the <em>kind</em> of help. Advice, options, questions, devil's advocacy, a summary of what you seem to be feeling — name what would actually help." },
        { type: "h3", text: "Big dreams, small prompts" },
        { type: "p", html: "INFP projects tend to be vast and luminous — a novel, a career change, a life philosophy. Vast things make bad single prompts. The craft is decomposition: <em>one conversation to map the territory, then one conversation per region.</em>" },
        { type: "p", html: "A lovely pattern: first ask, <em>“Here's my big fuzzy dream: [dream]. Help me break this into 5–7 explorable questions.”</em> Then take those questions one at a time, in separate chats, each with its own context. The dream stays whole in you; the prompts stay small enough to work." },
        { type: "note", html: "Decomposition isn't betraying the dream by shrinking it. It's how you protect the dream from dissolving into overwhelm. The model holds the pieces so you can keep holding the whole." },
        {
          type: "quiz",
          question: "You want help with a novel you've been imagining for years. What's the strongest opening move?",
          options: [
            { text: "\"Write my novel about a lighthouse keeper who collects lost letters.\"", correct: false, feedback: "That hands your dream away whole — and gets back something generic. Keep authorship; ask for help with pieces." },
            { text: "\"Here's my premise and what I want readers to feel. Help me map the 6 biggest unanswered questions about this story, so I can explore them one by one.\"", correct: true, feedback: "Yes — the dream stays yours, and you now have a set of small, workable prompts instead of one impossible one." },
            { text: "\"Give me 100 plot ideas.\"", correct: false, feedback: "Volume isn't clarity — you'd drown in options that ignore the story you already carry." }
          ]
        },
        { type: "exercise", label: "A moment of reflection", html: "Name one big, luminous project you've been carrying. Then break it — gently — into three questions you could actually ask a model this week.", placeholder: "The dream: ...\n\nThree askable questions:\n1.\n2.\n3." }
      ]
    },
    {
      id: "integrity",
      icon: "💛",
      title: "Keeping Your Voice",
      subtitle: "Values, authenticity, and using AI without losing what makes your work yours.",
      blocks: [
        { type: "p", html: "The deepest INFP worry about AI is rarely technical. It's this: <em>if a machine helps me say it, is it still mine?</em> That question deserves a real answer, not a shrug." },
        { type: "h3", text: "A working principle" },
        { type: "p", html: "Authorship lives in <em>selection and intention</em>, not keystrokes. You decide what's worth saying, what feels true, what stays and what goes. A model can widen your options; only you can recognize which option is honest. If you're choosing with care, the work is yours." },
        { type: "p", html: "Practically, this suggests a division of labor: use the model for <em>scaffolding</em> — outlines, alternatives, summaries, \"what am I missing?\" — and keep the <em>load-bearing sentences</em>, the ones that carry your actual heart, in your own hands. Or draft them yourself first, then ask the model only to tighten, never to replace." },
        {
          type: "pair",
          weak: "Write a heartfelt wedding toast for my sister.",
          strong: "I'm writing a toast for my sister's wedding. Here are my raw notes — real memories, in my own words: [notes]. Don't add invented details or borrowed sentiment. Help me find an order that builds, and tighten my sentences while keeping my phrasing wherever it works.",
          weakLabel: "Outsourced heart",
          strongLabel: "Your heart, supported"
        },
        { type: "note", html: "Notice the instruction <em>“don't add invented details.”</em> You can encode your values directly into prompts: honesty (\"if you're not sure, say so\"), credit (\"flag anything that resembles an existing work\"), kindness (\"help me say this firmly without humiliating anyone\"). Prompting is a place where your ideals get to be operational, not just felt." },
        { type: "h3", text: "When not to prompt" },
        { type: "p", html: "Some writing is the thinking. Journal entries, grief, the letter you'll never send — sometimes the fumbling <em>is</em> the point, and smoothing it would erase what it was for. A skilled prompter knows the tool well enough to know when to leave it in the drawer. That discernment is part of the craft, not a failure of it." },
        {
          type: "quiz",
          question: "Which practice best protects your voice while still using AI meaningfully?",
          options: [
            { text: "Only use AI for things you don't care about.", correct: false, feedback: "That works, but it leaves the tool's real value on the table. You can collaborate on things you love — with the right division of labor." },
            { text: "Draft the emotional core yourself, and ask the model to structure, trim, and question — never to invent your feelings.", correct: true, feedback: "Yes. Scaffolding from the model, load-bearing sentences from you. Selection and intention stay yours." },
            { text: "Let the model draft everything, then change a few words so it feels like yours.", correct: false, feedback: "That's the uncanny-valley zone — it will read as almost-you, which often feels worse than either extreme." }
          ]
        },
        { type: "exercise", label: "A closing reflection", html: "Write yourself a short \"terms of collaboration\" — two or three lines about what you will and won't hand to a model, so future-you has a compass.", placeholder: "I will let AI help me with...\nI will keep for myself...\nI will always..." }
      ]
    }
  ]
};
