/*
    Adds a unique index on People(NationalNo) only when the data allows it.

    This script never deletes or updates data. It first counts duplicate NationalNo
    groups. If any exist, it reports only PersonID values and the duplicate count,
    raises an error, and makes no schema change.

    Run manually in SSMS against the DVLD database.
*/

SET NOCOUNT ON;

DECLARE @DuplicateGroups int;

SELECT @DuplicateGroups = COUNT(*)
FROM (
    SELECT NationalNo
    FROM dbo.People
    GROUP BY NationalNo
    HAVING COUNT(*) > 1
) AS duplicates;

IF @DuplicateGroups > 0
BEGIN
    SELECT
        p.PersonID,
        duplicates.Cnt AS DuplicateCount
    FROM dbo.People AS p
    INNER JOIN (
        SELECT NationalNo, COUNT(*) AS Cnt
        FROM dbo.People
        GROUP BY NationalNo
        HAVING COUNT(*) > 1
    ) AS duplicates ON p.NationalNo = duplicates.NationalNo
    ORDER BY duplicates.Cnt DESC, p.PersonID;

    DECLARE @message nvarchar(4000) = CONCAT(
        N'Cannot create the unique index because ',
        @DuplicateGroups,
        N' duplicate NationalNo group(s) exist. No schema change was made.'
    );
    THROW 50001, @message, 1;
END;
ELSE
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'dbo.People')
          AND name = N'UX_People_NationalNo'
    )
    BEGIN
        CREATE UNIQUE INDEX UX_People_NationalNo
            ON dbo.People(NationalNo);

        SELECT N'Unique index UX_People_NationalNo created.' AS Result;
    END
    ELSE
    BEGIN
        SELECT N'Unique index UX_People_NationalNo already exists.' AS Result;
    END
END;
GO

-- Rollback (run only if you need to remove the index):
-- DROP INDEX IF EXISTS UX_People_NationalNo ON dbo.People;
