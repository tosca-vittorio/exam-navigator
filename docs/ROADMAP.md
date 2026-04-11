
# ROADMAP — Exam Navigator System

## Obiettivo generale
Consegnare una soluzione tecnicamente corretta, architetturalmente difendibile e già predisposta alla conversione web, con documentazione owner abbastanza solida da sostenere spiegazione tecnica, revisione e colloquio.

## Stato milestone corrente

Milestone già consolidate:
1. bootstrap repository + governance documentale;
2. baseline core condivisa (`Domain` + `Application`);
3. baseline SQL Server (`schema` + `seed` + `query`);
4. host WinForms baseline (`bootstrap progetto + layout statico del form`).
5. WinForms first (`wiring` cascata, conferma selezione, delete/reorder della griglia).

Milestone mission-critical consolidata più recente:
7. conversione web MVC baseline funzionale demo archiviata.

Milestone attiva:
8. chiusura V1 mission-critical su PostgreSQL runtime concreto, con divergenza SQL Server esplicitamente governata.

Milestone successive:
9. preparazione consegna / rilascio / demo V1;
10. preparazione colloquio #2;
11. EXTRA e sviluppi futuri post-colloquio.

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
Stato: attiva.

Obiettivo:
- chiudere il runtime concreto sul database PostgreSQL locale scelto;
- sostituire i bootstrap service in memoria con persistenza PostgreSQL runtime concreta;
- mantenere WinForms e MVC coerenti sulla stessa fonte dati reale;
- documentare in modo esplicito e difendibile la divergenza rispetto al requisito SQL Server originario;
- arrivare a una V1 concreta, robusta, affidabile, consegnabile e valutabile.

### 8. Preparazione consegna / rilascio / demo V1
Stato: congelata, attivabile solo dopo la chiusura della V1.

Obiettivo:
- preparare il formato di consegna più opportuno (`.exe`, script SQL, bundle demo, eventuale Docker solo se realmente utile);
- produrre tag dedicato alla consegna cliente;
- eseguire merge su `main` e release di baseline;
- congelare il punto di rilascio della richiesta cliente prima di ulteriori ottimizzazioni.

### 9. Preparazione colloquio #2
Stato: congelata, attivabile solo dopo la chiusura della V1 e la preparazione della consegna.

Obiettivo:
- costruire documentazione di studio, presentazione progetto e materiale di difesa tecnica;
- spiegare timeline, scelte, sintassi, architettura, database, moduli e responsabilità;
- preparare domande plausibili di colloquio e relative risposte;
- usare la V1 come base di studio su MVC, controller, SQL e architettura.

### 10. Sviluppi futuri / EXTRA post-colloquio
Stato: congelati, attivabili solo dopo la fase di preparazione colloquio.

Obiettivo:
- refactor pulito e possibile introduzione di pattern GOF o equivalenti;
- best practices più spinte di qualità, programmazione, ingegneria e architettura del software;
- eventuale UI moderna React/TypeScript collegata a backend .NET;
- region ordinate, commenti XML estesi e ulteriori miglioramenti non mission-critical.

## Track EXTRA congelati ma non attivati

Questi blocchi sono formalizzati ma **non prioritari** rispetto alla missione corrente:

- documentazione estesa A→Z per studio, spiegazione e preparazione colloquio;
- congelamento formale requisiti e futura riverifica implementativa/funzionale;
- lint;
- test unitari minimi;
- coverage;
- smoke test;
- quality campaign su baseline stabile;
- eventuale ambiente isolato / contenimento dipendenze.

## Decisioni guida

- Prima correttezza e chiarezza.
- Poi estensibilità.
- Nessun overengineering inutile.
- Nessun coupling forte tra UI, logica applicativa e persistenza.
- Gli EXTRA restano tracciati ma non devono disturbare la traiettoria mission-critical.
