## Branch: [development]

### [Unreleased]
> Scope corrente: **runtime SQL Server concreto consolidato anche a smoke reale desktop WinForms, con bootstrap locale SQL Server documentato e blocco H riattivato come percorso corrente**

#### G6 — SQL Server runtime conformance closure
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`2354e54` — docs(sqlserver): add bootstrap guide and enrich demo seed**
  - **Type:** ADDED · **Categoria:** Database/SQL Server Bootstrap
  - **Cosa cambia:** versiona `database/sql/sqlserver.md` come documento tecnico di bootstrap locale SQL Server e amplia `database/sql/002_seed.sql` con ulteriori ambulatori, esami e relazioni per una demo locale più ricca.
  - **Impatto:** consolida la superficie bootstrap locale SQL Server usata dalla demo e completa in modo truth-first gli artefatti dati/documentali del blocco `G6`.


- **`d366e18` — fix(winforms): complete sql server runtime closure for desktop host**
  - **Type:** FIXED · **Categoria:** WinForms/Runtime/SQL Server
  - **Cosa cambia:** completa la runtime closure desktop del client WinForms per il percorso SQL Server concreto, aggiungendo nel `.csproj` i package e la copia runtime delle dipendenze transitive richieste da `Microsoft.Data.SqlClient` (`Microsoft.Data.SqlClient.Extensions.Abstractions`, `Microsoft.Data.SqlClient.Internal.Logging`, `System.Buffers`, `System.Memory`, `System.Numerics.Vectors`, `System.Runtime.CompilerServices.Unsafe`, `System.Threading.Tasks.Extensions`) con fallback `net462` / `net461` / `netstandard2.0`.
  - **Impatto:** chiude il gap residuo emerso nello smoke reale WinForms del runtime SQL Server concreto e rende l'eseguibile desktop avviabile con `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`.


- **`d1fa273` — docs(project): realign owner docs after sql server runtime closure**
  - **Type:** CHANGED · **Categoria:** Docs/Governance
  - **Cosa cambia:** aggiorna i documenti owner impattati dal passaggio al runtime SQL Server concreto per riflettere lo stato reale post-push su `development`, chiudendo semanticamente il docs sync gate rimasto aperto dopo `e42a783`.
  - **Impatto:** completa il riallineamento truth-first del blocco `G6`, registra che `origin/development` contiene ora sia il wiring SQL Server concreto sia il commit documentale di chiusura, e sblocca `H` come blocco operativo corrente senza anticipare ancora tag, merge su `main` o release.

- **`e42a783` — feat(sqlserver): wire concrete sql server runtime across hosts**
  - **Type:** ADDED · **Categoria:** Infrastructure/Runtime
  - **Cosa cambia:** introduce `src/ExamNavigator.Infrastructure.SqlServer` con `SqlServerExamNavigationService`, aggiunge il nuovo progetto alla solution, riallinea entrambi gli host `WinForms` e `MVC` al runtime SQL Server concreto tramite `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`, sostituisce i `ProjectReference` host dal layer PostgreSQL al nuovo layer SQL Server e governa nel client desktop la runtime closure di `Microsoft.Data.SqlClient`.
  - **Impatto:** materializza a codice il gap mission-critical `G6` sul runtime SQL Server concreto, elimina la dipendenza runtime PostgreSQL dagli host applicativi, mantiene `Domain` e `Application` invariati e sposta il blocco attivo dal solo implementation work al docs sync gate finale prima dello sblocco di `H`.

- **`15121d2` — docs(project): reopen mission-critical flow to sql server runtime conformance**
  - **Type:** CHANGED · **Categoria:** Docs/Governance
  - **Cosa cambia:** aggiorna `README.md`, `docs/TIMELINE.md`, `docs/ROADMAP.md` e `docs/ARCHITECTURE.md` per riclassificare truth-first il gap residuo della missione come `G6`, cioè introduzione di un runtime SQL Server concreto prima della promozione finale.
  - **Impatto:** sospende il blocco `H` come percorso già preflightrato ma non ancora sbloccato, impedisce merge/tag/release prematuri e riallinea la governance al fatto che il bundle PostgreSQL è oggi una baseline demo difendibile ma non ancora la chiusura letterale della missione.

#### G5 — Final conformance & coherence gate pre-consegna
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`1af6bf0` — docs(project): realign owner docs after winforms legacy fallback audit**
  - **Type:** CHANGED · **Categoria:** Docs/Governance
  - **Cosa cambia:** aggiorna `README.md`, `docs/TIMELINE.md`, `docs/ROADMAP.md` e `docs/ARCHITECTURE.md` per classificare `BootstrapNavigationService` WinForms come fallback legacy in-memory ancora raggiungibile tramite il costruttore parameterless `Form1()`, ma non coincidente con il bootstrap runtime principale basato su `PostgreSqlExamNavigationService`.
  - **Impatto:** chiude il docs sync gate successivo all’audit `G5.4`, elimina l’ambiguità documentale sul residuo legacy WinForms e riallinea la documentazione owner al fatto che il gate `G5` è chiuso e che il prossimo blocco corretto diventa `H`, senza anticipare ancora merge su `main`, tag o release.

- **`39e3bdd` — fix(mvc): replace full-page reload with ajax fragment navigation**
  - **Type:** FIXED · **Categoria:** MVC/UX
  - **Cosa cambia:** aggiorna `src/ExamNavigator.Mvc/Controllers/HomeController.cs`, `src/ExamNavigator.Mvc/Views/Home/Index.cshtml` e introduce `src/ExamNavigator.Mvc/Views/Home/_ExamNavigationPage.cshtml`, separando la pagina web in shell + fragment e aggiungendo navigazione incrementale `fetch`-based con rendering parziale lato controller.
  - **Impatto:** chiude `G5.3`, elimina il salto della viewport su liste lunghe nell’host MVC, sostituisce il full-page reload interattivo con aggiornamenti asincroni di ricerca/pannelli/griglia e mantiene intatto il boundary applicativo condiviso.


- **`cfee331` — feat(database): extend postgresql seed with mixed legacy and normalized demo data**
  - **Type:** ADDED · **Categoria:** Database/Seed
  - **Cosa cambia:** aggiorna `database/postgresql/002_seed.sql` introducendo un secondo blocco di popolamento con nuove parti del corpo, nuovi ambulatori e nuovi esami demo, costruiti in modo eterogeneo per coprire sia label legacy/non normalizzati sia naming più professionali e plausibili.
  - **Impatto:** chiude il lavoro tecnico di audit su dati demo, naming e abbreviazioni del gate `G5.2`, rende il seed PostgreSQL più adatto a verificare i casi di normalizzazione/presentazione cross-host e migliora la difendibilità della baseline demo pre-consegna.

- **`de03d95` — fix(mvc): normalize room labels in web navigation**
  - **Type:** FIXED · **Categoria:** MVC/Presentation
  - **Cosa cambia:** aggiorna `src/ExamNavigator.Mvc/Controllers/HomeController.cs` per normalizzare i label degli ambulatori nel page model MVC e nelle righe deserializzate della griglia `Esami selezionati`, riallineando il web al comportamento presentazionale già presente nel client WinForms.
  - **Impatto:** chiude il primo mismatch reale del final conformance/coherence gate post-freeze, elimina la visualizzazione raw di label come `EcografiaMassimino`, `EcografiaPrivitera`, `EcografiaDoppler`, `Tac1` e `Tac2` nell’host MVC e mantiene invariato il boundary applicativo/infrastrutturale condiviso.

#### G — PostgreSQL runtime concrete wiring
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`95ba1df` — feat(mvc): wire postgresql runtime**
  - **Type:** ADDED · **Categoria:** MVC/PostgreSQL Runtime
  - **Cosa cambia:** aggiorna `src/ExamNavigator.Mvc/Program.cs` e `src/ExamNavigator.Mvc/ExamNavigator.Mvc.csproj` per rimuovere `BootstrapNavigationService`, referenziare `ExamNavigator.Infrastructure.PostgreSql`, registrare `PostgreSqlExamNavigationService` nel container DI e costruire la connection string locale PostgreSQL con password letta da `EXAM_NAVIGATOR_PG_PASSWORD`.
  - **Impatto:** chiude `G3` a codice e a smoke runtime, riallinea l'host MVC alla stessa sorgente dati concreta del client WinForms e lascia aperta solo la verifica formale finale `G4`.

- **`56d3129` — feat(winforms): wire postgresql runtime and normalize desktop exam presentation**
  - **Type:** ADDED · **Categoria:** WinForms/PostgreSQL Runtime
  - **Cosa cambia:** aggiorna `Program.cs`, `ExamNavigator.WinForms.csproj`, `App.config`, `Form1.cs` e `PostgreSqlExamNavigationService.cs` per agganciare il runtime reale WinForms al PostgreSQL locale, governare in modo deterministico la runtime closure necessaria a `Npgsql`, introdurre binding redirects espliciti, tipizzare i parametri Npgsql nullabili e migliorare leggibilità/allineamento UI del pannello `Esami`, delle label di ricerca e dei nomi ambulatorio.
  - **Impatto:** chiude `G2` a codice e a verifica forte sul client desktop, lasciando come prossimo blocco corretto il wiring MVC (`G3`).

