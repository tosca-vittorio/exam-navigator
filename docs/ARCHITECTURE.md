# ARCHITECTURE — Exam Navigator System (.NET / SQL Server concrete runtime + PostgreSQL heritage/demo artifacts)

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
   - contiene la baseline SQL Server di riferimento e gli artefatti PostgreSQL locali heritage/demo.

4. **Infrastructure SQL Server**
   - contiene il layer infrastructure concreto attivo, con implementazione SQL Server di `IExamNavigationService`.

5. **Infrastructure PostgreSQL**
   - contiene il precedente layer infrastructure PostgreSQL, oggi mantenuto come track legacy/reference e non più usato dal wiring runtime attivo degli host.

6. **Host WinForms**
   - contiene il primo host desktop della missione, ora wired al runtime SQL Server concreto tramite boundary applicativo, con foundation configurative tramite contenitore statico dei default di ricerca, parser raw del documento `.ini`, binder riflessivo type-safe e baseline runtime dei default di ricerca.

7. **Host MVC**
   - contiene il primo host ASP.NET Core MVC della soluzione, wired al runtime SQL Server concreto per navigazione, ricerca, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX, sempre tramite core condiviso.

8. **Owner docs**
   - governano stato operativo, storia del cambiamento, roadmap e struttura.

### Sottosistemi richiesti ma non ancora presenti nella codebase

1. test project e quality tooling dedicato.

---

## Repository layout

- `ExamNavigator.sln` — solution root del progetto
- `src/ExamNavigator.Domain` — entità di dominio minimali
- `src/ExamNavigator.Application` — contratti applicativi e interfaccia di servizio
- `src/ExamNavigator.Infrastructure.SqlServer` — layer infrastructure SQL Server con adapter concreto di navigazione
- `src/ExamNavigator.Infrastructure.PostgreSql` — layer infrastructure PostgreSQL legacy/reference non più usato dal wiring runtime attivo degli host
- `src/ExamNavigator.WinForms` — host desktop WinForms con wiring runtime SQL Server concreto
- `src/ExamNavigator.Mvc` — host web ASP.NET Core MVC con baseline funzionale completa di navigazione e selezione, wired al runtime SQL Server concreto
- `database/sql/001_schema.sql` — schema SQL Server di riferimento
- `database/sql/002_seed.sql` — dataset demo SQL Server di riferimento
- `database/sql/003_navigation_queries.sql` — query SQL Server di riferimento per cascata e ricerca
- `database/sql/sqlserver.md` — documento tecnico di bootstrap locale SQL Server del runtime attivo
- `database/postgresql/001_schema.sql` — schema PostgreSQL heritage/demo
- `database/postgresql/002_seed.sql` — seed PostgreSQL heritage/demo
- `database/postgresql/postgresql.md` — documento tecnico del precedente pivot PostgreSQL locale
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
- formalizzare la baseline dati SQL Server coerente con la missione;
- mantenere gli artefatti PostgreSQL locali come heritage/demo track e memoria del pivot tecnico precedente;
- mantenere schema, seed e query di riferimento utili al layer infrastructure concreto.

Stato attuale:
- baseline SQL Server presente come riferimento dati coerente con il wiring runtime attivo degli host;
- `database/sql/001_schema.sql`, `database/sql/002_seed.sql` e `database/sql/003_navigation_queries.sql` costituiscono il set di artefatti dati oggi allineato al runtime concreto SQL Server;
- `database/sql/sqlserver.md` documenta il bootstrap locale SQL Server attivo, i comandi di setup/verifica e il wiring runtime condiviso dei due host;
- `database/postgresql/001_schema.sql`, `database/postgresql/002_seed.sql` e `database/postgresql/postgresql.md` restano presenti come heritage/demo track del precedente pivot PostgreSQL locale;
- gli artefatti PostgreSQL non costituiscono più il bootstrap runtime attivo degli host applicativi;
- il repository mantiene quindi sia la baseline SQL Server concreta attiva sia il tracciato PostgreSQL storico/documentale, senza che quest’ultimo governi più il wiring runtime corrente.

