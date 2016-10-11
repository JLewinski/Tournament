using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tournament.Portable.Models;

namespace Tournament.Portable.Data
{
    public interface ITournamentContext<T> : ITournamentContextBase where T : class, IUser
    {
        DbSet<T> Users { get; set; }
    }
}