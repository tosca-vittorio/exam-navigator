
# ROADMAP — Exam Navigator System

## Obiettivo generale
Consegnare una soluzione tecnicamente corretta, architetturalmente difendibile e già predisposta alla conversione web, con documentazione owner abbastanza solida da sostenere spiegazione tecnica, revisione e trasferibilità progettuale.

## Stato milestone corrente

Milestone già consolidate:
1. bootstrap repository + governance documentale;
2. baseline core condivisa (`Domain` + `Application`);
3. baseline SQL Server (`schema` + `seed` + `query`);
4. host WinForms baseline (`bootstrap progetto + layout statico del form`).
5. WinForms first (`wiring` cascata, conferma selezione, delete/reorder della griglia).
6. ricerca e configurazione `.ini`.
7. conversione web MVC baseline funzionale demo archiviata.
8. chiusura V1 mission-critical su PostgreSQL runtime concreto, con divergenza SQL Server esplicitamente governata.
9. final conformance/coherence gate pre-consegna sulla baseline V1 formalmente congelata.
9.bis. materializzazione del runtime SQL Server concreto su entrambi gli host.
10. preparazione consegna / rilascio / demo V1 sul piano del preflight/demo locale controllata.

Milestone attiva:
12. promozione formale V1 / freeze su `main`.

Milestone successive:
13. EXTRA e sviluppi futuri post-freeze.

## Sequenza strategica mission-critical

### 1. Bootstrap e freeze requisiti
Stato: completato.

Obiettivo:
- congelare la missione;
- derivare requisiti verificabili;
- aprire governance repository.

### 2. Core condiviso
Stato: completato.

Obiettivo:
- definire dominio e contratti applicativi;
- evitare duplicazione di logica tra desktop e web;
- preparare il perimetro comune da cui dipenderanno gli host.

### 3. Persistenza baseline e bootstrap runtime locale
Stato: completato.

Obiettivo:
- introdurre schema minimale ma corretto;
- introdurre seed demo realistico;
- introdurre query di riferimento per cascata e ricerca;
- congelare il bootstrap runtime locale PostgreSQL come scelta tecnica attiva.

### 4. WinForms first
Stato: completato.

Obiettivo:
- soddisfare integralmente la missione desktop;
- consolidare il baseline host WinForms già introdotto;
- completare il wiring UI verso core applicativo e baseline dati.

### 5. Ricerca e configurazione
Stato: completato.

Obiettivo:
- completare il servizio di ricerca;
- integrare il wiring UI della ricerca;
- introdurre il loader `.ini` riflessivo e i default configurabili.

### 6. Web conversion
Stato: archiviata come baseline funzionale demo con `F0` / `F1` / `F2` chiusi.

Obiettivo:
- riutilizzare il core applicativo;
- esporre progressivamente lo stesso comportamento in MVC;
- evitare riscrittura della logica del caso d’uso;
- chiudere la parte di conversione web senza riaprire il blocco nei passi successivi.

### 7. Chiusura V1 mission-critical
Stato: completata (`G1`-`G4` chiusi; V1 mission-critical formalmente congelata).

Obiettivo:
- chiudere il runtime concreto sul database PostgreSQL locale scelto;
- sostituire i bootstrap service in memoria con persistenza PostgreSQL runtime concreta;
- mantenere WinForms e MVC coerenti sulla stessa fonte dati reale;
- documentare in modo esplicito e difendibile la divergenza rispetto al requisito SQL Server originario;
- arrivare a una V1 concreta, robusta, affidabile, consegnabile e valutabile.

Stato operativo corrente:
- il layer `src/ExamNavigator.Infrastructure.PostgreSql` è presente;
- `PostgreSqlExamNavigationService` implementa il boundary applicativo sul runtime PostgreSQL locale;
- il client WinForms è wired al runtime PostgreSQL concreto e verificato anche dopo clean rebuild della runtime closure;
- l'host MVC è wired al runtime PostgreSQL concreto e verificato con build verde e smoke manuale su host web reale;
- la V1 mission-critical è formalmente chiusa, ma la promozione resta sospesa da un final conformance/coherence gate pre-consegna tracciato separatamente nella sezione 8.

