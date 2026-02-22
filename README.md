# delta-node-framework
# Please note that this Readme.md file is generated with the help of AI, and is thoroughly checked by the senior developer

# 🃏 Card Match Game (Unity)

A cleanly architected **Card Matching (Memory) Game** built in **Unity**, following **SOLID principles**, **Clean Architecture**, and **event-driven design**.

This project was developed as an assignment with a strong emphasis on:
- Maintainable and scalable architecture
- Clear separation of concerns
- Proper Unity UI and input handling
- Extensibility through configuration instead of hard-coded values

---

## 🎮 Gameplay Overview

- Cards are laid out in a configurable grid
- Player flips two cards at a time
- Matching cards remain revealed
- Mismatched cards flip back after a short delay
- Score increases on matches and supports **combo multipliers**
- Score decreases on mismatches
- At game start, all cards are briefly revealed to help memorization
- When all cards are matched, the game **restarts automatically**

---

## 🧠 Scoring & Combo System

**Scoring rules:**
- Match: `+100 × combo`
- Mismatch: `-10`
- Combo increases with consecutive matches
- Combo resets on mismatch

**Combo feedback:**
- Combo text (e.g. `COMBO x3`) appears on screen
- Hidden when combo ≤ 1

Scoring and combo logic are fully event-driven and completely decoupled from UI.

### Layer Responsibilities

| Layer | Responsibility |
|-----|---------------|
| Core | Game rules, entities, domain events |
| Application | Use-cases (matching, scoring, game completion) |
| Infrastructure | Persistence and event bus implementations |
| Presentation | UI, animations, input handling |
| Bootstrap | Object creation and dependency wiring |
| Configs | Data-only configuration using ScriptableObjects |

---

## 🔌 Event-Driven Design

The game relies on domain events such as:
- `CardFlipStarted`
- `MatchResolved`
- `ScoreChanged`
- `ComboChanged`
- `GameCompleted`

This ensures:
- Loose coupling between systems
- UI reacts to state instead of controlling logic
- Easy extensibility (audio, VFX, analytics, etc.)

---

## 🧩 ScriptableObject Configuration (No Code Changes Required)

All major gameplay values can be changed using ScriptableObjects.

---

### 1️⃣ Board Configuration

**Asset:** `BoardConfigSO`  
**Path:** `Assets/Configs/BoardConfig.asset`

Controls the grid size.

Fields:
- `Rows` – number of rows
- `Columns` – number of columns

Changing these values instantly updates the board layout.

---

### 2️⃣ Card Visual Configuration

**Asset:** `CardVisualConfigSO`

Maps logical card identities (`MatchKey`) to sprites.

Fields:
- `CardBack` – back-side sprite
- `Visuals`
  - `MatchKey` (int)
  - `Sprite`

You can add new card types or replace visuals without touching code.

---

### 3️⃣ Audio Configuration

**Asset:** `AudioConfigSO`

Maps game events to audio clips.

Fields:
- Card flip sound
- Match sound
- Mismatch sound
- Game over sound

Audio is fully event-driven and never called directly from gameplay logic.

---

### 4️⃣ Score Configuration

**Asset:** `ScoreConfigSO`

Defines scoring values.

Fields:
- `MatchScore`
- `MismatchPenalty`

Scoring logic remains unchanged when these values are modified.

---

## 💾 Save & Load System

- Game state is saved automatically using `PlayerPrefs`
- Saved data includes:
  - Card states (face-down, revealed, locked)
  - Current score
- On reload:
  - Card visuals correctly reflect saved state
  - Revealed and locked cards show correct icons
- Save data is cleared automatically when the game completes

---

## 🔄 Game Lifecycle

1. Game starts
2. Board is loaded or generated
3. Initial preview reveals all cards briefly
4. Player plays the game
5. Score and combo update live
6. When all cards are matched:
   - Game completion is detected
   - Game restarts after a short delay

All lifecycle control is handled through events and the bootstrapper.

---

## 🧪 Technical Highlights

- No `Update()` polling
- No singletons
- No `FindObjectOfType`
- No UI-driven game logic
- Input handled via `IPointerClickHandler`
- Animations do not block gameplay logic
- Visual timing separated from domain timing

---

## 🚀 How to Run

1. Open the project in **Unity**
2. Open `Assets/Scenes/Main.unity`
3. Press **Play**

No additional setup required.

---

## 📝 Notes for Reviewers

- The project prioritizes clarity and maintainability
- Architecture avoids over-engineering while remaining extensible
- Each system can be replaced independently
- Designed to scale via configuration rather than code changes

---

## ✅ Assignment Requirements Checklist

✔ Card matching gameplay  
✔ Configurable grid  
✔ Scoring system  
✔ Combo system  
✔ Clean architecture  
✔ Unity UI best practices  
✔ Extensible and maintainable codebase  

---

## 📌 Final Remarks

This project demonstrates:
- Strong understanding of Unity UI and event systems
- Clean separation between logic and presentation
- Production-oriented, scalable architecture
