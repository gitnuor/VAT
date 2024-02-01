-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetListOfOrganizationIdWithChild]
(
    -- Add the parameters for the function here
    @OrganizationId INT
)
RETURNS @Organization TABLE
(
    OrganizationId INT NOT NULL
)
AS
BEGIN
    INSERT INTO @Organization
    (
        OrganizationId
    )
    SELECT org.OrganizationId
    FROM dbo.Organizations org
    WHERE org.OrganizationId = @OrganizationId OR org.ParentOrganizationId = @OrganizationId;
    RETURN;
END;
