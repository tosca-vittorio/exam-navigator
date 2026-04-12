-- ============================================================================
-- BLOCCO 1 — BASELINE LEGACY / NON NORMALIZZATA
-- ----------------------------------------------------------------------------

INSERT INTO public.body_part (name)
VALUES
    ('Testa'),
    ('Arti superiori'),
    ('Addome'),
    ('Torace');

INSERT INTO public.room (name)
VALUES
    ('Radiologia'),
    ('Tac1'),
    ('Tac2'),
    ('Risonanza'),
    ('EcografiaPrivitera'),
    ('EcografiaMassimino'),
    ('EcografiaDoppler');

INSERT INTO public.exam (codice_ministeriale, codice_interno, descrizione_esame, body_part_id)
VALUES
    ('RXMDX001', 'INTRX001', 'RX mano Dx', (SELECT id FROM public.body_part WHERE name = 'Arti superiori')),
    ('RMNCR001', 'INTRM001', 'RMN cranio', (SELECT id FROM public.body_part WHERE name = 'Testa')),
    ('ECOADD01', 'INTEC001', 'Eco Addome', (SELECT id FROM public.body_part WHERE name = 'Addome')),
    ('TACTOR01', 'INTTC001', 'TAC torace', (SELECT id FROM public.body_part WHERE name = 'Torace')),
    ('ECODOP01', 'INTDP001', 'Eco Doppler TSA', (SELECT id FROM public.body_part WHERE name = 'Testa')),
    ('RXPOLS01', 'INTRX002', 'RX polso sx', (SELECT id FROM public.body_part WHERE name = 'Arti superiori'));

INSERT INTO public.exam_room (exam_id, room_id)
VALUES
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX001'), (SELECT id FROM public.room WHERE name = 'Radiologia')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRM001'), (SELECT id FROM public.room WHERE name = 'Risonanza')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTEC001'), (SELECT id FROM public.room WHERE name = 'EcografiaPrivitera')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTEC001'), (SELECT id FROM public.room WHERE name = 'EcografiaMassimino')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC001'), (SELECT id FROM public.room WHERE name = 'Tac1')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC001'), (SELECT id FROM public.room WHERE name = 'Tac2')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTDP001'), (SELECT id FROM public.room WHERE name = 'EcografiaDoppler')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX002'), (SELECT id FROM public.room WHERE name = 'Radiologia'));

-- ============================================================================
-- BLOCCO 2 — POPOLAMENTO MISTO DI TEST (NORMALIZZATO + NON NORMALIZZATO)
-- ----------------------------------------------------------------------------

INSERT INTO public.body_part (name)
VALUES
    ('Collo'),
    ('Arti inferiori'),
    ('Colonna'),
    ('Pelvi');

INSERT INTO public.room (name)
VALUES
    ('RadiologiaCentrale'),
    ('Radiologia Nord'),
    ('Tac5'),
    ('Tac 6'),
    ('RisonanzaAvanzata'),
    ('Risonanza Est'),
    ('EcografiaCentrale'),
    ('Ecografia Ovest'),
    ('EcoDopplerVenoso'),
    ('Eco Doppler Arterioso');

INSERT INTO public.exam (codice_ministeriale, codice_interno, descrizione_esame, body_part_id)
VALUES
    ('RXGOMD02', 'INTRX201', 'RX gomito destro', (SELECT id FROM public.body_part WHERE name = 'Arti superiori')),
    ('RXCAVS02', 'INTRX202', 'RX caviglia sinistra', (SELECT id FROM public.body_part WHERE name = 'Arti inferiori')),
    ('RXBACI02', 'INTRX203', 'RX bacino', (SELECT id FROM public.body_part WHERE name = 'Pelvi')),
    ('RMNRACHC1', 'INTRM201', 'RMN rachide cervicale', (SELECT id FROM public.body_part WHERE name = 'Colonna')),
    ('RMNGIND1', 'INTRM202', 'RMN ginocchio destro', (SELECT id FROM public.body_part WHERE name = 'Arti inferiori')),
    ('TACADMC1', 'INTTC201', 'TAC addome con contrasto', (SELECT id FROM public.body_part WHERE name = 'Addome')),
    ('TACMASF1', 'INTTC202', 'TAC massiccio facciale', (SELECT id FROM public.body_part WHERE name = 'Testa')),
    ('TACPELV1', 'INTTC203', 'TAC pelvi senza contrasto', (SELECT id FROM public.body_part WHERE name = 'Pelvi')),
    ('ECOTIR02', 'INTEC201', 'Ecografia tiroide', (SELECT id FROM public.body_part WHERE name = 'Collo')),
    ('ECOTMCL1', 'INTEC202', 'Ecografia tessuti molli collo', (SELECT id FROM public.body_part WHERE name = 'Collo')),
    ('ECDARTI1', 'INTDP201', 'Eco Color Doppler arti inferiori', (SELECT id FROM public.body_part WHERE name = 'Arti inferiori')),
    ('ECDCARO1', 'INTDP202', 'Eco Color Doppler carotideo', (SELECT id FROM public.body_part WHERE name = 'Collo'));

INSERT INTO public.exam_room (exam_id, room_id)
VALUES
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX201'), (SELECT id FROM public.room WHERE name = 'RadiologiaCentrale')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX201'), (SELECT id FROM public.room WHERE name = 'Radiologia Nord')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX202'), (SELECT id FROM public.room WHERE name = 'RadiologiaCentrale')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX202'), (SELECT id FROM public.room WHERE name = 'Radiologia Nord')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRX203'), (SELECT id FROM public.room WHERE name = 'Radiologia Nord')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRM201'), (SELECT id FROM public.room WHERE name = 'RisonanzaAvanzata')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRM201'), (SELECT id FROM public.room WHERE name = 'Risonanza Est')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRM202'), (SELECT id FROM public.room WHERE name = 'RisonanzaAvanzata')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTRM202'), (SELECT id FROM public.room WHERE name = 'Risonanza Est')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC201'), (SELECT id FROM public.room WHERE name = 'Tac5')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC201'), (SELECT id FROM public.room WHERE name = 'Tac 6')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC202'), (SELECT id FROM public.room WHERE name = 'Tac5')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC202'), (SELECT id FROM public.room WHERE name = 'Tac 6')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTTC203'), (SELECT id FROM public.room WHERE name = 'Tac 6')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTEC201'), (SELECT id FROM public.room WHERE name = 'EcografiaCentrale')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTEC201'), (SELECT id FROM public.room WHERE name = 'Ecografia Ovest')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTEC202'), (SELECT id FROM public.room WHERE name = 'EcografiaCentrale')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTEC202'), (SELECT id FROM public.room WHERE name = 'Ecografia Ovest')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTDP201'), (SELECT id FROM public.room WHERE name = 'EcoDopplerVenoso')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTDP201'), (SELECT id FROM public.room WHERE name = 'Eco Doppler Arterioso')),

    ((SELECT id FROM public.exam WHERE codice_interno = 'INTDP202'), (SELECT id FROM public.room WHERE name = 'EcoDopplerVenoso')),
    ((SELECT id FROM public.exam WHERE codice_interno = 'INTDP202'), (SELECT id FROM public.room WHERE name = 'Eco Doppler Arterioso'));
    