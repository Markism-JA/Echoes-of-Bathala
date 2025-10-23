# ADR 0001 — Use C# for Backend Development
**Date:** 2025-10-12
**Status:** Accepted
**Context:**
We needed to choose a backend stack for the MMORPG. Options included Node.js, Go, and C# (ASP.NET).

**Decision:**
Use C# and ASP.NET Core for backend APIs due to existing team skill set and strong .NET ecosystem for API work.

**Consequences:**
- Easier onboarding for current members
- Strong integration with Windows/Linux environments
- Slightly heavier runtime than Go
- Fewer lightweight game server frameworks in .NET compared to Node.js
