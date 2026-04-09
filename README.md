# Exam Navigator System

Progetto tecnico realizzato per il test di programmazione NOLEX.

## Obiettivo

Realizzare un sistema desktop WinForms per la selezione guidata di esami medici, basato su:

- elenco ambulatori;
- elenco parti del corpo;
- elenco esami;
- selezione a cascata;
- ricerca testuale case-insensitive;
- griglia di riepilogo delle scelte;
- caricamento opzionale di configurazione `.ini`.

Il progetto viene impostato fin dall’inizio in modo da essere convertibile a web tramite ASP.NET Core MVC, evitando logiche applicative duplicate.

## Scelte tecniche iniziali

- Desktop UI: WinForms
- Target desktop: .NET Framework 4.8.1
- Database: SQL Server
- Accesso dati: SQL essenziale + repository/query service
- Web conversion target: ASP.NET Core MVC
- Architettura: core condiviso + host desktop + host web

## Strategia architetturale

La soluzione verrà costruita in modo stratificato:

1. **Core di dominio**
   - entità;
   - value object / enum;
   - regole base.

2. **Application layer**
   - servizi di selezione;
   - servizi di ricerca;
   - orchestrazione della cascata:
     - ambulatorio -> parti del corpo -> esami;
   - gestione della configurazione applicativa.

3. **Infrastructure layer**
   - accesso SQL Server;
   - seed dati;
   - lettore `.ini` riflessivo.

4. **Host WinForms**
   - presentazione desktop;
   - griglia selezioni;
   - gestione eventi UI.

5. **Host Web**
   - conversione a MVC;
   - riuso del core applicativo;
   - interfaccia web equivalente.

## Stato iniziale repository

Repository bootstrap:
- governance documentale iniziale presente;
- missione congelata in `docs/target/requirements`;
- timeline pronta per l’esecuzione;
- roadmap iniziale definita.

## Documentazione

- `docs/TIMELINE.md` -> stato operativo reale
- `docs/CHANGELOG.md` -> tracciabilità evolutiva
- `docs/ROADMAP.md` -> traiettoria e milestone
- `docs/target/requirements/01_original_mission.md` -> missione grezza
- `docs/target/requirements/02_requirements_freeze.md` -> estrazione strutturata dei requisiti
