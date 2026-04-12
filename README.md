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
- progetto `ExamNavigator.Infrastructure.PostgreSql` presente con adapter PostgreSQL concreto `PostgreSqlExamNavigationService`, query reali per ambulatori, parti del corpo ed esami, fallback di selezione `SelectedRoomId` / `SelectedBodyPartId` e parametri Npgsql tipizzati; host WinForms ora wired al runtime PostgreSQL concreto tramite `Program.cs`, con runtime closure `.NET Standard`/`Npgsql` governata dal `.csproj`, binding redirects espliciti in `App.config`, normalizzazione label degli ambulatori, etichette leggibili della ricerca e presentazione multi-line più leggibile del pannello `Esami`; host MVC ancora wired al bootstrap service locale in memoria; test, lint e coverage non ancora introdotti nella codebase; il caricamento runtime dei default da configurazione e il consumo runtime dei default nel bootstrap/UI sono presenti per la baseline della ricerca; l'host MVC è ora presente con baseline web funzionale completa in memoria per navigazione esami, ricerca GET, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX, sempre wired al core condiviso.

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
   - baseline funzionale web equivalente alla demo WinForms introdotta tramite controller dedicato, page view model e bootstrap service locale in memoria;
   - ricerca GET, conferma selezione, griglia riepilogativa, riordino, eliminazione riga e polish UI/UX presenti;
   - adattamento MVC ancora appoggiato a dataset demo in memoria e non a un adapter SQL concreto.

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
7. blocchi successivi → configurazione `.ini` avanzata con fondazione dei default di ricerca, parser raw del documento, binder riflessivo type-safe e wiring runtime della baseline di ricerca completati; conversione web MVC archiviata come baseline demo con host dedicato (`F0`), primo riallineamento funzionale di controller/view model (`F1`) e UI web equivalente con griglia selezioni e polish (`F2`); bootstrap runtime locale PostgreSQL documentato e consolidato; blocchi `G1`-`G2` completati con introduzione di `ExamNavigator.Infrastructure.PostgreSql`, del service concreto `PostgreSqlExamNavigationService` e del wiring runtime concreto del client WinForms; V1 mission-critical ancora aperta sul wiring MVC (`G3`) e sulla verifica formale finale (`G4`), con divergenza rispetto al requisito SQL Server originario esplicitamente governata nella documentazione.

## Perimetro V1

La V1 mission-critical coincide esclusivamente con i requisiti funzionali e non funzionali della mail, congelati nel freeze locale dei requisiti.

Per la V1 il criterio è:
- soluzione concreta;
- robusta, affidabile e solida;
- consegnabile, valutabile e difendibile;
- aderente ai requisiti della mail senza introdurre dipendenze dagli EXTRA.

Tutto ciò che non rientra in questo perimetro resta post-V1 oppure EXTRA congelato.

## Documentazione owner

- `docs/TIMELINE.md` -> stato operativo reale
- `docs/CHANGELOG.md` -> tracciabilità evolutiva
- `docs/ROADMAP.md` -> traiettoria e milestone
- `docs/ARCHITECTURE.md` -> struttura reale del sistema
