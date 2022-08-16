using System;
using FluentMigrator.Builders.Create.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Natureofbusinesses;
using System.Data.SqlClient;
using FluentMigrator.Runner;


namespace Nop.Data.Migrations
{
    //[NopMigration("2022-07-20 09:00:00", "4.50.2", UpdateMigrationType.Data, MigrationProcessType.Update)]
    [NopMigration("2022-07-20 14:43:16", "4.50.2", UpdateMigrationType.Data, MigrationProcessType.Update)]
    //[Migration(19072022100000, "4.50.3", UpdateMigrationType.Data, MigrationProcessType.NoMatter)]
    public class M20072022_100003_AddNatureOfBusiness : Migration
    {

        //private readonly IMigrationManager _migrationManager;

        //private readonly INopDataProvider _dataProvider;

        //public Migration_19072022100000(INopDataProvider dataProvider)
        //{
        //    _dataProvider = dataProvider;
        //}
        //public Migration_19072022100000(IMigrationManager migrationManager)
        //{
        //    _migrationManager = migrationManager;
        //}
        public override void Up()
        {

            // https://fluentmigrator.github.io/articles/fluent-interface.html
            //Create.Table("CustomerNatureOfBusiness")
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Id)).AsInt32().NotNullable().PrimaryKey("PK_CustomerNatureOfBusiness_Id")
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessName)).AsString(400).NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.CustomerId)).AsInt32().NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Email)).AsString(500).NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Username)).AsString(500).NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Published)).AsBoolean().NotNullable();

            //Create.Table("NatureOfBusiness")
            //    .AlterColumn(nameof(NatureOfBusiness.Id)).AsInt32().NotNullable().PrimaryKey("PK_NatureOfBusiness_Id")
            //    .AlterColumn(nameof(NatureOfBusiness.NatureOfBusinessName)).AsString(400).NotNullable()
            //    .AlterColumn(nameof(NatureOfBusiness.NatureOfBusinessId)).AsInt32().NotNullable().ForeignKey("CustomerNatureOfBusiness","Id")
            //    .AlterColumn(nameof(NatureOfBusiness.Deleted)).AsBoolean().Nullable()
            //    .AlterColumn(nameof(NatureOfBusiness.SubjectToAcl)).AsBoolean().NotNullable()
            //    .AlterColumn(nameof(NatureOfBusiness.Published)).AsBoolean().NotNullable()
            //    .AlterColumn(nameof(NatureOfBusiness.CreatedOnUtc)).AsDateTime2().Nullable()
            //    .AlterColumn(nameof(NatureOfBusiness.UpdatedOnUtc)).AsDateTime2().Nullable();

            Alter.Table("NatureOfBusiness")
                .AlterColumn(nameof(Natureofbusiness.Id)).AsInt32().NotNullable().PrimaryKey("PK_NatureOfBusiness_Id")
                .AlterColumn(nameof(Natureofbusiness.Name)).AsString(400).NotNullable()
                .AlterColumn(nameof(Natureofbusiness.Deleted)).AsBoolean().Nullable()
                .AlterColumn(nameof(Natureofbusiness.Published)).AsBoolean().NotNullable()
                .AlterColumn(nameof(Natureofbusiness.CreatedOnUtc)).AsDateTime2().Nullable()
                .AlterColumn(nameof(Natureofbusiness.UpdatedOnUtc)).AsDateTime2().Nullable();

            Alter.Table("Customer")
                .AlterColumn(nameof(Customer.ExpectedSalesVolume)).AsDecimal(16,2).Nullable()
            //   .AddColumn(nameof(Customer.ContactPersonAttention)).AsString(400).Nullable()
                .AlterColumn(nameof(Customer.CreditLimit)).AsDecimal(12,2).Nullable()
                .AddColumn(nameof(Customer.BillingType)).AsString(400).Nullable();


            //Alter.Table("CustomerNatureOfBusiness")
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Id)).AsInt32().NotNullable().PrimaryKey("PK_CustomerNatureOfBusiness_Id")
            //    //.AlterColumn(nameof(CustomerNatureOfBusiness.NatureOfBusinessName)).AsString(400).NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.CustomerId)).AsInt32().NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Email)).AsString(500).NotNullable()
            //    .AlterColumn(nameof(CustomerNatureOfBusiness.Username)).AsString(500).NotNullable();
            //.AlterColumn(nameof(CustomerNatureOfBusiness.Published)).AsBoolean().NotNullable();


            //Create.ForeignKey("FK_CustomerNatureOfBusiness_Id")
            //    .FromTable("NatureOfBusiness").ForeignColumn("NatureOfBusinessId")
            //    .ToTable("CustomerNatureOfBusiness").PrimaryColumn("Id");

            //Create.Column(nameof(NatureOfBusiness.NatureOfBusinessName))
            //.OnTable(nameof(NatureOfBusiness))
            //.AsString(400)
            //.NotNullable()
            //.Unique();

        }

        public override void Down()
        {
            //Delete.Table("CustomerNatureOfBusiness");
            //Delete.Column(nameof(Natureofbusiness.NatureOfBusinessId)).FromTable("NatureOfBusiness");
            
        }
    }
}
