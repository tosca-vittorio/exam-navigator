# 04 — Timeline estesa, storia progettuale completa e narrativa tecnica del progetto

## Finalità del documento

Questo documento ricostruisce il progetto come percorso tecnico reale, illustrando in modo esteso l’evoluzione del lavoro dalla missione iniziale fino allo stato attuale.

Per ciascuna fase vengono chiariti il problema affrontato, la scelta effettuata, il risultato prodotto e la ragione per cui il blocco successivo rappresentava il passo corretto.

Il documento mette in evidenza in modo maturo **come il progetto è nato, come è cresciuto, come è stato corretto e perché oggi si trova esattamente nello stato attuale**.

---

## 1. Perché una timeline tecnica è importante

Una buona soluzione non è soltanto un insieme di file corretti: è anche il risultato di una sequenza di decisioni coerenti.

In questo caso, la timeline è particolarmente importante perché il progetto ha attraversato varie fasi significative:

* definizione del perimetro della missione;
* costruzione del core condiviso;
* realizzazione progressiva del desktop;
* introduzione della ricerca e della configurazione;
* materializzazione della convertibilità web;
* fase PostgreSQL come runtime concreto locale;
* gate finale di coerenza;
* riallineamento conclusivo a SQL Server;
* chiusura del preflight/demo locale controllata;
* avvio della fase successiva di consolidamento documentale.

---

## 2. Quadro generale del percorso

L’intera traiettoria del progetto può essere letta così:

1. **prima** capire la missione;
2. **poi** congelare i requisiti;
3. **poi** costruire una base architetturale condivisa;
4. **poi** materializzare il comportamento richiesto in WinForms;
5. **poi** completare ricerca e configurazione;
6. **poi** dimostrare davvero la convertibilità web con MVC;
7. **poi** chiudere la V1 su un runtime concreto locale;
8. **poi** correggere i mismatch di qualità reale emersi dopo il freeze;
9. **poi** riallineare la soluzione alla richiesta letterale di SQL Server;
10. **poi** congelare il bundle demo locale controllato;
11. **infine** spostare il focus sulla fase successiva di consolidamento documentale.

Questa è la narrativa di alto livello.

---

## 3. Blocco A — Bootstrap repository + freeze requisiti

### Perché è stato aperto

Il primo problema non era ancora tecnico in senso stretto. Prima di scrivere codice era necessario evitare di sviluppare “a intuito”. Il testo della missione era ricco di dettagli: UI desktop, dati in SQL Server, ricerca case-insensitive, `.ini`, convertibilità web. Senza un freeze iniziale, il rischio sarebbe stato quello di perdere pezzi importanti o di confondere richieste obbligatorie con dettagli facoltativi.

### Cosa è stato fatto

In questa fase sono stati congelati:

* il testo originale della missione;
* il requirements freeze;
* la baseline documentale owner;
* il repository Git con branch `main` e `development`.

Il progetto non è nato dal codice, ma da una **lettura disciplinata della richiesta**.

### Che cosa ha sbloccato

Ha reso possibile il passaggio dal problema al modello. Solo dopo aver definito cosa andava costruito, aveva senso progettare la struttura della soluzione.

---

## 4. Blocco B — Solution skeleton + core condiviso

### Perché è stato aperto

Una volta chiarito il perimetro, il passo corretto era evitare che la soluzione nascesse come un progetto WinForms isolato e monolitico. Dal momento che il test chiedeva convertibilità web, la scelta più corretta era creare subito un nucleo condiviso.

### Cosa è stato fatto

Sono stati introdotti:

* la solution;
* il progetto `Domain`;
* il progetto `Application`;
* il modello di dominio minimo;
* i contratti applicativi e l’interfaccia `IExamNavigationService`.

Questa fase rappresenta il momento in cui il progetto smette di essere una semplice idea di GUI e diventa una base ingegneristica riusabile.

### Che cosa ha sbloccato

Ha creato il perimetro corretto da cui far dipendere gli host futuri. A questo punto si poteva costruire il desktop senza legare tutta la logica al form.

---

## 5. Blocco C — Database baseline di riferimento

### Perché è stato aperto

La missione chiedeva dati gestiti in ambiente SQL Server e una logica di navigazione a cascata. Questo implicava la necessità di modellare non solo classi, ma anche uno schema relazionale coerente e un dataset demo sufficiente a dimostrare il comportamento della UI.

### Cosa è stato fatto

Sono stati introdotti:

* `001_schema.sql`;
* `002_seed.sql`;
* `003_navigation_queries.sql`.

In una fase successiva dello stesso macro-percorso è stata introdotta anche la track PostgreSQL locale, inizialmente come scelta runtime concreta intermedia.

Questo blocco dimostra che il progetto non si è appoggiato a dati fittizi hardcoded nel form, ma ha costruito fin dall’inizio una base dati relazionale coerente con il dominio.

