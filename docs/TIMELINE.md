# TIMELINE — Nolex Exam Selection System

## Scopo
Questo documento governa l’avanzamento del progetto `Nolex Exam Selection System`.
La timeline è organizzata in step sequenziali con Definition of Done verificabile.

Principi:
- **Truth-first**
- **Progressione**
- **Anti-ridondanza**

---

## Legenda stati
- ☑️ = archiviato
- ✅ = completato e verificato
- 🟡 = presente ma da verificare/chiudere
- ⬜ = da fare

---

## Baseline / Evidenze standard richieste
Minimo iniziale:
- `git status -sb`
- `git log --oneline --decorate -5`

Quando la soluzione esisterà:
- build desktop
- test core applicativo
- smoke run funzionale

---

## A — ☑️ Bootstrap repository + freeze requisiti

**Obiettivo:** creare la baseline documentale e congelare il perimetro reale del test.

**DoD (A):**
- missione sorgente congelata;
- requirements freeze creato;
- repository inizializzato;
- branch `main` e `development` presenti;
- documenti owner minimi creati e coerenti.

### ✅ A0 — Congelamento missione sorgente
**Obiettivo:** copiare il testo originale del test in `docs/target/requirements/01_original_mission.md`.

**Evidenze (truth-first):**
- file presente nel repository;
- missione NOLEX congelata in forma grezza.

### ✅ A1 — Freeze requisiti strutturato
**Obiettivo:** derivare il documento `02_requirements_freeze.md` dalla missione grezza.

**Evidenze (truth-first):**
- `docs/target/requirements/02_requirements_freeze.md` presente;
- requisiti funzionali e non funzionali estratti in forma strutturata.

### ✅ A2 — Bootstrap owner docs
**Obiettivo:** introdurre `README.md`, `TIMELINE.md`, `CHANGELOG.md`, `ROADMAP.md`.

**Evidenze (truth-first):**
- documenti owner minimi presenti nel repository;
- baseline documentale iniziale disponibile e coerente con il bootstrap.

### ✅ A3 — Init repository e branching baseline
**Obiettivo:** inizializzare Git, creare baseline stabile e aprire `development`.

**Evidenze (truth-first):**
- root commit `1bf84d6` presente;
- branch `main` e `development` creati;
- push remoto eseguito su entrambi i branch;
- working tree dichiarata pulita nello snapshot fornito.

---

## B — ⬜ Solution skeleton + dominio condiviso

**Obiettivo:** creare la solution e il core condiviso.

**DoD (B):**
- solution presente;
- progetti core creati;
- entità dominio definite;
- contratti applicativi definiti.

### ⬜ B0 — Solution bootstrap
### ⬜ B1 — Domain model
### ⬜ B2 — Application services contracts

---

## C — ⬜ Database SQL Server + seed

**Obiettivo:** modellare e popolare il database.

**DoD (C):**
- schema SQL presente;
- seed coerente;
- relazioni molti-a-molti operative;
- dataset demo utilizzabile.

### ⬜ C0 — Schema iniziale
### ⬜ C1 — Seed demo
### ⬜ C2 — Query di filtro e ricerca

---

## D — ⬜ WinForms implementation

**Obiettivo:** implementare il client desktop richiesto dal test.

**DoD (D):**
- tre pannelli operativi;
- selezione default corretta;
- aggiornamento a cascata corretto;
- griglia selezioni operativa.

### ⬜ D0 — Layout form
### ⬜ D1 — Cascata ambulatorio -> parti del corpo -> esami
### ⬜ D2 — Conferma selezione + griglia
### ⬜ D3 — Reorder e delete righe

---

## E — ⬜ Ricerca e configurazione `.ini`

**Obiettivo:** completare filtro testuale e configurazione esterna.

**DoD (E):**
- ricerca case-insensitive su tre campi;
- invio e pulsante ricerca attivi;
- reset `Vedi tutti` attivo;
- loader `.ini` riflessivo operativo;
- default ricerca caricabili da configurazione.

### ⬜ E0 — Search service
### ⬜ E1 — Wiring UI ricerca
### ⬜ E2 — Ini parser
### ⬜ E3 — Default search configuration

---

## F — ⬜ Conversione web MVC

**Obiettivo:** portare la stessa logica funzionale su host web.

**DoD (F):**
- host MVC creato;
- riuso del core applicativo;
- stessa logica di filtro e ricerca;
- interfaccia web dimostrativa coerente col desktop.

### ⬜ F0 — Web host bootstrap
### ⬜ F1 — Controller e view model
### ⬜ F2 — UI web equivalente

---

## Note di governance
- `ARCHITECTURE.md` verrà attivato solo dopo la creazione reale della solution e dei moduli.
- La timeline è la source of truth operativa.
