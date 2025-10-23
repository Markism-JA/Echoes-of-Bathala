# Token Tax Model Overview

## 1. Transaction Tax Summary

| **Transaction Type** | **Goal** | **Tax Rate** |
|-----------------------|----------|---------------|
| **Buy** | Encourage players and investors to buy — lower entry tax promotes adoption. | **2%** |
| **Sell** | Discourage large sell-offs and price dumping through a higher tax rate. | **6%** |
| **Transfer** | Maintain stability for peer-to-peer and in-game transactions. | **2%** |

---

## 2. Purpose of the Tax System

The primary purpose of the transaction tax is to **support the game's tokenomics** by maintaining a sustainable ecosystem.  
Each tax contributes to:
- Ensuring **ready liquidity** for smooth buying and selling on the market.  
- Providing **continuous funding** for game development, marketing, and community events.  
- Maintaining **price stability** and player incentives within the in-game economy.

---

## 3. Tax Distribution Model

Whenever a token is **bought, sold, or transferred**, the smart contract automatically **deducts and redistributes** a percentage of the transaction according to the following allocations:

| **Allocation** | **Description** | **Rate** |
|-----------------|-----------------|-----------|
| **Liquidity** | Adds part of the tax back into the liquidity pool to stabilize the token’s price. | **4%** |
| **Marketing** | Provides funding for promotions, community events, partnerships, and exchange listings. | **3%** |
| **Reflections / Rewards** | Used for in-game rewards, player staking incentives, and DAO participation bonuses. | **2%** |

---

## 4. Economic Rationale

- The **liquidity pool** ensures that the in-game marketplace remains stable and resistant to price manipulation.  
- **Marketing funds** drive visibility and growth, supporting player retention and long-term project sustainability.  
- **Reward allocations** encourage active player participation by rewarding engagement, staking, and governance involvement.  

These mechanisms work together to sustain a healthy token economy that benefits both players and investors.

---

## 5. Technical Feasibility

This tax model is **fully feasible on Polygon (PoS)** using a modified **ERC-20 smart contract**.  
The contract can be programmed to:
- Automatically apply varying tax rates depending on transaction type (buy, sell, or transfer).  
- Distribute tax portions to predefined wallets or liquidity pools.  
- Maintain transparency, with all transactions visible on the Polygon blockchain via PolygonScan.

---

## Example Transaction: Player Buys 600 Tokens

Let’s illustrate how the tax system works in practice.

### Step 1: Purchase Overview
- **Transaction type:** Buy  
- **Amount purchased:** 600 tokens  
- **Applicable tax:** 2% (buy tax)

### Step 2: Tax Calculation
- **Tax deducted:** 600 × 0.02 = **12 tokens**
- **Net tokens received by player:** 600 − 12 = **588 tokens**

### Step 3: Tax Distribution
The 12 tokens collected as tax are automatically redistributed according to the allocation model:

| **Category** | **Allocation Rate** | **Tokens Received** | **Purpose** |
|---------------|---------------------|----------------------|--------------|
| Liquidity | 4 / (4+3+2) = 44.4% | 12 × 0.444 = **5.33 tokens** | Adds liquidity to the pool for price stability |
| Marketing | 3 / (4+3+2) = 33.3% | 12 × 0.333 = **4.00 tokens** | Supports marketing, events, and partnerships |
| Rewards | 2 / (4+3+2) = 22.2% | 12 × 0.222 = **2.67 tokens** | Funds in-game rewards and staking incentives |

> **Total tax distributed:** 12 tokens

### Step 4: Result Summary
| **Recipient** | **Amount (tokens)** | **Notes** |
|----------------|---------------------|------------|
| Player | **588** | Final tokens received after tax |
| Liquidity Pool | **5.33** | Auto-added for market stability |
| Marketing Wallet | **4.00** | Used for project growth |
| Rewards Pool | **2.67** | Reserved for player incentives |

### Step 5: On-Chain Transparency
All of the above transfers occur **automatically** within the Polygon smart contract.  
Each step is visible on **PolygonScan**, ensuring transparency and accountability.
