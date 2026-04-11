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
