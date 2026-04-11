# ARCHITECTURE — Exam Navigator System (.NET / SQL Server)

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

3. **Database SQL reference**
   - contiene schema, seed e query SQL di riferimento.

4. **Host WinForms**
   - contiene il primo host desktop della missione con cascata baseline wired tramite boundary applicativo, bootstrap service locale in memoria e prime foundation configurative tramite contenitore statico dei default di ricerca e parser raw del documento `.ini`.

5. **Owner docs**
   - governano stato operativo, storia del cambiamento, roadmap e struttura.

### Sottosistemi richiesti ma non ancora presenti nella codebase

5. adapter SQL eseguibile / infrastructure layer concreto;
6. caricamento runtime dei default da configurazione e relativo consumo nel bootstrap/UI;
7. host ASP.NET Core MVC;
8. test project e quality tooling dedicato.

---

## Repository layout

- `ExamNavigator.sln` — solution root del progetto
- `src/ExamNavigator.Domain` — entità di dominio minimali
- `src/ExamNavigator.Application` — contratti applicativi e interfaccia di servizio
- `src/ExamNavigator.WinForms` — host desktop WinForms con wiring baseline della cascata
- `database/sql/001_schema.sql` — schema SQL Server iniziale
- `database/sql/002_seed.sql` — dataset demo
- `database/sql/003_navigation_queries.sql` — query di riferimento per cascata e ricerca
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

### 3. Database SQL reference
Responsabilità correnti:
- formalizzare la baseline dati SQL Server;
- mantenere uno schema relazionale coerente con il dominio;
- mantenere seed e query di riferimento.

Stato attuale:
- presente come artefatto SQL separato;
- non ancora agganciato a un adapter C# eseguibile;
- utile come specifica operativa per il futuro layer infrastructure.

### 4. Host layer

#### 4.1 Host WinForms
Responsabilità correnti:
- materializzare il form desktop richiesto dalla missione;
- ospitare il layout statico della ricerca, dei tre pannelli di navigazione e della griglia selezioni;
- preparare il successivo wiring verso `Application`.

Stato attuale:
- presente come host desktop wired al boundary `Application`;
- contiene caricamento iniziale dei tre pannelli, aggiornamento a cascata da ambulatorio a parte del corpo a esami, ricerca testuale wired tramite pulsante `Cerca`, tasto Invio e reset `Vedi tutti`, oltre alla conferma selezione con append alla griglia tramite servizio bootstrap locale in memoria;
- contiene anche `Predefiniti_Ricerca` come primo contenitore statico dei default di `SearchText` e `SearchField`;
- contiene anche `IniConfigurationDocument` come parser raw di sezioni e coppie `chiave = valore` del file `.ini`;
- contiene anche `IniConfigurationBinder` come binder riflessivo type-safe verso `Predefiniti_*`;
- non contiene ancora adapter SQL concreto né caricamento runtime dei default né consumo runtime dei default nel bootstrap/UI; la griglia supporta conferma selezione, cancellazione della riga selezionata e riordinamento `move up / move down`.

#### 4.2 Host MVC
Stato attuale:
- assente.

Responsabilità futura prevista:
- conversione web del comportamento desktop senza duplicazione della logica applicativa.

### 5. Configurazione `.ini`
Stato attuale:
- presente come foundation parziale lato host WinForms;
- la classe statica `Predefiniti_Ricerca` centralizza i default di `SearchText` e `SearchField`;
- `IniConfigurationDocument` esegue il parsing raw di sezioni e coppie `chiave = valore` del file `.ini`;
- `IniConfigurationBinder` esegue il binding riflessivo type-safe sezione -> classe `Predefiniti_*` e proprietà -> valore;
- non esistono ancora il caricamento runtime dei default da file `.ini` né il consumo runtime dei default nel bootstrap/UI.

Responsabilità futura prevista:
- caricamento riflessivo dei default da file `.ini` nel bootstrap runtime senza contaminare il dominio.

---

## Stato AS-IS dei flussi

### Flusso realmente implementato oggi

Il flusso realmente implementato oggi è un baseline runtime parziale ma eseguibile:

- solution `.sln` compila;
- `Domain` compila;
- `Application` compila con reference a `Domain`;
- `ExamNavigator.WinForms` referenzia `ExamNavigator.Application`;
- `Program.cs` costruisce un `BootstrapNavigationService` locale in memoria;
- `ExamNavigator.WinForms` contiene `Predefiniti_Ricerca` come contenitore statico dei default di ricerca, non ancora consumato dal bootstrap runtime;
- `ExamNavigator.WinForms` contiene `IniConfigurationDocument` come parser raw del file `.ini`;
- `ExamNavigator.WinForms` contiene `IniConfigurationBinder` come binder riflessivo type-safe dei default verso `Predefiniti_*`, non ancora wired nel bootstrap runtime;
- `Form1` usa `IExamNavigationService` per popolare all’avvio i tre pannelli;
- la selezione dell’ambulatorio aggiorna parti del corpo ed esami;
- la selezione della parte del corpo aggiorna gli esami;
- la ricerca testuale filtra i tre pannelli sul bootstrap runtime tramite `SearchText` e `SearchField`, con attivazione da pulsante `Cerca`, tasto Invio e reset `Vedi tutti`;
- la conferma della selezione aggiunge una riga alla griglia riepilogativa;
- la rimozione della riga selezionata elimina elementi già confermati dalla griglia riepilogativa;
- lo spostamento su e giù riordina di una posizione la riga selezionata nella griglia riepilogativa mantenendo la selezione sul record spostato;
- baseline SQL esiste come script separati di riferimento.

In altre parole, la codebase possiede oggi:
- modello dominio;
- boundary applicativo;
- baseline dati SQL di riferimento;
- host WinForms compilabile e con navigazione a cascata baseline su servizio bootstrap locale.

Non possiede ancora:
- adapter SQL concreto;
- caricamento runtime dei default da configurazione e consumo runtime dei default nel bootstrap/UI;
- flusso end-to-end finale sulla persistenza reale.

### Flusso target già preparato a livello di boundary, ma non ancora implementato end-to-end

Host desktop/web
-> Application (`IExamNavigationService`)
-> Infrastructure SQL futura
-> script/query SQL coerenti con schema e seed
-> `ExamNavigationResult`
-> rendering host

Questo flusso è coerente con i confini della codebase, ma oggi è ancora solo preparato a livello di architettura e contratti.

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

- host WinForms wired a `Application` solo tramite bootstrap service locale in memoria; manca ancora l’aggancio a un adapter SQL concreto;
- nessun adapter SQL eseguibile presente;
- nessun binding riflessivo type-safe presente; il parsing raw `.ini` e i default di ricerca esistono ma non sono ancora applicati al bootstrap runtime;
- nessun progetto test presente;
- nessun lint / coverage / smoke automatizzato presente;
- possibile drift documentale se gli owner docs non restano esplicitamente allineati al fatto che i requisiti sorgente sono locali e non versionati.

---

## Linee guida per l’estensione del sistema

- la futura implementazione concreta di `IExamNavigationService` deve vivere fuori da `Application`, in un layer infrastructure dedicato;
- l’accesso SQL Server deve essere introdotto come adapter separato;
- il futuro host WinForms deve orchestrare il caso d’uso tramite `Application`;
- il futuro host MVC deve riusare lo stesso perimetro applicativo, evitando duplicazione di logica;
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
