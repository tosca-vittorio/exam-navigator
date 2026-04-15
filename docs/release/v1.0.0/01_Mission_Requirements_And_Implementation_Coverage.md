# 01 — Missione, requisiti e implementazione

## Finalità del documento

Il presente documento descrive la relazione tra missione sorgente, requisiti derivati e implementazione realizzata, evidenziando la corrispondenza tra richieste funzionali, scelte progettuali e soluzione effettivamente costruita.

Il documento inquadra il sistema dal punto di vista dei requisiti coperti, della struttura adottata, delle principali decisioni implementative e dei limiti residui ancora presenti.

---

## 1. Richieste della missione

Si richiedeva la realizzazione di un programma in C# WinForms, con dati gestiti in ambiente SQL Server, capace di guidare la selezione di esami medici attraverso tre dimensioni collegate tra loro:

* ambulatori;
* parti del corpo;
* esami.

Ogni esame doveva possedere tre attributi principali:

* `CodiceMinisteriale`;
* `CodiceInterno`;
* `DescrizioneEsame`.

Dal punto di vista logico, ogni esame doveva appartenere a una sola parte del corpo, ma poter essere associato a uno o più ambulatori. Questa richiesta implicava già una modellazione relazionale precisa: una relazione uno-a-molti tra parte del corpo ed esame, e una relazione molti-a-molti tra esame e ambulatorio.

### 1.1 Significato logico della richiesta

La richiesta sorgente esplicita che gli esami sono legati **a una singola parte del corpo** e **a uno o più ambulatori**. Questa interpretazione è poi stata congelata anche nei requisiti strutturati: “Ogni esame appartiene a una sola parte del corpo; può essere associato a uno o più ambulatori.”
La frase va letta così:

* una **parte del corpo** può avere **molti esami**
* un **esame** può riferirsi a **una sola parte del corpo**
* un **ambulatorio** può erogare **molti esami**
* uno **stesso esame** può essere erogato in **più ambulatori**

Da qui derivano due cardinalità diverse:

#### A. Parte del corpo ↔ Esame = uno-a-molti

Una parte del corpo può comparire in molti esami, mentre ogni esame appartiene a una sola parte del corpo.

Esempio:

* **Addome** → Eco Addome, TAC Addome, RM Addome
* **Testa** → RMN Cranio, RX Seni Paranasali

Quindi:

* **1 BodyPart** → **N Exam**
* **N Exam** → **1 BodyPart**

#### B. Esame ↔ Ambulatorio = molti-a-molti

Uno stesso esame può essere eseguito in più ambulatori, e uno stesso ambulatorio può ospitare molti esami.

Esempio:

* **Eco Addome** può essere fatto in:

  * EcografiaPrivitera
  * EcografiaMassimino

Ma anche:

* **EcografiaPrivitera** può fare:

  * Eco Addome
  * Eco Tiroide
  * Eco Rene
  * Eco Fegato

Quindi:

* **1 Exam** → **N Room**
* **1 Room** → **N Exam**

---

### 1.2 Disegno relazionale

In un database relazionale il **molti-a-molti non si implementa direttamente**. Va sempre spezzato con una tabella ponte.

Il modello relazionale corretto diventa questo:

```text
+------------------+
|    BODY_PART     |
+------------------+
| Id (PK)          |
| Name             |
+------------------+
         |
         | 1
         |
         | N
+------------------+
|       EXAM       |
+------------------+
| Id (PK)          |
| CodiceMinisteriale |
| CodiceInterno      |
| DescrizioneEsame   |
| BodyPartId (FK)    | ---> BODY_PART.Id
+------------------+
         |
         | 1
         |
         | N
+------------------+
|    EXAM_ROOM     |
+------------------+
| ExamId (FK)      | ---> EXAM.Id
| RoomId (FK)      | ---> ROOM.Id
| PK (ExamId,RoomId)|
+------------------+
         |
         | N
         |
         | 1
+------------------+
|       ROOM       |
+------------------+
| Id (PK)          |
| Name             |
+------------------+
```

---

### 1.3 Perché si modella così

#### Parte del corpo: relazione 1:N

Qui basta una **foreign key** dentro `Exam`.

In pratica, ogni riga di `Exam` contiene un solo `BodyPartId`.

Esempio:

```text
EXAM
Id | DescrizioneEsame | BodyPartId
---+------------------+-----------
1  | Eco Addome       | 3
2  | RMN Cranio       | 1
3  | RX Mano Dx       | 2
```

Qui:

* `Eco Addome` appartiene ad `Addome`
* `RMN Cranio` appartiene a `Testa`
* `RX Mano Dx` appartiene a `Arti superiori`

Questa è la firma tipica di una relazione **uno-a-molti**:

* il lato “molti” contiene la chiave esterna del lato “uno”.


#### Ambulatorio: relazione N:N

Qui una sola foreign key non basta.

Se mettessi `RoomId` dentro `Exam`, potresti associare ogni esame a **un solo** ambulatorio, ma il requisito dice esplicitamente che un esame può stare in **più ambulatori**.

Per questo serve `ExamRoom`, cioè una tabella ponte.

Esempio:

```text
ROOM
Id | Name
---+----------------------
1  | Radiologia
2  | Risonanza
3  | EcografiaPrivitera
4  | EcografiaMassimino
```

```text
EXAM
Id | DescrizioneEsame | BodyPartId
---+------------------+-----------
1  | Eco Addome       | 3
2  | RMN Cranio       | 1
3  | RX Mano Dx       | 2
```

```text
EXAM_ROOM
ExamId | RoomId
-------+--------
1      | 3
1      | 4
2      | 2
3      | 1
```

Interpretazione:

* `Eco Addome` è associato a `EcografiaPrivitera` e `EcografiaMassimino`
* `RMN Cranio` è associato a `Risonanza`
* `RX Mano Dx` è associato a `Radiologia`

Questa tabella trasforma il molti-a-molti in **due uno-a-molti**:

* `Exam -> ExamRoom`
* `Room -> ExamRoom`

Ed è esattamente il modo corretto di modellare una relazione N:N in SQL.

---

### 1.4 Perché questa scelta era importante anche per la UI

Questa modellazione non serve solo al database. Serve anche a far funzionare correttamente i tre pannelli richiesti.

La UI deve comportarsi così:

1. selezioni un **ambulatorio**
2. il sistema mostra solo le **parti del corpo** che hanno esami disponibili in quell’ambulatorio
3. selezioni una **parte del corpo**
4. il sistema mostra solo gli **esami** compatibili con:

   * ambulatorio selezionato
   * parte del corpo selezionata

Anche questo è coerente con i requisiti congelati.

#### Esempio pratico

Se selezioni `EcografiaPrivitera`, il sistema non deve mostrare tutte le parti del corpo esistenti, ma solo quelle raggiungibili passando per gli esami associati a quell’ambulatorio.

Quindi il percorso logico è:

```text
ROOM
 -> EXAM_ROOM
   -> EXAM
     -> BODY_PART
```

E quando poi scegli una parte del corpo, il filtro finale è:

```text
EXAM
WHERE
  Exam appartiene al Room selezionato
  AND
  Exam appartiene al BodyPart selezionato
```

---

### 1.5 Perché non sarebbe stato corretto fare diversamente

#### Errore 1: mettere `RoomId` direttamente in `Exam`

Esempio errato:

```text
EXAM
Id | DescrizioneEsame | BodyPartId | RoomId
```

Questo modello è sbagliato rispetto ai requisiti, perché costringe ogni esame a un solo ambulatorio.

Non potresti rappresentare correttamente:

* `Eco Addome` in `EcografiaPrivitera`
* `Eco Addome` in `EcografiaMassimino`

salvo duplicare l’esame due volte. Ma duplicare l’esame sarebbe concettualmente scorretto, perché l’esame è uno solo: cambia soltanto la stanza in cui può essere eseguito.

#### Errore 2: fare anche BodyPart ↔ Exam come molti-a-molti

Sarebbe eccessivo e contrario al requisito.

La richiesta non dice:

* “un esame può appartenere a più parti del corpo”

Dice invece:

* “gli esami sono legati ... a una singola parte del corpo”

Quindi lì non c’è ambiguità: la parte del corpo è una sola per esame.

---

### 1.6 Traduzione in schema SQL

Questo è il modo naturale di tradurlo in tabelle:

```sql
CREATE TABLE BodyPart (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Room (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Exam (
    Id INT PRIMARY KEY,
    CodiceMinisteriale NVARCHAR(10) NOT NULL,
    CodiceInterno NVARCHAR(10) NOT NULL,
    DescrizioneEsame NVARCHAR(100) NOT NULL,
    BodyPartId INT NOT NULL,
    CONSTRAINT FK_Exam_BodyPart
        FOREIGN KEY (BodyPartId) REFERENCES BodyPart(Id)
);

CREATE TABLE ExamRoom (
    ExamId INT NOT NULL,
    RoomId INT NOT NULL,
    CONSTRAINT PK_ExamRoom PRIMARY KEY (ExamId, RoomId),
    CONSTRAINT FK_ExamRoom_Exam
        FOREIGN KEY (ExamId) REFERENCES Exam(Id),
    CONSTRAINT FK_ExamRoom_Room
        FOREIGN KEY (RoomId) REFERENCES Room(Id)
);
```

Il frammento precedente ha valore illustrativo e rappresenta la struttura logica minima del modello. Lo schema SQL Server reale del repository è più esplicito sul piano implementativo, perché usa colonne `IDENTITY(1,1)` e include anche vincoli `UNIQUE` aggiuntivi su nomi e codici; il significato relazionale, però, resta invariato.

### 1.7 Estensione del perimetro funzionale richiesto

La missione non si limitava però alla modellazione dati. Richiedeva anche:

* una UI desktop con tre pannelli affiancati;
* aggiornamento a cascata delle selezioni;
* selezione iniziale di default del primo elemento disponibile;
* ricerca testuale case-insensitive sugli esami;
* possibilità di scegliere il campo di ricerca;
* attivazione della ricerca tramite pulsante o tasto Invio;
* reset del filtro con comando `Vedi tutti`;
* griglia di riepilogo delle selezioni confermate;
* delete e riordino delle righe della griglia;
* caricamento opzionale di un file `.ini` con binding riflessivo verso classi statiche `Predefiniti_*`;
* predisposizione alla conversione web in .NET Core MVC.

Questa richiesta, letta con attenzione, contiene in realtà tre problemi distinti ma collegati:

1. un problema di **modellazione del dominio e dei dati**;
2. un problema di **orchestrazione applicativa e filtraggio**;
3. un problema di **strutturazione architetturale**, perché il software doveva nascere desktop ma già pensato per una successiva estensione web.

---

## 2. Come la missione è stata trasformata in requisiti verificabili

Per evitare un’implementazione intuitiva o improvvisata, il testo originale è stato prima congelato come sorgente missione e poi trasformato in un requirements freeze strutturato.

Questo passaggio è stato importante perché ha consentito di separare tre livelli:

* il testo grezzo della richiesta;
* i requisiti funzionali e non funzionali derivati;
* la successiva implementazione tecnica.

In pratica, invece di partire subito dal codice, il lavoro è stato impostato così:

1. congelamento della missione sorgente;
2. derivazione di requisiti numerati e difendibili;
3. pianificazione progressiva dei blocchi di lavoro;
4. implementazione incrementale;
5. riallineamento continuo della documentazione owner allo stato reale del repository.

Questo approccio permette di sostenere che il progetto è stato sviluppato con una costruzione guidata da un perimetro esplicito.

---

## 3. Copertura dei requisiti funzionali principali

### 3.1 Gestione delle anagrafiche di base

La soluzione gestisce le tre anagrafiche richieste:

* `BodyPart`;
* `Room`;
* `Exam`.

A queste si aggiunge `ExamRoom`, che realizza la relazione molti-a-molti tra esami e ambulatori.

Questa scelta non è accessoria, ma deriva direttamente dalla struttura logica della missione. Senza una tabella ponte, infatti, non sarebbe possibile rappresentare in modo corretto il fatto che uno stesso esame possa essere eseguibile in più ambulatori.

### 3.2 Attributi degli esami

Gli esami espongono i tre attributi richiesti dalla missione:

* `CodiceMinisteriale`;
* `CodiceInterno`;
* `DescrizioneEsame`.

Il modello è coerente anche a livello SQL, con vincoli e strutture che impediscono una modellazione approssimativa.

### 3.3 Selezione a cascata

La UI WinForms realizza la cascata richiesta:

* selezione dell’ambulatorio;
* aggiornamento delle parti del corpo disponibili;
* aggiornamento degli esami coerenti con ambulatorio e parte del corpo.

Questo comportamento non è stato lasciato come logica dispersa nella UI, ma è stato governato tramite il servizio `IExamNavigationService`, in modo da poter riutilizzare lo stesso criterio anche nel successivo host MVC.

### 3.4 Selezione iniziale di default

All’avvio dell’interfaccia viene selezionato automaticamente il primo elemento disponibile, e ogni aggiornamento dei pannelli a destra mantiene la stessa logica di fallback sul primo elemento valido.

### 3.5 Ricerca testuale case-insensitive

La ricerca è stata implementata con i comportamenti richiesti:

* scelta del campo di ricerca;
* filtro per testo completo o sottostringa;
* attivazione tramite pulsante `Cerca`;
* attivazione tramite pressione del tasto `Invio`;
* reset con `Vedi tutti`.

La ricerca non filtra soltanto l’ultimo pannello degli esami, ma governa l’intero contenuto dei tre pannelli in modo coerente con le selezioni correnti, come richiesto dalla missione.

### 3.6 Griglia delle selezioni confermate

La griglia riepilogativa accumula gli esami confermati e supporta:

* aggiunta della selezione corrente;
* eliminazione della riga selezionata;
* riordino verso l’alto;
* riordino verso il basso.

Questo copre in modo diretto i requisiti relativi al riepilogo delle scelte utente.

### 3.7 Configurazione `.ini`

La missione richiedeva una gestione riflessiva della configurazione, non un semplice file di settaggi letto in modo hardcoded.

Per questo la soluzione introduce:

* `IniConfigurationDocument` per il parsing raw del documento `.ini`;
* `IniConfigurationBinder` per il binding riflessivo e type-safe;
* `Predefiniti_Ricerca` come prima classe statica concreta `Predefiniti_*`;
* caricamento dei default di ricerca all’avvio.

Tutto ciò è particolarmente importante, perché dimostra che non ci si è limitati a leggere un file di testo, ma si è rispettata la richiesta più sofisticata della missione: binding per sezioni e proprietà, indipendenza da nomi concreti, rispetto dei tipi e mantenimento dei default dichiarati in codice.

### 3.8 Convertibilità web

La missione richiedeva che il programma fosse poi convertito per il funzionamento web in .NET Core MVC.

Questa richiesta è stata affrontata con una materializzazione concreta:

* esiste un host `ExamNavigator.Mvc`;
* esiste un riuso reale del boundary applicativo;
* esiste una baseline funzionale equivalente per navigazione, ricerca, conferma selezione, griglia e comandi principali;
* l’host MVC è oggi wired allo stesso runtime SQL Server concreto del client WinForms.

Questo è uno dei punti più forti dell’intero progetto, perché mostra che la convertibilità web non è rimasta soltanto una predisposizione teorica.

---

## 4. Perché la soluzione non è stata costruita come un singolo progetto WinForms monolitico

Una delle scelte più importanti è stata evitare una soluzione tutta concentrata nel form desktop.

La strada apparentemente più semplice sarebbe stata:

* database;
* form WinForms;
* query e logica direttamente dentro la UI.

Questa soluzione sarebbe stata più rapida nel brevissimo periodo, ma sarebbe risultata più fragile, meno difendibile e soprattutto incoerente con la futura conversione web richiesta dal test.

Per questo è stato scelto un assetto architetturale a layer:

* `Domain`;
* `Application`;
* `Infrastructure`;
* host specifici.

Questa decisione risolve contemporaneamente più problemi:

1. separa il modello di dominio dalla tecnologia di persistenza;
2. separa i contratti applicativi dagli host;
3. rende possibile riutilizzare la stessa logica sia in WinForms sia in MVC;
4. evita duplicazioni di logica di navigazione e ricerca;
5. rende il progetto più spiegabile dal punto di vista ingegneristico.

**La missione chiedeva WinForms, ma chiedeva anche convertibilità web; quindi la struttura doveva essere pensata fin dall’inizio per non legare la logica del caso d’uso a un solo host**.

---

## 5. Progressione reale dell’implementazione

La soluzione è stata sviluppata in progressione, non tutta insieme.

La traiettoria reale è stata, in sintesi:

1. bootstrap repository e freeze requisiti;
2. costruzione del core condiviso (`Domain` + `Application`);
3. baseline SQL Server con schema, seed e query di riferimento;
4. host WinForms baseline;
5. cascata, conferma selezione, delete e reorder;
6. ricerca e configurazione `.ini`;
7. host MVC e baseline funzionale web;
8. pivot runtime locale PostgreSQL, con successiva chiusura V1 mission-critical;
9. gate di coerenza finale;
10. riallineamento conclusivo al requisito SQL Server con infrastructure concreta dedicata;
11. chiusura del preflight/demo locale controllata;
12. avvio della fase successiva di consolidamento documentale.

Questa storia mostra che il progetto non è stato statico: è stato invece verificato, corretto e riallineato in modo truth-first quando emergeva un gap reale.

Il caso più significativo è il riallineamento finale verso SQL Server. A un certo punto la baseline PostgreSQL era tecnicamente difendibile come runtime concreto locale, ma non coincideva più con la formulazione letterale della missione. Invece di nascondere il problema, il progetto ha riaperto esplicitamente il blocco corretto e ha introdotto un layer SQL Server concreto, fino a chiudere anche questo gap.

Questo è un elemento di forte maturità tecnica e metodologica.

---

## 6. Il valore tecnico del riallineamento finale verso SQL Server

Uno dei punti più interessanti del progetto è proprio il fatto che non si è fermato alla prima soluzione funzionante.

La baseline PostgreSQL aveva risolto il problema del runtime concreto locale e aveva consentito di completare una V1 molto solida. Tuttavia, la missione originaria chiedeva esplicitamente SQL Server oppure una forma importabile in SQL Server.

Da qui nasce la decisione di introdurre:

* `ExamNavigator.Infrastructure.SqlServer`;
* `SqlServerExamNavigationService`;
* wiring SQL Server concreto per entrambi gli host;
* bootstrap locale documentato in `database/sql/sqlserver.md`;
* stessa variabile ambiente condivisa per WinForms e MVC.

Questo passaggio dimostra due cose:

1. capacità di consegnare una soluzione realmente aderente al requisito testuale;
2. capacità di riallineare il progetto senza rompere il core condiviso e senza rifare tutta l’architettura.

Il progetto era già forte dal punto di vista architetturale, ma è stato ulteriormente rafforzato chiudendo il gap di conformità letterale verso SQL Server con un adapter dedicato e senza compromettere la separazione dei livelli.

---

## 7. Punti forti

I punti forti più importanti sono:

* copertura reale dei requisiti principali della missione;
* presenza di una modellazione dominio/dati coerente;
* separazione tra dominio, contratti applicativi, infrastructure e host;
* convertibilità web non solo prevista ma concretamente materializzata;
* sistema di configurazione `.ini` riflessivo e type-safe;
* riallineamento finale al requisito SQL Server;
* documentazione owner solida per tracciabilità, audit e handoff;
* demo locale controllata già verificata.

---

## 8. Limiti e rischi residui

Una descrizione tecnica corretta non nasconde i limiti residui.

I principali punti ancora aperti sono:

* assenza di test unitari dedicati;
* assenza di lint, coverage e smoke automatizzati;
* presenza del fallback legacy `BootstrapNavigationService` nel client WinForms;
* track PostgreSQL ancora presente nel repository come heritage/reference, con necessità di mantenerne chiara la classificazione documentale;
* promozione formale della V1 non ancora eseguita.

Dichiarare questi punti con chiarezza non indebolisce la soluzione. Al contrario, rafforza la credibilità dell’analisi, perché mostra consapevolezza del perimetro reale del progetto.

---

## Conclusione

Il progetto risolve la missione con una soluzione che non si limita a “far funzionare” tre pannelli e una ricerca, ma costruisce un sistema coerente, estendibile e architetturalmente solido.

Ogni scelta importante è riconducibile a una necessità reale della missione:

* il modello dati deriva dalle relazioni richieste;
* il boundary applicativo deriva dalla necessità di non duplicare logica tra desktop e web;
* l’host MVC deriva dalla convertibilità richiesta;
* il binding riflessivo `.ini` deriva direttamente dal testo della missione;
* il layer SQL Server concreto deriva dal riallineamento finale alla formulazione letterale del requisito.