### 8. Final conformance/coherence gate pre-consegna
Stato: completato.

Obiettivo:
- verificare in modo rigoroso che la baseline V1 formalmente chiusa sia anche coerente e professionale in termini di dati demo, naming, abbreviazioni, resa UI e comportamento percepito;
- correggere i mismatch reali emersi dopo il freeze senza riaprire arbitrariamente i blocchi già chiusi;
- bloccare ogni promozione verso consegna/rilascio finché tali mismatch non siano auditati e chiusi.

Stato operativo corrente:
- commit `de03d95` ha chiuso il mismatch presentazionale dei label raw degli ambulatori nell’host MVC;
- commit `cfee331` ha chiuso il lavoro tecnico di audit su dati demo, naming e abbreviazioni, estendendo il seed PostgreSQL con un dataset misto legacy + professionale utile alla validazione della normalizzazione;
- commit `39e3bdd` ha chiuso `G5.3`, sostituendo il full-page reload interattivo dell’host MVC con shell `Index.cshtml` + fragment `_ExamNavigationPage.cshtml`, rendering dual-mode `View/PartialView` nel controller e navigazione incrementale `fetch`-based;
- `dotnet build ExamNavigator.sln` è rimasto verde e la validazione manuale finale è risultata positiva sui cinque casi critici (`Ambulatori`, `Parti del corpo`, `Esami`, griglia selezioni e ricerca), con eliminazione del salto viewport su liste lunghe;
- l'audit `G5.4` ha classificato `BootstrapNavigationService` WinForms come fallback legacy in-memory ancora raggiungibile tramite il costruttore parameterless `Form1()`, ma non parte del bootstrap runtime principale, che passa da `Program.Main()` a `PostgreSqlExamNavigationService`;
- con questa classificazione truth-first il gate `G5` risulta chiuso a documentazione riallineata; i preflight `H0`-`H2` restano validi come preparazione della demo, ma il prossimo blocco mission-critical corretto diventa `9.bis / G6`, cioè il riallineamento finale al requisito SQL Server, senza anticipare ancora merge su `main`, tag o release.

### 9.bis. SQL Server runtime conformance closure
Stato: completata e archiviata.

Obiettivo:
- introdurre un runtime SQL Server concreto coerente con la missione originale;
- collegare gli host applicativi a tale runtime;
- chiudere il gap residuo tra baseline demo PostgreSQL difendibile e aderenza letterale alla richiesta;
- abilitare la successiva preparazione finale della consegna.

Stato operativo corrente:
- il repository espone ora un layer `ExamNavigator.Infrastructure.SqlServer` concreto;
- `SqlServerExamNavigationService` implementa il boundary applicativo sul runtime SQL Server;
- il client WinForms è wired al runtime SQL Server concreto tramite `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- l'host MVC è wired allo stesso runtime SQL Server concreto tramite `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- il client desktop governa la runtime closure di `Microsoft.Data.SqlClient` nel `.csproj`;
- il commit `d366e18` ha completato la runtime closure desktop reale WinForms, includendo le dipendenze transitive richieste dal provider SQL Server e uno smoke manuale positivo dell'eseguibile con `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`;
- il commit `2354e54` (`docs(sqlserver): add bootstrap guide and enrich demo seed`) consolida `database/sql/sqlserver.md` come documento tecnico del bootstrap locale SQL Server e amplia `database/sql/002_seed.sql` con un dataset demo più ricco;
- il quality gate minimo corrente è verde su `dotnet build ExamNavigator.sln`;
- il blocco `G6` risulta chiuso sul piano tecnico e non è più il focus operativo corrente.

### 9.ter. Docs sync gate owner + push controllato pre-H
Stato: completata.

Obiettivo:
- riallineare truth-first i documenti owner dopo la materializzazione del runtime SQL Server concreto;
- registrare la chiusura del riallineamento post-push su `development`;
- sbloccare formalmente il passaggio al blocco `H`.

Stato operativo corrente:
- `README.md`, `docs/TIMELINE.md` e `docs/CHANGELOG.md` sono stati riallineati allo stato post-push del runtime SQL Server concreto;
- il commit documentale `d1fa273` risulta pubblicato su `origin/development`;
- il docs sync gate owner non è più il blocco attivo corrente;
- il successivo avanzamento corretto è `10. Preparazione consegna / rilascio / demo V1`.

### 10. Preparazione consegna / rilascio / demo V1
Stato: completata e archiviata sul piano del preflight/demo locale controllata.

Obiettivo:
- congelare il formato demo più difendibile;
- fissare il playbook operativo SQL Server -> WinForms -> MVC;
- verificare bootstrap dati, build, avvio host e witness funzionale minima;
- chiudere la preparazione della demo senza anticipare tag, merge su `main` o release.

Stato operativo corrente:
- i sotto-step `H0`-`H7` risultano chiusi;
- il runtime SQL Server concreto resta la baseline attiva condivisa dai due host;
- il bootstrap locale SQL Server è stato rieseguito con conteggi `BodyPart = 4`, `Room = 11`, `Exam = 14`, `ExamRoom = 30`;
- `dotnet build ExamNavigator.sln` è verde;
- la demo primaria resta WinForms, mentre l'host MVC resta la prova secondaria della convertibilità web già concretamente implementata;
- restano esplicitamente fuori da questa chiusura tag, merge su `main`, release, installer, archivio finale di consegna, Docker salvo reale necessità e qualunque riapertura di `G6`.

### 11. Consolidamento documentale tecnico V1
Stato: completata e archiviata.

Obiettivo:
- consolidare il bundle documentale tecnico partendo da missione sorgente e freeze requisiti;
- riallineare copertura implementativa, architettura, moduli, SQL, MVC e flussi principali in forma neutra e riusabile;
- neutralizzare naming, titoli e formulazioni ancora troppo orientati a finalità non tecniche;
- chiudere il doc sync gate finale che prepara la promozione formale della V1.

Stato operativo corrente:
- commit `c0ea7ee` presente;
- bundle documentale tecnico `docs/release/v1.0.0/` consolidato;
- `docs/release/README.md` e owner docs riallineati;
- non residuano micro-task operative nel blocco 11.

### 12. Promozione formale V1 / freeze su `main`
Stato: attiva.

Obiettivo:
- promuovere formalmente la baseline V1 già chiusa e consolidata;
- eseguire merge controllato su `main`, tag e release;
- congelare definitivamente la V1 senza alterarne il perimetro funzionale;
- separare in modo netto la baseline V1 dalle evoluzioni successive.

Stato operativo corrente:
- questo è ora il blocco operativo corrente;
- la baseline V1 risulta già chiusa e consolidata su `development`;
- restano da eseguire in modo esplicito merge su `main`, tag e release;
- ogni ampliamento, refactor o nuova feature resta fuori da questa promozione.

### 13. Sviluppi futuri / EXTRA post-freeze
Stato: congelati, attivabili solo dopo la promozione formale della V1.

Obiettivo:
- refactor pulito e possibile introduzione di pattern GOF o equivalenti;
- best practices più spinte di qualità, programmazione, ingegneria e architettura del software;
- eventuale UI moderna React/TypeScript collegata a backend .NET;
- region ordinate, commenti XML estesi e ulteriori miglioramenti non mission-critical.

## Track EXTRA congelati ma non attivati

Questi blocchi sono formalizzati ma **non prioritari** rispetto alla missione corrente:

- congelamento formale requisiti e futura riverifica implementativa/funzionale;
- lint;
- test unitari minimi;
- coverage;
- smoke test;
- quality campaign su baseline stabile;
- eventuale ambiente isolato / contenimento dipendenze.
- eventuale superficie di distribuzione aggiuntiva della V1 già promossa (archivio, installer, package demo o bundle operativo dedicato);
- provider selector/factory per scegliere a configurazione l'adapter dati attivo (es. SQL Server, PostgreSQL, future data sources).

## Decisioni guida

- Prima correttezza e chiarezza.
- Poi estensibilità.
- Nessun overengineering inutile.
- Nessun coupling forte tra UI, logica applicativa e persistenza.
- Gli EXTRA restano tracciati ma non devono disturbare la traiettoria mission-critical.
