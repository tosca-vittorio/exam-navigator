# ARCHITECTURE — Exam Navigator System (.NET / PostgreSQL local runtime + SQL Server reference heritage)

Documento architetturale truth-first che descrive lo stato corrente del repository.

## Finalità del documento

Il presente documento definisce in modo formale l’architettura corrente del progetto, con l’obiettivo di:

- fornire una rappresentazione accurata e trasferibile dello stato della codebase;
- delineare confini, responsabilità e interazioni tra i sottosistemi oggi presenti;
- distinguere chiaramente ciò che è già implementato da ciò che è soltanto richiesto dalla missione ma ancora assente;
- stabilire invarianti e linee guida di estensione coerenti con l’AS-IS.

## Relazione con la documentazione owner

Distribuzione corretta delle responsabilità:

- `TIMELINE.md` → stato operativo, step, DoD, avanzamento
- `CHANGELOG.md` → storia dei cambiamenti rilevanti e milestone
- `ROADMAP.md` → traiettoria evolutiva e priorità
- `ARCHITECTURE.md` → struttura reale del sistema, confini dei moduli, flussi attuali e invarianti correnti

Nota:
- questo documento descrive l’AS-IS reale del repository;
- non sostituisce né roadmap né timeline;
- segnala esplicitamente i sottosistemi ancora mancanti quando sono rilevanti per capire il perimetro corrente.

---

## Panoramica di alto livello

### Sottosistemi presenti oggi

1. **Domain**
   - contiene il modello di dominio minimo dell’applicazione.

2. **Application**
   - contiene contratti applicativi e interfacce di servizio.

3. **Database artifacts**
   - contiene la baseline SQL Server di riferimento e il bootstrap runtime locale PostgreSQL.

4. **Infrastructure PostgreSQL**
   - contiene il primo layer infrastructure concreto separato dal core condiviso, con implementazione PostgreSQL di `IExamNavigationService`.

5. **Host WinForms**
   - contiene il primo host desktop della missione, ora wired al runtime PostgreSQL concreto tramite boundary applicativo, con foundation configurative tramite contenitore statico dei default di ricerca, parser raw del documento `.ini`, binder riflessivo type-safe e baseline runtime dei default di ricerca.

6. **Host MVC**
   - contiene il primo host ASP.NET Core MVC della soluzione, inizialmente introdotto come scaffold compilabile e ora wired al runtime PostgreSQL concreto per navigazione, ricerca, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX, sempre wired al core condiviso.

7. **Owner docs**
   - governano stato operativo, storia del cambiamento, roadmap e struttura.

### Sottosistemi richiesti ma non ancora presenti nella codebase

5. test project e quality tooling dedicato.

---

## Repository layout

- `ExamNavigator.sln` — solution root del progetto
- `src/ExamNavigator.Domain` — entità di dominio minimali
- `src/ExamNavigator.Application` — contratti applicativi e interfaccia di servizio
- `src/ExamNavigator.Infrastructure.PostgreSql` — layer infrastructure PostgreSQL con adapter concreto di navigazione
- `src/ExamNavigator.WinForms` — host desktop WinForms con wiring baseline della cascata
- `src/ExamNavigator.Mvc` — host web ASP.NET Core MVC con baseline funzionale completa di navigazione e selezione
- `database/sql/001_schema.sql` — schema SQL Server iniziale di riferimento
- `database/sql/002_seed.sql` — dataset demo SQL Server di riferimento
- `database/sql/003_navigation_queries.sql` — query SQL Server di riferimento per cascata e ricerca
- `database/postgresql/001_schema.sql` — schema PostgreSQL per il runtime locale scelto
- `database/postgresql/002_seed.sql` — seed PostgreSQL per il runtime locale scelto
- `database/postgresql/postgresql.md` — documento tecnico di setup e pivot PostgreSQL
- `docs/TIMELINE.md` — source of truth operativa
- `docs/CHANGELOG.md` — tracciabilità evolutiva
- `docs/ROADMAP.md` — traiettoria e milestone
- `docs/ARCHITECTURE.md` — fotografia AS-IS della struttura corrente

Materiale locale non versionato:
- `docs/target/requirements/*` — sorgenti missione/requisiti mantenute fuori dal repository pubblico per privacy.

---

## Runtime data model

### Entità di dominio correnti

**BodyPart**
- `Id`
- `Name`

**Room**
- `Id`
- `Name`

**Exam**
- `Id`
- `CodiceMinisteriale`
- `CodiceInterno`
- `DescrizioneEsame`
- `BodyPartId`

**ExamRoom**
- `ExamId`
- `RoomId`

