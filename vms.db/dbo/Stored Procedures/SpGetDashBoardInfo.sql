-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpGetDashBoardInfo]
    -- Add the parameters for the stored procedure here
    @OrganizationId INT

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    DECLARE @Dashboard TABLE
    (
        SalesAmount DECIMAL(18, 2) NOT NULL,
        PurchaseAmount DECIMAL(18, 2) NOT NULL,
        VdsAmount DECIMAL(18, 2) NOT NULL,
        MiscIncrementalAdjustAmount DECIMAL(18, 2) NOT NULL,
        MiscDecrementalAdjustAmount DECIMAL(18, 2) NOT NULL,
		TotalVAT DECIMAL(18,2) NOT NULL,
		Rebate DECIMAL(18,2) NOT NULL,
		NextSubmissionDate datetime NULL,
		PreviousDue DECIMAL(18,2) NOT NULL
    );

    -----Rivetable VAT Calculation----
	DECLARE @firstDayOfMonth DATETIME
           = CAST((cast(year(GETDATE()) as varchar(4)) + '-' + CAST( MONTH(getdate()) as varchar(2)) + '-' + '01') as datetime),
            @firstDayOfNextMonth DATETIME 
			= cast((cast( year(dateadd(month, 1,getdate())) as varchar(4)) + '-' + cast( month(dateadd(month, 1,getdate())) as varchar(2)) + '-' + '01') as datetime);                                         
	DECLARE
	@standardVatProdVatTypeId INT = 3,
    @mrpProdVatTypeId INT = 4,
	@fixedVatProdVatTypeId INT = 6,
	@retailOrWholesaleOrTradeProductVatTypeId INT = 8

	DECLARE @RebateAmount decimal(18,2)

	SELECT @RebateAmount
               = isnull(SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    ),0)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purchDtl.ProductVATTypeId in 
		  (@standardVatProdVatTypeId,
		   @mrpProdVatTypeId,
		   @fixedVatProdVatTypeId,
		   @retailOrWholesaleOrTradeProductVatTypeId)

    -- Insert statements for procedure here
    INSERT INTO @Dashboard
    (
        SalesAmount,
        PurchaseAmount,
        VdsAmount,
        MiscIncrementalAdjustAmount,
        MiscDecrementalAdjustAmount,
		TotalVAT,
		Rebate,
		NextSubmissionDate,
		PreviousDue
    )
    VALUES
    (   (select isnull(sum(s.TotalPriceWithoutVat),0)
         from Sales s
         where s.SalesTypeId <> 3
         and s.OrganizationId = @OrganizationId), -- SalesAmount - decimal(18, 2)       
         (select isnull(sum(p.TotalPriceWithoutVat),0)
         from Purchase p
         where p.PurchaseTypeId <> 3
         and p.OrganizationId = @OrganizationId), -- PurchaseAmount - decimal(18, 2)
        (select isnull(sum(isnull(p.VDSAmount,0)),0)
		from Purchase p
		where p.PurchaseTypeId <> 3
		and p.OrganizationId = @OrganizationId),   -- VdsAmount - decimal(18, 2)
        (select isnull(sum(ad.Amount),0)
        from Adjustment ad
        where ad.AdjustmentTypeId = 1
		and ad.OrganizationId = @OrganizationId),  -- MiscIncrementalAdjustAmount - decimal(18, 2)
        (select isnull(sum(ad.Amount),0)
        from Adjustment ad
        where ad.AdjustmentTypeId = 2
		and ad.OrganizationId = @OrganizationId),  -- MiscDecrementalAdjustAmount - decimal(18, 2)
        (select isnull(sum(s.TotalVAT),0)
		from Sales s
		where s.SalesTypeId <> 3
		and s.OrganizationId = @OrganizationId),   -- TotalVAT - decimal(18, 2)
		@RebateAmount,  --TotalRivet - decimal(18,2)
		dateadd(day,-1,@firstDayOfNextMonth), --NextSumissionDate - datetime
		0      --PreviousDue - decimal(18,2)
        );
		

    SELECT dshBrd.SalesAmount,
           dshBrd.PurchaseAmount,
           dshBrd.VdsAmount,
           dshBrd.MiscIncrementalAdjustAmount,
           dshBrd.MiscDecrementalAdjustAmount,
           dshBrd.TotalVAT,
		   dshBrd.Rebate,
		   dshBrd.NextSubmissionDate,
		   dshBrd.PreviousDue
    FROM @Dashboard dshBrd;
END;

--exec SpGetDashBoardInfo 2