### Che cosa ha sbloccato

Ha reso possibile la materializzazione della UI desktop su una base concettuale e dati già ragionati.

---

## 6. Blocco D — WinForms implementation

### Perché è stato aperto

La missione chiedeva in modo esplicito un form WinForms con tre pannelli affiancati, selezione a cascata, conferma selezione e griglia riepilogativa. Quindi, dopo aver definito dominio, application e database baseline, il passo corretto era materializzare davvero l’host desktop.

### Cosa è stato fatto

Questo blocco ha attraversato più sotto-fasi:

* layout del form;
* wiring della cascata;
* conferma selezione;
* griglia riepilogativa;
* delete della riga selezionata;
* move up / move down.

È qui che la missione inizia a diventare visibile e verificabile dall’utente finale. La soluzione smette di essere solo schema e contratti e diventa un comportamento interattivo reale.

### Che cosa ha sbloccato

Una volta consolidata la baseline desktop, diventava sensato completare due aree richieste ma ancora non finalizzate: ricerca e configurazione `.ini`.

---

## 7. Blocco E — Ricerca e configurazione `.ini`

### Perché è stato aperto

Dopo aver reso operativa la baseline desktop, mancavano ancora due aspetti essenziali della missione:

* ricerca case-insensitive;
* caricamento riflessivo della configurazione `.ini`.

### Cosa è stato fatto

Anche qui c’è stata una progressione chiara:

* audit del service di ricerca già implicito nel boundary applicativo;
* wiring UI della ricerca;
* introduzione di `Predefiniti_Ricerca`;
* parser raw `IniConfigurationDocument`;
* binder riflessivo `IniConfigurationBinder`;
* consumo runtime dei default della ricerca nel bootstrap/UI.

Questo blocco è molto significativo perché dimostra che il progetto non si è limitato alle parti più ovvie della missione, ma ha affrontato anche quella più “raffinata”, cioè il binding riflessivo del file `.ini`.

### Che cosa ha sbloccato

A questo punto la baseline desktop era abbastanza completa da poter affrontare seriamente la convertibilità web.

---

## 8. Blocco F — Conversione web MVC

### Perché è stato aperto

La missione chiedeva che il programma fosse convertito per il funzionamento web in .NET Core MVC. Non bastava quindi dire che l’architettura lo avrebbe permesso: bisognava almeno materializzare un host MVC concreto e coerente con la traiettoria architetturale.

### Cosa è stato fatto

Il blocco ha seguito una progressione naturale:

* introduzione dell’host MVC;
* primo riallineamento funzionale con controller e page model;
* UI web equivalente con ricerca, tre pannelli, conferma selezione, griglia e comandi;
* successivo miglioramento della UX fino alla fragment navigation.

### Che cosa ha sbloccato

Dopo questo punto il progetto era già molto avanzato, ma restava ancora aperta la questione del runtime concreto locale e della conformità complessiva della V1.

---

## 9. Blocco G — Chiusura V1 mission-critical su PostgreSQL runtime concreto

### Perché è stato aperto

A un certo punto il progetto aveva già:

* core condiviso;
* database baseline;
* WinForms;
* ricerca e `.ini`;
* MVC.

Mancava però una vera chiusura mission-critical su runtime concreto locale. In questa fase il progetto aveva scelto PostgreSQL come runtime locale attivo e fu introdotto un adapter dedicato.

### Cosa è stato fatto

Sono stati introdotti:

* artefatti PostgreSQL di bootstrap locale;
* `ExamNavigator.Infrastructure.PostgreSql`;
* `PostgreSqlExamNavigationService`;
* wiring concreto di WinForms e MVC verso PostgreSQL;
* verifica formale finale della V1.

Questa fase mostra che il progetto è stato capace di passare da una baseline principalmente strutturale a una **V1 concreta e dimostrabile**.

### Che cosa ha sbloccato

Ha consentito di congelare una V1 forte, ma proprio quel freeze ha fatto emergere con maggiore chiarezza alcuni mismatch residui di qualità e coerenza.

---

## 10. Blocco G5 — Final conformance & coherence gate pre-consegna

### Perché è stato aperto

Dopo il freeze formale della V1, il progetto non si è autoassolto. Ha invece verificato se la baseline fosse anche davvero coerente e professionale nella pratica. Da qui nasce un gate finale di conformità e coerenza.

### Cosa è stato fatto

Sono stati chiusi vari gap reali:

* normalizzazione dei label lato MVC;
* audit e arricchimento del seed demo;
* fix del problema UX della viewport in MVC tramite shell + fragment + fetch;
* classificazione del `BootstrapNavigationService` WinForms come fallback legacy.

### Che cosa ha sbloccato

Ha chiarito che il progetto era forte, ma non ancora definitivamente allineato al requisito letterale di SQL Server. Da qui la riapertura corretta del blocco successivo.