Questo modello rappresenta:

- relazione uno-a-molti tra `BodyPart` ed `Exam`;
- relazione molti-a-molti tra `Exam` e `Room` tramite `ExamRoom`.

### Contratti applicativi correnti

**ExamSearchField**
- enum dei tre campi di ricerca:
  - `CodiceMinisteriale`
  - `CodiceInterno`
  - `DescrizioneEsame`

**ExamNavigationRequest**
- selezione ambulatorio corrente;
- selezione parte del corpo corrente;
- testo di ricerca;
- campo di ricerca selezionato.

**LookupItem**
- coppia minima `Id` / `Label` per liste navigabili.

**ExamListItem**
- item di esame orientato alla presentazione/listing.

**ExamNavigationResult**
- collezione ambulatori;
- collezione parti del corpo;
- collezione esami;
- selezioni correnti risolte.

**IExamNavigationService**
- contratto applicativo minimo per risolvere la navigazione a cascata.

---

## Architettura per aree

### 1. Domain
Responsabilità correnti:
- modellare le entità minime del caso d’uso;
- restare indipendente da UI, SQL concreto e documentazione.

Stato attuale:
- puro e minimale;
- nessuna logica infrastrutturale;
- nessuna dipendenza verso `Application`.

### 2. Application
Responsabilità correnti:
- definire i contratti dei casi d’uso;
- esporre l’interfaccia di servizio applicativo;
- fungere da boundary tra host futuri e dominio.

Stato attuale:
- dipende da `Domain`;
- non contiene ancora implementazioni concrete di servizio;
- non contiene codice SQL o dettagli di host.

### 3. Database artifacts
Responsabilità correnti:
- formalizzare la baseline dati di riferimento SQL Server;
- mantenere un bootstrap runtime locale PostgreSQL coerente con il dominio;
- mantenere schema, seed e query di riferimento utili al futuro layer infrastructure.

Stato attuale:
- baseline SQL Server presente come reference storica/compatibility track;
- bootstrap runtime locale PostgreSQL presente come scelta tecnica attiva e documentata;
- gli artefatti PostgreSQL risultano ora agganciati a un primo adapter C# eseguibile nel layer infrastructure dedicato;
- il percorso SQL Server resta reference heritage e non costituisce il runtime locale attivo.

### 4. Infrastructure PostgreSQL
Responsabilità correnti:
- implementare il boundary `IExamNavigationService` contro il runtime PostgreSQL locale scelto;
- tradurre la logica di cascata e ricerca in query SQL concrete;
- isolare dettagli di provider, connection string e mapping fuori da `Application`.

Stato attuale:
- presente come progetto `src/ExamNavigator.Infrastructure.PostgreSql` su `netstandard2.0`;
- referenzia `ExamNavigator.Application`;
- usa `Npgsql 8.0.8` come provider PostgreSQL;
- contiene `PostgreSqlExamNavigationService` come prima implementazione concreta di `IExamNavigationService`;
- implementa query PostgreSQL reali per ambulatori, parti del corpo ed esami, con fallback di `SelectedRoomId` e `SelectedBodyPartId`;
- è wired in entrambi gli host applicativi tramite `Program.cs`: WinForms su .NET Framework 4.8 e MVC su ASP.NET Core `net9.0`.

### 5. Host layer

#### 5.1 Host WinForms
Responsabilità correnti:
- materializzare il form desktop richiesto dalla missione;
- ospitare il layout statico della ricerca, dei tre pannelli di navigazione e della griglia selezioni;
- preparare il successivo wiring verso `Application`.

Stato attuale:
- presente come host desktop wired al boundary `Application`;
- il runtime entrypoint costruisce ora `PostgreSqlExamNavigationService` con connection string locale PostgreSQL composta in `Program.cs` e password letta da variabile ambiente `EXAM_NAVIGATOR_PG_PASSWORD`;
- il `.csproj` governa la runtime closure necessaria a `Npgsql` / `.NET Standard` e `App.config` contiene i binding redirects espliciti necessari all'esecuzione WinForms su .NET Framework 4.8;
- contiene caricamento iniziale dei tre pannelli, aggiornamento a cascata da ambulatorio a parte del corpo a esami, ricerca testuale wired tramite pulsante `Cerca`, tasto Invio e reset `Vedi tutti`, oltre alla conferma selezione con append alla griglia su runtime PostgreSQL concreto;
- contiene anche `Predefiniti_Ricerca` come primo contenitore statico dei default di `SearchText` e `SearchField`;
- contiene anche `IniConfigurationDocument` come parser raw di sezioni e coppie `chiave = valore` del file `.ini`;
- contiene anche `IniConfigurationBinder` come binder riflessivo type-safe verso `Predefiniti_*`;
- normalizza i nomi ambulatorio, espone label leggibili per i campi di ricerca e rende il pannello `Esami` più leggibile con presentazione multi-line; la griglia supporta conferma selezione, cancellazione della riga selezionata e riordinamento `move up / move down`.

#### 5.2 Host MVC
Stato attuale:
- presente come host ASP.NET Core MVC su `net9.0`;
- aggiunto alla solution;
- referenziato al core condiviso tramite `ExamNavigator.Application`;
- `Program.cs` registra `PostgreSqlExamNavigationService` come implementazione di `IExamNavigationService`, aggiunge il reference infrastructure dedicato e costruisce la connection string locale PostgreSQL leggendo la password da `EXAM_NAVIGATOR_PG_PASSWORD`;
- `HomeController` costruisce `ExamNavigationRequest` dai parametri GET, gestisce `ApplySelectionCommand(...)` e mantiene uno stato minimale della griglia selezioni nella pagina;
- `Index.cshtml` espone una baseline web con ricerca GET, tre sezioni per ambulatori/parti del corpo/esami, pulsante `Conferma selezione`, griglia `Esami selezionati`, selezione riga e azioni `Sposta su` / `Sposta giù` / `Elimina riga`;
- `_Layout.cshtml` e `site.css` forniscono una shell UI/UX dedicata e non più scaffold generica;
- equivalente al comportamento baseline del client WinForms per navigazione, ricerca, conferma selezione, riordino ed eliminazione riga, ora alimentato dalla stessa sorgente dati PostgreSQL concreta.

Responsabilità futura prevista:
- conversione web del comportamento desktop senza duplicazione della logica applicativa e futuro aggancio a runtime SQL concreto condiviso.

### 5. Configurazione `.ini`
Stato attuale:
- presente come foundation parziale lato host WinForms;
- la classe statica `Predefiniti_Ricerca` centralizza i default di `SearchText` e `SearchField`;
- `IniConfigurationDocument` esegue il parsing raw di sezioni e coppie `chiave = valore` del file `.ini`;
- `IniConfigurationBinder` esegue il binding riflessivo type-safe sezione -> classe `Predefiniti_*` e proprietà -> valore;
- il caricamento runtime dei default da file `.ini` e il consumo runtime dei default nel bootstrap/UI sono presenti per la baseline della ricerca.

Responsabilità futura prevista:
- estendere il caricamento riflessivo dei default oltre la baseline della ricerca senza contaminare il dominio.

---

## Stato AS-IS dei flussi

### Flusso realmente implementato oggi

Il flusso realmente implementato oggi è un baseline runtime parziale ma eseguibile:

- solution `.sln` compila;
- `Domain` compila;
- `Application` compila con reference a `Domain`;
- `ExamNavigator.WinForms` referenzia `ExamNavigator.Application`;
- `ExamNavigator.Mvc` referenzia `ExamNavigator.Application`;
- `Program.cs` del client WinForms carica, se presente, un file `.ini`, applica i default verso `Predefiniti_*` e costruisce un `PostgreSqlExamNavigationService` con runtime locale PostgreSQL e password letta da variabile ambiente;
- `ExamNavigator.WinForms` contiene `Predefiniti_Ricerca` come contenitore statico dei default di ricerca, consumato dal bootstrap runtime per la baseline configurabile della ricerca;
- `ExamNavigator.WinForms` contiene `IniConfigurationDocument` come parser raw del file `.ini`;
- `ExamNavigator.WinForms` contiene `IniConfigurationBinder` come binder riflessivo type-safe dei default verso `Predefiniti_*`, wired nel bootstrap runtime per la baseline configurabile della ricerca;
- `Form1` inizializza la ricerca dai default configurati e usa `IExamNavigationService` per popolare all’avvio i tre pannelli;
- la selezione dell’ambulatorio aggiorna parti del corpo ed esami;
- la selezione della parte del corpo aggiorna gli esami;
- la ricerca testuale filtra i tre pannelli sul runtime PostgreSQL concreto tramite `SearchText` e `SearchField`, con attivazione da pulsante `Cerca`, tasto Invio e reset `Vedi tutti`;
- la conferma della selezione aggiunge una riga alla griglia riepilogativa;
- la rimozione della riga selezionata elimina elementi già confermati dalla griglia riepilogativa;
- lo spostamento su e giù riordina di una posizione la riga selezionata nella griglia riepilogativa mantenendo la selezione sul record spostato;
- `Program.cs` dell'host MVC registra `PostgreSqlExamNavigationService` nel container DI e costruisce la stessa connection string locale PostgreSQL già adottata dal client WinForms, con password letta da variabile ambiente `EXAM_NAVIGATOR_PG_PASSWORD`;
- `HomeController` usa `IExamNavigationService` per risolvere la navigazione a cascata, mappa il risultato in `ExamNavigationPageViewModel` e gestisce i comandi web di conferma selezione, riordino ed eliminazione riga;
- `Index.cshtml` rende una baseline web con ricerca GET, selezione esame, pulsante `Conferma selezione`, griglia `Esami selezionati`, radio di selezione riga e azioni `Sposta su` / `Sposta giù` / `Elimina riga`;
- `_Layout.cshtml` e `site.css` completano la shell visiva dedicata del host MVC;
- baseline SQL esiste come script separati di riferimento.

