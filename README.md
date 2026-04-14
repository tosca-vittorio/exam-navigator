# Exam Navigator System

Progetto tecnico realizzato come test di programmazione.

## Obiettivo

Realizzare un sistema desktop WinForms per la selezione guidata di esami medici, basato su:

- elenco ambulatori;
- elenco parti del corpo;
- elenco esami;
- selezione a cascata;
- ricerca testuale case-insensitive;
- griglia di riepilogo delle scelte;
- caricamento opzionale di configurazione `.ini`.

Il progetto viene impostato fin dall’inizio in modo da essere convertibile a web tramite ASP.NET Core MVC, evitando logiche applicative duplicate e preservando un core condiviso tra dominio, contratti applicativi e host futuri.

## Stato corrente della codebase

Baseline attuale verificata:

- solution `ExamNavigator.sln` presente;
- progetto `ExamNavigator.Domain` presente con entità minime del dominio;
- progetto `ExamNavigator.Application` presente con contratti di navigazione e interfaccia applicativa;
- progetto `ExamNavigator.WinForms` presente con host desktop wired al boundary `Application`, runtime SQL Server concreto tramite `SqlServerExamNavigationService`, ricerca testuale wired (pulsante `Cerca`, tasto Invio e reset `Vedi tutti`), append delle selezioni confermate nella griglia riepilogativa, cancellazione della riga selezionata, riordinamento `move up / move down`, primo contenitore statico `Predefiniti_Ricerca` per i default della ricerca, parser raw `IniConfigurationDocument` per il documento `.ini`, binder riflessivo type-safe `IniConfigurationBinder` verso `Predefiniti_*`, normalizzazione label degli ambulatori, etichette leggibili della ricerca e presentazione multi-line del pannello `Esami`;
- baseline database presente con:
  - baseline SQL Server attiva in `database/sql`:
    - `001_schema.sql`
    - `002_seed.sql`
    - `003_navigation_queries.sql`
    - `sqlserver.md`
  - artefatti PostgreSQL locali heritage/demo in `database/postgresql`:
    - `001_schema.sql`
    - `002_seed.sql`
    - `postgresql.md`
- progetto `ExamNavigator.Infrastructure.PostgreSql` ancora presente nel repository come track infrastructure legacy/reference non più usata dal wiring runtime attivo degli host;
- progetto `ExamNavigator.Infrastructure.SqlServer` presente con adapter SQL Server concreto `SqlServerExamNavigationService`, query reali per ambulatori, parti del corpo ed esami, fallback di selezione `SelectedRoomId` / `SelectedBodyPartId` e parametri `SqlDbType` tipizzati; host WinForms ora wired al runtime SQL Server concreto tramite `Program.cs`, con runtime closure desktop di `Microsoft.Data.SqlClient` governata dal `.csproj`, binding redirects espliciti in `App.config`, normalizzazione label degli ambulatori, etichette leggibili della ricerca e presentazione multi-line più leggibile del pannello `Esami`; host MVC ora wired allo stesso runtime SQL Server concreto tramite `Program.cs`, con reference esplicito a `ExamNavigator.Infrastructure.SqlServer` e connection string letta da variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`; test, lint e coverage non ancora introdotti nella codebase; il caricamento runtime dei default da configurazione e il consumo runtime dei default nel bootstrap/UI sono presenti per la baseline della ricerca; l'host MVC mantiene la baseline web funzionale completa per navigazione esami, ricerca GET, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX, ora alimentata dalla stessa sorgente dati SQL Server concreta del client WinForms, con normalizzazione dei label degli ambulatori anche lato controller MVC nella navigazione web e nella griglia `Esami selezionati`, e con navigazione incrementale a fragment (`Index.cshtml` shell + `_ExamNavigationPage.cshtml` + `fetch`/partial rendering) per ricerca, selezione pannelli e comandi griglia, eliminando il full-page reload interattivo e il salto viewport su liste lunghe.

## Scelte tecniche correnti

- Desktop UI target: WinForms
- Target desktop previsto: .NET Framework 4.8
- Layer condivisi correnti: `netstandard2.0`
- Database runtime locale attivo: SQL Server
- Baseline dati di riferimento: SQL Server
- Artefatti PostgreSQL locali: presenti come heritage/demo track non più usata dal wiring runtime attivo degli host
- Accesso dati previsto: infrastructure SQL Server dedicata + servizi applicativi
- Web conversion target: ASP.NET Core MVC
- Architettura: core condiviso + host desktop + host web

## Strategia architetturale

La soluzione è governata per strati:

1. **Domain**
   - entità di dominio minime;
   - modello dati indipendente da UI e persistenza.

2. **Application**
   - contratti di input/output per la navigazione;
   - interfacce di servizio applicativo;
   - orchestrazione della cascata e della ricerca.

3. **Database / Infrastructure baseline**
   - baseline SQL Server presente come perimetro dati attivo del runtime corrente;
   - artefatti PostgreSQL locali mantenuti come heritage/demo track del pivot tecnico precedente;
   - progetto `ExamNavigator.Infrastructure.SqlServer` presente come layer infrastructure attivo, separato da `Domain` e `Application`, con `SqlServerExamNavigationService` come implementazione concreta del boundary applicativo sul runtime SQL Server;
   - progetto `ExamNavigator.Infrastructure.PostgreSql` mantenuto nel repository come track legacy/reference non più usata dal wiring runtime attivo degli host.

4. **Host WinForms**
   - interfaccia desktop richiesta dalla missione;
   - wiring UI -> Application;
   - runtime entrypoint wired al SQL Server concreto tramite `Program.cs` e variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`, con normalizzazione label e pannello `Esami` più leggibile.

5. **Host Web**
   - host ASP.NET Core MVC presente nella solution e referenziato al core condiviso;
   - baseline funzionale web equivalente alla demo WinForms introdotta tramite controller dedicato e page view model dedicato;
   - ricerca GET, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX presenti;
   - wiring runtime agganciato a `SqlServerExamNavigationService` tramite `Program.cs`, con sorgente dati SQL Server concreta condivisa col client WinForms e connection string letta da variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
   - shell MVC + fragment partial + navigazione `fetch` incrementale presenti per evitare full-page reload durante le interazioni ad alta frequenza dell’host web.

## Repository layout

- `ExamNavigator.sln` — solution root del progetto
- `src/ExamNavigator.Domain` — entità di dominio minime
- `src/ExamNavigator.Application` — contratti applicativi e interfaccia di servizio
- `src/ExamNavigator.Infrastructure.SqlServer` — infrastructure SQL Server concreta con adapter di navigazione esami
- `src/ExamNavigator.Infrastructure.PostgreSql` — infrastructure PostgreSQL legacy/reference non più usata dal wiring runtime attivo degli host
- `src/ExamNavigator.WinForms` — host desktop WinForms wired al runtime SQL Server concreto
- `src/ExamNavigator.Mvc` — host web ASP.NET Core MVC wired al runtime SQL Server concreto
- `database/sql` — schema, seed, query di riferimento e documento tecnico SQL Server del runtime attivo
- `database/postgresql` — artefatti PostgreSQL heritage/demo (schema, seed, documento tecnico)
- `docs/TIMELINE.md` — source of truth operativa
- `docs/CHANGELOG.md` — tracciabilità evolutiva
- `docs/ROADMAP.md` — traiettoria e milestone
- `docs/ARCHITECTURE.md` — fotografia AS-IS della struttura corrente

## Avvio rapido e verifica manuale

Prerequisiti minimi:
- istanza SQL Server locale disponibile;
- schema e seed SQL Server già applicati al database target;
- variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING` valorizzata con una connection string SQL Server valida per gli host applicativi.

### Avvio host web MVC

```bash
export EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING='Server=(localdb)\MSSQLLocalDB;Database=ExamNavigator;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;'
dotnet run --project src/ExamNavigator.Mvc --urls http://localhost:5099
```

Endpoint locale usato abitualmente:

* `http://localhost:5099`

### Avvio client desktop WinForms

```bash
export EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING='Server=(localdb)\MSSQLLocalDB;Database=ExamNavigator;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;'
./src/ExamNavigator.WinForms/bin/Debug/ExamNavigator.WinForms.exe
```

### Verifica manuale minima consigliata

Verificare almeno i seguenti punti:

* caricamento iniziale corretto di ambulatori, parti del corpo ed esami;
* aggiornamento a cascata dei tre pannelli;
* ricerca testuale con pulsante `Cerca`;
* ricerca testuale con pressione del tasto `Invio`;
* reset della ricerca con `Vedi tutti`;
* conferma selezione esame nella griglia riepilogativa;
* riordino righe con `Sposta su` / `Sposta giù`;
* eliminazione riga selezionata;
* resa coerente dei label ambulatori sia lato WinForms sia lato MVC anche in presenza di dati legacy/non normalizzati;
* assenza di salto viewport nell’host MVC durante selezione su liste lunghe, ricerca e azioni della griglia, grazie alla navigazione incrementale a fragment.

### Nota sul perimetro qualità corrente

Alla data attuale il repository espone come gate verificabile minimo:

```bash
dotnet build ExamNavigator.sln
```

Lint, test unitari, coverage e smoke automatizzati restano tracciati come EXTRA non ancora introdotti nella codebase.

## Perimetro del bundle demo locale controllato

