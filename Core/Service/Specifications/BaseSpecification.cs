using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    abstract class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>>? CriteriaExpressions)
        {
            Criteria = CriteriaExpressions;
        }
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        #region Includes
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpressions) => IncludeExpressions.Add(includeExpressions);
        #endregion
        #region Sorting
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> OrderByExp) => OrderBy = OrderByExp;
        protected void AddOrderByDes(Expression<Func<TEntity, object>> OrderByDescExp) => OrderByDesc = OrderByDescExp;
        #endregion
        #region Pagination
        public int Take {get; private set; }

        public int Skip {get; private set; }

        public bool IsPaginated { get; set ; }

        protected void ApplyPagination(int PageSize,int PageIndex )
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex-1) * PageSize;
        }
        #endregion


    }
}