In altre parole, la codebase possiede oggi:
- modello dominio;
- boundary applicativo;
- baseline dati SQL di riferimento;
- adapter PostgreSQL concreto eseguibile nel layer infrastructure dedicato;
- host WinForms compilabile e con navigazione a cascata su runtime PostgreSQL concreto, verificato anche dopo clean rebuild della runtime closure;
- host MVC compilabile e wired al runtime PostgreSQL concreto, con baseline funzionale equivalente al client WinForms.

Non possiede ancora:
- test project e quality tooling dedicato ancora assenti dalla codebase;
- flusso end-to-end finale multi-host sulla persistenza reale;
- track qualità con test, lint, coverage e smoke automatizzati.

### Flusso target già preparato a livello di boundary, ma non ancora implementato end-to-end

Host desktop/web
-> Application (`IExamNavigationService`)
-> Infrastructure PostgreSQL futura
-> script/query SQL coerenti con schema e seed
-> `ExamNavigationResult`
-> rendering host

Questo flusso è ora implementato end-to-end sui due host applicativi attraverso lo stesso boundary `IExamNavigationService` e lo stesso adapter PostgreSQL concreto.

---

## Invarianti architetturali

1. `ExamNavigator.Domain` deve restare indipendente da UI, SQL concreto e host.
2. `ExamNavigator.Application` può dipendere da `ExamNavigator.Domain`, ma non deve inglobare dettagli SQL o codice UI.
3. Gli script SQL di riferimento devono restare esterni ai layer `Domain` e `Application`.
4. Gli host futuri devono consumare casi d’uso applicativi, non introdurre logica di dominio o query inline come asse portante del comportamento.
5. I file requisito sorgente locali e privati non fanno parte del contratto del repository pubblico e non devono essere descritti come owner docs versionati.

---

## Registro rischi

Rischi reali attuali:

- test project e quality tooling dedicato ancora assenti dalla codebase;
- il runtime WinForms dipende ora da una closure di assembly e binding redirects che devono restare governati dal sorgente senza regressioni;
- la scelta PostgreSQL diverge dal requisito SQL Server originario e richiede documentazione owner rigorosa per restare difendibile;
- configurazione `.ini` oggi limitata alla baseline della ricerca e consumata nel client WinForms ormai wired al runtime PostgreSQL concreto;
- nessun progetto test presente;
- nessun lint / coverage / smoke automatizzato presente;
- possibile drift documentale se gli owner docs non restano esplicitamente allineati al fatto che i requisiti sorgente sono locali e non versionati.

---

## Linee guida per l’estensione del sistema

- l’implementazione concreta di `IExamNavigationService` deve continuare a vivere fuori da `Application`, nel layer `src/ExamNavigator.Infrastructure.PostgreSql`;
- eventuali esigenze di portabilità o compatibilità SQL Server devono essere trattate come track separata/reference heritage, senza sostituire implicitamente il runtime PostgreSQL attivo;
- il futuro host WinForms deve orchestrare il caso d’uso tramite `Application`;
- l'host MVC riusa lo stesso perimetro applicativo del client WinForms, evitando duplicazione di logica;
- il binding riflessivo `.ini` verso `Predefiniti_*` deve restare un boundary infrastrutturale/configurativo, non nel dominio;
- test, lint, coverage e smoke devono essere aggiunti come track qualità su baseline stabile.

---

## Quality gates correnti

Baseline corrente disponibile:
- `dotnet build ExamNavigator.sln`

Gate non ancora introdotti:
- lint
- test unitari
- coverage
- smoke test

---

## Change policy

Questo documento si aggiorna solo quando cambia la struttura reale del sistema o quando è necessario un riallineamento truth-first.
Qui vive l’AS-IS della codebase, non il TO-BE speculativo.
