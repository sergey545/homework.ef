using Otus.Teaching.PromoCodeFactory.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {

        // подробнее https://metanit.com/sharp/entityframework/3.13.php

        public Task<IEnumerable<T>> GetAllAsync();

        public Task<List<T>> GetItemsListAsync();

        public Task<T> GetByIdOrNullAsync(Guid id);

        public Task<int> Count { get; }

        public  Task<bool> Exists(Guid id);

        public Task<CommonOperationResult> AddAsync(T t);

        public Task<CommonOperationResult> UpdateAsync(T t);

        public Task<CommonOperationResult> DeleteAsync(Guid id);

        public Task<CommonOperationResult> InitAsync(bool deleteDb = false);

    }
}
