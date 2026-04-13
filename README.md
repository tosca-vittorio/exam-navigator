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
- progetto `ExamNavigator.WinForms` presente con host desktop wired al boundary `Application`, runtime PostgreSQL concreto tramite `PostgreSqlExamNavigationService`, ricerca testuale wired (pulsante `Cerca`, tasto Invio e reset `Vedi tutti`), append delle selezioni confermate nella griglia riepilogativa, cancellazione della riga selezionata, riordinamento `move up / move down`, primo contenitore statico `Predefiniti_Ricerca` per i default della ricerca, parser raw `IniConfigurationDocument` per il documento `.ini`, binder riflessivo type-safe `IniConfigurationBinder` verso `Predefiniti_*`, normalizzazione label degli ambulatori, etichette leggibili della ricerca e presentazione multi-line del pannello `Esami`;
- baseline database presente con:
  - reference SQL Server in `database/sql`:
    - `001_schema.sql`
    - `002_seed.sql`
    - `003_navigation_queries.sql`
  - bootstrap runtime locale PostgreSQL in `database/postgresql`:
    - `001_schema.sql`
    - `002_seed.sql`
    - `postgresql.md`
- progetto `ExamNavigator.Infrastructure.PostgreSql` presente con adapter PostgreSQL concreto `PostgreSqlExamNavigationService`, query reali per ambulatori, parti del corpo ed esami, fallback di selezione `SelectedRoomId` / `SelectedBodyPartId` e parametri Npgsql tipizzati; host WinForms ora wired al runtime PostgreSQL concreto tramite `Program.cs`, con runtime closure `.NET Standard`/`Npgsql` governata dal `.csproj`, binding redirects espliciti in `App.config`, normalizzazione label degli ambulatori, etichette leggibili della ricerca e presentazione multi-line più leggibile del pannello `Esami`; host MVC ora wired al runtime PostgreSQL concreto tramite `Program.cs`, con reference esplicito a `ExamNavigator.Infrastructure.PostgreSql` e password letta da variabile ambiente `EXAM_NAVIGATOR_PG_PASSWORD`; test, lint e coverage non ancora introdotti nella codebase; il caricamento runtime dei default da configurazione e il consumo runtime dei default nel bootstrap/UI sono presenti per la baseline della ricerca; l'host MVC mantiene la baseline web funzionale completa per navigazione esami, ricerca GET, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX, ora alimentata dalla stessa sorgente dati PostgreSQL concreta del client WinForms, con normalizzazione dei label degli ambulatori anche lato controller MVC nella navigazione web e nella griglia `Esami selezionati`, e con navigazione incrementale a fragment (`Index.cshtml` shell + `_ExamNavigationPage.cshtml` + `fetch`/partial rendering) per ricerca, selezione pannelli e comandi griglia, eliminando il full-page reload interattivo e il salto viewport su liste lunghe.

## Scelte tecniche correnti

- Desktop UI target: WinForms
- Target desktop previsto: .NET Framework 4.8
- Layer condivisi correnti: `netstandard2.0`
- Database runtime locale: PostgreSQL
- Baseline SQL legacy/reference: SQL Server
- Accesso dati previsto: infrastructure PostgreSQL dedicata + servizi applicativi, con divergenza rispetto al requisito SQL Server originario esplicitamente documentata
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
   - baseline SQL Server mantenuta come reference storica/di compatibilità;
   - bootstrap runtime locale PostgreSQL presente con schema, seed e documento tecnico dedicato;
   - progetto `ExamNavigator.Infrastructure.PostgreSql` presente come layer infrastructure dedicato, separato da `Domain` e `Application`, con `PostgreSqlExamNavigationService` come prima implementazione concreta del boundary applicativo su runtime PostgreSQL.

4. **Host WinForms**
   - interfaccia desktop richiesta dalla missione;
   - wiring UI -> Application;
   - runtime entrypoint wired al PostgreSQL concreto, con normalizzazione label e pannello `Esami` più leggibile.

5. **Host Web**
   - host ASP.NET Core MVC presente nella solution e referenziato al core condiviso;
   - baseline funzionale web equivalente alla demo WinForms introdotta tramite controller dedicato e page view model dedicato;
   - ricerca GET, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX presenti;
   - wiring runtime ora agganciato a `PostgreSqlExamNavigationService` tramite `Program.cs`, con sorgente dati PostgreSQL concreta condivisa col client WinForms e password letta da variabile ambiente `EXAM_NAVIGATOR_PG_PASSWORD`;
   - shell MVC + fragment partial + navigazione `fetch` incrementale presenti per evitare full-page reload durante le interazioni ad alta frequenza dell’host web.

