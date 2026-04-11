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
Minimo corrente:
- `git status -sb`
- `git log --oneline --decorate -10`
- `dotnet build ExamNavigator.sln`

Quando la soluzione sarà più matura:
- lint
- test unitari
- coverage
- smoke run funzionale

---

## EXTRA — ⬜ Tracce differite / non prioritarie rispetto alla missione

**Regola di governance:**
- gli EXTRA sono congelati e tracciati;
- non sovrascrivono la priorità della missione corrente;
- restano non attivati finché la baseline mission-critical non giustifica la loro apertura operativa.

### ⬜ EXTRA_0 — Documentazione estesa A→Z per preparazione colloquio
**Obiettivo:** produrre una documentazione estesa, difendibile e studiabile su requisiti, sintassi, struttura, architettura, logica e funzionamento del progetto.

### ⬜ EXTRA_1 — Congelamento formale requisiti + riverifica futura
**Obiettivo:** mantenere un tracciamento esplicito dei requisiti della missione e introdurre una successiva riverifica implementativa/funzionale.

### ⬜ EXTRA_2 — Introduzione lint
**Obiettivo:** introdurre una baseline di lint coerente con lo stack .NET scelto.

### ⬜ EXTRA_3 — Introduzione test unitari minimi
**Obiettivo:** introdurre test minimi sui blocchi core condivisi.

### ⬜ EXTRA_4 — Coverage baseline
**Obiettivo:** introdurre raccolta coverage sulla baseline stabile.

### ⬜ EXTRA_5 — Smoke test di progressione / non regressione
**Obiettivo:** introdurre verifiche smoke ripetibili per constatare progressioni e non regressioni.

### ⬜ EXTRA_6 — Quality campaign successiva
**Obiettivo:** eseguire una campagna qualità su baseline stabile dopo il consolidamento del flusso mission-critical.

### ⬜ EXTRA_7 — Ambiente isolato / centralizzazione dipendenze
**Obiettivo:** valutare e introdurre un ambiente isolato coerente con lo stack per contenere dipendenze e tooling di progetto.

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
**Obiettivo:** mantenere il testo originale del test in un artefatto locale dedicato.

**Evidenze (truth-first):**
- artefatto locale presente in `docs/target/requirements/01_original_mission.md`;
- mantenuto fuori dal repo pubblico per privacy.

### ✅ A1 — Freeze requisiti strutturato
**Obiettivo:** derivare il documento di freeze requisiti dalla missione grezza.

**Evidenze (truth-first):**
- freeze locale presente in `docs/target/requirements/02_requirements_freeze.md`;
- mantenuto fuori dal repo pubblico per privacy;
- requisiti funzionali e non funzionali congelati nel working environment.

### ✅ A2 — Bootstrap owner docs
**Obiettivo:** introdurre `README.md`, `TIMELINE.md`, `CHANGELOG.md`, `ROADMAP.md`.

**Evidenze (truth-first):**
- documenti owner minimi presenti nel repository;
- baseline documentale iniziale disponibile e coerente con il bootstrap.

### ✅ A3 — Init repository e branching baseline
**Obiettivo:** inizializzare Git, creare baseline stabile e aprire `development`.

**Evidenze (truth-first):**
- root commit `feda771` presente;
- branch `main` e `development` creati;
- push remoto eseguito su entrambi i branch.

---

## B — ☑️ Solution skeleton + dominio condiviso

**Obiettivo:** creare la solution e il core condiviso.

**DoD (B):**
- solution presente;
- progetti core creati;
- entità dominio definite;
- contratti applicativi definiti.

### ✅ B0 — Solution bootstrap
**Obiettivo:** creare la solution e il primo modulo condiviso.

**Evidenze (truth-first):**
- commit `1c6b30f` presente;
- `ExamNavigator.sln` presente;
- `ExamNavigator.Domain` introdotto;
- `dotnet build ExamNavigator.sln` eseguito con esito verde.

### ✅ B1 — Domain model
**Obiettivo:** definire il modello di dominio minimo coerente con la missione.

**Evidenze (truth-first):**
- commit `c5e9f07` presente;
- entità `BodyPart`, `Room`, `Exam`, `ExamRoom` introdotte;
- build verde confermata sulla solution.

