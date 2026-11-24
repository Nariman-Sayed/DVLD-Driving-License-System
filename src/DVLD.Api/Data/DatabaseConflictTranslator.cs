using DVLD.Api.Constants;
using DVLD.Api.Exceptions;

namespace DVLD.Api.Data
{
    /// <summary>
    /// Translates SQL Server constraint error numbers into user-facing conflict exceptions.
    /// </summary>
    public static class DatabaseConflictTranslator
    {
        /// <summary>
        /// Error number for a duplicate unique index or primary key value.
        /// </summary>
        public const int DuplicateKeyNumber = 2601;

        /// <summary>
        /// Error number for a duplicate unique constraint value.
        /// </summary>
        public const int DuplicateConstraintNumber = 2627;

        /// <summary>
        /// Error number for a foreign key constraint violation.
        /// </summary>
        public const int ForeignKeyViolationNumber = 547;

        /// <summary>
        /// Maps a SQL Server error number to a conflict exception when one applies.
        /// </summary>
        /// <param name="errorNumber">The SQL Server error number.</param>
        /// <param name="isDelete"><see langword="true"/> when the failing operation was a delete.</param>
        /// <returns>The conflict exception, or <see langword="null"/> when the error is unrelated.</returns>
        public static ConflictException? Translate(int errorNumber, bool isDelete)
        {
            if (errorNumber is DuplicateKeyNumber or DuplicateConstraintNumber)
            {
                return new ConflictException(ApiErrorMessages.DuplicateNationalNo);
            }

            if (errorNumber == ForeignKeyViolationNumber && isDelete)
            {
                return new ConflictException(ApiErrorMessages.PersonDeleteBlocked);
            }

            return null;
        }
    }
}