## Repository layout

- `ExamNavigator.sln` — solution root del progetto
- `src/ExamNavigator.Domain` — entità di dominio minime
- `src/ExamNavigator.Application` — contratti applicativi e interfaccia di servizio
- `src/ExamNavigator.Infrastructure.PostgreSql` — infrastructure PostgreSQL concreta con adapter di navigazione esami
- `src/ExamNavigator.WinForms` — host desktop WinForms baseline
- `src/ExamNavigator.Mvc` — host web ASP.NET Core MVC baseline
- `database/sql` — schema, seed e query SQL Server di riferimento
- `database/postgresql` — bootstrap runtime locale PostgreSQL (schema, seed, documento tecnico)
- `docs/TIMELINE.md` — source of truth operativa
- `docs/CHANGELOG.md` — tracciabilità evolutiva
- `docs/ROADMAP.md` — traiettoria e milestone
- `docs/ARCHITECTURE.md` — fotografia AS-IS della struttura corrente

## Avvio rapido e verifica manuale

Prerequisiti minimi:
- database PostgreSQL locale disponibile;
- schema e seed già applicati al database `exam_navigator`;
- variabile ambiente `EXAM_NAVIGATOR_PG_PASSWORD` valorizzata con la password dell'utente PostgreSQL applicativo.

### Avvio host web MVC

```bash
export EXAM_NAVIGATOR_PG_PASSWORD='<POSTGRESQL_PASSWORD>'
dotnet run --project src/ExamNavigator.Mvc --urls http://localhost:5099
```

Endpoint locale usato abitualmente:

* `http://localhost:5099`

### Avvio client desktop WinForms

```bash
export EXAM_NAVIGATOR_PG_PASSWORD='<POSTGRESQL_PASSWORD>'
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

- **host demo primario:** `src/ExamNavigator.WinForms/bin/Debug/ExamNavigator.WinForms.exe` con relativa runtime closure `.NET Framework` / `Npgsql`;
- **host demo secondario:** `src/ExamNavigator.Mvc/bin/Debug/net9.0/ExamNavigator.Mvc.exe` con relativi artifact runtime ASP.NET Core MVC;
- **bootstrap runtime locale attivo:** `database/postgresql/001_schema.sql`, `database/postgresql/002_seed.sql`, `database/postgresql/postgresql.md`;
- **reference heritage/importabile SQL Server:** `database/sql/001_schema.sql`, `database/sql/002_seed.sql`, `database/sql/003_navigation_queries.sql`;
- **contratto operativo di demo:** prerequisiti, comandi di avvio e checklist di verifica manuale documentati in questo `README.md`.

Questo perimetro non coincide ancora con una release formale, con un installer o con un archivio di consegna definitivo: rappresenta la superficie demo oggi più difendibile e immediatamente eseguibile in ambiente locale controllato.

## Sequenza operativa raccomandata per la demo

Per una demo locale controllata, il flusso raccomandato è il seguente:

1. verificare che PostgreSQL locale sia disponibile;
2. applicare schema e seed del bootstrap runtime locale PostgreSQL;
3. valorizzare la variabile ambiente `EXAM_NAVIGATOR_PG_PASSWORD`;
4. avviare **prima** il client WinForms come host demo primario;
5. verificare caricamento iniziale, cascata, ricerca, conferma selezione, riordino ed eliminazione nella griglia;
6. avviare **poi** l'host MVC come dimostrazione secondaria della convertibilità web già implementata;
7. verificare nell'host MVC gli stessi punti funzionali essenziali, con attenzione anche all'assenza di full-page reload e salto viewport durante le interazioni.

Questo ordine è quello oggi più difendibile perché mostra prima l'host desktop richiesto dalla missione e poi l'host web come estensione concreta della stessa baseline applicativa e dello stesso runtime PostgreSQL.

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
3. baseline database di riferimento SQL Server (`schema` + `seed` + `query`) + bootstrap runtime locale PostgreSQL → completata;
4. host WinForms baseline (`bootstrap progetto + layout statico form`) → completato;
5. wiring desktop iniziale della cascata (`Application` boundary + bootstrap service locale + aggiornamento ambulatorio/parte del corpo/esami) → completato;
6. blocco ricerca desktop baseline (`wiring` UI) → completato;
7. blocchi successivi → configurazione `.ini` avanzata con fondazione dei default di ricerca, parser raw del documento, binder riflessivo type-safe e wiring runtime della baseline di ricerca completati; conversione web MVC archiviata come baseline demo con host dedicato (`F0`), primo riallineamento funzionale di controller/view model (`F1`) e UI web equivalente con griglia selezioni e polish (`F2`); bootstrap runtime locale PostgreSQL documentato e consolidato; blocchi `G1`-`G4` completati con introduzione di `ExamNavigator.Infrastructure.PostgreSql`, del service concreto `PostgreSqlExamNavigationService`, del wiring runtime concreto di entrambi gli host WinForms e MVC e della verifica formale finale di chiusura V1; la divergenza rispetto al requisito SQL Server originario resta esplicitamente governata nella documentazione owner.

Il gap residuo per la chiusura letterale della missione è ora tracciato come `G6`: introduzione e wiring di un runtime SQL Server concreto.

## Perimetro V1

La V1 mission-critical coincide esclusivamente con i requisiti funzionali e non funzionali della mail, congelati nel freeze locale dei requisiti.

Per la V1 il criterio è:
- soluzione concreta;
- robusta, affidabile e solida;
- consegnabile, valutabile e difendibile;
- aderente ai requisiti della mail senza introdurre dipendenze dagli EXTRA.

Tutto ciò che non rientra in questo perimetro resta post-V1 oppure EXTRA congelato.


## Stato operativo corrente (truth-first)

La V1 mission-critical era stata formalmente chiusa nei documenti owner al checkpoint `3897979`, ma il successivo riallineamento truth-first ha classificato un gap residuo di conformità letterale verso il requisito SQL Server, ora tracciato come `G6`. Per questo motivo, la promozione verso tag / merge su `main` / release / consegna non è ancora autorizzata.

Dopo `G5.1` e `G5.2`, la validazione manuale reale dell’host MVC su liste lunghe ha confermato un difetto UX aggiuntivo: la viewport tornava verso l’alto durante navigazione, ricerca e comandi della griglia. Il tentativo locale iniziale basato su scroll manuale e marcatori `data-viewport-*` è stato classificato come investigazione non consolidata e poi rimosso.

Il commit `39e3bdd` ha chiuso `G5.3` sostituendo il full-page reload interattivo dell’host MVC con una navigazione incrementale a fragment:
- `HomeController` ora rende `Index` o `_ExamNavigationPage` tramite `RenderNavigationPage(...)`, in base al tipo di richiesta;
- `Index.cshtml` è ora una shell MVC con root `#exam-navigation-root` e script `fetch` dedicato;
- `_ExamNavigationPage.cshtml` contiene il markup reale della pagina web;
- la ricerca, la navigazione dei pannelli e i comandi della griglia vengono aggiornati asincronamente senza ricaricare l’intera pagina.