### 4. Infrastructure SQL Server
Responsabilità correnti:
- implementare il boundary `IExamNavigationService` contro il runtime SQL Server concreto;
- tradurre la logica di cascata e ricerca in query SQL Server concrete;
- isolare dettagli di provider, connection string e mapping fuori da `Application`.

Stato attuale:
- presente come progetto `src/ExamNavigator.Infrastructure.SqlServer` su `netstandard2.0`;
- referenzia `ExamNavigator.Application`;
- usa `Microsoft.Data.SqlClient 7.0.0` come provider SQL Server;
- contiene `SqlServerExamNavigationService` come implementazione concreta attiva di `IExamNavigationService`;
- implementa query SQL Server reali per ambulatori, parti del corpo ed esami, con fallback di `SelectedRoomId` e `SelectedBodyPartId`;
- è wired in entrambi gli host applicativi tramite `Program.cs`: WinForms su .NET Framework 4.8 e MVC su ASP.NET Core `net9.0`.

### 5. Infrastructure PostgreSQL
Responsabilità correnti:
- preservare il precedente adapter PostgreSQL come riferimento tecnico e storico del pivot runtime locale già realizzato;
- mantenere disponibile una traccia concreta del precedente wiring su `IExamNavigationService`, senza governare il runtime attivo corrente.

Stato attuale:
- presente come progetto `src/ExamNavigator.Infrastructure.PostgreSql` su `netstandard2.0`;
- referenzia `ExamNavigator.Application`;
- usa `Npgsql 8.0.8` come provider PostgreSQL;
- contiene `PostgreSqlExamNavigationService` come precedente implementazione concreta di `IExamNavigationService`;
- non è più il layer infrastructure usato dal wiring runtime attivo degli host;
- resta nel repository come track legacy/reference, utile per audit architetturale, tracciabilità evolutiva e memoria del pivot PostgreSQL precedente.

### 6. Host layer

#### 6.1 Host WinForms
Responsabilità correnti:
- materializzare il form desktop richiesto dalla missione;
- ospitare il layout statico della ricerca, dei tre pannelli di navigazione e della griglia selezioni;
- orchestrare il boundary applicativo verso il runtime concreto attivo.

Stato attuale:
- presente come host desktop wired al boundary `Application`;
- il runtime entrypoint costruisce ora `SqlServerExamNavigationService` con connection string SQL Server letta da variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- `Form1` mantiene ancora un costruttore parameterless che delega a `BootstrapNavigationService`; questo path costituisce un fallback legacy in-memory ancora raggiungibile, ma non il bootstrap runtime principale del client desktop;
- il `.csproj` governa la runtime closure necessaria a `Microsoft.Data.SqlClient` per il client desktop su .NET Framework 4.8;
- la runtime closure desktop governa anche le dipendenze transitive richieste da `Microsoft.Data.SqlClient` (`Microsoft.Data.SqlClient.Extensions.Abstractions`, `Microsoft.Data.SqlClient.Internal.Logging`, `System.Buffers`, `System.Memory`, `System.Numerics.Vectors`, `System.Runtime.CompilerServices.Unsafe`, `System.Threading.Tasks.Extensions`) con fallback `net462` / `net461` / `netstandard2.0` dove necessario;
- `App.config` contiene i binding redirects espliciti necessari all'esecuzione WinForms su .NET Framework 4.8;
- contiene caricamento iniziale dei tre pannelli, aggiornamento a cascata da ambulatorio a parte del corpo a esami, ricerca testuale wired tramite pulsante `Cerca`, tasto Invio e reset `Vedi tutti`, oltre alla conferma selezione con append alla griglia sul runtime SQL Server concreto;
- contiene anche `Predefiniti_Ricerca` come primo contenitore statico dei default di `SearchText` e `SearchField`;
- contiene anche `IniConfigurationDocument` come parser raw del file `.ini`;
- contiene anche `IniConfigurationBinder` come binder riflessivo type-safe dei default verso `Predefiniti_*`, wired nel bootstrap runtime per la baseline configurabile della ricerca;
- normalizza i nomi ambulatorio, espone label leggibili per i campi di ricerca e rende il pannello `Esami` più leggibile con presentazione multi-line; la griglia supporta conferma selezione, cancellazione della riga selezionata e riordinamento `move up / move down`.

