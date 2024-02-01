PRINT 'Updating static data table [dbo].[Roles]'

-- Change this to 1 to delete missing records in the target
-- WARNING: Setting this to 1 can cause damage to your database
-- and cause failed deployment if there are any rows referencing
-- a record which has been deleted.

DECLARE @DeleteMissingRecords BIT
SET @DeleteMissingRecords = 0

DECLARE @tblTempTable TABLE 
(
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[OrganizationId] int NOT NULL,
	[RoleDefaultPageId] int NOT NULL,
	[IsActive] bit NULL,
	[CreatedBy] int NULL,
	[CreatedTime] datetime NULL
)
SET IDENTITY_INSERT [dbo].[Roles] ON

INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (1, N'NBR USER', 0, NULL, 1, NULL, NULL)
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (2, N'Company Admin', NULL, NULL, 1, NULL, NULL)
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (3, N'Super Admin', -1, NULL, 1, 2, CAST(N'2019-09-18T14:11:13.443' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (4, N'General User', -1, NULL, 1, 2, CAST(N'2019-09-18T14:29:57.747' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (5, N'Entry Office', 5, NULL, 1, 7, CAST(N'2019-11-07T15:16:26.597' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (6, N'Manager', 5, NULL, 1, 7, NULL)
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (7, N'Vat Personal', 5, NULL, 1, 7, CAST(N'2019-11-07T15:29:55.570' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (8, N'Entry Officer', 6, NULL, 1, 7, CAST(N'2019-09-18T15:01:36.713' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (9, N'Manager', 6, NULL, 1, 7, NULL)
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (10, N'Vat Personal', 6, NULL, 1, 7, CAST(N'2019-09-18T15:02:44.197' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (11, N'Entry Officer', 7, NULL, 1, 7, CAST(N'2019-09-18T15:01:36.713' AS DateTime))
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (12, N'Manager', 7, NULL, 1, 7, NULL)
INSERT @tblTempTable ([RoleId], [RoleName], [OrganizationId], [RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime]) VALUES (13, N'Vat Personal', 7, NULL, 1, 7, CAST(N'2019-09-18T15:02:44.197' AS DateTime))

-- 3: Insert any new items into the table from the table variable
INSERT INTO [dbo].[Roles] ([RoleId],[RoleName], [OrganizationId],[RoleDefaultPageId], [IsActive], [CreatedBy], [CreatedTime])
SELECT tmp.[RoleId],tmp.[RoleName], tmp.[OrganizationId],tmp.[RoleDefaultPageId], tmp.[IsActive], tmp.[CreatedBy], tmp.[CreatedTime]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[Roles] tbl ON tbl.[RoleId] = tmp.[RoleId]
WHERE tbl.[RoleId] IS NULL

SET IDENTITY_INSERT [dbo].[Roles] OFF
DBCC CHECKIDENT([Roles], RESEED, 14)

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[RoleName] = tmp.[RoleName],
LiveTable.[OrganizationId] = tmp.[OrganizationId],
LiveTable.[RoleDefaultPageId] = tmp.[RoleDefaultPageId],
LiveTable.[IsActive] = tmp.[IsActive],
LiveTable.[CreatedBy] = tmp.[CreatedBy],
LiveTable.[CreatedTime] = tmp.[CreatedTime]
FROM [dbo].[Roles] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[RoleId] = tmp.[RoleId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE LiveTable FROM [dbo].[Roles] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[RoleId] = tmp.[RoleId]
	WHERE tmp.[RoleId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[Roles]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO


