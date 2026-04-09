# ROADMAP — Nolex Exam Selection System

## Obiettivo generale
Consegnare una soluzione tecnicamente corretta, architetturalmente difendibile e già predisposta alla conversione web.

## Sequenza strategica

1. **Bootstrap e freeze requisiti**
   - congelare la missione;
   - derivare requisiti verificabili;
   - aprire governance repository.

2. **Core condiviso**
   - definire dominio e servizi applicativi;
   - evitare duplicazione di logica tra desktop e web.

3. **Persistenza SQL Server**
   - schema minimale ma corretto;
   - seed demo realistico;
   - query orientate al caso d’uso.

4. **WinForms first**
   - soddisfare integralmente il test desktop;
   - dimostrare correttezza della UX richiesta.

5. **Ricerca e configurazione**
   - completare filtro testuale;
   - integrare file `.ini` riflessivo.

6. **Web conversion**
   - riutilizzare il core;
   - esporre lo stesso comportamento in MVC.

## Decisioni guida
- Prima correttezza e chiarezza.
- Poi estensibilità.
- Nessun overengineering inutile.
- Nessun coupling forte tra UI e logica applicativa.
