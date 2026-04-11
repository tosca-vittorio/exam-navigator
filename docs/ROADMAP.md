
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
6. integrazione ricerca/configurazione `.ini`.

Milestone attiva:
7. conversione web MVC.

Milestone successive:
8. track qualità / EXTRA differiti.

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

### 3. Persistenza SQL Server
Stato: completato.

Obiettivo:
- introdurre schema minimale ma corretto;
- introdurre seed demo realistico;
- introdurre query di riferimento per cascata e ricerca.

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
Stato: attivato con host MVC baseline già presente.

Obiettivo:
- riutilizzare il core applicativo;
- esporre lo stesso comportamento in MVC;
- evitare riscrittura della logica del caso d’uso.

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
