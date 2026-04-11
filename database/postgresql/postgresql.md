# Setup locale PostgreSQL runtime per ExamNavigator

## Scopo e contesto

Questo documento descrive la procedura tecnica adottata per usare PostgreSQL come database runtime locale del progetto `exam-navigator`, in sostituzione del percorso SQL Server/LocalDB inizialmente esplorato.

Obiettivi di questo step:

1. definire il motore database runtime scelto per l’ambiente locale;
2. creare database e utente dedicati;
3. fissare i parametri di connessione applicativa;
4. preparare il terreno per il successivo wiring applicativo di WinForms e MVC verso PostgreSQL reale;
5. documentare in modo truth-first una deviazione consapevole rispetto al percorso SQL Server inizialmente avviato.

## Nota di compatibilità rispetto alla missione originale

La richiesta iniziale menziona SQL Server come ambiente dati di riferimento. La scelta di PostgreSQL come runtime locale viene adottata come decisione tecnica esplicita di progetto, da accompagnare con documentazione chiara e con una successiva strategia difendibile di portabilità/convertibilità verso SQL Server, se richiesta in sede di consegna o colloquio.

Questa decisione non deve essere implicita: deve risultare tracciata nei documenti owner.

## Stato precedente superato

Prima di questa decisione, il progetto aveva iniziato un wiring runtime verso SQL Server LocalDB.

Verità del checkpoint precedente:

- il packaging runtime WinForms era stato portato fino al punto di apertura reale di `SqlConnection`;
- l’istanza locale disponibile risultava `MSSQLLocalDB`;
- non risultava presente il database `ExamNavigator`;
- il percorso SQL Server era quindi ancora incompleto lato setup database;
- a questo punto il progetto ha deciso di adottare PostgreSQL come runtime locale principale.

## Parametri di riferimento PostgreSQL

| Elemento | Valore |
|---|---|
| Database | `exam_navigator` |
| Utente applicativo | `exam_navigator_app` |
| Password locale | `<DB_PASSWORD>` |
| Host | `localhost` |
| Porta | `5432` |
| Schema target | `public` |

## Stato reale già raggiunto

I seguenti passaggi risultano già eseguiti con successo in ambiente locale PostgreSQL:

```sql
CREATE USER exam_navigator_app WITH PASSWORD '<DB_PASSWORD>';
CREATE DATABASE exam_navigator;
GRANT ALL PRIVILEGES ON DATABASE exam_navigator TO exam_navigator_app;
ALTER DATABASE exam_navigator OWNER TO exam_navigator_app;
```

## Obiettivo architetturale

L’obiettivo non è legare UI o controller a PostgreSQL, ma mantenere il boundary applicativo invariato e sostituire l’adapter infrastrutturale concreto.

Direzione prevista:

* `ExamNavigator.Application` resta invariato;
* `ExamNavigator.Domain` resta invariato;
* il runtime DB concreto verrà implementato in infrastructure PostgreSQL;
* WinForms e MVC consumeranno lo stesso servizio applicativo tramite dependency wiring.

## Sequenza operativa prevista

### A1.1 — Verifica motore PostgreSQL locale

Verificare presenza di PostgreSQL, `psql`, servizio attivo e raggiungibilità locale.

### A1.2 — Creazione database e utente di progetto

Creare database dedicato e utenza applicativa dedicata.

### A1.3 — Permessi schema

Assegnare i privilegi necessari sullo schema `public` per consentire operazioni runtime applicative e, se necessario, setup schema.

### A1.4 — Setup schema e seed

Valutare come tradurre o rigenerare in PostgreSQL gli artefatti oggi presenti in `database/sql/001_schema.sql` e `database/sql/002_seed.sql`, nati per il percorso SQL Server.

### A1.5 — Wiring applicativo

Sostituire progressivamente il wiring SQL Server con wiring PostgreSQL, mantenendo invariati i contratti `Application`.

## Scope IN

* documentazione tecnica setup PostgreSQL locale;
* parametri runtime PostgreSQL;
* freeze della decisione tecnica;
* futura preparazione del percorso di wiring PostgreSQL.

## Scope OUT

* modifica immediata dei contratti `Application`;
* commit di riallineamento docs owner;
* implementazione immediata dell’adapter PostgreSQL;
* consegna finale;
* chiusura V1;
* rimozione immediata del lavoro SQL Server già esplorato.

## Rischi e nota metodologica

La scelta PostgreSQL è tecnicamente valida ma va gestita con disciplina, perché:

* diverge dal percorso SQL Server più letterale rispetto alla missione;
* richiede aggiornamento coerente di TIMELINE, CHANGELOG, ROADMAP, ARCHITECTURE e README quando il pivot sarà consolidato;
* richiede una strategia chiara per spiegare la scelta in sede di colloquio.

Per questo motivo, in questa fase si congela prima il documento tecnico di setup, poi si procederà ai passi runtime reali PostgreSQL.

## Esito atteso di A1

A1 sarà considerato chiuso quando saranno veri tutti i punti seguenti:

1. PostgreSQL locale verificato e raggiungibile;
2. database `exam_navigator` creato;
3. utente applicativo dedicato creato;
4. permessi principali verificati;
5. documento tecnico coerente e riusabile come riferimento operativo per il wiring successivo.
