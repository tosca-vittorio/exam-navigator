# 03 — Flussi principali, sintassi rilevante e ragionamento implementativo

## Finalità del documento

Questo documento descrive le parti più rilevanti del codice, i flussi principali e le scelte sintattiche e strutturali che governano il progetto.

L’obiettivo è chiarire dove vivono le responsabilità principali, quali classi e firme costituiscono l’asse della soluzione, come si muovono i dati tra UI, Application, Infrastructure e database e perché tali scelte risultano coerenti con il problema affrontato.

---

## 1. Mappa minima delle classi

Se dovessimo ridurre tutto il progetto a un set minimo di elementi da approfondire, i più importanti sono questi:

### Layer `Domain`

* `BodyPart`
* `Room`
* `Exam`
* `ExamRoom`

### Layer `Application`

* `ExamSearchField`
* `ExamNavigationRequest`
* `ExamNavigationResult`
* `LookupItem`
* `ExamListItem`
* `IExamNavigationService`

### Layer `Infrastructure.SqlServer`

* `SqlServerExamNavigationService`

### Layer `Infrastructure.PostgreSql` (storico / reference)

* `PostgreSqlExamNavigationService`

### Host WinForms

* `Program.cs`
* `Form1`
* `Predefiniti_Ricerca`
* `IniConfigurationDocument`
* `IniConfigurationBinder`
* `BootstrapNavigationService` come fallback legacy in-memory

### Host MVC

* `Program.cs`
* `HomeController`
* `Index.cshtml`
* `_ExamNavigationPage.cshtml`
* page model / command model dedicati

Questa è la spina dorsale del progetto.

---

## 2. Sintassi e senso del layer `Domain`

Il layer `Domain` è volutamente semplice. Le classi non devono contenere logica infrastrutturale o UI, ma modellare il problema.

Un esempio concettuale è questo:

```csharp
public sealed class Exam
{
    public int Id { get; set; }
    public string CodiceMinisteriale { get; set; }
    public string CodiceInterno { get; set; }
    public string DescrizioneEsame { get; set; }
    public int BodyPartId { get; set; }
}
```

Il significato tecnico di questa sintassi è semplice:

* la classe rappresenta un’entità di dominio;
* le proprietà descrivono stato, non comportamento UI;
* la presenza di `BodyPartId` collega l’esame alla parte del corpo;
* la relazione con gli ambulatori non è diretta nella classe, ma passa attraverso `ExamRoom`, perché è una relazione molti-a-molti.

Questa scelta è corretta perché evita di forzare nella singola entità una struttura che appartiene in realtà al modello relazionale.

> Il problema del test non richiedeva regole di dominio complesse o un rich domain model. Richiedeva invece una modellazione chiara, corretta e riusabile. Per questo si è mantenuto il dominio piccolo, pulito e indipendente da host e persistenza.

---

## 3. Sintassi e senso del layer `Application`

Il layer `Application` è il boundary del caso d’uso.

### 3.1 `ExamSearchField`

Dal punto di vista sintattico è un enum.

```csharp
public enum ExamSearchField
{
    CodiceMinisteriale,
    CodiceInterno,
    DescrizioneEsame
}
```

È importante perché evita stringhe arbitrarie sparse nel codice e rende espliciti i tre soli campi di ricerca ammessi.

### 3.2 `ExamNavigationRequest`

Questa classe è il contenitore dell’input applicativo.

```csharp
public sealed class ExamNavigationRequest
{
    public int? SelectedRoomId { get; set; }
    public int? SelectedBodyPartId { get; set; }
    public string SearchText { get; set; }
    public ExamSearchField SearchField { get; set; }
}
```

Questo significa che il servizio di navigazione riceve in un unico input:

* stato corrente dei pannelli;
* stato della ricerca.

È una scelta molto pulita, perché tutto il caso d’uso viene risolto a partire da una request unica e tipizzata.

### 3.3 `ExamNavigationResult`

Questa classe rappresenta l’output già pronto per gli host.

```csharp
public sealed class ExamNavigationResult
{
    public IReadOnlyList<LookupItem> Rooms { get; set; }
    public IReadOnlyList<LookupItem> BodyParts { get; set; }
    public IReadOnlyList<ExamListItem> Exams { get; set; }
    public int? SelectedRoomId { get; set; }
    public int? SelectedBodyPartId { get; set; }
}
```

Il suo valore progettuale è molto alto: gli host non devono ricostruire i dati da zero. Ricevono un risultato già coerente con la request.

### 3.4 `IExamNavigationService`

Questa è una delle firme più importanti dell’intero progetto.

