using System;

namespace Optsol.Components.Infra.Security.Response
{
    public class ValidationResult
    {
        private ValidationResult(bool isSuccess, string subjectId)
        {
            this.IsSuccess = isSuccess;
            this.SubjectId = subjectId;
        }

        public bool IsSuccess { get; }

        public string SubjectId { get; }

        public static ValidationResult Success(string subjectId)
        {
            if (string.IsNullOrWhiteSpace(subjectId))
            {
                throw new ArgumentNullException(nameof(subjectId));
            }

            return new ValidationResult(isSuccess: true, subjectId: subjectId);
        }

        public static ValidationResult Failure()
        {
            return new ValidationResult(isSuccess: false, subjectId: null);
        }
    }
}
