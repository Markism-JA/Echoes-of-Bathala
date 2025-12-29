# ADR 0001 — Account and Multi-Character Feature

**Date:** 2025-12-30
**Status:** To be discussed
**Context:**

We should support multiple characters, but manage it with the following rules:

1. Shared Global Inventory (The "Stash"): Give the Account a storage vault. This is where NFTs/Tokens live.
2. Character Inventory (The "Bag"): This is local to the character.
3. Account-Level Cooldowns: Daily rewards, dungeon lockouts, and energy systems should often be tracked by Account ID to prevent farming.
4. Single Active Session: Strictly enforce that only one character from an Account can be online at once.
