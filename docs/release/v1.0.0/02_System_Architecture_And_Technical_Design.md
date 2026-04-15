# 02 — Architettura, progettazione, logiche, database, MVC e scelte tecniche

## Finalità del documento

Il presente documento descrive la struttura tecnica del progetto in modo approfondito e ordinato, chiarendo come la soluzione sia stata costruita dal punto di vista ingegneristico: livelli applicativi, moduli, responsabilità, dipendenze, scelte architetturali, logiche di sviluppo, sintassi rilevante, modello dati, backend di navigazione e struttura dell’host MVC.

L’obiettivo del documento è fornire una lettura tecnica coerente della soluzione, mettendo in evidenza confini, responsabilità, flussi principali, razionale delle scelte e rischi residui.

---

## 1. Visione architetturale generale

La soluzione è stata costruita secondo una stratificazione semplice ma solida. Non è stata adottata un’architettura artificiosamente complessa, ma una struttura abbastanza ordinata da separare responsabilità diverse e da sostenere il doppio obiettivo del test:

* soddisfare la richiesta desktop WinForms;
* predisporre una reale estensione web MVC senza duplicare la logica applicativa.

La struttura attuale del progetto si può leggere come un sistema a livelli:

* `Domain`;
* `Application`;
* `Infrastructure.SqlServer`;
* `Infrastructure.PostgreSql` come track legacy/reference;
* `WinForms` come host desktop principale;
* `Mvc` come host web secondario ma reale;
* artefatti database SQL Server e PostgreSQL;
* documentazione owner di governo.

Questa scelta ha un vantaggio importante: impedisce che il progetto diventi un’applicazione WinForms monolitica con query, logiche di filtro, gestione di stato e presentazione tutte concentrate nel form. Una soluzione del genere avrebbe potuto funzionare nel breve periodo, ma sarebbe stata molto più fragile, meno riusabile e incoerente con la richiesta di convertibilità web.

---

## 2. Perché l’architettura a layer era la scelta giusta

La scelta di separare il progetto in layer non nasce da una preferenza estetica, ma da un bisogno concreto.

Il test richiedeva contemporaneamente:

* persistenza strutturata;
* comportamento UI desktop;
* predisposizione web;
* filtro, navigazione, griglia, configurazione;
* sostenibilità tecnica e chiarezza architetturale.

Per soddisfare tutto questo, la soluzione doveva evitare due errori classici:

1. mettere tutta la logica dentro WinForms;
2. duplicare la stessa logica tra WinForms e MVC.

La struttura a layer risolve questi due problemi.

### 2.1 `Domain`

Il layer `Domain` contiene il modello minimo del problema.

Le entità principali sono:

* `BodyPart`;
* `Room`;
* `Exam`;
* `ExamRoom`.

Qui la scelta progettuale è volutamente semplice. Il dominio non contiene regole complesse o servizi ricchi, perché il caso d’uso del test non richiede una logica di dominio sofisticata. Il suo compito principale è rappresentare correttamente le entità e le relazioni fondamentali.

Non sempre una buona architettura significa introdurre più astrazioni possibili. In questo caso, la scelta corretta è stata mantenere il dominio puro, minimale e indipendente da UI, SQL concreto e host.

### 2.2 `Application`

Il layer `Application` contiene i contratti del caso d’uso.

Qui si trovano:

* `ExamNavigationRequest`;
* `ExamNavigationResult`;
* `LookupItem`;
* `ExamListItem`;
* `ExamSearchField`;
* `IExamNavigationService`.

Questo layer è il boundary applicativo. È il punto di contatto tra gli host e la logica di navigazione.

La scelta decisiva è stata non inserire qui dettagli SQL o dipendenze dai form o dai controller. `Application` definisce che cosa serve agli host per lavorare, non come quel lavoro viene svolto sul database.

Questa separazione è molto importante, perché consente di dire che WinForms e MVC dipendono dallo stesso contratto applicativo, pur essendo host diversi.

### 2.3 `Infrastructure`

Il layer infrastructure è il luogo in cui la logica applicativa viene collegata a un runtime concreto di persistenza.

Nel progetto esistono due track:

* `ExamNavigator.Infrastructure.SqlServer`;
* `ExamNavigator.Infrastructure.PostgreSql`.

La track attiva corrente è quella SQL Server.

La track PostgreSQL resta invece nel repository come memoria precedente e come riferimento tecnico, ma non governa più il runtime attivo degli host.

Il progetto ha attraversato una fase in cui PostgreSQL era stato adottato come runtime locale concreto, ma in seguito è stato introdotto un adapter SQL Server dedicato per riallineare la soluzione alla formulazione letterale della missione.

### 2.4 Host `WinForms`

L’host WinForms è l’interfaccia primaria richiesta dal test.

Il suo compito è:

* mostrare i tre pannelli;
* orchestrare la selezione a cascata;
* attivare la ricerca;
* gestire la griglia delle selezioni;
* caricare i default di ricerca da configurazione;
* delegare il recupero dei dati al boundary applicativo.

Qui il punto importante è che WinForms non è il luogo in cui si decide la logica di filtraggio del database. Il form gestisce stato di presentazione, eventi UI e rendering, ma non diventa il centro architetturale dell’intero sistema.

### 2.5 Host `Mvc`

L’host MVC ha il compito di dimostrare che la stessa base applicativa può essere riusata anche sul web.

Non è soltanto una prova teorica di convertibilità: è un host reale, con controller, page model, griglia, ricerca e navigazione a fragment.

Questo è uno dei punti più forti del progetto. Significa che la richiesta “convertito per il funzionamento web in .NET Core MVC” è stata già concretamente materializzata.

---

## 3. Moduli e responsabilità

Una lettura architetturale completa deve chiarire non solo quali file esistono, ma quale responsabilità vive in ciascun modulo.

### 3.1 Responsabilità del dominio

Il dominio modella le entità del problema.

Non deve:

* conoscere SQL Server;
* conoscere PostgreSQL;
* conoscere WinForms;
* conoscere MVC;
* conoscere il file `.ini`.

Il dominio serve a mantenere pulito il nucleo concettuale del progetto.

### 3.2 Responsabilità dell’application layer

L’application layer definisce il contratto del caso d’uso di navigazione.

Non decide come aprire connessioni SQL, non costruisce form, non renderizza viste. Definisce solo:

* che cosa serve in input;
* che cosa viene prodotto in output;
* quale interfaccia un servizio concreto deve implementare.

### 3.3 Responsabilità dell’infrastructure SQL Server

L’infrastructure SQL Server contiene la logica concreta di accesso dati per:

* ambulatori;
* parti del corpo;
* esami;
* filtri di ricerca;
* fallback di selezione.

Qui vive `SqlServerExamNavigationService`, che implementa `IExamNavigationService`.

Il suo compito è tradurre il contratto applicativo in query SQL Server reali. In altre parole, è il punto in cui il modello astratto della navigazione incontra il database concreto.

### 3.4 Responsabilità dell’infrastructure PostgreSQL

L’infrastructure PostgreSQL ha oggi una responsabilità storica e documentale. Mostra la fase in cui la soluzione è stata concretamente cablata su PostgreSQL. Non è però più il punto di verità del runtime attivo.

### 3.5 Responsabilità dell’host WinForms

WinForms è il layer di presentazione desktop.

Gestisce:

* eventi utente;
* popolamento pannelli;
* aggiornamento selezioni;
* rendering della griglia;
* carico dei default di ricerca;
* owner draw e leggibilità del pannello `Esami`.

Non deve però diventare un contenitore di logica dati ad alto accoppiamento.

### 3.6 Responsabilità dell’host MVC

L’host MVC gestisce:

* binding dei parametri GET;
* mapping del risultato applicativo in un page model;
* gestione dei comandi di selezione e della griglia;
* rendering full page o fragment;
* navigazione incrementale lato browser tramite `fetch`.

È importante sottolineare che il controller non ricalcola la logica dati da zero: interroga il boundary applicativo e poi orchestra la vista.

---

## 4. Modello dati e logica relazionale

Il modello dati del progetto è semplice ma corretto.

### 4.1 Entità principali

#### `BodyPart`

Rappresenta la parte del corpo cui un esame appartiene.

#### `Room`

Rappresenta l’ambulatorio o stanza in cui un esame può essere effettuato.

#### `Exam`

Contiene:

* identificativo;
* `CodiceMinisteriale`;
* `CodiceInterno`;
* `DescrizioneEsame`;
* `BodyPartId`.

#### `ExamRoom`

Rappresenta la tabella ponte che associa un esame a uno o più ambulatori.

### 4.2 Relazioni

Il modello implementa due relazioni fondamentali:

* uno-a-molti tra `BodyPart` ed `Exam`;
* molti-a-molti tra `Exam` e `Room` tramite `ExamRoom`.

Questo è esattamente ciò che serve per rispettare la missione. Ogni esame appartiene a una sola parte del corpo, ma può essere eseguibile in più ambulatori.

### 4.3 Vincoli SQL

Lo schema SQL Server introduce:

* primary key;
* foreign key;
* vincoli univoci;
* tabella ponte con chiave composta.

Questo rende il modello non solo concettualmente corretto, ma anche formalmente coerente sul piano relazionale.

---