### ✅ B2 — Application services contracts
**Obiettivo:** introdurre i contratti applicativi minimi per navigazione e ricerca.

**Evidenze (truth-first):**
- commit `e68cca0` presente;
- `ExamNavigator.Application` introdotto e referenziato dalla solution;
- contratti `ExamNavigationRequest`, `ExamNavigationResult`, `ExamListItem`, `LookupItem`, `ExamSearchField` presenti;
- interfaccia `IExamNavigationService` presente;
- build verde confermata sulla solution.

---

## C — ☑️ Database baseline di riferimento + bootstrap runtime locale

**Obiettivo:** modellare la baseline dati di riferimento e congelare il bootstrap runtime locale PostgreSQL.

**DoD (C):**
- schema SQL presente;
- seed coerente;
- relazioni molti-a-molti operative;
- dataset demo utilizzabile;
- query di filtro e ricerca presenti;
- artefatti PostgreSQL locali presenti e verificati.

### ✅ C0 — Schema iniziale
**Obiettivo:** introdurre lo schema SQL Server iniziale.

**Evidenze (truth-first):**
- commit `55ecc96` presente;
- `database/sql/001_schema.sql` presente;
- PK, FK, vincoli univoci e tabella ponte molti-a-molti introdotti.

### ✅ C1 — Seed demo
**Obiettivo:** introdurre un dataset demo realistico e riusabile.

**Evidenze (truth-first):**
- commit `762fd4c` presente;
- `database/sql/002_seed.sql` presente;
- dataset demo con più parti del corpo, più ambulatori e più esami introdotto;
- relazioni molti-a-molti coperte dal seed.

### ✅ C2 — Query di filtro e ricerca
**Obiettivo:** introdurre query di riferimento per la navigazione a cascata e la ricerca.

**Evidenze (truth-first):**
- commit `ad8f56d` presente;
- `database/sql/003_navigation_queries.sql` presente;
- query separate per:
  - pannello ambulatori;
  - pannello parti del corpo;
  - pannello esami;
- ricerca resa esplicitamente case-insensitive tramite `UPPER(...)`.

### ✅ C3 — PostgreSQL local runtime bootstrap artifacts
**Obiettivo:** congelare il pivot PostgreSQL locale con documento tecnico, schema e seed dedicati.

**Evidenze (truth-first):**
- commit `3a95d60` presente;
- `database/postgresql/postgresql.md` presente con scelta PostgreSQL locale, parametri runtime, stato reale raggiunto, scope/rischi e traiettoria di wiring futura;
- `database/postgresql/001_schema.sql` presente come traduzione PostgreSQL dello schema baseline;
- `database/postgresql/002_seed.sql` presente come seed PostgreSQL coerente col dataset demo;
- verifica locale eseguita con `psql` sul database `exam_navigator` e utente `exam_navigator_app`, con conteggi finali:
  - `body_part = 4`
  - `room = 7`
  - `exam = 6`
  - `exam_room = 8`
- il percorso SQL Server runtime esplorato è stato archiviato come strada non proseguita; PostgreSQL è ora la scelta runtime locale attiva e documentata.

---

## D — ☑️ WinForms implementation

**Obiettivo:** implementare il client desktop richiesto dal test.

**DoD (D):**
- tre pannelli operativi;
- selezione default corretta;
- aggiornamento a cascata corretto;
- griglia selezioni operativa.

### ✅ D0 — Layout form
**Obiettivo:** introdurre il form WinForms base con layout coerente alla missione.

**Evidenze (truth-first):**
- commit `37b81ef` presente;
- progetto `ExamNavigator.WinForms` introdotto e aggiunto alla solution;
- form principale baseline con area ricerca, tre pannelli affiancati, pulsante di conferma e griglia selezioni introdotto;
- `dotnet build ExamNavigator.sln` verde con compilazione di `ExamNavigator.WinForms`.

### ✅ D1 — Cascata ambulatorio -> parti del corpo -> esami
**Obiettivo:** collegare il layout ai contratti applicativi e al flusso dati.

**Evidenze (truth-first):**
- commit `f3509c0` presente;
- `ExamNavigator.WinForms` referenzia `ExamNavigator.Application`;
- `Program.cs` introduce un bootstrap `IExamNavigationService` locale in memoria per sbloccare il wiring host -> Application senza adapter SQL concreto;
- `Form1` esegue il popolamento iniziale dei tre pannelli e aggiorna in cascata il pannello parti del corpo e il pannello esami su selezione di ambulatorio e parte del corpo;
- `dotnet build ExamNavigator.sln` verde dopo il wiring;
- smoke manuale verificato su dataset bootstrap per `EcografiaMassimino`/`EcografiaPrivitera`, `Radiologia` e `Risonanza`.

### ✅ D2 — Conferma selezione + griglia
**Obiettivo:** introdurre la selezione confermata e la griglia delle scelte.

**Evidenze (truth-first):**
- commit `472bed4` presente;
- `Form1` aggancia `btnConfirmExam.Click`;
- la conferma della selezione aggiunge una riga in `dgvSelectedExams` con codice ministeriale, codice interno, descrizione esame, parte del corpo e ambulatorio;
- smoke manuale verificato sul dataset bootstrap con accumulo coerente di tutte le combinazioni oggi selezionabili nella griglia;

### ✅ D3 — Reorder e delete righe
**Obiettivo:** completare la gestione operativa delle righe nella griglia selezioni.

### ✅ D3.1 — Delete riga selezionata
**Obiettivo:** permettere la cancellazione della riga selezionata dalla `dgvSelectedExams`.

**Evidenze (truth-first):**
- commit `6d50495` presente;
- `Form1` aggancia `btnRemoveSelected.Click`;
- `BtnRemoveSelected_Click(...)` rimuove le righe selezionate da `dgvSelectedExams`;
- `dotnet build ExamNavigator.sln` verde;
- smoke manuale verificato con creazione di 6 righe e cancellazione sparsa coerente.

### ✅ D3.2 — Move up / move down righe
**Obiettivo:** introdurre lo spostamento su e giù delle righe nella griglia selezioni.

**Evidenze (truth-first):**
- commit `bb85c7a` presente;
- `Form1` aggancia `btnMoveUp.Click` e `btnMoveDown.Click`;
- `BtnMoveUp_Click(...)` e `BtnMoveDown_Click(...)` riordinano di una posizione la riga selezionata in `dgvSelectedExams`;
- `dotnet build ExamNavigator.sln` verde;
- smoke manuale verificato con spostamento in alto, spostamento in basso ed eliminazione coerente.

---

## E — ☑️ Ricerca e configurazione `.ini`

**Obiettivo:** completare filtro testuale e configurazione esterna.

**DoD (E):**
- ricerca case-insensitive su tre campi;
- invio e pulsante ricerca attivi;
- reset `Vedi tutti` attivo;
- loader `.ini` riflessivo operativo;
- default ricerca caricabili da configurazione.

### ✅ E0 — Preflight search service
**Obiettivo:** congelare la superficie reale della ricerca prima di introdurre nuove patch implementative.

**Evidenze (truth-first):**
- audit su `Form1`, `Program.cs` e contratti `Application` eseguito;
- verificato che `ExamNavigationRequest` e `IExamNavigationService` erano già sufficienti per la ricerca;
- verificato che il bootstrap `IExamNavigationService` locale in memoria implementava già il filtro case-insensitive per `CodiceMinisteriale`, `CodiceInterno` e `DescrizioneEsame`;
- emerso come gap reale il solo wiring UI della ricerca lato WinForms, non il boundary applicativo.

### ✅ E1 — Wiring UI ricerca
**Obiettivo:** collegare la ricerca al host desktop WinForms.

**Evidenze (truth-first):**
- commit `05fff07` presente;
- `Form1` aggancia `btnSearch.Click`, `btnClearSearch.Click` e `txtSearchTerm.KeyDown`;
- introdotti `BtnSearch_Click(...)`, `BtnClearSearch_Click(...)`, `TxtSearchTerm_KeyDown(...)` e `ApplySearch()`;
- `dotnet build ExamNavigator.sln` verde;
- smoke manuale verificato con ricerca `eco` via pulsante `Cerca`, ricerca `rmn` via tasto Invio e reset `Vedi tutti` coerente sui tre pannelli.

