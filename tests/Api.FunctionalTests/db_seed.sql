INSERT INTO Country (CountryId, Name, CreatedAt, CreatedBy, LastModified, LastModifiedBy)
VALUES (1, 'Chile', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system');

INSERT INTO Region (RegionId, Name, CreatedAt, CreatedBy, LastModified, LastModifiedBy) VALUES
(1, 'Arica y Parinacota', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(2, 'Tarapacá', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(3, 'Antofagasta', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(4, 'Atacama', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(5, 'Coquimbo', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(6, 'Valparaíso', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(7, 'Metropolitana de Santiago', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(8, 'O’Higgins', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(9, 'Maule', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(10, 'Ñuble', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(11, 'Biobío', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(12, 'La Araucanía', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(13, 'Los Ríos', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(14, 'Los Lagos', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(15, 'Aysén', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(16, 'Magallanes y Antártica Chilena', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system');

INSERT INTO Municipality (MunicipalityId, RegionId, Name, CreatedAt, CreatedBy, LastModified, LastModifiedBy) VALUES
(1, 7, 'Santiago', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(2, 7, 'Providencia', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(3, 7, 'Las Condes', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(4, 6, 'Viña del Mar', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(5, 6, 'Valparaíso', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(6, 5, 'La Serena', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(7, 5, 'Coquimbo', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(8, 3, 'Antofagasta', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(9, 3, 'Calama', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(10, 4, 'Copiapó', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(11, 9, 'Talca', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(12, 9, 'Curicó', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(13, 11, 'Concepción', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(14, 11, 'Los Ángeles', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(15, 12, 'Temuco', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(16, 12, 'Padre Las Casas', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(17, 14, 'Puerto Montt', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(18, 14, 'Osorno', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(19, 13, 'Valdivia', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'),
(20, 10, 'Chillán', SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system');


DECLARE @i INT = 1;

WHILE @i <= 30
BEGIN
    INSERT INTO [User]
    (Avatar, FirstName, LastName, Username, Email, PasswordHash, DateOfBirth,
     PhoneNumber, CreatedAt, CreatedBy, LastModified, LastModifiedBy)
    VALUES
    (NULL,
     CONCAT('Nombre', @i),
     CONCAT('Apellido', @i),
     CONCAT('user', @i),
     CONCAT('user', @i, '@mail.com'),
     '123456',
     DATEADD(YEAR, -25, GETDATE()),
     CONCAT('+569000000', @i),
     SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'
    );

    SET @i = @i + 1;
END;


DECLARE @j INT = 1;

WHILE @j <= 30
BEGIN
    INSERT INTO Address
    (CountryId, MunicipalityId, Title, City, Street, Number, Apartament,
     Reference, PostalCode, CreatedAt, CreatedBy, LastModified, LastModifiedBy)
    VALUES
    (1,
     (SELECT MunicipalityId FROM Municipality ORDER BY NEWID() OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY),
     CONCAT('Direccion ', @j),
     'Ciudad',
     CONCAT('Calle ', @j),
     CAST(@j AS NVARCHAR(20)),
     NULL,
     CONCAT('Referencia ', @j),
     '0000000',
     SYSDATETIMEOFFSET(), 'system', SYSDATETIMEOFFSET(), 'system'
    );

    SET @j = @j + 1;
END;


DECLARE @u INT = 1;

WHILE @u <= 30
BEGIN
    INSERT INTO AddressUser
    (AddressId, UserId, IsDefault, CreatedAt, CreatedBy, LastModified, LastModifiedBy)
    VALUES
    (
        (SELECT TOP 1 AddressId FROM Address ORDER BY NEWID()),
        @u,                                                     
        1,                                                      
        SYSDATETIMEOFFSET(), 
        'system',
        SYSDATETIMEOFFSET(),
        'system'
    );

    SET @u = @u + 1;
END;
