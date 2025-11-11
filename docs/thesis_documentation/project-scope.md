# Project Scope & Definition of Done

This document defines the three-tiered criteria for project completion, allowing for adjustments based on time, resources, and technical constraints. The project's success will be measured against one of these three defined tiers. We can only define which of these 3 after the implementation phase based on the time and other resource available to us during that time. This document is constructed directly against the [Scope](01_Introduction/06-SCOPE_AND_LIMITATION_OF_THE_STUDY.md), [SOP](01_Introduction/03-STATEMENT-OF-THE-PROBLEM.md) and [Objective](01_Introduction/05-OBJECTIVES_OF_THE_STUDY.md).

> [!NOTE] Before undergoing any testing, we are essentially imposing a **feature freeze** on development. Our job here is test administrator and researcher not developers. Only bug fixes are allowed from here on out.

## Tier 1: Baseline (This answers whether the product was "technically feasible")

This tier represents the minimum viable product required to pass. The goal is to proved that the core technical components of the proposed system are functional.

- Goal: Demonstrate the technical feasibility of the prototype.
- Timeframe: ~1 week (Internal testing).
- Testers: Internal team, project advisor, maybe classmates and friends?.

Acceptance Criteria:

1. RQ 5 (Systems Architecture): This is the primary criterion for Tier 1. The end-to-end data pipeline from the game server to the blockchain is functional.

- The Game Server (centralized) can securely and verifiably communicate a player's earned Perlas to the Blockchain API Gateway.
- The API Gateway can successfully trigger the "Divine Tithe" Smart Contract.
- The test demonstrates that this communication is cheat-resistant (i.e., a simple client-side hack cannot grant unearned Perlas for conversion).

2. RQ 4 (Blockchain): The core blockchain components are functional.

- The Baku Coin (ERC20) and "Divine Tithe" smart contracts are successfully deployed and verifiable on a testnet.
- The smart contracts execute their logic (e.g., convert()) without errors.

3. RQ 1 (Economy): The features of the Stabilization Framework are implemented.

- A player can earn Perlas (faucet).

- A player can successfully execute the on-chain conversion (sink) via the "Divine Tithe" mechanism.

4. RQ 2 (Social): The features of the Social Sink are implemented.

- Core social systems (e.g., in-game chat, guild creation stub, player visibility) are functional.

5. RQ 3 (Balancing): This question is out of scope for Tier 1. No data is gathered.

6. System: Players can successfully log in, create a character, and move in the game world.

Thesis Conclusion (If only this tier is met): "This study successfully designed and prototyped a hybrid game architecture. The prototype's technical feasibility was validated in an internal test, confirming that the centralized 3-tier game architecture can securely and functionally integrate with the decentralized on-chain token infrastructure."

## Tier 2: Medium (Initial Validation)

This tier represents the target goal for this capstone. The goal is to not only "does it work?" but also gather preliminary data to support the core thesis.

- Goal: Gather initial supporting data for the economic and social hypotheses.

- Timeframe: 1-2 week "Closed Beta" event.

- Testers: 20-50 targeted "external" players (e.g., recruited from classmates and so on.).

Acceptance Criteria:

1. All Tier 1 criteria are met.

2. RQ 1 (Economy): Economic data (e.g., Perlas supply, conversion rates, treasury balance) is successfully tracked and logged over the entire test period. This data is sufficient to create basic charts for the thesis, allowing for a preliminary analysis of the framework's effectiveness.

3. RQ 2 (Social): Qualitative social data is collected via a mandatory entry survey (motivations) and exit survey (feedback, engagement). Discord/in-game chat logs are analyzed for themes related to community and lore.

4. RQ 3 (Balancing): Initial qualitative data is gathered via the exit survey (e.g., "Was the conversion 'worth it'? Was the game fun?"). This allows for a preliminary discussion of the balance between the game loop and the economy.

5. RQ 5 (Systems Architecture): The integrated architecture is proven to be stable and functional under the load of real, external players.

Thesis Conclusion (If this tier is met): "The framework was tested in a 2-week closed beta with 40 participants. Initial data suggests the adaptive throttle stabilized the conversion rate (see Fig 4.1). Furthermore, exit surveys provide preliminary support for the 'social sink' hypothesis, with 65% of players citing 'community' or 'lore' as a key motivator."

## Tier 3: Optimal (Empirical Validation through Experiment)

This tier represents the ideal research outcome, equivalent to a graduate-level study. It would require significant time and resources and would provide rigorous, statistically powerful evidence.

- Goal: Rigorously validate the economic and social theories with comparative data.
- Timeframe: 4+ week controlled test.
- Testers: 100+ players, ideally split into two groups (A/B testing).

Acceptance Criteria:

1. All Tier 2 criteria are met.

2. A/B Test Execution: The project team successfully launches and manages two separate, parallel game servers:

    - Group A (Control): Runs the game without the Stabilization Framework (using a simple, fixed-rate conversion).
    - Group B (Test): Runs the game with the full Stabilization Framework.

3. RQ 1 (Economy): Comparative economic data is gathered. The thesis can quantitatively compare the inflation, volatility, and stability of Group A vs. Group B.

4. RQ 2 (Social): Comparative retention data is gathered. The thesis can analyze if players in Group B (stable economy) have a higher retention rate and report higher intrinsic motivation than Group A.

5. RQ 3 (Balancing): Quantitative and qualitative data (from surveys and server analytics) allows for a deep analysis of the balance between the economic and social systems in both group

Thesis Conclusion (If this tier is met): "In a 4-week controlled study, the test group (N=50) utilizing the Stabilization Framework demonstrated a 95% reduction in currency inflation compared to the control group (N=50). This provides strong empirical evidence that the proposed model is a viable solution... Concurrently, player retention data validates the 'social sink' as a key driver of long-term intrinsic motivation."