#### 6.2 Host MVC
Stato attuale:
- presente come host ASP.NET Core MVC su `net9.0`;
- aggiunto alla solution;
- referenziato al core condiviso tramite `ExamNavigator.Application` e al layer infrastructure concreto tramite `ExamNavigator.Infrastructure.SqlServer`;
- `Program.cs` registra `SqlServerExamNavigationService` come implementazione di `IExamNavigationService` e legge la connection string SQL Server dalla variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- `HomeController` costruisce `ExamNavigationRequest` dai parametri GET, gestisce `ApplySelectionCommand(...)` e rende la pagina in modalità duale: `Index` completo per le richieste normali e fragment `_ExamNavigationPage` per le richieste AJAX, tramite `RenderNavigationPage(...)` e `IsAjaxNavigationRequest()`;
- `Index.cshtml` è una shell MVC con root `#exam-navigation-root` e script inline basato su `fetch`, `AbortController`, `history.pushState` e `popstate`, che intercetta link interni e submit dei form per aggiornare la pagina in modo incrementale senza full-page reload;
- `_ExamNavigationPage.cshtml` ospita il markup reale della pagina web: ricerca, tre pannelli di navigazione, conferma selezione, griglia riepilogativa e comandi di riordino/eliminazione;
- `_Layout.cshtml` e `site.css` forniscono una shell UI/UX dedicata e non più scaffold generica;
- equivalente al comportamento baseline del client WinForms per navigazione, ricerca, conferma selezione, riordino ed eliminazione riga, ora alimentato dalla stessa sorgente dati SQL Server concreta; `HomeController` continua inoltre a normalizzare lato host i label degli ambulatori per la navigazione web e per la griglia `Esami selezionati`, senza spostare tale responsabilità nel boundary applicativo o nel layer infrastructure;
- il passaggio a shell + fragment + fetch incrementale continua a eliminare il salto viewport su liste lunghe senza reintrodurre workaround UI invasivi.

Responsabilità futura prevista:
- estendere il comportamento web mantenendo la logica applicativa nel boundary condiviso e privilegiando aggiornamenti incrementali a fragment per le interazioni ad alta frequenza.

### 7. Configurazione `.ini`

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

Il flusso realmente implementato oggi è un baseline runtime eseguibile su SQL Server concreto:

- solution `.sln` compila;
- `Domain` compila;
- `Application` compila con reference a `Domain`;
- `ExamNavigator.WinForms` referenzia `ExamNavigator.Application`;
- `ExamNavigator.Mvc` referenzia `ExamNavigator.Application`;
- `Program.cs` del client WinForms carica, se presente, un file `.ini`, applica i default verso `Predefiniti_*` e costruisce un `SqlServerExamNavigationService` con connection string SQL Server letta da variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- `Form1` mantiene ancora un costruttore parameterless che delega a `BootstrapNavigationService`; questo percorso resta raggiungibile come fallback legacy in-memory, pur non coincidendo con il bootstrap runtime principale del client desktop;
- `ExamNavigator.WinForms` contiene `Predefiniti_Ricerca` come contenitore statico dei default di ricerca, consumato dal bootstrap runtime per la baseline configurabile della ricerca;
- `ExamNavigator.WinForms` contiene `IniConfigurationDocument` come parser raw del file `.ini`;
- `ExamNavigator.WinForms` contiene `IniConfigurationBinder` come binder riflessivo type-safe dei default verso `Predefiniti_*`, wired nel bootstrap runtime per la baseline configurabile della ricerca;
- `Form1` inizializza la ricerca dai default configurati e usa `IExamNavigationService` per popolare all’avvio i tre pannelli;
- la selezione dell’ambulatorio aggiorna parti del corpo ed esami;
- la selezione della parte del corpo aggiorna gli esami;
- la ricerca testuale filtra i tre pannelli sul runtime SQL Server concreto tramite `SearchText` e `SearchField`, con attivazione da pulsante `Cerca`, tasto Invio e reset `Vedi tutti`;
- la conferma della selezione aggiunge una riga alla griglia riepilogativa;
- la rimozione della riga selezionata elimina elementi già confermati dalla griglia riepilogativa;
- lo spostamento su e giù riordina di una posizione la riga selezionata nella griglia riepilogativa mantenendo la selezione sul record spostato;
- `Program.cs` dell'host MVC registra `SqlServerExamNavigationService` nel container DI e legge la connection string SQL Server dalla variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- `HomeController` usa `IExamNavigationService` per risolvere la navigazione a cascata, mappa il risultato in `ExamNavigationPageViewModel`, gestisce i comandi web di conferma selezione/riordino/eliminazione e rende `Index` completo o `_ExamNavigationPage` come fragment in base al tipo di richiesta;
- `Index.cshtml` funge da shell MVC con root `#exam-navigation-root` e script `fetch`-based che intercetta ricerca GET, navigazione dei pannelli e submit dei form per aggiornare la UI in modo incrementale;
- `_ExamNavigationPage.cshtml` rende il fragment reale della pagina MVC con ricerca, pannelli, conferma selezione e griglia `Esami selezionati`;
- `_Layout.cshtml` e `site.css` completano la shell visiva dedicata dell’host MVC;
- la baseline dati attiva del runtime corrente vive in `database/sql/*`;
- gli artefatti `database/postgresql/*` restano presenti come heritage/demo track del pivot precedente e non governano più il wiring runtime attivo.