---

## 11. Blocco G6 — SQL Server runtime conformance closure

### Perché è stato aperto

Questa è una delle fasi più importanti dell’intera storia progettuale. La V1 su PostgreSQL era ormai concreta e forte, ma i documenti owner hanno classificato truth-first un gap residuo: la missione originale richiedeva dati in ambiente SQL Server oppure una soluzione importabile in SQL Server. La baseline PostgreSQL costituiva un passaggio intermedio tecnicamente solido, ma non rappresentava ancora il riallineamento più forte al perimetro letterale del test.

### Cosa è stato fatto

È stato introdotto:

* `ExamNavigator.Infrastructure.SqlServer`;
* `SqlServerExamNavigationService`;
* wiring SQL Server concreto per WinForms e MVC;
* runtime closure desktop reale per `Microsoft.Data.SqlClient`;
* bootstrap locale SQL Server documentato e consolidato.

### Che cosa ha sbloccato

Ha permesso di chiudere il gap mission-critical più sensibile e di aprire la fase finale di demo locale controllata.

---

## 12. Blocco H — Consegna / rilascio / demo V1

### Perché è stato aperto

Una volta chiuso il gap SQL Server, il passo corretto non era ancora taggare o rilasciare, ma verificare in modo rigoroso quale fosse il **bundle demo locale controllato**.

### Cosa è stato fatto

Sono stati chiusi i sotto-step di preflight:

* classificazione del formato demo;
* censimento della superficie del bundle;
* playbook operativo SQL Server -> WinForms -> MVC;
* freeze del changeset minimo promuovibile;
* preflight della sequenza di presentazione;
* dry-run operativo finale;
* witness funzionale finale.

Questo blocco chiarisce una cosa fondamentale: il progetto è pronto per essere presentato in locale, ma non è ancora stato promosso formalmente come release ufficiale.

### Che cosa ha sbloccato

Ha chiuso il piano della demo locale controllata e ha permesso al progetto di passare dal lavoro implementativo alla fase successiva di documentazione e consolidamento.

---

## 13. Perché ogni blocco arriva esattamente quando arriva

Uno degli aspetti più importanti da rilevare è che la timeline non è arbitraria.

Ogni blocco arriva quando il precedente ha costruito abbastanza base da giustificare il successivo:

* senza freeze requisiti non aveva senso progettare il core;
* senza core non aveva senso costruire host multipli;
* senza baseline dati non aveva senso implementare cascata e ricerca;
* senza WinForms non aveva senso parlare seriamente di convertibilità web;
* senza runtime concreto non aveva senso congelare una V1;
* senza gate finale di coerenza non era serio promuovere la baseline;
* senza riallineamento SQL Server non era corretto chiudere la missione sul piano letterale;
* senza demo preflightrata non aveva senso aprire la fase successiva.

Questa progressione dimostra che il progetto è cresciuto per **necessità reali** e non per accumulo casuale di funzionalità.

---

## 14. Momenti più importanti

Se dovessi selezionare i momenti più importanti dell’intero percorso:

### 14.1 La scelta iniziale del core condiviso

È il momento in cui il progetto prende una forma ingegneristica e non soltanto UI.

### 14.2 L’introduzione del binder riflessivo `.ini`

È il momento in cui il progetto dimostra attenzione a una richiesta sofisticata della missione.

### 14.3 La materializzazione reale di MVC

È il momento in cui la convertibilità web smette di essere solo teorica.

### 14.4 Il pivot PostgreSQL e la successiva rivalutazione

È il momento in cui il progetto mostra flessibilità ma anche onestà tecnica.

### 14.5 Il riallineamento finale a SQL Server

È il momento in cui il progetto chiude il gap di conformità più sensibile.

### 14.6 La distinzione tra demo locale controllata e release ufficiale

È il momento in cui il progetto mostra rigore nella governance della promozione.

---

## Conclusione

La timeline del progetto non è solo una successione di step, ma la prova che la soluzione è stata costruita con progressione, autocorrezione e riallineamento continuo allo stato reale.

Questo è uno degli aspetti più maturi dell’intero lavoro, perché dimostra che:

* la missione è stata letta e congelata con disciplina;
* la struttura è stata introdotta prima dell’espansione degli host;
* la V1 è stata costruita per strati, non per patch sparse;
* i mismatch reali sono stati affrontati anche quando emergevano tardi;
* il riallineamento a SQL Server è stato una correzione intenzionale e coerente;
* la demo è stata trattata come superficie controllata e non come rilascio improvvisato;
* la fase successiva nasce su una baseline realmente consolidata.

In sintesi, questa timeline racconta un progetto che non si è limitato a “diventare funzionante”, ma ha saputo diventare progressivamente più coerente, più solido e più aderente alla missione da cui era nato.