## 5. Analisi di `database/sql/*.sql`

Questa parte è molto importante, perché spiega non solo il codice C#, ma anche il modo in cui il backend dati è stato concepito.

### 5.1 `001_schema.sql`

Questo file definisce lo schema SQL Server.

La sua funzione è creare:

* `BodyPart`;
* `Room`;
* `Exam`;
* `ExamRoom`.

Dal punto di vista logico, è la traduzione diretta del dominio in forma relazionale.

Lo schema non è casuale: riflette esattamente la struttura del problema. La tabella `ExamRoom` è la conseguenza naturale del fatto che un esame può comparire in più ambulatori.

### 5.2 `002_seed.sql`

Questo file popola il database con dati demo.

Il suo ruolo non è semplicemente “riempire il database”, ma creare una baseline che consenta di dimostrare:

* cascata tra i pannelli;
* presenza di più ambulatori;
* presenza di più parti del corpo;
* presenza di esami condivisi tra ambienti diversi;
* ricerca significativa.

Nella fase SQL Server finale il seed è stato anche arricchito per rendere la demo più robusta.

### 5.3 `003_navigation_queries.sql`

Questo file è particolarmente rilevante perché mostra il cuore della logica di navigazione.

Contiene query di riferimento per:

* elenco ambulatori visibili;
* elenco parti del corpo visibili;
* elenco esami visibili.

La logica è costruita in modo da rispettare sia la cascata sia il filtro testuale.

La presenza della ricerca case-insensitive è resa esplicita. Questo dettaglio è importante, perché traduce direttamente il requisito della missione in comportamento SQL verificabile.

### 5.4 `sqlserver.md`

Questo documento tecnico descrive il bootstrap locale SQL Server.

Il suo valore non è solo operativo, ma anche espositivo: consente di mostrare che il runtime SQL Server concreto non è una dichiarazione teorica, ma un percorso realmente eseguibile, con:

* creazione database;
* applicazione schema;
* applicazione seed;
* verifica conteggi;
* variabile ambiente condivisa;
* avvio dei due host.

---

## 6. Analisi di `database/postgresql/*.sql`

La presenza della track PostgreSQL va spiegata con precisione.

### 6.1 Perché esiste

Esiste perché il progetto ha attraversato una fase in cui PostgreSQL è stato adottato come runtime locale concreto.

### 6.2 Qual è il suo stato oggi

Gli artefatti PostgreSQL restano nel repository come:

* heritage track;
* memoria tecnica del pivot precedente;
* riferimento architetturale e storico.

Non governano più però il runtime attivo degli host; la loro presenza mostra la differenza tra:

* una soluzione tecnicamente funzionante in una certa fase del progetto;
* una soluzione pienamente riallineata alla formulazione letterale della missione.

In altre parole, il progetto è cresciuto anche attraverso una rivalutazione critica delle proprie scelte runtime.

---

## 7. Logica del backend di navigazione

Quando qui si parla di “backend”, non si intende un backend HTTP separato in stile API REST, ma la logica di risoluzione dati che alimenta gli host.

Il cuore di questa logica è il servizio che implementa `IExamNavigationService`.

### 7.1 Input

L’input è rappresentato da `ExamNavigationRequest`, che contiene:

* ambulatorio selezionato;
* parte del corpo selezionata;
* testo di ricerca;
* campo di ricerca.

### 7.2 Output

L’output è `ExamNavigationResult`, che contiene:

* lista degli ambulatori disponibili;
* lista delle parti del corpo disponibili;
* lista degli esami disponibili;
* selezioni correnti risolte.

### 7.3 Valore progettuale

Questa scelta è molto forte perché centralizza la risoluzione del caso d’uso. Gli host non devono conoscere i dettagli di come si popolano i pannelli: ricevono un risultato già coerente con lo stato corrente della navigazione.

Questo è ciò che rende possibile la riusabilità tra desktop e web.

---

## 8. Scelte di sintassi e stile implementativo

I principi sintattici e stilistici più rilevanti sono questi.

### 8.1 Contratti espliciti

La presenza di request/result DTO e di un’interfaccia di servizio rende il flusso leggibile. Non si passa direttamente da UI a query sparse, ma attraverso contratti nominati e semanticamente chiari.

### 8.2 Dipendenze direzionali pulite

Le dipendenze vanno nella direzione corretta:

* `Application` dipende da `Domain`;
* `Infrastructure` dipende da `Application`;
* gli host dipendono dal boundary applicativo e dal layer infrastructure concreto quando necessario per il wiring.

### 8.3 Pragmatismo anziché overengineering

Il progetto non introduce pattern ridondanti o astrazioni premature. La struttura è seria, ma non gonfiata.

