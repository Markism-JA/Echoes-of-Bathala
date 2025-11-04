# Reference Guidelines

## Main `BibLaTeX` Syntax

`@book`

```bibtex
@book{knuth1984,
  author    = {Knuth, Donald E.},
  year      = {1984},
  title     = {The TeXbook},
  publisher = {Addison-Wesley},
  address   = {Reading, MA}
}
```

`@article`

```bibtex
@article{einstein1905,
  author  = {Einstein, Albert},
  year    = {1905},
  title   = {On the electrodynamics of moving bodies},
  journal = {Annalen der Physik},
  volume  = {17},
  pages   = {891--921}
}
```

`@incollection` chapter in edited book.

```bibtex
@incollection{turing1937,
  author    = {Turing, Alan M.},
  year      = {1937},
  title     = {Computable numbers},
  booktitle = {Computing Machinery and Intelligence},
  editor    = {Smith, John},
  publisher = {Oxford University Press},
  pages     = {23--42}
}
```

`@inproceedings` chapter in conference paper.

```bibtex
@inproceedings{lecun1998,
  author    = {LeCun, Yann and Bottou, Léon and Bengio, Yoshua and Haffner, Patrick},
  year      = {1998},
  title     = {Gradient-based learning applied to document recognition},
  booktitle = {Proceedings of the IEEE},
  volume    = {86},
  pages     = {2278--2324}
}
```

`@thesis`(general)/`@phdthesis`/`@mastersthesis`.

```bibtex
@phdthesis{smith2020,
  author = {Smith, John},
  title  = {Deep learning for finance},
  school = {Massachusetts Institute of Technology},
  year   = {2020}
}
```

`@manual` Technical Documentation

```bibtex
@manual{pythonDoc,
  organization = {Python Software Foundation},
  year         = {2024},
  title        = {Python documentation}
}
```

`@report` Gov/Tech Report, APA calls these Reports.

```bibtex
@report{nasa2023,
  author      = {Johnson, Marie},
  title       = {Mars exploration update},
  institution = {NASA},
  year        = {2023},
  number      = {NASA-TR-2023-01}
}
```

`@online` (Website)

```bibtex
@report{nasa2023,
  author      = {Johnson, Marie},
  title       = {Mars exploration update},
  institution = {NASA},
  year        = {2023},
  number      = {NASA-TR-2023-01}
}
```

`@misc` fallback/thing with no type

```bibtex
@misc{openai2025,
  author = {OpenAI},
  year   = {2025},
  title  = {ChatGPT model documentation},
  url    = {https://platform.openai.com/docs}
}
```

## Other Valid Entry Types

| Entry Type      | What it's for                                 |
| --------------- | --------------------------------------------- |
| `@booklet`      | Printed doc with no publisher                 |
| `@collection`   | Whole edited volume                           |
| `@proceedings`  | Whole conference proceedings                  |
| `@manual`       | Technical manuals                             |
| `@software`     | Software citation (BibLaTeX extension)        |
| `@patent`       | Patents                                       |
| `@dataset`      | Research dataset                              |
| `@unpublished`  | Preprints, manuscripts not formally published |
| `@periodical`   | Entire journal/magazine (not an article)      |
| `@online`       | Web pages (APA-friendly)                      |
| `@video`        | Videos (YouTube etc.) BibLaTeX media          |
| `@audio`        | Audio / podcast                               |
| `@legislation`  | Laws / statutes                               |
| `@jurisdiction` | Courts / case law                             |
| `@report`       | Technical/government reports                  |

## Media Example

`@video`

```bibtex
@video{veritasium2023,
  author = {Veritasium},
  year   = {2023},
  title  = {Why AI will change humanity},
  url    = {https://youtu.be/xxxxx}
}
```

`@audio`

```bibtex
@audio{rogersPodcast2024,
  author = {Rogers, Emma},
  year   = {2024},
  title  = {AI futures podcast},
  url    = {https://spotify.com/...}
}
```

