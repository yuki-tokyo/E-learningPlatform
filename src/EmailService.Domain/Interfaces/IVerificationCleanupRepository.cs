using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Interfaces
{
    public interface IVerificationCleanupRepository
    {
        Task<int> DeleteExpiredVerifications(CancellationToken cancellationToken);
    }
}
