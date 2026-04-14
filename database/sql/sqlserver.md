# SQL Server local runtime bootstrap — Exam Navigator

Documento tecnico truth-first per il bootstrap locale SQL Server del progetto `ExamNavigator`.

## Scopo del documento

Questo documento descrive il percorso operativo minimo per:

- predisporre un database SQL Server locale coerente con la baseline corrente;
- applicare schema e seed del runtime attivo;
- verificare il popolamento minimo atteso;
- agganciare entrambi gli host applicativi (`WinForms` e `MVC`) allo stesso runtime SQL Server concreto.

## Perimetro reale corrente

Alla data attuale il runtime locale attivo della soluzione è SQL Server.

Gli artefatti SQL Server correnti sono:

- `database/sql/001_schema.sql`
- `database/sql/002_seed.sql`
- `database/sql/003_navigation_queries.sql`

Ruolo degli artefatti:

- `001_schema.sql` → materializza schema, PK, FK e vincoli;
- `002_seed.sql` → popola il dataset demo SQL Server corrente;
- `003_navigation_queries.sql` → conserva le query di riferimento per cascata e ricerca; non è richiesto per il bootstrap tecnico del database, ma resta utile come documento di supporto e audit.

Gli artefatti PostgreSQL presenti in `database/postgresql/` restano nel repository come heritage/demo track del pivot precedente e **non** governano più il runtime applicativo attivo.

## Prerequisiti minimi locali

- Windows con SQL Server LocalDB disponibile;
- comando `sqlcmd` disponibile nel terminale;
- repository già clonato localmente;
- working directory posizionata alla root del repository.

Istanza locale verificata nel flusso corrente:

- `(localdb)\MSSQLLocalDB`

Database usato nel flusso corrente:

- `ExamNavigator`

## Bootstrap database locale

### Creazione database, se assente

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "IF DB_ID(N'ExamNavigator') IS NULL CREATE DATABASE [ExamNavigator];"
```

### Applicazione schema

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "ExamNavigator" -i "database/sql/001_schema.sql"
```

### Applicazione seed

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "ExamNavigator" -i "database/sql/002_seed.sql"
```

## Verifica minima del popolamento

Comando di verifica usato nel flusso reale:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "ExamNavigator" -Q "SELECT 'BodyPart' AS TableName, COUNT(*) AS TotalRows FROM dbo.BodyPart UNION ALL SELECT 'Room' AS TableName, COUNT(*) AS TotalRows FROM dbo.Room UNION ALL SELECT 'Exam' AS TableName, COUNT(*) AS TotalRows FROM dbo.Exam UNION ALL SELECT 'ExamRoom' AS TableName, COUNT(*) AS TotalRows FROM dbo.ExamRoom;"
```

Conteggi verificati nel dataset SQL Server corrente:

* `BodyPart = 4`
* `Room = 11`
* `Exam = 14`
* `ExamRoom = 30`

Questi conteggi rappresentano la baseline demo SQL Server locale oggi usata per il runtime concreto degli host.

## Wiring runtime applicativo

Entrambi gli host leggono la stessa variabile ambiente:

* `EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING`

Valore usato nel flusso locale verificato:

```bash
export EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING='Server=(localdb)\MSSQLLocalDB;Database=ExamNavigator;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;'
```

## Avvio host MVC

```bash
export EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING='Server=(localdb)\MSSQLLocalDB;Database=ExamNavigator;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;'
dotnet run --project src/ExamNavigator.Mvc --urls http://localhost:5099
```

## Avvio host WinForms

```bash
export EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING='Server=(localdb)\MSSQLLocalDB;Database=ExamNavigator;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;'
./src/ExamNavigator.WinForms/bin/Debug/ExamNavigator.WinForms.exe
```

## Quality gate minimo corrente

Gate minimo verificabile corrente:

```bash
dotnet build ExamNavigator.sln
```

Questo gate non sostituisce smoke manuali, test unitari, lint o coverage, che restano fuori dal perimetro qualità minimo oggi consolidato.

## Smoke manuale minimo raccomandato — WinForms

Verificare almeno:

* caricamento iniziale corretto di ambulatori, parti del corpo ed esami;
* aggiornamento a cascata dei tre pannelli;
* ricerca testuale con pulsante `Cerca`;
* ricerca testuale con tasto `Invio`;
* reset della ricerca con `Vedi tutti`;
* conferma selezione esame nella griglia riepilogativa;
* riordino righe con `Sposta su` / `Sposta giù`;
* eliminazione riga selezionata;
* resa leggibile e coerente dei label degli ambulatori.

## Note operative

* questo documento descrive il bootstrap locale SQL Server realmente usato dalla baseline corrente;
* non rappresenta ancora un installer, una release formale o un archivio di consegna finale;
* la soluzione resta oggi più difendibile come **bundle demo locale controllato**;
* `database/sql/003_navigation_queries.sql` è un artefatto di supporto e riferimento, non un prerequisito di esecuzione obbligatorio del bootstrap;
* gli host `WinForms` e `MVC` condividono lo stesso runtime SQL Server concreto e la stessa variabile ambiente di configurazione.