- **`6166345` — feat(infrastructure): add postgresql exam navigation service**
  - **Type:** ADDED · **Categoria:** Infrastructure/PostgreSQL
  - **Cosa cambia:** introduce `src/ExamNavigator.Infrastructure.PostgreSql`, lo aggiunge alla solution, lo collega a `ExamNavigator.Application`, aggiunge `Npgsql 8.0.8` e materializza `PostgreSqlExamNavigationService` con query PostgreSQL reali per ambulatori, parti del corpo ed esami e fallback di selezione nel boundary infrastrutturale.
  - **Impatto:** chiude il baseline codice di `G1`, rende presente un adapter PostgreSQL concreto separato da `Domain` e `Application`, e lascia deliberatamente differito il wiring host ai blocchi `G2` e `G3`.

- **`3a95d60` — docs(db): add postgresql local runtime bootstrap artifacts**
  - **Type:** ADDED · **Categoria:** Database/PostgreSQL
  - **Cosa cambia:** introduce `database/postgresql/001_schema.sql`, `database/postgresql/002_seed.sql` e `database/postgresql/postgresql.md`, congelando il setup locale PostgreSQL, la traduzione dello schema/seed e la decisione tecnica di adottare PostgreSQL come runtime locale.
  - **Impatto:** archivia il percorso SQL Server/LocalDB esplorato come non proseguito, rende auditabile il pivot runtime e apre il successivo wiring applicativo verso PostgreSQL concreto.

#### F — MVC conversion
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`4c45148` — feat(mvc): complete web exam navigation baseline**
  - **Type:** ADDED · **Categoria:** MVC/Web Host
  - **Cosa cambia:** aggiorna `Program.cs`, `HomeController.cs`, `Models/ErrorViewModel.cs`, `Views/Home/Index.cshtml`, `Views/Shared/_Layout.cshtml` e `wwwroot/css/site.css` per completare la baseline web con conferma selezione, griglia riepilogativa, riordino, eliminazione riga, naming demo più leggibile e shell UI/UX coerente col progetto.
  - **Impatto:** chiude `F2` e porta l'host MVC a una baseline demo funzionale coerente con il client WinForms, pur lasciando ancora differiti adapter SQL concreto e track qualità.

- **`af3711c` — feat(mvc): wire baseline exam navigation page**
  - **Type:** ADDED · **Categoria:** MVC/Web Host
  - **Cosa cambia:** aggiorna `Program.cs`, `HomeController.cs`, `Models/ErrorViewModel.cs` e `Views/Home/Index.cshtml` per registrare un `BootstrapNavigationService` MVC in DI, introdurre un page view model dedicato e renderizzare una prima pagina di navigazione esami con ricerca GET e sezioni `Ambulatori` / `Parti del corpo` / `Esami`.
  - **Impatto:** chiude `F1` con il primo riallineamento funzionale dell'host web al core condiviso, superando lo scaffold demo e preparando `F2` senza introdurre ancora adapter SQL, griglia web o parità completa col client WinForms.

- **`c1ba47f` — feat(mvc): add baseline aspnet core mvc host**
  - **Type:** ADDED · **Categoria:** MVC/Web Host
  - **Cosa cambia:** introduce `src/ExamNavigator.Mvc` come nuovo host ASP.NET Core MVC, lo aggiunge alla solution e lo collega al core condiviso tramite reference a `ExamNavigator.Application`.
  - **Impatto:** apre il ciclo di conversione web con un host dedicato già compilabile, separato dal client WinForms e pronto ai successivi allineamenti funzionali.

#### E — Search and configuration
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`8344dcc` — feat(winforms): wire configurable search defaults at startup**
  - **Type:** ADDED · **Categoria:** WinForms/Configuration
  - **Cosa cambia:** aggiorna `Program.cs` e `Form1.cs` per caricare all'avvio un file `.ini`, applicare i default tramite `IniConfigurationBinder` e inizializzare la ricerca WinForms con `Predefiniti_Ricerca.SearchText` e `Predefiniti_Ricerca.SearchField`.
  - **Impatto:** chiude la baseline runtime della ricerca configurabile richiesta dalla missione `.ini`, mantenendo ancora separati adapter SQL concreto, test/quality track e conversione MVC.