Alla data attuale, il bundle demo locale controllato è composto da questi elementi realmente materializzati nel repository e nei build output locali:

- **host demo primario:** `src/ExamNavigator.WinForms/bin/Debug/ExamNavigator.WinForms.exe` con relativa runtime closure `.NET Framework` / `Microsoft.Data.SqlClient`;
- **host demo secondario:** `src/ExamNavigator.Mvc/bin/Debug/net9.0/ExamNavigator.Mvc.exe` con relativi artifact runtime ASP.NET Core MVC;
- **bootstrap runtime locale attivo:** `database/sql/001_schema.sql`, `database/sql/002_seed.sql`, `database/sql/003_navigation_queries.sql`, `database/sql/sqlserver.md`;
- **artefatti PostgreSQL heritage/demo:** `database/postgresql/001_schema.sql`, `database/postgresql/002_seed.sql`, `database/postgresql/postgresql.md`;
- **contratto operativo di demo:** prerequisiti, comandi di avvio e checklist di verifica manuale documentati in questo `README.md`.

Questo perimetro non coincide ancora con una release formale, con un installer o con un archivio di consegna definitivo: rappresenta la superficie demo oggi più difendibile e immediatamente eseguibile in ambiente locale controllato.

## Sequenza operativa raccomandata per la demo

Per una demo locale controllata, il flusso raccomandato è il seguente:

1. verificare che l'istanza SQL Server locale sia disponibile;
2. applicare schema, seed e query di riferimento SQL Server;
3. valorizzare la variabile ambiente `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
4. avviare **prima** il client WinForms come host demo primario;
5. verificare caricamento iniziale, cascata, ricerca, conferma selezione, riordino ed eliminazione nella griglia;
6. avviare **poi** l'host MVC come dimostrazione secondaria della convertibilità web già implementata;
7. verificare nell'host MVC gli stessi punti funzionali essenziali, con attenzione anche all'assenza di full-page reload e salto viewport durante le interazioni.

Questo ordine è quello oggi più difendibile perché mostra prima l'host desktop richiesto dalla missione e poi l'host web come estensione concreta della stessa baseline applicativa e dello stesso runtime SQL Server.

## Privacy e fonti requisito

Le fonti requisito originali e il freeze requisito sorgente sono mantenuti localmente in:

- `docs/target/requirements/01_original_mission.md`
- `docs/target/requirements/02_requirements_freeze.md`

Questi file **non sono versionati nel repository pubblico** per ragioni di privacy.  
Gli owner docs versionati devono quindi restare coerenti con il fatto che tali sorgenti esistono nel working environment locale, ma non fanno parte della superficie pubblica del repo.

## Stato missione

Stato corrente della missione principale:

1. bootstrap repository + governance documentale → completato;
2. core condiviso (`Domain` + `Application`) → completato;
3. baseline database di riferimento SQL Server (`schema` + `seed` + `query`) + artefatti PostgreSQL locali heritage/demo → completata;
4. host WinForms baseline (`bootstrap progetto + layout statico form`) → completato;
5. wiring desktop iniziale della cascata (`Application` boundary + bootstrap service locale + aggiornamento ambulatorio/parte del corpo/esami) → completato;
6. blocco ricerca desktop baseline (`wiring` UI) → completato;
7. blocchi successivi → configurazione `.ini` avanzata con fondazione dei default di ricerca, parser raw del documento, binder riflessivo type-safe e wiring runtime della baseline di ricerca completati; conversione web MVC archiviata come baseline demo con host dedicato (`F0`), primo riallineamento funzionale di controller/view model (`F1`) e UI web equivalente con griglia selezioni e polish (`F2`) completati; bootstrap runtime locale PostgreSQL documentato e consolidato come heritage/demo track; blocchi `G1`-`G4` completati con introduzione di `ExamNavigator.Infrastructure.PostgreSql`, del service concreto `PostgreSqlExamNavigationService`, del wiring runtime concreto PostgreSQL dei due host e della verifica formale finale di chiusura V1; blocchi `G6.0`, `G6.1` e `G6.2` completati con introduzione di `ExamNavigator.Infrastructure.SqlServer`, del service concreto `SqlServerExamNavigationService`, del wiring runtime SQL Server concreto di entrambi gli host WinForms e MVC, del completamento della runtime closure desktop reale WinForms e del consolidamento del bootstrap locale SQL Server.

Il gap residuo per la chiusura letterale della missione non è più la materializzazione del runtime SQL Server né il docs sync gate owner, entrambi ormai chiusi sul branch `development`. Il blocco operativo corrente è ora `H — Preparazione consegna / rilascio / demo V1`; restano ancora sospesi, fino a successiva esecuzione esplicita, tag, merge su `main`, release e consegna finale.

## Perimetro V1

La V1 mission-critical coincide esclusivamente con i requisiti funzionali e non funzionali della mail, congelati nel freeze locale dei requisiti.

Per la V1 il criterio è:
- soluzione concreta;
- robusta, affidabile e solida;
- consegnabile, valutabile e difendibile;
- aderente ai requisiti della mail senza introdurre dipendenze dagli EXTRA.

Tutto ciò che non rientra in questo perimetro resta post-V1 oppure EXTRA congelato.


## Stato operativo corrente (truth-first)

La V1 mission-critical era stata formalmente chiusa nei documenti owner al checkpoint `3897979`, ma il successivo riallineamento truth-first ha classificato un gap residuo di conformità letterale verso il requisito SQL Server, tracciato come `G6`.

Dopo `G5.1` e `G5.2`, la validazione manuale reale dell’host MVC su liste lunghe aveva confermato un difetto UX aggiuntivo: la viewport tornava verso l’alto durante navigazione, ricerca e comandi della griglia. Il tentativo locale iniziale basato su scroll manuale e marcatori `data-viewport-*` è stato classificato come investigazione non consolidata e poi rimosso.

Il commit `39e3bdd` ha chiuso `G5.3` sostituendo il full-page reload interattivo dell’host MVC con una navigazione incrementale a fragment:
- `HomeController` rende `Index` o `_ExamNavigationPage` tramite `RenderNavigationPage(...)`, in base al tipo di richiesta;
- `Index.cshtml` è una shell MVC con root `#exam-navigation-root` e script `fetch` dedicato;
- `_ExamNavigationPage.cshtml` contiene il markup reale della pagina web;
- la ricerca, la navigazione dei pannelli e i comandi della griglia vengono aggiornati asincronicamente senza ricaricare l’intera pagina.

