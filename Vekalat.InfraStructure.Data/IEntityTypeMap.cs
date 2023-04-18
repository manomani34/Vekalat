using Vekalat.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System;
using System.Reflection.Emit;
using System.Linq;

namespace Vekalat.InfraStructure.Data
{
    public interface IEntityTypeMap
    {
        void Map(ModelBuilder builder);
    }

    public abstract class BaseEntityMap<TEntityType> : IEntityTypeMap
        where TEntityType : class
    {
        public void Map(ModelBuilder builder)
        {
            InternalMap(builder.Entity<TEntityType>());

            GloballMap(builder);
        }

        private static void GloballMap(ModelBuilder builder)
        {
            Expression<Func<Entity, bool>> filterExpr = x => x.IsDeleted == false;
            foreach (var mutableEntityType in builder.Model.GetEntityTypes())
            {
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(Entity)))
                {
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);
                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }
        }

        protected abstract void InternalMap(EntityTypeBuilder<TEntityType> builder);
    }
}