`@dataset`

```bibtex
@dataset{mnist1998,
  author = {LeCun, Yann},
  title  = {MNIST handwritten digits dataset},
  year   = {1998},
  url    = {http://yann.lecun.com/exdb/mnist/}
}
```

## Rules to Remember in APA 7

| Rule | Correction and Clarification |
| :--- | :--- |
| APA uses year, not `date`, unless specific month/day needed. Use `@online` for websites when possible. | Use the `year` field for most entries. Use **`date`** (or `year` + `month` + `day`) only for sources requiring the full date (e.g., news). Use `@online` entry type for web pages (BibLaTeX standard). |
| Useful names if available — Pandoc formats initials | Enter names in the format: `Lastname, Firstname and Lastname, Firstname` in the `author` field. Pandoc (via CSL) handles initials and conjunctions. |
| Always include access date for unstable URLs | Use the `urldate` field (BibLaTeX) or include the access date in a `note` field (classic BibTeX) to fulfill the APA retrieval date requirement. |
| Avoid ALL CAPS (APA sentence case will be applied by CSL) | Use sentence case for titles. Protect proper nouns and acronyms by enclosing them in curly braces (`{}`) (e.g., `{United Nations}`). |

## Guidelines for Selecting Valid and Credible References

### Reference Priority Hierarchy

| Priority | Source Type | Notes |
|--------|-------------|------|
| Highest | Peer-reviewed journals and research papers (ACM, IEEE, Springer, Elsevier) on blockchain, cryptography, tokenomics, game design, virtual economies | Primary academic support |
| High | Reputable academic books and textbooks on software engineering, economics, game design, distributed systems | Methodology and theoretical grounding |
| Strong | University-published theses on blockchain, game development, virtual economies | Secondary academic support |
| Good | Industry whitepapers (e.g., Ethereum Yellow Paper, Solana Docs, Polygon), official game engine docs, credible technical reports | Technical implementation and standards |
| Acceptable | Market analysis from known firms (Deloitte, Gartner, McKinsey), GDC talks, developer blogs from Unreal/Unity/Godot | Use to support real-world relevance or trends |
| Limited | News articles, crypto-trend websites, forum posts | Only for contextual or contemporary examples — not theory or conclusions |

### What a Valid Reference Should Demonstrate

A valid source should at least meet one of these:

- Provides peer-reviewed theory or empirical results
- Demonstrates recognized industry standards or frameworks
- Explains software engineering, networking, or distributed system principles
- Describes proven game economy models or virtual world design
- Discusses cryptocurrency mechanisms, tokenomics, and blockchain systems
- Has clear authorship, date, and institutional credibility

---

## When Can Old Sources Be Used?

### Acceptable Use of Older Sources

Older sources (5–15+ years old) may be used if they are:

- **Foundational works**
  Example: Nakamoto’s Bitcoin Whitepaper (2008), early virtual economy studies
- Seminal publications in software engineering, game theory, cryptography, or economics
- Original creators of theories, models, or algorithms
- Still cited and respected in modern literature

Examples of foundational valid sources:

- Bitcoin Whitepaper (Nakamoto, 2008)
- Proof-of-Work, Byzantine Fault Tolerance papers
- Richard Bartle’s virtual worlds theory (MMORPG foundations)
- Classic SE texts (Pressman, Sommerville)

### When Old Sources Should Be Avoided

Avoid old references if they:

- Present outdated information on fast-moving fields (e.g., blockchain protocols)
- Refer to obsolete technology or tools
- Are replaced by new standards or updated research

### How to Judge If an Old Source Is Still Valid

Ask:

1. Is this source still cited in recent research?
2. Is it a foundational concept or original model?
3. Has the field moved past this idea?
4. Is the technology or method still used today?
5. Is there a more updated authoritative source available?

If **yes** to (1–2), typically valid.
If **no** to (3–5), avoid or replace with newer research.
