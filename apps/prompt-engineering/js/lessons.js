/*
 * Practice Garden heuristics for Lumen.
 * Labels/hints and lesson copy live in js/i18n/{en,nl}.js.
 * Tests accept EN and NL cue words so the garden works in either locale.
 */

const GARDEN_CHECK_DEFS = [
  {
    id: "length",
    test: (text) => text.trim().split(/\s+/).filter(Boolean).length >= 15
  },
  {
    id: "context",
    test: (text) =>
      /\b(i'?m|i am|my|we are|we'?re|this is for|because|context|situation|background|for a|for my|ik ben|ik'|mijn|wij zijn|we zijn|dit is voor|omdat|context|situatie|achtergrond|voor een|voor mijn)\b/i.test(
        text
      )
  },
  {
    id: "role",
    test: (text) =>
      /\b(you are|you'?re a|act as|as a|imagine you|take the role|speaking as|persona of|jij bent|je bent|doe alsof|als een|stel je voor|neem de rol|spreek als)\b/i.test(
        text
      )
  },
  {
    id: "tone",
    test: (text) =>
      /\b(tone|warm|gentle|wry|playful|formal|casual|earnest|bittersweet|hopeful|calm|kind|honest|poetic|spare|quiet|soft|serious|light-?hearted|whimsical|sincere|friendly|professional|toon|zacht|droog|speels|formeel|informeel|oprecht|bitterszoet|hoopvol|kalm|eerlijk|poëtisch|stil|serieus|grillig|vriendelijk|professioneel|spaarzaam)\b/i.test(
        text
      )
  },
  {
    id: "shape",
    test: (text) =>
      /\b(list|bullet|numbered|table|steps?|outline|paragraphs?|sentences?|words?|options?|versions?|examples?|format|sections?|headings?|short|brief|\d+|lijst|opsomming|genummerd|tabel|stappen?|outline|alinea'?s?|zinnen?|woorden?|opties?|versies?|voorbeelden?|formaat|secties?|kopjes?|kort|bondig)\b/i.test(
        text
      )
  },
  {
    id: "boundaries",
    test: (text) =>
      /\b(don'?t|do not|avoid|no |without|never|keep|preserve|exclude|skip|not too|instead of|rather than|niet|vermijd|geen |zonder|nooit|houd|bewaar|sluit uit|sla over|niet te|in plaats van)\b/i.test(
        text
      )
  },
  {
    id: "ask",
    test: (text) =>
      /\b(write|rewrite|draft|list|summari[sz]e|compare|explain|suggest|help me|create|generate|revise|edit|brainstorm|outline|translate|describe|critique|review|analy[sz]e|plan|break down|give me|show me|find|sort|tighten|map|schrijf|herschrijf|draft|lijst|vat samen|samenvatten|vergelijk|leg uit|stel voor|help me|maak|genereer|herzie|bewerk|brainstorm|outline|vertaal|beschrijf|bekritiseer|beoordeel|analyseer|plan|breek|geef me|laat me zien|vind|sorteer|scherp|karteer)\b/i.test(
        text
      )
  }
];
