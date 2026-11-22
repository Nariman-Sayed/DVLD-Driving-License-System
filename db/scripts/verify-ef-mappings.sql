/*
    Read-only EF Core mapping verification for the DVLD development database.

    Run with:
        sqlcmd -S <server> -d DVLD -E -i db/scripts/verify-ef-mappings.sql

    This script only reads metadata views. It never modifies schema or data.
    It is intended to be compared with:
        src/DVLD.Api/Data/Configurations/ApplicationTypeConfiguration.cs
        src/DVLD.Api/Data/Configurations/TestTypeConfiguration.cs
        src/DVLD.Api/Data/Configurations/ApplicationConfiguration.cs
        src/DVLD.Api/Data/Configurations/UserConfiguration.cs
        src/DVLD.Api/Data/Configurations/PersonConfiguration.cs
        src/DVLD.Api/Data/Configurations/CountryConfiguration.cs
*/

SET NOCOUNT ON;

SELECT
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    NUMERIC_PRECISION,
    NUMERIC_SCALE,
    IS_NULLABLE,
    COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IS_IDENTITY
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Countries', 'ApplicationTypes', 'TestTypes', 'Applications', 'Users', 'People')
ORDER BY TABLE_NAME, ORDINAL_POSITION;

SELECT
    tc.TABLE_NAME,
    tc.CONSTRAINT_TYPE,
    kcu.COLUMN_NAME,
    kcu.ORDINAL_POSITION
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
    ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
    AND tc.TABLE_SCHEMA = kcu.TABLE_SCHEMA
WHERE tc.TABLE_NAME IN ('Countries', 'ApplicationTypes', 'TestTypes', 'Applications', 'Users', 'People')
ORDER BY tc.TABLE_NAME, tc.CONSTRAINT_TYPE, kcu.ORDINAL_POSITION;

SELECT
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    c.name AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    rc.name AS ReferencedColumn
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc
    ON fk.object_id = fkc.constraint_object_id
JOIN sys.columns c
    ON fkc.parent_object_id = c.object_id
    AND fkc.parent_column_id = c.column_id
JOIN sys.columns rc
    ON fkc.referenced_object_id = rc.object_id
    AND fkc.referenced_column_id = rc.column_id
WHERE OBJECT_NAME(fk.parent_object_id) IN
    ('Countries', 'ApplicationTypes', 'TestTypes', 'Applications', 'Users', 'People')
ORDER BY TableName, ForeignKeyName;

/* Every People column projected by PersonRepository, in ordinal order. */
SELECT
    ORDINAL_POSITION,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'People'
  AND COLUMN_NAME IN (
      'PersonID', 'NationalNo', 'FirstName', 'SecondName', 'ThirdName',
      'LastName', 'DateOfBirth', 'Gendor', 'Address', 'Phone', 'Email',
      'NationalityCountryID', 'ImagePath')
ORDER BY ORDINAL_POSITION;

/* Country columns projected by CountryRepository. */
SELECT
    ORDINAL_POSITION,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Countries'
  AND COLUMN_NAME IN ('CountryID', 'CountryName')
ORDER BY ORDINAL_POSITION;

/* Shows the unique index created by scripts/001-unique-national-no.sql, if present. */
SELECT
    i.name AS IndexName,
    i.is_unique AS IsUnique,
    COL_NAME(ic.object_id, ic.column_id) AS ColumnName
FROM sys.indexes i
JOIN sys.index_columns ic
    ON i.object_id = ic.object_id
    AND i.index_id = ic.index_id
WHERE i.object_id = OBJECT_ID(N'dbo.People')
  AND COL_NAME(ic.object_id, ic.column_id) = 'NationalNo';