### ✅ E2 — Ini parser
**Obiettivo:** introdurre il caricatore `.ini` riflessivo.

**Evidenze (truth-first):**
- audit configurativo su `App.config`, `Program.cs`, `ExamNavigationRequest`, `Form1`, `Properties/Settings.settings` e `Properties/Settings.Designer.cs` eseguito;
- verificato che `App.config` e `Properties/Settings` non rappresentano il boundary corretto per soddisfare la missione `.ini`;
- commit `b0ef0c2` presente;
- introdotta la classe statica `Predefiniti_Ricerca` nel progetto `ExamNavigator.WinForms`;
- commit `8e1fa21` presente;
- introdotto `IniConfigurationDocument` come parser raw del file `.ini`, con supporto a sezioni `[Sezione]`, coppie `chiave = valore`, commenti `#` e righe vuote;
- commit `c1c6170` presente;
- introdotto `IniConfigurationBinder` come binder riflessivo type-safe verso le classi statiche `Predefiniti_*`;
- il binder risolve la classe `Predefiniti_<Sezione>`, cerca proprietà statiche pubbliche per nome, ignora sezioni/chiavi non riconosciute e mantiene i default dichiarati per le proprietà assenti;
- conversione type-safe presente per `string` con virgolette obbligatorie, `int`, `bool` (`1/0`, `true/false`) ed `enum`;
- `ExamNavigator.WinForms.csproj` aggiornato per includere i nuovi file di configurazione nel build;
- `dotnet build ExamNavigator.sln` verde dopo l’introduzione del contenitore statico, del parser raw e del binder riflessivo;
- caricamento runtime in `Program.cs` e consumo runtime dei default nel bootstrap/UI ancora assenti e differiti al blocco successivo.

### ✅ E2.5 — Binding riflessivo type-safe verso `Predefiniti_*`
**Obiettivo:** introdurre il binder riflessivo minimale per applicare i valori del documento `.ini` alle classi statiche `Predefiniti_*`.

**Evidenze (truth-first):**
- commit `c1c6170` presente;
- introdotto `IniConfigurationBinder` nel progetto `ExamNavigator.WinForms`;
- il binder risolve riflessivamente la classe `Predefiniti_<Sezione>`;
- il binder cerca proprietà statiche pubbliche per nome e ignora chiavi non riconosciute;
- conversione type-safe presente per `string` con virgolette obbligatorie, `int`, `bool` (`1/0`, `true/false`) ed `enum`;
- `ExamNavigator.WinForms.csproj` aggiornato per includere il nuovo file nel build;
- `dotnet build ExamNavigator.sln` verde dopo l’introduzione del binder.

### ✅ E3 — Default search configuration
**Obiettivo:** supportare ricerca predefinita e tipo di ricerca predefinito da configurazione.

**Evidenze (truth-first):**
- commit `8344dcc` presente;
- `Program.cs` carica all'avvio un file `.ini`, se presente nella cartella del programma, e applica i default tramite `IniConfigurationDocument` + `IniConfigurationBinder`;
- `Form1` inizializza `txtSearchTerm` da `Predefiniti_Ricerca.SearchText`;
- `Form1` inizializza `cmbSearchField` da `Predefiniti_Ricerca.SearchField`, con fallback al default legacy se il valore configurato non è rappresentabile nella combo;
- il primo `LoadNavigation(...)` usa i default configurati di `SearchText` e `SearchField` già in fase di avvio del form;
- `dotnet build ExamNavigator.sln` verde dopo il wiring runtime dei default di ricerca;
- il caricamento resta su bootstrap service locale in memoria e non introduce ancora adapter SQL concreto né host MVC.

---

## F — ☑️ Conversione web MVC

**Obiettivo:** portare la stessa logica funzionale su host web.

**DoD (F):**
- host MVC creato;
- riuso del core applicativo;
- stessa logica di filtro e ricerca;
- interfaccia web dimostrativa coerente col desktop.

