PRINT 'Updating static data table [dbo].[ObjectType]'

-- Change this to 1 to delete missing records in the target
-- WARNING: Setting this to 1 can cause damage to your database
-- and cause failed deployment if there are any rows referencing
-- a record which has been deleted.

DECLARE @DeleteMissingRecords BIT
SET @DeleteMissingRecords = 0

DECLARE @tblTempTable TABLE 
(
	[ObjectTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[IsActive] bit NOT NULL,
	[CreatedBy] int NULL,
	[CreatedTime] datetime NULL
)
SET IDENTITY_INSERT [dbo].[ObjectType] ON

INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (1, N'ProductVAT', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (2, N'User', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (3, N'Vendor', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (4, N'Customer', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (5, N'COAGroup', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (6, N'ExportType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (7, N'PurchaseReason', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (8, N'MeasurementUnit', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (9, N'Product', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (10, N'Organization', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (11, N'ProductGroup', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (12, N'AuditLog', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (13, N'AuditOperation', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (14, N'ObjectType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (15, N'PurchaseType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (16, N'Right', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (17, N'RoleRight', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (18, N'SalesType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (19, N'PurchaseDetail', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (20, N'TransectionType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (21, N'Purchase', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (22, N'SalesDetail', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (23, N'UserType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (24, N'Sale', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (25, N'Role', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (26, N'ProductVATType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (27, N'SalesDeliveryType', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (28, N'Mushak61', N'Mushak', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (29, N'Mushak62', N'Mushak', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (30, N'Mushak63', N'Mushak', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (31, N'Mushak91', N'Mushak', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (32, N'PriceSetup', N'table', 1, NULL, NULL)
INSERT @tblTempTable ([ObjectTypeId], [Name], [Type], [IsActive], [CreatedBy], [CreatedTime]) VALUES (33, N'ProductionReceive', N'table', 1, NULL, NULL)

-- 3: Insert any new items into the table from the table variable
INSERT INTO [dbo].[ObjectType] ([ObjectTypeId],[Name], [Type], [IsActive], [CreatedBy], [CreatedTime])
SELECT tmp.[ObjectTypeId],tmp.[Name], tmp.[Type], tmp.[IsActive], tmp.[CreatedBy], tmp.[CreatedTime]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[ObjectType] tbl ON tbl.[ObjectTypeId] = tmp.[ObjectTypeId]
WHERE tbl.[ObjectTypeId] IS NULL

SET IDENTITY_INSERT [dbo].[ObjectType] OFF
DBCC CHECKIDENT([ObjectType], RESEED, 34)

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Name] = tmp.[Name],
LiveTable.[Type] = tmp.[Type],
LiveTable.[IsActive] = tmp.[IsActive],
LiveTable.[CreatedBy] = tmp.[CreatedBy],
LiveTable.[CreatedTime] = tmp.[CreatedTime]
FROM [dbo].[ObjectType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ObjectTypeId] = tmp.[ObjectTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE LiveTable FROM [dbo].[ObjectType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ObjectTypeId] = tmp.[ObjectTypeId]
	WHERE tmp.[ObjectTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[Areas]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO


