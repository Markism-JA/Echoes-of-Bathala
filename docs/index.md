# Project Documentation Guide

## Conventions

- This repo is the official doc base.
- **Format:** All documentation is in Markdown (`.md`) format.
- **Pull Requests:** Required for every update.

## Internal Documentation

- **Branch:** Use a `docs/<topic>` branch for internal documentation changes.
- **File Naming:**
  - Use UPPERCASE + underscores for major documents (e.g., `GDD.md`).
  - Use lowercase + hyphen for minor documents.

## Thesis Documentation

- **Branch**: Use a `thesis/<topic>` branch for thesis documentation changes.
- The rendering of the Markdown files in the `thesis/*` is handled via latex.
- Use `bolding` and `italic` sparingly. In example don't bold or italicized every word for emphasis.

  - Don't bold headings:

    ```markdown
    # **H1**
    ```

  - Always check for any extra spaces added by chatbots when generating markdown.

    ```markdown
    # Example
    lorenipsum"  " <- Extra spacing after.
    ```

    > [!NOTE]
    > I've noticed this in the recent thesis documentation PR added. Please fix this. This is also an issue in internal documentation. As we change the general structure of the internal docs remediate this.

### References

- Because we are using LaTex (the markdown files compiles to this format via `pandoc`) citations follows a specific syntax.

| Type             | Syntax             | Meaning                   |
| ---------------- | ------------------ | ------------------------- |
| Parenthetical    | `[@key]`           | (Author, Year)            |
| Narrative        | `@key`             | Author (Year)             |
| Multiple sources | `[@key1; @key2]`   | Multiple citations        |
| Page number      | `[@key, p. 42]`    | Page numbers              |
| Prefix           | `[See @key]`       | See Author (Year)         |
| Suffix           | `[@key, p. 42–45]` | (Author, Year, pp. 42–45) |

- This manner help builds internal linking for the citations in the Bibliography section of the paper.
- It also standardized our writing.

**Example:**

```markdown

As shown in @einstein1905...

Theory has evolved significantly [@knuth1984; @turing1936].

According to @smith2020, analysis shows...
```

- For every references, an entry in the `references.bib` must be created. There are different types of references entry in the .bib file. When referencing you call the key, which is the first field in a bibliography entry.

Example: [References](thesis_documentation/references-guidelines.md#main-biblatex-syntax)

> Google Drive copies (for submission) are generated automatically by the build system. The lead dev would be responsible for the CD pipeline for this [Not yet implemented as CD pipeline, still local (Lead Dev's Machine)].

## Contribution Workflow

1. **Create or edit** Markdown docs locally or via GitHub’s editor. Please learn to do this locally.
2. **Commit** to your `docs/<topic>` branch.
3. **Open a Pull Request** to `dev` for review.
4. After approval, the documentation builds actions will run automatically (Publishing to website and outputting to drive [Not yet implemented]).

Example branch:

```bash
git branch docs/update-gdd-combat-system
```

## Notes for Members

- **Update technical docs when systems change**.
- Keep documentation changes **in sync with code**.
- For further information about team standard and workflow consult [Team Guidelines](team/workflow.md).

> _Maintained by the Lead Developer. Updated as team conventions evolve._
