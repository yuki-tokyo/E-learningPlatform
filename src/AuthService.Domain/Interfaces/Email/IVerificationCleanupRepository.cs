using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces.Email
{
    public interface IVerificationCleanupRepository
    {
        Task<int> DeleteExpiredVerifications(CancellationToken cancellationToken);
    }
}
