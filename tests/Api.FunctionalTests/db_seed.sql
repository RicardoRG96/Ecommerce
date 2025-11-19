------------------------------------------------------------
-- COUNTRY
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Country] ON;

INSERT INTO [dbo].[Country] 
                ([CountryId],
                [Name],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES 
            (1, 'Chile', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Country] OFF;


------------------------------------------------------------
-- REGIONS DE CHILE (16 REGIONES)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Region] ON;

INSERT INTO [dbo].[Region] 
                ([RegionId],
                [Name],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy]) 
        VALUES
            (1, 'Arica y Parinacota', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (2, 'Tarapacá', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (3, 'Antofagasta', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (4, 'Atacama', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (5, 'Coquimbo', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (6, 'Valparaíso', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (7, 'Metropolitana de Santiago', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (8, 'O’Higgins', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (9, 'Maule', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (10, 'Ñuble', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (11, 'Biobío', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (12, 'La Araucanía', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (13, 'Los Ríos', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (14, 'Los Lagos', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (15, 'Aysén', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (16, 'Magallanes', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Region] OFF;


------------------------------------------------------------
-- MUNICIPALITIES (20 REGISTROS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Municipality] ON;

INSERT INTO [dbo].[Municipality] 
            ([MunicipalityId], 
            [RegionId], 
            [Name], 
            [CreatedAt],
            [CreatedBy],
            [LastModified],
            [LastModifiedBy]) 
    VALUES
        (1, 7, 'Santiago', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (2, 7, 'Providencia', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (3, 7, 'Las Condes', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (4, 7, 'La Florida', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (5, 7, 'Puente Alto', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (6, 6, 'Viña del Mar', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (7, 6, 'Valparaíso', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (8, 5, 'La Serena', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (9, 5, 'Coquimbo', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (10, 9, 'Talca', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (11, 11, 'Concepción', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (12, 11, 'Talcahuano', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (13, 12, 'Temuco', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (14, 14, 'Puerto Montt', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (15, 14, 'Osorno', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (16, 1, 'Arica', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (17, 2, 'Iquique', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (18, 3, 'Antofagasta', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (19, 4, 'Copiapó', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (20, 8, 'Rancagua', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Municipality] OFF;


------------------------------------------------------------
-- USERS (30 REGISTROS, IDs FIJOS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[User] ON;

INSERT INTO [dbo].[User] 
                ([UserId],
                [Avatar],
                [FirstName],
                [LastName],
                [Username],
                [Email],
                [PasswordHash],
                [DateOfBirth],
                [PhoneNumber],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            -- 1
            (1, N'user1.png', N'Ricardo', N'Retamal', N'Ricardor', N'ricardo@example.com', N'$2a$10$abc123hash', '1990-05-12', N'+56911111111', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 2
            (2, N'user2.png', N'Carla', N'Tur', N'carlat', N'carla@example.com', N'$2a$10$def456hash', '1992-07-25', N'+56922222222', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 3
            (3, N'user3.png', N'Mateo', N'Retamal', N'mretamal', N'mateo@example.com', N'$2a$10$ghi789hash', '1988-02-14', N'+56933333333', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 4
            (4, N'user4.png', N'Alejandra', N'Guerrero', N'aguerrero', N'alejandra@example.com', N'$2a$10$jkl012hash', '1995-09-10', N'+56944444444', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 5
            (5, N'user5.png', N'Luis', N'Retamal', N'lretamal', N'luis_ricardo@example.com', N'$2a$10$mno345hash', '1985-12-05', N'+56955555555', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 6
            (6, N'user6.png', N'Diego', N'Retamal', N'dretamal', N'diego@example.com', N'$2a$10$pqr678hash', '1993-03-18', N'+56966666666', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 7
            (7, N'user7.png', N'Sebastián', N'Torres', N'storres', N'storres@example.com', N'$2a$10$stu901hash', '1991-11-21', N'+56977777777', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 8
            (8, N'user8.png', N'Camila', N'Díaz', N'cdiaz', N'cdiaz@example.com', N'$2a$10$vwx234hash', '1998-06-30', N'+56988888888', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 9
            (9, N'user9.png', N'Felipe', N'Morales', N'fmorales', N'fmorales@example.com', N'$2a$10$yzA567hash', '1994-08-22', N'+56999999999', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 10
            (10, N'user10.png', N'Isidora', N'Cáceres', N'icaceres', N'icaceres@example.com', N'$2a$10$AbC890hash', '1996-04-15', N'+56910101010', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 11
            (11, N'user11.png', N'Matías', N'Navarro', N'mnavarro', N'mnavarro@example.com', N'$2a$10$DeF123hash', '1987-01-05', N'+56911112222', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 12
            (12, N'user12.png', N'Antonia', N'Fuentes', N'afuentes', N'afuentes@example.com', N'$2a$10$GhI456hash', '1999-12-30', N'+56912223333', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 13
            (13, N'user13.png', N'Benjamín', N'Silva', N'bsilva', N'bsilva@example.com', N'$2a$10$JkL789hash', '1993-10-01', N'+56913334444', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 14
            (14, N'user14.png', N'Catalina', N'Muñoz', N'cmunoz', N'cmunoz@example.com', N'$2a$10$MnO012hash', '1997-02-09', N'+56914445555', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 15
            (15, N'user15.png', N'Tomás', N'Castro', N'tcastro', N'tcastro@example.com', N'$2a$10$PqR345hash', '1990-09-25', N'+56915556666', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 16
            (16, N'user16.png', N'Francisca', N'Leiva', N'fleiva', N'fleiva@example.com', N'$2a$10$StU678hash', '1994-07-17', N'+56916667777', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 17
            (17, N'user17.png', N'Andrés', N'Carrasco', N'acarrasco', N'acarrasco@example.com', N'$2a$10$VwX901hash', '1992-01-22', N'+56917778888', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 18
            (18, N'user18.png', N'Fernanda', N'Contreras', N'fcontreras', N'fcontreras@example.com', N'$2a$10$YzA234hash', '1998-05-08', N'+56918889999', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 19
            (19, N'user19.png', N'Cristóbal', N'Rojas', N'crojas', N'crojas@example.com', N'$2a$10$BcD567hash', '1991-11-03', N'+56919990000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 20
            (20, N'user20.png', N'Paula', N'Campos', N'pcampos', N'pcampos@example.com', N'$2a$10$EfG890hash', '1995-03-12', N'+56920001111', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[User] OFF;


------------------------------------------------------------
-- ADDRESS (30 REGISTROS, IDs FIJOS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Address] ON;

INSERT INTO [dbo].[Address] 
                ([AddressId],
                [CountryId],
                [MunicipalityId],
                [Title],
                [City],
                [Street],
                [Number],
                [Apartament],
                [Reference],
                [PostalCode],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
    VALUES
        (1, 1, 1, 'Casa 1', 'Santiago', 'Calle 1', '10', 'A', 'Frente a plaza', '7550001', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (2, 1, 2, 'Casa 2', 'Providencia', 'Calle 2', '20', NULL, NULL, '7500000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (3, 1, 3, 'Casa 3', 'Las Condes', 'Calle 3', '30', '101', NULL, '7590000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (4, 1, 4, 'Casa 4', 'La Florida', 'Calle 4', '40', NULL, NULL, '8240000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (5, 1, 5, 'Casa 5', 'Puente Alto', 'Calle 5', '50', NULL, NULL, '8150000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (6, 1, 6, 'Depto 6', 'Viña del Mar', 'Calle 6', '60', '10B', NULL, '2520000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (7, 1, 7, 'Depto 7', 'Valparaíso', 'Calle 7', '70', NULL, NULL, '2340000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (8, 1, 8, 'Depto 8', 'La Serena', 'Calle 8', '80', NULL, NULL, '1700000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (9, 1, 9, 'Depto 9', 'Coquimbo', 'Calle 9', '90', '2C', NULL, '1780000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (10, 1, 10, 'Casa 10', 'Talca', 'Calle 10', '100', NULL, NULL, '3460000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (11, 1, 11, 'Casa 11', 'Concepción', 'Calle 11', '110', NULL, NULL, '4030000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (12, 1, 12, 'Casa 12', 'Talcahuano', 'Calle 12', '120', '1A', NULL, '4260000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (13, 1, 13, 'Casa 13', 'Temuco', 'Calle 13', '130', NULL, NULL, '4780000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (14, 1, 14, 'Casa 14', 'Puerto Montt', 'Calle 14', '140', NULL, NULL, '5480000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (15, 1, 15, 'Casa 15', 'Osorno', 'Calle 15', '150', NULL, NULL, '5290000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (16, 1, 16, 'Casa 16', 'Arica', 'Calle 16', '160', NULL, NULL, '1000000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (17, 1, 17, 'Casa 17', 'Iquique', 'Calle 17', '170', NULL, NULL, '1100000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (18, 1, 18, 'Casa 18', 'Antofagasta', 'Calle 18', '180', NULL, NULL, '1240000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (19, 1, 19, 'Casa 19', 'Copiapó', 'Calle 19', '190', NULL, NULL, '1530000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (20, 1, 20, 'Casa 20', 'Rancagua', 'Calle 20', '200', NULL, NULL, '2820000', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Address] OFF;


------------------------------------------------------------
-- ADDRESSUSER (30 RELACIONES, IDs FIJOS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[AddressUser] ON;

INSERT INTO [dbo].[AddressUser] 
                  ([AddressId], 
                  [UserId], 
                  [IsDefault])
        VALUES
            (1, 1, true),
            (2, 2, true),
            (3, 3, true),
            (4, 4, true),
            (5, 5, true),
            (6, 6, true),
            (7, 7, true),
            (8, 8, true),
            (9, 9, true),
            (10, 10, true),
            (11, 11, true),
            (12, 12, true),
            (13, 13, true),
            (14, 14, true),
            (15, 15, true),
            (16, 16, true),
            (17, 17, true),
            (18, 18, true),
            (19, 19, true),
            (20, 20, true);

SET IDENTITY_INSERT [dbo].[AddressUser] OFF;