### ✅ F0 — Web host bootstrap
**Obiettivo:** introdurre il primo host ASP.NET Core MVC e agganciarlo alla solution con riferimento al core condiviso.

**Evidenze (truth-first):**
- commit `c1ba47f` presente;
- creato `src/ExamNavigator.Mvc` tramite template `mvc`;
- `ExamNavigator.Mvc.csproj` aggiunto a `ExamNavigator.sln`;
- `ExamNavigator.Mvc.csproj` referenzia `..\ExamNavigator.Application\ExamNavigator.Application.csproj`;
- `dotnet build ExamNavigator.sln` verde con compilazione di `ExamNavigator.Mvc` su `net9.0`;
- host MVC presente in baseline come scaffold tecnico, non ancora allineato al comportamento funzionale del client WinForms.

### ✅ F1 — Controller e view model
**Obiettivo:** introdurre il primo riallineamento MVC al core condiviso senza ancora completare tutta la UI web equivalente.

**Evidenze (truth-first):**
- commit `af3711c` presente;
- `HomeController` consuma `IExamNavigationService` e costruisce `ExamNavigationRequest` a partire dai parametri GET della pagina;
- `Program.cs` registra `BootstrapNavigationService` come implementazione locale in memoria di `IExamNavigationService` nel container DI del nuovo host MVC;
- `Models/ErrorViewModel.cs` ospita anche `ExamNavigationPageViewModel` come primo view model dedicato al caso d'uso;
- `Views/Home/Index.cshtml` sostituisce la pagina demo scaffold con una baseline MVC di navigazione esami, ricerca GET e tre sezioni `Ambulatori` / `Parti del corpo` / `Esami`;
- `dotnet build ExamNavigator.sln` verde dopo il wiring MVC;
- smoke manuale verificato con `dotnet run --project src/ExamNavigator.Mvc --urls http://localhost:5099` e controllo `curl` dei marker funzionali della pagina `/`.

### ✅ F2 — UI web equivalente
**Obiettivo:** completare una baseline web equivalente al comportamento demo del client WinForms, includendo shell UI, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish visivo minimo.

**Evidenze (truth-first):**
- commit `4c45148` presente;
- `Views/Shared/_Layout.cshtml` e `wwwroot/css/site.css` non sono più scaffold generico e materializzano una shell visiva coerente col progetto;
- `HomeController` introduce `ApplySelectionCommand(...)`, gestione dei comandi `confirm` / `up` / `down` / `remove` e serializzazione minimale dello stato della griglia selezioni nella pagina;
- `Models/ErrorViewModel.cs` ospita anche `ExamNavigationCommandInputModel`, `SelectedExamRowViewModel` e l'estensione di `ExamNavigationPageViewModel` per supportare selezione esame e griglia web;
- `Views/Home/Index.cshtml` introduce selezione esame nel pannello `Esami`, pulsante `Conferma selezione`, griglia `Esami selezionati`, radio di selezione riga e azioni `Sposta su` / `Sposta giù` / `Elimina riga`;
- `Program.cs` normalizza anche il naming demo degli ambulatori MVC (`Ecografia Massimino`, `Ecografia Privitera`);
- il campo ricerca lato MVC usa etichette leggibili (`Codice ministeriale`, `Codice interno`, `Descrizione esame`) e la leggibilità dello stato selezionato nei pannelli è stata migliorata;
- `dotnet build ExamNavigator.sln` verde dopo il completamento di `F2`;
- smoke manuale verificato su browser con ricerca, conferma selezione, aggiunta righe in griglia, riordino, eliminazione e verifica UI/UX complessivamente positiva.



**Nota di stato (F):**
- il blocco `F` è archiviato;
- non deve essere riaperto per completare la V1;
- i residui mission-critical sono ora tracciati nel blocco `G`.

---

## G — 🟡 Chiusura V1 mission-critical (PostgreSQL runtime concreto)

**Obiettivo:** chiudere la V1 sul runtime PostgreSQL locale scelto, mantenendo la divergenza rispetto al requisito SQL Server originario esplicitamente documentata, auditabile e difendibile.

