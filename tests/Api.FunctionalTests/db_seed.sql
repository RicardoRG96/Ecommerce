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