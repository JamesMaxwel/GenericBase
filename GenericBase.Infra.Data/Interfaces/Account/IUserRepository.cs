﻿using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common.Generic;
using System.Linq.Expressions;

namespace GenericBase.Infra.Data.Interfaces.Account
{
    public interface IUserRepository :
        IGenericCreateRepository<User>,
        IGenericReadRepository<User>,
        IGenericUpdateRepository<User>,
        IGenericDeleteRepository<User>
    {
        Task<User?> GetFirstOrDefaultWithCredentialsAsync(Guid id);
        Task<User?> GetFirstOrDefaultWithCredentialsAsync(Expression<Func<User, bool>> expression);

    }
}