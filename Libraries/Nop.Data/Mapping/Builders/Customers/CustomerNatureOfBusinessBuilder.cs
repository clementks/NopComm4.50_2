using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Natureofbusinesses;
using Nop.Data.Extensions;
using System.Data;

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
            //table
            //    .WithColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessId)).AsInt64().ForeignKey<NatureOfBusiness>(onDelete: Rule.None).NotNullable()
            //    .WithColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessName)).AsString(400).NotNullable().Unique()
            //    .WithColumn(nameof(CustomerNatureOfBusiness.CustomerId)).AsInt32().ForeignKey<Customer>(onDelete: Rule.None).NotNullable()
            //    .WithColumn(nameof(CustomerNatureOfBusiness.Username)).AsString(1000).NotNullable().Unique()
            //    .WithColumn(nameof(CustomerNatureOfBusiness.Email)).AsString(1000)
            //    .WithColumn(nameof(CustomerNatureOfBusiness.Published)).AsBoolean().NotNullable()
            //    .WithColumn(nameof(CustomerNatureOfBusiness.DisplayOrder)).AsInt32();

            table
               .InSchema("CustomerNatureOfBusiness")
               .WithColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessId)).AsInt64().Identity().PrimaryKey()
               .WithColumn(NameCompatibilityManager.GetColumnName(typeof(CustomerNatureOfBusiness), nameof(CustomerNatureOfBusiness.NatureOfBusinessName))).AsString()
               .WithColumn(NameCompatibilityManager.GetColumnName(typeof(CustomerNatureOfBusiness), nameof(CustomerNatureOfBusiness.NatureOfBusinessId)))
                   .AsInt32().PrimaryKey().ForeignKey<Natureofbusiness>()
               .WithColumn(NameCompatibilityManager.GetColumnName(typeof(CustomerNatureOfBusiness), nameof(CustomerNatureOfBusiness.Id)))
                   .AsInt32().ForeignKey<Customer>();



        }

        #endregion
    }
}