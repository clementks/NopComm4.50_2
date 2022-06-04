using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Builders.Customers
{
    /// <summary>
    /// Represents a Nature Of Business entity builder
    /// </summary>
    public class NatureOfBusinessBuilder : NopEntityBuilder<NatureOfBusiness>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NatureOfBusiness.NatureOfBusinessName)).AsString(400).NotNullable()
                .WithColumn(nameof(NatureOfBusiness.Deleted)).AsBoolean().Nullable()
                .WithColumn(nameof(NatureOfBusiness.LimitedToStores)).AsBoolean().NotNullable()
                .WithColumn(nameof(NatureOfBusiness.SubjectToAcl)).AsBoolean().NotNullable()
                .WithColumn(nameof(NatureOfBusiness.CreatedOnUtc)).AsDateTime2().Nullable()
                .WithColumn(nameof(NatureOfBusiness.UpdatedOnUtc)).AsDateTime2().Nullable();
        }

        #endregion
    }
}