INSERT INTO VRSTAORGANIZATOR (Naziv) VALUES ('Atletski klub'), ('Privatna tvrtka'), ('Udruga');

INSERT INTO STATUSUTRKE (Naziv) VALUES ('Otvorene prijave'), ('U tijeku'), ('Završena'), ('Otkazano'), ('Odgođeno');

INSERT INTO TIPKATEGORIJE (Naziv) VALUES ('Cestovna utrka'), ('Trail'), ('Humanitarna utrka');

INSERT INTO STATUSREZULTATA (Naziv) VALUES ('Prijavljen'), ('Završio'), ('Nije startao (DNS)'), ('Nije završio (DNF)');

INSERT INTO ATLETSKIKLUB (Ime, Grad, Drzava, Osnovano) VALUES
('AK Dinamo-Zrinjevac', 'Zagreb', 'Hrvatska', 1945),
('AK Split', 'Split', 'Hrvatska', 1954),
('AK Sljeme', 'Zagreb', 'Hrvatska', 2002);

INSERT INTO ORGANIZATOR (Ime, Email, Broj_mobitela, OIB, Web_stranica, Opis, VrstaOrganizatoraId) VALUES
('Zagrebački savez sportova', 'info@zss.hr', '012345678', '12345678901', 'www.zss.hr', 'Glavni savez za sport u Zagrebu', 1),
('Run Croatia', 'kontakt@runcroatia.hr', '091555666', '98765432109', 'www.runcroatia.hr', 'Organizacija međunarodnih utrka', 2),
('Wings for Life Zadar', 'zadar@wfl.hr', '023111222', '45612378900', 'www.wingsforlife.hr', 'Humanitarna zaklada', 3);

INSERT INTO TRKAC (Ime, Prezime, Datum_rodenja, Spol, Nacionalnost, Email, AKlubId) VALUES
('Ivan', 'Horvat', '1990-05-15', 'M', 'Hrvatska', 'ivan.horvat@email.hr', 1),
('Marija', 'Kovačević', '1995-08-22', 'Ž', 'Hrvatska', 'marija.k@email.hr', 1),
('Marko', 'Marić', '1985-02-10', 'M', 'Hrvatska', 'marko.maric@email.hr', 2),
('Ana', 'Rodić', '2000-11-30', 'Ž', 'Hrvatska', 'ana.rodic@email.hr', 3),
('Luka', 'Modrić', '1988-09-09', 'M', 'Hrvatska', 'luka.m@email.hr', 2),
('Petra', 'Ivić', '1992-04-04', 'Ž', 'Hrvatska', 'petra.ivic@email.hr', 3);

INSERT INTO UTRKA (Naziv, Datum, Grad, Drzava, OrganizatorId, StatusId) VALUES
('Zagrebački Maraton', '2026-10-11', 'Zagreb', 'Hrvatska', 1, 1),
('Splitski Maraton', '2026-02-22', 'Split', 'Hrvatska', 2, 1),
('Wings for Life', '2026-05-03', 'Zadar', 'Hrvatska', 3, 1),
('Noćni Maraton Zagreb', '2026-08-15', 'Zagreb', 'Hrvatska', 2, 1),
('Riječka utrka', '2026-04-12', 'Rijeka', 'Hrvatska', 1, 1),
('Dubrovnik Run', '2026-09-20', 'Dubrovnik', 'Hrvatska', 2, 1);

INSERT INTO KATEGORIJA (Naziv, Duljina, Max_broj_trkaca, Startnina, Početak, UtrkaId, TipId) VALUES
('Glavni Maraton', 42.2, 1000, 50.00, '2026-10-11', 1, 1),
('Polumaraton', 21.1, 2000, 35.00, '2026-10-11', 1, 1),
('Građanska petica', 5.0, 5000, 15.00, '2026-10-11', 1, 1),
('ST Polumaraton', 21.1, 1500, 30.00, '2026-02-22', 2, 1),
('Zadar Wheelchair', 100.0, 8000, 20.00, '2026-05-03', 3, 3),
('Noćna 10-ka', 10.0, 500, 25.00, '2026-08-15', 4, 1);

INSERT INTO REZULTAT (Finalno_vrijeme, StatusRezultataId, TrkacId, KategorijaId) VALUES
(NULL, 1, 1, 1), 
(NULL, 1, 1, 2), 
('00:22:15', 2, 3, 3), 
(NULL, 1, 4, 5), 
(NULL, 1, 5, 4), 
(NULL, 4, 6, 6); 

INSERT INTO TRENING (Lokacija, Trajanje, Duljina, TrkacId) VALUES
('Park Maksimir', '00:45:00', 8.5, 1),
('Marjan', '01:10:00', 12.0, 2),
('Jarun', '00:30:00', 5.0, 3),
('Nasip', '02:00:00', 21.0, 1),
('Bačvice', '00:20:00', 3.0, 5),
('Sljeme - planinarska staza', '01:30:00', 10.2, 6);