**DoD (G):**
- adapter / infrastructure PostgreSQL concreta presente;
- host WinForms wired a runtime PostgreSQL;
- host MVC wired a runtime PostgreSQL;
- coerenza finale verificata rispetto al freeze requisiti, con divergenza SQL Server dichiarata nei documenti owner;
- baseline V1 concreta, robusta, affidabile, solida, consegnabile e valutabile.

### ✅ G0 — Preflight strategia PostgreSQL runtime + documento tecnico di setup
**Obiettivo:** congelare il boundary tecnico corretto per PostgreSQL runtime e il documento tecnico di setup locale.

**Evidenze (truth-first):**
- commit `3a95d60` presente;
- documento tecnico `database/postgresql/postgresql.md` presente e versionato;
- artefatti `database/postgresql/001_schema.sql` e `database/postgresql/002_seed.sql` presenti e coerenti con il dataset demo;
- setup locale verificato tramite `psql`, con database `exam_navigator`, utente `exam_navigator_app` e conteggi finali coerenti sul seed;
- percorso SQL Server/LocalDB esplicitamente superato come runtime locale attivo.

### ⬜ G1 — Adapter PostgreSQL concreto
**Obiettivo:** introdurre uno strato infrastructure PostgreSQL reale, separato da `Domain` e `Application`, senza riaprire i blocchi UI già chiusi.

### ⬜ G2 — Wiring WinForms su runtime PostgreSQL
**Obiettivo:** sostituire nel client desktop il bootstrap service in memoria con la sorgente dati PostgreSQL concreta.

### ⬜ G3 — Wiring MVC su runtime PostgreSQL
**Obiettivo:** sostituire nell'host MVC il bootstrap service in memoria con la stessa sorgente dati PostgreSQL concreta del client WinForms.

### ⬜ G4 — Verifica formale chiusura V1
**Obiettivo:** verificare in modo esplicito la copertura del perimetro mail/freeze, dichiarare la divergenza rispetto alla richiesta SQL Server e congelare la baseline V1 come richiesta cliente.

---

## H — ⬜ Preparazione consegna / rilascio / demo V1

**Obiettivo:** preparare il rilascio della baseline V1 nel formato più opportuno, con tag dedicato, merge su `main`, release e materiale di demo coerente con la richiesta cliente.

**Note operative congelate:**
- valutare il formato di consegna più opportuno (`.exe`, script SQL, bundle demo, eventuale Docker solo se realmente utile);
- creare tag dedicato alla consegna cliente;
- eseguire merge + release su `main`;
- usare questo punto come freeze del rilascio cliente prima di ulteriori ottimizzazioni su `development`.

---

## I — ⬜ Preparazione colloquio #2

**Obiettivo:** costruire il materiale di studio e presentazione dopo la chiusura completa della V1.

**Note operative congelate:**
- analizzare modo, scelte, logiche di sviluppo e relativa sintassi;
- spiegare passi temporali e percorso in timeline in modo esteso e dettagliato;
- creare domande plausibili sulla base del codice realizzato;
- descrivere ideazione, progettazione, analisi, scelte e architettura;
- descrivere moduli, responsabilità e mansioni;
- analizzare e spiegare `database/sql/*.sql`;
- spiegare architettura, pattern e scelte riconducibili;
- ripassare MVC usando il codice della V1 come caso di studio;
- chiarire esempi base su controller MVC e sui concetti di stato in UI moderne.

---

## J — ⬜ Sviluppi futuri / EXTRA post-colloquio

**Obiettivo:** continuare a migliorare la soluzione solo dopo la fase di preparazione colloquio.

**Note operative congelate:**
- refactor pulito e possibile introduzione di pattern GOF o equivalenti;
- best practices più spinte di qualità, programmazione, ingegneria e architettura del software;
- eventuale UI React/TypeScript collegata a backend .NET;
- region ordinate e commenti XML estesi.

---

## Note di governance
- `ARCHITECTURE.md` è ora attivo perché la struttura del repository è diventata abbastanza stabile da meritare una fotografia AS-IS dedicata.
- I file requisito sorgente restano locali e non versionati nel repo pubblico.
- Gli EXTRA sono congelati ma non attivati; non sovrascrivono il flusso mission-critical.
- La timeline resta la source of truth operativa.
