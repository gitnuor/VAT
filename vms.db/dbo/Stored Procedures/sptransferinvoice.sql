
CREATE PROCEDURE [dbo].[sptransferinvoice]
(
    @organizationId INT,
    @SaleId INT
)
AS
BEGIN
    DECLARE @parentOrgId INT;
    SELECT @parentOrgId = ISNULL(ParentOrganizationId,@organizationId)
    FROM dbo.Organizations
    WHERE OrganizationId = @organizationId;

    SELECT *
    FROM dbo.Organizations org
        INNER JOIN dbo.Sales sal
            ON sal.OrganizationId = org.OrganizationId
        INNER JOIN dbo.SalesDetails sald
            ON sald.SalesId = sal.SalesId
	WHERE 1=1
	AND sal.SalesId = @SaleId
	AND sal.SalesTypeId = 3
END;
