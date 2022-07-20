using System;
using System.Linq;
using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Shipping;
using Nop.Data.Mapping;
using Nop.Core.Domain.Orders;

// added these libraries for fluent migration 
// using FluentMigrator;

namespace Nop.Data.Migrations.UpgradeTo450
{
    [NopMigration("2021-04-23 00:00:00", "4.50.0", UpdateMigrationType.Data, MigrationProcessType.Update)]
    public class DataMigration : Migration
    {
        private readonly INopDataProvider _dataProvider;

        public DataMigration(INopDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            // add column
            var shipmentTableName = nameof(Shipment);
            var collectedDateUtcColumnName = "ReadyForPickupDateUtc";

            if (!Schema.Table(shipmentTableName).Column(collectedDateUtcColumnName).Exists())
            {
                Alter.Table(shipmentTableName)
                    .AddColumn(collectedDateUtcColumnName).AsDateTime2().Nullable();
            }

            // add message template
            if (!_dataProvider.GetTable<MessageTemplate>().Any(pr => string.Compare(pr.Name, MessageTemplateSystemNames.ShipmentReadyForPickupCustomerNotification, true) == 0))
            {
                var messageTemplate = _dataProvider.InsertEntity(
                    new MessageTemplate
                    {
                        Name = MessageTemplateSystemNames.ShipmentReadyForPickupCustomerNotification,
                        Subject = "Your order from %Store.Name% has been %if (!%Order.IsCompletelyReadyForPickup%) partially endif%ready for pickup.",
                        Body = $"<p>{Environment.NewLine}<a href=\"%Store.URL%\"> %Store.Name%</a>{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Hello %Order.CustomerFullName%!,{Environment.NewLine}<br />{Environment.NewLine}Good news! You order has been%if (!%Order.IsCompletelyReadyForPickup%) partially endif%ready for pickup.{Environment.NewLine}<br />{Environment.NewLine}Order Number: %Order.OrderNumber%{Environment.NewLine}<br />{Environment.NewLine}Order Details: <a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>{Environment.NewLine}<br />{Environment.NewLine}Date Ordered: %Order.CreatedOn%{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Billing Address{Environment.NewLine}<br />{Environment.NewLine}%Order.BillingFirstName% %Order.BillingLastName%{Environment.NewLine}<br />{Environment.NewLine}%Order.BillingAddress1%{Environment.NewLine}<br />{Environment.NewLine}%Order.BillingAddress2%{Environment.NewLine}<br />{Environment.NewLine}%Order.BillingCity% %Order.BillingZipPostalCode%{Environment.NewLine}<br />{Environment.NewLine}%Order.BillingStateProvince% %Order.BillingCountry%{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}%if (%Order.Shippable%) Shipping Address{Environment.NewLine}<br />{Environment.NewLine}%Order.ShippingFirstName% %Order.ShippingLastName%{Environment.NewLine}<br />{Environment.NewLine}%Order.ShippingAddress1%{Environment.NewLine}<br />{Environment.NewLine}%Order.ShippingAddress2%{Environment.NewLine}<br />{Environment.NewLine}%Order.ShippingCity% %Order.ShippingZipPostalCode%{Environment.NewLine}<br />{Environment.NewLine}%Order.ShippingStateProvince% %Order.ShippingCountry%{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Shipping Method: %Order.ShippingMethod%{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine} endif% Products ready for pickup:{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}%Shipment.Product(s)%{Environment.NewLine}</p>{Environment.NewLine}",
                        IsActive = true,
                        EmailAccountId = _dataProvider.GetTable<EmailAccount>().FirstOrDefault()?.Id ?? 0
                    }
                );
            }
            //#5547
            var scheduleTaskTableName = NameCompatibilityManager.GetTableName(typeof(ScheduleTask));

            //add column
            if (!Schema.Table(scheduleTaskTableName).Column(nameof(ScheduleTask.LastEnabledUtc)).Exists())
            {
                Alter.Table(scheduleTaskTableName)
                    .AddColumn(nameof(ScheduleTask.LastEnabledUtc)).AsDateTime2().Nullable();
            }
            else
            {
                Alter.Table(scheduleTaskTableName).AlterColumn(nameof(ScheduleTask.LastEnabledUtc)).AsDateTime2().Nullable();
            }

            //#5939
            if (!_dataProvider.GetTable<PermissionRecord>().Any(pr => string.Compare(pr.SystemName, "SalesSummaryReport", StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                var salesSummaryReportPermission = _dataProvider.InsertEntity(
                    new PermissionRecord
                    {
                        Name = "Admin area. Access sales summary report",
                        SystemName = "SalesSummaryReport",
                        Category = "Orders"
                    }
                );

                //add it to the Admin role by default
                var adminRole = _dataProvider
                    .GetTable<CustomerRole>()
                    .FirstOrDefault(x => x.IsSystemRole && x.SystemName == NopCustomerDefaults.AdministratorsRoleName);

                _dataProvider.InsertEntity(
                    new PermissionRecordCustomerRoleMapping
                    {
                        CustomerRoleId = adminRole.Id,
                        PermissionRecordId = salesSummaryReportPermission.Id
                    }
                );
            }

            //add column
            var returnRequestTableName = NameCompatibilityManager.GetTableName(typeof(ReturnRequest));
            var returnedQuantityColumnName = "ReturnedQuantity";

            if (!Schema.Table(returnRequestTableName).Column(returnedQuantityColumnName).Exists())
            {
                Alter.Table(returnRequestTableName)
                    .AddColumn(returnedQuantityColumnName).AsInt32().NotNullable().SetExistingRowsTo(0);
            }

            //#6053
            if (!_dataProvider.GetTable<PermissionRecord>().Any(pr => string.Compare(pr.SystemName, "ManageAppSettings", StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                var manageConnectionStringPermission = _dataProvider.InsertEntity(
                    new PermissionRecord
                    {
                        Name = "Admin area. Manage App Settings",
                        SystemName = "ManageAppSettings",
                        Category = "Configuration"
                    }
                );

                //add it to the Admin role by default
                var adminRole = _dataProvider
                    .GetTable<CustomerRole>()
                    .FirstOrDefault(x => x.IsSystemRole && x.SystemName == NopCustomerDefaults.AdministratorsRoleName);

                _dataProvider.InsertEntity(
                    new PermissionRecordCustomerRoleMapping
                    {
                        CustomerRoleId = adminRole.Id,
                        PermissionRecordId = manageConnectionStringPermission.Id
                    }
                );
            }

            //add or create table "Nature of Business"
            // Create.Table("NatureOfBusiness")
            //.WithColumn(nameof(NatureOfBusiness.Id)).AsInt64().NotNullable().Identity().PrimaryKey()
            //.WithColumn(nameof(NatureOfBusiness.NatureOfBusinessName)).AsString().NotNullable().Unique()
            //.WithColumn(nameof(NatureOfBusiness.Deleted)).AsBoolean().Nullable()
            //.WithColumn(nameof(NatureOfBusiness.SubjectToAcl)).AsBoolean().NotNullable()
            //.WithColumn(nameof(NatureOfBusiness.Published)).AsBoolean().NotNullable()
            //.WithColumn(nameof(NatureOfBusiness.CreatedOnUtc)).AsDateTime2().NotNullable()
            //.WithColumn(nameof(NatureOfBusiness.UpdatedOnUtc)).AsDateTime2().NotNullable();


            // add or create table "Customer Nature of Business"
            // Create.Table("CustomerNatureOfBusiness")
            //.WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            //.WithColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessId)).AsInt32().ForeignKey<NatureOfBusiness>(onDelete: Rule.None).NotNullable()
            //.WithColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessName)).AsString(255).NotNullable().Unique()
            //.WithColumn(nameof(CustomerNatureOfBusiness.CustomerId)).AsInt32().ForeignKey<Customer>(onDelete: Rule.None).NotNullable()
            //.WithColumn(nameof(CustomerNatureOfBusiness.Username)).AsString(1000).NotNullable().Unique()
            //.WithColumn(nameof(CustomerNatureOfBusiness.Email)).AsString(1000)
            //.WithColumn(nameof(CustomerNatureOfBusiness.Published)).AsBoolean().NotNullable()
            //.WithColumn(nameof(CustomerNatureOfBusiness.DisplayOrder)).AsInt32();


        }

        public override void Down()
        {
            //add the downgrade logic if necessary 
            Delete.Table("NatureOfBusiness");
        }
    }
}