È una scelta molto coerente: il test chiedeva correttezza, convertibilità, chiarezza e solidità, non un’architettura astratta fine a se stessa.

---

## 9. Pattern e scelte riconducibili

Anche se il progetto non è stato costruito come catalogo GOF, alcuni pattern o principi sono chiaramente riconoscibili.

### 9.1 Layered Architecture

È il pattern principale. Ogni layer ha una responsabilità diversa e dipendenze relativamente controllate.

### 9.2 Ports and Adapters in forma leggera

`IExamNavigationService` funge da porta applicativa. Le implementazioni concrete SQL Server e PostgreSQL possono essere lette come adapter verso diversi runtime dati.

Non è una Clean Architecture rigida nel senso più teorico; può essere ricondotta, in forma leggera, a un insieme di principi vicini a Layered Architecture, Ports and Adapters e Separation of Concerns.

### 9.3 Separation of Concerns

L’intero progetto mostra una chiara separazione tra:

* dominio;
* contratti applicativi;
* persistenza concreta;
* host di presentazione;
* documentazione di governo.

---

## 10. Struttura MVC dell’host web

Il progetto consente anche di leggere MVC in modo concreto attraverso un caso reale.

### 10.1 Model

Nel caso MVC di questo progetto, il “model” non coincide solo con il dominio, ma include anche i view model necessari alla pagina, per esempio il page model che rappresenta il contenuto della UI web.

### 10.2 Controller

`HomeController` riceve la richiesta, costruisce il `ExamNavigationRequest`, invoca il servizio applicativo, traduce il risultato in dati utili alla vista e decide se restituire una pagina completa o un fragment.

Il controller quindi:

* non contiene query SQL;
* non contiene il dominio;
* non calcola da solo la cascata;
* orchestra il flusso della richiesta.

Questo è esattamente il modo corretto di gestire il ruolo di un controller MVC.

### 10.3 View

Le view si dividono in:

* shell principale;
* fragment reale della pagina.

La shell gestisce il contenitore generale e lo script di navigazione incrementale.

Il fragment contiene il markup effettivo della ricerca, dei tre pannelli, della griglia e dei comandi.

Questa separazione è molto utile perché permette di evitare reload completi della pagina e di mantenere la UI più fluida.

### 10.4 Esempi base su controller MVC

La struttura MVC dell’host web può essere descritta in modo sintetico così.

Un controller MVC:

1. riceve input dalla richiesta HTTP;
2. interpreta i parametri;
3. invoca il servizio che conosce il caso d’uso;
4. prepara i dati per la view;
5. restituisce HTML completo o parziale.

Nel tuo progetto questo si vede bene:

* i parametri GET rappresentano lo stato della navigazione;
* il controller costruisce la request applicativa;
* il servizio restituisce il risultato coerente;
* il controller crea il page model;
* la view renderizza il contenuto.

---

## 11. Dipendenze principali e loro razionale

Le dipendenze principali del progetto non sono molte, ma sono significative.

### 11.1 `Microsoft.Data.SqlClient`

Serve per il runtime SQL Server concreto. È una scelta coerente con lo stack .NET e con il requisito finale di conformità a SQL Server.

### 11.2 `Npgsql`

È presente nella track PostgreSQL legacy/reference. Il suo ruolo oggi non è più quello di governare il runtime attivo, ma resta come parte della storia tecnica del repository.

---

## 12. Rischi tecnici residui

Una lettura matura dell’architettura non nasconde i punti ancora migliorabili.

I principali sono:

* assenza di test dedicati;
* assenza di lint e coverage;
* presenza del fallback legacy `BootstrapNavigationService` nel client WinForms;
* configurazione `.ini` ancora limitata alla baseline della ricerca;
* permanenza della track PostgreSQL, che richiede chiarezza documentale per non generare ambiguità.

---

## Conclusione

Il valore architetturale del progetto non sta nell’aver adottato un formalismo pesante, ma nell’aver costruito una soluzione semplice, coerente e solida.

Le scelte principali sono tutte riconducibili a bisogni reali:

* il dominio è minimale perché il problema lo consente;
* l’application layer esiste per proteggere il caso d’uso e riusarlo tra host;
* l’infrastructure esiste per isolare il runtime concreto;
* gli host sono separati per rispettare desktop e convertibilità web;
* il modello dati è corretto rispetto alle relazioni richieste;
* la configurazione `.ini` è stata implementata in modo riflessivo e type-safe;
* il riallineamento finale verso SQL Server dimostra capacità di correggere il progetto senza distruggerne la struttura.

In sintesi, la soluzione è solida perché non è solo funzionante: è leggibile, stratificata il giusto e coerente con la missione da cui è nata.
