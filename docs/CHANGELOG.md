## Branch: [development]

### [Unreleased]
> Scope corrente: **WinForms navigation cascade baseline + owner docs sync truth-first post-D1**

> Nota privacy: i file sorgente di missione/requisiti restano locali in `docs/target/requirements/` e non sono versionati nel repository pubblico.

#### D — WinForms implementation
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`f3509c0` — feat(winforms): wire baseline navigation cascade**
  - **Type:** ADDED · **Categoria:** WinForms/Desktop UI
  - **Cosa cambia:** collega `ExamNavigator.WinForms` al boundary `ExamNavigator.Application`, introduce un `BootstrapNavigationService` locale in memoria e cabla `Form1` per il caricamento iniziale e l’aggiornamento a cascata dei pannelli ambulatori, parti del corpo ed esami.
  - **Impatto:** trasforma l’host desktop da layout statico a baseline funzionale di navigazione, verificata da build verde e smoke manuale sul dataset bootstrap, pur restando ancora senza adapter SQL concreto, ricerca wired e griglia operativa.

- **`37b81ef` — feat(winforms): add baseline desktop host layout**
  - **Type:** ADDED · **Categoria:** WinForms/Desktop UI
  - **Cosa cambia:** introduce il progetto `ExamNavigator.WinForms`, lo aggancia alla solution e materializza un `Form1` baseline con area ricerca, tre pannelli affiancati, pulsante di conferma e griglia selezioni.
  - **Impatto:** apre il primo host desktop richiesto dalla missione e prepara il successivo wiring della cascata.

#### C — Database foundation
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`ad8f56d` — feat(database): add navigation and search reference queries**
  - **Type:** ADDED · **Categoria:** Database/Query
  - **Cosa cambia:** introduce `database/sql/003_navigation_queries.sql` con tre blocchi SQL di riferimento per pannello ambulatori, pannello parti del corpo e pannello esami, coerenti con la navigazione a cascata richiesta dalla missione.
  - **Impatto:** prepara il futuro adapter SQL e il wiring UI con una baseline query già allineata alla cascata e alla ricerca case-insensitive.

- **`762fd4c` — feat(database): add demo seed data for exam navigation**
  - **Type:** ADDED · **Categoria:** Database/Seed
  - **Cosa cambia:** introduce `database/sql/002_seed.sql` con dati demo realistici per parti del corpo, ambulatori, esami e relazioni `ExamRoom`.
  - **Impatto:** rende il database dimostrabile con un dataset minimo ma significativo, non dipendente da una cardinalità ridotta o artificiale.

- **`55ecc96` — feat(database): introduce initial sql server schema**
  - **Type:** ADDED · **Categoria:** Database/Schema
  - **Cosa cambia:** introduce `database/sql/001_schema.sql` con tabelle `BodyPart`, `Room`, `Exam`, `ExamRoom`, PK/FK e vincoli univoci.
  - **Impatto:** consolida la base relazionale SQL Server coerente con il modello dominio e con la relazione molti-a-molti tra esami e ambulatori.

#### B — Shared core foundation
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`e68cca0` — feat(application): add navigation contracts and service interface**
  - **Type:** ADDED · **Categoria:** Application
  - **Cosa cambia:** introduce il progetto `ExamNavigator.Application`, i contratti di navigazione/ricerca e l’interfaccia `IExamNavigationService`, oltre al collegamento solution e alla reference verso `ExamNavigator.Domain`.
  - **Impatto:** separa la definizione dei casi d’uso dall’host futuro e prepara il wiring WinForms/MVC su un perimetro applicativo condiviso.

- **`c5e9f07` — feat(domain): introduce core exam navigation entities**
  - **Type:** ADDED · **Categoria:** Domain
  - **Cosa cambia:** sostituisce il placeholder iniziale con le entità `BodyPart`, `Room`, `Exam`, `ExamRoom`.
  - **Impatto:** stabilisce un modello dominio minimo, leggibile e coerente con i requisiti funzionali del test.

- **`1c6b30f` — build(solution): bootstrap sln and shared domain library**
  - **Type:** ADDED · **Categoria:** Build/Repo
  - **Cosa cambia:** introduce `ExamNavigator.sln`, il primo progetto condiviso `ExamNavigator.Domain` e riallinea `.gitignore` alla realtà .NET del repository.
  - **Impatto:** apre una base strutturale difendibile per il core condiviso ed elimina il rumore di repository dovuto a un `.gitignore` non allineato allo stack reale.

#### A — Bootstrap repository + freeze requisiti
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`feda771` — docs(project): bootstrap repo governance and freeze mission baseline**
  - **Type:** ADDED · **Categoria:** Repo/Docs
  - **Cosa cambia:** inizializza repository Git e baseline documentale con `.gitignore`, `README.md`, `docs/TIMELINE.md`, `docs/CHANGELOG.md`, `docs/ROADMAP.md`; congela la missione e il freeze requisiti come artefatti locali non versionati in `docs/target/requirements/`.
  - **Impatto:** fornisce una base governata, tracciabile e pronta per l’evoluzione controllata del progetto senza esporre nel repo pubblico il materiale sorgente riservato.
