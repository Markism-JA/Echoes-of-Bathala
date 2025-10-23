# Project Documentation Guide

## Conventions

- **Format:** All documentation is in Markdown (`.md`) format.
- **Branch:** Use a `docs/<topic>` branch for major documentation changes.
- **Pull Requests:** Required for every update to ensure consistency.
- **File Naming:**
  - Use UPPERCASE + underscores for major documents (e.g., `GDD.md`).
  - use lowercase + hypen for minor documents.
- **Single Source of Truth:** This folder, not Google Drive, is the official doc base.

> Google Drive copies (for submission) are generated automatically by the build system.  
  The lead dev would be responsible for the CD pipeline for this.

## Automated Document Builds

Our GitHub Actions pipeline handles:

- Generating `.docx` files from `thesis/*.md`
- Injecting current date/time for consult forms
- Uploading outputs as **Artifacts** (auto-expire after 3 days)

**No generated files are stored in the repo.**

## Contribution Workflow

1. **Create or edit** Markdown docs locally or via GitHub’s editor.
2. **Commit** to your `docs/<topic>` branch.
3. **Open a Pull Request** to `dev` for review.
4. After approval, the doc build workflow will run automatically.

Example branch:

```bash
git branch docs/update-gdd-combat-system
```

## Notes for Members

- If you’re assigned to thesis writing, **never edit `.docx` directly**.  Work in Markdown — the system will handle formatting.
- **Update technical docs when systems change**.
- Keep documentation changes **in sync with code**.

> _Maintained by the Lead Developer. Updated as team conventions evolve._