- **`c1c6170` — feat(winforms): add reflective ini configuration binder**
  - **Type:** ADDED · **Categoria:** WinForms/Configuration
  - **Cosa cambia:** introduce `IniConfigurationBinder` nel progetto `ExamNavigator.WinForms` e aggiorna il `.csproj` per includerlo nel build, aggiungendo il binding riflessivo type-safe dal documento `.ini` verso le classi statiche `Predefiniti_*`.
  - **Impatto:** chiude il blocco tecnico del binder configurativo richiesto dalla missione `.ini`, mantenendo ancora separato e differito il consumo runtime dei default nel bootstrap/UI.

- **`8e1fa21` — feat(winforms): add raw ini configuration document parser**
  - **Type:** ADDED · **Categoria:** WinForms/Configuration
  - **Cosa cambia:** introduce `IniConfigurationDocument` nel progetto `ExamNavigator.WinForms` e aggiorna il `.csproj` per includerlo nel build, aggiungendo il parsing raw di sezioni e coppie `chiave = valore` dal file `.ini`.
  - **Impatto:** consolida il primo boundary di lettura del documento `.ini` e prepara il successivo binding riflessivo verso le classi `Predefiniti_*` senza consumare ancora i default nel bootstrap runtime.

- **`b0ef0c2` — feat(winforms): add static search defaults container**
  - **Type:** ADDED · **Categoria:** WinForms/Configuration
  - **Cosa cambia:** introduce `Predefiniti_Ricerca` come classe statica nel progetto `ExamNavigator.WinForms` e aggiorna il `.csproj` per includerla nel build, centralizzando i default di `SearchText` e `SearchField`.
  - **Impatto:** apre la prima fondazione concreta per la configurazione della ricerca e prepara il successivo loader `.ini` riflessivo senza cambiare ancora bootstrap runtime, `Form1` o adapter SQL.

- **`05fff07` — feat(winforms): wire exam search actions**
  - **Type:** ADDED · **Categoria:** WinForms/Desktop UI
  - **Cosa cambia:** aggancia nel `Form1` gli eventi `btnSearch.Click`, `btnClearSearch.Click` e `txtSearchTerm.KeyDown`, introducendo gli handler dedicati e il metodo `ApplySearch()` per applicare il filtro tramite `ExamNavigationRequest`.
  - **Impatto:** chiude il baseline di wiring UI della ricerca desktop, rendendo operativi il pulsante `Cerca`, il tasto Invio e il reset `Vedi tutti` sul dataset bootstrap senza introdurre ancora parser `.ini` né adapter SQL concreto.

#### D — WinForms implementation
> Ordinamento: **git log (più recente → più vecchio)** · principio **truth-first**: qui è riportato solo ciò che è committato.

- **`bb85c7a` — feat(winforms): add selection grid row reorder actions**
  - **Type:** ADDED · **Categoria:** WinForms/Desktop UI
  - **Cosa cambia:** aggancia `btnMoveUp` e `btnMoveDown` nel wiring degli eventi di `Form1` e introduce gli handler `BtnMoveUp_Click(...)` e `BtnMoveDown_Click(...)` per riordinare di una posizione la riga selezionata nella `dgvSelectedExams`.
  - **Impatto:** chiude il blocco `D3`, rendendo operativa la gestione completa delle righe della griglia tramite cancellazione e riordinamento `move up / move down`.

- **`6d50495` — feat(winforms): add selected row deletion to exam grid**
  - **Type:** ADDED · **Categoria:** WinForms/Desktop UI
  - **Cosa cambia:** aggancia `btnRemoveSelected` nel wiring degli eventi di `Form1` e introduce l’handler `BtnRemoveSelected_Click(...)` per rimuovere dalla `dgvSelectedExams` le righe selezionate non placeholder.
  - **Impatto:** chiude la prima metà del blocco `D3`, rendendo operativa la cancellazione delle righe nella griglia riepilogativa e lasciando aperto solo il riordinamento `move up / move down`.

- **`472bed4` — feat(winforms): add selection confirmation grid append**
  - **Type:** ADDED · **Categoria:** WinForms/Desktop UI
  - **Cosa cambia:** aggancia il pulsante di conferma selezione e aggiunge alla `dgvSelectedExams` una riga per ogni esame confermato, riportando codice ministeriale, codice interno, descrizione, parte del corpo e ambulatorio correnti.
  - **Impatto:** rende operativa la griglia riepilogativa come accumulatore delle selezioni confermate e chiude il baseline della conferma selezione lato host desktop, lasciando ancora aperti solo delete/reorder, ricerca wired e persistenza SQL concreta.

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
