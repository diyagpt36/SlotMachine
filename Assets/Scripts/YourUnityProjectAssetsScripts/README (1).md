# 🎰 Unity Slot Machine Game

A classic 3-reel slot machine built in Unity with clean OOP architecture, smooth reel animations, RNG-based outcomes, and a bonus feature.

---

## 🎮 Game Overview

- **3 reels**, each with a configurable symbol pool
- **Win condition**: all 3 centre (payline) symbols match
- **Staggered reel stop** for authentic slot feel
- **Bounce landing animation** per reel
- **Bonus trigger**: landing 3 bonus symbols awards 5× the bet
- **Balance system**: starts at 1000 coins, 10 coins per spin

---

## 🚀 How to Run the WebGL Build

1. Open the `/Build/WebGL` folder
2. Serve it via a local web server (e.g. `npx serve .` or `python -m http.server`)
3. Open `http://localhost:PORT` in Chrome or Firefox
4. Click **SPIN** to play

> **Note:** WebGL builds must be served over HTTP — double-clicking `index.html` won't work due to browser security restrictions.

---

## ✨ Bonus Features

| Feature | Description |
|---|---|
| Staggered reel stops | Reels stop left-to-right with a short delay each |
| Bounce landing | Each reel bounces slightly when it lands |
| Bonus symbol | 3× Bonus symbol triggers a 5× bet prize |
| Game over state | Spin button disables when balance hits 0 |

---

## 🏗️ Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs        ← Singleton, manages balance & game state
│   │   ├── SlotMachine.cs        ← Orchestrates spins across all reels
│   │   ├── ReelController.cs     ← Handles one reel's animation & symbol display
│   │   └── PaylineManager.cs     ← Evaluates win conditions & payouts
│   ├── Data/
│   │   ├── SymbolData.cs         ← ScriptableObject: sprite + payout per symbol
│   │   └── SlotConfiguration.cs  ← ScriptableObject: spin speed, bet amount, etc.
│   └── UI/
│       ├── UIManager.cs          ← Updates balance text, win panel, messages
│       └── SpinButton.cs         ← Relays button click to SlotMachine
├── Prefabs/
│   ├── Reel.prefab               ← One reel with 3 symbol Image slots
│   └── WinPanel.prefab           ← Animated win overlay
├── Animations/
│   └── WinPanelPop.anim          ← Win panel scale-in animation
├── ScriptableObjects/
│   ├── Symbols/                  ← One SymbolData asset per symbol
│   └── SlotConfig.asset          ← Global SlotConfiguration asset
└── UI/
    └── SlotMachineCanvas.prefab  ← Main canvas layout
```

---

## 🧠 Thought Process & Approach

### Architecture decisions
- **ScriptableObjects for data** (`SymbolData`, `SlotConfiguration`) so designers can tweak values without touching code.
- **Callback-based reel completion** instead of polling — `SlotMachine` passes an `Action` to each `ReelController` and only evaluates the payline once every reel has confirmed it stopped.
- **Staggered coroutines** give each reel its own independently running `SpinRoutine`, making the timing trivially adjustable via config.

### RNG
Unity's `UnityEngine.Random.Range` uses a Mersenne Twister seeded at startup — sufficient for a casual slot game. Each reel picks its target symbol independently, so outcomes are uniformly distributed across the symbol pool.

### Animation
The spin is split into two phases:
1. **Fast scroll** — symbols cycle at `spinSpeed` px/s until the timer expires.
2. **Bounce** — the payline slot overshoots by `bounceOvershoot` px then snaps back, giving a physical landing feel.

### Extensibility
- Add more symbols by creating new `SymbolData` assets and adding them to a reel's `symbolPool`.
- Add more paylines by extending `PaylineManager.Evaluate()`.
- Add free spins by extending `GameManager.TriggerBonusRound()`.