In altre parole, la codebase possiede oggi:
- modello dominio;
- boundary applicativo;
- baseline dati SQL Server di riferimento;
- adapter SQL Server concreto eseguibile nel layer infrastructure dedicato;
- adapter PostgreSQL legacy/reference mantenuto nel repository per tracciabilità tecnica;
- host WinForms compilabile e wired al runtime SQL Server concreto;
- host MVC compilabile e wired allo stesso runtime SQL Server concreto, con baseline funzionale equivalente al client WinForms e navigazione incrementale a fragment per le interazioni ad alta frequenza.

Non possiede ancora:
- test project e quality tooling dedicato ancora assenti dalla codebase;
- track qualità con test, lint, coverage e smoke automatizzati.

### Flusso runtime concreto corrente

Host desktop/web
-> Application (`IExamNavigationService`)
-> Infrastructure SQL Server concreta
-> SQL Server (`database/sql` come baseline dati e query di riferimento)
-> `ExamNavigationResult`
-> rendering host

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
- la permanenza nel repository del precedente track PostgreSQL può generare ambiguità di lettura se gli owner docs non restano esplicitamente allineati al fatto che esso non governa più il runtime attivo corrente;
- configurazione `.ini` oggi limitata alla baseline della ricerca;
- `BootstrapNavigationService` è ancora presente nel client WinForms come fallback legacy in-memory raggiungibile tramite il costruttore parameterless `Form1()`; non coincide con il bootstrap runtime principale, ma la sua presenza può ancora generare ambiguità di audit o lettura architetturale finché non viene governato o rimosso in modo esplicito;
- nessun progetto test presente;
- nessun lint / coverage / smoke automatizzato presente;
- il blocco `H` è chiuso sul piano del preflight/demo locale controllata; il blocco `I` è chiuso sul piano del consolidamento documentale tecnico V1; il blocco attivo corrente è `J — Promozione formale V1 / freeze su main`, mentre merge su `main`, tag e release restano ancora non eseguiti;
- possibile drift documentale se gli owner docs non restano esplicitamente allineati al fatto che i requisiti sorgente sono locali e non versionati.

---

## Linee guida per l’estensione del sistema

- le implementazioni concrete di `IExamNavigationService` devono continuare a vivere fuori da `Application`, in layer infrastructure dedicati;
- `src/ExamNavigator.Infrastructure.SqlServer` è il layer infrastructure concreto attivo del runtime corrente e deve restare il punto di verità per il wiring SQL Server degli host;
- `src/ExamNavigator.Infrastructure.PostgreSql` resta una track legacy/reference: non deve tornare a governare implicitamente il runtime attivo senza una decisione esplicita e documentata;
- il client WinForms deve continuare a orchestrare il caso d’uso tramite `Application`, senza inglobare logica dati nel form;
- l'host MVC deve continuare a riusare lo stesso perimetro applicativo del client WinForms, evitando duplicazione di logica;
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
