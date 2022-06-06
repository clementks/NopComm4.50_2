using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers
{
    /// <summary>
    /// Represents a Nature Of Business entity builder
    /// </summary>
    public class CustomerNatureOfBusinessBuilder : NopEntityBuilder<CustomerNatureOfBusiness>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(CustomerNatureOfBusiness.Username)).AsString(400).ForeignKey<Customer>()
                .WithColumn(nameof(CustomerNatureOfBusiness.Email)).AsString(400).ForeignKey<Customer>()
                .WithColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessId)).AsInt32().ForeignKey<NatureOfBusiness>()
                .WithColumn(nameof(NatureOfBusiness.NatureOfBusinessName)).AsString(400).NotNullable()
                .WithColumn(nameof(NatureOfBusiness.Published)).AsBoolean().Nullable()
                .WithColumn(nameof(NatureOfBusiness.Deleted)).AsBoolean().Nullable();

        }

        #endregion
    }
}