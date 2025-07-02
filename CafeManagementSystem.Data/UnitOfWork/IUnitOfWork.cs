using System;
namespace CafeManagementSystem.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
        Task<int> SaveChangesAsync();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollBackTransaction();
    }
}

