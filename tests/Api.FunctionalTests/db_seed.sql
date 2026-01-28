USE master;

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;

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
            (1, 'Chile', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (2, 'Argentina', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

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
        (20, 8, 'Rancagua', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
        (21, 8, 'Villarrica', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Municipality] OFF;


------------------------------------------------------------
-- USERS (30 REGISTROS, IDs FIJOS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[AspNetUsers] ON;

INSERT INTO [dbo].[AspNetUsers] 
                ([Id],
                [Avatar],
                [FirstName],
                [LastName],
                [DateOfBirth],
                [Username],
                [NormalizedUserName],
                [Email],
                [NormalizedEmail],
                [EmailConfirmed],
                [PasswordHash],
                [SecurityStamp],
                [ConcurrencyStamp],
                [PhoneNumber],
                [PhoneNumberConfirmed],
                [TwoFactorEnabled],
                [LockoutEnd],
                [LockoutEnabled],
                [AccessFailedCount])
        VALUES
            -- 1
            (1, N'user1.png', N'Juan', N'Pérez', '1990-01-15', N'juan.perez', N'JUAN.PEREZ', N'juan.perez@mail.com', N'JUAN.PEREZ@MAIL.COM', 1, N'$2a$10$hash001', NEWID(), NEWID(), N'+56911111111', 1, 0, NULL, 1, 0),
            -- 2
            (2, N'user2.png', N'María', N'González', '1992-03-22', N'maria.gonzalez', N'MARIA.GONZALEZ', N'maria.gonzalez@mail.com', N'MARIA.GONZALEZ@MAIL.COM', 1, N'$2a$10$hash002', NEWID(), NEWID(), N'+56922222222', 1, 0, NULL, 1, 0),
            -- 3
            (3, N'user3.png', N'Carlos', N'Ramírez', '1988-07-10', N'carlos.ramirez', N'CARLOS.RAMIREZ', N'carlos.ramirez@mail.com', N'CARLOS.RAMIREZ@MAIL.COM', 1, N'$2a$10$hash003', NEWID(), NEWID(), N'+56933333333', 1, 0, NULL, 1, 0),
            -- 4
            (4, N'user4.png', N'Ana', N'López', '1995-11-05', N'ana.lopez', N'ANA.LOPEZ', N'ana.lopez@mail.com', N'ANA.LOPEZ@MAIL.COM', 1, N'$2a$10$hash004', NEWID(), NEWID(), N'+56944444444', 1, 0, NULL, 1, 0),
            -- 5
            (5, N'user5.png', N'Pedro', N'Soto', '1987-02-18', N'pedro.soto', N'PEDRO.SOTO', N'pedro.soto@mail.com', N'PEDRO.SOTO@MAIL.COM', 1, N'$2a$10$hash005', NEWID(), NEWID(), N'+56955555555', 1, 0, NULL, 1, 0),
            -- 6
            (6, N'user6.png', N'Lucía', N'Martínez', '1993-08-09', N'lucia.martinez', N'LUCIA.MARTINEZ', N'lucia.martinez@mail.com', N'LUCIA.MARTINEZ@MAIL.COM', 1, N'$2a$10$hash006', NEWID(), NEWID(), N'+56966666666', 1, 0, NULL, 1, 0),
            -- 7
            (7, N'user7.png', N'Diego', N'Contreras', '1991-04-27', N'diego.contreras', N'DIEGO.CONTRERAS', N'diego.contreras@mail.com', N'DIEGO.CONTRERAS@MAIL.COM', 1, N'$2a$10$hash007', NEWID(), NEWID(), N'+56977777777', 1, 0, NULL, 1, 0),
            -- 8
            (8, N'user8.png', N'Camila', N'Rojas', '1996-12-01', N'camila.rojas', N'CAMILA.ROJAS', N'camila.rojas@mail.com', N'CAMILA.ROJAS@MAIL.COM', 1, N'$2a$10$hash008', NEWID(), NEWID(), N'+56988888888', 1, 0, NULL, 1, 0),
            -- 9
            (9, N'user9.png', N'Matías', N'Herrera', '1989-06-30', N'matias.herrera', N'MATIAS.HERRERA', N'matias.herrera@mail.com', N'MATIAS.HERRERA@MAIL.COM', 1, N'$2a$10$hash009', NEWID(), NEWID(), N'+56999999999', 1, 0, NULL, 1, 0),
            -- 10
            (10, N'user10.png', N'Sofía', N'Muñoz', '1994-09-14', N'sofia.munoz', N'SOFIA.MUNOZ', N'sofia.munoz@mail.com', N'SOFIA.MUNOZ@MAIL.COM', 1, N'$2a$10$hash010', NEWID(), NEWID(), N'+56910101010', 1, 0, NULL, 1, 0),
            -- 11
            (11, N'user11.png', N'Andrés', N'Castro', '1986-05-20', N'andres.castro', N'ANDRES.CASTRO', N'andres.castro@mail.com', N'ANDRES.CASTRO@MAIL.COM', 1, N'$2a$10$hash011', NEWID(), NEWID(), N'+56911111112', 1, 0, NULL, 1, 0),
            -- 12
            (12, N'user12.png', N'Valentina', N'Silva', '1997-01-08', N'valentina.silva', N'VALENTINA.SILVA', N'valentina.silva@mail.com', N'VALENTINA.SILVA@MAIL.COM', 1, N'$2a$10$hash012', NEWID(), NEWID(), N'+56912121212', 1, 0, NULL, 1, 0),
            -- 13
            (13, N'user13.png', N'Tomás', N'Vargas', '1990-10-03', N'tomas.vargas', N'TOMAS.VARGAS', N'tomas.vargas@mail.com', N'TOMAS.VARGAS@MAIL.COM', 1, N'$2a$10$hash013', NEWID(), NEWID(), N'+56913131313', 1, 0, NULL, 1, 0),
            -- 14
            (14, N'user14.png', N'Daniela', N'Navarro', '1992-07-19', N'daniela.navarro', N'DANIELA.NAVARRO', N'daniela.navarro@mail.com', N'DANIELA.NAVARRO@MAIL.COM', 1, N'$2a$10$hash014', NEWID(), NEWID(), N'+56914141414', 1, 0, NULL, 1, 0),
            -- 15
            (15, N'user15.png', N'Felipe', N'Araya', '1985-03-11', N'felipe.araya', N'FELIPE.ARAYA', N'felipe.araya@mail.com', N'FELIPE.ARAYA@MAIL.COM', 1, N'$2a$10$hash015', NEWID(), NEWID(), N'+56915151515', 1, 0, NULL, 1, 0),
            -- 16
            (16, N'user16.png', N'Paula', N'Figueroa', '1993-11-25', N'paula.figueroa', N'PAULA.FIGUEROA', N'paula.figueroa@mail.com', N'PAULA.FIGUEROA@MAIL.COM', 1, N'$2a$10$hash016', NEWID(), NEWID(), N'+56916161616', 1, 0, NULL, 1, 0),
            -- 17
            (17, N'user17.png', N'Javier', N'Morales', '1988-02-06', N'javier.morales', N'JAVIER.MORALES', N'javier.morales@mail.com', N'JAVIER.MORALES@MAIL.COM', 1, N'$2a$10$hash017', NEWID(), NEWID(), N'+56917171717', 1, 0, NULL, 1, 0),
            -- 18
            (18, N'user18.png', N'Francisca', N'Peña', '1996-06-17', N'francisca.pena', N'FRANCISCA.PENA', N'francisca.pena@mail.com', N'FRANCISCA.PENA@MAIL.COM', 1, N'$2a$10$hash018', NEWID(), NEWID(), N'+56918181818', 1, 0, NULL, 1, 0),
            -- 19
            (19, N'user19.png', N'Rodrigo', N'Campos', '1991-12-29', N'rodrigo.campos', N'RODRIGO.CAMPOS', N'rodrigo.campos@mail.com', N'RODRIGO.CAMPOS@MAIL.COM', 1, N'$2a$10$hash019', NEWID(), NEWID(), N'+56919191919', 1, 0, NULL, 1, 0),
            -- 20
            (20, N'user20.png', N'Natalia', N'Reyes', '1995-04-02', N'natalia.reyes', N'NATALIA.REYES', N'natalia.reyes@mail.com', N'NATALIA.REYES@MAIL.COM', 1, N'$2a$10$hash020', NEWID(), NEWID(), N'+56920202020', 1, 0, NULL, 1, 0);

SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF;


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

INSERT INTO [dbo].[AddressUser] 
                  ([AddressId], 
                  [ApplicationUserId], 
                  [IsDefault],
                  [CreatedAt],
                  [CreatedBy],
                  [LastModified],
                  [LastModifiedBy])
        VALUES
            (1, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (2, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (3, 3, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (4, 4, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (5, 5, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (6, 6, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (7, 7, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (8, 8, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (9, 9, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (10, 10, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (11, 11, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (12, 12, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (13, 13, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (14, 14, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (15, 15, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (16, 16, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (17, 17, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (18, 18, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (19, 19, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (20, 20, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');


------------------------------------------------------------
-- REFRESHTOKEN
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[RefreshTokens] ON;

INSERT INTO [dbo].[RefreshTokens] 
                ([Id],
                [Token],
                [UserId],
                [ExpiresOnUtc])
        VALUES 
            (1, 'FANyke9UesiUO/RTqXiv5fgvYi7AdeKKRHUi0EevUcE=', 1, '2026-01-08 14:40:57.2357932'),
            (2, 'aAjYRb6zJmg5Kj8abtgRArWHQUP7T8yZmImDgVfBPVs=', 1, '2026-01-09 14:40:57.2357932');

SET IDENTITY_INSERT [dbo].[RefreshTokens] OFF;


------------------------------------------------------------
-- BRAND (20 REGISTROS, IDs FIJOS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Brand] ON;

INSERT INTO [dbo].[Brand] 
                ([Id],
                [Name],
                [Slug],
                [Description],
                [LogoUrl],
                [BannerUrl],
                [WebsiteUrl],
                [IsActive],
                [IsFeatured],
                [DisplayOrder],
                [MetaTitle],
                [MetaDescription],
                [MetaKeywords],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            -- 1
            (1, N'Nike', N'nike', N'Just Do It - Global leader in athletic footwear, apparel, and equipment', N'https://example.com/logos/nike.png', N'https://example.com/banners/nike-banner.jpg', N'https://www.nike.com', 1, 1, 1, N'Nike - Athletic Shoes & Sportswear', N'Shop the latest Nike shoes, clothing and accessories for men, women and kids', N'nike, shoes, athletic, sportswear, sneakers, running', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 2
            (2, N'Adidas', N'adidas', N'Impossible is Nothing - Leading brand in sports apparel and footwear', N'https://example.com/logos/adidas.png', N'https://example.com/banners/adidas-banner.jpg', N'https://www.adidas.com', 1, 1, 2, N'Adidas - Sports Shoes & Clothing', N'Discover the latest Adidas collection of sports shoes, clothing and accessories', N'adidas, sports, shoes, clothing, athletic wear', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 3
            (3, N'Puma', N'puma', N'Forever Faster - International sportswear brand designing athletic and casual footwear', N'https://example.com/logos/puma.png', N'https://example.com/banners/puma-banner.jpg', N'https://www.puma.com', 1, 1, 3, N'Puma - Sports & Lifestyle Brand', N'Shop Puma shoes, clothing and accessories for sports and lifestyle', N'puma, sportswear, shoes, lifestyle, athletics', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 4
            (4, N'Reebok', N'reebok', N'Be More Human - Fitness and lifestyle brand inspired by sport heritage', N'https://example.com/logos/reebok.png', N'https://example.com/banners/reebok-banner.jpg', N'https://www.reebok.com', 1, 0, 4, N'Reebok - Fitness & Training Gear', N'Explore Reebok fitness shoes, training apparel and accessories', N'reebok, fitness, training, crossfit, shoes', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 5
            (5, N'New Balance', N'new-balance', N'Fearlessly Independent Since 1906 - Premium athletic footwear manufacturer', N'https://example.com/logos/new-balance.png', N'https://example.com/banners/new-balance-banner.jpg', N'https://www.newbalance.com', 1, 1, 5, N'New Balance - Athletic Footwear & Apparel', N'Discover New Balance shoes, clothing and accessories for running and lifestyle', N'new balance, running shoes, athletic footwear, sneakers', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 6
            (6, N'Under Armour', N'under-armour', N'I Will - Performance apparel, footwear and accessories brand', N'https://example.com/logos/under-armour.png', N'https://example.com/banners/under-armour-banner.jpg', N'https://www.underarmour.com', 1, 1, 6, N'Under Armour - Performance Sports Gear', N'Shop Under Armour performance shoes, clothing and gear for athletes', N'under armour, performance, sports gear, training, athletic', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 7
            (7, N'Converse', N'converse', N'Made By You - Iconic sneaker brand known for Chuck Taylor All Stars', N'https://example.com/logos/converse.png', N'https://example.com/banners/converse-banner.jpg', N'https://www.converse.com', 1, 0, 7, N'Converse - Classic Sneakers & Apparel', N'Shop iconic Converse Chuck Taylor sneakers and lifestyle apparel', N'converse, chuck taylor, sneakers, casual shoes, lifestyle', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 8
            (8, N'Vans', N'vans', N'Off The Wall - Original action sports footwear and apparel brand', N'https://example.com/logos/vans.png', N'https://example.com/banners/vans-banner.jpg', N'https://www.vans.com', 1, 0, 8, N'Vans - Skateboarding Shoes & Streetwear', N'Discover Vans skateboarding shoes, streetwear and accessories', N'vans, skateboarding, streetwear, sneakers, action sports', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 9
            (9, N'Asics', N'asics', N'Sound Mind, Sound Body - Japanese sports equipment and athletic shoe company', N'https://example.com/logos/asics.png', N'https://example.com/banners/asics-banner.jpg', N'https://www.asics.com', 1, 1, 9, N'Asics - Running Shoes & Sports Gear', N'Shop Asics running shoes, sports apparel and training gear', N'asics, running shoes, sports equipment, athletic gear, marathon', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 10
            (10, N'Fila', N'fila', N'Fortezza, Innovazione, Leggerezza, Avanguardia - Italian sportswear brand', N'https://example.com/logos/fila.png', N'https://example.com/banners/fila-banner.jpg', N'https://www.fila.com', 0, 0, 10, N'Fila - Sportswear & Lifestyle Brand', N'Explore Fila sportswear, sneakers and lifestyle apparel collections', N'fila, sportswear, sneakers, lifestyle, italian brand', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 11
            (11, N'Champion', N'champion', N'Authentic Athletic Apparel Since 1919 - American sportswear brand', N'https://example.com/logos/champion.png', N'https://example.com/banners/champion-banner.jpg', N'https://www.champion.com', 1, 0, 11, N'Champion - Athletic Apparel & Activewear', N'Shop Champion hoodies, sweatshirts and athletic apparel', N'champion, athletic apparel, hoodies, sweatshirts, sportswear', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 12
            (12, N'Skechers', N'skechers', N'The Comfort Technology Company - Lifestyle and performance footwear brand', N'https://example.com/logos/skechers.png', N'https://example.com/banners/skechers-banner.jpg', N'https://www.skechers.com', 1, 0, 12, N'Skechers - Comfort Footwear & Apparel', N'Discover Skechers comfort shoes, sneakers and athletic footwear', N'skechers, comfort shoes, sneakers, walking shoes, lifestyle', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 13
            (13, N'The North Face', N'the-north-face', N'Never Stop Exploring - Outdoor recreation products and apparel company', N'https://example.com/logos/north-face.png', N'https://example.com/banners/north-face-banner.jpg', N'https://www.thenorthface.com', 1, 1, 13, N'The North Face - Outdoor Gear & Apparel', N'Shop The North Face jackets, hiking gear and outdoor apparel', N'north face, outdoor gear, hiking, jackets, outdoor apparel', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 14
            (14, N'Columbia', N'columbia', N'Tested Tough - Outdoor apparel and footwear company', N'https://example.com/logos/columbia.png', N'https://example.com/banners/columbia-banner.jpg', N'https://www.columbia.com', 1, 0, 14, N'Columbia - Outdoor Clothing & Footwear', N'Explore Columbia outdoor clothing, jackets and hiking footwear', N'columbia, outdoor clothing, hiking, jackets, outdoor footwear', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 15
            (15, N'Patagonia', N'patagonia', N'Build the Best Product - Outdoor clothing and gear for the silent sports', N'https://example.com/logos/patagonia.png', N'https://example.com/banners/patagonia-banner.jpg', N'https://www.patagonia.com', 0, 0, 15, N'Patagonia - Outdoor & Adventure Gear', N'Shop Patagonia sustainable outdoor clothing and adventure gear', N'patagonia, outdoor clothing, sustainable, adventure gear, jackets', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 16
            (16, N'Lululemon', N'lululemon', N'This Is Yoga - Athletic apparel retailer specializing in yoga and running gear', N'https://example.com/logos/lululemon.png', N'https://example.com/banners/lululemon-banner.jpg', N'https://www.lululemon.com', 1, 1, 16, N'Lululemon - Yoga & Athletic Apparel', N'Discover Lululemon yoga pants, athletic wear and workout clothing', N'lululemon, yoga, athletic wear, leggings, workout clothing', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 17
            (17, N'Jordan', N'jordan', N'Engineered to the Exact Specifications for Championship Athletes', N'https://example.com/logos/jordan.png', N'https://example.com/banners/jordan-banner.jpg', N'https://www.jordan.com', 1, 1, 17, N'Jordan - Basketball Shoes & Apparel', N'Shop Air Jordan sneakers, basketball shoes and athletic apparel', N'jordan, air jordan, basketball shoes, sneakers, athletic', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 18
            (18, N'Brooks', N'brooks', N'Run Happy - Running shoes and apparel company focused on performance', N'https://example.com/logos/brooks.png', N'https://example.com/banners/brooks-banner.jpg', N'https://www.brooksrunning.com', 1, 0, 18, N'Brooks - Running Shoes & Gear', N'Explore Brooks running shoes, apparel and gear for runners', N'brooks, running shoes, performance, marathon, running gear', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 19
            (19, N'Salomon', N'salomon', N'Time to Play - Outdoor sports gear company specializing in trail running and skiing', N'https://example.com/logos/salomon.png', N'https://example.com/banners/salomon-banner.jpg', N'https://www.salomon.com', 0, 0, 19, N'Salomon - Trail Running & Outdoor Gear', N'Shop Salomon trail running shoes, hiking boots and outdoor equipment', N'salomon, trail running, hiking, outdoor gear, skiing', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 20
            (20, N'Hoka One One', N'hoka-one-one', N'Fly Human Fly - Performance running shoes with maximum cushioning', N'https://example.com/logos/hoka.png', N'https://example.com/banners/hoka-banner.jpg', N'https://www.hoka.com', 1, 1, 20, N'Hoka One One - Cushioned Running Shoes', N'Discover Hoka One One maximally cushioned running shoes and footwear', N'hoka, running shoes, cushioned, performance, ultra marathon', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Brand] OFF;


------------------------------------------------------------
-- CATEGORY (20 REGISTROS CON JERARQUÍA DE 3 NIVELES, IDs FIJOS)
------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Category] ON;

INSERT INTO [dbo].[Category] 
                ([Id],
                [ParentId],
                [Name],
                [Slug],
                [IsActive],
                [IsVisibleInMenu],
                [DisplayOrder],
                [ImageUrl],
                [Icon],
                [Description],
                [MetaTitle],
                [MetaDescription],
                [MetaKeywords],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            -- NIVEL 1: CATEGORÍAS RAÍZ (ParentId = NULL)
            -- 1
            (1, NULL, N'Electronics', N'electronics', 1, 1, 1, N'https://example.com/images/electronics.png', N'fa-bolt', N'Electronic devices and accessories', N'Electronics - Devices & Gadgets', N'Browse our wide selection of electronic devices, gadgets and accessories', N'electronics, devices, gadgets, technology', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 2
            (2, NULL, N'Computers', N'computers', 1, 1, 2, N'https://example.com/images/computers.png', N'fa-laptop', N'Laptops, desktops and computer accessories', N'Computers - Laptops & Desktops', N'Shop for laptops, desktop computers and computer accessories', N'computers, laptops, desktops, PC, hardware', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 3
            (3, NULL, N'Smartphones', N'smartphones', 1, 1, 3, N'https://example.com/images/smartphones.png', N'fa-mobile-alt', N'Mobile phones and accessories', N'Smartphones - Mobile Phones', N'Discover the latest smartphones and mobile accessories', N'smartphones, mobile phones, cell phones, iPhone, Android', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 4
            (4, NULL, N'Home Appliances', N'home-appliances', 1, 1, 4, N'https://example.com/images/appliances.png', N'fa-home', N'Kitchen and home appliances', N'Home Appliances - Kitchen & Home', N'Browse our collection of home and kitchen appliances', N'appliances, kitchen, home, refrigerators, ovens', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 5
            (5, NULL, N'Sports & Outdoors', N'sports-outdoors', 1, 1, 5, N'https://example.com/images/sports.png', N'fa-running', N'Sports equipment and outdoor gear', N'Sports & Outdoors - Equipment & Gear', N'Shop for sports equipment, fitness gear and outdoor accessories', N'sports, outdoors, fitness, exercise, camping', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 6
            (6, NULL, N'Fashion', N'fashion', 0, 1, 6, N'https://example.com/images/fashion.png', N'fa-tshirt', N'Clothing, shoes and accessories', N'Fashion - Clothing & Accessories', N'Explore the latest fashion trends in clothing, shoes and accessories', N'fashion, clothing, shoes, apparel, style', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            
            -- NIVEL 2: SUBCATEGORÍAS DE ELECTRONICS (ParentId = 1)
            -- 7
            (7, 1, N'Televisions', N'televisions', 1, 1, 1, N'https://example.com/images/tvs.png', N'fa-tv', N'Smart TVs and television accessories', N'Televisions - Smart TVs', N'Shop for smart TVs, 4K TVs and television accessories', N'televisions, TVs, smart TV, 4K, LED', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 8
            (8, 1, N'Audio Equipment', N'audio-equipment', 1, 1, 2, N'https://example.com/images/audio.png', N'fa-headphones', N'Headphones, speakers and audio systems', N'Audio Equipment - Headphones & Speakers', N'Browse headphones, speakers, soundbars and audio systems', N'audio, headphones, speakers, sound, music', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 9
            (9, 1, N'Cameras', N'cameras', 1, 1, 3, N'https://example.com/images/cameras.png', N'fa-camera', N'Digital cameras and photography equipment', N'Cameras - Photography Equipment', N'Shop for digital cameras, lenses and photography accessories', N'cameras, photography, DSLR, lenses, digital', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            
            -- NIVEL 3: SUB-SUBCATEGORÍAS DE CAMERAS (ParentId = 9)
            -- 10
            (10, 9, N'DSLR Cameras', N'dslr-cameras', 1, 1, 1, N'https://example.com/images/dslr.png', N'fa-camera-retro', N'Professional DSLR cameras and equipment', N'DSLR Cameras - Professional Photography', N'Shop for professional DSLR cameras and accessories', N'DSLR, cameras, professional, photography, Canon, Nikon', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 11
            (11, 9, N'Mirrorless Cameras', N'mirrorless-cameras', 1, 1, 2, N'https://example.com/images/mirrorless.png', N'fa-camera', N'Compact mirrorless camera systems', N'Mirrorless Cameras - Compact Systems', N'Browse mirrorless cameras and compact camera systems', N'mirrorless, cameras, compact, Sony, Fujifilm', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            
            -- NIVEL 2: SUBCATEGORÍAS DE COMPUTERS (ParentId = 2)
            -- 12
            (12, 2, N'Laptops', N'laptops', 1, 1, 1, N'https://example.com/images/laptops.png', N'fa-laptop-code', N'Portable computers and notebooks', N'Laptops - Portable Computers', N'Browse our selection of laptops and portable computers', N'laptops, notebooks, portable computers, ultrabooks', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 13
            (13, 2, N'Desktop Computers', N'desktop-computers', 1, 1, 2, N'https://example.com/images/desktops.png', N'fa-desktop', N'Desktop PCs and workstations', N'Desktop Computers - PCs & Workstations', N'Shop for desktop computers, all-in-ones and workstations', N'desktop, PC, computers, workstations, towers', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 14
            (14, 2, N'Computer Accessories', N'computer-accessories', 1, 1, 3, N'https://example.com/images/accessories.png', N'fa-keyboard', N'Keyboards, mice and computer peripherals', N'Computer Accessories - Peripherals', N'Explore keyboards, mice, monitors and other computer accessories', N'accessories, keyboards, mice, monitors, peripherals', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            
            -- NIVEL 2: SUBCATEGORÍAS DE SMARTPHONES (ParentId = 3)
            -- 15
            (15, 3, N'iPhone', N'iphone', 1, 1, 1, N'https://example.com/images/iphone.png', N'fa-apple', N'Apple iPhone smartphones', N'iPhone - Apple Smartphones', N'Shop the latest iPhone models and accessories', N'iPhone, Apple, iOS, smartphone', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 16
            (16, 3, N'Android Phones', N'android-phones', 1, 1, 2, N'https://example.com/images/android.png', N'fa-android', N'Android smartphones and devices', N'Android Phones - Smartphones', N'Browse Android smartphones from top brands', N'android, smartphones, Samsung, Google, Pixel', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 17
            (17, 3, N'Phone Accessories', N'phone-accessories', 0, 0, 3, N'https://example.com/images/phone-accessories.png', N'fa-mobile', N'Cases, chargers and phone accessories', N'Phone Accessories - Cases & Chargers', N'Shop for phone cases, screen protectors and chargers', N'phone accessories, cases, chargers, screen protectors', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            
            -- NIVEL 2: SUBCATEGORÍAS DE HOME APPLIANCES (ParentId = 4)
            -- 18
            (18, 4, N'Kitchen Appliances', N'kitchen-appliances', 1, 1, 1, N'https://example.com/images/kitchen.png', N'fa-utensils', N'Refrigerators, ovens and kitchen appliances', N'Kitchen Appliances - Refrigerators & Ovens', N'Shop for kitchen appliances including refrigerators and ovens', N'kitchen, appliances, refrigerators, ovens, microwaves', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            -- 19
            (19, 4, N'Vacuum Cleaners', N'vacuum-cleaners', 1, 1, 2, N'https://example.com/images/vacuum.png', N'fa-broom', N'Vacuum cleaners and floor care', N'Vacuum Cleaners - Floor Care', N'Browse vacuum cleaners and floor care appliances', N'vacuum, cleaners, floor care, cleaning', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            
            -- NIVEL 2: SUBCATEGORÍAS DE SPORTS & OUTDOORS (ParentId = 5)
            -- 20
            (20, 5, N'Fitness Equipment', N'fitness-equipment', 1, 1, 1, N'https://example.com/images/fitness.png', N'fa-dumbbell', N'Home fitness equipment and gear', N'Fitness Equipment - Home Gym', N'Shop for fitness equipment, weights and exercise gear', N'fitness, equipment, gym, weights, exercise', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Category] OFF;


------------------------------------------------------------
-- ProductTaxCategory (6 REGISTROS, IDs FIJOS)
------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[ProductTaxCategory] ON;

INSERT INTO [dbo].[ProductTaxCategory] 
                ([Id],
                [Name],
                [Code],
                [Description],
                [IsActive],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            (1, N'Afecto a IVA', N'IVA_STD', N'Productos y servicios afectos a IVA general en Chile.', 1,
             SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (2, N'Exento de IVA', N'IVA_EXEMPT', N'Productos o servicios exentos de IVA seg�n normativa chilena.', 1,
             SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (3, N'No Afecto', N'NO_TAX', N'Operaciones no afectas a impuestos.', 1,
             SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (4, N'Servicios', N'SERVICES', N'Servicios gravados con IVA en Chile.', 1,
             SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (5, N'Productos Digitales', N'DIGITAL', N'Software y servicios digitales afectos a IVA.', 1,
             SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (6, N'Exportaciones', N'EXPORT', N'Exportaciones con tasa 0% seg�n ley chilena.', 1,
             SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[ProductTaxCategory] OFF;


------------------------------------------------------------
-- TaxRate (4 REGISTROS, IDs FIJOS)
------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[TaxRate] ON;

INSERT INTO [dbo].[TaxRate] 
                ([Id],
                [Name],
                [Code],
                [CountryCode],
                [TaxType],
                [Rate],
                [IsCompound],
                [IsActive],
                [ValidFrom],
                [ValidTo],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            (1, N'IVA Chile 19%', N'IVA_19', N'CL', N'VAT', 0.1900, 0, 1,
                '2003-10-01', NULL,
                SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (2, N'IVA Exportaci�n 0%', N'IVA_0_EXPORT', N'CL', N'VAT', 0.0000, 0, 1,
               '2003-10-01', NULL,
                SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (3, N'Exento de IVA', N'IVA_EXEMPT', N'CL', N'VAT', 0.0000, 0, 1,
                '2003-10-01', NULL,
                SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (4, N'IVA Servicios Digitales', N'IVA_DIGITAL', N'CL', N'VAT', 0.1900, 0, 1,
                '2020-06-01', NULL,
                SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[TaxRate] OFF;


------------------------------------------------------------
-- ProductTaxCategoryRate (6 REGISTROS
------------------------------------------------------------

INSERT INTO [dbo].[ProductTaxCategoryRate] 
                ([ProductTaxCategoryId],
                [TaxRateId])
        VALUES
            -- Afecto a IVA
            (1, 1),

            -- Exento
            (2, 3),

            -- No Afecto
            (3, 3),

            -- Servicios
            (4, 1),

            -- Productos Digitales
            (5, 4),

            -- Exportaciones
            (6, 2);


------------------------------------------------------------
-- Attribute (5 REGISTROS, IDs FIJOS)
------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[Attribute] ON;

INSERT INTO [dbo].[Attribute]
                ([Id],
                [Code],
                [Name],
                [Description],
                [DataType],
                [IsVariant],
                [IsFilterable],
                [IsRequired],
                [DisplayOrder],
                [IsActive],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            (1, N'color', N'Color', N'Color del producto', N'String', 1, 1, 1, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (2, N'storage', N'Almacenamiento', N'Capacidad de almacenamiento', N'Number', 1, 1, 1, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (3, N'ram', N'Memoria RAM', N'Cantidad de memoria RAM', N'Number', 1, 1, 1, 3, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (4, N'size', N'Tamaño', N'Tama�o o talla del producto', N'String', 1, 1, 1, 4, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (5, N'material', N'Material', N'Material principal del producto', N'String', 0, 1, 0, 5, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Attribute] OFF;



------------------------------------------------------------
-- AttributeValue (14 REGISTROS, IDs FIJOS)
------------------------------------------------------------

DECLARE @ColorId BIGINT = (SELECT Id FROM [dbo].[Attribute] WHERE Code = N'color');
    DECLARE @StorageId BIGINT = (SELECT Id FROM [dbo].[Attribute] WHERE Code = N'storage');
    DECLARE @RamId BIGINT = (SELECT Id FROM [dbo].[Attribute] WHERE Code = N'ram');
    DECLARE @SizeId BIGINT = (SELECT Id FROM [dbo].[Attribute] WHERE Code = N'size');
    DECLARE @MaterialId BIGINT = (SELECT Id FROM [dbo].[Attribute] WHERE Code = N'material');

SET IDENTITY_INSERT [dbo].[AttributeValue] ON;

    INSERT INTO [dbo].[AttributeValue]
                    ([Id],
                    [AttributeId],
                    [Value],
                    [NormalizedValue],
                    [NumericValue],
                    [BooleanValue],
                    [DisplayOrder],
                    [IsActive],
                    [CreatedAt],
                    [CreatedBy],
                    [LastModified],
                    [LastModifiedBy])
        VALUES
            -- Colores
            (1, @ColorId, N'Negro', N'negro', NULL, NULL, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (2, @ColorId, N'Blanco', N'blanco', NULL, NULL, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (3, @ColorId, N'Azul', N'azul', NULL, NULL, 3, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Almacenamiento (GB)
            (4, @StorageId, N'128 GB', N'128', 128, NULL, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (5, @StorageId, N'256 GB', N'256', 256, NULL, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (6, @StorageId, N'512 GB', N'512', 512, NULL, 3, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- RAM (GB)
            (7, @RamId, N'8 GB', N'8', 8, NULL, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (8, @RamId, N'12 GB', N'12', 12, NULL, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (9, @RamId, N'16 GB', N'16', 16, NULL, 3, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Tama�o / Talla
            (10, @SizeId, N'S', N's', NULL, NULL, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (11, @SizeId, N'M', N'm', NULL, NULL, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (12, @SizeId, N'L', N'l', NULL, NULL, 3, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Material
            (13, @MaterialId, N'Aluminio', N'aluminio', NULL, NULL, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (14, @MaterialId, N'Pl�stico', N'plastico', NULL, NULL, 2, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[AttributeValue] OFF;


------------------------------------------------------------
-- Product (20 REGISTROS, IDs FIJOS)
------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[Product] ON;

INSERT INTO [dbo].[Product] 
                ([Id],
                [Name],
                [Slug],
                [Description],
                [ShortDescription],
                [BrandId],
                [CategoryId],
                [ProductTaxCategoryId],
                [IsActive],
                [IsFeatured],
                [IsDigital],
                [IsPublished],
                [MetaTitle],
                [MetaDescription],
                [MetaKeywords],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            (1, N'Smartphone Galaxy X', N'smartphone-galaxy-x', N'Smartphone de alto rendimiento con pantalla AMOLED.', N'Smartphone premium AMOLED', 1, 1, 1, 1, 1, 0, 1, N'Smartphone Galaxy X', N'Smartphone de última generación con pantalla AMOLED.', N'smartphone, amoled, android', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (2, N'iPhone Pro Max', N'iphone-pro-max', N'Smartphone premium con ecosistema Apple.', N'iPhone de gama alta', 2, 1, 1, 1, 1, 0, 1, N'iPhone Pro Max', N'iPhone con máximo rendimiento y diseño.', N'iphone, apple, smartphone', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (3, N'Laptop Ultrabook Pro', N'laptop-ultrabook-pro', N'Laptop liviana y potente para profesionales.', N'Ultrabook profesional', 2, 2, 1, 1, 1, 0, 1, N'Ultrabook Pro', N'Laptop profesional de alto rendimiento.', N'laptop, ultrabook, trabajo', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (4, N'Auriculares Wireless ANC', N'auriculares-wireless-anc', N'Auriculares inalámbricos con cancelación de ruido.', N'Auriculares con ANC', 3, 3, 1, 1, 0, 0, 1, N'Auriculares ANC', N'Auriculares con cancelación activa de ruido.', N'auriculares, anc, bluetooth', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (5, N'Smart TV 65 4K', N'smart-tv-65-4k', N'Televisor inteligente 4K UHD de 65 pulgadas.', N'Smart TV 65 pulgadas', 4, 4, 1, 1, 1, 0, 1, N'Smart TV 65 4K', N'Televisor 4K con aplicaciones inteligentes.', N'smart tv, 4k, television', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (6, N'Tablet Android Plus', N'tablet-android-plus', N'Tablet Android para productividad y entretenimiento.', N'Tablet Android', 1, 1, 1, 1, 0, 0, 1, N'Tablet Android Plus', N'Tablet vers�til para trabajo y ocio.', N'tablet, android, multimedia', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (7, N'Mouse Inal�mbrico Ergo', N'mouse-inalambrico-ergo', N'Mouse ergon�mico inal�mbrico de precisi�n.', N'Mouse ergon�mico', 5, 5, 1, 1, 0, 0, 1, N'Mouse Ergon�mico', N'Mouse inal�mbrico c�modo y preciso.', N'mouse, ergonomico, inalambrico', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (8, N'Teclado Mec�nico Pro', N'teclado-mecanico-pro', N'Teclado mec�nico con retroiluminaci�n RGB.', N'Teclado mec�nico RGB', 5, 5, 1, 1, 1, 0, 1, N'Teclado Mec�nico', N'Teclado mec�nico para gamers y desarrolladores.', N'teclado, mecanico, rgb', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (9, N'Monitor 27 QHD', N'monitor-27-qhd', N'Monitor de 27 pulgadas con resoluci�n QHD.', N'Monitor QHD', 4, 4, 1, 1, 1, 0, 1, N'Monitor QHD 27', N'Monitor ideal para productividad.', N'monitor, qhd, oficina', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (10, N'Disco SSD NVMe 1TB', N'ssd-nvme-1tb', N'Almacenamiento SSD NVMe de alta velocidad.', N'SSD 1TB', 6, 6, 1, 1, 0, 1, 1, N'SSD NVMe 1TB', N'SSD r�pido para sistemas modernos.', N'ssd, nvme, almacenamiento', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (11, N'Impresora Multifuncional WiFi', N'impresora-multifuncional-wifi', N'Impresora con esc�ner y conexi�n WiFi.', N'Impresora multifuncional', 7, 7, 1, 1, 0, 0, 1, N'Impresora WiFi', N'Impresora todo en uno para oficina.', N'impresora, wifi, oficina', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (12, N'C�mara Mirrorless Pro', N'camara-mirrorless-pro', N'C�mara mirrorless profesional.', N'C�mara profesional', 8, 8, 1, 1, 1, 0, 1, N'C�mara Mirrorless', N'C�mara avanzada para fotograf�a.', N'camara, fotografia, profesional', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (13, N'Smartwatch Fitness', N'smartwatch-fitness', N'Reloj inteligente enfocado en salud y deporte.', N'Smartwatch deportivo', 1, 9, 1, 1, 0, 0, 1, N'Smartwatch Fitness', N'Reloj inteligente para actividad f�sica.', N'smartwatch, fitness, salud', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (14, N'Parlante Bluetooth Port�til', N'parlante-bluetooth-portatil', N'Parlante port�til con sonido potente.', N'Parlante port�til', 3, 3, 1, 1, 0, 0, 1, N'Parlante Bluetooth', N'Parlante inal�mbrico port�til.', N'parlante, bluetooth, audio', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (15, N'Router WiFi 6 AX', N'router-wifi-6-ax', N'Router de alto rendimiento con WiFi 6.', N'Router WiFi 6', 9, 10, 1, 1, 1, 0, 1, N'Router WiFi 6', N'Router de �ltima generaci�n.', N'router, wifi6, red', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (16, N'Webcam Full HD', N'webcam-full-hd', N'C�mara web para videollamadas en alta definici�n.', N'Webcam Full HD', 7, 5, 1, 1, 0, 0, 1, N'Webcam HD', N'C�mara web para trabajo remoto.', N'webcam, video, full hd', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (17, N'Consola Gaming NextGen', N'consola-gaming-nextgen', N'Consola de videojuegos de nueva generaci�n.', N'Consola gaming', 10, 11, 1, 1, 1, 0, 1, N'Consola NextGen', N'Consola de alto rendimiento.', N'consola, gaming, videojuegos', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (18, N'Control Inal�mbrico Pro', N'control-inalambrico-pro', N'Control inal�mbrico ergon�mico.', N'Control gaming', 10, 11, 1, 1, 0, 0, 1, N'Control Pro', N'Control inal�mbrico para consola.', N'control, gaming, inalambrico', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (19, N'Silla Gamer Ergon�mica', N'silla-gamer-ergonomica', N'Silla gamer con soporte lumbar.', N'Silla gamer', 11, 12, 1, 1, 1, 0, 1, N'Silla Gamer', N'Silla c�moda para largas sesiones.', N'silla, gamer, ergonomica', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (20, N'Escritorio Ajustable Pro', N'escritorio-ajustable-pro', N'Escritorio regulable en altura.', N'Escritorio ajustable', 11, 12, 1, 1, 0, 0, 1, N'Escritorio Ajustable', N'Escritorio ergon�mico de oficina.', N'escritorio, oficina, ergonomico', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[Product] OFF;


------------------------------------------------------------
-- ProductSku (25 REGISTROS, IDs FIJOS)
------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[ProductSku] ON;

INSERT INTO [dbo].[ProductSku] 
                ([Id],
                [ProductId],
                [SkuCode],
                [BarCode],
                [Price],
                [ComparedAtPrice],
                [Cost],
                [Weight],
                [Length],
                [Width],
                [Height],
                [IsActive],
                [DisplayOrder],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            -- Product 1: Smartphone Galaxy X
            (1, 1, N'GALX-128-BLK', N'780000000001', 799990, 899990, 550000, 0.180, 15.8, 7.4, 0.8, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (2, 1, N'GALX-256-BLK', N'780000000002', 849990, 949990, 600000, 0.182, 15.8, 7.4, 0.8, 1, 2, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 2: iPhone Pro Max
            (3, 2, N'IPPM-256-SLV', N'780000000003', 1199990, 1299990, 900000, 0.221, 16.0, 7.8, 0.8, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (4, 2, N'IPPM-512-SLV', N'780000000004', 1349990, 1449990, 1050000, 0.224, 16.0, 7.8, 0.8, 1, 2, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 3: Laptop Ultrabook Pro
            (5, 3, N'ULTRA-I7-16GB', N'780000000005', 1499990, 1599990, 1200000, 1.250, 32.0, 22.0, 1.6, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 4: Auriculares Wireless ANC
            (6, 4, N'ANC-BLK', N'780000000006', 199990, 249990, 120000, 0.280, 20.0, 18.0, 8.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 5: Smart TV 65 4K
            (7, 5, N'STV65-4K', N'780000000007', 899990, 999990, 700000, 18.500, 145.0, 85.0, 8.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 6: Tablet Android Plus
            (8, 6, N'TAB-128-GRY', N'780000000008', 349990, 399990, 260000, 0.480, 25.0, 16.0, 0.7, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 7: Mouse Inal�mbrico Ergo
            (9, 7, N'MOUSE-ERGO', N'780000000009', 29990, 39990, 15000, 0.095, 12.0, 7.0, 4.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 8: Teclado Mec�nico Pro
            (10, 8, N'KEY-RGB-RED', N'780000000010', 89990, 119990, 60000, 0.980, 44.0, 13.0, 3.5, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 9: Monitor 27 QHD
            (11, 9, N'MON27-QHD', N'780000000011', 329990, 379990, 250000, 4.800, 61.0, 36.0, 5.5, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 10: Disco SSD NVMe 1TB
            (12, 10, N'SSD-NVME-1TB', N'780000000012', 129990, 159990, 90000, 0.030, 8.0, 2.2, 0.3, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 11: Impresora Multifuncional WiFi
            (13, 11, N'PRN-WIFI', N'780000000013', 199990, 229990, 150000, 6.200, 42.0, 36.0, 25.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 12: C�mara Mirrorless Pro
            (14, 12, N'CAM-MIR-24MP', N'780000000014', 899990, 999990, 700000, 0.650, 13.5, 10.0, 7.5, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 13: Smartwatch Fitness
            (15, 13, N'SW-FIT-BLK', N'780000000015', 149990, 179990, 100000, 0.055, 4.5, 4.5, 1.2, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 14: Parlante Bluetooth Port�til
            (16, 14, N'SPK-BT-20W', N'780000000016', 79990, 99990, 55000, 0.720, 18.0, 8.0, 8.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 15: Router WiFi 6
            (17, 15, N'RT-WIFI6', N'780000000017', 129990, 159990, 95000, 0.680, 24.0, 16.0, 4.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 16: Webcam Full HD
            (18, 16, N'WEBCAM-FHD', N'780000000018', 59990, 79990, 40000, 0.120, 7.5, 5.0, 4.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 17: Consola Gaming NextGen
            (19, 17, N'CONSOLE-NG', N'780000000019', 549990, 599990, 420000, 4.200, 39.0, 26.0, 10.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 18: Control Inal�mbrico Pro
            (20, 18, N'CTRL-PRO', N'780000000020', 69990, 89990, 45000, 0.280, 16.0, 11.0, 6.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 19: Silla Gamer Ergon�mica
            (21, 19, N'SILLA-GAMER', N'780000000021', 229990, 279990, 180000, 18.000, 85.0, 65.0, 32.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 20: Escritorio Ajustable Pro
            (22, 20, N'DESK-ADJ', N'780000000022', 299990, 349990, 230000, 28.000, 140.0, 75.0, 12.0, 1, 1, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- SKUs adicionales (variantes simples)
            (23, 1, N'GALX-128-WHT', N'780000000023', 799990, 899990, 550000, 0.180, 15.8, 7.4, 0.8, 1, 3, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (24, 2, N'IPPM-256-GRY', N'780000000024', 1199990, 1299990, 900000, 0.221, 16.0, 7.8, 0.8, 1, 3, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),
            (25, 4, N'ANC-WHT', N'780000000025', 199990, 249990, 120000, 0.280, 20.0, 18.0, 8.0, 1, 2, SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[ProductSku] OFF;


------------------------------------------------------------
-- ProductAttributeValue 
------------------------------------------------------------

INSERT INTO [dbo].[ProductAttributeValue]
                ([ProductSkuId],
                [AttributeValueId],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        SELECT
            ps.Id AS ProductSkuId,
            av.Id AS AttributeValueId,
            SYSDATETIMEOFFSET(),
            N'System',
            SYSDATETIMEOFFSET(),
            N'System'
        FROM [dbo].[ProductSku] ps
        INNER JOIN [dbo].[AttributeValue] av
            ON av.AttributeId IN (
                SELECT Id FROM [dbo].[Attribute] WHERE IsVariant = 1
            )
        WHERE ps.Id <= (
            SELECT MIN(Id) + 4 FROM [dbo].[ProductSku]
        );


------------------------------------------------------------
-- ProductGallery (17 REGISTROS, IDs FIJOS)
------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[ProductGallery] ON;

INSERT INTO [dbo].[ProductGallery] 
                ([Id],
                [ProductId],
                [ProductSkuId],
                [MediaUrl],
                [MediaType],
                [MimeType],
                [IsPrimary],
                [DisplayOrder],
                [AltText],
                [CreatedAt],
                [CreatedBy],
                [LastModified],
                [LastModifiedBy])
        VALUES
            -- Product 1: Smartphone Galaxy X
            (1, 1, 1, N'https://cdn.example.com/products/galaxy-x/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Smartphone Galaxy X vista frontal', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (2, 1, 1, N'https://cdn.example.com/products/galaxy-x/back.jpg', N'image', N'image/jpeg', 0, 2,
             N'Smartphone Galaxy X vista trasera', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (3, 1, 2, N'https://cdn.example.com/products/galaxy-x/side.jpg', N'image', N'image/jpeg', 0, 3,
             N'Smartphone Galaxy X vista lateral', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 2: iPhone Pro Max
            (4, 2, 3, N'https://cdn.example.com/products/iphone-pro-max/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'iPhone Pro Max color plata', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (5, 2, 4, N'https://cdn.example.com/products/iphone-pro-max/camera.jpg', N'image', N'image/jpeg', 0, 2,
             N'iPhone Pro Max detalle c�mara', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 3: Laptop Ultrabook Pro
            (6, 3, 5, N'https://cdn.example.com/products/ultrabook-pro/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Ultrabook Pro vista principal', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (7, 3, 5, N'https://cdn.example.com/products/ultrabook-pro/keyboard.jpg', N'image', N'image/jpeg', 0, 2,
             N'Ultrabook Pro teclado retroiluminado', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 4: Auriculares Wireless ANC
            (8, 4, 6, N'https://cdn.example.com/products/headphones-anc/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Auriculares inal�mbricos con cancelaci�n activa', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (9, 4, 6, N'https://cdn.example.com/products/headphones-anc/case.jpg', N'image', N'image/jpeg', 0, 2,
             N'Auriculares ANC con estuche de carga', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 5: Smart TV 65 4K
            (10, 5, 7, N'https://cdn.example.com/products/smart-tv-65/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Smart TV 65 pulgadas resoluci�n 4K', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 6: Tablet Android Plus
            (11, 6, 8, N'https://cdn.example.com/products/tablet-android/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Tablet Android Plus vista frontal', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (12, 6, 8, N'https://cdn.example.com/products/tablet-android/back.jpg', N'image', N'image/jpeg', 0, 2,
             N'Tablet Android Plus vista posterior', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 9: Monitor 27 QHD
            (13, 9, 11, N'https://cdn.example.com/products/monitor-27qhd/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Monitor 27 pulgadas QHD', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 12: C�mara Mirrorless
            (14, 12, 14, N'https://cdn.example.com/products/mirrorless/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'C�mara mirrorless profesional', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (15, 12, 14, N'https://cdn.example.com/products/mirrorless/lens.jpg', N'image', N'image/jpeg', 0, 2,
             N'C�mara mirrorless con lente', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            -- Product 17: Consola Gaming
            (16, 17, 19, N'https://cdn.example.com/products/console/main.jpg', N'image', N'image/jpeg', 1, 1,
             N'Consola gaming de �ltima generaci�n', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System'),

            (17, 17, 19, N'https://cdn.example.com/products/console/controller.jpg', N'image', N'image/jpeg', 0, 2,
             N'Control inal�mbrico de consola gaming', SYSDATETIMEOFFSET(), N'System', SYSDATETIMEOFFSET(), N'System');

SET IDENTITY_INSERT [dbo].[ProductGallery] OFF;