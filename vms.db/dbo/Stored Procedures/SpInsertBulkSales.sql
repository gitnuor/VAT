-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertBulkSales]
    -- Add the parameters for the stored procedure here
    @OrganizationId INT,
    @SecurityToken NVARCHAR(100),
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @SalesJson NVARCHAR(MAX),
    @SalesDetailsJson NVARCHAR(MAX),
    @PaymentReceiveJson NVARCHAR(MAX),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ErrorMsg NVARCHAR(500) = N'';
    BEGIN TRY
        BEGIN TRANSACTION;
        --Statement
        DECLARE @NumberOfDuplicateProdInSingleSale INT,
                @ApiTransactionId BIGINT,
                @ApiTransactionTypeId INT = 1;

        DECLARE @osdtVatType INT = 14,
                @osdtBankBranch INT = 3,
                @osdtCountry INT = 5,
                @osdtSalesType INT = 17,
                @osdtExportType INT = 9,
                @osdtSalesDeliveryType INT = 16,
                @osdtDeliveryMethod INT = 7,
                @osdtNBREconomicCode INT = 12,
                @osdtPaymentMethod INT = 13,
                @osdtCustomsAndVATCommissionarate INT = 6;




        DECLARE @SlsDtlRaw TABLE
        (
            [ProductId] [INT] NOT NULL,
            [ProductVATTypeId] [INT] NOT NULL,
            [Quantity] [DECIMAL](18, 2) NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [DiscountPerItem] [DECIMAL](18, 2) NOT NULL,
            [VATPercent] [DECIMAL](18, 2) NOT NULL,
            [SupplementaryDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL,
            [ReferenceKey] NVARCHAR(100) NULL,
            [SalesReferenceKey] NVARCHAR(100) NULL
        );

        INSERT INTO @SlsDtlRaw
        (
            ProductId,
            ProductVATTypeId,
            Quantity,
            UnitPrice,
            DiscountPerItem,
            VATPercent,
            SupplementaryDutyPercent,
            MeasurementUnitId,
            ReferenceKey,
            SalesReferenceKey
        )
        SELECT prod.ProductId,              -- ProductId - int
               osd.DataKey,                 -- ProductVATTypeId - int
               jd.Quantity,                 -- Quantity - decimal(18, 2)
               jd.UnitPrice,                -- UnitPrice - decimal(18, 2)
               jd.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
               jd.Vatpercent,               -- VATPercent - decimal(18, 2)
               jd.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
               mu.MeasurementUnitId,        -- MeasurementUnitId - int
               jd.SalesDetailId,            -- ReferenceKey - nvarchar(100)
               jd.SalesId                   -- SalesReferenceKey - nvarchar(100)
        FROM
            OPENJSON(@SalesDetailsJson)
            WITH
            (
                [SalesDetailId] NVARCHAR(100),
                [SalesId] NVARCHAR(100),
                [ProductId] [INT],
                [ProductVattypeId] [INT],
                [Quantity] [DECIMAL](18, 2),
                [UnitPrice] [DECIMAL](18, 2),
                [DiscountPerItem] [DECIMAL](18, 2),
                [Vatpercent] [DECIMAL](18, 2),
                [SupplementaryDutyPercent] [DECIMAL](18, 2),
                [MeasurementUnitId] [INT]
            ) jd
            LEFT JOIN dbo.Products prod
                ON prod.ReferenceKey = jd.ProductId
            LEFT JOIN dbo.OrgStaticData osd
                ON osd.ReferenceKey = jd.ProductVattypeId
            LEFT JOIN dbo.MeasurementUnits mu
                ON mu.ReferenceKey = jd.MeasurementUnitId
        WHERE (
                  osd.OrgStaticDataTypeId = @osdtVatType
                  AND osd.OrganizationId = @OrganizationId
                  AND osd.IsActive = 1
              )
              AND prod.OrganizationId = @OrganizationId
              AND mu.OrganizationId = @OrganizationId;

        WITH cteSalesDetails
        AS (SELECT ROW_NUMBER() OVER (PARTITION BY sdr.SalesReferenceKey,
                                                   sdr.ProductId
                                      ORDER BY sdr.ProductId
                                     ) AS NumberOfSingleProdInSingleSales,
                   sdr.ProductId,
                   sdr.SalesReferenceKey
            FROM @SlsDtlRaw sdr)
        SELECT @NumberOfDuplicateProdInSingleSale = COUNT(1)
        FROM cteSalesDetails
        WHERE cteSalesDetails.NumberOfSingleProdInSingleSales > 1;

        SET @NumberOfDuplicateProdInSingleSale = ISNULL(@NumberOfDuplicateProdInSingleSale, 0);

        IF @NumberOfDuplicateProdInSingleSale > 0
        BEGIN
            SET @ErrorMsg = N'Product duplication in sale is not allowed!!';
            RAISERROR(   @ErrorMsg, -- Message text.  
                         20,        -- Severity.  
                         -1         -- State.  
                     );
        END;

        DECLARE @PaymentReceive TABLE
        (
            [ReceivedPaymentMethodId] [INT] NOT NULL,
            [ReceiveAmount] [DECIMAL](18, 2) NOT NULL,
            [ReceiveDate] [DATETIME] NOT NULL,
            [ReferenceKey] NVARCHAR(100) NULL,
            [SalesReferenceKey] NVARCHAR(100) NULL
        );

        IF @PaymentReceiveJson IS NOT NULL
           AND LEN(@PaymentReceiveJson) > 0
        BEGIN
            INSERT INTO @PaymentReceive
            (
                ReceivedPaymentMethodId,
                ReceiveAmount,
                ReceiveDate,
                ReferenceKey,
                SalesReferenceKey
            )
            SELECT osd.DataKey,
                   jd.ReceiveAmount,
                   jd.ReceiveDate,
                   jd.SalesPaymentReceiveId,
                   jd.SalesId
            FROM
                OPENJSON(@PaymentReceiveJson)
                WITH
                (
                    [SalesPaymentReceiveId] NVARCHAR(100),
                    [SalesId] NVARCHAR(100),
                    [ReceivedPaymentMethodId] [INT],
                    [ReceiveAmount] [DECIMAL](18, 2),
                    [ReceiveDate] [DATETIME]
                ) jd
                LEFT JOIN dbo.OrgStaticData osd
                    ON osd.ReferenceKey = jd.ReceivedPaymentMethodId
            WHERE (
                      osd.OrgStaticDataTypeId = @osdtPaymentMethod
                      AND osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                  )
                  OR osd.OrgStaticDataId IS NULL;

        END;



        DECLARE @Sales TABLE
        (
            [InvoiceNo] [NVARCHAR](50) NOT NULL,
            [VatChallanNo] [NVARCHAR](50) NULL,
            [OrganizationId] [INT] NOT NULL,
            [NoOfIteams] [INT] NOT NULL,
            [TotalPriceWithoutVat] [DECIMAL](18, 2) NOT NULL,
            [DiscountOnTotalPrice] [DECIMAL](18, 2) NOT NULL,
            [TotalDiscountOnIndividualProduct] [DECIMAL](18, 2) NOT NULL,
            [TotalVAT] [DECIMAL](18, 2) NOT NULL,
            [TotalSupplimentaryDuty] [DECIMAL](18, 2) NOT NULL,
            [IsVatDeductedInSource] [BIT] NOT NULL,
            [VDSAmount] [DECIMAL](18, 2) NULL,
            [IsVDSCertificateReceived] [BIT] NULL,
            [VDSCertificateNo] [NVARCHAR](50) NULL,
            [VDSCertificateIssueTime] [DATETIME] NULL,
            [VDSPaymentBankBranchId] [INT] NULL,
            [VDSPaymentDate] [DATETIME] NULL,
            [VDSPaymentChallanNo] [NVARCHAR](20) NULL,
            [VDSPaymentEconomicCode] [NVARCHAR](20) NULL,
            [VDSPaymentBookTransferNo] [NVARCHAR](50) NULL,
            [VDSNote] [NVARCHAR](500) NULL,
            [PaymentReceiveAmount] [DECIMAL](18, 2) NULL,
            [CustomerId] [INT] NULL,
            [ReceiverName] [NVARCHAR](200) NULL,
            [ReceiverContactNo] [VARCHAR](20) NULL,
            [ShippingAddress] [NVARCHAR](200) NULL,
            [ShippingCountryId] [INT] NULL,
            [SalesTypeId] [INT] NOT NULL,
            [SalesDeliveryTypeId] [INT] NOT NULL,
            [WorkOrderNo] [NVARCHAR](50) NULL,
            [SalesDate] [DATETIME] NOT NULL,
            [ExpectedDeliveryDate] [DATETIME] NULL,
            [DeliveryDate] [DATETIME] NULL,
            [DeliveryMethodId] [INT] NULL,
            [ExportTypeId] [INT] NULL,
            [LcNo] [NVARCHAR](50) NULL,
            [LcDate] [DATETIME] NULL,
            [BillOfEntry] [NVARCHAR](50) NULL,
            [BillOfEntryDate] [DATETIME] NULL,
            [DueDate] [DATETIME] NULL,
            [TermsOfLc] [NVARCHAR](500) NULL,
            [CustomerPoNumber] [NVARCHAR](50) NULL,
            [IsComplete] [BIT] NOT NULL,
            [IsTaxInvoicePrined] [BIT] NOT NULL,
            [TaxInvoicePrintedTime] [DATETIME] NULL,
            [OtherBranchOrganizationId] [INT] NULL,
            [TransferChallanNo] [NVARCHAR](50) NULL,
            [TransferChallanPrintedTime] [DATETIME] NULL,
            [ReferenceKey] [NVARCHAR](100) NULL,
            [CreatedBy] [INT] NULL,
            [CreatedTime] [DATETIME] NULL
        );

        DECLARE @SalesReference TABLE
        (
            [SalesId] [INT] NOT NULL,
            [ReferenceKey] [NVARCHAR](100) NULL,
            [CreatedBy] [INT] NULL,
            [CreatedTime] [DATETIME] NULL
        );

        --Variables for cursor
        DECLARE @SlsRefSalesId INT,
                @SlsRefReferenceKey NVARCHAR(100),
                @SlsRefCreatedBy INT,
                @SlsRefCreatedTime DATETIME;
        --/Variables for cursor

        INSERT INTO @Sales
        (
            InvoiceNo,
            VatChallanNo,
            OrganizationId,
            NoOfIteams,
            TotalPriceWithoutVat,
            DiscountOnTotalPrice,
            TotalDiscountOnIndividualProduct,
            TotalVAT,
            TotalSupplimentaryDuty,
            IsVatDeductedInSource,
            VDSAmount,
            IsVDSCertificateReceived,
            VDSCertificateNo,
            VDSCertificateIssueTime,
            VDSPaymentBankBranchId,
            VDSPaymentDate,
            VDSPaymentChallanNo,
            VDSPaymentEconomicCode,
            VDSPaymentBookTransferNo,
            VDSNote,
            PaymentReceiveAmount,
            CustomerId,
            ReceiverName,
            ReceiverContactNo,
            ShippingAddress,
            ShippingCountryId,
            SalesTypeId,
            SalesDeliveryTypeId,
            WorkOrderNo,
            SalesDate,
            ExpectedDeliveryDate,
            DeliveryDate,
            DeliveryMethodId,
            ExportTypeId,
            LcNo,
            LcDate,
            BillOfEntry,
            BillOfEntryDate,
            DueDate,
            TermsOfLc,
            CustomerPoNumber,
            IsComplete,
            IsTaxInvoicePrined,
            TaxInvoicePrintedTime,
            OtherBranchOrganizationId,
            TransferChallanNo,
            TransferChallanPrintedTime,
            ReferenceKey,
            CreatedBy,
            CreatedTime
        )
        SELECT jdSls.InvoiceNo,
               jdSls.VatChallanNo,
               @OrganizationId,
               ISNULL(sd.NoOfIteams, 0),
               ISNULL(sd.TotalPriceWithoutVat, 0),
               jdSls.DiscountOnTotalPrice,
               ISNULL(sd.TotalDiscountOnIndividualProduct, 0),
               ISNULL(sd.TotalVAT, 0),
               ISNULL(sd.TotalSupplimentaryDuty, 0),
               jdSls.IsVatDeductedInSource,
               jdSls.VDSAmount,
               jdSls.IsVDSCertificateReceived,
               jdSls.VDSCertificateNo,
               jdSls.VDSCertificateIssueTime,
               osdVDSPaymentBankBranch.DataKey,
               jdSls.VDSPaymentDate,
               jdSls.VDSPaymentChallanNo,
               jdSls.VDSPaymentEconomicCode,
               jdSls.VDSPaymentBookTransferNo,
               jdSls.VDSNote,
               ISNULL(pr.PaymentReceiveAmount, 0),
               cust.CustomerId,
               jdSls.ReceiverName,
               jdSls.ReceiverContactNo,
               jdSls.ShippingAddress,
               osdShippingCountry.DataKey,
               osdSalesType.DataKey,
               osdSalesDeliveryType.DataKey,
               jdSls.WorkOrderNo,
               jdSls.SalesDate,
               jdSls.ExpectedDeliveryDate,
               jdSls.DeliveryDate,
               osdDeliveryMethod.DataKey,
               osdExportType.DataKey,
               jdSls.LcNo,
               jdSls.LcDate,
               jdSls.BillOfEntry,
               jdSls.BillOfEntryDate,
               jdSls.DueDate,
               jdSls.TermsOfLc,
               jdSls.CustomerPoNumber,
               jdSls.IsComplete,
               jdSls.IsTaxInvoicePrined,
               jdSls.TaxInvoicePrintedTime,
               jdSls.OtherBranchOrganizationId,
               jdSls.TransferChallanNo,
               jdSls.TransferChallanPrintedTime,
               jdSls.SalesId,
               jdSls.CreatedBy,
               jdSls.CreatedTime
        FROM
            OPENJSON(@SalesJson)
            WITH
            (
                [SalesId] [NVARCHAR](100),
                [InvoiceNo] [NVARCHAR](50),
                [VatChallanNo] [NVARCHAR](50),
                [DiscountOnTotalPrice] [DECIMAL](18, 2),
                [IsVatDeductedInSource] [BIT],
                [VDSAmount] [DECIMAL](18, 2),
                [IsVDSCertificateReceived] [BIT],
                [VDSCertificateNo] [NVARCHAR](50),
                [VDSCertificateIssueTime] [DATETIME],
                [VDSPaymentBankBranchId] NVARCHAR(100),
                [VDSPaymentDate] [DATETIME],
                [VDSPaymentChallanNo] [NVARCHAR](20),
                [VDSPaymentEconomicCode] [NVARCHAR](20),
                [VDSPaymentBookTransferNo] [NVARCHAR](50),
                [VDSNote] [NVARCHAR](500),
                [CustomerId] NVARCHAR(100),
                [ReceiverName] [NVARCHAR](200),
                [ReceiverContactNo] [VARCHAR](20),
                [ShippingAddress] [NVARCHAR](200),
                [ShippingCountryId] NVARCHAR(100),
                [SalesTypeId] NVARCHAR(100),
                [SalesDeliveryTypeId] NVARCHAR(100),
                [WorkOrderNo] [NVARCHAR](50),
                [SalesDate] [DATETIME],
                [ExpectedDeliveryDate] [DATETIME],
                [DeliveryDate] [DATETIME],
                [DeliveryMethodId] NVARCHAR(100),
                [ExportTypeId] NVARCHAR(100),
                [LcNo] [NVARCHAR](50),
                [LcDate] [DATETIME],
                [BillOfEntry] [NVARCHAR](50),
                [BillOfEntryDate] [DATETIME],
                [DueDate] [DATETIME],
                [TermsOfLc] [NVARCHAR](500),
                [CustomerPoNumber] [NVARCHAR](50),
                [IsComplete] [BIT],
                [IsTaxInvoicePrined] [BIT],
                [TaxInvoicePrintedTime] [DATETIME],
                [OtherBranchOrganizationId] [INT],
                [TransferChallanNo] [NVARCHAR](50),
                [TransferChallanPrintedTime] [DATETIME],
                [CreatedBy] [INT],
                [CreatedTime] [DATETIME]
            ) jdSls
            LEFT JOIN dbo.OrgStaticData osdVDSPaymentBankBranch
                ON osdVDSPaymentBankBranch.ReferenceKey = jdSls.VDSPaymentBankBranchId
            LEFT JOIN dbo.OrgStaticData osdShippingCountry
                ON osdShippingCountry.ReferenceKey = jdSls.ShippingCountryId
            LEFT JOIN dbo.OrgStaticData osdSalesType
                ON osdSalesType.ReferenceKey = jdSls.SalesTypeId
            LEFT JOIN dbo.OrgStaticData osdSalesDeliveryType
                ON osdSalesDeliveryType.ReferenceKey = jdSls.SalesDeliveryTypeId
            LEFT JOIN dbo.OrgStaticData osdDeliveryMethod
                ON osdDeliveryMethod.ReferenceKey = jdSls.DeliveryMethodId
            LEFT JOIN dbo.OrgStaticData osdExportType
                ON osdExportType.ReferenceKey = jdSls.ExportTypeId
            LEFT JOIN dbo.Customer cust
                ON cust.ReferenceKey = jdSls.CustomerId
            LEFT JOIN
            (
                SELECT sdr.SalesReferenceKey,
                       SUM(sdr.Quantity * sdr.UnitPrice) AS TotalPriceWithoutVat,
                       SUM(sdr.Quantity * sdr.DiscountPerItem) AS TotalDiscountOnIndividualProduct,
                       SUM(dbo.FnGetCalculatedOutputVat(
                                                           sdr.Quantity * sdr.UnitPrice,
                                                           sdr.VATPercent,
                                                           sdr.SupplementaryDutyPercent
                                                       )
                          ) AS TotalVAT,
                       SUM(dbo.FnGetCalculatedOutputSuppDuty(sdr.Quantity * sdr.UnitPrice, sdr.SupplementaryDutyPercent)) AS TotalSupplimentaryDuty,
                       COUNT(1) AS NoOfIteams
                FROM @SlsDtlRaw sdr
                GROUP BY sdr.SalesReferenceKey
            ) sd
                ON sd.SalesReferenceKey = jdSls.SalesId
            LEFT JOIN
            (
                SELECT SUM(prr.ReceiveAmount) AS PaymentReceiveAmount,
                       prr.SalesReferenceKey
                FROM @PaymentReceive prr
                GROUP BY prr.SalesReferenceKey
            ) pr
                ON pr.SalesReferenceKey = jdSls.SalesId
        WHERE (
                  (
                      osdVDSPaymentBankBranch.OrgStaticDataTypeId = @osdtBankBranch
                      AND osdVDSPaymentBankBranch.OrganizationId = @OrganizationId
                      AND osdVDSPaymentBankBranch.IsActive = 1
                  )
                  OR osdVDSPaymentBankBranch.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdShippingCountry.OrgStaticDataTypeId = @osdtCountry
                      AND osdShippingCountry.OrganizationId = @OrganizationId
                      AND osdShippingCountry.IsActive = 1
                  )
                  OR osdShippingCountry.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdSalesType.OrgStaticDataTypeId = @osdtSalesType
                      AND osdSalesType.OrganizationId = @OrganizationId
                      AND osdSalesType.IsActive = 1
                  )
                  OR osdSalesType.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdSalesDeliveryType.OrgStaticDataTypeId = @osdtSalesDeliveryType
                      AND osdSalesDeliveryType.OrganizationId = @OrganizationId
                      AND osdSalesDeliveryType.IsActive = 1
                  )
                  OR osdSalesDeliveryType.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdDeliveryMethod.OrgStaticDataTypeId = @osdtDeliveryMethod
                      AND osdDeliveryMethod.OrganizationId = @OrganizationId
                      AND osdDeliveryMethod.IsActive = 1
                  )
                  OR osdDeliveryMethod.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdExportType.OrgStaticDataTypeId = @osdtExportType
                      AND osdExportType.OrganizationId = @OrganizationId
                      AND osdExportType.IsActive = 1
                  )
                  OR osdExportType.OrgStaticDataId IS NULL
              );

        INSERT INTO dbo.ApiTransaction
        (
            ApiTransactionTypeId,
            SecurityToken,
            NumberOfItem,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (   @ApiTransactionTypeId, -- ApiTransactionTypeId - int
            @SecurityToken,        -- SecurityToken - nvarchar(100)
            (
                SELECT COUNT(1) FROM @Sales sls
            ),                     -- NumberOfItem - int
            @CreatedBy,            -- CreatedBy - int
            @CreatedTime           -- CreatedTime - datetime
            );

        SET @ApiTransactionId = SCOPE_IDENTITY();

        INSERT INTO dbo.Sales
        (
            InvoiceNo,
            VatChallanNo,
            OrganizationId,
            NoOfIteams,
            TotalPriceWithoutVat,
            DiscountOnTotalPrice,
            TotalDiscountOnIndividualProduct,
            TotalVAT,
            TotalSupplimentaryDuty,
            IsVatDeductedInSource,
            VDSAmount,
            IsVDSCertificateReceived,
            VDSCertificateNo,
            VDSCertificateIssueTime,
            VDSPaymentBankBranchId,
            VDSPaymentDate,
            VDSPaymentChallanNo,
            VDSPaymentEconomicCode,
            VDSPaymentBookTransferNo,
            VDSNote,
            PaymentReceiveAmount,
            CustomerId,
            ReceiverName,
            ReceiverContactNo,
            ShippingAddress,
            ShippingCountryId,
            SalesTypeId,
            SalesDeliveryTypeId,
            WorkOrderNo,
            SalesDate,
            ExpectedDeliveryDate,
            DeliveryDate,
            DeliveryMethodId,
            ExportTypeId,
            LcNo,
            LcDate,
            BillOfEntry,
            BillOfEntryDate,
            DueDate,
            TermsOfLc,
            CustomerPoNumber,
            IsComplete,
            IsTaxInvoicePrined,
            TaxInvoicePrintedTime,
            OtherBranchOrganizationId,
            TransferChallanNo,
            TransferChallanPrintedTime,
            ReferenceKey,
            CreatedBy,
            CreatedTime,
            ApiTransactionId
        )
        OUTPUT Inserted.SalesId,
               Inserted.ReferenceKey,
               Inserted.CreatedBy,
               Inserted.CreatedTime
        INTO @SalesReference
        (
            SalesId,
            ReferenceKey,
            CreatedBy,
            CreatedTime
        )
        SELECT sls.InvoiceNo,
               sls.VatChallanNo,
               sls.OrganizationId,
               sls.NoOfIteams,
               sls.TotalPriceWithoutVat,
               sls.DiscountOnTotalPrice,
               sls.TotalDiscountOnIndividualProduct,
               sls.TotalVAT,
               sls.TotalSupplimentaryDuty,
               sls.IsVatDeductedInSource,
               sls.VDSAmount,
               sls.IsVDSCertificateReceived,
               sls.VDSCertificateNo,
               sls.VDSCertificateIssueTime,
               sls.VDSPaymentBankBranchId,
               sls.VDSPaymentDate,
               sls.VDSPaymentChallanNo,
               sls.VDSPaymentEconomicCode,
               sls.VDSPaymentBookTransferNo,
               sls.VDSNote,
               sls.PaymentReceiveAmount,
               sls.CustomerId,
               sls.ReceiverName,
               sls.ReceiverContactNo,
               sls.ShippingAddress,
               sls.ShippingCountryId,
               sls.SalesTypeId,
               sls.SalesDeliveryTypeId,
               sls.WorkOrderNo,
               sls.SalesDate,
               sls.ExpectedDeliveryDate,
               sls.DeliveryDate,
               sls.DeliveryMethodId,
               sls.ExportTypeId,
               sls.LcNo,
               sls.LcDate,
               sls.BillOfEntry,
               sls.BillOfEntryDate,
               sls.DueDate,
               sls.TermsOfLc,
               sls.CustomerPoNumber,
               sls.IsComplete,
               sls.IsTaxInvoicePrined,
               sls.TaxInvoicePrintedTime,
               sls.OtherBranchOrganizationId,
               sls.TransferChallanNo,
               sls.TransferChallanPrintedTime,
               sls.ReferenceKey,
               sls.CreatedBy,
               sls.CreatedTime,
               @ApiTransactionId
        FROM @Sales sls;

        --Sales Detail Insert
        DECLARE SALES_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY FOR
        SELECT slsRef.SalesId,
               slsRef.ReferenceKey,
               slsRef.CreatedBy,
               slsRef.CreatedTime
        FROM @SalesReference slsRef;
        OPEN SALES_CURSOR;
        FETCH NEXT FROM SALES_CURSOR
        INTO @SlsRefSalesId,
             @SlsRefReferenceKey,
             @SlsRefCreatedBy,
             @SlsRefCreatedTime;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            INSERT INTO dbo.SalesDetails
            (
                SalesId,
                ProductId,
                ProductVATTypeId,
                ProductTransactionBookId,
                Quantity,
                UnitPrice,
                DiscountPerItem,
                VATPercent,
                SupplementaryDutyPercent,
                MeasurementUnitId,
                ReferenceKey,
                CreatedBy,
                CreatedTime
            )
            SELECT @SlsRefSalesId,                     -- SalesId - int
                   slsDtl.ProductId,                   -- ProductId - int
                   slsDtl.ProductVATTypeId,            -- ProductVATTypeId - int
                   transBook.ProductTransactionBookId, -- ProductTransactionBookId - bigint
                   slsDtl.Quantity,                    -- Quantity - decimal(18, 2)
                   slsDtl.UnitPrice,                   -- UnitPrice - decimal(18, 2)
                   slsDtl.DiscountPerItem,             -- DiscountPerItem - decimal(18, 2)
                   slsDtl.VATPercent,                  -- VATPercent - decimal(18, 2)
                   slsDtl.SupplementaryDutyPercent,    -- SupplementaryDutyPercent - decimal(18, 2)
                   slsDtl.MeasurementUnitId,           -- MeasurementUnitId - int
                   slsDtl.ReferenceKey,                -- ReferenceKey - nvarchar(100)
                   @SlsRefCreatedBy,                   -- CreatedBy - int
                   @SlsRefCreatedTime                  -- CreatedTime - datetime
            FROM @SlsDtlRaw slsDtl
                LEFT JOIN
                (
                    SELECT ptb.ProductTransactionBookId,
                           ptb.ProductId,
                           ComputedColumn.EndQty AS EndQty,
                           CASE
                               WHEN ComputedColumn.EndQty = 0 THEN
                                   0
                               ELSE
                                   ComputedColumn.EndPrice / ComputedColumn.EndQty
                           END AS EndUnitPrice
                    FROM dbo.ProductTransactionBook ptb
                        LEFT JOIN dbo.PurchaseDetails purchDtl
                            ON purchDtl.PurchaseDetailId = ptb.PurchaseDetailId
                        LEFT JOIN dbo.DebitNoteDetail dnd
                            ON dnd.DebitNoteDetailId = ptb.DebitNoteDetailId
                        LEFT JOIN dbo.PurchaseDetails purchDtlDnd
                            ON purchDtlDnd.PurchaseDetailId = dnd.PurchaseDetailId
                        LEFT JOIN dbo.BillOfMaterial bom
                            ON bom.BillOfMaterialId = ptb.UsedInProductionId
                        LEFT JOIN dbo.ProductionReceive pr
                            ON pr.ProductionReceiveId = ptb.ProductionReceiveId
                        LEFT JOIN dbo.PriceSetup ps
                            ON ps.PriceSetupId = pr.PriceSetupId
                        LEFT JOIN dbo.SalesDetails slsDtl
                            ON slsDtl.SalesDetailId = ptb.SalesDetailId
                        LEFT JOIN dbo.CreditNoteDetail cnd
                            ON cnd.CreditNoteDetailId = ptb.CreditNoteDetailId
                        LEFT JOIN dbo.SalesDetails slsDtlCnd
                            ON slsDtlCnd.SalesDetailId = cnd.SalesDetailId
                        LEFT JOIN dbo.Damage dmg
                            ON dmg.DamageId = ptb.DamageId
                        INNER JOIN
                        (
                            SELECT MAX(ptb.ProductTransactionBookId) AS ProductTransactionBookId
                            FROM dbo.ProductTransactionBook ptb
                            WHERE ptb.OrganizationId = @OrganizationId
                            GROUP BY ptb.ProductId
                        ) lastPtb
                            ON lastPtb.ProductTransactionBookId = ptb.ProductTransactionBookId
                        CROSS APPLY
                    (
                        SELECT (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                                - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0)
                                - ISNULL(slsDtl.Quantity, 0) + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
                               ) AS EndQty,
                               (ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
                                - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0)
                                - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
                                + ISNULL(pr.ReceiveQuantity * ps.SalesUnitPrice, 0)
                                - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0)
                                + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0)
                                - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0)
                               ) AS EndPrice
                    ) AS ComputedColumn
                ) transBook
                    ON transBook.ProductId = slsDtl.ProductId
            WHERE slsDtl.SalesReferenceKey = @SlsRefReferenceKey;

            --Insert sales details in ProductTransactionBook
            INSERT INTO dbo.ProductTransactionBook
            (
                OrganizationId,
                ProductId,
                InitUnitPrice,
                InitQty,
                PurchaseDetailId,
                DebitNoteDetailId,
                UsedInProductionId,
                ProductionReceiveId,
                SalesDetailId,
                CreditNoteDetailId,
                DamageId,
                TransactionTime
            )
            SELECT @OrganizationId,                   -- OrganizationId - int
                   slsDtl.ProductId,                  -- ProductId - int
                   ISNULL(transBook.EndUnitPrice, 0), -- InitUnitPrice - decimal(19, 4)
                   ISNULL(transBook.EndQty, 0),       -- InitQty - decimal(19, 4)
                   NULL,                              -- PurchaseDetailId - int
                   NULL,                              -- DebitNoteDetailId - int
                   NULL,                              -- UsedInProductionId - bigint
                   NULL,                              -- ProductionReceiveId - bigint
                   slsDtl.SalesDetailId,              -- SalesDetailId - int
                   NULL,                              -- CreditNoteDetailId - int
                   NULL,                              -- DamageId - int
                   @SlsRefCreatedTime                 -- TransactionTime - datetime
            FROM dbo.SalesDetails slsDtl
                INNER JOIN
                (
                    SELECT ptb.ProductId,
                           ComputedColumn.EndQty AS EndQty,
                           CASE
                               WHEN ComputedColumn.EndQty = 0 THEN
                                   0
                               ELSE
                                   ComputedColumn.EndPrice / ComputedColumn.EndQty
                           END AS EndUnitPrice
                    FROM dbo.ProductTransactionBook ptb
                        LEFT JOIN dbo.PurchaseDetails purchDtl
                            ON purchDtl.PurchaseDetailId = ptb.PurchaseDetailId
                        LEFT JOIN dbo.DebitNoteDetail dnd
                            ON dnd.DebitNoteDetailId = ptb.DebitNoteDetailId
                        LEFT JOIN dbo.PurchaseDetails purchDtlDnd
                            ON purchDtlDnd.PurchaseDetailId = dnd.PurchaseDetailId
                        LEFT JOIN dbo.BillOfMaterial bom
                            ON bom.BillOfMaterialId = ptb.UsedInProductionId
                        LEFT JOIN dbo.ProductionReceive pr
                            ON pr.ProductionReceiveId = ptb.ProductionReceiveId
                        LEFT JOIN dbo.PriceSetup ps
                            ON ps.PriceSetupId = pr.PriceSetupId
                        LEFT JOIN dbo.SalesDetails slsDtl
                            ON slsDtl.SalesDetailId = ptb.SalesDetailId
                        LEFT JOIN dbo.CreditNoteDetail cnd
                            ON cnd.CreditNoteDetailId = ptb.CreditNoteDetailId
                        LEFT JOIN dbo.SalesDetails slsDtlCnd
                            ON slsDtlCnd.SalesDetailId = cnd.SalesDetailId
                        LEFT JOIN dbo.Damage dmg
                            ON dmg.DamageId = ptb.DamageId
                        INNER JOIN
                        (
                            SELECT MAX(ptb.ProductTransactionBookId) AS ProductTransactionBookId,
                                   ptb.ProductId
                            FROM dbo.ProductTransactionBook ptb
                                INNER JOIN dbo.SalesDetails slsDtlToCompare
                                    ON ptb.ProductId = slsDtlToCompare.ProductId
                            WHERE ptb.OrganizationId = @OrganizationId
                                  AND slsDtlToCompare.SalesId = @SlsRefSalesId
                            GROUP BY ptb.ProductId
                        ) lastPtb
                            ON lastPtb.ProductTransactionBookId = ptb.ProductTransactionBookId
                        CROSS APPLY
                    (
                        SELECT (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                                - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0)
                                - ISNULL(slsDtl.Quantity, 0) + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
                               ) AS EndQty,
                               (ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
                                - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0)
                                - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
                                + ISNULL(pr.ReceiveQuantity * ps.SalesUnitPrice, 0)
                                - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0)
                                + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0)
                                - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0)
                               ) AS EndPrice
                    ) AS ComputedColumn
                ) transBook
                    ON transBook.ProductId = slsDtl.ProductId
            WHERE slsDtl.SalesId = @SlsRefSalesId;
            --Insert sales details in ProductTransactionBook


            FETCH NEXT FROM SALES_CURSOR
            INTO @SlsRefSalesId,
                 @SlsRefReferenceKey,
                 @SlsRefCreatedBy,
                 @SlsRefCreatedTime;
        END;
        CLOSE SALES_CURSOR;
        DEALLOCATE SALES_CURSOR;

        --/Sales Detail Insert

        --Insert Payment

        INSERT INTO dbo.SalesPaymentReceive
        (
            SalesId,
            ReceivedPaymentMethodId,
            ReceiveAmount,
            ReceiveDate,
            ReferenceKey,
            CreatedBy,
            CreatedTime,
            ApiTransactionId
        )
        SELECT slsRef.SalesId,             -- SalesId - int
               pr.ReceivedPaymentMethodId, -- ReceivedPaymentMethodId - int
               pr.ReceiveAmount,           -- ReceiveAmount - decimal(18, 2)
               pr.ReceiveDate,             -- ReceiveDate - datetime
               pr.SalesReferenceKey,       -- SalesReferenceKey - nvarchar(100)
               slsRef.CreatedBy,           -- CreatedBy - int
               slsRef.CreatedTime,         -- CreatedTime - datetime
               @ApiTransactionId           -- ApiTransactionId - bigint
        FROM @PaymentReceive pr
            INNER JOIN @SalesReference slsRef
                ON slsRef.ReferenceKey = pr.SalesReferenceKey;
        --Insert Payment

        --Insert content
        IF @ContentJson IS NOT NULL
           AND LEN(@ContentJson) > 0
        BEGIN
            INSERT INTO dbo.Content
            (
                DocumentTypeId,
                OrganizationId,
                FileUrl,
                MimeType,
                Node,
                ObjectId,
                ObjectPrimaryKey,
                IsActive,
                CreatedBy,
                CreatedTime
            )
            SELECT dtype.DocumentTypeId,
                   @OrganizationId,
                   jd.FileUrl,
                   jd.MimeType,
                   NULL,
                   24,
                   slsRef.SalesId,
                   1,
                   slsRef.CreatedBy,
                   slsRef.CreatedTime
            FROM
                OPENJSON(@ContentJson)
                WITH
                (
                    [SalesId] NVARCHAR(100),
                    [DocumentTypeId] NVARCHAR(100),
                    [FileUrl] [NVARCHAR](500),
                    [MimeType] [NVARCHAR](50)
                ) jd
                INNER JOIN @SalesReference slsRef
                    ON slsRef.ReferenceKey = jd.SalesId
                INNER JOIN dbo.DocumentType dtype
                    ON dtype.ReferenceKey = jd.DocumentTypeId;
        END;

        --/Statement
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        IF @ErrorMsg = N''
        BEGIN
            SELECT @ErrorMsg = ERROR_MESSAGE();
        END;
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH;
END;