Fix consolidati di questo gate:
- commit `de03d95` — normalizzazione dei label degli ambulatori nell’host MVC, sia nella navigazione web sia nella griglia `Esami selezionati`;
- commit `cfee331` — estensione del seed PostgreSQL con dataset demo misto, comprendente baseline legacy/non normalizzata e nuovi dati più eterogenei, plausibili e professionalmente più coerenti per audit di naming, abbreviazioni e casi di normalizzazione;
- commit `39e3bdd` — eliminazione del salto viewport MVC su liste lunghe tramite fragment navigation AJAX e verifica manuale finale positiva sui cinque casi critici: `Ambulatori`, `Parti del corpo`, `Esami`, griglia selezioni e ricerca.

Il gate `G5.4` è stato chiarito truth-first: `BootstrapNavigationService` non è il runtime principale del client WinForms, ma un fallback legacy in-memory ancora raggiungibile tramite il costruttore parameterless `Form1()`. Il bootstrap reale del client desktop continua a passare da `Program.Main()` verso `PostgreSqlExamNavigationService` e poi verso `new Form1(navigationService)`.

Con questa classificazione il gate `G5` risulta chiuso a docs sync consolidato. I preflight `H0`-`H2` restano validi come lavoro preparatorio di demo/consegna, ma non sbloccano ancora la promozione finale.

La baseline funzionale attuale è solida, verificata e dimostrabile tramite bundle demo locale controllato basato su PostgreSQL. Tuttavia, la chiusura letterale della missione non è ancora autorizzata, perché il repository non espone ancora un runtime SQL Server concreto wired agli host applicativi.

Il gap mission-critical residuo è ora classificato truth-first come `G6 — SQL Server runtime conformance closure`. Finché `G6` non sarà chiuso, il bundle PostgreSQL resta una baseline demo locale difendibile ma non ancora il punto finale di rilascio pienamente aderente alla formulazione letterale della missione.

## Documentazione owner

- `docs/TIMELINE.md` -> stato operativo reale
- `docs/CHANGELOG.md` -> tracciabilità evolutiva
- `docs/ROADMAP.md` -> traiettoria e milestone
- `docs/ARCHITECTURE.md` -> struttura reale del sistema