Fix consolidati del gate `G5`:
- commit `de03d95` — normalizzazione dei label degli ambulatori nell’host MVC, sia nella navigazione web sia nella griglia `Esami selezionati`;
- commit `cfee331` — estensione del seed PostgreSQL con dataset demo misto, comprendente baseline legacy/non normalizzata e nuovi dati più eterogenei, plausibili e professionalmente più coerenti per audit di naming, abbreviazioni e casi di normalizzazione;
- commit `39e3bdd` — eliminazione del salto viewport MVC su liste lunghe tramite fragment navigation AJAX e verifica manuale finale positiva sui cinque casi critici: `Ambulatori`, `Parti del corpo`, `Esami`, griglia selezioni e ricerca.

Il gate `G5.4` è stato chiarito truth-first: `BootstrapNavigationService` non è il runtime principale del client WinForms, ma un fallback legacy in-memory ancora raggiungibile tramite il costruttore parameterless `Form1()`. Il bootstrap reale del client desktop continua a passare da `Program.Main()` verso il service concreto e poi verso `new Form1(navigationService)`.

Con questa classificazione il gate `G5` risulta chiuso a docs sync consolidato. I preflight `H0`-`H2` restano validi come lavoro preparatorio di demo/consegna, ma non hanno sbloccato automaticamente la promozione finale.

Il commit `e42a783` ha chiuso il lavoro codice di `G6`, introducendo `ExamNavigator.Infrastructure.SqlServer`, il service concreto `SqlServerExamNavigationService` e il wiring runtime SQL Server concreto di entrambi gli host WinForms e MVC tramite `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`. Nel client desktop, il `.csproj` governa ora la runtime closure di `Microsoft.Data.SqlClient`, con verifica positiva dopo purge di `bin/` e `obj/`, rebuild verde e assenza di `Npgsql.dll` nel runtime output finale di WinForms.

La baseline funzionale attuale è quindi solida, verificata e dimostrabile su runtime SQL Server concreto. I commit `d366e18` (`fix(winforms): complete sql server runtime closure for desktop host`) e `2354e54` (`docs(sqlserver): add bootstrap guide and enrich demo seed`) hanno consolidato rispettivamente la runtime closure desktop reale del client WinForms e il bootstrap locale SQL Server della demo (`database/sql/sqlserver.md`, `database/sql/002_seed.sql`).

Con questo riallineamento truth-first, `G6` risulta chiuso anche rispetto alla runtime closure desktop reale del client WinForms e il blocco attivo corretto torna `H — Preparazione consegna / rilascio / demo V1`. Restano ancora sospesi, fino a successiva esecuzione esplicita, tag, merge su `main`, release e consegna finale.

## Documentazione owner

- `docs/TIMELINE.md` -> stato operativo reale
- `docs/CHANGELOG.md` -> tracciabilità evolutiva
- `docs/ROADMAP.md` -> traiettoria e milestone
- `docs/ARCHITECTURE.md` -> struttura reale del sistema