```csharp
public interface IExamNavigationService
{
    ExamNavigationResult GetNavigation(ExamNavigationRequest request);
}
```

Il senso è il seguente:

* l’host non parla direttamente con SQL;
* l’host non conosce il provider concreto;
* l’host non decide come filtrare i dati;
* l’host chiede a un servizio applicativo di risolvere il caso d’uso.

Questa singola interfaccia rende possibile il riuso della stessa logica tra WinForms e MVC e permette l’esistenza di implementazioni diverse, per esempio PostgreSQL prima e SQL Server poi.

> Introducendo un boundary applicativo, minimo ma forte, il comportamento richiesto dal test non dipende dall’host, ma da un contratto condiviso che gli host consumano.

---

## 4. Sintassi e logica dell’host WinForms

Il progetto desktop ruota soprattutto attorno a `Program.cs` e `Form1`.

### 4.1 Wiring nel `Program.cs`

La forma concettuale attuale del bootstrap è questa:

```csharp
var connectionString = Environment.GetEnvironmentVariable(
    "EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING");

var navigationService = new SqlServerExamNavigationService(connectionString);
Application.Run(new Form1(navigationService));
```

Questo frammento è importantissimo.

Mostra infatti che:

* la connection string non è hardcoded nel form;
* il provider concreto viene deciso al bootstrap;
* il form riceve una dipendenza già pronta;
* il runtime principale del desktop è SQL Server concreto.

### 4.2 Doppio costruttore di `Form1`

Dal punto di vista architetturale, `Form1` ha due percorsi:

* il costruttore parameterless legacy;
* il costruttore che riceve il servizio applicativo.

La forma concettuale è questa:

```csharp
public Form1()
    : this(new BootstrapNavigationService())
{
}

public Form1(IExamNavigationService navigationService)
{
    _navigationService = navigationService;
    InitializeComponent();
}
```

Questo è un punto delicato.

* il costruttore principale usato dal runtime reale è quello che riceve `IExamNavigationService`;
* il costruttore senza parametri esiste ancora come fallback legacy in-memory;
* non è il bootstrap principale del runtime corrente;
* proprio per questo è stato classificato come fallback legacy e non come asse del sistema.

### 4.3 Event wiring di WinForms

Molte parti del comportamento desktop si leggono bene attraverso gli handler.

Esempi concettuali:

```csharp
btnSearch.Click += BtnSearch_Click;
btnClearSearch.Click += BtnClearSearch_Click;
txtSearchTerm.KeyDown += TxtSearchTerm_KeyDown;
btnConfirmExam.Click += BtnConfirmExam_Click;
btnRemoveSelected.Click += BtnRemoveSelected_Click;
btnMoveUp.Click += BtnMoveUp_Click;
btnMoveDown.Click += BtnMoveDown_Click;
```

* la UI espone eventi;
* il form aggancia metodi handler;
* ogni handler orchestra un’azione coerente;
* la logica di interazione resta leggibile e segmentata.

### 4.4 Flusso della ricerca in WinForms

La forma concettuale è questa:

```csharp
private void ApplySearch()
{
    var request = new ExamNavigationRequest
    {
        SelectedRoomId = GetSelectedRoomId(),
        SelectedBodyPartId = GetSelectedBodyPartId(),
        SearchText = txtSearchTerm.Text,
        SearchField = (ExamSearchField)cmbSearchField.SelectedItem
    };

    LoadNavigation(request);
}
```

* si legge lo stato della UI;
* si costruisce una request applicativa tipizzata;
* si invoca il boundary condiviso;
* il risultato viene usato per ripopolare i pannelli.

Questa è una delle parti più importanti, perché collega sintassi, logica e architettura in un unico punto.

### 4.5 Griglia selezioni

Anche la griglia segue una logica semplice e coerente:

* conferma selezione → append alla griglia;
* delete → rimozione della riga selezionata;
* up/down → scambio o riordino della riga corrente.

Qui il punto da sottolineare è che la griglia è uno stato UI di sessione, non persistenza sul database. Questa scelta è coerente con il freeze requisiti iniziale.

---

## 5. Sintassi e logica del sistema `.ini`

Questa è probabilmente una delle parti più particolari del progetto, quindi va esaminata con attenzione.

### 5.1 `Predefiniti_Ricerca`

Questa classe statica rappresenta la prima sezione concreta dei default.

Esempio concettuale:

```csharp
public static class Predefiniti_Ricerca
{
    public static string SearchText { get; set; } = string.Empty;
    public static ExamSearchField SearchField { get; set; } = ExamSearchField.DescrizioneEsame;
}
```

La sua funzione è fare da contenitore dei default applicativi iniziali.

### 5.2 `IniConfigurationDocument`

Questo componente legge il file `.ini` in modo raw.

Il suo compito è:

* riconoscere sezioni `[Sezione]`;
* leggere coppie `chiave = valore`;
* ignorare commenti `#`;
* conservare la struttura del documento in una rappresentazione intermedia.

Dal punto di vista progettuale, questa è la prima metà del problema. Prima si legge il documento, poi si fa il binding.

### 5.3 `IniConfigurationBinder`

Questo è il cuore della parte riflessiva.

La logica concettuale è questa:

```csharp
foreach (var section in document.Sections)
{
    var targetType = ResolvePredefinitiType(section.Name);
    if (targetType == null)
    {
        continue;
    }

    foreach (var entry in section.Entries)
    {
        var property = targetType.GetProperty(entry.Key, ...);
        if (property == null)
        {
            continue;
        }

        var convertedValue = ConvertValue(entry.Value, property.PropertyType);
        property.SetValue(null, convertedValue);
    }
}
```

Il significato tecnico è molto forte:

* il codice non dipende da nomi hardcoded di sezione nel binder;
* la sezione viene tradotta nel nome `Predefiniti_<Sezione>`;
* le proprietà vengono cercate per nome;
* il valore viene convertito in modo type-safe;
* chiavi non riconosciute vengono ignorate;
* proprietà non presenti mantengono il default dichiarato nel codice.

Questo dimostra che si è rispettato una richiesta non banale del test. Non si è letto un file di configurazione come puro dizionario, ma è stato implementato un meccanismo riflessivo, estendibile e coerente con la missione.

---

## 6. Sintassi e logica di `SqlServerExamNavigationService`

### 6.1 Costruttore

La forma concettuale è questa:

```csharp
public sealed class SqlServerExamNavigationService : IExamNavigationService
{
    private readonly string _connectionString;

    public SqlServerExamNavigationService(string connectionString)
    {
        _connectionString = connectionString;
    }
}
```

Questo mostra che il servizio è concretamente dipendente da SQL Server, ma riceve la configurazione dall’esterno.

È un punto importante: il servizio non decide da solo quale database usare. Riceve la connection string dal bootstrap.

### 6.2 Metodo principale

La firma centrale resta questa:

```csharp
public ExamNavigationResult GetNavigation(ExamNavigationRequest request)
```

Dal punto di vista semantico, il metodo svolge sei operazioni principali:

1. apre la connessione SQL Server;
2. risolve gli ambulatori visibili;
3. risolve le parti del corpo coerenti con selezione e ricerca;
4. risolve gli esami coerenti con stato e filtro;
5. applica fallback di selezione quando necessario;
6. restituisce un risultato già pronto.

### 6.3 Uso di `SqlConnection`, `SqlCommand`, parametri tipizzati

La forma tipica del codice è del tipo:

```csharp
using (var connection = new SqlConnection(_connectionString))
using (var command = new SqlCommand(sql, connection))
{
    command.Parameters.Add("@SelectedRoomId", SqlDbType.Int).Value = ...;
    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value = ...;

    connection.Open();

    using (var reader = command.ExecuteReader())
    {
        ...
    }
}
```

Questa sintassi mostra che:

* `using` governa correttamente il ciclo di vita delle risorse;
* `SqlConnection` incapsula la connessione al database;
* `SqlCommand` rappresenta la query SQL da eseguire;
* i parametri tipizzati evitano query fragili e migliorano chiarezza e sicurezza;
* `ExecuteReader()` consente di leggere i risultati in modo controllato.

### 6.4 Fallback di selezione

Un punto sottile ma importante è che il servizio non si limita a restituire liste. Risolve anche le selezioni correnti valide.

In pratica, se la request contiene una selezione non più coerente con il filtro o con la cascata, il servizio può riallinearla al primo elemento disponibile.

Questo è molto importante perché evita che gli host debbano reinventare la logica di coerenza dello stato.

---

## 7. Sintassi e logica dell’host MVC

L’host MVC va studiato come caso di riuso del boundary applicativo in un contesto web.

### 7.1 Wiring nel `Program.cs`

La forma concettuale è questa:

```csharp
builder.Services.AddTransient<IExamNavigationService>(
    _ => new SqlServerExamNavigationService(connectionString));
```

Il punto importante è che il controller non costruisce da sé il servizio. Lo riceve dal container DI.

Questo significa:

* inversione del controllo più pulita;
* bootstrap centralizzato;
* controller più semplice da leggere.

### 7.2 `HomeController`

Il flusso concettuale del controller è:

```csharp
public IActionResult Index(...)
{
    var request = BuildRequestFromQuery(...);
    var navigation = _navigationService.GetNavigation(request);
    var pageModel = BuildPageModel(navigation, ...);

    return RenderNavigationPage(pageModel);
}
```

Qui la cosa importante da dire è che il controller:

* non esegue query SQL;
* non conosce i dettagli della persistenza;
* costruisce la request a partire dalla query string;
* delega al servizio applicativo la risoluzione del caso d’uso;
* traduce il risultato in un model adatto alla view.

### 7.3 `ApplySelectionCommand(...)`

Questo metodo è importante perché materializza i comandi web della UI:

* conferma selezione;
* move up;
* move down;
* delete.

In pratica, sul web questi comandi non possono vivere esattamente come in WinForms, perché lo stato deve essere ricostruibile a ogni richiesta. Per questo è stato introdotto un command model dedicato.

### 7.4 `RenderNavigationPage(...)` e `IsAjaxNavigationRequest()`

Questi due punti sono il cuore del fix architetturale MVC finale.

Il senso è questo:

* se la richiesta è normale, si restituisce la pagina completa;
* se la richiesta è AJAX/fetch, si restituisce solo il fragment `_ExamNavigationPage`.

Questa scelta ha permesso di eliminare il full-page reload e il salto viewport su liste lunghe.

---

## 8. Shell MVC e fragment rendering

### 8.1 `Index.cshtml`

`Index.cshtml` oggi è una shell.

La sua funzione non è contenere tutto il markup della pagina, ma:

* ospitare il root della pagina;
* contenere lo script di navigazione incrementale;
* delegare il contenuto reale al fragment.

### 8.2 `_ExamNavigationPage.cshtml`

Questo file contiene invece il markup reale:

* form di ricerca;
* pannelli `Ambulatori`, `Parti del corpo`, `Esami`;
* pulsante di conferma selezione;
* griglia `Esami selezionati`;
* comandi della griglia.

Questa separazione è architetturalmente coerente, perché consente una UI più fluida senza trasformare il controller in una soluzione caotica.

---

## 9. Elementi JavaScript rilevanti nell’host MVC

Nell’host MVC esiste una parte di logica lato browser che vale la pena approfondire almeno a livello concettuale.

Elementi chiave:

* `fetch` per richieste incrementali;
* `AbortController` per gestire richieste concorrenti;
* `history.pushState` per mantenere una URL coerente;
* `popstate` per sincronizzare la navigazione del browser con lo stato della pagina.

> Nel desktop gran parte dello stato vive direttamente nei controlli del form. Nel web, invece, lo stato va reso ricostruibile dalla richiesta e sincronizzato con la navigazione del browser. Per questo l’host MVC usa GET, fragment rendering e aggiornamento incrementale con `fetch`.

---

## 10. Walkthrough del flusso completo end-to-end

### 10.1 Flusso WinForms

1. `Program.cs` legge la connection string SQL Server e crea `SqlServerExamNavigationService`.
2. `Program.cs` avvia `Form1` passandogli il servizio.
3. `Program.cs` carica, se presente, il file `.ini` e applica i default verso `Predefiniti_*`.
4. `Form1` inizializza la UI dai default già caricati e costruisce `ExamNavigationRequest` con stato pannelli + ricerca.
5. `IExamNavigationService` restituisce `ExamNavigationResult`.
6. `Form1` aggiorna i tre pannelli e la selezione corrente.
7. L’utente può confermare un esame, aggiungerlo in griglia, riordinarlo o eliminarlo.

### 10.2 Flusso MVC

1. `Program.cs` registra `SqlServerExamNavigationService` nel container DI.
2. Il browser invia una richiesta GET con parametri di stato.
3. `HomeController` costruisce `ExamNavigationRequest`.
4. Il servizio risolve la navigazione.
5. Il controller costruisce il page model.
6. Se serve, restituisce un fragment anziché l’intera pagina.
7. Lo script `fetch` aggiorna il contenuto della root senza full-page reload.

Questo flusso consente di leggere il progetto come sistema, non come insieme di file isolati.

---

## Conclusione

Il codice del progetto non va analizzato come un insieme di file, ma come una serie di scelte sintattiche e strutturali coerenti con un problema preciso.

Le firme importanti, i costrutti centrali e i flussi principali da sapere davvero sono questi:

* entità del dominio;
* request/result applicativi;
* `IExamNavigationService` come boundary;
* bootstrap di WinForms e MVC;
* `SqlServerExamNavigationService` come adapter concreto attivo;
* `.ini` parser + binder riflessivo;
* controller MVC e fragment rendering;
* gestione dello stato tra UI desktop e UI web.
