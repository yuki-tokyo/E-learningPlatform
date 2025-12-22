using System;
using System.Collections.Generic;
using System.Text;

namespace ProgressService.Domain.Interfaces
{
    public interface IAuthClientForProgress
    {
        Task UpdateUserLevel(string userId, int points);
    }
}
